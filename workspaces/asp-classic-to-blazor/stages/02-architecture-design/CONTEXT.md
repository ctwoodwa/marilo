# Architecture Design

Define the target Blazor architecture, map ASP patterns to Blazor equivalents, and select Telerik components.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../01-inventory-assessment/output/{{PROJECT_SLUG}}-page-inventory.md` | Full file | Know what pages exist and their complexity |
| Previous stage | `../01-inventory-assessment/output/{{PROJECT_SLUG}}-dependency-map.md` | Full file | Understand shared dependencies |
| Pattern reference | `../../shared/asp-to-blazor-patterns.md` | Full file | Map each ASP pattern to its Blazor equivalent |
| Telerik guide | `../../shared/telerik-component-map.md` | Full file | Select Telerik components for each UI pattern |
| DAB reference | `../../shared/dab-reference.md` | "Entity Definition" and "REST Endpoint Conventions" sections | Classify endpoints as DAB-eligible vs custom API |

## Process

1. Read the inventory and dependency map from Stage 01.
2. Decide on the Blazor hosting model ({{BLAZOR_HOSTING_MODEL}}) and document the rationale.
3. Design the project structure: solution layout, shared component library, service layer projects.
4. Map each ASP include file to a Blazor shared component or injected service.
5. Map Session/Application usage to the chosen state management approach (cascading parameters, Fluxor, ProtectedSessionStorage).
6. For each UI pattern found in the inventory, select the Telerik Blazor component using the component map.
7. Classify each data endpoint as DAB-eligible (simple CRUD, listing, filtering) or custom API (business logic, multi-step operations, complex aggregations). Record the classification in the architecture output.
8. Design authentication migration: {{CURRENT_AUTH_METHOD}} to {{TARGET_AUTH_METHOD}}.
9. Define the component hierarchy: layouts, shared components, page components.
10. Produce the architecture decision document.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 3 | Proposed solution structure and hosting rationale | Confirm or adjust project layout |
| 6 | Telerik component selections for each UI pattern | Approve component choices or substitute |

## Audit

| Check | Pass Condition |
|-------|---------------|
| Every inventory page mapped | Each page from the inventory has a target Blazor component or group |
| No orphaned includes | Every ASP include maps to a shared component or service |
| State management defined | Session and Application usage has a named Blazor replacement |
| Endpoints classified | Every data operation is marked DAB-eligible or custom API |
| Auth path documented | Authentication migration approach is explicit with named packages |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Architecture decisions | `output/{{PROJECT_SLUG}}-architecture.md` | Sections: Hosting, Project Structure, Pattern Map, Endpoint Classification (DAB vs Custom), Telerik Selections, Auth, State Management |

<!-- Target: keep this file under 80 lines. -->
