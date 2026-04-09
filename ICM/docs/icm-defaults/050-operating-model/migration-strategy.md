# Migration Strategy

> **Role in ICM:** Layer 3 - operating-model defaults for ownership, migration strategy, and long-term execution governance.
>
> **Used by ICM stages (example Stage CONTEXT Inputs rows):**
> | Input | Source | Stage usage |
> | --- | --- | --- |
> | Default guidance: migration-strategy | docs/icm-defaults/050-operating-model/migration-strategy.md | Referenced from a stage CONTEXT Inputs table as the baseline default for this topic. |
> | Stack adaptations (if any) | workspaces/<workspace>/stages/<stage>/references/*.md | Stage records ADR-backed deviations when implementation stack differs from defaults. |
>
> **Technology defaults (adaptable):** These defaults assume .NET/Aspire/Blazor/Entra as the reference stack. The methodology remains stack-agnostic; other stacks can adapt by documenting deviations in project ADRs.

> Status: Draft  
> Owner: Architecture Team  
> Last updated: 2026-01-22

## Purpose

This document defines the **incremental migration path** from current state to target architecture. The strategy prioritizes:
- **Risk reduction** - validate architecture with small pilots before full commitment
- **Parallel operation** - run old and new systems side-by-side during transition
- **Reversibility** - ability to roll back if migration encounters issues
- **Business continuity** - no disruption to users during migration

**Steenberg principle:** *"Build the platform first, then build test applications, then migrate real workloads. Prove each layer works before depending on it."*

---

## Migration Philosophy

### Incremental Adoption, Not Big Bang

**Avoid:** Rewrite everything, deploy all at once, hope it works.

**Instead:**
1. Build **platform foundation** (abstractions, infrastructure)
2. Create **reference implementation** (one pilot service using new architecture)
3. Validate with **production traffic** (small percentage, feature flag controlled)
4. **Gradually migrate** services and features
5. Run **old and new in parallel** until proven
6. **Decommission old** only when new is validated

### Glue Code Is Temporary Scaffolding

During migration, write **translation layers** between old and new:
- Old service calls → new service APIs
- New service calls → old service APIs
- Old events → new event format
- New events → old event format

**Plan to delete glue code** once migration completes. Don't optimize it; it's temporary.

---

## Current State Assessment

### Existing Architecture (Assumed)

**Based on typical legacy patterns:**
- Monolithic application or loosely-coupled services
- Direct database access across modules
- Synchronous API calls without event-driven patterns
- Tightly coupled to specific cloud services (Azure)
- Limited observability (application logs, basic metrics)
- Manual deployment processes

**Assets to preserve:**
- Domain knowledge embedded in existing code
- Existing APIs consumed by external clients
- Data in production databases
- User workflows and UX

**Liabilities to eliminate:**
- Tight coupling between modules
- Direct dependency on Azure SDKs
- Lack of service boundaries and contracts
- No systematic event streaming
- Limited testing (especially integration/contract tests)

---

## Migration Phases

## Phase 1: Foundation (Months 1-3)

**Goal:** Build platform abstractions and prove they work with pilot service.

### 1.1 Build Platform Abstraction Layer

**Deliverables:**
- `{{ PRODUCT_NAME }}.Platform.Messaging` package
  - `IEventPublisher`, `IEventConsumer`, `ICommandQueue` interfaces
  - Azure Event Hub/Service Bus implementations
  - In-memory fake implementations for testing
- `{{ PRODUCT_NAME }}.Platform.Identity` package
  - `IIdentityProvider` interface
  - Entra ID implementation
  - Fake implementation for testing
- `{{ PRODUCT_NAME }}.Platform.AI` package
  - `ILargeLanguageModel` interface
  - OpenAI implementation
  - Mock implementation for testing
- `{{ PRODUCT_NAME }}.Platform.Storage` package
  - `IRepository<T>`, `IUnitOfWork` interfaces
  - SQL Server implementation
  - In-memory implementation for testing
- `{{ PRODUCT_NAME }}.Platform.Caching` package
  - `IDistributedCache` interface
  - Redis implementation
  - In-memory implementation for testing

**Success criteria:**
- All platform packages published to internal NuGet feed
- All packages have unit tests (90%+ coverage)
- All packages have integration tests against real infrastructure
- Documentation published (README, API docs)

**Timeline:** 4 weeks

---

### 1.2 Define Domain Primitives

**Deliverables:**
- `{{ PRODUCT_NAME }}.Domain.Primitives` package
  - `FacilityAsset`, `AssessmentEvent`, `WorkOrder`, `FacilityMetrics` records
  - `DomainEvent` base class
  - Validation rules
- Primitive documentation (see `domain-primitives.md`)

**Success criteria:**
- Primitives reviewed and approved by domain experts
- All primitives have validation tests
- Primitives serializable to/from JSON (for events and APIs)

**Timeline:** 2 weeks

---

### 1.3 Create Service Template

**Deliverables:**
- Service template project (`dotnet new {{ PRODUCT_NAME }}-service`)
  - Aspire integration for local development
  - Platform abstraction references
  - OTEL instrumentation configured
  - Health checks, readiness probes
  - Dockerfile and CI/CD pipeline
  - Contract test structure
  - README with getting-started guide

**Success criteria:**
- Developer can scaffold new service in < 30 minutes
- Template service passes all architecture tests
- Template deployable to local Docker Compose and production

**Timeline:** 2 weeks

---

### 1.4 Build Pilot Service (Asset Service)

**Deliverables:**
- **Asset Service** using new architecture
  - REST API (`/api/v1/assets`) using platform abstractions
  - CQRS pattern (read/write separation)
  - Event publishing (`AssetConditionChanged`, `AssetCreated`)
  - OpenAPI specification
  - Contract tests (Pact)
  - OTEL instrumentation

**Pilot scope:**
- Implement subset of Asset functionality (CRUD + condition updates)
- Deploy to **staging environment only** initially
- Test with synthetic traffic

**Success criteria:**
- Pilot service handles 1000 req/sec in staging
- p95 latency < 200ms
- Events published successfully to Event Hub
- OTEL traces visible in observability platform
- Zero direct Azure SDK references (only platform abstractions)

**Timeline:** 4 weeks

**Risk mitigation:**
- Keep existing Asset system running in production
- Pilot service is **read-only shadow** initially (publishes events but doesn't update production data)
- Gradual traffic shifting (1% → 10% → 50% → 100%)

---

## Phase 2: Parallel Operation (Months 4-9)

**Goal:** Run new architecture services alongside legacy, migrate traffic gradually, build confidence.

### 2.1 Glue Code Layer (Old ↔ New Translation)

**Deliverables:**
- **Event Bridge** service
  - Subscribes to old system events (if any)
  - Translates to new `DomainEvent` format
  - Publishes to Event Hub
  - Reverse translation (Event Hub → old system events)
- **API Proxy** layer
  - Routes requests to old or new services based on feature flag
  - Translates responses if formats differ
- **Data Sync** jobs
  - Keep old database and new service databases in sync
  - Bidirectional sync during parallel operation

**Example:**

```csharp
public class AssetEventBridge
{
    private readonly ILegacyAssetEventSource _legacyEvents;
    private readonly IEventPublisher _newEventBus;
    
    public async Task BridgeAsync(CancellationToken ct)
    {
        await foreach (var legacyEvent in _legacyEvents.StreamAsync(ct))
        {
            // Translate legacy event to new DomainEvent
            var domainEvent = TranslateAssetEvent(legacyEvent);
            
            // Publish to Event Hub
            await _newEventBus.PublishAsync(domainEvent, ct);
            
            _logger.LogInformation("Bridged event {LegacyId} to {EventId}", 
                legacyEvent.Id, domainEvent.EventId);
        }
    }
    
    private DomainEvent TranslateAssetEvent(LegacyAssetEvent legacy)
    {
        // Translation logic...
    }
}
```

**Success criteria:**
- Event bridge processes 10,000 events/sec with < 100ms latency
- Data sync maintains < 1 second lag between old and new databases
- API proxy routes requests with < 10ms overhead

**Timeline:** 4 weeks

---

### 2.2 Migrate High-Value Services

**Services to migrate (in priority order):**
1. **Asset Service** (already piloted, expand to full functionality)
2. **Assessment Service** (depends on Asset Service)
3. **Work Order Service** (depends on Asset Service)
4. **Analytics Service** (consumes events from all domain services)
5. **MCP Service** (AI integration)

**Per-service migration process:**
1. **Build new service** using template (2 weeks)
2. **Deploy to staging** with glue code integration (1 week)
3. **Run shadow mode** (new service processes requests but doesn't affect users, 1 week)
4. **Canary rollout** (1% → 10% → 25% → 50% → 100% traffic, 4 weeks)
5. **Validate metrics** (SLOs met, no error rate increase)
6. **Decommission glue code** (once 100% traffic on new service)

**Feature flag pattern:**

```csharp
public class AssetApiController
{
    private readonly IFeatureFlag _flags;
    private readonly IAssetService _newService;
    private readonly ILegacyAssetService _legacyService;
    
    public async Task<AssetDto> GetAssetAsync(string id)
    {
        if (await _flags.IsEnabledAsync("use-new-asset-service"))
        {
            return await _newService.GetAssetAsync(id);
        }
        else
        {
            return await _legacyService.GetAssetAsync(id);
        }
    }
}
```

**Success criteria per service:**
- New service handles 100% production traffic
- SLOs maintained or improved
- No increase in error rate or customer complaints
- OTEL observability fully operational
- Contract tests passing
- Legacy service decommissioned

**Timeline:** 20 weeks (5 services × 4 weeks avg, with 2 weeks overlap)

---

### 2.3 Migrate Event Streaming Backbone

**Current state:** Limited or no event-driven patterns.

**Target state:** Event Hub as primary integration mechanism.

**Migration steps:**
1. **Deploy Event Hub** infrastructure
2. **Instrument new services** to publish events (already done in Phase 2.2)
3. **Build read model updaters** (consume events, update CQRS read databases)
4. **Build analytics ingestion** (Event Hub → Data Lake pipeline)
5. **Add event-driven functions** (notifications, automation, enrichment)

**Example read model updater:**

```csharp
public class AssetReadModelUpdater : BackgroundService
{
    private readonly IEventConsumer _consumer;
    private readonly IAssetReadRepository _readRepo;
    
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await _consumer.SubscribeAsync<AssetConditionChanged>(
            async (@event, ctx) =>
            {
                var asset = await _readRepo.GetByIdAsync(@event.AssetId);
                asset.Condition = @event.NewScore;
                asset.LastUpdated = @event.Timestamp;
                await _readRepo.UpdateAsync(asset);
            },
            new ConsumerOptions("read-model-updater", MaxConcurrency: 10, 
                RetryDelay: TimeSpan.FromSeconds(5), MaxRetries: 3),
            ct
        );
    }
}
```

**Success criteria:**
- Event Hub processes 50,000 events/sec
- Read models updated within 1 second of write
- Data Lake ingestion lag < 5 minutes
- Event-driven functions executing successfully

**Timeline:** 6 weeks

---

### 2.4 Migrate External Integrations

**Current state:** Direct API calls to external systems from domain services.

**Target state:** Integration adapters isolate external dependencies.

**Migration steps:**
1. **Build Integration Adapters** (one per external system)
   - System A Adapter (procurement)
   - System B Adapter (HR)
   - System C Adapter (finance)
   - System D Adapter (IoT sensors)
2. **Deploy Service Bus** infrastructure
3. **Refactor domain services** to use adapters (not direct external calls)
4. **Add DLQ monitoring** and remediation workflows
5. **Implement circuit breakers** for external system resilience

**Adapter example:**

```csharp
public class ProcurementSystemAdapter
{
    private readonly ICommandQueue _serviceBus;
    private readonly ProcurementSystemClient _externalClient;
    
    public async Task SendPurchaseOrderAsync(WorkOrder workOrder)
    {
        // Translate {{ PRODUCT_NAME }} WorkOrder → external system PurchaseOrder
        var purchaseOrder = new ExternalPurchaseOrder
        {
            OrderId = workOrder.WorkOrderId,
            Amount = workOrder.EstimatedCost,
            // ...
        };
        
        try
        {
            await _externalClient.CreatePurchaseOrderAsync(purchaseOrder);
            
            await _serviceBus.SendAsync(new PurchaseOrderCreated
            {
                WorkOrderId = workOrder.WorkOrderId,
                ExternalOrderId = purchaseOrder.OrderId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send purchase order to external system");
            // Message goes to DLQ for remediation
            throw;
        }
    }
}
```

**Success criteria:**
- All external systems integrated via adapters
- No direct external API calls from domain services
- DLQ remediation workflow operational
- Circuit breakers prevent cascade failures

**Timeline:** 8 weeks

---

## Phase 3: Consolidation (Months 10-18)

**Goal:** Complete migration, remove glue code, optimize based on lessons learned.

### 3.1 Decommission Legacy Systems

**Per legacy system:**
1. **Verify 100% traffic on new services** (via feature flags and metrics)
2. **Export legacy data** for archival
3. **Shut down legacy service**
4. **Delete legacy database** (after backup retention period)
5. **Remove glue code and data sync jobs**
6. **Update documentation** (remove legacy references)

**Success criteria:**
- All legacy services decommissioned
- No glue code remaining in production
- Cost savings realized (fewer VMs, databases)

**Timeline:** 8 weeks

---

### 3.2 Optimize Platform Abstractions

**Lessons learned review:**
- Which abstractions were painful to use?
- Which abstractions leaked implementation details?
- Where did teams build workarounds?

**Refactorings:**
- Simplify complex interfaces
- Add missing capabilities discovered during migration
- Improve performance (caching, batching)
- Enhance testing fakes (better feature parity)

**Success criteria:**
- Developer satisfaction survey (target: 4/5 or higher)
- Reduced platform support tickets
- Faster new service development (< 4 hours to scaffold)

**Timeline:** 6 weeks

---

### 3.3 Implement Advanced Patterns

**Now that foundation is stable, add advanced capabilities:**
- **Saga orchestration** for complex workflows
- **Event sourcing** for select services (if valuable)
- **GraphQL gateway** for flexible client queries
- **Service mesh** for advanced observability and resilience

**Success criteria:**
- Advanced patterns documented with reference implementations
- Teams can adopt patterns independently (no coordination required)

**Timeline:** 12 weeks (ongoing)

---

## Risk Management

### Risk 1: Performance Degradation During Migration

**Mitigation:**
- Load test new services before production rollout
- Canary deployments with automatic rollback
- Monitor SLOs continuously, alert on degradation
- Feature flags allow instant rollback to legacy

**Rollback plan:**
- Disable feature flag → traffic routes to legacy
- Investigate performance issue in staging
- Fix and retest before re-enabling

---

### Risk 2: Data Inconsistency Between Old and New

**Mitigation:**
- Bidirectional data sync during parallel operation
- Consistency checks (compare old vs. new database)
- Reconciliation jobs to fix discrepancies
- Read-only shadow mode before live writes

**Detection:**
- Daily consistency reports
- Alert if discrepancy rate > 0.1%
- Manual review of inconsistencies before 100% cutover

---

### Risk 3: Event Ordering and Duplicate Events

**Mitigation:**
- Use partition keys (e.g., AssetId) for ordered delivery
- Idempotent event handlers (process duplicate safely)
- Event deduplication in consumers (track processed event IDs)
- Integration tests for event ordering scenarios

**Validation:**
- Chaos testing: inject duplicate and out-of-order events
- Verify system remains consistent

---

### Risk 4: Breaking Changes to External Consumers

**Mitigation:**
- Maintain API contract compatibility (see `interface-governance.md`)
- Run consumer contract tests in CI/CD
- Deprecation warnings before removing v1 APIs
- Coordinated migration with external teams

**Communication:**
- Email external consumers 6 months before deprecation
- Provide migration guides and support
- Monitor v1 API usage, contact stragglers

---

### Risk 5: Team Skill Gaps

**Mitigation:**
- Training on new architecture (workshops, docs)
- Reference implementations as learning materials
- Pair programming during early migrations
- Platform Team office hours for support

**Success indicators:**
- Team velocity maintained or improved
- Code review quality remains high
- Reduced escalations to Architecture Team

---

## Validation and Success Metrics

### Phase 1 Success Criteria

- [ ] Platform abstractions published and tested
- [ ] Domain primitives defined and validated
- [ ] Service template available
- [ ] Pilot service handling production traffic (even if small %)
- [ ] OTEL observability operational
- [ ] Zero critical incidents caused by new architecture

---

### Phase 2 Success Criteria

- [ ] All domain services migrated (Asset, Assessment, Work Order, Analytics, MCP)
- [ ] Event Hub processing 50,000+ events/sec
- [ ] External systems integrated via adapters
- [ ] Legacy and new systems running in parallel successfully
- [ ] SLOs met or improved vs. legacy
- [ ] Developer velocity maintained (cycle time for new features)

---

### Phase 3 Success Criteria

- [ ] All legacy systems decommissioned
- [ ] Glue code removed
- [ ] Platform abstractions optimized based on lessons learned
- [ ] Advanced patterns available (saga, event sourcing, etc.)
- [ ] Cost savings realized (fewer resources, reduced maintenance)
- [ ] Architecture tests enforcing boundaries (100% pass rate)

---

### Overall Success Metrics (End of Month 18)

| Metric | Target |
|--------|--------|
| **Service uptime** | 99.9% |
| **API latency (p95)** | < 200ms |
| **Event processing lag** | < 1 second |
| **Data Lake ingestion lag** | < 5 minutes |
| **Developer onboarding time** | < 1 week |
| **New service scaffold time** | < 4 hours |
| **Platform breaking changes** | < 2 per year |
| **Contract test coverage** | 100% of service integrations |
| **Incident MTTR** | < 1 hour |
| **Architecture test pass rate** | 100% |

---

## Rollback Plans

### Scenario: Phase 1 Pilot Fails

**Trigger:** Pilot service shows critical bugs, performance issues, or architecture flaws.

**Action:**
1. Disable pilot service (route traffic back to legacy)
2. Conduct post-mortem (root cause analysis)
3. Fix platform abstractions or service design
4. Retest in staging
5. Restart pilot with fixes

**Impact:** Delay Phase 1 by 2-4 weeks; no production user impact.

---

### Scenario: Phase 2 Service Migration Causes Outage

**Trigger:** New service deployment causes SLO violation or customer impact.

**Action:**
1. Instant rollback via feature flag (disable new service)
2. Incident response process (mitigate user impact)
3. Post-mortem and fix
4. Retest in staging with increased canary period
5. Retry migration

**Impact:** Temporary SLO violation (< 1 hour); delay migration by 1-2 sprints.

---

### Scenario: Event Hub Overload

**Trigger:** Event Hub cannot handle production load (throttling, lag).

**Action:**
1. Scale Event Hub partitions and throughput units
2. Optimize event payloads (reduce size)
3. Add backpressure handling in publishers
4. If critical: temporarily disable event publishing for non-critical events

**Impact:** Temporary event processing lag; address with capacity planning.

---

## Communication Plan

### Internal Stakeholders

**Engineering Teams:**
- Weekly migration status updates (Slack + email)
- Biweekly architecture review meetings
- Migration playbook and runbooks published

**Product/Business:**
- Monthly executive summary (progress, risks, milestones)
- Feature release coordination (migration does not block features)
- Cost/benefit tracking (savings from decommissioned systems)

---

### External Stakeholders

**API Consumers:**
- 6-month advance notice for breaking changes
- Migration guides published
- Sandbox environment for testing
- Direct support channel for migration questions

**External System Owners:**
- Coordination on integration adapter development
- Testing support (validate adapters against external systems)
- Incident escalation contacts

---

## Related Documentation

- [domain-primitives.md](domain-primitives.md) - primitives to implement
- [025-platform-abstraction-layer.md](025-platform-abstraction-layer.md) - platform to build first
- [interface-governance.md](interface-governance.md) - contract stability during migration
- [095-team-ownership.md](095-team-ownership.md) - who migrates what
- `../02-decision-records/` - ADRs documenting migration decisions
- `../07-operations/` - operational runbooks for new architecture
