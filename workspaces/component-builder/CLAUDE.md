# Component Builder

This workspace guides you through adding a new Blazor component to the Marilo library -- from requirements discovery through implementation, theming, documentation, demos, and testing.

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
│   └── 06-testing/        (unit tests, integration tests, validation)
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
| `status` | Show pipeline completion for all six stages |

### How `status` works

Scan `stages/*/output/` folders. For each stage, if the output folder contains files (other than .gitkeep), the stage is COMPLETE. Otherwise PENDING. Render:

```
Pipeline Status: component-builder

  [01-discovery]  -->  [02-api-design]  -->  [03-implementation]  -->  [04-theming]  -->  [05-demos-and-docs]  -->  [06-testing]
      STATUS              STATUS                  STATUS                 STATUS               STATUS                 STATUS
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

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Discover requirements | `shared/component-patterns.md`, `references/examples/button-walkthrough.md` | `stages/02-api-design/` through `stages/06-testing/` |
| Design API | `stages/01-discovery/output/`, `shared/component-patterns.md`, `shared/css-naming.md` | `stages/03-implementation/` through `stages/06-testing/`, `references/examples/` |
| Implement component | `stages/02-api-design/output/`, `shared/component-patterns.md`, `shared/file-organization.md` | `stages/01-discovery/`, `references/examples/` |
| Add theming | `stages/02-api-design/output/`, `shared/css-naming.md`, `shared/file-organization.md` | `stages/01-discovery/`, `references/examples/` |
| Create demos and docs | `stages/02-api-design/output/`, `stages/03-implementation/output/`, `shared/file-organization.md` | `stages/01-discovery/`, `references/examples/` |
| Write tests | `stages/02-api-design/output/`, `stages/03-implementation/output/`, `shared/component-patterns.md` | `stages/01-discovery/`, `references/examples/` |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there. If you edit an output file between stages, the next stage picks up your edits.

The typical flow is sequential (01 through 06), but stages 04, 05, and 06 all read from stage 02's API design output directly.
