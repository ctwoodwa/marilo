<!-- Target: ~800 tokens. Trim if longer. -->
# Component Delivery Workspace -- MariloScheduler

Coordinates spec accuracy, Example UX completeness, and source+tests alignment for a single complex Blazor component.

## Folder Map

```
scheduler-delivery/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (start here for task routing)
├── _config/
│   └── delivery-context.md            (component paths, state tracking, gate status)
├── _status/
│   └── workspace-status.md            (Layer 0 orientation snapshot)
├── stages/
│   ├── 01-spec-review/                (audit API spec vs. implementation)
│   │   └── output/                    (spec gap list)
│   ├── 02-example-ux/                 (audit and update demo page scenarios)
│   │   ├── shared/                    (demo scenario format)
│   │   └── output/                    (demo gap list, updated demo page)
│   └── 03-sync-check/                 (confirm all three artifacts are in sync)
│       ├── shared/                    (delivery checklist)
│       └── output/                    (delivery report)
└── shared/
    └── spec-coverage-format.md        (gap record format for spec audits)
```

## Cold Start

Load `_status/workspace-status.md` first for pipeline orientation (Layer 0 snapshot -- not authoritative).
Then load `_config/delivery-context.md` for full component paths and gate status.

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `spec` | Enter stages/01-spec-review/CONTEXT.md |
| `demo` | Enter stages/02-example-ux/CONTEXT.md |
| `sync` | Enter stages/03-sync-check/CONTEXT.md |
| `deliver` | Run all three stages in sequence |

### How `status` works

Scan `stages/*/output/` folders. For each stage, if the output folder contains files (other than .gitkeep), the stage is COMPLETE. Otherwise PENDING. Render:

```
Pipeline Status: scheduler-delivery

  [01-spec-review]  ------>  [02-example-ux]  ------>  [03-sync-check]
      STATUS                     STATUS                    STATUS
```

## Routing

| You want to... | Go to |
|----------------|-------|
| Audit the API spec vs. implementation | stages/01-spec-review/CONTEXT.md |
| Audit and update the Example UX | stages/02-example-ux/CONTEXT.md |
| Confirm all three artifacts are in sync | stages/03-sync-check/CONTEXT.md |
| Read delivery configuration | _config/delivery-context.md |
| Read gap-analysis workspace for this component | ../scheduler-gap-analysis/CLAUDE.md |
| Read global gap resolution plan (read-only) | /workspaces/Marilo/src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md |

## External Dependencies

| Dependency | Path | Access |
|------------|------|--------|
| GAP_ANALYSIS_RESOLUTION_PLAN.md | /workspaces/Marilo/src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md | Read-only reference |
| Scheduler gap workspace | ../scheduler-gap-analysis/ | Raise gaps; read closure reports |

This workspace never edits `src/` directly. It coordinates and raises gaps to `scheduler-gap-analysis`.

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Spec review | _config/delivery-context.md, stages/01-spec-review/CONTEXT.md | Stages 02-03 files |
| Example UX work | _config/delivery-context.md, stages/02-example-ux/CONTEXT.md | Stages 01, 03 files |
| Sync check | All three stage outputs, stages/03-sync-check/shared/delivery-checklist.md | Reference files |
| Any task | _config/delivery-context.md always | Other component CDWs |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there.

<!-- CDW owns: spec accuracy, Example UX completeness.
     CDW delegates: source changes and test writing to
     ../scheduler-gap-analysis/.
     Never modify component source files directly from this workspace. -->
