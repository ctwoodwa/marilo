# Logical Architecture

> **Role in ICM:** Layer 3 - architecture defaults for structure, data flow, integrations, security, observability, and platform boundaries.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: logical-architecture | docs/icm-defaults/030-architecture/logical-architecture.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: TBD  
> Last updated: 2026-01-10

## Target Diagram Reference
`../img/softwareArchitecture.jpg`

---

## 1. Edge Layer (Client + Delivery)

### Components
- **Client** (web/mobile/desktop)
- **Client Cache + Client Queue** (offline support, resiliency)
- **CDN**
- **Static hosting** (SPA/assets)

### Responsibilities
- Client authenticates via **Entra ID**, obtains JWTs
- Client calls **APIM** only (never directly to services)
- Client-side **queue** buffers work for intermittent connectivity
- Client-side **cache** improves UX and reduces API calls
- CDN + Static provide fast UI delivery and reduce load on APIs

---

## 2. API Gateway Layer (APIM)

### Responsibilities
- **Single ingress** for all client traffic
- **JWT validation** + RBAC enforcement
- **Routing** to:
  - {{ PRODUCT_NAME }} microservices APIs
  - AI/MCP endpoints (if exposed to clients)
- **Rate limiting**, throttling, request shaping
- **API versioning** and centralized API documentation integration

---

## 3. Core Domain Layer (Microservices + Component-based Scalable Logical Architecture + Signal-Enhanced Reactivity)

The center of the architecture shows a **microservices cluster**, where domains use **Component-based Scalable Logical Architecture** with separate read/write stores and **signal-enhanced reactive patterns** for real-time updates.

### 3.1 Standard Microservice Pattern

Each microservice contains:
- **API** (REST/gRPC endpoints)
- **Domain logic** (business rules, aggregates)
- **SQL** (service-owned database)

### 3.2 Component-based Scalable Logical Architecture + Signal-Enhanced Service Pattern (Read/Write Separation with Real-Time Updates)

For domains requiring scale, complex queries, or real-time UI reactivity:

```
WRITE component → WRITE database
   ↓ (publishes domain events via Message-Driven Application Framework Transactional Outbox)
Event Hub
   ↓ (read model updater consumes)
READ component ← READ database
   ↓ (signal detector evaluates thresholds/conditions)
Signal Bus (In-Process)
   ↓ (signal handler pushes via SignalR)
Client (Signal Store) → UI updates without full page reload
```

**WRITE side responsibilities:**
- Accept **commands**
- Enforce business invariants
- Persist to **WRITE database**
- Publish **domain events** to Event Hub via **Message-Driven Application Framework Transactional Outbox**

**READ side responsibilities:**
- Accept **queries**
- Return data from **READ database** (materialized/denormalized views)
- Read model updated **asynchronously** from events (eventual consistency)

**SIGNAL-ENHANCED side responsibilities:**
- Detect **threshold breaches** or **significant conditions** from events
- Raise **lightweight signals** for real-time reactions
- Push **targeted updates** to clients via **SignalR**
- Update **Signal Store** on client for reactive UI

### 3.3 Event → Signal Bridge Pattern

**How events and signals work together:**

1. **Command** - User clicks "Submit"
2. **Event (Event-Sourcing)** - System validates rule and saves immutable `TransactionCompleted` event to database
3. **Bridge** - Message-Driven Application Framework picks up event from Transactional Outbox
4. **Signal (Reactive)** - Message-Driven Application Framework pushes specific data update via SignalR to client (e.g., "Balance" field)
5. **UI (Signal-Enhanced)** - Signal Store receives update; specific "Balance" text flashes and updates without page reload

**Benefits:**
- **Events** provide audit trail, time-based queries, and analytics
- **Signals** provide snappy, granular UI updates without polling
- **Message-Driven Application Framework** ensures reliable transformation (events guaranteed delivered before signals raised)

---

## 4. Event-Driven Compute (Functions/Workers)

### Components
- **Functions** (triggered from Event Hub)
- **Workers** (background processors)

### Responsibilities
Lightweight event handlers for:
- **Notifications** (email, SMS, push)
- **Enrichment** (augment events with additional data)
- **Projections** (update read models)
- **Scheduled/triggered automation**
- **Glue** between eventing and external endpoints

**Implementation note:** In Docker-based environments, these may be worker containers; in cloud production, actual serverless Functions—architecture intent is event-triggered compute.

---

## 5. End-to-End Flows

### 5.1 User/API Request Flow

1. Client authenticates via **Entra ID** → receives **JWT**
2. Client calls **APIM** with JWT
3. APIM validates JWT + RBAC → routes to microservice API
4. Service executes:
   - **Query**: hits **READ DB** (Component-based Scalable Logical Architecture) or service SQL
   - **Command**: writes to **WRITE DB** and emits event to **Event Hub**

### 5.2 Event Propagation Flow

1. Domain event published to **Event Hub**
2. Consumers process with **queue/retry** patterns
3. Read model updater updates **READ DB**
4. Functions/Workflows/Integrations trigger in parallel
5. Optional: ETL consumes events and lands data in **Data Lake**

### 5.3 Integration Flow

1. {{ PRODUCT_NAME }} publishes/consumes integration messages via **Service Bus**
2. Failures go to **Dead Letter Queue**
3. Operations workflow investigates/replays from DLQ
