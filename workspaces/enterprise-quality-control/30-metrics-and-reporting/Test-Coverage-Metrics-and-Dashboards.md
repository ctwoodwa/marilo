# Test Coverage Metrics and Dashboards

> **Purpose:** Define code coverage metrics, CI integration, and dashboard specifications for driving test coverage improvements.

> Status: Draft
> Owner: Architecture Team
> Last updated: 2026-03-21

## Coverage Metrics

### By Module and Layer

Track coverage broken down by architecture layer within each module:

| Layer | What to measure | Minimum target | Stretch target |
|-------|----------------|----------------|----------------|
| **State** | State action handlers, derivations, RouteState | 80% lines | 90% lines |
| **Service** | Application service orchestration logic | 80% lines | 90% lines |
| **Persistence (CQRS)** | Request handlers, parameter building, mapping | 75% lines | 85% lines |
| **UI Components** | Component rendering, interaction handlers | 60% lines | 75% lines |
| **Shared/Infrastructure** | Middleware, utilities, configuration | 70% lines | 80% lines |

### Hot Path Coverage

Identify and track coverage for critical code paths:

- **ADR-governed features:** Features implementing decisions from ADR-0024 (State vs Service), ADR-0007 (Navigation), ADR-0023 (Eventing).
- **High-risk operations:** Create, Change, Delete CQRS handlers.
- **Integration boundaries:** API controllers, event publishers/subscribers.
- **Business logic:** Domain primitives, validation rules, business rule evaluators.

### Diff-Based Coverage (Per PR)

| Metric | Description | Enforcement |
|--------|-------------|-------------|
| New lines covered | Percentage of new lines with test coverage | CI gate: block merge if < threshold |
| Changed lines covered | Percentage of modified lines with test coverage | CI gate: warn if < threshold |
| Deleted lines impact | Tests covering deleted code are removed or updated | Manual review |

---

## CI Integration

### Coverage Tool Setup

```text
Recommended tools:
- Coverlet (cross-platform .NET coverage)
- dotnet-coverage (Microsoft coverage collector)
- ReportGenerator (HTML/Cobertura report generation)

Pipeline integration:
1. Run tests with coverage collection enabled.
2. Generate Cobertura XML report.
3. Publish report as build artifact.
4. Parse report for threshold enforcement.
5. Post coverage summary to PR comment.
```

### Threshold Enforcement

| Gate | Threshold | Action |
|------|-----------|--------|
| PR merge gate | Diff coverage >= project threshold | Block merge |
| Sprint release gate | Overall coverage >= baseline (no regression) | Warn / block release |
| Module health gate | Module coverage >= layer-specific target | Warn in dashboard |

### Pipeline Steps

```text
1. dotnet test --collect:"XPlat Code Coverage"
2. reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage-report
3. Parse coverage-report/Summary.xml for threshold check
4. Post summary to PR (GitHub Actions, Azure DevOps, etc.)
5. Fail pipeline if thresholds not met
```

---

## Dashboard Specifications

### Dashboard 1: Module Coverage Overview

| Widget | Data source | View |
|--------|-------------|------|
| Module coverage heatmap | CI coverage reports | Color-coded by module and layer |
| Coverage trend line | Historical CI data | Line chart over sprints |
| Threshold violations | CI gate results | List of modules below threshold |

### Dashboard 2: ADR-Governed Feature Coverage

| Widget | Data source | View |
|--------|-------------|------|
| ADR coverage summary | Tagged coverage reports | Table: ADR ID, feature, coverage % |
| Seam coverage | Test classification tags | Bar chart: State, Service, CQRS, Nav, Event |
| Untested ADR features | ADR list cross-referenced with coverage | List of ADRs with zero coverage |

### Dashboard 3: Sprint Coverage Delta

| Widget | Data source | View |
|--------|-------------|------|
| Before/after comparison | Baseline vs current coverage | Delta table by module |
| New code coverage | Diff coverage from merged PRs | Trend line per sprint |
| Test count trends | Test execution counts | Stacked bar: unit, integration, contract, e2e |

### Dashboard 4: Team Coverage

| Widget | Data source | View |
|--------|-------------|------|
| Coverage by team ownership | Team-module mapping + coverage data | Table: team, modules, coverage |
| Test task completion | JIRA sub-task status | Progress bars per team |
| Coverage improvement rate | Historical delta per team | Trend lines |

---

## Reporting Templates

### PR Coverage Comment

```markdown
## Coverage Report
| Metric | Value |
|--------|-------|
| Overall coverage | XX.X% (baseline: YY.Y%) |
| New code coverage | XX.X% |
| Changed code coverage | XX.X% |
| Threshold | ZZ.Z% |
| Status | PASS / FAIL |

### Files with low coverage
| File | Coverage | Delta |
|------|----------|-------|
| ... | ... | ... |
```

### Sprint Summary

```markdown
## Sprint Coverage Summary - Sprint NN
| Module | Start | End | Delta | Target | Status |
|--------|-------|-----|-------|--------|--------|
| ... | ... | ... | ... | ... | ... |

### Key improvements
- ...

### Areas needing attention
- ...
```
