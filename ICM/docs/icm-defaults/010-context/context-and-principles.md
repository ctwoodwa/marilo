# Context and Principles

> **Role in ICM:** Layer 3 - enterprise architecture context for software development workspaces. Canonical rules for system goals and cross-cutting capabilities.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: context-and-principles | docs/icm-defaults/010-context/context-and-principles.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: TBD  
> Last updated: 2026-01-10

## System Context

{{ PRODUCT_NAME }} is the **end-state architecture** for a **secure, observable, event-driven microservices** system with **signal-enhanced reactive architecture**, featuring:

- **CQRS** (Read/Write separation) for scalable domain services
- **Signal-Enhanced Reactivity** (Real-time UI updates and threshold-based reactions)
- **API Gateway** (APIM) as single client ingress
- **AI Integration** (OpenAI + MCP) with controlled tool access
- **Enterprise Integration** via Service Bus with external systems
- **Analytics/ML Pipeline** via Data Lake + EL/ETL
- **Full observability** via OpenTelemetry across all components

Reference diagram: `../img/softwareArchitecture.jpg`
described in [Modern Architecture 101 for New Engineers & Forgetful Experts - Jerry Nixon - NDC Copenhagen 2025](https://www.youtube.com/watch?v=WRg13Ze_UpY)

---

## Architectural Principles

### 1. Service-Owned Data
Each microservice owns its data store. No direct cross-service database access. Integration occurs through:
- **Events** (domain changes via Event Hub)
- **APIs** (synchronous queries/commands)
- **Messages** (asynchronous commands via Service Bus)

### 2. Event-Driven Integration with Signal-Enhanced Reactivity
Domain events and reactive signals work together as complementary patterns:
- **Events (Event Hub)** - Immutable facts for persistence, audit trail, and analytics (Write Model)
- **Signals (In-Process + SignalR)** - Lightweight reactive triggers for real-time UI updates and threshold reactions (Read Model)
- **Message-Driven Application Framework as Bridge** - Transforms heavy domain events into lightweight reactive signals via Transactional Outbox
- **Service Bus** for durable command messaging and enterprise integration
- Eventual consistency where appropriate; strong consistency where required

**How they work together:**
1. **Command** → User action (e.g., "Submit")
2. **Event** → System validates and saves immutable event to database (Event-Sourcing)
3. **Bridge** → Message-Driven Application Framework picks up event from Transactional Outbox
4. **Signal** → Message-Driven Application Framework pushes specific data update via SignalR to client
5. **UI** → Signal Store receives it, UI updates without full page reload

### 3. Tiered Reactive Architecture
Use the right pattern for each layer:
- **Event-Sourcing for Write Model (Persistence)** - Audit trail, time-based logic, compliance
- **Signals for Read Model (UI/Local Logic)** - Snappy, high-performance user experience
- **Message-Driven Application Framework as Glue** - Turns heavy domain events into lightweight reactive signals

**Decision Matrix:**
| Layer | Pattern | Purpose |
|-------|---------|---------|
| Write Model | Events (Event Hub) | Persistence, audit trail, analytics pipeline |
| Read Model | Signals (SignalR + Signal Store) | Real-time UI updates, threshold monitoring |
| Bridge | Message-Driven Application Framework (Transactional Outbox) | Reliable event → signal transformation |

### 4. Secure by Default
All components enforce authentication and authorization:
- **Entra ID** as identity provider (OIDC/OAuth2)
- **JWT tokens** for authentication across all services
- **RBAC policies** enforced at gateway and service boundaries
- Least-privilege access for AI tools and service principals

### 5. Observable by Default
Every component emits telemetry via OpenTelemetry:
- **Distributed traces** across gateway, services, workers
- **Metrics** for performance and business KPIs
- **Structured logs** with correlation IDs
- Consistent naming and instrumentation standards

### 6. Polyglot Persistence
Use the right storage technology for each use case:
- **Row store (SQL)** for transactional OLTP
- **NoSQL** for flexible schemas and high throughput
- **In-memory** for caching and transient state
- **Column store** for analytics and aggregations
- **Graph** for relationship-heavy queries

---

## Implementation Constraints

These constraints establish the technology foundation:

| Constraint | Architectural Impact |
|------------|---------------------|
| **.NET 10 / C#** | All APIs, workers, and integration services implemented in C# |
| **Docker hosting** | Every microservice/worker runs as a container |
| **.NET Aspire** | Local composition, service discovery/config wiring, OTEL defaults |
| **{{ PRODUCT_NAME }} patterns** | Microservice boundaries, integration/eventing patterns, dev experience conventions |

---

## Cross-Cutting Capabilities

These capabilities apply to all components:

| Component | Purpose | Implementation |
|-----------|---------|----------------|
| **Entra ID** | Identity provider | OIDC/OAuth2; users + service principals |
| **JWT + RBAC** | AuthN/AuthZ | JWT validation at gateway + services; role/permission policies |
| **Docs** | Central documentation | Architecture + APIs + runbooks |
| **OTEL** | Observability | Distributed traces, metrics, logs across gateway, services, workers |
| **Workflow** | Orchestration | Long-running business processes and compensating actions |
| **Reporting** | Operational/BI reporting | Pulls from read models and/or lake curated data |
