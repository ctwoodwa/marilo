# PM Demo Builder

Plan, sequence, and execute implementation passes for the Marilo PM Demo.

## Task Routing

| Task Type | Go To | Description |
|-----------|-------|-------------|
| Assess current state | `stages/01-current-state/CONTEXT.md` | Inventory existing pages, services, entities, and shell wiring |
| Plan IA and shell | `stages/02-ia-and-shell/CONTEXT.md` | Navigation, layout nesting, route structure for new features |
| Model domains | `stages/03-domain-modeling/CONTEXT.md` | Entities, services, DTOs for settings, assets, inspections |
| Build pages | `stages/04-page-build/CONTEXT.md` | Implement Razor pages, components, layouts |
| Integrate and wire | `stages/05-integration/CONTEXT.md` | DI registration, seed data, cross-cutting concerns |
| Review and polish | `stages/06-review/CONTEXT.md` | QA, acceptance criteria, gap check |

## Shared Resources

| Resource | Location | Contains |
|----------|----------|----------|
| Current PM demo state | `_config/pm-demo-context.md` | Baseline assessment of samples/Marilo.PmDemo |
| Domain expansion scope | `_config/domain-expansion.md` | Asset management, dynamic forms, inspections |
| Implementation guardrails | `shared/implementation-guardrails.md` | Agreed architecture and coding rules |
| Component inventory | `shared/component-inventory.md` | Existing Marilo components available to the demo |
| Shell and IA conventions | `shared/shell-and-ia.md` | App shell, layout nesting, navigation patterns |
| Settings status (canonical) | `samples/Marilo.PmDemo/SETTINGS_STATUS.md` | Live tracking file (source of truth, not a copy) |
