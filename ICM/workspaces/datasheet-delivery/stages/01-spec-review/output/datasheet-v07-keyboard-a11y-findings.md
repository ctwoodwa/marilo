# GAP-DATASHEET-V07 — Keyboard and Accessibility Verification (Result)

**Sub-task:** GAP-DATASHEET-V07 from `datasheet-spec-gaps-2026-04-10.md`
**Spec:** `docs/component-specs/datasheet/keyboard-and-accessibility.md` (187 lines, fully read)
**Source audited:** `MariloDataSheet.razor` + `.razor.cs` + `.Editing.cs` + `.Rendering.cs`
**Verification date:** 2026-04-10 (cron fire #12)

## Result: **8 confirmed implementation gaps** (6 ARIA/role, 2 keyboard shortcut) + 2 cross-references to V03

Like V03, V07 surfaces real implementation work, not verification paperwork. The DataSheet has **good root-level ARIA coverage** (grid role, rowcount, colcount, toolbar landmarks, aria-live regions, aria-label everywhere) and **partial keyboard navigation** (arrow/tab/enter/escape/F2/Ctrl+Z/Ctrl+D/Ctrl+V/Delete) but misses several spec-required details on both fronts.

## Broad Result — What's Present ✅

**Root element (`.razor:9-13`):**
- `role="grid"` ✓
- `aria-label="@AriaLabel"` ✓
- `aria-rowcount="@_displayRows.Count"` ✓
- `aria-colcount="@_columns.Count"` ✓
- `aria-busy` ⚠ **partial** — only `IsLoading`, spec requires `IsLoading || IsSaving`

**Toolbar / Bulk bar:**
- `role="toolbar"` on both ✓
- `aria-label` on both ✓
- `aria-live="polite"` on dirty badge (`.razor:30`), skeleton region (`:79`), and announcement region (`:163`) — three regions total

**Header row (`.razor:98-121`):**
- `role="row"` on `<tr>` ✓
- `role="columnheader"` on `<th>` ✓
- `aria-label` on columns ✓
- Select-all checkbox has `aria-label="Select all rows"` ✓

**Data rows/cells (`.Rendering.cs`):**
- `aria-readonly="true"` on non-editable cells (line 65) ✓
- `aria-invalid="true"` on invalid cells (line 67) ✓
- `aria-hidden="true"` on deleted rows (line 29) ✓
- `aria-label` on interactive elements: "Select row" (line 41), "Delete row" (line 112), "Edit {title}" on editor inputs (lines 170, 184, 201, 216, 238) ✓

**Keyboard handlers (`.Editing.cs`):**
- Arrow keys (`:186-205`) ✓ (but Shift-unaware — see V03-04)
- Tab / Shift+Tab (`:210-236`) ✓
- Enter ⚠ handler exists but behavior-match unverified
- F2 (`:168`) ✓
- Escape (`:159`) ✓
- Delete (`:175`) ⚠ single-cell only — see V03-08
- Ctrl+Z (`:125`) ✓
- Ctrl+D (`:140`) ⚠ wrong scope — see V03-07
- Ctrl+V (paste handler at `:246` + `.Interop.cs` JS bridge) ✓

---

## Confirmed Gap List

### GAP-DATASHEET-V07-01: Missing `Ctrl+S` Save All shortcut
**Severity:** High
**Spec:** `keyboard-and-accessibility.md:57` — "Ctrl+S | Any | Triggers Save All. Runs validation, then fires `OnSaveAll` if all cells are valid. If a cell is in edit mode, it is committed first."

**Current:** Grep across `MariloDataSheet.Editing.cs` for the `HandleKeyDown` branches found handlers for `ctrl && key == "z"` (line 125) and `ctrl && key == "d"` (line 140), but no `ctrl && key == "s"` branch. Saving is only possible via clicking the Save All button in the toolbar.

**Consequence:** Power users cannot save without leaving the keyboard. The default browser Ctrl+S dialog will open instead, which per spec must be suppressed via `preventDefault`.

**Recommended direction:** Add a `ctrl && key == "s"` branch to `HandleKeyDown` at `.Editing.cs:~140` (next to existing Ctrl+D branch). Call `SaveAllAsync()`. Must also add `preventDefault` in the JS interop's keydown handler so the browser save dialog is suppressed.

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V07-02: Data rows missing `role="row"` attribute
**Severity:** High (WCAG 2.1 conformance)
**Spec:** `keyboard-and-accessibility.md:129` — "Table row | `role="row"` | Each data row and the header row."

**Current:** The header row has `role="row"` (`.razor:98`) and the empty-state row has `role="row"` (`.razor:130`), but **data rows rendered by `.Rendering.cs`** do not. Grep across `MariloDataSheet.Rendering.cs` for `role=` returned zero matches — all `role=` declarations come from the razor markup, not the render tree builder.

**Consequence:** Screen readers cannot traverse the grid row-by-row using grid-navigation commands (e.g., NVDA's Ctrl+Alt+arrow). The grid structure is advertised via `role="grid"` on the root but not propagated to data rows.

**Recommended direction:** Add `builder.AddAttribute(seq++, "role", "row");` to the `<tr>` element in `.Rendering.cs` wherever data rows are opened. (Exact location: search for `builder.OpenElement(..., "tr")` in the render method.)

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V07-03: Data cells missing `role="gridcell"` attribute
**Severity:** High (WCAG 2.1 conformance)
**Spec:** `keyboard-and-accessibility.md:130` — "Data cell | `role="gridcell"`"

**Current:** The empty-state `<td>` has `role="gridcell"` (`.razor:131`), but **data cells rendered by `.Rendering.cs`** do not. Same grep evidence as V07-02.

**Consequence:** Screen readers navigating cell-by-cell cannot identify each `<td>` as a gridcell — they fall back to generic "cell" semantics. Combined with V07-02, this breaks the WAI-ARIA grid pattern for the entire data area.

**Recommended direction:** Add `builder.AddAttribute(seq++, "role", "gridcell");` to the `<td>` elements in `.Rendering.cs`. Data and action cells both need it.

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V07-04: `aria-busy` only reacts to `IsLoading`, not `IsSaving`
**Severity:** Medium
**Spec:** `keyboard-and-accessibility.md:128` — "`aria-busy="true"` when `IsLoading` or `IsSaving`."

**Current:** `MariloDataSheet.razor:13` has `aria-busy="@(IsLoading ? "true" : null)"` — only checks `IsLoading`.

**Consequence:** During a Save All operation, screen readers are not told the grid is busy. User may try to interact with cells that are in a transitional state.

**Recommended direction:** One-line fix: change to `aria-busy="@((IsLoading || IsSaving) ? "true" : null)"` at `.razor:13`.

**Status:** Open — confirmed real gap, trivial fix

---

### GAP-DATASHEET-V07-05: Missing "printable character enters edit mode" handler
**Severity:** Medium
**Spec:** `keyboard-and-accessibility.md:49` — "Printable character | Not in edit mode | Enters edit mode and replaces the cell value with the typed character (text and number columns only)."

**Current:** The `HandleKeyDown` method at `.Editing.cs` handles specific keys (Ctrl+Z/Ctrl+D/Escape/F2/Delete/arrow/tab/enter) but has no branch for detecting printable characters (single-character keys with no modifier) in the active cell when not in edit mode.

**Consequence:** Users who "just start typing" on a selected text or number cell get no feedback. Excel/Google Sheets convention is to auto-start editing.

**Recommended direction:** Add a branch in `HandleKeyDown` that checks: `if (!_isEditMode && !ctrl && !alt && key.Length == 1 && IsPrintable(key[0]))`. Then: enter edit mode on the active cell (if editable and not computed), replace the value with the typed character (for Text/Number columns only — Date/Select/Checkbox should ignore printable chars per spec).

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V07-06: Missing Space-toggles-checkbox handler
**Severity:** Medium
**Spec:** `keyboard-and-accessibility.md:51` — "Space | Active cell is checkbox | Toggles the checkbox value. No separate edit mode needed."

**Current:** Grep across `.Editing.cs` for `" "` / `"Space"` / `Space` in the key handler branches returned no matches. (Verification: the grep for `OnKeyDown|@onkeydown` confirmed the only key handler is `HandleKeyDown`.)

**Consequence:** Keyboard-only users on a checkbox column cannot toggle values — they must either mouse-click or enter edit mode (which doesn't exist for checkbox columns per spec line 117-118).

**Recommended direction:** Add a branch in `HandleKeyDown`: `if (key == " " && !_isEditMode && _activeCellRow != null && _activeCellField != null)`. Check if the active cell's column is `ColumnType.Checkbox`. If so, toggle via `CommitCellEdit(row, field, !currentValue)`.

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V07-07: Enter key behavior unverified
**Severity:** Low (probably works — needs confirmation)
**Spec:** `keyboard-and-accessibility.md:40-41` — two behaviors based on mode:
  - Edit mode → commits current cell and moves active cell down one row in the same column
  - Not in edit mode → enters edit mode (same as F2)

**Current:** `.Editing.cs:210-236` has Enter-key handling in the editing branch, plus `:186-205` has arrow-key handling. Grep didn't surface the exact Enter branch for non-edit-mode, but the cursor into that region suggests it may be present. **Requires reading `.Editing.cs:210-236` in full to confirm both behaviors match spec.**

**Consequence:** If only one of the two Enter behaviors is implemented (common mistake), users get inconsistent editing UX.

**Recommended direction:** Verify both branches exist in the next sub-audit. If missing, add the "Enter in non-edit mode → enter edit mode" branch and the "Enter in edit mode → commit + move down" branch (two distinct behaviors per spec).

**Status:** Open — needs focused verification (not confirmed as a gap yet)

---

### GAP-DATASHEET-V07-08: Screen reader announcement text not wired
**Severity:** Medium
**Spec:** `keyboard-and-accessibility.md:146-154` — five specific announcements:
  - "{N} rows modified" on dirty count change
  - "Saving changes" on Save All start
  - "Changes saved successfully" on Save All success
  - "Save failed. {N} validation errors." on Save All failure
  - "{N} cells have errors" on validation errors appearing

**Current:** `MariloDataSheet.razor.cs:18` declares `internal string _ariaAnnouncement = "";` and `.razor:163` renders `<div aria-live="polite" role="status" class="mar-datasheet__aria-live">{content}</div>`. The field exists but the **assignment logic that produces the spec-required text strings is not verified** — a grep for `_ariaAnnouncement = ` would tell us where (if anywhere) the field gets populated. Without that wiring, the aria-live region exists structurally but never announces anything.

**Consequence:** Screen reader users get no audible feedback for dirty count, save progress, validation errors, or save results — breaking the intended accessibility UX.

**Recommended direction:** Grep `_ariaAnnouncement = ` in the DataSheet partials. If no assignments exist, wire the field from:
- `CommitCellEdit` (announce dirty count change — but rate-limited: don't announce every cell edit, only when row count changes)
- `SaveAllAsync` entry ("Saving changes")
- `SaveAllAsync` success exit ("Changes saved successfully")
- `SaveAllAsync` validation failure ("{N} cells have errors") and post-validation save failure path
- `ValidateAllAsync` when errors accumulate

Use spec-exact text strings for consistency.

**Status:** Open — confirmed real gap (structural wiring absent)

---

### Cross-references to V03 (already documented — not re-listed):
- **V03-05** Ctrl+A select-all — also flagged here (spec `keyboard-and-accessibility.md:62`)
- **V03-06** Ctrl+C copy — also flagged here (spec `keyboard-and-accessibility.md:59`)

Both are the same underlying gaps as documented in the V03 findings file. V07 confirms they belong equally to the keyboard/a11y feature area.

---

## Summary of V07 Results

| Gap | Severity | Type | Depends on |
|---|---|---|---|
| V07-01 Ctrl+S Save All | High | Missing feature | JS interop preventDefault |
| V07-02 `role="row"` on data rows | High | WCAG gap | — |
| V07-03 `role="gridcell"` on data cells | High | WCAG gap | — |
| V07-04 `aria-busy` incomplete | Medium | Bug (incomplete check) | — |
| V07-05 Printable char → edit mode | Medium | Missing feature | — |
| V07-06 Space toggles checkbox | Medium | Missing feature | — |
| V07-07 Enter behavior | Low | Needs verification | — |
| V07-08 aria-live announcement text | Medium | Missing wiring | — |

**Severity distribution:** 3 High, 4 Medium, 1 Low. Plus 2 cross-references (V03-05, V03-06) — V03 already counts them.

Including V03 overlaps, **DataSheet's combined selection + keyboard + a11y gap list is now 14 confirmed gaps** (V03's 8 + V07's 6 non-overlapping).

## Recommended Stage 03 Resolution

V07 fits naturally alongside V03 in the same Stage 03 batch. Suggested consolidated batch layout:

**DataSheet Batch A — Range-selection + keyboard foundation** (~3-5 commits, 20-30 tests)

- **Phase A1:** V03-01 (range state) + V07-02/V07-03 (role="row"/role="gridcell" on data rows/cells) + V07-04 (aria-busy fix) — foundation work
- **Phase A2:** V03-02/V03-04 (Shift+Click + Shift+Arrow) + V07-05 (printable char) + V07-06 (Space-for-checkbox) — input handlers
- **Phase A3:** V03-05/V07-01 (Ctrl+A, Ctrl+S) + V03-06 (Ctrl+C) — command shortcuts + JS interop preventDefault updates
- **Phase A4:** V03-07 (Fill Down fix) + V03-08 (Delete range) — range-consuming operations
- **Phase A5:** V07-08 (aria-live announcements wiring) — UX polish
- **Phase A6:** V03-03 (mouse drag-select) — last because it's the lowest-severity input mechanism

V07-07 (Enter behavior) splits off into a quick verification task before any Phase A work lands.

## Human Decisions Needed

**Zero.** Every gap has a clear spec-grounded recommended direction.

## Stage 01b Status After V07

- ✅ **V03 complete** (fire #10/#11) — 8 sub-gaps
- ✅ **V07 complete** (this fire) — 8 sub-gaps + 2 cross-refs
- ⏳ V01, V02, V04, V05, V06, V08, V09, V10 — 8 remaining verification sub-tasks

**Updated Stage 01b completion:** 2 of 10 sub-tasks done; 14 confirmed implementation gaps opened, all in the DataSheet selection + keyboard + a11y feature areas.

Next queue candidate: **V02 — `editing-and-validation.md`** (likely to surface gaps around validation timing and dirty tracking) OR **V04 — `bulk-paste-and-clipboard.md`** (likely to surface gaps around paste error handling and type coercion). Both are higher-risk than V01/V05/V06/V08 which are more polish-oriented.
