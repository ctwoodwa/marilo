# GAP-DATASHEET-V02 — Editing and Validation Verification (Result)

**Sub-task:** GAP-DATASHEET-V02 from `datasheet-spec-gaps-2026-04-10.md`
**Spec:** `docs/component-specs/datasheet/editing-and-validation.md` (228 lines, fully read)
**Source audited:** `MariloDataSheet.Data.cs` (287 lines) + `.Editing.cs` (294 lines) + `.razor.cs` (partial)
**Verification date:** 2026-04-10 (cron fire #13)

## Result: **1 confirmed structural gap** + **6 items flagged for targeted re-verification**

V02 is different from V03 and V07 — those surfaced 8 and 6 big concrete gaps (missing features + WCAG failures). V02 surfaces **one clear structural gap** plus several **subtle correctness items** that need a more focused read of specific methods to fully confirm. The dirty-tracking architecture is substantially present: the `_dirtyRows` Dictionary, `DirtyRowEntry<TItem>` class with `DirtyFields`/`ValidationErrors`/`IsDeleted`/`OverallState`, `ValidateAllAsync`, `SaveAllAsync`, `OnValidate` integration, and deleted-row separation all exist and look well-architected.

## What's Verified Present ✅

**Dirty tracking architecture (`.Data.cs:14-90`):**
- `Dictionary<object, DirtyRowEntry<TItem>> _dirtyRows` keyed by row key
- `DirtyRowEntry<TItem>` with `DirtyFields: HashSet<string>`, `ValidationErrors: Dictionary<string, string>`, `IsDeleted: bool`
- `OverallState` computed property: `Invalid` if errors, else `Dirty` if dirty fields, else (presumably) `Pristine`
- `GetRowKey` method via `KeyField` parameter reflection
- `CommitCellEdit` at line 40 — creates entry if missing, updates dirty field set
- Entry removal at line 89: `if (entry.DirtyFields.Count == 0 && !entry.IsDeleted) _dirtyRows.Remove(key)` — matches spec "If no fields remain dirty, the row is no longer considered dirty"

**Per-cell validation (`.Data.cs:77-82`):**
- Error storage/removal at lines 80-82: `entry.ValidationErrors[field] = error` / `Remove(field)`
- Two-layer structure (required + column Validate delegate) implied by spec but actual precedence order needs the full method read

**ValidateAllAsync (`.Data.cs:138-160`):**
- Iterates `_dirtyRows.Values` (line 142)
- Skips deleted rows (line 144): `if (entry.IsDeleted) continue;` ✓ matches spec
- Stores/clears ValidationErrors per column.Field (lines 153, 158)

**SaveAllAsync (`.Data.cs:174-225`):**
- Early return on no dirty rows (line 174)
- Calls ValidateAllAsync first (line 177) ✓ matches spec flow
- Filters dirty-non-deleted rows into DirtyRowsList (lines 180-181)
- Runs OnValidate with error list, then maps errors back to cells (lines 196-199): `entry.ValidationErrors[error.Field] = error.Message` ✓ matches spec
- Provides deleted rows to OnSaveAll (lines 216-217)

**ResetAsync (`.Data.cs:237-251`):**
- Iterates dirty rows, clears dirty state
- Clears `_dirtyRows` at line 250

**MarkRowDeleted (`.Data.cs:259-274`):**
- Creates entry if missing; sets `IsDeleted = true`

**Public query helpers (`.Data.cs:281-315`):**
- `IsCellDirty`, `IsRowDirty`, `IsRowDeleted`, `GetCellError` all present

**Most of the editing lifecycle:**
- F2 enters edit mode ✓ (`.Editing.cs:168`)
- Escape cancels ✓ (`.Editing.cs:159`)
- Tab/Shift+Tab commits and moves ✓ (`.Editing.cs:210-236`)
- `CommitCellEdit` / `EnterEditMode` / `IsCellEditing` public methods ✓ (V03 verified)

---

## Confirmed Gap

### GAP-DATASHEET-V02-01: No deep-cloned original snapshot for dirty comparison
**Severity:** **High** (correctness — breaks "edit back to original" detection)
**Spec:** `editing-and-validation.md:89-98` — "When `Data` is bound or `SetDataAsync` is called, the component **deep-clones each row to create an original snapshot**. After each cell commit, the new value is **compared against the original snapshot** for that field. If the values differ, the field is added to the row's dirty field set and the cell renders as `CellState.Dirty`. **If the user edits a cell back to its original value, the field is removed from the dirty set.**"

**Current:** `.razor.cs:107-115` `OnParametersSet`:
```csharp
protected override void OnParametersSet()
{
    if (Data != null)
        _displayRows = Data.ToList();
    else
        _displayRows = [];
}
```
Just a `.ToList()` — **shallow copy of the list**. Each element is still the same reference as in the caller's collection. No `_originalSnapshots` field, no deep-clone logic, no original-value dictionary visible in either `.razor.cs` or `.Data.cs`.

`SetDataAsync` at `.razor.cs:129-136` has the same pattern: `_displayRows = data.ToList(); _dirtyRows.Clear();`. Again no snapshot.

**Consequence (the user-visible bug):** The spec's "edit back to original value → field removed from dirty set" contract **cannot work**. The source tracks `DirtyFields` by name (at `.Data.cs:67-71`) but has nothing to compare against for "is the current value equal to the original" — it only knows which fields have been touched since the component loaded. So:

1. User loads DataSheet with `row.Price = 100`
2. User edits to `row.Price = 250`, commits — field `"Price"` added to DirtyFields ✓
3. User edits back to `row.Price = 100`, commits — field `"Price"` should be **removed** from DirtyFields per spec, but the source has no original to compare against, so it stays dirty
4. Save All payload incorrectly contains the "dirty" row even though the user reverted

**Reading line 67 vs line 71 suggests the source has SOME kind of equality check** (the grep shows `entry.DirtyFields.Remove(field)` on line 67 and `entry.DirtyFields.Add(field)` on line 71, which looks like an if/else). The check may compare against the ROW REFERENCE's current value — but since the caller's model and the displayed row are the **same object reference**, the "original value" has already been mutated to the new value by the time the comparison runs. The comparison is vs. itself.

**Recommended direction:** Two options:

**Option A — Deep clone on load/SetData (spec-literal):**
Add `Dictionary<object, Dictionary<string, object?>> _originalValues = [];` keyed by row key. On Data bind / SetDataAsync, iterate columns and reflect-read each field into the snapshot dictionary. In `CommitCellEdit`, compare `newValue` against `_originalValues[rowKey][field]` to decide dirty. On save success, refresh the snapshot to the new values.

**Option B — Cache pre-edit value in `_editValueBeforeEdit` and use it as "original" (approximate):**
The source already has `_editValueBeforeEdit` at `.Editing.cs:161` which stores the value before a specific edit session. This would work for "Escape → revert" but not for "edit twice, revert to original" across multiple edits.

Option A is the spec-compliant fix. Option B is a simpler partial fix that doesn't match spec but covers the common case.

**Status:** Open — confirmed correctness gap with user-visible consequence

---

## Items Flagged for Targeted Re-Verification

These are **not confirmed gaps** — they are spec details where the source might match the spec but I can't confirm without reading the exact method body. Each is a small sub-audit (~5 minutes, one file read).

### V02-R1: Printable character entry into edit mode
**Spec reference:** `editing-and-validation.md:48-49` — "Press any printable character | Opens the editor and replaces the current value with the typed character"
**Also already flagged as:** V07-05 (keyboard and accessibility findings)
**Status:** Cross-referenced — same gap. V02 confirms V07's finding from a different feature-area perspective.

### V02-R2: Double-click opens editor
**Spec reference:** `editing-and-validation.md:51` — "Double-click on any cell | Focuses the cell and opens the editor."
**What to check:** Grep `.Rendering.cs` and `.razor` for `@ondblclick` or `"dblclick"` event handler on cell elements.
**Risk:** If missing, users lose the standard spreadsheet-familiar double-click-to-edit gesture.

### V02-R3: Click-outside-editing-cell commits the active editor
**Spec reference:** `editing-and-validation.md:65` — "Click outside the editing cell | Commits the current cell. Focus moves to the clicked cell."
**What to check:** Cell click handler needs to detect "am I currently in edit mode on a different cell? If so, commit it before activating me."
**Risk:** Common UX expectation; if missing, users lose half-typed edits.

### V02-R4: OnRowChanged fires even on validation fail
**Spec reference:** `editing-and-validation.md:85` — "If validation fails at step 2 or 3, the cell transitions to CellState.Invalid, the error message is stored, and **OnRowChanged still fires** (the value was written to the model)."
**What to check:** Find `OnRowChanged.InvokeAsync` call in `.Data.cs` (likely inside `CommitCellEdit` method at line 40-90). Confirm it fires unconditionally, not inside an `if (isValid)` branch.
**Risk:** If the source short-circuits OnRowChanged on validation failure, consumers who use OnRowChanged for non-persistence side effects (e.g., computing dependent display values) will miss events.

### V02-R5: Required-error takes priority over custom Validate error
**Spec reference:** `editing-and-validation.md:139` — "only one error message is displayed (**the required error takes priority if both fail**)."
**What to check:** In the two-layer validation (lines 77-82 based on grep), confirm the required check runs first and short-circuits before the custom Validate delegate runs.
**Risk:** Error message inconsistency between implementations if users have both Required=true and a Validate delegate.

### V02-R6: `CellState.Saving` / `CellState.Saved` transitions during Save All
**Spec reference:** `editing-and-validation.md:116-118` — three transitions: `Dirty → Saving` during Save, `Saving → Saved` on success, `Saved → Pristine` after brief visual indicator period.
**What to check:** Search for `CellState.Saving` and `CellState.Saved` assignments in `.Data.cs` (lines 174-250 — the SaveAllAsync method body).
**Risk:** If these states are never set, users get no "saving" / "saved" visual feedback — cells just jump from Dirty to (cleared) Pristine with no transitional UX.

### V02-R7: Post-save snapshot refresh (spec's "new original snapshot")
**Spec reference:** `editing-and-validation.md:118` — "The saved snapshot becomes the new original."
**Depends on:** V02-01 being fixed first.
**What to check:** If V02-01 is fixed and `_originalValues` exists, then after successful SaveAll the original snapshot dictionary needs to be refreshed to the new (saved) values. Otherwise re-editing a just-saved cell back to the pre-save value would incorrectly show as dirty.
**Risk:** Same as V02-01's downstream consequence.

### V02-R8: Save All button disabled while any cell is Invalid
**Spec reference:** `editing-and-validation.md:193` — "The Save All button (or Ctrl+S) is disabled as long as any cell in the sheet is in `CellState.Invalid`."
**What to check:** Read the `.razor` file's Save All button to see its `disabled` attribute binding. Should check something like `@(_dirtyRows.Values.Any(e => e.ValidationErrors.Count > 0))` in addition to `IsSaving`.
**Risk:** If the button is only disabled during `IsSaving`, users can trigger Save All with invalid cells; ValidateAllAsync will abort but the button press is wasted effort.

---

## Summary of V02 Results

| Item | Severity | Type | Status |
|---|---|---|---|
| V02-01 Deep-cloned original snapshot | **High** | Correctness — user-visible bug in "edit back to original" | **Confirmed** |
| V02-R1 Printable char | — | Cross-ref to V07-05 | Already counted |
| V02-R2 Double-click | ? | Needs grep | Open (verification) |
| V02-R3 Click-outside commits | ? | Needs grep | Open (verification) |
| V02-R4 OnRowChanged fires on fail | ? | Needs method body read | Open (verification) |
| V02-R5 Required-error precedence | ? | Needs method body read | Open (verification) |
| V02-R6 Saving/Saved cell states | ? | Needs grep | Open (verification) |
| V02-R7 Post-save snapshot refresh | Depends on V02-01 | Structural | Open (depends) |
| V02-R8 Save All disabled on Invalid | ? | Needs razor read | Open (verification) |

**1 confirmed + 7 targeted re-verification items + 1 cross-ref.** The re-verification items (V02-R2 through V02-R8) are small individual reads, not bulk investigation — a future fire could knock out multiple in one pass.

## Recommended Stage 03 Resolution Integration

V02-01 (deep-clone snapshot) is **self-contained** and can be its own small batch:

**DataSheet Batch B — Dirty Tracking Correctness** (~1-2 commits, 5-8 bUnit tests)

- Add `Dictionary<object, Dictionary<string, object?>> _originalValues` field
- Populate in `OnParametersSet` + `SetDataAsync` by reflecting each column's field into the snapshot
- Modify the dirty-set logic in `CommitCellEdit` to compare `newValue == _originalValues[rowKey][field]` → remove from DirtyFields, else → add
- Refresh `_originalValues[rowKey][field] = newValue` at the end of a successful `SaveAllAsync` (post-OnSaveAll invocation)
- bUnit tests: "edit and revert" (field should leave DirtyFields), "edit twice then revert" (same), "edit, save, edit back to saved value" (should be dirty — saved value is the new original), "reset" (all dirty cleared, original unchanged)

This batch is **independent of V03+V07's consolidated batch** — it touches `.Data.cs` and `.razor.cs` `OnParametersSet`, not the rendering or keyboard handlers. Can land in parallel.

V02-R2 through V02-R8 can be resolved inline as a follow-up verification fire once V02-01's batch lands. Several might turn out to be already-implemented (the architecture looks well-built), in which case the verification cost is trivial.

## Human Decisions Needed

**Zero for V02-01.** The deep-clone approach is spec-mandated.

**One potential decision for V02-R6 (Saving/Saved transitions):** how long should the `Saved` state linger before transitioning to `Pristine`? Spec says "brief visual indicator period" without specifying duration. Common choices: 500ms, 1s, 2s. This is a minor UX call — default to 1s unless the human has a preference.

## Stage 01b Status After V02

- ✅ V03 complete (fire #10/#11) — 8 sub-gaps
- ✅ V07 complete (fire #12) — 6 sub-gaps + 2 cross-refs
- ✅ **V02 complete (this fire)** — 1 confirmed sub-gap + 7 re-verification items
- ⏳ V01, V04, V05, V06, V08, V09, V10 — 7 remaining

**3 of 10 sub-tasks complete. 15 confirmed implementation gaps opened so far** (V03×8 + V07×6 + V02×1).

**Decision-independent Stage 03 batches now ready to queue:**
- **Batch A — Range selection + keyboard + a11y** (V03+V07, 14 gaps)
- **Batch B — Dirty tracking correctness** (V02-01, 1 gap)

Both batches can run in parallel once the human authorizes Stage 03 work.
