# Enterprise ICM

ICM workspace for designing and scaffolding enterprise-grade software, services, and utilities.
Built on the Model Workspace Protocol (MWP) five-layer architecture.

## Folder Map

```
enterprise-icm/
├── CLAUDE.md                          (you are here)
├── README.md                          (project overview)
├── LICENSE
├── _core/                             (shared conventions, templates, and references)
│   ├── CONVENTIONS.md                 (source of truth for all patterns)
│   ├── placeholder-syntax.md          (how {{VARIABLES}} work)
│   ├── references/
│   │   └── layer-3-defaults-index.md  (canonical Layer-3 defaults index)
│   └── templates/                     (blank starting points for new workspaces)
├── docs/icm-defaults/                 (enterprise canonical defaults -- Layer 3)
└── workspaces/
    ├── enterprise-software-dev/               (greenfield .NET platform design)
    ├── enterprise-standards-upgrade/          (brownfield standards gap analysis)
    ├── enterprise-feature-change/             (ticket-to-PR feature workflow)
    ├── enterprise-api-change/                 (API contract change management)
    ├── enterprise-observability-enhancement/  (telemetry modernization)
    ├── enterprise-test-coverage/              (test strategy and coverage planning)
    ├── enterprise-quality-control/            (quality gates and checklists reference)
    ├── data-layer-migration/                  (in-memory to SQL Server migration)
    ├── ci-cd-pipeline/                        (pipeline design and scaffolding)
    ├── test-coverage-expansion/               (test scaffold generation)
    ├── docs-generator/                        (project documentation generation)
    ├── asp-classic-to-blazor/                 (ASP Classic to Blazor migration)
    ├── jquery-to-blazor/                      (jQuery page to Blazor/Telerik migration)
    └── gap-analysis-resolution/               (gap analysis to verified resolution)
```

## Routing

| You want to...                                        | Go to                                                         |
| ----------------------------------------------------- | ------------------------------------------------------------- |
| Design an enterprise software platform                | `workspaces/enterprise-software-dev/CLAUDE.md`                |
| Upgrade an existing repo to enterprise standards      | `workspaces/enterprise-standards-upgrade/CLAUDE.md`           |
| Implement a feature from ticket to PR                 | `workspaces/enterprise-feature-change/CLAUDE.md`              |
| Change an API or event contract                       | `workspaces/enterprise-api-change/CLAUDE.md`                  |
| Modernize observability and telemetry                 | `workspaces/enterprise-observability-enhancement/CLAUDE.md`   |
| Plan and expand test coverage                         | `workspaces/enterprise-test-coverage/CLAUDE.md`               |
| Review quality gates, checklists, and examples        | `workspaces/enterprise-quality-control/CLAUDE.md`             |
| Migrate data layer from in-memory to SQL Server       | `workspaces/data-layer-migration/CLAUDE.md`                   |
| Design or scaffold a CI/CD pipeline                   | `workspaces/ci-cd-pipeline/CLAUDE.md`                         |
| Generate test scaffolds for existing code             | `workspaces/test-coverage-expansion/CLAUDE.md`                |
| Generate project documentation                       | `workspaces/docs-generator/CLAUDE.md`                         |
| Migrate ASP Classic pages to Blazor                   | `workspaces/asp-classic-to-blazor/CLAUDE.md`                  |
| Upgrade jQuery pages to Blazor with Telerik           | `workspaces/jquery-to-blazor/CLAUDE.md`                       |
| Resolve gaps from a gap analysis                      | `workspaces/gap-analysis-resolution/CLAUDE.md`                |
| Read the full MWP specification                       | `_core/CONVENTIONS.md`                                        |
| Understand the placeholder system                     | `_core/placeholder-syntax.md`                                 |
| Use a template for a new workspace                    | `_core/templates/`                                            |
| Read template-building guidance                       | `_core/templates/template-guide.md`                           |
| Set up a component-delivery workspace                 | `_core/templates/component-delivery-template.md`              |
| Use reconstructed-pipeline mode                       | `_core/templates/reconstructed-pipeline-guide.md`             |
| Add status snapshots to a workspace                   | `_core/templates/summary-snapshot-template.md`                |
| View the canonical Layer-3 defaults index             | `_core/references/layer-3-defaults-index.md`                  |

## Triggers

| Keyword  | Action                                             |
| -------- | -------------------------------------------------- |
| `setup`  | Run onboarding in whatever workspace you are in    |
| `status` | Show pipeline completion for the current workspace |

## How It Works

Each workspace is self-contained with its own CLAUDE.md. Navigate into a workspace
folder and that workspace's CLAUDE.md takes over. You do not need to read this root
file once you are inside a workspace.
