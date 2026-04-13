# Delivery Context -- MariloDataSheet

## Component Identity

| Field | Value |
|-------|-------|
| Component name | MariloDataSheet |
| Component slug | datasheet |
| Complexity tier | Complex (CDW warranted) |
| Active phase | Phase 1 (no prior gap work; starting fresh) |

## Artifact Paths

| Field | Value |
|-------|-------|
| API spec | /workspaces/Marilo/docs/component-specs/datasheet/ |
| Example UX | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/DataSheet/ |
| Component source | /workspaces/Marilo/src/Marilo.Components/DataGrid/ (MariloDataSheet*.cs) |
| Test files | /workspaces/Marilo/tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs |
| Gap workspace | /workspaces/Marilo/workspaces/datasheet-gap-analysis/ |

## Spec State

| Field | Value |
|-------|-------|
| Spec version | unversioned |
| Last spec audit | 2026-04-11 |
| Open spec gaps | 0 (prior blocker resolved by new datasheet/ spec; 2 cross-branch drift items escalated) |

## Example UX State

| Field | Value |
|-------|-------|
| Demo page(s) | DataSheet/Overview.razor, Editing-and-Validation.razor, BulkOperations.razor, Keyboard-and-Accessibility.razor (4 pages, 16 scenarios, ~1,488 lines) |
| Last demo audit | 2026-04-11 (Wave 2) |
| Open demo gaps | 6 (1 Missing theming-architecture-blocked, 4 Partial, 1 Blocked-by-source). Follow-ups EU-01..EU-08. |

## Visual Parity State

| Field | Value |
|-------|-------|
| Last parity review | 2026-04-11 (Wave 3, static-analysis pass -- no browser capture) |
| Themes reviewed | Fluent L/D, Bootstrap L/D, Material L/D (6 theme/mode combos) |
| Open parity gaps | 12 tracked records (VP-datasheet-01 umbrella + VP-02..VP-12) + 3 deferrals (D01 theming, D02 range selection, D03 10k rows). 8 Critical + 3 Major + 0 Minor. All providers aggregate 0.22/3 -- structural gap (zero `mar-datasheet*` SCSS rules in any provider). |

## Delivery Gate

| Field | Value |
|-------|-------|
| Last sync check | 2026-04-11 (Wave 4) |
| Gate status | BLOCKED |
| Blocking items | 7 (UD-01 datasheet-theming-architecture OPEN, UD-02 datasheet-10k-rows OPEN, VP-datasheet-01 umbrella SCSS gap with 8 Critical + 3 Major children, Wave 1 V03 range selection source model missing, Wave 1 SA-01 grid root tabindex missing, Wave 1 SA-13 aria-live announcements missing, no datasheet-gap-analysis workspace bootstrapped yet) |

## Gap Workspace Link

| Field | Value |
|-------|-------|
| Latest closure reports | /workspaces/Marilo/workspaces/datasheet-gap-analysis/stages/06-validate/output/ |
| Coverage summary | /workspaces/Marilo/workspaces/datasheet-gap-analysis/_config/coverage-summary.md |

## Spec Feature Areas

| Feature Area | Spec Path | Status |
|---|---|---|
| overview | docs/component-specs/datasheet/overview.md | COMPLETE |
| columns-and-schema | docs/component-specs/datasheet/columns-and-schema.md | COMPLETE |
| editing-and-validation | docs/component-specs/datasheet/editing-and-validation.md | COMPLETE |
| selection-and-ranges | docs/component-specs/datasheet/selection-and-ranges.md | COMPLETE |
| bulk-paste-and-clipboard | docs/component-specs/datasheet/bulk-paste-and-clipboard.md | COMPLETE |
| bulk-operations-and-saveall | docs/component-specs/datasheet/bulk-operations-and-saveall.md | COMPLETE |
| virtualization-and-performance | docs/component-specs/datasheet/virtualization-and-performance.md | COMPLETE |
| keyboard-and-accessibility | docs/component-specs/datasheet/keyboard-and-accessibility.md | COMPLETE |
| theming-and-css-provider | docs/component-specs/datasheet/theming-and-css-provider.md | COMPLETE |

Stage 01 processes one feature area at a time.
Update status to IN PROGRESS / COMPLETE per area as work proceeds.
Do not attempt to audit all feature areas in a single Stage 01 run.
