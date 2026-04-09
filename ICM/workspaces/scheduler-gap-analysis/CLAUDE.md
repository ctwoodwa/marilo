<!-- Target: ~800 tokens. Trim if longer. -->
# Component Gap-Analysis Workspace -- MariloScheduler

Structured gap tracking and resolution for MariloScheduler.
This workspace owns Scheduler-specific gaps only.

## Folder Map

```
scheduler-gap-analysis/
├── CLAUDE.md                          (you are here)
├── CONTEXT.md                         (start here for task routing)
├── _config/
│   ├── gap-context.md                 (component paths, resolution tracking)
│   └── coverage-summary.md            (test coverage rollup)
├── _status/
│   └── workspace-status.md            (Layer 0 orientation snapshot)
├── stages/
│   ├── 01-intake/                     (import or assess gaps)
│   │   └── output/
│   ├── 02-prioritize/                 (score and sequence gaps)
│   │   └── output/
│   ├── 03-resolution-design/          (design fix for each gap)
│   │   └── output/
│   ├── 04-remediation-plan/           (break into atomic tasks)
│   │   └── output/
│   ├── 05-implement/                  (execute changes)
│   │   └── output/
│   └── 06-validate/                   (verify and close gaps)
│       └── output/
└── shared/
    └── gap-record-format.md           (normalized gap shape)
```

## Entry Paths

| Starting from... | Begin at |
|-------------------|----------|
| Existing gap list (from delivery spec review) | stages/01-intake/CONTEXT.md (import mode) |
| Fresh component (no prior gap work) | stages/01-intake/CONTEXT.md (assess mode) |

Gaps are created when scheduler-delivery Stage 01 or Stage 03 detects spec/source/demo/test mismatches.
The pipeline follows the same 6-stage pattern as the main gap-analysis-resolution workspace.

## Cold Start

Load `_status/workspace-status.md` first for pipeline orientation.
Then load `_config/gap-context.md` for full component paths and resolution state.

## Triggers

| Keyword | Action |
|---------|--------|
| `setup` | Run onboarding questionnaire |
| `status` | Show pipeline completion for all stages |
| `ingest` | Enter stages/01-intake/CONTEXT.md |
| `prioritize` | Enter stages/02-prioritize/CONTEXT.md |
| `resolve` | Enter stages/03-resolution-design/CONTEXT.md |
| `plan` | Enter stages/04-remediation-plan/CONTEXT.md |
| `implement` | Enter stages/05-implement/CONTEXT.md |
| `close` | Enter stages/06-validate/CONTEXT.md |

### How `status` works

Scan `stages/*/output/` folders. For each stage, if the output folder contains files (other than .gitkeep), the stage is COMPLETE. Otherwise PENDING. Render:

```
Pipeline Status: scheduler-gap-analysis

  [01-intake] --> [02-prioritize] --> [03-resolution-design] --> [04-remediation-plan] --> [05-implement] --> [06-validate]
    STATUS           STATUS                STATUS                     STATUS                  STATUS           STATUS
```

## Routing

| You want to... | Go to |
|----------------|-------|
| Import or discover gaps | stages/01-intake/CONTEXT.md |
| Score and sequence gaps | stages/02-prioritize/CONTEXT.md |
| Design resolution for each gap | stages/03-resolution-design/CONTEXT.md |
| Break resolutions into tasks | stages/04-remediation-plan/CONTEXT.md |
| Execute implementation | stages/05-implement/CONTEXT.md |
| Verify and close gaps | stages/06-validate/CONTEXT.md |
| Read gap configuration | _config/gap-context.md |
| Read delivery workspace | ../scheduler-delivery/CLAUDE.md |

## External Dependencies

| Dependency | Path | Access |
|------------|------|--------|
| GAP_ANALYSIS_RESOLUTION_PLAN.md | /workspaces/Marilo/src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md | Read-only reference |
| Delivery workspace | ../scheduler-delivery/ | Receives gap triggers from Stages 01/03 |

New Scheduler gaps are tracked locally in this workspace. Optionally mirror into the global plan if needed.

## What to Load

| Task | Load These | Do NOT Load |
|------|-----------|-------------|
| Intake | _config/gap-context.md, stages/01-intake/CONTEXT.md | Stages 02-06 |
| Prioritize | _config/gap-context.md, stages/01-intake/output/, stages/02-prioritize/CONTEXT.md | Stages 03-06 |
| Resolution design | stages/02-prioritize/output/, stages/03-resolution-design/CONTEXT.md | Stages 01, 04-06 |
| Remediation plan | stages/03-resolution-design/output/, stages/04-remediation-plan/CONTEXT.md | Stages 01-02, 05-06 |
| Implement | stages/04-remediation-plan/output/, stages/05-implement/CONTEXT.md | Stages 01-03, 06 |
| Validate | stages/05-implement/output/, stages/06-validate/CONTEXT.md, shared/gap-record-format.md | Stages 01-04 |
| Any task | _config/gap-context.md always | Other component workspaces |

## Stage Handoffs

Each stage writes its output to its own `output/` folder. The next stage reads from there.

## Global Constraints

1. **Traceability:** Every resolution must trace back to a gap ID from Stage 01.
2. **Code-first:** Read actual source before designing resolutions; never resolve from spec alone.
3. **Append-only records:** Gap records are never deleted, only status-updated.
4. **One-gap-one-decision:** Each gap gets exactly one resolution decision (even if batched for implementation).

<!-- Gap workspace owns: gap discovery, resolution design, implementation.
     Delivery workspace owns: spec accuracy, Example UX completeness.
     Never modify spec or demo files from this workspace. -->
