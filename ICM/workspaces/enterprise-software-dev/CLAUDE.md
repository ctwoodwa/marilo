# Enterprise Software Dev Workspace

Design and scaffold enterprise-grade .NET solutions: Aspire, Blazor, YARP, DAB, Scalar, GraphQL, and OpenTelemetry.

## Folder Map

```
enterprise-software-dev/
├── CLAUDE.md                   (you are here)
├── CONTEXT.md                  (task routing for this workspace)
├── stages/
│   ├── 01-discovery-and-context/  (problem, domain, constraints)
│   ├── 02-architecture-and-patterns/ (topology, ADRs, boundaries)
│   ├── 03-standards-and-practices/  (coding standards, 12-factor, KISS)
│   ├── 04-service-and-app-specs/    (Blazor, YARP, DAB, Scalar, OTEL specs)
│   └── 05-scaffolds-and-checklists/ (project templates, config stubs, checklists)
├── _config/                    (answers from setup - filled once)
├── shared/                     (cross-cutting reference material)
├── skills/                     (bundled domain skills)
└── setup/
    └── questionnaire.md          (onboarding questions)
```

## Task Routing

| Task Type                        | Go To                                      |
|----------------------------------|--------------------------------------------|
| Capture domain and constraints   | `stages/01-discovery-and-context/CONTEXT.md` |
| Define architecture and ADRs     | `stages/02-architecture-and-patterns/CONTEXT.md` |
| Generate coding standards        | `stages/03-standards-and-practices/CONTEXT.md` |
| Spec individual services         | `stages/04-service-and-app-specs/CONTEXT.md` |
| Generate scaffolds and checklists| `stages/05-scaffolds-and-checklists/CONTEXT.md` |
| Configure the workspace          | `setup/questionnaire.md`                   |

## Triggers

| Keyword  | Action                                               |
|----------|------------------------------------------------------|
| `setup`  | Read `setup/questionnaire.md` and run onboarding     |
| `status` | Scan all `stages/*/output/` and render pipeline view |

## Global Constraints

- All services must emit OpenTelemetry (traces, metrics, logs) to the configured backend.
- API visibility UIs (Scalar, GraphQL Playground) are environment-gated - see `shared/api-visibility.md`.
- Every service follows the 12-factor app principles mapped in Stage 03 output.
- KISS: prefer simple, readable code over clever abstractions.
- No service goes to production without the Stage 05 production-readiness checklist passing.
