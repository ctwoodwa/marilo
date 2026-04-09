# Stage 01: Current State Assessment

Inventory what exists in the live PM demo sample app to establish a baseline for planning.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Config | `../../_config/pm-demo-context.md` | Full file | Last-known state summary to verify/update |
| Live source | `samples/Marilo.PmDemo/` | File listing + key files | Actual current code |
| Settings status | `samples/Marilo.PmDemo/SETTINGS_STATUS.md` | Full file | Canonical settings progress |
| Reference | `../../shared/component-inventory.md` | Full file | Know which Marilo components are available |

## Process

1. List all `.razor` page files in `Marilo.PmDemo.Client/Pages/` and their `@page` routes.
2. Read `MainLayout.razor` — document: injected services, sidebar nav structure, footer wiring, layout nesting.
3. List all files in `Marilo.PmDemo.Client/Notifications/` — confirm canonical notification pipeline state.
4. List all services registered in `Marilo.PmDemo/Program.cs` (grep for `AddScoped`, `AddSingleton`, `AddTransient`).
5. Read `Marilo.PmDemo.Data/Entities/Entities.cs` — document entity names and key relationships.
6. Read `SETTINGS_STATUS.md` — extract settings build order status table.
7. Identify any gap between `_config/pm-demo-context.md` and actual current state. Update `_config/pm-demo-context.md` if stale.
8. Write the baseline inventory to output. Separate "what exists and works today" from "what is planned or aspirational". The baseline must reflect reality, not the roadmap.

## Audit

| Check | Pass Condition |
|-------|---------------|
| Reality check | Every item marked DONE has been verified against the live codebase |
| No aspiration bleed | Output does not list planned/future items as current state |
| Source of truth | `_config/pm-demo-context.md` matches the output — if not, the config file was updated |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Baseline inventory | `output/baseline-inventory.md` | Tables: pages, services, entities, shell state, settings status, identified gaps |
