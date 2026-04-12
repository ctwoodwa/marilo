# Gap Resolution Context -- MariloDataSheet

## Target Project

| Field | Value |
|-------|-------|
| Project name | Marilo.Components |
| Project path | /workspaces/Marilo/src/Marilo.Components |
| Component source root | `src/Marilo.Components/DataGrid/MariloDataSheet.*` (shares folder with MariloDataGrid) |
| Technology stack | .NET 10 / Blazor / C# / Razor Class Library |
| Repository URL | https://github.com/ctwoodwa/Marilo |

## Gap Analysis Source

| Field | Value |
|-------|-------|
| Entry path | `stages/01-intake/output/gap-inventory.md` |
| Source files | Wave 4 delivery report + Wave 1/2/3 stage outputs + tick-8 decisions record |
| Analysis date | 2026-04-11 |
| Scope | `systematic` (cross-cutting gaps across 4 sync areas: source, spec, demo, provider SCSS) |
| Mode | Assess (fresh bootstrap — no prior gap-analysis workspace existed for DataSheet) |

### Specific input files

- `ICM/workspaces/datasheet-delivery/stages/04-sync-check/output/datasheet-delivery-report.md` (Wave 4)
- `ICM/workspaces/datasheet-delivery/stages/01-spec-review/output/datasheet-spec-gap-list.md` (Wave 1)
- `ICM/workspaces/datasheet-delivery/stages/02-example-ux/output/datasheet-example-ux-gap-list.md` (Wave 2)
- `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-visual-parity-gaps.md` (Wave 3)
- `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-parity-summary.md` (Wave 3 summary)
- `.claude/orchestration/_orchestrator/decisions/tick-8-2026-04-11-1830.md` (UD-01 + UD-02 resolutions)

## Target State

DataSheet parity matches MariloDataGrid's delivery quality bar:

1. Every `[Parameter]` documented in `docs/component-specs/datasheet/` is implemented in source and covered by a bUnit test.
2. Every spec topic has a demo scenario in `samples/Marilo.Demo/Pages/Components/DataSheet/` (9 topics covered).
3. Every `mar-datasheet*` BEM class emitted by `MariloDataSheet.razor` / `MariloDataSheet.Rendering.cs` is styled by the provider (FluentUI primary, Bootstrap bridge, Material stub) in both light and dark modes.
4. Every user-facing state scores >= 2 on the visual-parity rubric for all 3 providers × 2 modes (54 scoring points total, same matrix as `datasheet-parity-summary.md`).
5. Rectangular range selection, `Ctrl+A`, `tabindex=0` grid root, and the 3 missing `aria-live` announcements are implemented.
6. Demo virtualization scenarios cap at 5,000 rows with a spec note that 10k is tested-but-not-demoed (pending Phase B JS interop virtualization).

## Resolution Scope

| Field | Value |
|-------|-------|
| Area/module | DataGrid folder (DataSheet component) |
| Component | MariloDataSheet |
| Total gaps identified | 39 actionable + 3 tracked deferrals + 6 CLEAR-of-record = 48 records |
| Total gaps resolved | 0 (intake just completed) |
| Test coverage status | Unknown — no bUnit per-parameter coverage audit has been run for DataSheet; Wave 4 checklist 3.1 BLOCKED pending audit |
| Active phase | Phase 1 / Stage 01 COMPLETE — Stage 02 (prioritize) next |

### Orchestrator-only lanes (not in this workspace's remediation pipeline)

| Lane | Origin | Why orchestrator-only | Status |
|---|---|---|---|
| `IDataSheetTheme` surface definition (L1) | tick-8 UD-01 | provider-contract change, orchestrator-only per `marilo.json` | Pending orchestrator dispatch |
| FluentUI provider `IDataSheetTheme` impl (L2) | tick-8 UD-01 | provider implementation mirroring DataGrid SCSS layer | Pending L1 |
| Material provider 5-line stub (L3) | tick-8 UD-01 | Material-runtime stub pattern | Pending L1 |

These lanes unblock VP-datasheet-01 (umbrella) and all 10 child records (VP-datasheet-02 through -11, except -08 and -12 which are folded / source-blocked). They do NOT flow through this workspace's Stage 02-06 pipeline.

## Resolution Tracking

| Stage | Status | Output |
|-------|--------|--------|
| 01-intake | **COMPLETE** (2026-04-11) | `stages/01-intake/output/gap-inventory.md` (39 actionable + 3 deferrals + 6 CLEAR) |
| 02-prioritize | **COMPLETE** (2026-04-11) | `stages/02-prioritize/output/datasheet-priority-lanes.md` (38 actionable in 14 lanes + 9 VP sub-lanes, 5 phases) |
| 03-resolution-design | not started | -- |
| 04-remediation-plan | not started | -- |
| 05-implement | not started | -- |
| 06-validate | not started | -- |

## Links

| Field | Value |
|-------|-------|
| Delivery workspace | ../datasheet-delivery/ |
| Delivery context | ../datasheet-delivery/_config/delivery-context.md |
| Tick-8 decisions | ../../../.claude/orchestration/_orchestrator/decisions/tick-8-2026-04-11-1830.md |
| Orchestration rules | ../../../.claude/rules/orchestration.md |
| Marilo project config | ../../../.claude/orchestration/_memory/projects/marilo.json |
