# Domain Primitives

> **Role in ICM:** Layer 3 - domain defaults defining business-rule boundaries, domain primitives, and behavioral governance patterns.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: domain-primitives | docs/icm-defaults/020-domain/domain-primitives.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Purpose

This document defines the **fundamental data types** (primitives) that flow through the {{ PRODUCT_NAME }} system. These primitives are the foundation for:
- Service boundary decisions
- API contract design
- Event schema design
- Data model consistency
- Future requirement flexibility

**Steenberg principle:** *"Identify what flows through your system first—the primitives determine everything else."*

---

## Core Primitive: FacilityAsset

The fundamental unit of information in {{ PRODUCT_NAME }} is a **FacilityAsset** with **ConditionData**.

### Definition

A **FacilityAsset** represents any physical component of a facility that:
1. Has a **location** (building, floor, room, or spatial coordinate)
2. Has a **lifecycle** (installation date, expected useful life, renewal date)
3. Has a **condition** (assessment scores, deficiencies, health metrics)
4. Has a **cost profile** (replacement value, deferred maintenance cost)
5. Has a **classification** (system, subsystem, component hierarchy)

### Primitive Properties

```csharp
// The primitive that flows through {{ PRODUCT_NAME }}
public record FacilityAsset
{
    public AssetIdentity Identity { get; init; }
    public AssetLocation Location { get; init; }
    public AssetLifecycle Lifecycle { get; init; }
    public AssetCondition Condition { get; init; }
    public AssetCostProfile CostProfile { get; init; }
    public AssetClassification Classification { get; init; }
}

public record AssetIdentity(
    string AssetId,           // Stable identifier
    string AssetName,
    AssetType Type            // HVAC, Electrical, Structural, etc.
);

public record AssetLocation(
    string FacilityId,
    string BuildingId,
    string? FloorId,
    string? RoomId,
    GeoCoordinate? Coordinate
);

public record AssetLifecycle(
    DateOnly InstallationDate,
    int ExpectedUsefulLifeYears,
    DateOnly NextRenewalDate,
    decimal PercentLifeUsed
);

public record AssetCondition(
    ConditionScore Score,      // Numeric or categorical (Excellent/Good/Fair/Poor)
    List<Deficiency> Deficiencies,
    DateOnly LastAssessmentDate
);

public record AssetCostProfile(
    decimal ReplacementValue,
    decimal DeferredMaintenanceCost,
    decimal SoftCostsFactor
);

public record AssetClassification(
    string MajorSystem,        // e.g., "HVAC"
    string System,             // e.g., "Heating"
    string? Subsystem          // e.g., "Boiler"
);
```

---

## Secondary Primitives

### AssessmentEvent

An **AssessmentEvent** records an observation about a FacilityAsset at a point in time.

```csharp
public record AssessmentEvent
{
    public string EventId { get; init; }
    public string AssetId { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public ActorContext Actor { get; init; }
    public ConditionScore ObservedScore { get; init; }
    public List<Deficiency> IdentifiedDeficiencies { get; init; }
    public string? Notes { get; init; }
    public List<MediaReference> Photos { get; init; }
}
```

**Operations on AssessmentEvent:**
- Capture during site visit
- Validate against business rules
- Publish to Event Hub
- Persist to event store
- Update read models (Asset inventory, dashboards)

### WorkOrder

A **WorkOrder** represents a maintenance or repair action required for an asset.

```csharp
public record WorkOrder
{
    public string WorkOrderId { get; init; }
    public string AssetId { get; init; }
    public WorkOrderType Type { get; init; }  // Preventive, Corrective, Renewal
    public WorkOrderPriority Priority { get; init; }
    public decimal EstimatedCost { get; init; }
    public DateOnly ScheduledDate { get; init; }
    public WorkOrderStatus Status { get; init; }
}
```

**Operations on WorkOrder:**
- Create from deficiency
- Schedule and assign
- Track completion
- Calculate deferred maintenance impact
- Publish lifecycle events

### FacilityMetrics

Aggregate metrics calculated across assets.

```csharp
public record FacilityMetrics
{
    public string FacilityId { get; init; }
    public decimal FacilityConditionIndex { get; init; }  // FCI = Deficiencies / ReplacementValue
    public decimal TotalReplacementValue { get; init; }
    public decimal TotalDeferredMaintenance { get; init; }
    public DateOnly CalculatedDate { get; init; }
    public Dictionary<string, decimal> SystemBreakdown { get; init; }  // By major system
}
```

---

## Primitive Operations

For each primitive, define the **minimal set of operations** required:

### FacilityAsset Operations
- **Create** - register new asset
- **Update** - change properties (location, classification, cost)
- **Assess** - record condition observation → generates AssessmentEvent
- **Query** - retrieve by ID, location, classification, condition
- **Archive** - mark as retired/demolished

### AssessmentEvent Operations
- **Capture** - record observation during assessment
- **Publish** - emit to Event Hub for downstream processing
- **Query** - retrieve by asset, date range, actor
- **Export** - include in batch export packages

### WorkOrder Operations
- **Create** - generate from deficiency or scheduled maintenance
- **Assign** - schedule and allocate resources
- **Complete** - mark done and record actual cost
- **Defer** - postpone and track deferred maintenance accumulation
- **Query** - retrieve by asset, priority, status

---

## Design Principles for Primitives

### 1. Implementation-Agnostic

Primitives describe **what the data means**, not **how it's stored**.

❌ **Bad:** `SqlFacilityAssetRow` with database-specific annotations  
✅ **Good:** `FacilityAsset` record with semantic properties

### 2. Future-Proof Structure

Design primitives to handle **10x current requirements** without breaking changes.

**Example:** AssetLocation includes optional `GeoCoordinate` even if not used initially—enables future IoT sensor integration without schema changes.

### 3. Stable Identifiers

All primitives use **deterministic, stable IDs** that survive:
- Database migrations
- Cross-system integration
- Export/import cycles
- Merge operations

### 4. Event-Friendly

Primitives should be **serializable** and **self-contained** for event streaming:
- No circular references
- No database-specific types
- Include all context needed for downstream processing

---

## Primitive-to-Service Mapping

| Primitive | Owning Service | Consumers |
|-----------|---------------|-----------|
| **FacilityAsset** | Asset Service | Assessment Service, Work Order Service, Reporting Service |
| **AssessmentEvent** | Assessment Service | Asset Service (read model updates), Analytics, Export Service |
| **WorkOrder** | Work Order Service | Asset Service (condition impact), Scheduling Service, Reporting |
| **FacilityMetrics** | Analytics Service | Reporting Service, Dashboard Service, AI/MCP (RAG context) |

**Rule:** Services communicate by passing **complete primitives** through APIs or events. Never expose internal storage structures.

---

## Generalization for Future Use Cases

These primitives support expansion without breaking changes:

### Current Use Case: Building Condition Assessment
- FacilityAsset = HVAC system, roof, electrical panel
- AssessmentEvent = inspector observation during site visit
- WorkOrder = repair or renewal project

### Future Use Case: Predictive Maintenance (IoT Sensors)
- FacilityAsset = **same primitive**, now includes IoT device references
- AssessmentEvent = **same primitive**, now includes sensor readings (not just human observations)
- WorkOrder = **same primitive**, now generated by ML predictions

### Future Use Case: Portfolio Management
- FacilityAsset = **same primitive**, aggregated across multiple facilities
- FacilityMetrics = **same primitive**, rolled up to portfolio level
- WorkOrder = **same primitive**, scheduled across portfolio budget constraints

**Key insight:** Primitives remain stable; services and operations evolve around them.

---

## Validation Rules

Enforce these invariants at primitive boundaries:

### FacilityAsset Invariants
- `AssetId` must be deterministic (hash of facility + location + classification)
- `ExpectedUsefulLifeYears` must be > 0
- `PercentLifeUsed` must be 0-100
- `NextRenewalDate` must be >= today if asset is active

### AssessmentEvent Invariants
- `Timestamp` must be <= current time (no future observations)
- `AssetId` must reference existing asset
- `ObservedScore` must be in valid range for asset type
- `Actor` must include authenticated user context

### WorkOrder Invariants
- `EstimatedCost` must be > 0
- `ScheduledDate` cannot be in past if Status = Pending
- Priority calculation: based on condition score + safety impact

**Implementation:** Validate at API gateway and again at service boundary. Never trust input.

---

## Messaging Primitive: DomainEvent

All events flowing through Event Hub extend this base:

```csharp
public abstract record DomainEvent
{
    public string EventId { get; init; } = Guid.NewGuid().ToString();
    public string EventType { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public string CorrelationId { get; init; }
    public string? CausationId { get; init; }
    public ActorContext Actor { get; init; }
}

// Example concrete event
public record AssetConditionChanged : DomainEvent
{
    public string AssetId { get; init; }
    public ConditionScore PreviousScore { get; init; }
    public ConditionScore NewScore { get; init; }
    public List<Deficiency> NewDeficiencies { get; init; }
}
```

**Rule:** All services must publish/consume events using this schema. No custom event formats.

---

## Next Steps

1. **Review with domain experts** - validate primitives cover all current use cases
2. **Build reference implementations** - Asset Service using FacilityAsset primitive
3. **Define event catalog** - all DomainEvent types with schemas
4. **Create validation library** - shared invariant enforcement across services
5. **Update API contracts** - all endpoints accept/return primitives (not DTOs)

---

## Related Documentation

- [logical-architecture.md](logical-architecture.md) - how services use these primitives
- [integration-architecture.md](integration-architecture.md) - how primitives flow through Event Hub
- [data-architecture.md](data-architecture.md) - how primitives are persisted
- `../03-apis/` - API contracts using these primitives
- `../00-overview/010-glossary.md` - domain terminology
