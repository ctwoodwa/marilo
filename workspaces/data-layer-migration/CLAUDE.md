# Data Layer Migration Workspace

Replace an in-memory data store with a production SQL Server data layer using a hybrid architecture:
**Microsoft DAB** (Data API Builder) auto-generates REST and GraphQL endpoints directly from the schema,
while the existing API service retains **ad-hoc endpoints** for complex business logic and solver operations.

## Architecture Model

```
                ┌─────────────────────────────────────────┐
                │          SQL Server Database             │
                └────────────┬────────────────┬────────────┘
                             │                │
               ┌─────────────▼──┐     ┌───────▼──────────────┐
               │  Microsoft DAB │     │  Parsons.Comet.ApiService│
               │  (auto-gen)    │     │  (ad-hoc endpoints)  │
               │                │     │                      │
               │  REST CRUD     │     │  Solver pipeline     │
               │  GraphQL       │     │  Complex logic       │
               │  Filtering     │     │  Auth/session        │
               │  Pagination    │     │  Aggregations        │
               └────────────────┘     └──────────────────────┘
```

## Folder Map

```
data-layer-migration/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (workspace routing and rules)
├── _config/
│   ├── migration-context.md           (project paths, DB config, target environment)
│   ├── endpoint-map.md                (DAB-eligible vs ad-hoc endpoint classification)
│   └── schema-decisions.md           (ADR log for schema design choices)
├── stages/
│   ├── 01-endpoint-classification/    (categorize each endpoint: DAB vs ad-hoc)
│   ├── 02-schema-design/              (SQL Server schema from in-memory model)
│   ├── 03-dab-setup/                  (DAB config, entity definitions, permissions)
│   ├── 04-ef-core-setup/              (DbContext, migrations, seed scripts)
│   └── 05-repository-wiring/          (repository layer, DI swap, integration tests)
├── shared/
│   ├── dab-reference.md               (DAB config syntax and patterns)
│   ├── dab-config-template.json       (starter DAB configuration file)
│   └── ef-core-patterns.md            (EF Core patterns for this stack)
├── setup/
│   └── questionnaire.md               (onboarding questions)
└── output/                            (workspace-level summaries)
```

## Task Routing

| Task Type | Go To |
|---|---|
| First-time setup | `setup/questionnaire.md` |
| Classify endpoints (DAB vs ad-hoc) | `stages/01-endpoint-classification/CONTEXT.md` |
| Design SQL Server schema | `stages/02-schema-design/CONTEXT.md` |
| Configure DAB | `stages/03-dab-setup/CONTEXT.md` |
| Set up EF Core for ad-hoc endpoints | `stages/04-ef-core-setup/CONTEXT.md` |
| Wire repositories, swap in-memory store | `stages/05-repository-wiring/CONTEXT.md` |

## Triggers

| Keyword | Action |
|---|---|
| `setup` | Read `setup/questionnaire.md` and populate `_config/` |
| `status` | Scan `stages/*/output/` and show pipeline completion |
| `classify` | Jump to Stage 01 with current endpoint list as input |
| `schema` | Jump to Stage 02; requires Stage 01 output or a filled `_config/endpoint-map.md` |

## Global Constraints

- DAB is the default for any endpoint that is purely data retrieval or simple CRUD — do not re-implement in the API what DAB provides for free.
- The API service retains only endpoints that require business logic, solver execution, or multi-step operations.
- EF Core owns schema migrations. DAB reads from the same database but does not own migrations.
- All schema design decisions are recorded as ADRs in `_config/schema-decisions.md`.
- Do not modify existing API endpoint contracts during migration — preserve request/response shapes.
