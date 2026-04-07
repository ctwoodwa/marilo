# ASP Classic to Blazor Migration

Upgrade ASP Classic pages (VBScript/.asp) to .NET Core Blazor with Telerik Blazor UI components.

## Folder Map

```
asp-classic-to-blazor/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (start here for task routing)
├── setup/
│   └── questionnaire.md               (onboarding questions)
├── stages/
│   ├── 01-inventory-assessment/       (catalog ASP pages, dependencies, complexity)
│   ├── 02-architecture-design/        (Blazor hosting model, component hierarchy, patterns)
│   ├── 03-data-layer-migration/       (DAB for CRUD endpoints, EF Core for complex logic)
│   ├── 04-page-conversion/            (convert ASP pages to Blazor components with Telerik)
│   └── 05-integration-validation/     (functional parity, theming, cutover plan)
├── shared/
│   ├── asp-to-blazor-patterns.md      (ASP Classic to Blazor pattern mapping)
│   ├── dab-reference.md               (DAB config syntax and patterns)
│   ├── telerik-component-map.md       (Telerik Blazor component selection guide)
│   └── vbscript-to-csharp.md          (VBScript to C# conversion reference)
└── output/                            (workspace-level consolidated artifacts)
```

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire from `setup/questionnaire.md` |
| `status` | Scan `stages/*/output/` and show pipeline completion |
| `inventory` | Jump to Stage 01 with source project path as input |
| `convert` | Jump to Stage 04 with a specific page name to convert |

## Routing

| Task | Go To |
|------|-------|
| First-time setup | `setup/questionnaire.md` |
| Catalog ASP pages and dependencies | `stages/01-inventory-assessment/CONTEXT.md` |
| Design Blazor architecture | `stages/02-architecture-design/CONTEXT.md` |
| Migrate data access layer | `stages/03-data-layer-migration/CONTEXT.md` |
| Convert pages to Blazor components | `stages/04-page-conversion/CONTEXT.md` |
| Validate and plan cutover | `stages/05-integration-validation/CONTEXT.md` |

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Inventory | Stage 01 CONTEXT, source ASP project files | Shared references (not needed yet) |
| Architecture | Stage 02 CONTEXT, Stage 01 output, all shared/ files | Stage 03-05 contexts |
| Data layer | Stage 03 CONTEXT, Stage 02 output, `shared/asp-to-blazor-patterns.md`, `shared/dab-reference.md` | Telerik map (not relevant to data layer) |
| Page conversion | Stage 04 CONTEXT, Stage 02+03 outputs, all shared/ files | Stage 01 CONTEXT (already consumed) |
| Validation | Stage 05 CONTEXT, Stage 04 output, Stage 02 output | Shared references (patterns already applied) |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there. If you edit an output file, the next stage picks up your edits.

## Global Constraints

- Run stages in order from 01 to 05 unless a stage explicitly states a valid shortcut.
- DAB is the default for any endpoint that is purely data retrieval or simple CRUD. Do not re-implement in a custom API what DAB provides for free.
- All VBScript-to-C# conversions must produce parameterized queries, never string-concatenated SQL.
- Telerik Blazor components are the default UI choice. Only use plain HTML/Blazor when Telerik has no suitable component.
- Preserve functional parity: every converted page must match the original ASP page behavior before adding new features.
