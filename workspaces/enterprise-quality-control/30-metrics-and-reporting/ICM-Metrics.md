# ICM Metrics

> **Purpose:** Consolidated metrics for tracking ICM process health, ADR quality, and test coverage integration.
> **Aligned with:** ADR-0003 (Enhanced Hybrid Documentation Architecture) success criteria.

> Status: Draft
> Owner: Architecture Team
> Last updated: 2026-03-21

## ADR and Decision Metrics

| Metric | Description | Target | Measurement |
|--------|-------------|--------|-------------|
| ADR completeness rate | Percentage of accepted ADRs with all template sections filled | 100% | Quarterly audit |
| ADR test strategy rate | Percentage of accepted ADRs with explicit test strategy (Validation section) | 100% | Quarterly audit |
| ADR review cycle time | Average time from "proposed" to "accepted" | < 2 sprints | ADR date fields |
| ADR currency | Percentage of accepted ADRs reviewed within last 12 months | 100% | Quarterly audit |

## Boundary Decision Metrics (ADR-0024)

| Metric | Description | Target | Measurement |
|--------|-------------|--------|-------------|
| Boundary classification rate | Percentage of features with explicit State/Service/CQRS classification | 100% | JIRA field or ADR check |
| Boundary-to-test mapping rate | Percentage of classified features with tests at selected seams | > 90% | CI coverage + JIRA sub-tasks |
| Boundary decision accuracy | Percentage of boundary decisions not revised post-implementation | > 85% | Retrospective tracking |

## Test Coverage Metrics

| Metric | Description | Target | Measurement |
|--------|-------------|--------|-------------|
| Overall line coverage | Line coverage across all modules | Project-specific | CI coverage tool |
| New/changed code coverage | Diff-based coverage for each PR | > 80% | CI diff coverage |
| ADR-governed feature coverage | Coverage for code areas governed by specific ADRs | > 85% | Tagged coverage reports |
| Contract test coverage | Percentage of API/event interfaces with contract tests | 100% for governed interfaces | Contract test inventory |

## Documentation Metrics (per ADR-0003)

| Metric | Description | Target | Measurement |
|--------|-------------|--------|-------------|
| Discovery time | Average time to find relevant documentation | < 5 minutes | Team survey |
| Documentation coverage | Percentage of modules with up-to-date docs | > 90% | Quarterly audit |
| Update frequency | Documentation updates per sprint | > 0 per active module | Git commit tracking |
| Team satisfaction | Developer satisfaction with documentation quality | > 4/5 | Quarterly survey |

## Quality Gate Metrics

| Metric | Description | Target | Measurement |
|--------|-------------|--------|-------------|
| Gate pass rate | Percentage of features passing all 5 gates | > 95% | JIRA workflow tracking |
| Gate skip rate | Percentage of features bypassing gates | < 5% | JIRA audit |
| Test sub-task completion rate | Percentage of test sub-tasks completed before "Done" | 100% | JIRA sub-task tracking |

## Reporting Cadence

| Report | Frequency | Audience | Contains |
|--------|-----------|----------|----------|
| Sprint coverage summary | Per sprint | Dev team | Coverage trends, new/changed code coverage, failing thresholds |
| ADR health report | Quarterly | Architecture team | ADR completeness, currency, test strategy rates |
| Quality gate compliance | Monthly | Engineering leads | Gate pass/skip rates, common failure points |
| Documentation health | Quarterly | All teams | Discovery time, coverage, satisfaction scores |
