# jQuery to Blazor Migration

Upgrade jQuery-based pages to .NET Core Blazor with Telerik Blazor UI components within an existing Blazor project.

## Folder Map

```
jquery-to-blazor/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (start here for task routing)
├── setup/
│   └── questionnaire.md               (onboarding questions)
├── stages/
│   ├── 01-feature-inventory/          (catalog jQuery features, plugins, DOM patterns)
│   ├── 02-codebase-alignment/         (analyze target Blazor project patterns to follow)
│   ├── 03-component-mapping/          (map jQuery features to Telerik Blazor components)
│   ├── 04-page-conversion/            (convert pages to Blazor components with Telerik)
│   └── 05-integration-validation/     (functional parity, regression testing, cutover)
├── shared/
│   ├── jquery-to-blazor-patterns.md   (jQuery constructs mapped to Blazor equivalents)
│   ├── jquery-plugin-map.md           (common jQuery plugins mapped to Telerik components)
│   ├── telerik-component-map.md       (Telerik Blazor component selection guide)
│   └── js-interop-bridge.md           (JSInterop patterns for features without native Blazor equivalent)
└── output/                            (workspace-level consolidated artifacts)
```

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire from `setup/questionnaire.md` |
| `status` | Scan `stages/*/output/` and show pipeline completion |
| `inventory` | Jump to Stage 01 with source page path as input |
| `convert` | Jump to Stage 04 with a specific page name to convert |

## Routing

| Task | Go To |
|------|-------|
| First-time setup | `setup/questionnaire.md` |
| Catalog jQuery features and dependencies | `stages/01-feature-inventory/CONTEXT.md` |
| Analyze existing Blazor project patterns | `stages/02-codebase-alignment/CONTEXT.md` |
| Map jQuery features to Telerik components | `stages/03-component-mapping/CONTEXT.md` |
| Convert pages to Blazor components | `stages/04-page-conversion/CONTEXT.md` |
| Validate and plan cutover | `stages/05-integration-validation/CONTEXT.md` |

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Feature inventory | Stage 01 CONTEXT, source jQuery page files | Shared references (not needed yet) |
| Codebase alignment | Stage 02 CONTEXT, Stage 01 output, target Blazor project files | Stages 03-05 contexts |
| Component mapping | Stage 03 CONTEXT, Stage 01+02 outputs, all shared/ files | Stage 04-05 contexts |
| Page conversion | Stage 04 CONTEXT, Stage 02+03 outputs, all shared/ files | Stage 01 CONTEXT (already consumed) |
| Validation | Stage 05 CONTEXT, Stage 04 output, Stage 02 output | Shared references (patterns already applied) |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there. If you edit an output file, the next stage picks up your edits.

## Global Constraints

- Run stages in order from 01 to 05 unless a stage explicitly states a valid shortcut.
- The target Blazor project already exists. This workspace does not scaffold a new project. It adds pages into the existing project structure.
- All conversions must follow the patterns, naming conventions, and service registration approach already established in the target Blazor project.
- Telerik Blazor components are the default UI choice. Only use plain HTML/Blazor when Telerik has no suitable component.
- jQuery features must be fully cataloged before conversion begins. No ad-hoc conversion without inventory.
- Preserve functional parity: every converted page must match the original jQuery page behavior before adding new features.
- JSInterop is the bridge of last resort. Prefer native Blazor/Telerik solutions. Document any JSInterop usage with a rationale.
- No direct DOM manipulation from Blazor code. If the jQuery page manipulates the DOM, the Blazor equivalent must use component state and re-rendering.
