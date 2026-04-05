# Component Builder

Add a new Blazor component to the Marilo library through a seven-stage guided process. The process is all-inclusive: it creates the component source, tests, themes, demos, spec documentation, delivery workspace, and gap-analysis workspace.

## Task Routing

| Task Type | Go To | Description |
|-----------|-------|-------------|
| Discover requirements | `stages/01-discovery/CONTEXT.md` | Understand use cases, behavior, accessibility needs |
| Design component API | `stages/02-api-design/CONTEXT.md` | Define parameters, events, enums, CSS provider contract |
| Implement source code | `stages/03-implementation/CONTEXT.md` | Build core models, enums, component razor/cs files |
| Add theme styles | `stages/04-theming/CONTEXT.md` | Implement FluentUI and Bootstrap provider styles |
| Create demos and docs | `stages/05-demos-and-docs/CONTEXT.md` | Write demo pages and API documentation specs |
| Write tests | `stages/06-testing/CONTEXT.md` | Unit tests, CSS provider tests, integration tests |
| Scaffold workspaces | `stages/07-workspace-scaffolding/CONTEXT.md` | Create delivery workspace, gap-analysis workspace, spec docs |

## Reference Materials

| Resource | Location | Contains |
|----------|----------|----------|
| Component patterns | `shared/component-patterns.md` | Base class, CSS provider, parameter conventions |
| CSS naming | `shared/css-naming.md` | BEM-like mar- prefix naming rules |
| File organization | `shared/file-organization.md` | Where each artifact lives in the Marilo repo |
| MWP conventions | `references/conventions-reference.md` | Pointer to core workspace conventions |
| Completed example | `references/examples/button-walkthrough.md` | Summary of the Button component as a reference |

## Workspace Templates (used by Stage 07)

| Template | Location | Creates |
|----------|----------|---------|
| Delivery workspace | `workspaces/shared/component-delivery-template.md` | `{slug}-delivery/` workspace |
| Gap-analysis workspace | `workspaces/shared/component-gap-analysis-template.md` | `{slug}-gap-analysis/` workspace |
| Spec documentation | `workspaces/shared/component-spec-template.md` | `docs/component-specs/{slug}/` |
| Workspace routing | `workspaces/shared/workspace-routing.md` | Component registration |
