# Rename/Removal Cleanup Report

**Date:** 2026-04-03
**Scope:** Repo-wide Grid/DataGrid/Table/GridLayout naming alignment

## Canonical Decisions Applied

1. **Demo path:** `samples/Marilo.Demo/Pages/Components/DataGrid` is the canonical path. The duplicate nested `DataGrid/DataGrid` has been flattened.
2. **MariloTable:** Removed as obsolete. Superseded by `MariloDataGrid`. No active references remain.
3. **MariloGrid → MariloGridLayout:** Layout grid component renamed. File renamed from `MariloGrid.razor` to `MariloGridLayout.razor`. All active references updated.
4. **Conceptual separation maintained:**
   - `MariloDataGrid` = typed business data grid
   - `MariloDataSheet` = typed bulk-edit sheet (no refs found — not yet implemented)
   - `MariloSpreadsheet` = workbook/worksheet/formula spreadsheet (spec docs exist)
   - `MariloGridLayout` = layout grid system

## Inventory

### Demo
- [x] `samples/Marilo.Demo/Pages/Components/DataGrid/DataGrid/*.razor` — Moved 4 files up to `DataGrid/`, removed empty nested folder
- [x] `samples/Marilo.Demo/Pages/Components/Grid/Overview.razor` — Updated all `<MariloGrid>` to `<MariloGridLayout>` (21 occurrences)
- [x] `samples/Marilo.Demo/Pages/Components/GridLayout/Overview.razor` — Replaced "coming soon" placeholder with real MariloGridLayout CSS Grid demo
- [x] `samples/Marilo.Demo.FluentUI/Pages/LayoutPage.razor` — Updated `<MariloGrid>` to `<MariloGridLayout>` (4 occurrences)
- [x] `samples/Marilo.Demo.FluentUI/Pages/DataDisplayPage.razor` — Replaced `<MariloTable>` with `<table class="mar-table">`
- [x] `samples/Marilo.Demo.Bootstrap/Pages/DataDisplayPage.razor` — Replaced `<MariloTable>` with `<table class="mar-table">`

### Source
- [x] `src/Marilo.Components/DataDisplay/MariloTable.razor` — **Deleted**
- [x] `src/Marilo.Components/Layout/MariloGrid.razor` — **Renamed** to `MariloGridLayout.razor`
- [x] `src/Marilo.Components/Layout/MariloGridLayoutColumn.razor` — Updated `CascadingParameter` type from `MariloGrid` to `MariloGridLayout`
- [x] `src/Marilo.Components/Layout/MariloGridLayoutRow.razor` — Updated `CascadingParameter` type from `MariloGrid` to `MariloGridLayout`
- [x] `src/Marilo.Components/Layout/MariloGridLayoutItem.razor` — No change needed (no MariloGrid reference)

### Docs/Specs/Mappings
- [x] `docs/component-specs/component-mapping.json` — `"grid"` updated from `"MariloTable"` to `"MariloDataGrid"` (status: `"implemented"`); `"gridlayout"` updated from `"MariloGrid"` to `"MariloGridLayout"`
- [x] `docs/component-specs/README.md` — Updated grid row from `MariloTable | Partial` to `MariloDataGrid | Implemented`
- [x] `README.md` — Updated Layout table: `MariloGrid` → `MariloGridLayout`; Display table: `MariloTable` → `MariloDataGrid`
- [x] `docs/component-specs/grid/**` — **No change** (Telerik-convention data grid spec reference docs; mapping file correctly points `"grid"` → `"MariloDataGrid"`)
- [x] `docs/component-specs/gridlayout/**` — **No change** (already uses `MariloGridLayout` correctly)

### Tests
- [x] `tests/Marilo.Tests.Unit/Foundation/GridLayoutTests.cs` — Updated all `Render<MariloGrid>` to `Render<MariloGridLayout>`; renamed test methods (`MariloGrid_*` → `MariloGridLayout_*`)

### Workspaces/Config (Historical — annotated, not rewritten)
- [x] `src/Marilo.Components/GAP_ANALYSIS_INDEX.md` — Added `**OBSOLETE**` note to MariloTable row
- [x] `src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md` — Added `**OBSOLETE**` note to MariloTable row
- [x] `src/Marilo.Components/DataDisplay/GAP_ANALYSIS_PART2.md` — Added deprecation note at top
- [x] `src/Marilo.Components/Layout/GAP_ANALYSIS_PART1.md` — Added rename note at top
- [x] `src/Marilo.Components/Layout/resolution/RESOLUTION_STATUS.md` — Updated frontmatter component list; added rename note
- [x] `src/Marilo.Components/Layout/resolution/IMPLEMENTATION_NOTES.md` — Added rename note at top
- [x] `workspaces/gap-analysis-resolution/stages/03-resolution-design/output/gap-grid-resolutions.md` — Added rename note, updated title
- [x] `workspaces/gap-analysis-resolution/stages/05-implement/output/gap-grid-implementation-log.md` — Added rename note, updated title
- [x] `workspaces/gap-analysis-resolution/stages/06-validate/output/gap-grid-closure-report.md` — Added rename note, updated title

## Files Changed

| Path | Change Type | Reason |
|------|-------------|--------|
| `src/Marilo.Components/DataDisplay/MariloTable.razor` | delete | Obsolete; superseded by MariloDataGrid |
| `src/Marilo.Components/Layout/MariloGrid.razor` → `MariloGridLayout.razor` | rename | Canonical decision #3 |
| `src/Marilo.Components/Layout/MariloGridLayoutColumn.razor` | update | CascadingParameter type: MariloGrid → MariloGridLayout |
| `src/Marilo.Components/Layout/MariloGridLayoutRow.razor` | update | CascadingParameter type: MariloGrid → MariloGridLayout |
| `samples/Marilo.Demo/Pages/Components/DataGrid/DataGrid/*.razor` (4 files) | move | Flatten duplicate nested path |
| `samples/Marilo.Demo/Pages/Components/Grid/Overview.razor` | update | MariloGrid → MariloGridLayout in component tags and code strings |
| `samples/Marilo.Demo/Pages/Components/GridLayout/Overview.razor` | update | Replace placeholder with real CSS Grid Layout demo |
| `samples/Marilo.Demo.FluentUI/Pages/LayoutPage.razor` | update | MariloGrid → MariloGridLayout |
| `samples/Marilo.Demo.FluentUI/Pages/DataDisplayPage.razor` | update | MariloTable → plain HTML table |
| `samples/Marilo.Demo.Bootstrap/Pages/DataDisplayPage.razor` | update | MariloTable → plain HTML table |
| `docs/component-specs/component-mapping.json` | update | grid→MariloDataGrid, gridlayout→MariloGridLayout |
| `docs/component-specs/README.md` | update | grid→MariloDataGrid |
| `README.md` | update | Layout: MariloGridLayout; Display: MariloDataGrid |
| `tests/Marilo.Tests.Unit/Foundation/GridLayoutTests.cs` | update | MariloGrid → MariloGridLayout in types and method names |
| `src/Marilo.Components/GAP_ANALYSIS_INDEX.md` | update | OBSOLETE note on MariloTable |
| `src/Marilo.Components/GAP_ANALYSIS_RESOLUTION_PLAN.md` | update | OBSOLETE note on MariloTable |
| `src/Marilo.Components/DataDisplay/GAP_ANALYSIS_PART2.md` | update | Deprecation note |
| `src/Marilo.Components/Layout/GAP_ANALYSIS_PART1.md` | update | Rename note |
| `src/Marilo.Components/Layout/resolution/RESOLUTION_STATUS.md` | update | Frontmatter + rename note |
| `src/Marilo.Components/Layout/resolution/IMPLEMENTATION_NOTES.md` | update | Rename note |
| `workspaces/.../gap-grid-resolutions.md` | update | Rename note + title |
| `workspaces/.../gap-grid-implementation-log.md` | update | Rename note + title |
| `workspaces/.../gap-grid-closure-report.md` | update | Rename note + title |

## Validation

### Searches Rerun
| Search Pattern | Active References Found | Status |
|---|---|---|
| `MariloTable` | 0 active (5 historical with OBSOLETE/Note annotations) | CLEAN |
| `\bMariloGrid\b` (exact match, not MariloGridLayout) | 0 active source/demo/test refs; ~80 in docs/component-specs/grid/** (data grid specs, correct); ~15 in historical workspace files (annotated) | CLEAN |
| `Pages/Components/DataGrid/DataGrid` | 0 | CLEAN |

### Build/Test Commands
| Command | Result |
|---|---|
| `dotnet build src/Marilo.Components/Marilo.Components.csproj` | **Build succeeded** — 0 warnings, 0 errors |
| `dotnet test tests/Marilo.Tests.Unit --filter GridLayout` | **Passed** — 10/10 tests, 0 failures |

## Remaining Blockers

None. All cleanup items completed successfully.

### Intentional Historical References Left Unchanged
- `docs/component-specs/grid/**` (~90 files) — Use `MariloGrid` as the Telerik-convention data grid name in spec examples. These are read-only reference specs. The `component-mapping.json` correctly maps `"grid"` → `"MariloDataGrid"`.
- Historical gap analysis/resolution files in `src/Marilo.Components/` and `workspaces/` — Retain original `MariloGrid`/`MariloTable` wording for audit accuracy, each annotated with a dated note explaining the rename/removal.

## Final Confirmation

The repo is now free of active references to:
- **MariloTable** as an active component (deleted; no source/demo/test/mapping refs)
- **MariloGrid** as the layout component name (renamed to MariloGridLayout; all active refs updated)
- **The duplicate DataGrid demo path** `Pages/Components/DataGrid/DataGrid` (flattened to `DataGrid/`)

The four conceptual components remain cleanly separated:
- `MariloDataGrid` — typed business data grid
- `MariloDataSheet` — typed bulk-edit sheet (not yet implemented)
- `MariloSpreadsheet` — workbook/worksheet/formula spreadsheet (spec only)
- `MariloGridLayout` — layout grid system (with MariloGridLayoutRow, MariloGridLayoutColumn, MariloGridLayoutItem)
