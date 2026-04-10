# MariloFileManager — Demo Gap List

**Audit date:** 2026-04-10
**Existing demo page:** `samples/Marilo.Demo/Pages/Components/FileManager/Overview.razor`
**Current scenario count:** 1 (empty basic usage only)
**Target scenario count:** 12

---

## Current Coverage

| # | Existing Scenario | Parameters Covered | Events Covered |
|---|---|---|---|
| 1 | Basic Usage | TItem, Style (hardcoded) | None |

---

## Demo Gaps

### Category A — Missing scenarios for implemented parameters

| # | Gap | Parameter(s) | Priority |
|---|-----|-------------|----------|
| A1 | No Data binding scenario | Data, field mapping strings | P1 |
| A2 | No View mode toggle | View, ViewChanged | P1 |
| A3 | No folder tree toggle | ShowFolderTree | P2 |
| A4 | No preview pane scenario | ShowPreviewPane | P2 |
| A5 | No sizing scenario | Height, Width | P3 |
| A6 | No loader container scenario | EnableLoaderContainer | P3 |

### Category B — Missing scenarios for implemented events

| # | Gap | Event(s) | Priority |
|---|-----|---------|----------|
| B1 | No navigation scenario | Path, PathChanged, OnOpen | P1 |
| B2 | No selection scenario | SelectedItems, SelectedItemsChanged, OnSelect | P1 |
| B3 | No CRUD operations scenario | AllowCreate, AllowDelete, AllowRename, OnCreate, OnDelete, OnEdit, OnUpdate | P1 |
| B4 | No upload scenario | UploadSettings | P2 |
| B5 | No download scenario | OnDownload | P2 |

### Category C — Missing scenarios for interactive features

| # | Gap | Feature | Priority |
|---|-----|---------|----------|
| C1 | No search/filter demo | Search box in toolbar | P2 |
| C2 | No sort demo | Column header sorting | P2 |
| C3 | No context menu demo | Right-click menu | P2 |
| C4 | No OnRead (server-side data) demo | OnRead event | P2 |

### Category D — Missing edge cases

| # | Gap | Scenario | Priority |
|---|-----|---------|----------|
| D1 | No empty state demo | Empty Data collection | P3 |
| D2 | No custom toolbar demo | ToolBarTemplate | P3 |

---

## Proposed Demo Sections (12 scenarios)

| # | Section Title | Gaps Covered | Interactive Controls |
|---|---|---|---|
| 1 | Basic File Browser | A1, B1 | Path display, breadcrumb navigation |
| 2 | View Modes | A2 | Toggle between Grid and List |
| 3 | File Selection | B2 | Click to select, display selected item |
| 4 | CRUD Operations | B3 | Create folder, rename, delete buttons |
| 5 | Folder Tree & Preview Pane | A3, A4 | Checkboxes to toggle tree/preview |
| 6 | Search & Sort | C1, C2 | Type in search box, click column headers |
| 7 | File Upload | B4 | Upload button with settings |
| 8 | Context Menu & Download | C3, B5 | Right-click on file |
| 9 | Server-Side Data (OnRead) | C4 | Simulated async data loading |
| 10 | Sizing & Loading | A5, A6 | Height/Width inputs, loading toggle |
| 11 | Custom Toolbar | D2 | Custom toolbar template |
| 12 | Empty State | D1 | Empty data source |

---

## Decision

All 12 scenarios target **implemented features only** — no spec-ahead parameters. Sample data uses the built-in `FileManagerEntry` model.
