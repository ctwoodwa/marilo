# Component Builder

This workspace guides you through adding a new Blazor component to the Marilo library -- from requirements discovery through implementation, theming, documentation, demos, testing, and full ICM workspace scaffolding.

The process is **all-inclusive**: when complete, the new component has source code, tests, theme providers, demo pages, API spec documentation, a delivery workspace, and a gap-analysis workspace -- everything needed to participate in the full ICM pipeline.

## Folder Map

```
component-builder/
├── CLAUDE.md              (you are here)
├── CONTEXT.md             (start here for task routing)
├── setup/
│   └── questionnaire.md   (onboarding -- asks about the component you are building)
├── stages/
│   ├── 01-discovery/      (understand requirements, use cases, accessibility)
│   ├── 02-api-design/     (define parameters, events, enums, CSS provider methods)
│   ├── 03-implementation/ (build core infrastructure and component source)
│   ├── 04-theming/        (FluentUI and Bootstrap provider styles)
│   ├── 05-demos-and-docs/ (demo pages and API documentation)
│   ├── 06-testing/        (unit tests, integration tests, validation)
│   └── 07-workspace-scaffolding/ (delivery workspace, gap-analysis workspace, spec docs)
├── shared/
│   ├── component-patterns.md    (base class, CSS provider, parameter conventions)
│   ├── css-naming.md            (BEM-like mar- prefix naming rules)
│   └── file-organization.md     (where each artifact goes in the repo)
└── references/
    ├── conventions-reference.md (pointer to core MWP conventions)
    └── examples/
        └── button-walkthrough.md (example of a completed component)
```

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding -- asks about the component you want to build |
| `status` | Show pipeline completion for all seven stages |
| `build` | Run all seven stages in sequence (full component creation) |

### How `status` works

Scan `stages/*/output/` folders. For each stage, if the output folder contains files (other than .gitkeep), the stage is COMPLETE. Otherwise PENDING. Render:

```
Pipeline Status: component-builder

  [01-discovery] --> [02-api-design] --> [03-implementation] --> [04-theming] --> [05-demos-and-docs] --> [06-testing] --> [07-workspace-scaffolding]
      STATUS              STATUS                STATUS               STATUS             STATUS               STATUS              STATUS
```

## Routing

| Task | Go To |
|------|-------|
| Discover component requirements | `stages/01-discovery/CONTEXT.md` |
| Design the component API | `stages/02-api-design/CONTEXT.md` |
| Implement the component | `stages/03-implementation/CONTEXT.md` |
| Add theme provider styles | `stages/04-theming/CONTEXT.md` |
| Create demos and documentation | `stages/05-demos-and-docs/CONTEXT.md` |
| Write tests | `stages/06-testing/CONTEXT.md` |
| Scaffold workspaces and spec docs | `stages/07-workspace-scaffolding/CONTEXT.md` |

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Discover requirements | `shared/component-patterns.md`, `references/examples/button-walkthrough.md` | `stages/02-api-design/` through `stages/07-workspace-scaffolding/` |
| Design API | `stages/01-discovery/output/`, `shared/component-patterns.md`, `shared/css-naming.md` | `stages/03-implementation/` through `stages/07-workspace-scaffolding/`, `references/examples/` |
| Implement component | `stages/02-api-design/output/`, `shared/component-patterns.md`, `shared/file-organization.md` | `stages/01-discovery/`, `references/examples/` |
| Add theming | `stages/02-api-design/output/`, `shared/css-naming.md`, `shared/file-organization.md` | `stages/01-discovery/`, `references/examples/` |
| Create demos and docs | `stages/02-api-design/output/`, `stages/03-implementation/output/`, `shared/file-organization.md` | `stages/01-discovery/`, `references/examples/` |
| Write tests | `stages/02-api-design/output/`, `stages/03-implementation/output/`, `shared/component-patterns.md` | `stages/01-discovery/`, `references/examples/` |
| Scaffold workspaces | `stages/01-discovery/output/`, `stages/02-api-design/output/`, `stages/03-implementation/output/`, `stages/06-testing/output/`, shared templates from `workspaces/shared/` | `stages/04-theming/`, `stages/05-demos-and-docs/`, `references/examples/` |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there. If you edit an output file between stages, the next stage picks up your edits.

The typical flow is sequential (01 through 07), but stages 04, 05, and 06 all read from stage 02's API design output directly. Stage 07 reads from stages 01, 02, 03, and 06.

## What Gets Created

After all seven stages complete, the new component has:

| Artifact | Location |
|----------|----------|
| Core enums and models | `src/Marilo.Core/` |
| Component source (.razor, .razor.cs) | `src/Marilo.Components/{Category}/` |
| FluentUI CSS provider methods | `src/Marilo.Providers.FluentUI/` |
| Bootstrap CSS provider methods | `src/Marilo.Providers.Bootstrap/` |
| SCSS theme files | Provider style directories |
| Demo pages | `samples/Marilo.Demo/Pages/Components/{Name}/` |
| API spec documentation | `docs/component-specs/{slug}/` |
| Unit tests | `tests/Marilo.Tests.Unit/{Category}/` |
| Delivery workspace | `workspaces/{slug}-delivery/` |
| Gap-analysis workspace | `workspaces/{slug}-gap-analysis/` |
| Routing registration | `workspaces/shared/workspace-routing.md` |
