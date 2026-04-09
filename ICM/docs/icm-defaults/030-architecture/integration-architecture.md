# Integration Architecture

> **Role in ICM:** Layer 3 - architecture defaults for structure, data flow, integrations, security, observability, and platform boundaries.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: integration-architecture | docs/icm-defaults/030-architecture/integration-architecture.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: TBD  
> Last updated: 2026-01-10

## Event Streaming Backbone: Event Hub

### Purpose
Primary **domain event stream** for {{ PRODUCT_NAME }}, providing high-throughput event distribution. Events represent **immutable facts** for the **Write Model** (persistence, audit trail, analytics).

### Responsibilities

**Event Hub** enables:
- **Persistence** - Immutable facts for audit trail and compliance
- **Fan-out** to multiple consumers without coupling
- **Read model updates** (CQRS projections)
- **Signal detection** - Events trigger signal detectors for real-time reactions
- **Workflow triggers** (orchestration initiation)
- **Function triggers** (event-driven compute)
- **Analytics ingestion** (EL/ETL to Data Lake)
- **External integrations** (when event-driven patterns apply)

### Event → Signal Bridge

Events serve as the source for signal detection:

```
Event Hub (Write Model)
   ↓ (event consumed by signal detector)
Signal Detector (evaluates thresholds/conditions)
   ↓ (raises lightweight signal)
Signal Bus (Read Model)
   ↓ (pushes via SignalR)
Client UI (Signal Store)
   ↓
Granular UI update without page reload
```

**Example:**
- **Event**: `TransactionCompletedEvent` (complete state saved to DB)
- **Signal**: `AccountBalanceChangedSignal` (lightweight, only UI-needed data)
- **Result**: "Balance" field flashes and updates without full page reload

### Event Processing Patterns

The architecture explicitly supports:

| Pattern | Purpose | Implementation |
|---------|---------|----------------|
| **Cache** | Reduce repeated lookups; idempotency support | Redis or in-memory cache per consumer |
| **Queue** | Buffer bursts; decouple processing | Consumer-side buffering (e.g., in-memory or local queue) |
| **Retry** | Transient fault handling | Exponential backoff with jitter |

**Note:** These patterns are implemented in **consumers/workers**, not inside Event Hub itself.

### Event Design Principles

1. **Events are immutable facts** - describe what happened
2. **Events are versioned** - schema evolution support
3. **Events contain enough context** - avoid chatty lookups when possible
4. **Events use correlation IDs** - enable distributed tracing

---

## Enterprise Messaging Backbone: Service Bus

### Purpose
Reliable **asynchronous messaging** for commands and enterprise integration.

### Responsibilities

**Service Bus** provides:
- **Command messaging** between services (when async commands required)
- **Integration with external systems** A–D
- **Dead Letter Queue (DLQ)** for poison messages and non-recoverable failures
- **Operational replay** and remediation workflows

### When to Use Service Bus vs Event Hub

| Use Case | Platform |
|----------|----------|
| High-throughput event streaming | **Event Hub** |
| Domain event fan-out | **Event Hub** |
| Durable command messaging | **Service Bus** |
| Enterprise integration patterns | **Service Bus** |
| Guaranteed delivery with DLQ | **Service Bus** |

---

## Dead Letter Queue (DLQ) Strategy

### Purpose
Handle messages that cannot be processed after configured retry attempts.

### DLQ Workflow

```
Message fails processing
   → Retry N times (exponential backoff)
   → Still failing → move to DLQ
   → Alert operations team
   → Investigate root cause
   → Fix issue (code/data/external system)
   → Replay from DLQ
```

### Replay Capabilities

- **Manual replay** - ops team triggers after investigation
- **Automated replay** - based on rules (e.g., external system back online)
- **Batch replay** - process multiple DLQ messages together

---

## External Systems Integration

### Systems A–D

External systems integrate with {{ PRODUCT_NAME }} via:
- **Service Bus topics/queues** (preferred for reliability)
- **Event subscriptions** (if external system can consume events)
- **API calls** (for synchronous integration needs)

### Integration Patterns

#### Adapter Pattern (Recommended)

```
External System A
   ↕ (adapter isolates {{ PRODUCT_NAME }} from external instability)
Service Bus Queue/Topic
   ↕
{{ PRODUCT_NAME }} Integration Worker
```

**Benefits:**
- **Isolation** - external system changes don't directly impact {{ PRODUCT_NAME }}
- **Retry/DLQ** - failed integrations are recoverable
- **Versioning** - adapters translate between external contracts and {{ PRODUCT_NAME }} domain

#### Anti-Corruption Layer

For complex external systems:
- Translate external domain model to {{ PRODUCT_NAME }} domain model
- Prevent external complexity from leaking into {{ PRODUCT_NAME }} services

---

## Integration Event Flow

### Event Propagation Flow

1. Domain event published to **Event Hub**
2. Consumers process with **queue/retry** patterns
3. Read model updater updates **READ DB**
4. Functions/Workflows/Integrations trigger in parallel
5. Optional: ETL consumes events and lands data in **Data Lake**

### Integration Command Flow

1. {{ PRODUCT_NAME }} publishes integration message to **Service Bus**
2. External system adapter consumes message
3. Adapter calls external system API
4. If failure → retry → DLQ
5. Operations workflow investigates/replays from DLQ

---

## Messaging Resilience Strategies

### Producer Side
- **Idempotent writes** - handle duplicate sends
- **Correlation IDs** - track message lineage
- **Retry with exponential backoff** - transient failures

### Consumer Side
- **At-least-once delivery** - handle duplicates via idempotency keys
- **Circuit breaker** - prevent cascading failures
- **DLQ** - isolate poison messages
