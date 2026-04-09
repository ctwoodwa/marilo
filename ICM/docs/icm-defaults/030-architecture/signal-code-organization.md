# Signal-Based Code Organization and Structure

> **Role in ICM:** Layer 3 - architecture defaults for structure, data flow, integrations, security, observability, and platform boundaries.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: signal-code-organization | docs/icm-defaults/030-architecture/signal-code-organization.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> **Status**: Draft  
> **Owner**: Architecture Team  
> **Last updated**: 2026-01-25

---

## Purpose

This document defines the **code organization patterns** for signal-based reactive architecture in {{ PRODUCT_NAME }}. It maps Component-based Scalable Logical Architecture-style business logic patterns to {{ PRODUCT_NAME }}'s microservice structure and provides clear guidance on where to place signal-related code.

---

## 1. Project Structure Overview

### 1.1 Service Template Structure

Each {{ PRODUCT_NAME }} service follows this layered structure:

```
src/
├── ServiceName.Domain/
│   ├── Aggregates/           # Domain aggregates (Component-based Scalable Logical Architecture Editable Root equivalent)
│   ├── Entities/             # Domain entities
│   ├── ValueObjects/         # Value objects
│   ├── Events/               # Domain events raised by aggregates
│   ├── Signals/              # Signal definitions for this service
│   ├── Policies/             # Business rule policies (signal handlers)
│   └── Services/             # Domain services
│
├── ServiceName.Application/
│   ├── Commands/             # Command definitions (Component-based Scalable Logical Architecture Command Object equivalent)
│   ├── Queries/              # Query definitions
│   ├── Handlers/             # Command/query handlers
│   │   ├── CommandHandlers/
│   │   ├── QueryHandlers/
│   │   └── SignalHandlers/   # Cross-domain signal handlers
│   └── Validators/           # FluentValidation validators
│
├── ServiceName.Infrastructure.Write/
│   ├── Persistence/          # EF Core DbContext, repositories
│   ├── Configuration/        # Entity configurations
│   └── Migrations/           # Database migrations
│
├── ServiceName.Infrastructure.Read/  (CQRS services only)
│   ├── ReadModels/           # Denormalized read models (Component-based Scalable Logical Architecture Read-Only equivalent)
│   ├── Projections/          # Event/signal handlers that update read models
│   └── Queries/              # Read model query handlers
│
└── ServiceName.API.Write/
    ├── Controllers/          # HTTP endpoints
    ├── Authorization/        # Authorization policies
    ├── Middleware/           # Custom middleware
    └── Program.cs            # Service configuration
```

---

## 2. Signal Code Placement

### 2.1 Signal Type Definitions

**Location:** `{ServiceName}.Domain/Signals/`

**Purpose:** Define signal types that represent domain-specific conditions.

**Example:**
```
AssetManagement.Domain/
└── Signals/
    ├── AssetConditionThresholdSignal.cs
    ├── DeficiencyEscalationSignal.cs
    └── AssetLifecycleSignal.cs
```

**File Content:**
```csharp
// AssetManagement.Domain/Signals/AssetConditionThresholdSignal.cs
namespace AssetManagement.Domain.Signals;

/// <summary>
/// Signal: Asset condition index breached threshold
/// Raised by: Asset aggregate or condition assessment service
/// Handled by: WorkPlanning service (creates work order)
/// </summary>
public record AssetConditionThresholdSignal : Signal
{
    public required string AssetId { get; init; }
    public required decimal CurrentCI { get; init; }
    public required decimal ThresholdCI { get; init; }
    public required ConditionTrend Trend { get; init; }
    public DateOnly ProjectedFailureDate { get; init; }
    public List<string> AffectedSystemIds { get; init; } = new();
}
```

**Naming Convention:**
- Signal names end with `Signal`
- Use descriptive names indicating the condition: `{Condition}{Signal}`
- Examples: `AssetConditionThresholdSignal`, `PredictiveMaintenanceSignal`

---

### 2.2 Signal Detection (Event Handlers)

**Location:** `{ServiceName}.Application/Handlers/SignalHandlers/`

**Purpose:** Detect signals from domain events and raise them on the signal bus.

**Example:**
```
AssetManagement.Application/
└── Handlers/
    └── SignalHandlers/
        ├── AssetConditionChangedSignalDetector.cs
        └── DeficiencyAddedSignalDetector.cs
```

**File Content:**
```csharp
// AssetManagement.Application/Handlers/SignalHandlers/AssetConditionChangedSignalDetector.cs
namespace AssetManagement.Application.Handlers.SignalHandlers;

/// <summary>
/// Listens to AssetConditionChangedEvent and raises threshold signals
/// </summary>
public class AssetConditionChangedSignalDetector : 
    IEventHandler<AssetConditionChangedEvent>
{
    private readonly ISignalBus _signalBus;
    private readonly IBusinessRuleEngine _ruleEngine;
    private readonly ILogger<AssetConditionChangedSignalDetector> _logger;
    
    public async Task Handle(
        AssetConditionChangedEvent @event,
        EventContext context)
    {
        // Evaluate threshold business rules
        var thresholdRules = await _ruleEngine.EvaluateAsync(
            "AssetConditionThreshold",
            new { AssetId = @event.AssetId, CurrentCI = @event.NewCI });
        
        if (thresholdRules.ThresholdBreached)
        {
            await _signalBus.RaiseAsync(new AssetConditionThresholdSignal
            {
                SignalType = "AssetConditionThreshold",
                SourceId = @event.AssetId,
                CorrelationId = @event.CorrelationId,
                Priority = DeterminePriority(thresholdRules),
                AssetId = @event.AssetId,
                CurrentCI = @event.NewCI,
                ThresholdCI = thresholdRules.ThresholdValue,
                Trend = CalculateTrend(@event.PreviousScore, @event.NewScore)
            });
            
            _logger.LogInformation(
                "Raised AssetConditionThresholdSignal for asset {AssetId} " +
                "(CI: {CurrentCI}, Threshold: {ThresholdCI})",
                @event.AssetId, @event.NewCI, thresholdRules.ThresholdValue);
        }
    }
}
```

**Naming Convention:**
- Class names end with `SignalDetector`
- Format: `{EventName}SignalDetector`

---

### 2.3 Business Rule Policies (Signal Handlers)

**Location:** `{ServiceName}.Domain/Policies/`

**Purpose:** Domain-level business rules that react to signals within the same service.

**Example:**
```
AssetManagement.Domain/
└── Policies/
    ├── AssetConditionThresholdPolicy.cs
    └── DeficiencyEscalationPolicy.cs
```

**File Content:**
```csharp
// AssetManagement.Domain/Policies/AssetConditionThresholdPolicy.cs
namespace AssetManagement.Domain.Policies;

/// <summary>
/// Business policy: Asset condition threshold enforcement
/// Version: 2
/// Approved by: Chief Engineer, Jane Doe
/// Compliance: ISO 55000 (Asset Management)
/// </summary>
public class AssetConditionThresholdPolicy : 
    VersionedBusinessPolicy<AssetConditionThresholdSignal>
{
    private readonly IWorkOrderService _workOrderService;
    private readonly INotificationService _notificationService;
    
    public override string PolicyId => "AssetConditionThreshold";
    public override int Version => 2;
    public override DateTime EffectiveDate => new DateTime(2026, 2, 1);
    public override DateTime? ExpirationDate => null;
    public override string ApprovedBy => "Chief Engineer, Jane Doe";
    public override string ComplianceFramework => "ISO 55000";
    
    public override async Task<PolicyEvaluationResult> EvaluateAsync(
        AssetConditionThresholdSignal signal,
        CancellationToken ct)
    {
        var result = new PolicyEvaluationResult();
        
        // Rule 1: Critical CI requires immediate work order
        if (signal.CurrentCI < 0.3m)
        {
            result.Actions.Add(new PolicyAction
            {
                ActionType = "CreateWorkOrder",
                Priority = WorkOrderPriority.Critical,
                Reason = $"Asset CI {signal.CurrentCI:F2} below critical threshold 0.30"
            });
        }
        // Rule 2: Warning CI requires notification
        else if (signal.CurrentCI < 0.4m)
        {
            result.Actions.Add(new PolicyAction
            {
                ActionType = "SendNotification",
                Priority = NotificationPriority.High,
                Reason = $"Asset CI {signal.CurrentCI:F2} below warning threshold 0.40"
            });
        }
        
        return result;
    }
}
```

**Naming Convention:**
- Class names end with `Policy`
- Format: `{SignalName}Policy`
- Inherit from `VersionedBusinessPolicy<TSignal>` for compliance

---

### 2.4 Cross-Service Signal Handlers

**Location:** `{ServiceName}.Application/Handlers/SignalHandlers/`

**Purpose:** Handle signals raised by other services (via Event Hub bridging).

**Example:**
```
WorkPlanning.Application/
└── Handlers/
    └── SignalHandlers/
        ├── AssetConditionThresholdSignalHandler.cs
        ├── PredictiveMaintenanceSignalHandler.cs
        └── DeficiencyEscalationSignalHandler.cs
```

**File Content:**
```csharp
// WorkPlanning.Application/Handlers/SignalHandlers/AssetConditionThresholdSignalHandler.cs
namespace WorkPlanning.Application.Handlers.SignalHandlers;

/// <summary>
/// Handles AssetConditionThresholdSignal from AssetManagement service
/// Creates work orders based on policy evaluation
/// </summary>
public class AssetConditionThresholdSignalHandler : 
    ISignalHandler<AssetConditionThresholdSignal>
{
    private readonly ICommandQueue _commandQueue;
    private readonly IAssetRepository _assetRepo;
    private readonly ILogger<AssetConditionThresholdSignalHandler> _logger;
    
    public async Task HandleAsync(
        AssetConditionThresholdSignal signal,
        CancellationToken ct)
    {
        // Load asset details
        var asset = await _assetRepo.GetByIdAsync(signal.AssetId);
        
        // Generate work order command
        await _commandQueue.SendAsync(new CreateWorkOrderCommand
        {
            AssetId = signal.AssetId,
            WorkOrderType = WorkOrderType.Corrective,
            Priority = MapPriority(signal.CurrentCI),
            EstimatedCost = await EstimateRepairCostAsync(asset),
            Reason = $"Asset CI ({signal.CurrentCI:F2}) below threshold ({signal.ThresholdCI:F2})",
            TriggeredBySignalId = signal.SignalId,
            ScheduledDate = CalculateScheduledDate(signal.ProjectedFailureDate)
        });
        
        _logger.LogInformation(
            "Created work order for asset {AssetId} from signal {SignalId}",
            signal.AssetId, signal.SignalId);
    }
}
```

**Naming Convention:**
- Class names end with `SignalHandler`
- Format: `{SignalName}Handler`
- Implement `ISignalHandler<TSignal>`

---

### 2.5 Read Model Updaters (CQRS)

**Location:** `{ServiceName}.Infrastructure.Read/Projections/`

**Purpose:** Update read models in response to signals.

**Example:**
```
AssetManagement.Infrastructure.Read/
└── Projections/
    ├── AssetListReadModelProjection.cs  (event-based)
    └── AssetAlertReadModelProjection.cs (signal-based)
```

**File Content:**
```csharp
// AssetManagement.Infrastructure.Read/Projections/AssetAlertReadModelProjection.cs
namespace AssetManagement.Infrastructure.Read.Projections;

/// <summary>
/// Updates AssetAlertReadModel when threshold signals raised
/// Enables real-time dashboard updates
/// </summary>
public class AssetAlertReadModelProjection : 
    ISignalHandler<AssetConditionThresholdSignal>
{
    private readonly IReadModelRepository<AssetAlertReadModel> _readRepo;
    
    public async Task HandleAsync(
        AssetConditionThresholdSignal signal,
        CancellationToken ct)
    {
        await _readRepo.UpsertAsync(new AssetAlertReadModel
        {
            AssetId = signal.AssetId,
            AlertType = "ConditionThreshold",
            Severity = DetermineSeverity(signal.CurrentCI),
            Message = $"Asset CI {signal.CurrentCI:F2} below threshold {signal.ThresholdCI:F2}",
            RaisedAt = signal.Timestamp,
            SignalId = signal.SignalId,
            RequiresAction = signal.CurrentCI < 0.3m
        });
    }
}
```

**Naming Convention:**
- Class names end with `Projection`
- Format: `{ReadModelName}Projection`

---

## 3. Component-based Scalable Logical Architecture Pattern Mappings

### 3.1 Editable Root → Domain Aggregate

| Component-based Scalable Logical Architecture Pattern | {{ PRODUCT_NAME }} Location | Notes |
|--------------|-----------------|-------|
| `BusinessBase<T>` | `{Service}.Domain/Aggregates/` | Domain aggregate root |
| Property tracking | EF Core change tracking | Built-in with EF Core |
| Business rules | `{Service}.Domain/Policies/` | Signal-triggered policies |
| Data Portal | WolverineFx command handlers | Async command processing |

**Example Mapping:**

**Component-based Scalable Logical Architecture:**
```csharp
public class Customer : BusinessBase<Customer>
{
    public static async Task<Customer> GetAsync(int id)
    {
        return await DataPortal.FetchAsync<Customer>(id);
    }
}
```

**{{ PRODUCT_NAME }}:**
```csharp
// AssetManagement.Application/Commands/GetAssetCommand.cs
public record GetAssetCommand(string AssetId);

// AssetManagement.Application/Handlers/CommandHandlers/GetAssetCommandHandler.cs
public class GetAssetCommandHandler
{
    public async Task<FacilityAssetAggregate> Handle(GetAssetCommand command)
    {
        return await _assetRepo.GetByIdAsync(command.AssetId);
    }
}
```

---

### 3.2 Read-Only Object → CQRS Read Model

| Component-based Scalable Logical Architecture Pattern | {{ PRODUCT_NAME }} Location | Notes |
|--------------|-----------------|-------|
| `ReadOnlyBase<T>` | `{Service}.Infrastructure.Read/ReadModels/` | Denormalized read model |
| Data fetching | Event/signal projection | Updated asynchronously |
| Read-only properties | Immutable records | C# record types |

**Example Mapping:**

**Component-based Scalable Logical Architecture:**
```csharp
public class CustomerInfo : ReadOnlyBase<CustomerInfo>
{
    public string Name => GetProperty(NameProperty);
    
    [Fetch]
    private async Task Fetch(int id)
    {
        var data = await _dal.GetCustomerAsync(id);
        LoadProperty(NameProperty, data.Name);
    }
}
```

**{{ PRODUCT_NAME }}:**
```csharp
// AssetManagement.Infrastructure.Read/ReadModels/AssetListReadModel.cs
public record AssetListReadModel
{
    public string AssetId { get; init; }
    public string Name { get; init; }
    public decimal ConditionIndex { get; init; }
    public bool RequiresAttention { get; init; }  // Set by signal projection
}

// AssetManagement.Infrastructure.Read/Projections/AssetListReadModelProjection.cs
public class AssetListReadModelProjection : 
    IEventHandler<AssetConditionChangedEvent>,
    ISignalHandler<AssetConditionThresholdSignal>
{
    // Event handler updates basic properties
    // Signal handler updates reactive flags (RequiresAttention)
}
```

---

### 3.3 Command Object → WolverineFx Command

| Component-based Scalable Logical Architecture Pattern | {{ PRODUCT_NAME }} Location | Notes |
|--------------|-----------------|-------|
| `CommandBase<T>` | `{Service}.Application/Commands/` | Command definition |
| `[Execute]` | `{Service}.Application/Handlers/CommandHandlers/` | Handler implementation |
| Data Portal | WolverineFx routing | Convention-based routing |

**Example Mapping:**

**Component-based Scalable Logical Architecture:**
```csharp
public class DeleteCustomerCommand : CommandBase<DeleteCustomerCommand>
{
    public static async Task ExecuteAsync(int customerId)
    {
        var cmd = new DeleteCustomerCommand { CustomerId = customerId };
        await DataPortal.ExecuteAsync(cmd);
    }
    
    [Execute]
    private async Task Execute()
    {
        await _dal.DeleteCustomerAsync(CustomerId);
    }
}
```

**{{ PRODUCT_NAME }}:**
```csharp
// AssetManagement.Application/Commands/DeleteAssetCommand.cs
public record DeleteAssetCommand(string AssetId);

// AssetManagement.Application/Handlers/CommandHandlers/DeleteAssetCommandHandler.cs
public class DeleteAssetCommandHandler
{
    private readonly IAssetRepository _assetRepo;
    
    public async Task Handle(DeleteAssetCommand command, CancellationToken ct)
    {
        await _assetRepo.DeleteAsync(command.AssetId);
    }
}
```

---

### 3.4 Business Rules → Signal Policies

| Component-based Scalable Logical Architecture Pattern | {{ PRODUCT_NAME }} Location | Notes |
|--------------|-----------------|-------|
| `AddBusinessRules()` | `{Service}.Domain/Policies/` | Signal-triggered policies |
| `BusinessRule<T>` | `IBusinessPolicy<TSignal>` | Policy interface |
| `IsInRole()` | ASP.NET Core authorization | Claim-based authorization |

**Example Mapping:**

**Component-based Scalable Logical Architecture:**
```csharp
protected override void AddBusinessRules()
{
    BusinessRules.AddRule(new Required(NameProperty));
    BusinessRules.AddRule(new MaxLength(NameProperty, 100));
    BusinessRules.AddRule(new IsInRole(
        AuthorizationActions.WriteProperty, 
        SalaryProperty, 
        "HR"));
}
```

**{{ PRODUCT_NAME }}:**
```csharp
// AssetManagement.Domain/Policies/AssetConditionThresholdPolicy.cs
public class AssetConditionThresholdPolicy : 
    VersionedBusinessPolicy<AssetConditionThresholdSignal>
{
    public override async Task<PolicyEvaluationResult> EvaluateAsync(
        AssetConditionThresholdSignal signal,
        CancellationToken ct)
    {
        var result = new PolicyEvaluationResult();
        
        // Business rule: Critical CI requires work order
        if (signal.CurrentCI < 0.3m)
        {
            result.Actions.Add(new PolicyAction
            {
                ActionType = "CreateWorkOrder",
                Priority = WorkOrderPriority.Critical,
                Reason = "Asset CI below critical threshold"
            });
        }
        
        return result;
    }
}

// Authorization handled separately via ASP.NET Core
[Authorize(Policy = "AssetManager")]
public class UpdateAssetConditionController { }
```

---

## 4. Service Registration Patterns

### 4.1 Register Signal Bus

```csharp
// Location: {ServiceName}.API.Write/Program.cs
var builder = WebApplication.CreateBuilder(args);

// Register signal bus
builder.Services.AddSignalBus(options =>
{
    // Bridge critical signals to Event Hub for cross-service consumption
    options.BridgeToEventHub<AssetConditionThresholdSignal>();
    options.BridgeToEventHub<PredictiveMaintenanceSignal>();
    
    // Keep local (high-frequency, low-criticality signals)
    // options.BridgeToEventHub<WorkOrderPrioritySignal>();  // NOT bridged
});
```

### 4.2 Register Signal Handlers

```csharp
// Auto-register all signal handlers in assembly
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(Program))
    .AddClasses(classes => classes.AssignableTo(typeof(ISignalHandler<>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Or manually register specific handlers
builder.Services.AddScoped<ISignalHandler<AssetConditionThresholdSignal>, 
                          AssetConditionThresholdSignalHandler>();
```

### 4.3 Register Business Policies

```csharp
// Register policies per release channel
var releaseChannel = builder.Configuration.GetValue<ReleaseChannel>("ReleaseChannel");

builder.Services.AddBusinessPolicies(options =>
{
    if (releaseChannel == ReleaseChannel.Production)
    {
        options.RegisterPolicy<AssetConditionThresholdSignal>(
            new AssetConditionThresholdPolicy_V2());
    }
    else if (releaseChannel == ReleaseChannel.UAT)
    {
        // Test new policy version in UAT
        options.RegisterPolicy<AssetConditionThresholdSignal>(
            new AssetConditionThresholdPolicy_V3_Beta());
    }
});
```

---

## 5. File Naming Conventions Summary

| Component | Location | File Naming | Example |
|-----------|----------|-------------|---------|
| Signal Type | `{Service}.Domain/Signals/` | `{Condition}Signal.cs` | `AssetConditionThresholdSignal.cs` |
| Signal Detector | `{Service}.Application/Handlers/SignalHandlers/` | `{EventName}SignalDetector.cs` | `AssetConditionChangedSignalDetector.cs` |
| Business Policy | `{Service}.Domain/Policies/` | `{SignalName}Policy.cs` | `AssetConditionThresholdPolicy.cs` |
| Signal Handler | `{Service}.Application/Handlers/SignalHandlers/` | `{SignalName}Handler.cs` | `AssetConditionThresholdSignalHandler.cs` |
| Read Projection | `{Service}.Infrastructure.Read/Projections/` | `{ReadModelName}Projection.cs` | `AssetAlertReadModelProjection.cs` |
| Command | `{Service}.Application/Commands/` | `{Action}{Entity}Command.cs` | `CreateWorkOrderCommand.cs` |
| Command Handler | `{Service}.Application/Handlers/CommandHandlers/` | `{CommandName}Handler.cs` | `CreateWorkOrderCommandHandler.cs` |

---

## 6. Testing File Organization

### 6.1 Unit Tests

```
tests/
└── {ServiceName}.Domain.Tests/
    ├── Aggregates/
    │   └── FacilityAssetAggregateTests.cs
    ├── Policies/
    │   └── AssetConditionThresholdPolicyTests.cs
    └── Signals/
        └── AssetConditionThresholdSignalTests.cs
```

### 6.2 Integration Tests

```
tests/
└── {ServiceName}.Integration.Tests/
    ├── SignalFlowTests/
    │   ├── AssetConditionThresholdFlowTests.cs
    │   └── PredictiveMaintenanceFlowTests.cs
    └── Handlers/
        └── AssetConditionThresholdSignalHandlerTests.cs
```

---

## 7. Examples by Service

### 7.1 AssetManagement Service

```
src/AssetManagement.Domain/
├── Aggregates/
│   └── FacilityAssetAggregate.cs          # Domain aggregate with signal raising
├── Signals/
│   ├── AssetConditionThresholdSignal.cs   # Threshold breach signal
│   └── DeficiencyEscalationSignal.cs      # Deficiency count signal
└── Policies/
    └── AssetConditionThresholdPolicy.cs   # Business rule policy

src/AssetManagement.Application/
└── Handlers/
    └── SignalHandlers/
        └── AssetConditionChangedSignalDetector.cs  # Detects signals from events

src/AssetManagement.Infrastructure.Read/
└── Projections/
    └── AssetAlertReadModelProjection.cs   # Updates read model from signals
```

### 7.2 WorkPlanning Service

```
src/WorkPlanning.Application/
└── Handlers/
    └── SignalHandlers/
        ├── AssetConditionThresholdSignalHandler.cs  # Creates work orders
        └── PredictiveMaintenanceSignalHandler.cs    # Creates preventive work
```

### 7.3 PredictiveAnalytics Service

```
src/PredictiveAnalytics.Domain/
├── Signals/
│   └── PredictiveMaintenanceSignal.cs     # ML model prediction signal
└── Services/
    └── AssetDeteriorationPredictor.cs     # Raises prediction signals
```

---

## 8. Dependency Flow

```
┌─────────────────────────────────────────────────────────┐
│ API Layer                                               │
│  - Controllers                                          │
│  - Authorization policies                               │
│  - Middleware                                           │
└────────────────────┬────────────────────────────────────┘
                     │ depends on
                     ▼
┌─────────────────────────────────────────────────────────┐
│ Application Layer                                       │
│  - Commands/Queries                                     │
│  - Command/Query/Signal Handlers                        │
│  - Validators                                           │
└────────────────────┬────────────────────────────────────┘
                     │ depends on
                     ▼
┌─────────────────────────────────────────────────────────┐
│ Domain Layer                                            │
│  - Aggregates (raise signals)                           │
│  - Domain services                                      │
│  - Business policies (handle signals)                   │
│  - Signal type definitions                              │
└────────────────────┬────────────────────────────────────┘
                     │ depends on
                     ▼
┌─────────────────────────────────────────────────────────┐
│ Platform Abstractions                                   │
│  - ISignalBus                                           │
│  - ISignalHandler<T>                                    │
│  - IBusinessPolicy<T>                                   │
└─────────────────────────────────────────────────────────┘
```

**Rule:** Domain layer never depends on infrastructure or application layers. Signals are defined in domain but handled by application/infrastructure.

---

## Related Documentation

- [Signal-Based Reactive Architecture](070-signal-reactive-architecture.md)
- [Business Logic Governance](075-business-logic-governance.md)
- [Signal Patterns Guide](../../04-events-and-messaging/signal-patterns.md)
- [How to Build a Service](../../09-contributing/how-to-build-a-service.md)
