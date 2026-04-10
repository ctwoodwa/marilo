<!-- Target: ~800 tokens. Trim if longer. -->
# Component Delivery Workspace -- MariloTreeList

Coordinates spec accuracy, Example UX completeness, and source+tests alignment for a single complex Blazor component.

## Folder Map

```
treelist-delivery/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (start here for task routing)
├── _config/
│   └── delivery-context.md            (component paths, state tracking, gate status)
├── stages/
│   ├── 01-spec-review/                (audit API spec vs. implementation)
│   │   └── output/                    (spec gap list)
│   ├── 02-example-ux/                 (audit and update demo page scenarios)
│   │   ├── shared/                    (demo scenario format)
│   │   └── output/                    (demo gap list, updated demo page)
│   ├── 03-visual-parity/             (theme-aware visual comparison and gap scoring)
│   │   ├── shared/                    (capture matrix, rubric, gap format, remediation template)
│   │   └── output/                    (parity gaps, parity summary)
│   └── 04-sync-check/                 (confirm all artifacts are in sync)
│       ├── shared/                    (delivery checklist)
│       └── output/                    (delivery report)
└── shared/
    └── spec-coverage-format.md        (gap record format for spec audits)
```

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `spec` | Enter stages/01-spec-review/CONTEXT.md |
| `demo` | Enter stages/02-example-ux/CONTEXT.md |
| `parity` | Enter stages/03-visual-parity/CONTEXT.md |
| `sync` | Enter stages/04-sync-check/CONTEXT.md |
| `deliver` | Run all four stages in sequence |

### How `status` works

Scan `stages/*/output/` folders. For each stage, if the output folder contains files (other than .gitkeep), the stage is COMPLETE. Otherwise PENDING. Render:

```
Pipeline Status: treelist-delivery

  [01-spec-review]  ------>  [02-example-ux]  ------>  [03-visual-parity]  ------>  [04-sync-check]
      STATUS                     STATUS                     STATUS                    STATUS
```

## Routing

| You want to... | Go to |
|----------------|-------|
| Audit the API spec vs. implementation | stages/01-spec-review/CONTEXT.md |
| Audit and update the Example UX | stages/02-example-ux/CONTEXT.md |
| Review visual parity across themes | stages/03-visual-parity/CONTEXT.md |
| Confirm all artifacts are in sync | stages/04-sync-check/CONTEXT.md |
| Read delivery configuration | _config/delivery-context.md |
| Read gap-analysis workspace for this component | /workspaces/Marilo/workspaces/treelist-gap-analysis/CLAUDE.md |

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Spec review | _config/delivery-context.md, stages/01-spec-review/CONTEXT.md | Stages 02-04 files |
| Example UX work | _config/delivery-context.md, stages/02-example-ux/CONTEXT.md | Stages 01, 03-04 files |
| Visual parity | _config/delivery-context.md, stages/03-visual-parity/CONTEXT.md, stages/02-example-ux/output/ | Stages 01, 04 files |
| Sync check | All four stage outputs, stages/04-sync-check/shared/delivery-checklist.md | Reference files |
| Any task | _config/delivery-context.md always | Other component CDWs |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there. If you edit an output file, the next stage picks up your edits.

<!-- CDW owns: spec accuracy, Example UX completeness.
     CDW delegates: source changes and test writing to
     /workspaces/Marilo/workspaces/treelist-gap-analysis/.
     Never modify component source files directly from this workspace. -->
