# Enterprise Quality Control - Workspace Context

Reference toolkit for making test coverage an explicit output of every ICM-governed decision.

## How to Use

This workspace is not a sequential pipeline. Use it as a reference library alongside other ICM workspaces:

1. **Starting a feature?** → Use `10-quality-gates/ICM-Quality-Gates.md` to identify which gates apply and what tests must be planned at each gate.
2. **Writing an ADR?** → Use `20-checklists/ADR-Author-Checklist.md` or `Module-ADR-Checklist.md` to ensure test strategy is included.
3. **Making a State vs Service decision?** → Use `20-checklists/State-Service-Decision-and-Test-Checklist.md` per ADR-0024.
4. **Implementing CQRS handlers?** → Use `20-checklists/Persistence-CQRS-Test-Checklist.md` per ADR-0006/0013.
5. **Working with navigation or events?** → Use `20-checklists/Navigation-and-Eventing-Test-Checklist.md` per ADR-0007/0023.
6. **Testing DAB API endpoints?** → Use `20-checklists/DAB-API-Endpoint-Test-Checklist.md` for REST/GraphQL endpoint coverage.
7. **Setting up CI coverage?** → Use `30-metrics-and-reporting/Test-Coverage-Metrics-and-Dashboards.md`.
8. **Need examples?** → Browse `50-examples/` for tested patterns at each architecture layer.

## Reference Materials

| Resource | Location | Contains |
|---|---|---|
| Canonical Layer-3 defaults | `references/layer-3-defaults-index.md` | Read-only links to `docs/icm-defaults` guidance |
| Quality gates | `10-quality-gates/` | Lifecycle gates with testing hooks |
| Checklists | `20-checklists/` | Per-boundary and per-ADR test checklists |
| Metrics | `30-metrics-and-reporting/` | Coverage metrics and dashboard specs |
| Playbooks | `40-playbooks/` | Operational guides for TDD and QC workflow |
| Examples | `50-examples/` | Concrete test code examples by layer |

## Rules

1. Treat every document linked from `references/layer-3-defaults-index.md` as canonical and read-only.
2. This workspace produces no sequential outputs; it is consumed as a reference by other workspaces and by developers.
3. Checklists reference project-specific ADRs (ADR-0005 through ADR-0024) that live in the project repo, not in icm-defaults.
4. Adapt checklists and playbooks to project specifics; they are templates, not rigid prescriptions.
