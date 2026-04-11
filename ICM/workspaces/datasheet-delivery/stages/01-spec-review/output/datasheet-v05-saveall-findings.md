# GAP-DATASHEET-V05 — Bulk Operations and Save All Verification (Result)

**Sub-task:** GAP-DATASHEET-V05 from `datasheet-spec-gaps-2026-04-10.md`
**Spec:** `docs/component-specs/datasheet/bulk-operations-and-saveall.md` (232 lines, fully read)
**Source audited:** `MariloDataSheet.Data.cs:14-255` (ValidateAllAsync, SaveAllAsync, ResetAsync, DirtyRowEntry class, CommitCellEdit) + cross-ref to earlier findings
**Verification date:** 2026-04-10 (cron fire #17)

## Result: **7 confirmed gaps** + **2 prior-finding corrections** + reuse from V02

V05 is the highest-yield verification pass after V03/V07 — the Save All flow is architecturally present but has **several spec-compliance gaps in post-save state transitions and Add Row/Reset edge cases**. Two of my earlier fire findings (V02-01 and V07-08) are also **corrected** based on code I now see for the first time.

---

## Correction 1: V02-01 is **INVALIDATED** (false positive)

**Original claim (fire #13):** "DataSheet has no deep-cloned original snapshot for dirty comparison because `OnParametersSet` uses `.ToList()` which is a shallow copy."

**What I missed:** The deep clone **does exist**, but it happens **lazily inside `CommitCellEdit` at `.Data.cs:47-51`**:

```csharp
entry = new DirtyRowEntry<TItem>
{
    Original = GridReflectionHelper.DeepClone(row),
    Current = row
};
```

The `DirtyRowEntry<T>` class at `.Data.cs:16-26` declares both `Original` and `Current` as separate `T` properties. Line 64 reads the original value from `entry.Original` for dirty comparison, and the logic at lines 64-72 correctly implements "if newValue equals original → remove from DirtyFields, else → add":

```csharp
var originalValue = GridReflectionHelper.GetValue(entry.Original, field);
if (!entryIsNew && object.Equals(newValue, originalValue))
{
    entry.DirtyFields.Remove(field);
}
else
{
    entry.DirtyFields.Add(field);
}
```

**The spec contract is implemented.** The "edit back to original → field removed from dirty set" behavior works. My V02 fire #13 grep missed this because I filtered for `_dirtyRows|DirtyFields|IsDeleted` etc., but not `DeepClone` or `Original =` — which is where the evidence was.

**Minor spec deviation (not a gap):** Spec says the deep clone happens "when Data is bound or SetDataAsync is called". Source does it lazily on first commit to each row. In normal usage these are equivalent (no edit happens between bind and first-touch). An edge case where the parent component imperatively mutates a row object between bind and first DataSheet touch would cause the lazy-snapshot to capture the mutated state rather than the bind-time state — but this is academic; Blazor consumers don't typically mutate bound data imperatively.

**V02-01 status:** ✅ **Invalidated** (closed as false positive, not a bug)

---

## Correction 2: V07-08 is **PARTIALLY VALID** (downgraded severity)

**Original claim (fire #12):** "`_ariaAnnouncement` field exists but isn't wired to produce the spec-required screen reader announcements."

**What I missed:** The `_ariaAnnouncement` field IS wired in `SaveAllAsync`:

- `.Data.cs:208` — `_ariaAnnouncement = "Save blocked: fix validation errors first.";` (fires when validation blocks save)
- `.Data.cs:228` — `_ariaAnnouncement = "Changes saved successfully.";` (matches spec keyboard-and-accessibility.md:151 exactly)
- `.Data.cs:252` — `_ariaAnnouncement = "All changes have been reset.";` (in ResetAsync — a spec-neutral announcement)

**2 of 5 spec-required announcements are wired.** The 3 that are still missing:

- **Dirty count changes** (spec: "{N} rows modified") — CommitCellEdit doesn't update `_ariaAnnouncement` when a row joins/leaves the dirty set
- **Saving start** (spec: "Saving changes") — not fired; source jumps straight from "Save blocked" check to OnSaveAll
- **Validation errors appear** (spec: "{N} cells have errors") — not fired when a commit produces an invalid state

**V07-08 status:** Downgraded from **"structural wiring absent"** → **"partially wired (2 of 5 announcements present)"**. Still needs fixing to reach full spec compliance, but not as bad as the V07 report suggested.

---

## V05 Findings — What's Verified Present ✅

**SaveAllAsync structure (`.Data.cs:172-230`):**
- Early return on no dirty rows (line 174) ✓
- Calls ValidateAllAsync first (line 177) ✓ matches spec flow step 1
- Computes filtered DirtyRowsList (lines 180-183) — `Where(e => e.DirtyFields.Count > 0 && !e.IsDeleted)` ✓ correctly excludes deleted rows from DirtyRows
- OnValidate event fires (line 191) ✓ spec step 3
- Maps OnValidate errors back to cells (lines 194-202) ✓
- Blocks save if errors (line 206-211) ✓ spec step 4
- OnSaveAll fires with DataSheetSaveArgs populated (lines 221-225) ✓ spec step 6
- Deleted rows populated separately via `_dirtyRows.Values.Where(e => e.IsDeleted)` (lines 216-219) ✓ matches spec line 90 ("row that is both dirty and deleted appears only in DeletedRows")

**ValidateAllAsync (`.Data.cs:138-167`):**
- Iterates `_dirtyRows.Values` ✓
- Skips deleted rows (line 144) ✓
- Runs column validation via `RunColumnValidation` (line 150) ✓
- Correctly stores/clears errors (lines 152-159) ✓
- Returns `!hasErrors` ✓

**ResetAsync (`.Data.cs:235-255`):**
- Iterates `_dirtyRows.Values` ✓
- Restores each dirty field from `entry.Original` to `entry.Current` via reflection (lines 241-246) ✓ — **confirms the deep-clone snapshot works**
- Clears `_dirtyRows` (line 250) ✓
- Clears active cell (line 251) ✓
- Sets aria announcement ✓

**AddRowAsync (`.razor.cs:183-190`):**
- Checks `AllowAddRow` gate ✓
- Creates via `Activator.CreateInstance<TItem>()` ✓ matches spec line 116 "parameterless constructor"
- Inserts into `_displayRows` ✓

---

## Confirmed Gaps

### GAP-DATASHEET-V05-01: `CellState.Saving` never assigned
**Severity:** Medium
**Spec:** `bulk-operations-and-saveall.md:46, 105` — "Cell state transition to Saving — all dirty cells transition to `CellState.Saving`. The CSS provider applies saving-specific styles." Spec step 5 of the Save All flow.

**Current:** `SaveAllAsync` at `.Data.cs:172-230` never writes `CellState.Saving` to any entry. The `OverallState` computed property on `DirtyRowEntry` returns `CellState.Invalid` (if errors), `Dirty` (if DirtyFields non-empty), or `Pristine` — there's no path that produces `Saving`. Cells go from `Dirty` directly to... whatever happens post-save (see V05-02).

**Consequence:** No visual saving-indicator on cells during the save operation. Users get no feedback that specific cells are being persisted.

**Recommended direction:** Add a `public bool IsInSaveOperation { get; set; }` flag on `DirtyRowEntry` (or make `OverallState` aware of a component-level `_isSavingCells` flag). Set it on the entries being saved, fire `StateHasChanged`, then clear after OnSaveAll completes. Alternative: add `Saving` to the OverallState formula when a component-level `_savingInProgress` flag is true.

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V05-02: No post-save cell state transitions (Saving → Saved → Pristine)
**Severity:** Medium
**Spec:** `bulk-operations-and-saveall.md:47-49, 105-108` — "On success, dirty cells transition to `CellState.Saved` (brief visual indicator), then to `CellState.Pristine`."

**Current:** After `OnSaveAll.InvokeAsync` completes at `.Data.cs:226`, the source just sets `_ariaAnnouncement = "Changes saved successfully."` and calls `StateHasChanged()`. **There is no post-save processing:**
- Entries remain in `_dirtyRows` with their DirtyFields sets intact
- `CellState.Saved` is never assigned
- `CellState.Pristine` transition relies on `_dirtyRows` entries being cleared, but they aren't
- No delay / brief-visual-indicator logic

**Consequence:** After a successful save, the cells continue to render as Dirty indefinitely. The user gets no visual confirmation that the save completed (beyond the aria announcement). The DataSheet is in an incorrect state until the parent component re-binds Data or the user triggers another operation.

**Recommended direction:** After `OnSaveAll.InvokeAsync` at line 226:
1. Iterate `_dirtyRows.Values.Where(e => !e.IsDeleted)` and update each `entry.Original = GridReflectionHelper.DeepClone(entry.Current)` (refresh snapshot — see V05-04)
2. Clear each `entry.DirtyFields` (transition to Saved/Pristine in the OverallState formula)
3. Optionally set a temporary `Saved` state flag for a brief indicator period
4. Remove deleted rows from `_displayRows` (see V05-03)
5. Call `_dirtyRows.Clear()` to drop all entries

**Status:** Open — confirmed real gap

---

### GAP-DATASHEET-V05-03: Deleted rows not removed from `_displayRows` after save
**Severity:** High (user-visible data corruption potential)
**Spec:** `bulk-operations-and-saveall.md:49, 91, 134` — "Deleted rows are removed from the dataset" / "After Save All, deleted rows are removed from the dataset and are no longer visible."

**Current:** `SaveAllAsync` at `.Data.cs:172-230` ends without touching `_displayRows`. Deleted rows (marked via `entry.IsDeleted = true` in `MarkRowDeleted`) remain visible in the grid after OnSaveAll fires and the caller's persistence completes.

**Consequence:** User clicks "Delete" on a row, confirms via Save All, the backend deletes it, but **the row is still visible in the UI**, still styled as "deleted" (struck-through), indefinitely. User's mental model says "I deleted that row" but the UI contradicts. On the next external data re-bind (if any), the row disappears, giving the user an inconsistent experience.

**Recommended direction:** After OnSaveAll completes successfully, at around line 226:
```csharp
var deletedKeys = _dirtyRows.Values
    .Where(e => e.IsDeleted)
    .Select(e => GetRowKey(e.Current))
    .Where(k => k != null)
    .ToHashSet();
_displayRows.RemoveAll(row => deletedKeys.Contains(GetRowKey(row)));
```
Then clear `_dirtyRows` as part of V05-02's fix.

**Status:** Open — confirmed real gap with user-visible consequence

---

### GAP-DATASHEET-V05-04: Original snapshots not refreshed after successful save
**Severity:** Medium (correctness — affects re-edit after save)
**Spec:** `bulk-operations-and-saveall.md:49, 91` — "The saved values become the new original snapshots." / "After a successful save, rows in `DirtyRows` have their original snapshots updated to the current values."

**Current:** Post-OnSaveAll code at `.Data.cs:226-229` does not touch `entry.Original` for any dirty row.

**Consequence:** After a save, if the user edits a just-saved cell back to the pre-save value, the dirty detection logic at `.Data.cs:64-72` compares against the stale pre-save `entry.Original` — so editing `100 → 250 → save → edit back to 250 → edit to 100` would incorrectly see 100 as equal to the original (because Original is still 100, never refreshed to 250) and mark the field as "back to original → remove from dirty set". This is a subtle correctness bug where post-save revert detection is wrong.

**Recommended direction:** In V05-02's fix, when clearing DirtyFields on successful save, first update `entry.Original = GridReflectionHelper.DeepClone(entry.Current)` so the snapshot reflects the saved state.

**Status:** Open — confirmed real gap (subtle but correct-by-design concern)

---

### GAP-DATASHEET-V05-05: AddRowAsync inserts at top of list, spec says end
**Severity:** Medium (spec compliance)
**Spec:** `bulk-operations-and-saveall.md:117` — "Appends the row to **the end** of the internal data list."

**Current:** `.razor.cs:187` — `_displayRows.Insert(0, newItem);` — inserts at position 0 (top).

**Consequence:** New rows appear at the top instead of the bottom, contradicting typical spreadsheet add-row UX where new rows go to the end. Violates spec verbatim.

**Recommended direction:** Change line 187 to `_displayRows.Add(newItem);` (or `Insert(_displayRows.Count, newItem)` for explicit end-of-list semantics).

**Status:** Open — confirmed real gap (one-line fix)

---

### GAP-DATASHEET-V05-06: Newly added row not marked dirty immediately
**Severity:** Medium
**Spec:** `bulk-operations-and-saveall.md:118, 120` — "The new row is immediately considered **dirty** (all fields differ from a 'no original' baseline)." / "The new row appears in `DataSheetSaveArgs.DirtyRows` when Save All is triggered."

**Current:** `AddRowAsync` at `.razor.cs:183-190` just inserts the row and calls StateHasChanged. It does **not** add an entry to `_dirtyRows`. The new row will NOT appear in `DataSheetSaveArgs.DirtyRows` unless the user touches at least one cell on it (which triggers `CommitCellEdit` and creates an entry).

**Consequence:** Users who Add Row, then immediately press Save All, **silently lose the new row** — it's in `_displayRows` visually but never reaches the persistence layer because it's not in DirtyRows.

**Recommended direction:** In AddRowAsync, after inserting the row, create a DirtyRowEntry and add all column fields to the DirtyFields set (or use a sentinel "isNewlyAdded" flag on the entry that SaveAllAsync respects):

```csharp
var key = GetRowKey(newItem);
if (key != null)
{
    var entry = new DirtyRowEntry<TItem>
    {
        Original = GridReflectionHelper.DeepClone(newItem),  // snapshot of initial state
        Current = newItem
    };
    foreach (var column in _columns.Where(c => c.Editable && c.ColumnType != DataSheetColumnType.Computed))
    {
        entry.DirtyFields.Add(column.Field);
    }
    _dirtyRows[key] = entry;
}
```

**Status:** Open — confirmed real gap (data loss risk)

---

### GAP-DATASHEET-V05-07: AddRowAsync doesn't move active cell to new row
**Severity:** Low
**Spec:** `bulk-operations-and-saveall.md:119` — "The active cell moves to the first editable column of the new row."

**Current:** `AddRowAsync` does not touch `_activeCellRow` / `_activeCellField`.

**Consequence:** Minor UX — after Add Row, the user must click or tab to begin editing, rather than the cursor being placed automatically.

**Recommended direction:** Find the first editable non-computed column and call `ActivateCell(newItem, firstEditableColumn.Field)` after the insert.

**Status:** Open — confirmed real gap (UX polish)

---

### GAP-DATASHEET-V05-08: ResetAsync doesn't remove newly-added rows
**Severity:** Medium
**Spec:** `bulk-operations-and-saveall.md:161` — "**Added rows (created via Add Row but not yet saved) are removed.**"

**Current:** `ResetAsync` at `.Data.cs:235-255` iterates `_dirtyRows.Values` and restores original values, then clears `_dirtyRows`. **It does not remove rows from `_displayRows`.** Since newly-added rows are only in `_displayRows` (not tracked anywhere else), Reset leaves them in place.

**Consequence:** User clicks Add Row (adds 3 rows), then clicks Reset. Spec says all 3 new rows should disappear. Source keeps them in the list.

**Recommended direction:** After V05-06 is fixed (added rows have entries), the ResetAsync logic should detect "added" entries (perhaps via a new `IsNewlyAdded` flag on DirtyRowEntry, or by checking `Original == null` / sentinel) and remove the corresponding row from `_displayRows` before clearing `_dirtyRows`.

**Status:** Open — confirmed real gap (depends on V05-06 for clean fix)

---

## Items Flagged for Further Verification (not confirmed gaps)

### V05-R1: ValidateAllAsync `// Also validate required fields` comment — dead code or placeholder?
**Spec:** `bulk-operations-and-saveall.md:42` — "runs required + column-level `Validate` delegates on each dirty field"
**Current:** `.Data.cs:163-164` has the comment `// Also validate required fields on all dirty rows` followed only by `StateHasChanged(); await Task.CompletedTask;` — no additional required-field logic. The required check happens inside `RunColumnValidation` (called at line 150), but the comment suggests an additional pass was planned.
**What to check:** Read `RunColumnValidation` implementation to confirm required check is inside it. If yes, delete the misleading comment. If no, implement the planned required pass.

### V05-R2: ValidateAllAsync iterates all columns on dirty rows, not just dirty fields
**Spec:** `bulk-operations-and-saveall.md:42` — "runs required + column-level `Validate` delegates on **each dirty field**"
**Current:** `.Data.cs:146-147` — `foreach (var column in _columns)` iterates ALL columns, not `entry.DirtyFields`. Then inner check skips non-editable/computed. **This validates untouched fields as well as dirty ones.**
**Risk:** Broader than spec (stricter). Not incorrect but may produce validation errors on fields the user hasn't touched. Could be intentional (e.g., catching data that was invalid at load time). Worth confirming the design intent.

### V05-R3: Deleted-dirty row appears only in DeletedRows
**Spec:** `bulk-operations-and-saveall.md:90` — "A row that is both dirty and deleted appears **only** in `DeletedRows`, not in `DirtyRows`."
**Current:** `.Data.cs:180-183` filters `Where(e => e.DirtyFields.Count > 0 && !e.IsDeleted)` for `dirtyRowsList`, and `.Data.cs:216-217` filters `Where(e => e.IsDeleted)` for `deletedRows`. These filters appear to be mutually exclusive ✓ — but a row that's both dirty AND deleted is caught by the deleted filter only (correct per spec). **Confirmed on read**, no gap.

---

## Summary of V05 Results

| Item | Severity | Type | Status |
|---|---|---|---|
| V05-01 CellState.Saving never set | Medium | Missing state transition | Confirmed |
| V05-02 No post-save state transitions | Medium | Missing post-save logic | Confirmed |
| V05-03 Deleted rows not removed from `_displayRows` | **High** | User-visible state bug | Confirmed |
| V05-04 Original snapshots not refreshed after save | Medium | Subtle correctness bug | Confirmed |
| V05-05 AddRowAsync inserts at top (spec: end) | Medium | Spec compliance (one-line fix) | Confirmed |
| V05-06 New row not marked dirty | Medium | **Data loss risk** | Confirmed |
| V05-07 AddRowAsync doesn't move active cell | Low | UX polish | Confirmed |
| V05-08 ResetAsync doesn't remove added rows | Medium | Correctness | Confirmed |
| V05-R1 ValidateAllAsync dead-code comment | ? | Code hygiene | Open |
| V05-R2 Column iteration vs dirty-field iteration | ? | Design intent | Open |
| V05-R3 Deleted-dirty row exclusion | — | ✅ **Confirmed matches spec** | Closed |

**8 confirmed gaps + 2 re-verify items + 1 closed-as-correct.** V05-03 and V05-06 are the highest-severity — V05-03 causes deleted rows to persist visibly after save, V05-06 causes newly-added rows to silently not save.

## Corrections to Prior Findings

| Prior finding | Original status | Corrected status |
|---|---|---|
| **V02-01** (deep-clone snapshot) | High — confirmed latent bug | ✅ **Invalidated** — deep clone exists (lazy, in CommitCellEdit). False positive. |
| **V07-08** (aria-live wiring) | Medium — structural wiring absent | Downgraded — 2 of 5 spec-required announcements wired; 3 missing (dirty count, saving start, validation errors appear) |

**Updated confirmed latent bug count:** was 4 → now **3** (V03-07, V07-04, V01-01 pending). V02-01 removed as false positive.

**Updated confirmed gap count overall:** was 17 → now **24** (16 prior - V02-01 + 8 V05 + 1 V01 = 24).

## Recommended Stage 03 Integration

**V05-01 through V05-08** constitute a new **Batch C — DataSheet Save All Lifecycle Correctness**:

- Phase C1: V05-01 + V05-02 (CellState.Saving/Saved/Pristine transitions)
- Phase C2: V05-03 + V05-04 (Remove deleted rows + refresh Original snapshots after save)
- Phase C3: V05-05 + V05-06 + V05-07 (AddRow fixes: append not insert, mark dirty, move active cell)
- Phase C4: V05-08 (ResetAsync removes added rows — depends on V05-06's flag)

Estimated scope: 2-3 commits, ~15-20 bUnit tests. Similar size to Batch B (Dirty Tracking Correctness — which is now EMPTY because V02-01 is invalidated).

**Revised Stage 03 batch layout after V05:**

- **Batch A** — Range Selection + Keyboard + A11y + Column filter (V03+V07+V04-01+V01-01 = **15 gaps**)
- ~~Batch B — Dirty Tracking Correctness (V02-01)~~ → **DROPPED** (false positive)
- **Batch C** — Save All Lifecycle Correctness (V05-01 through V05-08 = **8 gaps**)
- **Batch D** (optional) — Aria-live announcements completion (V07-08 partial: add the 3 missing announcements)

## Human Decisions Needed

**Zero.** All 8 V05 gaps have clear spec-grounded fix directions.

## Stage 01b Status After V05

- ✅ V01 complete (fire #16)
- ✅ V02 complete (fire #13) — **V02-01 now invalidated by V05 finding**
- ✅ V03 complete (fire #10/#11)
- ✅ V04 complete (fire #14)
- ✅ **V05 complete (this fire)** — 8 confirmed gaps + 2 re-verify + 1 closed-correct + 2 prior-finding corrections
- ⏳ V06, V08 — 2 remaining
- ✅ V07 complete (fire #12) — **V07-08 now downgraded**
- ✅ V09 complete (fire #15)
- ✅ V10 complete (fire #15)

**8 of 10 sub-tasks complete. 24 confirmed implementation gaps opened.** V06 and V08 are the last untouched sub-tasks, both expected to be lower-yield.
