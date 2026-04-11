# DataSheet Spec Gap List

> ⚠ **SUPERSEDED AS OF 2026-04-10** — this audit is against the **wrong spec**.
>
> This file audits `MariloDataSheet<TItem>` against `docs/component-specs/spreadsheet/` (the MariloSpreadsheet Excel-clone spec). The 2026-04-09 architecture decision ruled that `MariloDataSheet` is a **distinct component** from `MariloSpreadsheet` and has its own new spec at `docs/component-specs/datasheet/` (9 files).
>
> **This file should be superseded by a new Stage 01 audit** pairing `MariloDataSheet*` source against `docs/component-specs/datasheet/`. See `_status/workspace-status.md` "Stage 01 — Spec Review" for the re-run plan.
>
> **Retained for historical value:** The "(A) Implemented API Surface" section below remains accurate for the DataSheet source as of 2026-04-03 and is useful as a starting reference for the re-run. The "(B) Spec-Ahead Features" and "(C) Undocumented Features" sections apply to the Spreadsheet/DataSheet mis-pairing and should be discarded by the re-run.

---

**Audit Date:** 2026-04-03 (superseded 2026-04-10)
**Component:** MariloDataSheet<TItem>
**Spec Directory Audited:** `/workspaces/Marilo/docs/component-specs/spreadsheet/` ⚠ **WRONG — should be** `/workspaces/Marilo/docs/component-specs/datasheet/`
**Source Directory:** /workspaces/Marilo/src/Marilo.Components/DataGrid/MariloDataSheet*.cs

---

## Critical Observation: Spec-Implementation Architecture Mismatch

The spec documents a **full Excel-like Spreadsheet** (`MariloSpreadsheet`) with XLSX file import/export, cell-level styling, formulas, named ranges, images, links, and a toolbar with formatting tools.

The implementation (`MariloDataSheet<TItem>`) is a **strongly-typed editable data grid** with column definitions, cell-level dirty tracking, bulk save, and paste support. It works with `IEnumerable<TItem>` data, not XLSX byte arrays.

**These are fundamentally different components.** The spec describes an Excel clone; the implementation is an inline-editing DataGrid variant. This mismatch must be resolved at a design level before detailed spec alignment can proceed.

**Decision needed:** Is MariloDataSheet intended to eventually become the full Spreadsheet from spec, or is it a deliberately simpler component that needs its own spec?

---

## Summary

| Type | Count |
|------|-------|
| Architecture mismatch (spec ≠ implementation) | 1 (blocking) |
| Spec-ahead (documented, not implemented) | ~25 |
| Undocumented (implemented, not in spec) | ~12 |
| **Total** | **~38** |

| Severity | Count |
|----------|-------|
| Blocking | 1 (architecture decision) |
| Important | ~15 |
| Nice-to-have | ~22 |

---

## (A) Implemented API Surface (from source)

### Parameters (MariloDataSheet.razor.cs)
| Parameter | Type | Default | In Spec? |
|-----------|------|---------|----------|
| Data | IEnumerable<TItem>? | null | Mismatch (spec: byte[]) |
| KeyField | string | "Id" | No |
| OnSaveAll | EventCallback<DataSheetSaveArgs<TItem>> | — | No |
| OnRowChanged | EventCallback<DataSheetRowChangedArgs<TItem>> | — | No |
| OnValidate | EventCallback<DataSheetValidateArgs<TItem>> | — | No |
| IsSaving | bool | false | No |
| AllowAddRow | bool | false | No |
| AllowDeleteRow | bool | false | No |
| AllowBulkPaste | bool | true | No |
| EmptyStateMessage | string | "No data." | No |
| Height | string? | null | Yes (different semantics) |
| IsLoading | bool | false | Partial (EnableLoaderContainer in spec) |
| EnableVirtualization | bool | true | No |
| AriaLabel | string | "Editable data grid" | No |
| ChildContent | RenderFragment? | null | No (spec uses Data byte[]) |
| ToolbarTemplate | RenderFragment? | null | Partial (spec uses SpreadsheetToolSet) |

### Methods
| Method | In Spec? |
|--------|----------|
| GetDirtyRows() | No |
| SetDataAsync(IEnumerable<TItem>) | No |

### Partial Files
| File | Purpose |
|------|---------|
| MariloDataSheet.Data.cs | Dirty tracking, row key resolution |
| MariloDataSheet.Editing.cs | Cell editing, paste handling |
| MariloDataSheet.Interop.cs | JS interop (clipboard) |
| MariloDataSheet.Rendering.cs | RenderTreeBuilder logic |

### Column Component: MariloDataSheetColumn<TItem>
Provides column definitions with field binding, header text, width, editability, and cell templates.

---

## (B) Spec-Ahead Features (in spec, not in implementation)

### Core Configuration (from spec overview.md)
| Parameter | Spec Type | Severity | Notes |
|-----------|-----------|----------|-------|
| Data | byte[] (XLSX) | **Blocking** | Spec: Excel file bytes. Code: IEnumerable<TItem> |
| ColumnsCount | int (50) | Important | Dynamic column count |
| RowsCount | int (200) | Important | Dynamic row count |
| ColumnWidth | double (64) | Important | Default column width |
| RowHeight | double (20) | Important | Default row height |
| ColumnHeaderHeight | double (20) | Nice-to-have | Header height |
| RowHeaderWidth | double (32) | Nice-to-have | Row header width |
| EnableLoaderContainer | bool | Nice-to-have | Loading state |
| Width | string | Nice-to-have | Root width |
| Tools | SpreadsheetToolSet | Important | Toolbar configuration |

### Functions & Formulas (from spec functions-formulas.md)
| Feature | Severity | Notes |
|---------|----------|-------|
| Cell formulas (=SUM, =AVERAGE, etc.) | Important | ~200+ Excel functions documented |
| Named ranges | Nice-to-have | |
| Cross-sheet references | Nice-to-have | |

### Tools (from spec tools.md)
| Feature | Severity | Notes |
|---------|----------|-------|
| Built-in toolbar tabs (Home, Insert, Data, View) | Important | |
| Cell formatting (bold, italic, borders, fill) | Important | |
| Number formatting | Important | |
| Merge/unmerge cells | Nice-to-have | |
| Image insertion | Nice-to-have | |
| Link insertion | Nice-to-have | |
| Custom tool support | Nice-to-have | |

### Events (from spec events.md)
| Event | Severity | Notes |
|-------|----------|-------|
| OnCellSelect | Important | Cell selection callback |
| OnCellEdit | Important | Cell edit callback |
| OnSheetRename | Nice-to-have | Sheet management |
| OnDataExport | Important | Export callback |

### Accessibility (from spec accessibility/wai-aria-support.md)
| Feature | Severity | Notes |
|---------|----------|-------|
| Full WAI-ARIA grid pattern | Important | role=grid, role=gridcell, aria-rowindex, etc. |
| Screen reader announcements | Important | Cell navigation announcements |
| Keyboard shortcuts (Ctrl+C, Ctrl+V, Tab, Enter) | Important | Excel-like keyboard model |

---

## (C) Spec Ambiguities Requiring Human Clarification

1. **Architecture direction:** Is MariloDataSheet meant to become the full Spreadsheet from spec (XLSX-based), or is it intentionally a different, simpler component?
   - If yes: the gap set is ~50+ items and requires a formula engine, XLSX parser, cell styling system.
   - If no: the spec needs to be rewritten for the actual MariloDataSheet API surface.

2. **Naming:** Spec calls it "MariloSpreadsheet"; code calls it "MariloDataSheet". Are these the same component?

3. **Data model:** Spec uses `byte[]` (XLSX file); code uses `IEnumerable<TItem>` (strongly typed). These serve fundamentally different use cases.

---

## Gaps Raised in Gap Workspace

Given the architecture mismatch, no individual gaps were raised in the gap workspace. The blocking decision (architecture direction) must be resolved first.

---

## Next Recommended Trigger

1. **Human decision:** Resolve the architecture question (Spreadsheet vs DataSheet).
2. If DataSheet is its own component: write a new spec for its actual API surface, then re-run Stage 01.
3. If DataSheet should become Spreadsheet: plan the phased implementation in gap-analysis-resolution with XLSX engine as Phase 1.
