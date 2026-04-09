# Business Logic Governance with Signal-Based Architecture

> **Role in ICM:** Layer 3 - domain defaults defining business-rule boundaries, domain primitives, and behavioral governance patterns.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: business-logic-governance | docs/icm-defaults/020-domain/business-logic-governance.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> **Status**: Draft  
> **Owner**: Architecture Team  
> **Last updated**: 2026-01-25

---

## Purpose

This document defines how {{ PRODUCT_NAME }} implements **Component-based Scalable Logical Architecture-inspired business logic governance** within a signal-based reactive architecture. It maps Component-based Scalable Logical Architecture's proven patterns (business objects, rules, authorization) to {{ PRODUCT_NAME }}'s microservices/CQRS architecture while maintaining strict domain rule enforcement.

**Strategic goal:** Combine Component-based Scalable Logical Architecture's rigorous business logic organization with modern event-driven/signal-based patterns for long-term maintainability.

---

## 1. Component-based Scalable Logical Architecture Principles Applied to {{ PRODUCT_NAME }}

### 1.1 Core Component-based Scalable Logical Architecture Concepts

From `Examples\Component-based Scalable Logical Architecture\Component-based Scalable Logical ArchitectureSummary.md`, Component-based Scalable Logical Architecture provides:

1. **Object Stereotypes** - Editable roots, read-only lists, command objects
2. **Business Rules Framework** - Validation, authorization, calculations
3. **Data Portal** - Async lifetime management
4. **N-Level Undo** - Change tracking
5. **Property-Level Authorization** - Fine-grained security

### 1.2 Mapping to {{ PRODUCT_NAME }} Architecture

| Component-based Scalable Logical Architecture Pattern | {{ PRODUCT_NAME }} Implementation | Location |
|--------------|----------------------|----------|
| **Editable Root** | Domain Aggregate with WolverineFx command handlers | `*.Domain/Aggregates/` |
| **Read-Only Root** | CQRS Read Model | `*.Infrastructure.Read/ReadModels/` |
| **Command Object** | WolverineFx Command | `*.Application/Commands/` |
| **Business Rules** | Signal-Triggered Policy Engine | `*.Domain/Policies/` |
| **Authorization Rules** | ASP.NET Core + Claim-based policies | `*.API/Authorization/` |
| **Data Portal** | WolverineFx + EF Core with Outbox | Infrastructure layer |

---

## 2. Business Object Patterns

### 2.1 Editable Domain Aggregate (Replaces Component-based Scalable Logical Architecture Editable Root)

{{ PRODUCT_NAME }} uses **DDD Aggregates** with **signal-based validation**.

**Component-based Scalable Logical Architecture Pattern:**
```csharp
[Serializable]
public class Customer : BusinessBase<Customer>
{
    public static readonly PropertyInfo<string> NameProperty = 
        RegisterProperty<string>(nameof(Name));
    
    public string Name
    {
        get => GetProperty(NameProperty);
        set => SetProperty(NameProperty, value);
    }
    
    protected override void AddBusinessRules()
    {
        BusinessRules.AddRule(new Required(NameProperty));
        BusinessRules.AddRule(new MaxLength(NameProperty, 100));
    }
}
```

**{{ PRODUCT_NAME }} Equivalent:**
```csharp
/// <summary>
/// Domain aggregate - owns business invariants
/// Raises domain events and signals on state changes
/// </summary>
public class FacilityAssetAggregate : AggregateRoot<AssetId>
{
    // Properties use private setters (immutable from outside)
    public AssetId Id { get; private set; }
    public AssetName Name { get; private set; }
    public AssetCondition Condition { get; private set; }
    
    // Factory method (replaces Component-based Scalable Logical Architecture CreateAsync)
    public static FacilityAssetAggregate Create(
        AssetId id,
        AssetName name,
        AssetClassification classification)
    {
        var asset = new FacilityAssetAggregate(id);
        
        // Business rule validation
        asset.ValidateCreation(name, classification);
        
        // Raise domain event
        asset.AddDomainEvent(new AssetCreatedEvent
        {
            AssetId = id.Value,
            Name = name.Value,
            Classification = classification
        });
        
        asset.Name = name;
        asset.Classification = classification;
        
        return asset;
    }
    
    // Business operation with validation
    public void UpdateCondition(
        ConditionScore newScore,
        List<Deficiency> deficiencies,
        ActorContext actor)
    {
        // Validate business rules
        if (newScore.Numeric < 1 || newScore.Numeric > 5)
            throw new BusinessRuleViolationException("Condition score must be 1-5");
        
        var previousCondition = Condition;
        Condition = new AssetCondition(newScore, deficiencies, DateOnly.FromDateTime(DateTime.UtcNow));
        
        // Raise domain event
        AddDomainEvent(new AssetConditionChangedEvent
        {
            AssetId = Id.Value,
            PreviousScore = previousCondition.Score,
            NewScore = newScore,
            NewDeficiencies = deficiencies,
            Actor = actor
        });
        
        // Check if signal should be raised
        CheckConditionThresholds(newScore);
    }
    
    // Signal detection (new capability beyond Component-based Scalable Logical Architecture)
    private void CheckConditionThresholds(ConditionScore newScore)
    {
        // Signal raised if condition index drops below threshold
        if (CalculateConditionIndex(newScore) < 0.4m)
        {
            AddSignal(new AssetConditionThresholdSignal
            {
                SignalType = "AssetConditionThreshold",
                SourceId = Id.Value,
                AssetId = Id.Value,
                CurrentCI = CalculateConditionIndex(newScore),
                ThresholdCI = 0.4m,
                Trend = CalculateTrend()
            });
        }
    }
    
    private decimal CalculateConditionIndex(ConditionScore score)
    {
        // FCI = 1 - (score / max_score)
        return 1.0m - (score.Numeric / 5.0m);
    }
}
```

**Key differences from Component-based Scalable Logical Architecture:**
- No property change tracking (use event sourcing or EF change tracking)
- Signals supplement events for reactive triggers
- WolverineFx handles command routing (replaces Data Portal)

### 2.2 Read-Only Object (CQRS Read Model)

**Component-based Scalable Logical Architecture Pattern:**
```csharp
[Serializable]
public class CustomerInfo : ReadOnlyBase<CustomerInfo>
{
    public static readonly PropertyInfo<string> NameProperty = 
        RegisterProperty<string>(nameof(Name));
    public string Name => GetProperty(NameProperty);
    
    [Fetch]
    private async Task Fetch(int id)
    {
        var data = await _dal.GetCustomerAsync(id);
        LoadProperty(NameProperty, data.Name);
    }
}
```

**{{ PRODUCT_NAME }} Equivalent:**
```csharp
/// <summary>
/// Read model - optimized for queries
/// Updated by signal handlers or event consumers
/// </summary>
public class AssetListReadModel
{
    public string AssetId { get; init; }
    public string Name { get; init; }
    public decimal ConditionIndex { get; init; }
    public ConditionLabel ConditionLabel { get; init; }
    public DateOnly LastAssessmentDate { get; init; }
    public int DeficiencyCount { get; init; }
    public bool RequiresAttention { get; init; }  // Set by signal handler
}

/// <summary>
/// Read model updater - consumes events and signals
/// </summary>
public class AssetListReadModelUpdater : 
    IEventHandler<AssetConditionChangedEvent>,
    ISignalHandler<AssetConditionThresholdSignal>
{
    private readonly IReadModelRepository<AssetListReadModel> _readRepo;
    
    // Event handler: update read model with new condition
    public async Task Handle(AssetConditionChangedEvent @event, EventContext ctx)
    {
        var readModel = await _readRepo.GetByIdAsync(@event.AssetId);
        
        await _readRepo.UpdateAsync(@event.AssetId, rm => rm with
        {
            ConditionIndex = CalculateCI(@event.NewScore),
            ConditionLabel = @event.NewScore.Label,
            LastAssessmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            DeficiencyCount = @event.NewDeficiencies.Count
        });
    }
    
    // Signal handler: flag asset as requiring attention
    public async Task HandleAsync(AssetConditionThresholdSignal signal, CancellationToken ct)
    {
        await _readRepo.UpdateAsync(signal.AssetId, rm => rm with
        {
            RequiresAttention = true  // Reactive flag set by signal
        });
    }
}
```

**Benefits over Component-based Scalable Logical Architecture read-only objects:**
- Eventual consistency (updated asynchronously via events)
- Denormalized for query performance
- Signal handlers enable real-time flags without polling

### 2.3 Command Objects (WolverineFx Commands)

**Component-based Scalable Logical Architecture Pattern:**
```csharp
[Serializable]
public class DeleteCustomerCommand : CommandBase<DeleteCustomerCommand>
{
    public int CustomerId { get; private set; }
    
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

**{{ PRODUCT_NAME }} Equivalent:**
```csharp
/// <summary>
/// Command - intent to change system state
/// Handled by WolverineFx (replaces Component-based Scalable Logical Architecture Data Portal)
/// </summary>
public record UpdateAssetConditionCommand
{
    public required string AssetId { get; init; }
    public required ConditionScore NewScore { get; init; }
    public required List<Deficiency> Deficiencies { get; init; }
    public required ActorContext Actor { get; init; }
}

/// <summary>
/// Command handler - executes business logic
/// </summary>
public class UpdateAssetConditionHandler
{
    private readonly IAssetRepository _assetRepo;
    private readonly IBusinessRuleEngine _ruleEngine;
    private readonly ISignalBus _signalBus;
    
    // WolverineFx convention-based handler
    public async Task<AssetConditionUpdatedResult> Handle(
        UpdateAssetConditionCommand command,
        CancellationToken ct)
    {
        // 1. Load aggregate
        var asset = await _assetRepo.GetByIdAsync(command.AssetId);
        
        // 2. Evaluate business rules
        var rules = await _ruleEngine.EvaluateAsync("UpdateAssetCondition", new
        {
            Asset = asset,
            NewScore = command.NewScore,
            Actor = command.Actor
        });
        
        if (rules.AnyViolated)
        {
            throw new BusinessRuleViolationException(rules.Violations);
        }
        
        // 3. Execute domain operation
        asset.UpdateCondition(command.NewScore, command.Deficiencies, command.Actor);
        
        // 4. Persist (EF Core with Wolverine Outbox)
        await _assetRepo.UpdateAsync(asset);
        
        // 5. Signals raised by aggregate are processed here
        foreach (var signal in asset.GetPendingSignals())
        {
            await _signalBus.RaiseAsync(signal, ct);
        }
        
        return new AssetConditionUpdatedResult
        {
            AssetId = asset.Id.Value,
            NewConditionIndex = asset.Condition.CalculateIndex(),
            SignalsRaised = asset.GetPendingSignals().Count
        };
    }
}
```

---

## 3. Business Rules Framework

### 3.1 Component-based Scalable Logical Architecture Business Rules vs {{ PRODUCT_NAME }} Policy Engine

**Component-based Scalable Logical Architecture Approach:**
```csharp
protected override void AddBusinessRules()
{
    // Property-level rules
    BusinessRules.AddRule(new Required(NameProperty));
    BusinessRules.AddRule(new MaxLength(NameProperty, 100));
    
    // Object-level rules
    BusinessRules.AddRule(new ValidAge());
    
    // Authorization rules
    BusinessRules.AddRule(new IsInRole(AuthorizationActions.WriteProperty, 
        NameProperty, "Admin"));
}
```

**{{ PRODUCT_NAME }} Policy Engine (Signal-Triggered):**
```csharp
/// <summary>
/// Business rule policy - executed when signal raised
/// Replaces Component-based Scalable Logical Architecture business rule registration
/// </summary>
public interface IBusinessPolicy<TSignal> where TSignal : Signal
{
    Task<PolicyEvaluationResult> EvaluateAsync(TSignal signal, CancellationToken ct);
}

/// <summary>
/// Example: Asset condition threshold policy
/// </summary>
public class AssetConditionThresholdPolicy : IBusinessPolicy<AssetConditionThresholdSignal>
{
    private readonly IWorkOrderService _workOrderService;
    private readonly INotificationService _notificationService;
    
    public async Task<PolicyEvaluationResult> EvaluateAsync(
        AssetConditionThresholdSignal signal,
        CancellationToken ct)
    {
        var result = new PolicyEvaluationResult();
        
        // Rule 1: Critical CI (< 0.3) requires immediate work order
        if (signal.CurrentCI < 0.3m)
        {
            result.Actions.Add(new PolicyAction
            {
                ActionType = "CreateWorkOrder",
                Priority = WorkOrderPriority.Critical,
                Reason = "Asset CI below critical threshold"
            });
        }
        // Rule 2: Warning CI (0.3-0.4) requires notification
        else if (signal.CurrentCI < 0.4m)
        {
            result.Actions.Add(new PolicyAction
            {
                ActionType = "SendNotification",
                Priority = NotificationPriority.High,
                Reason = "Asset CI below warning threshold"
            });
        }
        
        // Rule 3: Safety-critical assets always escalate
        if (await IsSafetyCriticalAsset(signal.AssetId))
        {
            result.Actions.Add(new PolicyAction
            {
                ActionType = "EscalateToManagement",
                Priority = EscalationPriority.Immediate,
                Reason = "Safety-critical asset degradation"
            });
        }
        
        return result;
    }
}

/// <summary>
/// Policy evaluation result
/// </summary>
public class PolicyEvaluationResult
{
    public List<PolicyAction> Actions { get; init; } = new();
    public List<RuleViolation> Violations { get; init; } = new();
    public Dictionary<string, object> Metadata { get; init; } = new();
}

public record PolicyAction
{
    public required string ActionType { get; init; }
    public required object Priority { get; init; }
    public required string Reason { get; init; }
}
```

### 3.2 Policy Execution Pipeline

Signals trigger policy evaluation automatically.

```csharp
/// <summary>
/// Signal handler that enforces business policies
/// Executes registered policies and generates commands
/// </summary>
public class PolicyEnforcementHandler : ISignalHandler<AssetConditionThresholdSignal>
{
    private readonly IEnumerable<IBusinessPolicy<AssetConditionThresholdSignal>> _policies;
    private readonly ICommandQueue _commandQueue;
    private readonly IPolicyAuditService _auditService;
    
    public async Task HandleAsync(AssetConditionThresholdSignal signal, CancellationToken ct)
    {
        var allResults = new List<PolicyEvaluationResult>();
        
        // Execute all registered policies
        foreach (var policy in _policies)
        {
            var result = await policy.EvaluateAsync(signal, ct);
            allResults.Add(result);
            
            // Audit policy execution
            await _auditService.RecordPolicyEvaluationAsync(new PolicyAuditEntry
            {
                PolicyType = policy.GetType().Name,
                SignalId = signal.SignalId,
                SignalType = signal.SignalType,
                Result = result,
                EvaluatedAt = DateTimeOffset.UtcNow
            });
        }
        
        // Execute actions from all policies
        foreach (var action in allResults.SelectMany(r => r.Actions))
        {
            await ExecuteActionAsync(action, signal, ct);
        }
    }
    
    private async Task ExecuteActionAsync(
        PolicyAction action,
        AssetConditionThresholdSignal signal,
        CancellationToken ct)
    {
        switch (action.ActionType)
        {
            case "CreateWorkOrder":
                await _commandQueue.SendAsync(new CreateWorkOrderCommand
                {
                    AssetId = signal.AssetId,
                    Priority = (WorkOrderPriority)action.Priority,
                    Reason = action.Reason,
                    TriggeredBySignalId = signal.SignalId
                });
                break;
            
            case "SendNotification":
                await _commandQueue.SendAsync(new SendNotificationCommand
                {
                    Recipients = await GetAssetOwners(signal.AssetId),
                    Subject = $"Asset {signal.AssetId} requires attention",
                    Body = action.Reason,
                    Priority = (NotificationPriority)action.Priority
                });
                break;
            
            case "EscalateToManagement":
                await _commandQueue.SendAsync(new EscalateIncidentCommand
                {
                    IncidentType = "AssetDegradation",
                    AssetId = signal.AssetId,
                    Severity = (EscalationPriority)action.Priority,
                    Details = action.Reason
                });
                break;
        }
    }
}
```

### 3.3 Versioned Policy Contracts

Policies are versioned for compliance and auditability.

```csharp
/// <summary>
/// Versioned business policy
/// Enables A/B testing and regulatory compliance
/// </summary>
public abstract class VersionedBusinessPolicy<TSignal> : IBusinessPolicy<TSignal>
    where TSignal : Signal
{
    public abstract string PolicyId { get; }
    public abstract int Version { get; }
    public abstract DateTime EffectiveDate { get; }
    public abstract DateTime? ExpirationDate { get; }
    public abstract string ApprovedBy { get; }  // Compliance: who approved policy
    public abstract string ComplianceFramework { get; }  // ISO/IEC 42001, NIST, etc.
    
    public abstract Task<PolicyEvaluationResult> EvaluateAsync(TSignal signal, CancellationToken ct);
}

/// <summary>
/// Example: Versioned asset condition policy
/// </summary>
public class AssetConditionThresholdPolicy_V2 : VersionedBusinessPolicy<AssetConditionThresholdSignal>
{
    public override string PolicyId => "AssetConditionThreshold";
    public override int Version => 2;
    public override DateTime EffectiveDate => new DateTime(2026, 2, 1);
    public override DateTime? ExpirationDate => null;
    public override string ApprovedBy => "Chief Engineer, Jane Doe";
    public override string ComplianceFramework => "ISO 55000 (Asset Management)";
    
    public override async Task<PolicyEvaluationResult> EvaluateAsync(
        AssetConditionThresholdSignal signal,
        CancellationToken ct)
    {
        // V2: More aggressive thresholds based on updated risk assessment
        var result = new PolicyEvaluationResult();
        
        if (signal.CurrentCI < 0.35m)  // V1 was 0.3m
        {
            result.Actions.Add(new PolicyAction
            {
                ActionType = "CreateWorkOrder",
                Priority = WorkOrderPriority.Critical,
                Reason = $"Asset CI {signal.CurrentCI:F2} below V2 critical threshold 0.35"
            });
        }
        
        return result;
    }
}
```

---

## 4. Authorization and Security

### 4.1 Component-based Scalable Logical Architecture Authorization vs {{ PRODUCT_NAME }} Claims-Based

**Component-based Scalable Logical Architecture Property-Level Authorization:**
```csharp
public static readonly PropertyInfo<decimal> SalaryProperty =
    RegisterProperty<decimal>(nameof(Salary));

public decimal Salary
{
    get => CanReadProperty(SalaryProperty) 
        ? GetProperty(SalaryProperty) 
        : 0m;
    set
    {
        if (CanWriteProperty(SalaryProperty))
            SetProperty(SalaryProperty, value);
    }
}

protected override void AddBusinessRules()
{
    BusinessRules.AddRule(new IsInRole(
        AuthorizationActions.ReadProperty, 
        SalaryProperty, 
        "HR", "Manager"));
}
```

**{{ PRODUCT_NAME }} Equivalent:**
```csharp
/// <summary>
/// Authorization policy for signal handling
/// </summary>
public class AssetConditionAuthorizationPolicy : 
    IAuthorizationHandler<UpdateAssetConditionCommand>
{
    public Task<AuthorizationResult> AuthorizeAsync(
        UpdateAssetConditionCommand command,
        ClaimsPrincipal user)
    {
        // Claim-based authorization
        if (!user.HasClaim("permission", "asset:update"))
        {
            return Task.FromResult(AuthorizationResult.Fail(
                "User lacks 'asset:update' permission"));
        }
        
        // Role-based authorization
        if (!user.IsInRole("AssetManager") && !user.IsInRole("Inspector"))
        {
            return Task.FromResult(AuthorizationResult.Fail(
                "User must be AssetManager or Inspector"));
        }
        
        // Property-level authorization (facility access)
        var facilityId = GetFacilityIdForAsset(command.AssetId);
        if (!user.HasClaim("facility:access", facilityId))
        {
            return Task.FromResult(AuthorizationResult.Fail(
                $"User lacks access to facility {facilityId}"));
        }
        
        return Task.FromResult(AuthorizationResult.Succeed());
    }
}

/// <summary>
/// WolverineFx middleware enforces authorization
/// </summary>
public class AuthorizationMiddleware : ICommandMiddleware
{
    private readonly IAuthorizationService _authz;
    
    public async Task<TResult> ExecuteAsync<TCommand, TResult>(
        TCommand command,
        CommandContext context,
        Func<Task<TResult>> next)
    {
        var handler = GetAuthorizationHandler<TCommand>();
        var result = await handler.AuthorizeAsync(command, context.User);
        
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException(result.FailureReason);
        }
        
        return await next();
    }
}
```

### 4.2 Signal-Based Authorization

Signals can trigger authorization policy re-evaluation.

```csharp
/// <summary>
/// Signal: User authorization policy changed
/// Triggers re-evaluation of cached permissions
/// </summary>
public record AuthorizationPolicyChangedSignal : Signal
{
    public required string PolicyId { get; init; }
    public required string AffectedUserId { get; init; }
    public required List<string> ChangedPermissions { get; init; }
}

/// <summary>
/// Handler: Invalidate cached authorization decisions
/// </summary>
public class AuthorizationCacheInvalidationHandler : 
    ISignalHandler<AuthorizationPolicyChangedSignal>
{
    private readonly IAuthorizationCache _cache;
    
    public async Task HandleAsync(
        AuthorizationPolicyChangedSignal signal,
        CancellationToken ct)
    {
        // Invalidate cached permissions for affected user
        await _cache.InvalidateAsync(signal.AffectedUserId);
        
        // Re-evaluate active sessions
        await ReevaluateActiveSessions(signal.AffectedUserId);
    }
}
```

---

## 5. AI Governance Integration

### 5.1 AI-Driven Business Rules

Signals can trigger AI-assisted policy evaluation.

```csharp
/// <summary>
/// AI-enhanced business policy
/// Uses LLM for complex decision-making with human oversight
/// </summary>
public class AIPredictiveMaintenancePolicy : IBusinessPolicy<PredictiveMaintenanceSignal>
{
    private readonly ILargeLanguageModel _llm;
    private readonly IPromptValidationService _promptValidator;
    private readonly IPolicyAuditService _auditService;
    
    public async Task<PolicyEvaluationResult> EvaluateAsync(
        PredictiveMaintenanceSignal signal,
        CancellationToken ct)
    {
        // Step 1: Validate signal meets AI governance requirements
        var governanceCheck = await ValidateAIGovernance(signal);
        if (!governanceCheck.Passed)
        {
            throw new AIGovernanceException(governanceCheck.Violations);
        }
        
        // Step 2: Build approved prompt (no user input)
        var prompt = ApprovedPrompts.EvaluateMaintenancePriority(
            assetId: signal.AssetId,
            failureProbability: signal.FailureProbability,
            daysToFailure: signal.DaysToFailure,
            failureMode: signal.PredictedFailureMode
        );
        
        // Step 3: Call LLM with PII redaction
        var redactedPrompt = await _promptValidator.RedactPII(prompt);
        var aiResponse = await _llm.CompleteAsync(redactedPrompt);
        
        // Step 4: Validate AI output
        var validatedResponse = await _promptValidator.ValidateOutput(aiResponse);
        
        // Step 5: Parse AI decision into policy actions
        var actions = ParseAIResponseToActions(validatedResponse);
        
        // Step 6: Audit AI decision
        await _auditService.RecordAIDecisionAsync(new AIDecisionAudit
        {
            SignalId = signal.SignalId,
            ModelId = signal.ModelId,
            Prompt = redactedPrompt,
            Response = validatedResponse,
            Actions = actions,
            Timestamp = DateTimeOffset.UtcNow
        });
        
        return new PolicyEvaluationResult
        {
            Actions = actions,
            Metadata = new Dictionary<string, object>
            {
                ["AIModelId"] = signal.ModelId,
                ["AIConfidence"] = signal.ModelConfidence,
                ["HumanReviewRequired"] = signal.FailureProbability > 0.8m
            }
        };
    }
    
    private async Task<AIGovernanceCheckResult> ValidateAIGovernance(
        PredictiveMaintenanceSignal signal)
    {
        // Ensure compliance with ISO/IEC 42001, NIST AI RMF
        var checks = new List<string>();
        
        if (signal.ModelConfidence < 0.7m)
            checks.Add("Model confidence below threshold (0.7)");
        
        if (string.IsNullOrEmpty(signal.ModelId))
            checks.Add("Model ID missing (traceability required)");
        
        // Check for bias indicators (from AI governance framework)
        var biasCheck = await CheckForBias(signal);
        if (biasCheck.HasBias)
            checks.Add($"Bias detected: {biasCheck.BiasType}");
        
        return new AIGovernanceCheckResult
        {
            Passed = checks.Count == 0,
            Violations = checks
        };
    }
}
```

### 5.2 Computable Governance Trail

All signal → policy → AI → action sequences are auditable.

```sql
-- Query: Trace signal to AI decision to work order
SELECT 
    s.signal_id,
    s.signal_type,
    s.source_id AS asset_id,
    s.raised_at,
    pe.policy_id,
    pe.policy_version,
    pe.evaluated_at,
    ai.model_id,
    ai.prompt_version,
    ai.confidence,
    ai.decision_at,
    wo.work_order_id,
    wo.created_at
FROM signal_audit s
JOIN policy_evaluation_audit pe ON pe.signal_id = s.signal_id
JOIN ai_decision_audit ai ON ai.policy_evaluation_id = pe.id
JOIN work_orders wo ON wo.triggered_by_signal_id = s.signal_id
WHERE s.signal_id = 'S-12345';
```

**Result:**
```
signal_id | signal_type              | asset_id       | policy_id                | model_id    | work_order_id
----------|--------------------------|----------------|--------------------------|-------------|---------------
S-12345   | PredictiveMaintenance    | ASSET-HVAC-001 | PredictiveMaintenance_V2 | GPT-4-Turbo | WO-67890
```

**Compliance benefit:** Auditors can trace every AI decision from signal to action with full provenance.

---

## 6. Release Channel Isolation

Policies and signals are versioned per release channel.

```csharp
/// <summary>
/// Policy registry supports multiple release channels
/// </summary>
public class PolicyRegistry
{
    public void RegisterPolicy<TSignal>(
        IBusinessPolicy<TSignal> policy,
        ReleaseChannel channel)
        where TSignal : Signal
    {
        _policies[channel].Add(typeof(TSignal), policy);
    }
}

public enum ReleaseChannel
{
    Development,
    UAT,
    Training,
    Production
}

// Configuration per environment
services.AddBusinessPolicies(options =>
{
    var channel = _configuration.GetValue<ReleaseChannel>("ReleaseChannel");
    
    if (channel == ReleaseChannel.Production)
    {
        // Production: Use approved, audited policies
        options.RegisterPolicy<AssetConditionThresholdSignal>(
            new AssetConditionThresholdPolicy_V2(), 
            ReleaseChannel.Production);
    }
    else if (channel == ReleaseChannel.UAT)
    {
        // UAT: Test new policy version
        options.RegisterPolicy<AssetConditionThresholdSignal>(
            new AssetConditionThresholdPolicy_V3_Beta(), 
            ReleaseChannel.UAT);
    }
});
```

---

## 7. Testing Business Logic

### 7.1 Unit Testing Policies

```csharp
[TestClass]
public class AssetConditionThresholdPolicyTests
{
    [TestMethod]
    public async Task CriticalCI_ShouldCreateWorkOrder()
    {
        // Arrange
        var policy = new AssetConditionThresholdPolicy_V2();
        var signal = new AssetConditionThresholdSignal
        {
            SignalType = "AssetConditionThreshold",
            SourceId = "ASSET-001",
            AssetId = "ASSET-001",
            CurrentCI = 0.25m,  // Below critical threshold (0.35)
            ThresholdCI = 0.4m,
            Priority = SignalPriority.Critical
        };
        
        // Act
        var result = await policy.EvaluateAsync(signal, CancellationToken.None);
        
        // Assert
        Assert.AreEqual(1, result.Actions.Count);
        Assert.AreEqual("CreateWorkOrder", result.Actions[0].ActionType);
        Assert.AreEqual(WorkOrderPriority.Critical, result.Actions[0].Priority);
    }
}
```

### 7.2 Integration Testing Signal Flow

```csharp
[TestClass]
public class SignalFlowIntegrationTests
{
    [TestMethod]
    public async Task AssetConditionChange_ShouldRaiseSignalAndCreateWorkOrder()
    {
        // Arrange
        var testHarness = new SignalTestHarness();
        var assetId = "ASSET-TEST-001";
        
        // Act: Raise domain event
        await testHarness.PublishEventAsync(new AssetConditionChangedEvent
        {
            AssetId = assetId,
            NewScore = new ConditionScore(5, ConditionLabel.Critical)
        });
        
        // Assert: Signal was raised
        var signal = await testHarness.WaitForSignalAsync<AssetConditionThresholdSignal>(
            s => s.AssetId == assetId,
            timeout: TimeSpan.FromSeconds(5));
        
        Assert.IsNotNull(signal);
        Assert.IsTrue(signal.CurrentCI < 0.4m);
        
        // Assert: Work order command was sent
        var command = await testHarness.WaitForCommandAsync<CreateWorkOrderCommand>(
            c => c.AssetId == assetId,
            timeout: TimeSpan.FromSeconds(5));
        
        Assert.IsNotNull(command);
        Assert.AreEqual(WorkOrderPriority.Critical, command.Priority);
    }
}
```

---

## 8. Design Principles Summary

### 8.1 Component-based Scalable Logical Architecture Strengths Preserved

✅ **Business logic centralization** - Policies are black boxes  
✅ **Validation framework** - Rules enforced at policy boundaries  
✅ **Authorization governance** - Claims-based + signal-triggered  
✅ **Testability** - Policies are pure functions (signal → result)  
✅ **Reusability** - Policies used across multiple services

### 8.2 {{ PRODUCT_NAME }} Enhancements

✅ **Reactive architecture** - Signals enable real-time decisioning  
✅ **Microservice compatibility** - Works with service boundaries  
✅ **Event sourcing support** - Signals augment events  
✅ **AI integration** - LLM-assisted policy evaluation  
✅ **Compliance traceability** - Full audit trails for governance

---

## Related Documentation

- [Signal-Based Reactive Architecture](070-signal-reactive-architecture.md)
- [Signal Patterns Guide](../../04-events-and-messaging/signal-patterns.md)
- [AI Governance Architecture](../../08-security-and-compliance/120-ai-governance-architecture.md)
- [Component-based Scalable Logical Architecture Summary](../../Examples/Component-based Scalable Logical Architecture/Component-based Scalable Logical ArchitectureSummary.md)
- [WolverineFx ADR](../../02-decision-records/adr-0003-use-wolverinefx.md)
