# Data Architecture

> **Role in ICM:** Layer 3 - architecture defaults for structure, data flow, integrations, security, observability, and platform boundaries.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: data-architecture | docs/icm-defaults/030-architecture/data-architecture.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: TBD  
> Last updated: 2026-01-10

## Operational Data Stores

### Service-Owned Databases

**Principle:** Each microservice owns its data store—no direct cross-service database access.

**Default pattern:**
- **SQL (Row Store)** for transactional OLTP
- Service-owned schema
- Service controls data model evolution

### CQRS Read/Write Separation

For domains requiring scale or complex queries:

```
┌─────────────────────┐
│  WRITE DATABASE     │ ← Commands write here
│  (normalized, ACID) │    (strong consistency)
└─────────────────────┘
         ↓ (publishes events)
    Event Hub
         ↓ (read model updater)
┌─────────────────────┐
│  READ DATABASE      │ ← Queries read from here
│  (denormalized)     │    (eventual consistency)
└─────────────────────┘
```

**WRITE DB characteristics:**
- Normalized schema
- Enforces business invariants
- ACID transactions
- Optimized for writes

**READ DB characteristics:**
- Denormalized/materialized views
- Optimized for queries
- Eventually consistent with WRITE DB
- May use different storage technology (SQL, NoSQL, Column Store)

---

## Polyglot Persistence Strategy

Select storage technology based on **access patterns**, not one-size-fits-all:

| Store Type | {{ PRODUCT_NAME }} Use Cases | Example Technologies |
|------------|------------------|----------------------|
| **Row Store** | Transactional OLTP (most write models) | PostgreSQL, SQL Server |
| **NoSQL** | Flexible schema, high throughput documents/key-value | Cosmos DB, MongoDB, DynamoDB |
| **In-Memory** | Caching, transient state, session storage | Redis, Memcached |
| **Column Store** | Analytics, large scans, aggregations | Snowflake, BigQuery, Synapse |
| **Graph** | Relationship-heavy queries (networks, dependencies) | Neo4j, Cosmos DB (Gremlin API) |
| **No Index** | Append-only / log-style stores, event sourcing | Event Store, Kafka (as storage) |

**Default:** SQL (Row Store), with specialized stores as needed per domain.

---

## Analytics Data Platform

### Architecture

```
┌─────────────────────────────────────┐
│  Operational Data Sources           │
│  - Service DBs (read replicas)      │
│  - Event Hub (event streams)        │
│  - External system data feeds       │
└─────────────────────────────────────┘
              ↓
      ┌─────────────┐
      │   EL/ETL    │
      └─────────────┘
              ↓
      ┌─────────────┐
      │  DATA LAKE  │
      │  - Bronze   │ (raw)
      │  - Silver   │ (curated)
      │  - Gold     │ (modeled/aggregated)
      └─────────────┘
              ↓
      ┌─────────────┐
      │   ML/AI     │
      │  - Training │
      │  - Scoring  │
      └─────────────┘
```

### EL/ETL Layer

**Ingestion sources:**
- **Operational data** - read replicas or CDC (Change Data Capture)
- **Event streams** - Event Hub consumer for domain events
- **External datasets** - integration feeds from Systems A–D

**Transformation:**
- Raw → Curated → Modeled (Bronze → Silver → Gold pattern)
- Data quality validation
- Schema evolution handling

### Data Lake

**Storage layers:**

| Layer | Purpose | Characteristics |
|-------|---------|-----------------|
| **Bronze** | Raw ingestion | Immutable, append-only, original format |
| **Silver** | Curated | Cleaned, validated, standardized schema |
| **Gold** | Modeled | Aggregated, business-ready, optimized for consumption |

**Technologies:** Azure Data Lake Storage, S3, GCS, or equivalent

### ML/AI Consumption

**Training:**
- Batch training from Gold layer datasets
- Feature engineering pipelines
- Model versioning and registry

**Scoring:**
- Batch scoring from Lake datasets
- Real-time scoring via APIs (using trained models)
- Results published back to {{ PRODUCT_NAME }} via **events** or **APIs**

---

## Data Lifecycle Management

### Retention Policies

| Data Type | Retention | Rationale |
|-----------|-----------|-----------|
| **Hot operational data** | 90 days in primary DB | Performance, cost |
| **Warm archive** | 1-7 years in archive storage | Compliance, historical analysis |
| **Cold archive** | 7+ years or indefinite | Regulatory requirements |
| **Event streams** | 7-30 days in Event Hub | Replay capability, cost |
| **Lake Bronze** | 2+ years | Reprocessing capability |
| **Lake Silver/Gold** | Indefinite or per business rules | Analytics, ML training |

### Backup and Recovery

- **Operational DBs** - automated backups, point-in-time recovery
- **Data Lake** - versioned storage, immutable Bronze layer
- **Event Hub** - limited retention; replay from source systems or Lake if needed

---

## Data Governance

### Data Ownership

- **Domain services** own operational data
- **Data platform team** owns Lake infrastructure
- **Business domains** own Lake datasets (logical ownership)

### Data Catalog

- Metadata registry for Lake datasets
- Schema versioning and lineage tracking
- Data quality metrics

### Privacy and Compliance

- **PII handling** - encryption at rest and in transit
- **Data masking** - sensitive fields masked in non-prod environments
- **Access controls** - RBAC for Lake datasets
- **Audit logs** - all data access logged
