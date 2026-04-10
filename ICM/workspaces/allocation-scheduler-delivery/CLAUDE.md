# Component Delivery Workspace -- MariloAllocationScheduler

Coordinates spec accuracy, Example UX completeness, and source+tests alignment for the AllocationScheduler component.

## Folder Map

```
allocation-scheduler-delivery/
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
│   ├── 03-visual-parity/              (theme-aware visual comparison and gap scoring)
│   │   ├── shared/                    (capture matrix, rubric, gap format, remediation template)
│   │   └── output/                    (parity gaps, parity summary)
│   └── 04-sync-check/                 (confirm all four artifacts are in sync)
│       ├── shared/                    (delivery checklist)
│       └── output/                    (delivery report)
└── shared/
    └── spec-coverage-format.md        (gap record format for spec audits)
```

## Triggers

| Keyword | Action |
|---------|--------|
| `status` | Show pipeline completion for all stages |
| `spec` | Enter stages/01-spec-review/CONTEXT.md |
| `demo` | Enter stages/02-example-ux/CONTEXT.md |
| `parity` | Enter stages/03-visual-parity/CONTEXT.md |
| `sync` | Enter stages/04-sync-check/CONTEXT.md |

## Routing

| You want to... | Go to |
|----------------|-------|
| Audit the API spec vs. implementation | stages/01-spec-review/CONTEXT.md |
| Audit and update the Example UX | stages/02-example-ux/CONTEXT.md |
| Review visual parity across themes | stages/03-visual-parity/CONTEXT.md |
| Confirm all four artifacts are in sync | stages/04-sync-check/CONTEXT.md |

## IMPORTANT

- This is MariloAllocationScheduler (resource allocation), NOT MariloScheduler (calendar scheduling).
- Do NOT load scheduler-delivery, scheduler-gap-analysis, or MariloScheduler source files.
