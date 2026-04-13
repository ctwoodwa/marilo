# DataSheet S05 Wave 1 Report

**Worker:** `w-datasheet-gap-analysis`
**Date:** 2026-04-12
**Wave:** 1 (Phase A -- 8 tasks)
**Build:** `dotnet build Marilo.slnx` -- exit 0
**Tests:** `dotnet test` DataSheet -- 80/80 passed (73 existing + 7 new)

---

## Task Summary

### TASK-DS-001: Workspace Coverage Audit (WS-01) -- DONE
- Populated `_config/coverage-summary.md` with per-parameter, per-event, per-keyboard-shortcut, per-cell-state, and per-aria-announcement coverage tables.
- Cross-referenced MariloDataSheet source (16 parameters, 3 events, 19 keyboard shortcuts) against existing bUnit tests, demo pages, and spec docs.
- Every public parameter, event, and keyboard shortcut has a row with Yes/No/Partial status and gap-ID references.

### TASK-DS-002: Grid Root tabindex="0" (SA-01) -- DONE
- Added `tabindex="0"` to the grid root `<div>` in `MariloDataSheet.razor`, positioned after `class` and before `style`.
- bUnit test `GridRoot_Has_Tabindex_Zero` verifies `[role='grid']` has `tabindex="0"`.
- Matches spec `keyboard-and-accessibility.md:66,74`.

### TASK-DS-003: Virtualization Threshold Spec + Demo (UD-02, EU-01) -- DONE
- Added WASM threshold note to `virtualization-and-performance.md` stating 10k rows supported with EnableVirtualization=true, demo capped at 5k.
- Created `Virtualization.razor` demo with row-count toggle (100/1k/5k) and automatic EnableVirtualization for 1k+.
- No 10k demo option exists. Auto-closes VP-datasheet-D03 deferral.
- Also applied SRC-01 skeleton row wording fix (TASK-DS-023 partial) while in the file.

### TASK-DS-004: Paste-during-save Guard (SA-08, EU-03) -- DONE
- Added `if (IsSaving) return;` early-return guard to `PasteFromClipboard` in `MariloDataSheet.Editing.cs`, after existing `AllowBulkPaste` guard.
- bUnit test `Paste_DuringSave_IsNoOp` verifies paste is blocked when `IsSaving=true`.
- Demo EU-03 behavior is now truthful (paste blocked during save window).

### TASK-DS-005: Missing aria-live Announcements (SA-13, EU-05) -- DONE
- Added 3 aria-live announcements to SaveAllAsync in `MariloDataSheet.Data.cs`:
  1. "Saving changes." after `_isSaving = true` (before validation).
  2. "Save failed. {N} validation error(s)." when validation blocks save (replaces generic wording).
  3. "Save failed. An error occurred." in catch block.
- bUnit tests: `SaveAll_Announces_SavingChanges_AtStart`, `SaveAll_ValidationFail_AnnouncesErrorCount`, `SaveAll_Exception_AnnouncesSaveFailed`.
- All 3 announcement paths verified.

### TASK-DS-006: AddRow ActivateCell (SA-03) -- DONE
- Added `ActivateCell(newItem, firstEditableCol.Field)` at the end of `AddRowAsync` in `MariloDataSheet.razor.cs`, after DirtyFields seeding. Finds first editable, non-computed column.
- bUnit test `AddRow_ActivatesFirstEditableCell` verifies active cell is on the new row's first editable column (skipping computed columns).

### TASK-DS-007: Reset Clears Undo Buffer (SA-04) -- DONE
- Added `_undoBuffer.Clear();` to `ResetAsync` in `MariloDataSheet.Data.cs`, after `_dirtyRows.Clear()` and before `ClearActiveCell()`.
- bUnit test `Reset_ClearsUndoBuffer_CtrlZ_IsNoOp` verifies Ctrl+Z after ResetAsync is a no-op.

### TASK-DS-008: Copy-Paste Round-Trip Demo (EU-02) -- DONE
- Added Scenario F to `BulkOperations.razor`: grid with formatted columns (Currency via `ToString("C2")`, Date via `ToString("MMMM dd, yyyy")`).
- Visual indicator confirms raw value preservation through `data-raw-value` contract.
- Exercises Format + data-raw-value round-trip path.

---

## Files Changed

### Source
| File | Change |
|---|---|
| `src/Marilo.Components/DataGrid/MariloDataSheet.razor` | tabindex="0" on grid root (DS-002) |
| `src/Marilo.Components/DataGrid/MariloDataSheet.razor.cs` | ActivateCell after AddRow (DS-006) |
| `src/Marilo.Components/DataGrid/MariloDataSheet.Editing.cs` | IsSaving paste guard (DS-004) |
| `src/Marilo.Components/DataGrid/MariloDataSheet.Data.cs` | 3 aria-live announcements (DS-005), undo buffer clear (DS-007) |

### Tests
| File | Change |
|---|---|
| `tests/Marilo.Tests.Unit/DataGrid/MariloDataSheetTests.cs` | +7 new bUnit tests (DS-002,004,005x3,006,007) |

### Specs
| File | Change |
|---|---|
| `docs/component-specs/datasheet/virtualization-and-performance.md` | WASM threshold note (DS-003), skeleton row fix (DS-023 partial) |

### Demos
| File | Change |
|---|---|
| `samples/Marilo.Demo/Pages/Components/DataSheet/Virtualization.razor` | New demo (DS-003) |
| `samples/Marilo.Demo/Pages/Components/DataSheet/BulkOperations.razor` | Scenario F round-trip demo (DS-008) |

### Gap-plan
| File | Change |
|---|---|
| `ICM/workspaces/datasheet-gap-analysis/_config/coverage-summary.md` | Full coverage audit (DS-001) |

---

## Verification

- `dotnet build Marilo.slnx`: exit 0
- `dotnet test` (DataSheet): 80/80 passed, 0 failures
- 7 new tests written TDD-style (RED confirmed before implementation)
- All 8 acceptance criteria met per remediation plan
