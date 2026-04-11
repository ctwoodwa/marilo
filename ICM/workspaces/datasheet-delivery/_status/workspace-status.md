# Workspace Status: DataSheet Delivery

**Last updated:** 2026-04-10 (unblocked — old 2026-04-03 status was stale after the 2026-04-09 architecture decision)

## Pipeline Status

```text
  [01-spec-review]  ------>  [01b verification]  ------>  [02-example-ux]  ------>  [03-visual-parity]  ------>  [04-sync-check]
    COMPLETE 2026-04-10       PENDING (10 sub-tasks)       PENDING                    PENDING                    PENDING
```

## Architecture Decision — Resolved 2026-04-09

The blocker from the 2026-04-03 Stage 01 audit ("Spec-Implementation Architecture Mismatch — is MariloDataSheet intended to become the full Spreadsheet, or is it a deliberately simpler component that needs its own spec?") was resolved by the 2026-04-09 human decisions session:

> **"DataSheet: True spreadsheet with its own architecture (not DataGrid reuse)"**
> — per `ICM/workspaces/gap-analysis-resolution/_status/workspace-status.md:112` "Human Decisions Resolved (2026-04-09)"

**Outcome:** Two components are now tracked as distinct deliverables:

- **`MariloSpreadsheet`** (Editors folder) — Excel-clone direction, spec at `docs/component-specs/spreadsheet/` (5 files: overview, events, functions-formulas, tools, accessibility/wai-aria-support). Tracked in the main plan's API Completion Pass table (line 59) as `COMPLETE | PRESENT_PARTIAL | Editable grid; formula engine deferred`.
- **`MariloDataSheet<TItem>`** (DataGrid folder) — strongly-typed editable data grid, spec at `docs/component-specs/datasheet/` (9 files: overview, columns-and-schema, editing-and-validation, selection-and-ranges, bulk-paste-and-clipboard, bulk-operations-and-saveall, virtualization-and-performance, keyboard-and-accessibility, theming-and-css-provider). **Owned by this delivery workspace.**

## Stage 01 — Spec Review

- **Status:** ✅ **Complete (re-run 2026-04-10)**
- **Current output:** `stages/01-spec-review/output/datasheet-spec-gaps-2026-04-10.md` — new audit against the correct spec
- **Superseded output:** `stages/01-spec-review/output/datasheet-spec-gaps.md` (2026-04-03) — marked with SUPERSEDED banner; retained for historical value
- **Headline finding:** Broad-surface alignment is excellent. 53 distinct API elements verified present (18 parameters + 11 column params + 6 event args + 2 enums + 9 methods + 7 CSS provider methods). Demo exists at `samples/Marilo.Demo/Pages/Components/DataSheet/Overview.razor` (205-line "Investment Position Editor"). **Zero confirmed missing features.**
- **Remaining:** 10 Stage 01b verification sub-tasks (GAP-DATASHEET-V01 through -V10) against the 8 feature-area detail spec files + the 2 enum value lists. Each is a small independent audit. V03 (cell range selection) has the highest probability of surfacing a real implementation gap.

## Current DataSheet Source Footprint

1,324 lines across 7 files in `src/Marilo.Components/DataGrid/`:

- `MariloDataSheet.razor` (166 lines — markup)
- `MariloDataSheet.razor.cs` (191 lines — parameters, state, lifecycle)
- `MariloDataSheet.Data.cs` (287 lines — dirty tracking, row key resolution)
- `MariloDataSheet.Editing.cs` (294 lines — cell editing, paste handling)
- `MariloDataSheet.Interop.cs` (87 lines — clipboard JS interop)
- `MariloDataSheet.Rendering.cs` (244 lines — RenderTreeBuilder)
- `MariloDataSheetColumn.razor` (55 lines — column child component)

Architecturally consistent with the DataGrid partial-class pattern (`MariloDataGrid.Data.cs`, `.Editing.cs`, `.Interop.cs`, `.Rendering.cs`).

## Key Open Issues (post-2026-04-09)

1. ~~Resolve architecture direction: Spreadsheet vs DataSheet~~ → **RESOLVED 2026-04-09:** both tracked as distinct components
2. ~~If DataSheet: write new spec for actual API surface~~ → **RESOLVED:** new spec at `docs/component-specs/datasheet/` (9 files)
3. ~~If Spreadsheet: plan phased XLSX engine implementation~~ → **DEFERRED:** MariloSpreadsheet formula engine tracked as deferred work in main plan's API Completion Pass table
4. **Re-run Stage 01 spec review** against the new `docs/component-specs/datasheet/` spec to produce a superseding gaps file
5. **Sanity-check the 9 new spec files** against the current 1,324-line source for coverage (every documented feature in spec has an implementation touch point; every non-trivial implementation surface has a spec reference)

## Next Trigger

Run Stage 01b — execute the 10 verification sub-tasks from `datasheet-spec-gaps-2026-04-10.md`. Each sub-task is an independent small session. The highest-priority one is **V03 (cell range selection)** — the source has row selection but cell range selection (shift-click, drag-select, Ctrl-click) is unverified and likely to surface a real gap. V07 (keyboard and accessibility) is a close second for thoroughness. The other eight are lower-risk verification passes that can run in parallel.

**Skipping Stage 02 prioritization** is recommended because the gap surface is already tiny (10 verification sub-tasks vs Scheduler's 32 or TreeList's 43 implementation gaps). The effective pipeline becomes: `01 → 01b → 03 → 05 → 06`.
