# Enterprise Quality Control Workspace

Reference workspace providing quality gates, checklists, metrics, playbooks, and examples that make test coverage an explicit output of every ICM-governed decision.

## Folder Map

```
enterprise-quality-control/
|- CLAUDE.md                            (you are here)
|- CONTEXT.md                           (workspace routing and usage)
|- references/
|  |- layer-3-defaults-index.md         (read-only links to docs/icm-defaults)
|- 10-quality-gates/
|  |- ICM-Quality-Gates.md              (lifecycle gates with testing hooks)
|  |- ICM-JIRA-Workflow-Mapping.md      (gate-to-JIRA status mapping)
|- 20-checklists/
|  |- ADR-Author-Checklist.md           (system-wide ADR authoring checklist)
|  |- Module-ADR-Checklist.md           (module-scoped ADR checklist)
|  |- State-Service-Decision-and-Test-Checklist.md  (ADR-0024 boundary decisions)
|  |- Persistence-CQRS-Test-Checklist.md            (ADR-0006/0013 CQRS tests)
|  |- Navigation-and-Eventing-Test-Checklist.md     (ADR-0007/0023 nav+event tests)
|  |- DAB-API-Endpoint-Test-Checklist.md           (DAB REST/GraphQL endpoint tests)
|- 30-metrics-and-reporting/
|  |- ICM-Metrics.md                    (consolidated ICM + testing metrics)
|  |- Test-Coverage-Metrics-and-Dashboards.md  (code coverage metrics and CI)
|- 40-playbooks/
|  |- ICM-QC-Playbook.md               (how to use this workspace)
|  |- TDD-and-Test-Seams-Playbook.md    (test seams by architecture layer)
|- 50-examples/
|  |- High-Quality-ADR-Examples.md      (exemplary ADR with test strategy)
|  |- High-Quality-Test-Examples-State.md              (state test patterns)
|  |- High-Quality-Test-Examples-Service-and-CQRS.md   (service + CQRS tests)
|  |- High-Quality-Test-Examples-Navigation-and-Events.md  (nav + event tests)
|  |- High-Quality-Test-Examples-bUnit.md                  (bUnit component tests)
|  |- High-Quality-Test-Examples-Playwright.md             (Playwright E2E tests)
|  |- High-Quality-Test-Examples-DAB-API.md                (DAB REST/GraphQL test patterns)
```

## Task Routing

| You want to... | Go to |
|---|---|
| Understand quality gates and testing hooks | `10-quality-gates/ICM-Quality-Gates.md` |
| Map gates to JIRA workflow | `10-quality-gates/ICM-JIRA-Workflow-Mapping.md` |
| Check ADR authoring quality | `20-checklists/ADR-Author-Checklist.md` |
| Check module ADR quality | `20-checklists/Module-ADR-Checklist.md` |
| Decide State vs Service boundaries and tests | `20-checklists/State-Service-Decision-and-Test-Checklist.md` |
| Check CQRS/persistence test coverage | `20-checklists/Persistence-CQRS-Test-Checklist.md` |
| Check navigation and eventing tests | `20-checklists/Navigation-and-Eventing-Test-Checklist.md` |
| Check DAB API endpoint tests | `20-checklists/DAB-API-Endpoint-Test-Checklist.md` |
| Review ICM and testing metrics | `30-metrics-and-reporting/ICM-Metrics.md` |
| Set up coverage dashboards | `30-metrics-and-reporting/Test-Coverage-Metrics-and-Dashboards.md` |
| Learn how to use this workspace | `40-playbooks/ICM-QC-Playbook.md` |
| Learn TDD at each architecture seam | `40-playbooks/TDD-and-Test-Seams-Playbook.md` |
| See example ADRs with test strategy | `50-examples/High-Quality-ADR-Examples.md` |
| See example state tests | `50-examples/High-Quality-Test-Examples-State.md` |
| See example service/CQRS tests | `50-examples/High-Quality-Test-Examples-Service-and-CQRS.md` |
| See example navigation/event tests | `50-examples/High-Quality-Test-Examples-Navigation-and-Events.md` |
| See example bUnit component tests | `50-examples/High-Quality-Test-Examples-bUnit.md` |
| See example Playwright E2E tests | `50-examples/High-Quality-Test-Examples-Playwright.md` |
| See example DAB API tests | `50-examples/High-Quality-Test-Examples-DAB-API.md` |

## Triggers

| Keyword | Action |
|---|---|
| `setup` | Walk through the QC playbook and confirm which checklists apply to the current project |
| `status` | Summarize which quality gates, checklists, and metrics are in use |

## Global Constraints

- This workspace is a reference toolkit, not a sequential pipeline.
- Documents reference project ADRs (ADR-0005 through ADR-0024) that live in the project repo.
- Treat all references in `references/layer-3-defaults-index.md` as canonical and read-only.
- Checklists and playbooks are templates; adapt to project specifics when applying.
