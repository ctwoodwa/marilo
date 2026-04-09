# Workspace Routing & Graduation Criteria

## 1. Workspace Taxonomy

| Category | Workspaces | Purpose |
|---|---|---|
| **Gap Analysis** | gap-analysis-resolution, datagrid-gap-analysis, datasheet-gap-analysis, resizable-container-gap-analysis, chart-gap-analysis, editor-gap-analysis, splitter-gap-analysis, wizard-gap-analysis, treelist-gap-analysis, scheduler-gap-analysis, gantt-gap-analysis, filemanager-gap-analysis, diagram-gap-analysis, dockmanager-gap-analysis, map-gap-analysis, pivotgrid-gap-analysis | Intake, prioritize, resolve, and close documented component gaps |
| **Component Delivery** | treeview-delivery, datagrid-delivery, resizable-container-delivery, chart-delivery, editor-delivery, datasheet-delivery, splitter-delivery, wizard-delivery, treelist-delivery, scheduler-delivery, gantt-delivery, filemanager-delivery, diagram-delivery, dockmanager-delivery, map-delivery, pivotgrid-delivery | Coordinate spec accuracy, demo completeness, and release readiness for complex components |
| **Component Pipeline** | component-builder | End-to-end new component creation (discovery → build → test) |
| **Migration** | asp-classic-to-blazor, dotnet-framework-to-blazor, jquery-to-blazor, data-layer-migration | Structured legacy-to-modern porting workflows |
| **Enterprise Patterns** | enterprise-software-dev, enterprise-api-change, enterprise-feature-change, enterprise-standards-upgrade, enterprise-test-coverage, enterprise-observability-enhancement, enterprise-quality-control | Cross-cutting engineering practices and standards |
| **Infrastructure** | ci-cd-pipeline, workspace-builder, docs-generator, test-coverage-expansion | Tooling, automation, and shared scaffolding |

---

## 2. Routing Quick Reference

| I want to… | Use this workspace |
|---|---|
| Fix a documented component gap | gap-analysis-resolution |
| Triage and prioritize outstanding gaps | gap-analysis-resolution |
| Build a brand-new component from scratch | component-builder |
| Coordinate spec, demo, and release for a complex component | component-delivery workspace (e.g., treeview-delivery) |
| Run a gap analysis scoped to one component | component-specific gap-analysis (e.g., datagrid-gap-analysis) |
| Port ASP Classic, jQuery, or .NET Framework code to Blazor | matching migration workspace |
| Migrate a data layer | data-layer-migration |
| Improve API consistency or versioning strategy | enterprise-api-change |
| Raise test coverage across the repo | enterprise-test-coverage or test-coverage-expansion |
| Add observability / logging / tracing | enterprise-observability-enhancement |
| Enforce or upgrade engineering standards | enterprise-standards-upgrade |
| Generate or update shared docs | docs-generator |
| Extend or repair CI/CD pipelines | ci-cd-pipeline |
| Scaffold a new workspace | workspace-builder |

---

## 3. When to Graduate from gap-analysis-resolution

Graduate a component to its own delivery workspace when **3 or more** of the following are true:

| # | Criterion |
|---|---|
| 1 | Component has multiple deliverable surfaces (source, docs, examples, tests) that must ship together |
| 2 | Work spans many sessions and risks context drift within the shared workspace |
| 3 | Component-specific API or demo decisions are being made that do not affect other components |
| 4 | High coupling across code, documentation, and UX examples requires coordinated tracking |
| 5 | gap-analysis-resolution is becoming crowded with component-specific artifacts |

**Decision rule:** If 3+ criteria are true, create a `<component>-delivery` workspace and move delivery coordination there.

---

## 4. What Stays in gap-analysis-resolution

All of the following remain in gap-analysis-resolution regardless of component maturity:

| Activity | Stays here |
|---|---|
| Gap intake and documentation | Yes |
| Gap prioritization and sequencing | Yes |
| Resolution design and approach decisions | Yes |
| Implementation of code fixes | Yes |
| Validation and closure sign-off | Yes |
| **Delivery coordination (spec, demo, release readiness)** | **Graduates out** |

Only delivery coordination graduates. Everything upstream of "it works and is closed" stays in gap-analysis-resolution.
