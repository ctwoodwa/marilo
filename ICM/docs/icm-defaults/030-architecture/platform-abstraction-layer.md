# Platform Abstraction Layer

> **Role in ICM:** Layer 3 - architecture defaults for structure, data flow, integrations, security, observability, and platform boundaries.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: platform-abstraction-layer | docs/icm-defaults/030-architecture/platform-abstraction-layer.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Platform Team  
> Last updated: 2026-01-22

## Purpose

The **Platform Abstraction Layer** wraps all external dependencies to protect {{ PRODUCT_NAME }} services from:
- Technology vendor lock-in (Azure, OpenAI)
- Breaking API changes in external SDKs
- Cloud provider migration requirements
- Framework/library obsolescence

**Steenberg principle:** *"Wrap what you don't control. Your services should depend on YOUR interfaces, not external vendors."*

---

## Architecture Principle

```
┌─────────────────────────────────────────┐
│  {{ PRODUCT_NAME }} Domain Services      │
│  (Asset Service, Work Order, etc.)      │
└──────────────┬──────────────────────────┘
               │ depends on
               ▼
┌─────────────────────────────────────────┐
│  Platform Abstraction Layer             │
│  (YOUR interfaces, YOUR control)        │
└──────────────┬──────────────────────────┘
               │ implements using
               ▼
┌─────────────────────────────────────────┐
│  External Dependencies                  │
│  (Azure SDK, OpenAI, 3rd party libs)    │
└─────────────────────────────────────────┘
```

**Rule:** Domain services NEVER directly reference external SDK packages. They only depend on Platform Abstraction interfaces.

---

## Abstraction Components

### 1. Messaging Abstraction

**Purpose:** Wrap Event Hub and Service Bus so services use {{ PRODUCT_NAME }}'s messaging interfaces, not Azure-specific APIs.

#### Interface

```csharp
namespace {{ PRODUCT_NAME }}.Platform.Messaging;

// Domain services depend on this interface
public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : DomainEvent;
    
    Task PublishBatchAsync<TEvent>(IEnumerable<TEvent> events, CancellationToken ct = default)
        where TEvent : DomainEvent;
}

public interface IEventConsumer
{
    Task SubscribeAsync<TEvent>(
        Func<TEvent, EventContext, Task> handler,
        ConsumerOptions options,
        CancellationToken ct = default)
        where TEvent : DomainEvent;
}

public interface ICommandQueue
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand;
    
    Task<TCommand?> ReceiveAsync<TCommand>(CancellationToken ct = default)
        where TCommand : ICommand;
}

public record EventContext(
    string PartitionKey,
    long SequenceNumber,
    DateTimeOffset EnqueuedTime,
    Dictionary<string, string> Properties
);

public record ConsumerOptions(
    string ConsumerGroup,
    int MaxConcurrency,
    TimeSpan RetryDelay,
    int MaxRetries
);
```

#### Implementation (Azure Event Hub/Service Bus)

```csharp
namespace {{ PRODUCT_NAME }}.Platform.Messaging.Azure;

// Platform team implements this
internal class AzureEventHubPublisher : IEventPublisher
{
    private readonly EventHubProducerClient _client;
    private readonly ILogger<AzureEventHubPublisher> _logger;

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : DomainEvent
    {
        var eventData = new EventData(JsonSerializer.SerializeToUtf8Bytes(@event))
        {
            PartitionKey = @event.CorrelationId,
            MessageId = @event.EventId
        };
        
        await _client.SendAsync([eventData], ct);
        _logger.LogInformation("Published event {EventType} {EventId}", 
            @event.EventType, @event.EventId);
    }
}

// Alternative implementation (Kafka, RabbitMQ, etc.)
internal class KafkaEventPublisher : IEventPublisher
{
    // Can swap entire implementation without touching domain services
}
```

**Services use it like this:**

```csharp
public class AssetService
{
    private readonly IEventPublisher _events;  // Platform abstraction, not Azure SDK
    
    public async Task UpdateConditionAsync(string assetId, ConditionScore score)
    {
        // Business logic...
        
        await _events.PublishAsync(new AssetConditionChanged
        {
            AssetId = assetId,
            NewScore = score
        });
    }
}
```

**Benefits:**
- Swap Event Hub → Kafka: change ONE class, not 50 services
- Test services with in-memory fake: implement `IEventPublisher` without real infrastructure
- Gradual migration: run both implementations side-by-side

---

### 2. Identity Abstraction

**Purpose:** Wrap Entra ID authentication so services don't directly depend on Microsoft.Identity libraries.

#### Interface

```csharp
namespace {{ PRODUCT_NAME }}.Platform.Identity;

public interface IIdentityProvider
{
    Task<AuthenticationResult> AuthenticateAsync(AuthenticationRequest request, CancellationToken ct = default);
    Task<TokenValidationResult> ValidateTokenAsync(string token, CancellationToken ct = default);
    Task<IReadOnlyList<string>> GetUserRolesAsync(string userId, CancellationToken ct = default);
}

public record AuthenticationRequest(
    string Username,
    string Password,
    string? Scope = null
);

public record AuthenticationResult(
    bool Success,
    string? AccessToken,
    string? RefreshToken,
    DateTimeOffset? ExpiresAt,
    string? ErrorMessage
);

public record TokenValidationResult(
    bool IsValid,
    ClaimsPrincipal? Principal,
    string? UserId,
    string? ErrorReason
);
```

#### Implementation (Entra ID)

```csharp
namespace {{ PRODUCT_NAME }}.Platform.Identity.EntraId;

internal class EntraIdProvider : IIdentityProvider
{
    private readonly IConfidentialClientApplication _app;
    private readonly TokenValidationParameters _validationParams;

    public async Task<TokenValidationResult> ValidateTokenAsync(string token, CancellationToken ct)
    {
        var handler = new JwtSecurityTokenHandler();
        
        try
        {
            var principal = handler.ValidateToken(token, _validationParams, out _);
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            return new TokenValidationResult(true, principal, userId, null);
        }
        catch (Exception ex)
        {
            return new TokenValidationResult(false, null, null, ex.Message);
        }
    }
}
```

**Services use it:**

```csharp
public class AssetApiController
{
    private readonly IIdentityProvider _identity;
    
    public async Task<IActionResult> GetAsset(string assetId)
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var validation = await _identity.ValidateTokenAsync(token);
        
        if (!validation.IsValid)
            return Unauthorized();
        
        // Business logic...
    }
}
```

---

### 3. AI Abstraction

**Purpose:** Wrap OpenAI API so services don't depend on specific LLM provider.

#### Interface

```csharp
namespace {{ PRODUCT_NAME }}.Platform.AI;

public interface ILargeLanguageModel
{
    Task<CompletionResult> CompleteAsync(CompletionRequest request, CancellationToken ct = default);
    Task<EmbeddingResult> GenerateEmbeddingAsync(string text, CancellationToken ct = default);
    IAsyncEnumerable<CompletionChunk> StreamCompletionAsync(CompletionRequest request, CancellationToken ct = default);
}

public record CompletionRequest(
    string Prompt,
    List<Message> ConversationHistory,
    decimal Temperature = 0.7m,
    int MaxTokens = 1000,
    List<ToolDefinition>? Tools = null
);

public record CompletionResult(
    string Content,
    int TokensUsed,
    TimeSpan ResponseTime,
    List<ToolCall>? ToolCalls = null
);

public record ToolDefinition(
    string Name,
    string Description,
    JsonSchema Parameters
);

public record ToolCall(
    string ToolName,
    JsonDocument Arguments
);
```

#### Implementation (OpenAI)

```csharp
namespace {{ PRODUCT_NAME }}.Platform.AI.OpenAI;

internal class OpenAILanguageModel : ILargeLanguageModel
{
    private readonly OpenAIClient _client;
    private readonly ILogger<OpenAILanguageModel> _logger;
    
    public async Task<CompletionResult> CompleteAsync(CompletionRequest request, CancellationToken ct)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var response = await _client.GetChatCompletionsAsync(new ChatCompletionsOptions
        {
            Messages = { new ChatMessage(ChatRole.User, request.Prompt) },
            Temperature = (float)request.Temperature,
            MaxTokens = request.MaxTokens
        }, ct);
        
        stopwatch.Stop();
        
        return new CompletionResult(
            response.Value.Choices[0].Message.Content,
            response.Value.Usage.TotalTokens,
            stopwatch.Elapsed
        );
    }
}

// Alternative implementation (Claude, Llama, etc.)
internal class ClaudeLanguageModel : ILargeLanguageModel
{
    // Swap OpenAI → Claude by changing DI registration, no service changes
}
```

**MCP uses it:**

```csharp
public class MCPToolExecutor
{
    private readonly ILargeLanguageModel _llm;  // Not OpenAI SDK directly
    
    public async Task<string> ExecuteConversationAsync(string userMessage)
    {
        var result = await _llm.CompleteAsync(new CompletionRequest(
            Prompt: userMessage,
            ConversationHistory: _context.Messages,
            Tools: _availableTools
        ));
        
        return result.Content;
    }
}
```

---

### 4. Storage Abstraction

**Purpose:** Wrap database access so services don't depend on specific ADO.NET/EF/Dapper implementations.

#### Interface

```csharp
namespace {{ PRODUCT_NAME }}.Platform.Storage;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<IReadOnlyList<TEntity>> QueryAsync(ISpecification<TEntity> spec, CancellationToken ct = default);
    Task<string> AddAsync(TEntity entity, CancellationToken ct = default);
    Task UpdateAsync(TEntity entity, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
}

public interface ISpecification<T>
{
    // Query specification pattern - implementation-agnostic filtering
}

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}
```

#### Implementation (SQL Server)

```csharp
namespace {{ PRODUCT_NAME }}.Platform.Storage.SqlServer;

internal class SqlRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    
    public async Task<TEntity?> GetByIdAsync(string id, CancellationToken ct)
    {
        return await _context.Set<TEntity>().FindAsync([id], ct);
    }
}

// Alternative implementation (NoSQL, in-memory, etc.)
internal class CosmosRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    // Swap SQL → Cosmos by changing DI registration
}
```

---

### 5. Caching Abstraction

**Purpose:** Wrap Redis/in-memory cache.

#### Interface

```csharp
namespace {{ PRODUCT_NAME }}.Platform.Caching;

public interface IDistributedCache
{
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);
    Task SetAsync<T>(string key, T value, CacheOptions options, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);
    Task<bool> ExistsAsync(string key, CancellationToken ct = default);
}

public record CacheOptions(
    TimeSpan? AbsoluteExpiration = null,
    TimeSpan? SlidingExpiration = null
);
```

---

## Registration and Configuration

Platform abstractions are registered in a **platform startup module**:

```csharp
namespace {{ PRODUCT_NAME }}.Platform;

public static class PlatformServiceExtensions
{
    public static IServiceCollection AddCannPlatform(
        this IServiceCollection services,
        IConfiguration config)
    {
        // Messaging
        services.AddSingleton<IEventPublisher, AzureEventHubPublisher>();
        services.AddSingleton<IEventConsumer, AzureEventHubConsumer>();
        services.AddSingleton<ICommandQueue, AzureServiceBusQueue>();
        
        // Identity
        services.AddSingleton<IIdentityProvider, EntraIdProvider>();
        
        // AI
        services.AddSingleton<ILargeLanguageModel, OpenAILanguageModel>();
        
        // Storage
        services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
        services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
        
        // Caching
        services.AddSingleton<IDistributedCache, RedisCache>();
        
        return services;
    }
}
```

**Services reference platform abstractions:**

```csharp
// In Asset.Service project
builder.Services.AddCannPlatform(builder.Configuration);  // One line to get all platform capabilities
```

---

## Testing Strategy

### Unit Tests: Use Fakes

```csharp
public class InMemoryEventPublisher : IEventPublisher
{
    public List<DomainEvent> PublishedEvents { get; } = new();
    
    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken ct = default)
        where TEvent : DomainEvent
    {
        PublishedEvents.Add(@event);
        return Task.CompletedTask;
    }
}

// Test
[Fact]
public async Task UpdateCondition_PublishesEvent()
{
    var fakePublisher = new InMemoryEventPublisher();
    var service = new AssetService(fakePublisher);
    
    await service.UpdateConditionAsync("asset-1", new ConditionScore(3));
    
    Assert.Single(fakePublisher.PublishedEvents);
    Assert.IsType<AssetConditionChanged>(fakePublisher.PublishedEvents[0]);
}
```

### Integration Tests: Use Real Implementations

```csharp
[Fact]
public async Task EventPublisher_RoundTrip()
{
    var publisher = new AzureEventHubPublisher(/* real config */);
    var consumer = new AzureEventHubConsumer(/* real config */);
    
    var received = new TaskCompletionSource<AssetConditionChanged>();
    
    await consumer.SubscribeAsync<AssetConditionChanged>(
        (@event, ctx) => { received.SetResult(@event); return Task.CompletedTask; },
        new ConsumerOptions("test-group", 1, TimeSpan.FromSeconds(1), 3)
    );
    
    await publisher.PublishAsync(new AssetConditionChanged { AssetId = "test" });
    
    var result = await received.Task.WaitAsync(TimeSpan.FromSeconds(10));
    Assert.Equal("test", result.AssetId);
}
```

---

## Migration Strategy

### Phase 1: Build Abstractions (Current)
- Define interfaces in `{{ PRODUCT_NAME }}.Platform` project
- Implement Azure-based adapters
- Create fake implementations for testing

### Phase 2: Adopt in New Services
- All new services use platform abstractions from day one
- Reference implementations demonstrate patterns
- No direct Azure SDK references in service projects

### Phase 3: Migrate Existing Services
- Replace direct Azure SDK calls with platform abstractions
- One service at a time
- Validate with integration tests

### Phase 4: Prove Replaceability
- Implement alternative adapters (Kafka, Postgres, Claude)
- Run parallel implementations temporarily
- Validate swap with no service changes

---

## Governance Rules (Architecture Tests)

Enforce these with ArchUnit or similar:

```csharp
[Fact]
public void Services_MustNotReference_AzureSDK()
{
    var services = Types.InAssembly(typeof(AssetService).Assembly);
    var azurePackages = new[] 
    { 
        "Azure.Messaging.EventHubs",
        "Azure.Identity",
        "Microsoft.Azure.ServiceBus"
    };
    
    services.Should().NotDependOnAny(azurePackages);
}

[Fact]
public void Services_MustOnlyDependOn_PlatformAbstractions()
{
    var services = Types.InAssembly(typeof(AssetService).Assembly);
    
    services.Should().OnlyDependOn("{{ PRODUCT_NAME }}.Platform.*");
}
```

---

## Replaceability Test Plan

Quarterly exercise to prove abstraction effectiveness:

### Q1: Messaging Swap Exercise
- Implement `KafkaEventPublisher` and `KafkaEventConsumer`
- Swap DI registration from Azure Event Hub → Kafka
- Run full integration test suite
- Success criteria: Zero service code changes required

### Q2: AI Provider Swap Exercise
- Implement `ClaudeLanguageModel`
- Swap DI registration from OpenAI → Claude
- Run MCP integration tests
- Success criteria: Same tool execution results

### Q3: Storage Swap Exercise
- Implement `PostgresRepository<T>`
- Swap DI registration from SQL Server → Postgres
- Run full service test suite
- Success criteria: All tests pass without service changes

---

## Team Responsibilities

### Platform Team Owns:
- All abstraction interface definitions
- Azure/OpenAI/external SDK implementations
- Fake implementations for testing
- Platform package versioning and releases
- Breaking change management (RFC process)

### Domain Teams Use:
- Platform abstractions only (never direct SDK references)
- Report issues/gaps to Platform team
- Contribute to fake implementations
- Participate in replaceability tests

---

## Related Documentation

- [domain-primitives.md](domain-primitives.md) - what flows through these abstractions
- [logical-architecture.md](logical-architecture.md) - how services use abstractions
- [deployment-architecture.md](deployment-architecture.md) - DI registration and configuration
- [interface-governance.md](interface-governance.md) - versioning and contract management
