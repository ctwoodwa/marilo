# Gap Resolution Context -- MariloFileManager

## Target Component

| Field | Value |
|-------|-------|
| Component | MariloFileManager |
| Source path | `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor` |
| Spec path | `docs/component-specs/filemanager/` (15 files) |
| Technology stack | .NET 10 / Blazor / C# / Razor Class Library |
| Active phase | Stage 01 (intake from CDW spec review) |

## Gap Analysis Source

| Field | Value |
|-------|-------|
| Entry path | existing (CDW spec review) |
| Source file | `filemanager-delivery/stages/01-spec-review/output/filemanager-spec-gap-list.md` |
| Analysis date | 2026-04-09 |
| Scope | systematic (cross-cutting gaps across 13 feature areas) |

## Resolution Scope

| Field | Value |
|-------|-------|
| Total gaps identified | 36 |
| Undocumented (source-only) | 4 |
| Spec-ahead (not implemented) | 28 |
| Mismatch (naming/type/behavior) | 4 |
| P1 — Blocking | 5 (generic TItem, Data rename, Path rename, OnRead, event args types) |
| P2 — This phase | 26 (context menu, preview pane, toolbar, breadcrumb, upload, search, sort, templates, appearance) |
| P3 — Next phase | 5 (ARIA, nice-to-have polish) |

## Resolution Tracking

| Stage | Status | Output |
|-------|--------|--------|
| 01-intake | complete | `filemanager-delivery/stages/01-spec-review/output/filemanager-spec-gap-list.md` |
| 02-prioritize | complete | `output/stage-02/filemanager-prioritized-backlog.md` |
| 03-resolution-design | Phase A complete | `output/stage-03/filemanager-phase-a-resolutions.md` |
| 04-remediation-plan | skipped (batched) | — |
| 05-implement | Phase A complete | `output/stage-05/filemanager-phase-a-implementation-log.md` |
| 06-validate | Phase A complete | `output/stage-06/filemanager-phase-a-closure-report.md` |

## Test Coverage Rollup

| Batch | Tests written | Tests passing | Coverage notes |
|-------|--------------|---------------|----------------|
| (none yet) | 0 | 0 | Awaiting implementation |

## Architecture Notes

The current source is a 170-line single-file component with:
- Concrete `FileManagerEntry` class (not generic)
- Grid/List views, folder tree sidebar, basic navigation
- 12 parameters, no code-behind

The spec requires:
- Generic `MariloFileManager<TItem>` with 14 field-binding string parameters
- Composite child components (`FileManagerToolBar`, `FileManagerSettings`, `FileManagerUploadSettings`)
- Context menu, preview pane, breadcrumb navigation, search, sort
- `OnRead` event for server-side data loading
- Full event model with typed `EventArgs` classes

This is a **full rewrite** similar to MariloGantt (which went from 95-line scaffold to complete component).
