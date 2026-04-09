# PM Demo Builder

Organize planning, implementation sequencing, and handoff prompts for the ongoing Marilo PM Demo at `samples/Marilo.PmDemo`.

## Folder Map

```
pm-demo-builder/
├── CLAUDE.md                 (you are here)
├── CONTEXT.md                (task routing)
├── setup/
│   └── questionnaire.md      (onboarding — scope and priorities for a build pass)
├── _config/
│   ├── pm-demo-context.md    (current state assessment from samples/Marilo.PmDemo)
│   └── domain-expansion.md   (asset management, dynamic forms, inspections scope)
├── _status/
│   └── workspace-status.md   (snapshot of pipeline completion)
├── shared/
│   ├── implementation-guardrails.md  (agreed coding + architecture rules)
│   ├── component-inventory.md        (existing Marilo components usable by the demo)
│   └── shell-and-ia.md              (app shell, navigation, layout conventions)
├── stages/
│   ├── 01-current-state/     (baseline what exists in the live sample app)
│   ├── 02-ia-and-shell/      (information architecture, navigation, layout decisions)
│   ├── 03-domain-modeling/   (entity models, services, data layer for new features)
│   ├── 04-page-build/        (implement pages — settings, views, domain features)
│   ├── 05-integration/       (wire services, seed data, cross-cutting concerns)
│   └── 06-review/            (QA, polish, gap check, acceptance)
└── references/
    └── settings-status.md    (pointer to samples/Marilo.PmDemo/SETTINGS_STATUS.md)
```

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding — asks about which build pass to execute |
| `status` | Show pipeline completion for all six stages |

### How `status` works

Scan `stages/*/output/` folders. For each stage, if the output folder contains files (other than .gitkeep), the stage is COMPLETE. Otherwise PENDING. Render:

```
Pipeline Status: pm-demo-builder

  [01-current-state]  -->  [02-ia-and-shell]  -->  [03-domain-modeling]  -->  [04-page-build]  -->  [05-integration]  -->  [06-review]
       STATUS                  STATUS                   STATUS                  STATUS                STATUS               STATUS
```

## Routing

| Task | Go To |
|------|-------|
| Assess what exists in the PM demo today | `stages/01-current-state/CONTEXT.md` |
| Plan navigation, layout, and IA for new features | `stages/02-ia-and-shell/CONTEXT.md` |
| Model entities, services, and data contracts | `stages/03-domain-modeling/CONTEXT.md` |
| Build pages and components | `stages/04-page-build/CONTEXT.md` |
| Wire services, seed data, register DI | `stages/05-integration/CONTEXT.md` |
| Review, polish, and verify acceptance | `stages/06-review/CONTEXT.md` |

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Current state assessment | `_config/pm-demo-context.md`, `shared/component-inventory.md` | `stages/02-*` through `stages/06-*`, `_config/domain-expansion.md` |
| IA and shell planning | `stages/01-current-state/output/`, `_config/domain-expansion.md`, `shared/shell-and-ia.md` | `stages/03-*` through `stages/06-*` |
| Domain modeling | `stages/02-ia-and-shell/output/`, `shared/implementation-guardrails.md` | `stages/01-*`, `stages/04-*` through `stages/06-*` |
| Page build | `stages/03-domain-modeling/output/`, `shared/component-inventory.md`, `shared/implementation-guardrails.md` | `stages/01-*`, `stages/02-*`, `stages/05-*`, `stages/06-*` |
| Integration | `stages/04-page-build/output/`, `shared/implementation-guardrails.md` | `stages/01-*` through `stages/03-*`, `stages/06-*` |
| Review | `stages/05-integration/output/`, `shared/implementation-guardrails.md`, `_config/pm-demo-context.md` | `stages/01-*` through `stages/04-*` |
