# Team Ownership Model

> **Role in ICM:** Layer 3 - operating-model defaults for ownership, migration strategy, and long-term execution governance.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: team-ownership | docs/icm-defaults/050-operating-model/team-ownership.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Purpose

The Team Ownership Model defines **who owns what** in the {{ PRODUCT_NAME }} architecture to:
- Minimize coordination overhead between teams
- Enable parallel development without conflicts
- Establish clear accountability for components
- Limit cognitive load per developer (one person can understand their module)
- Prevent "tragedy of the commons" (nobody owns shared code)

**Steenberg principle:** *"Maximum one person per module. If two people need to coordinate on the same code, your boundaries are wrong."*

---

## Ownership Principles

### 1. Clear Module Boundaries

Each team owns **complete modules** with well-defined interfaces:
- Team has **full autonomy** over internal implementation
- Other teams can only interact through **published interfaces**
- No shared code ownership between teams
- No cross-team approval required for internal changes

### 2. Team Size Limits

**Cognitive load constraint:**
- One developer should understand **entire module** (code, tests, deployment, monitoring)
- Team of 2-3 developers can maintain **2-3 related services maximum**
- If team needs > 4 people, split the module

### 3. Conway's Law Awareness

*"Organizations design systems that mirror their communication structure."*

**Design team structure to match desired architecture:**
- Want loosely-coupled microservices? → Loosely-coupled teams
- Want shared platform abstractions? → Dedicated platform team
- Want rapid domain service evolution? → Autonomous domain teams

---

## Team Structure

### Platform Team

**Size:** 2-3 developers

**Owns:**
- Platform Abstraction Layer interfaces and implementations
  - `{{ PRODUCT_NAME }}.Platform.Messaging` (Event Hub/Service Bus wrappers)
  - `{{ PRODUCT_NAME }}.Platform.Identity` (Entra ID wrapper)
  - `{{ PRODUCT_NAME }}.Platform.AI` (OpenAI wrapper)
  - `{{ PRODUCT_NAME }}.Platform.Storage` (Repository abstractions)
  - `{{ PRODUCT_NAME }}.Platform.Caching` (Redis wrapper)
- Cross-cutting infrastructure
  - OpenTelemetry (OTEL) configuration and collector deployment
  - API Gateway (APIM) shared policies and configuration
  - Shared NuGet package publishing
- Developer experience
  - Service templates and scaffolding tools
  - Local development environment (Aspire composition)
  - CI/CD pipeline templates

**Responsibilities:**
- Maintain stable platform interfaces (breaking changes require RFC)
- Publish platform packages with semantic versioning
- Provide fake implementations for testing
- Quarterly replaceability tests (prove abstractions work)
- Support domain teams with platform issues

**Success metrics:**
- Domain teams can build new service in < 1 day using templates
- Platform interface breaking changes < 2 per year
- 95% of platform issues resolved within 1 sprint

**Communication:**
- Weekly platform office hours for domain team questions
- RFC process for platform interface changes
- Platform changelog published with each release

---

### Asset Domain Team

**Size:** 2-3 developers

**Owns:**
- **Asset Service**
  - Asset CRUD APIs (`/api/v1/assets`)
  - Asset read models (CQRS read side)
  - Asset write models (CQRS write side)
  - Asset event publishers (`AssetConditionChanged`, etc.)
- **Asset Database**
  - Asset write store (SQL Server)
  - Asset read store (SQL Server or NoSQL)
  - Database schema migrations
- **Asset Contracts**
  - OpenAPI specifications
  - Event schemas
  - Consumer contracts (Pact tests)

**Responsibilities:**
- Maintain Asset API SLOs (99.9% uptime, p95 < 200ms)
- Respond to Asset-related incidents
- Evolve Asset domain model based on business needs
- Ensure Asset events are backward-compatible or versioned
- Keep Asset read models up-to-date with write side events

**Autonomy:**
- Can change internal database schema without external approval
- Can refactor Asset domain logic freely
- Can switch Asset read store technology (SQL → NoSQL) independently
- **CANNOT** break published Asset API or event contracts

**Dependencies:**
- Platform Team: messaging, identity, storage abstractions
- Assessment Team: consumes `AssetConditionChanged` events
- Work Order Team: consumes Asset APIs for asset lookups

---

### Assessment Domain Team

**Size:** 2-3 developers

**Owns:**
- **Assessment Service**
  - Assessment CRUD APIs (`/api/v1/assessments`)
  - Checklist and scoring logic
  - Assessment event publishers (`AssessmentCompleted`, `DeficiencyIdentified`)
- **Assessment Database**
  - Assessment data store (SQL Server)
  - Assessment history and audit trail
- **Assessment Contracts**
  - OpenAPI specifications
  - Event schemas

**Responsibilities:**
- Maintain Assessment API SLOs
- Implement condition scoring business rules
- Integrate with Asset Service (publish condition changes)
- Support offline assessment scenarios (client queue/sync)

**Autonomy:**
- Full control over scoring algorithms
- Can change assessment data model independently
- Can add new checklist types without coordination

**Dependencies:**
- Platform Team: messaging, identity abstractions
- Asset Team: calls Asset API to update asset condition
- Reporting Team: provides assessment data for analytics

---

### Work Order Domain Team

**Size:** 2-3 developers

**Owns:**
- **Work Order Service**
  - Work Order CRUD APIs (`/api/v1/work-orders`)
  - Work Order scheduling and assignment logic
  - Work Order lifecycle events (`WorkOrderCreated`, `WorkOrderCompleted`)
- **Work Order Database**
  - Work Order store (SQL Server)
  - Work Order scheduling and resource allocation tables
- **Work Order Contracts**
  - OpenAPI specifications
  - Event schemas

**Responsibilities:**
- Maintain Work Order API SLOs
- Implement scheduling and prioritization logic
- Calculate deferred maintenance costs
- Integrate with external procurement systems (via Integration Team adapters)

**Autonomy:**
- Full control over work order prioritization algorithms
- Can change scheduling logic independently
- Can add work order types without coordination

**Dependencies:**
- Platform Team: messaging, storage abstractions
- Asset Team: calls Asset API to get asset details
- Integration Team: uses adapters for external system integration

---

### Integration Team

**Size:** 2 developers

**Owns:**
- **External System Adapters**
  - System A Adapter (procurement system)
  - System B Adapter (HR system)
  - System C Adapter (finance system)
  - System D Adapter (IoT sensor platform)
- **Adapter Infrastructure**
  - Service Bus topic/queue configuration for external integrations
  - Dead Letter Queue monitoring and remediation workflows
  - Circuit breakers and retry policies for external calls
- **Integration Contracts**
  - Translation logic (external formats ↔ {{ PRODUCT_NAME }} primitives)
  - Integration event schemas

**Responsibilities:**
- Isolate {{ PRODUCT_NAME }} from external system changes
- Monitor integration health and error rates
- Handle DLQ remediation (replay failed messages)
- Coordinate with external system owners

**Autonomy:**
- Can change adapter implementations without affecting domain services
- Can add new external systems independently
- Can switch integration patterns (API polling → webhooks) internally

**Dependencies:**
- Platform Team: messaging abstractions (Service Bus wrapper)
- Domain Teams: translate external data to {{ PRODUCT_NAME }} primitives

**Special rule:** Integration Team is the **only team** allowed to call external system APIs directly. Domain teams must use adapters.

---

### Analytics Team

**Size:** 2-3 developers

**Owns:**
- **Data Lake Pipeline**
  - EL/ETL jobs (Event Hub → Data Lake)
  - Data Lake storage layers (bronze/silver/gold)
  - Data quality and validation rules
- **Analytics Services**
  - Facility Metrics Service (FCI calculations, trend analysis)
  - Reporting API (`/api/v1/reports`)
  - Dashboard data aggregators
- **ML/AI Pipelines**
  - RAG pipeline (embeddings, vector store, retrieval)
  - Predictive maintenance models
  - Model training and deployment workflows

**Responsibilities:**
- Maintain Data Lake SLAs (data freshness, quality)
- Provide analytics APIs for reporting and dashboards
- Support AI/MCP with RAG context retrieval
- Ensure data governance and privacy compliance

**Autonomy:**
- Can change Data Lake technology (Azure Data Lake → S3 + Databricks) independently
- Can switch ML frameworks without affecting consumers
- Can add new analytics models freely

**Dependencies:**
- Platform Team: messaging abstraction (Event Hub consumer)
- Domain Teams: consume domain events for analytics
- AI Team: provides data for RAG pipelines

---

### AI/MCP Team

**Size:** 2 developers

**Owns:**
- **MCP Service**
  - MCP tool registry and execution engine
  - MCP security and audit logging
  - MCP API (`/api/v1/mcp`)
- **AI Integration**
  - OpenAI API wrapper (via Platform abstraction)
  - Prompt engineering and prompt governance
  - AI call monitoring and cost tracking
- **RAG Infrastructure**
  - Vector store integration
  - Embedding generation
  - Context retrieval logic

**Responsibilities:**
- Maintain MCP SLOs (tool execution success rate, latency)
- Govern AI data access (least privilege, PII handling)
- Audit AI tool usage for compliance
- Optimize AI costs (caching, prompt tuning)

**Autonomy:**
- Can change LLM provider (OpenAI → Claude) via Platform abstraction
- Can modify prompt strategies independently
- Can add new MCP tools without coordination (if using existing APIs)

**Dependencies:**
- Platform Team: AI abstraction (LLM wrapper)
- Analytics Team: RAG data from Data Lake
- Domain Teams: MCP calls domain APIs as "tools"

**Special rule:** AI Team is the **only team** allowed to call LLM APIs directly. Domain teams must use MCP tools.

---

### Reporting/Dashboard Team

**Size:** 1-2 developers

**Owns:**
- **Reporting Service**
  - Report generation API (`/api/v1/reports`)
  - Report scheduling and delivery
  - Report templates and customization
- **Dashboard Backend**
  - Dashboard data aggregation
  - Real-time metrics calculation
  - Dashboard API (`/api/v1/dashboards`)

**Responsibilities:**
- Maintain Reporting SLOs (report generation time, availability)
- Support operational and compliance reporting needs
- Provide data for executive dashboards

**Autonomy:**
- Can change report formats independently
- Can add new report types without coordination
- Can optimize report queries freely

**Dependencies:**
- Domain Teams: calls read model APIs for operational data
- Analytics Team: calls analytics APIs for curated/aggregated data

---

## Cross-Team Coordination Patterns

### Pattern 1: RFC Process (Platform Interface Changes)

**When:** Platform Team needs to change platform abstraction interface (breaking change)

**Process:**
1. Platform Team writes RFC (Request for Comments) document
2. RFC posted in shared docs repo + Slack notification
3. Domain teams have 1 week to review and comment
4. RFC discussion meeting (all affected teams)
5. RFC approved/rejected by Architecture Team
6. If approved: Platform Team implements, publishes migration guide
7. Domain teams migrate within agreed timeline (3-6 months)

**Example RFC:**
```markdown
# RFC-001: Add Batch Publishing to IEventPublisher

## Problem
Services frequently publish 10-100 events at once. Current API requires
individual calls, causing performance issues.

## Proposed Change
Add method to IEventPublisher:
```csharp
Task PublishBatchAsync<TEvent>(IEnumerable<TEvent> events, CancellationToken ct);
```

## Impact
- **Breaking?** No (additive change)
- **Affected Teams:** All domain teams using event publishing
- **Migration Effort:** Optional (teams can adopt when beneficial)

## Timeline
- Implementation: Sprint 15
- Release: v2.3.0 of {{ PRODUCT_NAME }}.Platform.Messaging
```

---

### Pattern 2: Event Contract Changes

**When:** Domain team needs to change event schema

**Process:**
1. Domain team proposes change in contract registry PR
2. Automated check identifies breaking vs. non-breaking
3. If **non-breaking** (new optional field): auto-approve, merge
4. If **breaking** (remove field, change type):
   - Domain team creates new schema version (v2)
   - Domain team publishes both versions for migration period
   - Consumer teams update to handle both versions
   - After 6 months, v1 can be deprecated

**Example:**
- Asset Team wants to change `AssetConditionChanged` event
- Breaking: remove `oldScore` field
- Action: Create `AssetConditionChanged` v2 schema, keep v1
- Consumers update handlers to support both versions
- After 6 months, Asset Service stops publishing v1

---

### Pattern 3: Cross-Service Feature Development

**When:** Feature requires changes in multiple services

**Process:**
1. Feature owner creates epic with sub-tasks per service
2. Each domain team implements their part independently
3. Integration tested via contract tests
4. Feature flag controls rollout across services

**Example Feature:** "Predictive Maintenance Alerts"
- **Asset Team:** Add `predictedFailureDate` field to Asset API (non-breaking)
- **Analytics Team:** Build ML model, publish predictions via event
- **Work Order Team:** Subscribe to prediction events, auto-create work orders
- **MCP Team:** Add tool to query predictions

**Coordination:** Async via events; no cross-team code dependencies

---

## Ownership Boundaries (Architecture Tests)

Enforce ownership with automated tests:

```csharp
[Fact]
public void AssetService_CannotReference_WorkOrderService()
{
    var assetAssembly = typeof(AssetService).Assembly;
    var workOrderTypes = typeof(WorkOrderService).Assembly.GetTypes();
    
    assetAssembly.Should().NotReference(workOrderTypes);
}

[Fact]
public void DomainServices_CannotReference_AzureSDK()
{
    var domainAssemblies = new[]
    {
        typeof(AssetService).Assembly,
        typeof(AssessmentService).Assembly,
        typeof(WorkOrderService).Assembly
    };
    
    domainAssemblies.Should().NotReferenceAny(
        "Azure.Messaging.EventHubs",
        "Azure.Identity",
        "OpenAI"
    );
}

[Fact]
public void OnlyIntegrationTeam_CanReference_ExternalSystemClients()
{
    var domainAssemblies = GetDomainServiceAssemblies();
    
    domainAssemblies.Should().NotReferenceAny(
        "ExternalSystem.A.Client",
        "ExternalSystem.B.SDK"
    );
    
    // Only integration service allowed
    typeof(SystemAAdapter).Assembly.Should().Reference("ExternalSystem.A.Client");
}
```

---

## Team Metrics and Accountability

### Platform Team Metrics
- **Platform stability:** Breaking changes per quarter (target: < 2)
- **Developer velocity:** Time to scaffold new service (target: < 4 hours)
- **Support responsiveness:** Platform issue resolution time (target: < 1 sprint)

### Domain Team Metrics
- **Service uptime:** SLO adherence (target: 99.9%)
- **API latency:** p95 response time (target: < 200ms)
- **Incident response:** MTTR (Mean Time To Recovery, target: < 1 hour)
- **Contract stability:** Breaking changes per quarter (target: 0)

### Integration Team Metrics
- **Integration health:** External system connection success rate (target: 99.5%)
- **DLQ remediation:** Dead letter message processing time (target: < 24 hours)
- **Adapter coverage:** % external systems with adapters (target: 100%)

### Analytics Team Metrics
- **Data freshness:** Event-to-lake latency (target: < 5 minutes)
- **Data quality:** Failed validation rate (target: < 0.1%)
- **RAG performance:** Context retrieval latency (target: < 500ms)

---

## Escalation and Conflict Resolution

### Scenario: Team Boundary Dispute

**Example:** Asset Team and Work Order Team both claim ownership of "deficiency" entity.

**Resolution process:**
1. Teams present cases to Architecture Team
2. Architecture Team analyzes:
   - Which team's **core domain** includes deficiencies?
   - Which **primitive** does deficiency belong to?
   - What **interface** should expose deficiencies?
3. Architecture Team decides ownership
4. Losing team refactors to use winner's API
5. Document decision as ADR

### Scenario: Platform Change Blocking Domain Team

**Example:** Domain team needs platform feature, but Platform Team lacks capacity.

**Resolution process:**
1. Domain team submits RFC for platform feature
2. Platform Team assesses priority and effort
3. If high priority + low effort: Platform Team implements
4. If high priority + high effort: Architecture Team allocates resources
5. If low priority: Domain team builds workaround or waits for backlog

---

## Cognitive Load Management

### Team Capacity Assessment

**Before adding new service to team, verify:**
- [ ] Can one developer understand entire service codebase in < 1 week?
- [ ] Can team respond to production incident without external help?
- [ ] Can team deploy service independently (no cross-team approvals)?
- [ ] Does service have < 5 external dependencies?
- [ ] Does team have < 3 services total?

**If any answer is "no":** Split the service or create new team.

### Onboarding Targets

**New developer should be able to:**
- Deploy service locally within **1 day**
- Merge first PR within **3 days**
- Understand service domain and interfaces within **1 week**
- Respond to production incident within **2 weeks**

**If targets not met:** Service complexity too high; refactor or split.

---

## Team Communication

### Synchronous Communication (Meetings)

**Platform Office Hours:** Weekly, 1 hour
- Domain teams ask platform questions
- Platform team demos new features
- Discussion of upcoming RFCs

**Architecture Review:** Biweekly, 1 hour
- Review cross-cutting design decisions
- Approve RFCs
- Discuss team boundary issues

**Domain Team Standups:** Daily, 15 min
- Internal to each team
- No cross-team attendance required

### Asynchronous Communication (Documentation)

**Slack channels:**
- `#platform-team` - Platform Team support
- `#architecture-discuss` - Cross-team architecture questions
- `#rfc-review` - RFC notifications and discussion

**Documentation:**
- Contract registry - API and event contracts
- ADRs - Major architectural decisions
- Runbooks - Operational procedures per service

---

## Related Documentation

- [domain-primitives.md](domain-primitives.md) - what teams exchange via interfaces
- [025-platform-abstraction-layer.md](025-platform-abstraction-layer.md) - Platform Team's interfaces
- [interface-governance.md](interface-governance.md) - how teams evolve contracts
- [100-migration-strategy.md](100-migration-strategy.md) - how teams adopt new architecture
- `../02-decision-records/GOVERNANCE.md` - RFC and ADR processes
