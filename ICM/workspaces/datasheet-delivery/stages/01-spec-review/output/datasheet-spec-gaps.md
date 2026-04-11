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

## Blocker Resolution

The prior Stage 01 run (2026-04-03) flagged a **blocking architecture mismatch**
between the spec (`spreadsheet/`, XLSX-based Excel clone) and the source
(`MariloDataSheet<TItem>`, strongly-typed editable grid). **That blocker is
resolved.**

A new spec directory `docs/component-specs/datasheet/` now documents the
actual implementation with nine feature-area files (overview,
columns-and-schema, editing-and-validation, selection-and-ranges,
bulk-paste-and-clipboard, bulk-operations-and-saveall,
virtualization-and-performance, keyboard-and-accessibility,
theming-and-css-provider). The legacy `spreadsheet/` directory is now stale
and should be either deleted or explicitly marked as the (separate) future
Spreadsheet component. That cleanup is out of scope for this spec review and
is flagged as a follow-up.

---

## Summary

| Type | Count |
|------|-------|
| Implemented but not documented | 0 |
| Documented but not implemented | 0 |
| Documented and implemented but mismatched | 0 |
| ICM metadata drift (stale paths) | 3 |
| Cross-branch drift (missing from this worktree) | 2 |
| **Total actionable items** | **5** |

| Severity | Count |
|----------|-------|
| Blocking | 0 |
| Important | 2 |
| Nice-to-have | 3 |

---

## (A) Implemented API Surface — Spec Alignment Check

All nine public methods listed in `datasheet/overview.md` are present in
source:

| Spec method | Source location | Match |
|---|---|---|
| `ResetAsync()` | `MariloDataSheet.Data.cs:205` | Yes |
| `ValidateAllAsync()` | `MariloDataSheet.Data.cs:108` | Yes |
| `GetDirtyRows()` | `MariloDataSheet.razor.cs:120` | Yes |
| `SetDataAsync(IEnumerable<TItem>)` | `MariloDataSheet.razor.cs:129` | Yes |
| `ScrollToRowAsync(object)` | `MariloDataSheet.Interop.cs:38` | Yes |
| `CommitCellEdit(TItem, string, object?)` | `MariloDataSheet.Data.cs:38` | Yes |
| `EnterEditMode(TItem, string)` | `MariloDataSheet.Editing.cs:34` | Yes |
| `IsCellEditing(TItem, string)` | `MariloDataSheet.Editing.cs:63` | Yes |
| `SaveAllAsync()` | `MariloDataSheet.Data.cs:142` | Yes |

All 17 parameters listed in `datasheet/overview.md` (Data, KeyField, OnSaveAll,
OnRowChanged, OnValidate, IsSaving, AllowAddRow, AllowDeleteRow,
AllowBulkPaste, EmptyStateMessage, Height, IsLoading, EnableVirtualization,
AriaLabel, ChildContent, ToolbarTemplate, Class, Style) are present in source
(`Class`, `Style` inherited from `MariloComponentBase`). No parameter gaps.

All 12 MariloDataSheetColumn parameters match source. No gaps.

All event arg types (`DataSheetSaveArgs`, `DataSheetRowChangedArgs`,
`DataSheetValidateArgs`, `DataSheetValidationError`, `DataSheetCellContext`,
`DataSheetSelectOption`) match. Both enums (`DataSheetColumnType`, `CellState`)
match.

---

## (B) Cross-Branch Drift (NOT spec gaps — branch state)

Two items referenced in the task prompt were introduced on the `workInProgress`
branch at `ca71e0a` but are NOT reachable from this worktree's HEAD
(`32e064c`, merge of PR #55). Both would need to be cherry-picked or re-done
in a separate change; reapplying them from within spec-review is out of
scope and flagged as coordinator escalation.

1. **ComponentRegistry `DataSheet` entry — missing here.** (`Important`)
   `samples/Marilo.Demo/Data/ComponentRegistry.cs` has no DataSheet entry or
   `DataSheetSubPages` array. Consequence: `ComponentDemoLayout.ParseRoute()`
   cannot render breadcrumb/header for DataSheet demo pages. Consistent with
   the *intent* of the new spec (which assumes the 4 sub-pages), but absent
   from current source.
2. **ResetAsync / RestoreEntryOrRemoveNewRow doc remarks — missing here.**
   (`Nice-to-have`) This worktree's `MariloDataSheet.Data.cs` is 287 lines
   (older); the referenced helper and `IsNewlyAdded` tracking do not exist.
   The current `ResetAsync` implementation is simpler and has no combined
   `IsNewlyAdded && IsDeleted` edge case to document. Spec does not claim
   any edge-case behavior, so spec is consistent with current source as-is.

---

## (C) ICM Metadata Drift (fixed in this pass)

1. `_config/delivery-context.md` listed `API spec` as `spreadsheet/` — updated
   to point at `datasheet/`. `Spec Feature Areas` table replaced with the
   nine `datasheet/` files.
2. `_config/delivery-context.md` still read "Open spec gaps: ~38 (1 blocking)"
   — updated to reflect the resolved state.
3. `_status/workspace-status.md` showed Stage 01 as "COMPLETE (blocked)" —
   updated to "COMPLETE (unblocked)".

---

## (D) Demo-page Drift (NOT a spec gap — branch state)

The new spec's "Feature Areas" section references eight feature pages and the
ComponentRegistry entry (on the other branch) assumes four demo razor files:
Overview, BulkOperations, Editing-and-Validation, Keyboard-and-Accessibility.
This worktree contains only `Overview.razor` — the other three exist on the
`workInProgress` branch and are not yet merged here. Noted as a Stage 02
prerequisite, not a Stage 01 gap.

---

## Next Recommended Trigger

1. **Coordinator:** decide how to land `workInProgress`-branch items
   (`ca71e0a` ComponentRegistry entry, demo pages) into the mainline
   delivery path. This is outside the scope of a per-component worktree.
2. Once merged, re-run Stage 02 (`demo`) against the four demo pages.
3. Delete or explicitly deprecate the legacy
   `docs/component-specs/spreadsheet/` directory (separate workspace task).
