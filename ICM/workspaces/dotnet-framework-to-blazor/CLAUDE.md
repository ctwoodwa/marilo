# .NET Framework to Blazor Migration

Upgrade .NET Framework (4.5/4.6/4.7/4.8) projects -- WebForms, MVC, Web API, WCF -- to .NET Core Blazor with Telerik Blazor UI components and Data API Builder.

## Folder Map

```
dotnet-framework-to-blazor/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (start here for task routing)
├── setup/
│   └── questionnaire.md               (onboarding questions)
├── stages/
│   ├── 01-inventory-assessment/       (catalog projects, NuGet deps, framework APIs, complexity)
│   ├── 02-architecture-design/        (Blazor hosting, DAB config, component hierarchy, patterns)
│   ├── 03-data-layer-migration/       (EF6 to EF Core/DAB, ADO.NET to services, stored procs)
│   ├── 04-ui-conversion/              (convert pages/views/controls to Blazor components with Telerik)
│   └── 05-integration-validation/     (functional parity, YARP cutover, auth, deployment)
├── shared/
│   ├── dotnet-fx-to-blazor-patterns.md   (framework construct to Blazor pattern mapping)
│   ├── telerik-component-map.md          (Telerik Blazor component selection guide)
│   ├── dab-integration-guide.md          (Data API Builder setup and entity mapping)
│   └── webforms-controls-map.md          (WebForms server controls to Blazor/Telerik equivalents)
└── output/                            (workspace-level consolidated artifacts)
```

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire from `setup/questionnaire.md` |
| `status` | Scan `stages/*/output/` and show pipeline completion |
| `inventory` | Jump to Stage 01 with source solution path as input |
| `convert` | Jump to Stage 04 with a specific page or controller to convert |

## Routing

| Task | Go To |
|------|-------|
| First-time setup | `setup/questionnaire.md` |
| Catalog projects, dependencies, APIs | `stages/01-inventory-assessment/CONTEXT.md` |
| Design Blazor + DAB architecture | `stages/02-architecture-design/CONTEXT.md` |
| Migrate data access layer to DAB/EF Core | `stages/03-data-layer-migration/CONTEXT.md` |
| Convert pages/views to Blazor components | `stages/04-ui-conversion/CONTEXT.md` |
| Validate and plan cutover | `stages/05-integration-validation/CONTEXT.md` |

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Inventory | Stage 01 CONTEXT, source .sln/.csproj files | Shared references (not needed yet) |
| Architecture | Stage 02 CONTEXT, Stage 01 output, all shared/ files | Stage 03-05 contexts |
| Data layer | Stage 03 CONTEXT, Stage 02 output, `shared/dab-integration-guide.md`, `shared/dotnet-fx-to-blazor-patterns.md` | Telerik/WebForms maps (not relevant to data layer) |
| UI conversion | Stage 04 CONTEXT, Stage 02+03 outputs, all shared/ files | Stage 01 CONTEXT (already consumed) |
| Validation | Stage 05 CONTEXT, Stage 04 output, Stage 02 output | Shared references (patterns already applied) |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there. If you edit an output file, the next stage picks up your edits.

## Global Constraints

- Run stages in order from 01 to 05 unless a stage explicitly states a valid shortcut.
- All migrated data access must use parameterized queries or DAB endpoints, never string-concatenated SQL.
- Telerik Blazor components are the default UI choice. Only use plain HTML/Blazor when Telerik has no suitable component.
- Data API Builder is the preferred data access layer for straightforward CRUD. EF Core is used for complex domain logic and transactions.
- Preserve functional parity: every converted page must match the original behavior before adding new features.
- Use the .NET Upgrade Assistant output as a starting point where available, but verify and refine all automated conversions.
