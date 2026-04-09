# Observability (OTEL)

> **Role in ICM:** Layer 3 - architecture defaults for structure, data flow, integrations, security, observability, and platform boundaries.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: observability | docs/icm-defaults/030-architecture/observability.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: TBD  
> Last updated: 2026-01-10

## Observability Principle

**Observable by default** - every component emits telemetry via OpenTelemetry (OTEL) from day one.

---

## The Three Signals

### 1. Distributed Traces

**What:** End-to-end request flows across services

**Captured:**
- Request path (client → APIM → service → database/events)
- Latency per span (operation)
- Service dependencies
- Errors and exceptions

**Use cases:**
- Diagnose slow requests
- Understand service dependencies
- Root cause analysis for failures

### 2. Metrics

**What:** Time-series data for performance and business KPIs

**Technical metrics:**
- Request rate (requests/sec)
- Latency (p50, p95, p99)
- Error rate (errors/sec, error %)
- Resource utilization (CPU, memory, disk)

**Business metrics:**
- Orders processed/min
- Basket conversions
- Revenue/hour
- Active users

### 3. Structured Logs

**What:** Event records with context

**Captured:**
- **Timestamp** - when event occurred
- **Severity** - INFO, WARN, ERROR, FATAL
- **Correlation ID** - links logs to traces
- **Service** - which service emitted the log
- **Message** - human-readable description
- **Structured fields** - key-value pairs (e.g., userId, orderId)

**Use cases:**
- Debugging specific requests
- Audit trails
- Compliance reporting

---

## OpenTelemetry (OTEL) Standards

### Correlation IDs

**Purpose:** Link related events across services

**Flow:**
```
Client request (generates trace ID)
   ↓
APIM (propagates trace ID)
   ↓
Service A (uses trace ID, adds span ID)
   ↓
Service B (same trace ID, new span ID)
```

**Implementation:**
- **Trace ID** - unique identifier for entire request flow
- **Span ID** - unique identifier for each operation within trace
- **Baggage** - key-value pairs propagated across services (e.g., userId, tenantId)

### Context Propagation

**Mechanism:** W3C Trace Context standard

**Headers:**
- `traceparent: 00-{trace-id}-{span-id}-{flags}`
- `tracestate: {vendor-specific-data}`

### Service and Resource Naming

**Consistency is critical for querying and dashboards.**

**Service naming convention:**
- `{{ PRODUCT_NAME }}.{domain}.{component}`
- Examples: `{{ PRODUCT_NAME }}.ordering.api`, `{{ PRODUCT_NAME }}.basket.worker`, `{{ PRODUCT_NAME }}.catalog.readmodel`

**Resource attributes:**
- `service.name` - service identifier
- `service.version` - deployment version
- `deployment.environment` - dev/test/prod

---

## Instrumentation Strategy

### Automatic Instrumentation

**.NET Aspire provides OTEL defaults:**
- HTTP client/server traces
- Database calls (Entity Framework, ADO.NET)
- Messaging (RabbitMQ, Azure Service Bus)
- Redis cache operations

### Custom Instrumentation

**Add for:**
- Business operations (e.g., "OrderPlaced", "BasketCheckout")
- Critical code paths
- AI tool invocations
- Integration with external systems

**Example:**
```csharp
using var activity = ActivitySource.StartActivity("ProcessOrder");
activity?.SetTag("order.id", orderId);
activity?.SetTag("user.id", userId);
// ... business logic ...
```

---

## OTEL Collector Deployment

### Local (Development)

**.NET Aspire Dashboard:**
- Built-in OTEL collector
- Real-time trace/metric/log viewer
- No additional setup required

### Production

**OTEL Collector (sidecar or gateway pattern):**

```
Services → OTEL Collector → Backend (e.g., Azure Monitor, Jaeger, Prometheus)
```

**Collector responsibilities:**
- Receive telemetry from services
- Buffer and batch for efficiency
- Export to observability backend(s)
- Filter/sample (if needed for cost control)

---

## Observability Backend

### Options

| Backend | Signals | Use Case |
|---------|---------|----------|
| **Azure Monitor** | Traces, Metrics, Logs | Azure-native, integrated |
| **Jaeger** | Traces | Open-source distributed tracing |
| **Prometheus + Grafana** | Metrics | Open-source metrics/dashboards |
| **ELK Stack** | Logs | Open-source log aggregation |
| **Datadog / New Relic** | All | Commercial APM platforms |

**{{ PRODUCT_NAME }} default (TBD):** Likely Azure Monitor (Application Insights) for cloud; Jaeger + Prometheus for self-hosted.

---

## Dashboards and Alerts

### Golden Signals Dashboard

Monitor the "Four Golden Signals" per service:

1. **Latency** - request duration (p50, p95, p99)
2. **Traffic** - requests per second
3. **Errors** - error rate
4. **Saturation** - resource utilization (CPU, memory, connections)

### Business Metrics Dashboard

Track domain-specific KPIs:
- Orders per hour
- Basket conversion rate
- Revenue trends
- Active users

### AI Metrics Dashboard

Monitor AI/MCP usage:
- AI requests/min
- Token consumption
- AI latency
- Tool invocation counts
- Error rates (by tool)

### Alerts

**Critical alerts:**
- Error rate > threshold (e.g., 5%)
- Latency p95 > threshold (e.g., 2 seconds)
- Service down (health check failed)
- Dead letter queue growing (integration failures)

**Warning alerts:**
- Elevated latency (approaching threshold)
- Elevated resource usage (CPU > 80%)
- Token consumption spike (AI cost control)

---

## Compliance and Audit

### Audit Logging

**Capture:**
- Authentication/authorization events
- Data access (especially PII)
- AI tool invocations
- Admin operations

**Storage:**
- Immutable log store (compliance requirement)
- Retention per regulatory requirements (e.g., 7 years)

### PII in Logs

**Rules:**
- **Never log** raw passwords, tokens, credit card numbers
- **Hash or mask** PII if logging required (e.g., email → e***@example.com)
- **Use IDs** instead of names/emails where possible

---

## Operational Runbooks

For detailed monitoring and troubleshooting procedures, see:
- `docs/07-operations/monitoring` (TBD)
- `docs/07-operations/incident-response` (TBD)
