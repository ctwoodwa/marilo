# GAP-DATASHEET-V04 — Bulk Paste and Clipboard Verification (Result)

**Sub-task:** GAP-DATASHEET-V04 from `datasheet-spec-gaps-2026-04-10.md`
**Spec:** `docs/component-specs/datasheet/bulk-paste-and-clipboard.md` (159 lines, fully read)
**Source audited:** `MariloDataSheet.Interop.cs` (87 lines, fully read) + `.Editing.cs:246-263` (paste handler start from V03 evidence) + cross-ref to V03-06 and V07-01
**Verification date:** 2026-04-10 (cron fire #14)

## Result: **1 new structural gap** (orphaned copy bridge) + **5 targeted re-verification items** for paste execution details

V04 is a nuanced audit. Unlike V03 (missing subsystem) or V07 (missing WCAG attributes), V04 found that the **clipboard infrastructure is largely present but half-wired**: the JS interop module exists, the copy bridge method exists, the paste handler stub exists, and the `AllowBulkPaste` parameter exists. But the copy path is **completely unreachable** due to V03-06 + V03-01, and the paste handler's per-`ColumnType` coercion logic was not verified in this fire (flagged for focused re-verification).

## What's Verified Present ✅

**JS interop infrastructure (`.Interop.cs`):**

- `IJSObjectReference _jsModule` field — lazy-loaded from `./_content/Marilo.Components/js/marilo-datasheet.js` (line 26)
- `DotNetObjectReference<MariloDataSheet<TItem>>` for .NET callbacks (line 14)
- `_gridId = $"mar-datasheet-{Guid.NewGuid():N}"` per-instance identifier (line 15)
- `registerKeydownHandler(gridId, dotNetRef)` registered on first render (line 28)
- `unregisterKeydownHandler(gridId)` in `DisposeAsync` (line 80)
- `IAsyncDisposable` implementation (line 9)

**`CopyToClipboardAsync(string text)` method (`.Interop.cs:50-60`):**

```csharp
internal async Task CopyToClipboardAsync(string text)
{
    if (_jsModule != null)
    {
        try { await _jsModule.InvokeVoidAsync("copyToClipboard", text); }
        catch (JSDisconnectedException) { }
    }
}
```

- Internal visibility (appropriate for component-internal use)
- Proper null-check on `_jsModule`
- Proper `JSDisconnectedException` handling

**`ScrollToRowAsync(object key)` public method (`.Interop.cs:38-48`):**

- Spec-declared method from overview; calls `scrollToRow` JS function
- Same defensive pattern as `CopyToClipboardAsync`

**`FocusCellAsync(object rowKey, string field)` internal method (`.Interop.cs:62-72`):**

- Not in the spec's public API but used for internal focus management
- Calls `focusCell` JS function

**Paste handler entry point (`.Editing.cs:246-263`, partial — full method not read this fire):**

From V03 evidence:
- Line 246: `if (!AllowBulkPaste || _activeCellRow is null || _activeCellField is null) return;`
- Line 248: `var startRowIdx = _displayRows.IndexOf(_activeCellRow);`
- Line 263: `await CommitCellEdit(row, column.Field, parsedValue);`

**Correctly handled spec gates:**
- ✅ `AllowBulkPaste` parameter honored (spec line 65)
- ✅ Active cell required (spec line 66 "No active cell exists" → paste disabled)
- ✅ Commits via the standard `CommitCellEdit` path (triggers validation + dirty tracking per spec line 60)

**`AllowBulkPaste` parameter (`.razor.cs:51`):**

- Default `true` matches spec
- Documented XML comment: "Enables Ctrl+V TSV paste into cell range"

---

## Confirmed Gap

### GAP-DATASHEET-V04-01: `CopyToClipboardAsync` method is orphaned — no call sites
**Severity:** Medium (dead code that can't be triggered)
**Spec:** `bulk-paste-and-clipboard.md:28-47` (entire Copy Behavior section)

**Current state:**
- The `CopyToClipboardAsync(string text)` method exists at `.Interop.cs:50-60` and correctly delegates to the `copyToClipboard` JS function.
- **No call sites** for `CopyToClipboardAsync` exist anywhere in the component (V03 grep and this fire's Interop.cs read both confirm).
- **No code builds a TSV string from a selection** — there is no method like `BuildTsvFromRange()` or similar.
- **No Ctrl+C keyboard handler** in `.Editing.cs` — V03-06 already confirmed this.

**Dependency chain** (all downstream of V03-01):

1. V03-01: no range state → nothing to copy from
2. No "build TSV from range" method → nothing to send to the clipboard bridge
3. V03-06: no Ctrl+C keyboard handler → the user cannot trigger copy
4. V04-01: `CopyToClipboardAsync` exists as orphaned plumbing

**Consequence:** The copy path is entirely dead code. Users cannot copy selected cells. The scaffolding suggests the feature was partially implemented then left dormant — possibly because the range-selection dependency wasn't in place.

**Recommended direction:** Resolve as part of V03+V07's consolidated **Batch A** (`Range Selection + Keyboard + A11y`), specifically the **Commands phase**. The fix sequence:

1. Land V03-01 (range state fields) — provides the data source
2. Add a `BuildTsvFromRange()` internal method in `.Data.cs` or `.Editing.cs` that iterates the range's rows × columns, reads each cell value via `GridReflectionHelper.GetValue` (already used at `.Editing.cs:142`), respects computed columns by reading the raw field value (per spec line 37 "raw value, not formatted display string"), tab-separates cells within a row, newline-separates rows
3. Wire V03-06 (Ctrl+C handler) to call `CopyToClipboardAsync(BuildTsvFromRange())`
4. Done — the orphaned bridge method becomes live

No new JS interop work needed; the `copyToClipboard` JS function already exists.

**Status:** Open — confirmed dead code, resolvable as part of Batch A

---

## Targeted Re-Verification Items (Stage 01b continuation)

These are **not confirmed gaps** — they are paste-execution details that require reading the full `.Editing.cs:246-320`ish range to verify. Each is a small focused audit.

### V04-R1: TSV parsing (rows split by `\n`, columns by `\t`)
**Spec:** `bulk-paste-and-clipboard.md:57` — "parses the TSV into a two-dimensional array of strings (split by `\n` for rows, `\t` for columns)"

**What to check:** The paste method body must split `tsvData` by `\n` to get rows, then each row by `\t` to get cells. Windows clipboard may produce `\r\n` line endings which should also be handled.

**Risk if wrong:** Pasting from Windows Excel could produce rows with trailing `\r` characters appended to the last cell value, causing type-coercion failures for Number/Date columns.

---

### V04-R2: Per-`ColumnType` coercion matches spec table
**Spec:** `bulk-paste-and-clipboard.md:85-96` (6 rows: Text, Number, Date, Select, Checkbox, Computed)

**What to check:** The paste method must have a switch/match statement on `column.ColumnType` (or equivalent) with six branches:
- `Text` → as-is, never fails
- `Number` → `decimal.TryParse` with **invariant culture**; fail → `CellState.Invalid` with "Invalid number"
- `Date` → `DateTime.TryParse` with **current culture**; fail → "Invalid date"
- `Select` → case-sensitive match against `column.Options.Value`; fail → "Value not in options"
- `Checkbox` → `"true"`/`"1"` case-insensitive → `true`; else `false`; never fails
- `Computed` → skipped entirely, not assigned any value

**Risk if wrong:** Culture-sensitive number parsing (e.g., German decimal separator) could silently convert wrong values. Missing "invariant culture" is a common bug in C# numeric parsing code.

---

### V04-R3: Computed and non-editable columns skipped during paste
**Spec:** `bulk-paste-and-clipboard.md:80` — "Computed columns and columns with `Editable='false'` are **skipped** during paste. The paste cursor advances past them, and the next pasted value maps to the next editable column."

**What to check:** In the paste column-iteration loop, both `column.ColumnType == Computed` and `column.Editable == false` must be skipped. Critically, the **paste cursor (TSV column index) must advance** past the spec skip to the next TSV cell — i.e., TSV column N maps to DataSheet editable column M where the skip preserves one-to-one editable mapping per the worked example at lines 122-144.

**Risk if wrong:** Pasting data into a grid with a computed column in position 3 would incorrectly shift the paste cursor, causing TSV column 3's data to land in DataSheet column 4 instead of being "consumed by the skip and next TSV value landing in column 4".

**Note:** The spec's worked example at `bulk-paste-and-clipboard.md:112-144` is explicit about this semantics. Re-verification should walk the worked example step-by-step to confirm the source matches.

---

### V04-R4: Deleted rows skipped during paste
**Spec:** `bulk-paste-and-clipboard.md:81` — "Rows marked for deletion are skipped during paste."

**What to check:** The paste row-iteration loop must check `IsRowDeleted` (or equivalent lookup) and advance past deleted rows. The `_dirtyRows.Values.Where(e => e.IsDeleted)` pattern from `.Data.cs:217` suggests the machinery is available.

**Risk if wrong:** Paste data lands in rows the user marked for deletion — confusing and silently lost on save.

---

### V04-R5: Best-effort error handling — failed coercion does not write raw string
**Spec:** `bulk-paste-and-clipboard.md:101-106`:
> "Each cell in the paste region is processed independently. A failure in one cell does not abort the paste for other cells. Cells that fail type coercion are marked `CellState.Invalid` with an appropriate error message. **The raw pasted string is not written to the model** — the cell retains its previous value."

**What to check:** When coercion fails (e.g., TryParse returns false), the paste handler must:
1. **Not call `CommitCellEdit` with the raw string** (that would mutate the model to an invalid state)
2. Mark the cell `CellState.Invalid` — the current architecture uses `entry.ValidationErrors[field] = errorMessage`
3. Continue to the next cell in the paste region

**Risk if wrong:** If coercion failure still calls `CommitCellEdit(row, field, rawString)`, the model's typed property receives a string value which may throw at reflection-set time (boxing mismatch) or silently store a wrong value. This is a correctness bug with user-visible consequences.

**Also:** Spec line 106 says "The `OnRowChanged` event fires once for each successfully committed cell during paste, not once for the entire paste operation." This depends on the commit path matching spec.

---

### V04-R6: IsSaving gate during paste
**Spec:** `bulk-paste-and-clipboard.md:67` — "Paste is disabled when: The DataSheet is in a saving state (`IsSaving=true`)."

**What to check:** The paste handler's early guard at `.Editing.cs:246` currently checks `!AllowBulkPaste || _activeCellRow is null || _activeCellField is null`. It may or may not also check `|| IsSaving`. Needs one-line verification.

**Risk if wrong:** Users could paste in the middle of a Save All flow, creating race conditions between the save payload snapshot and the paste mutations.

---

## Summary of V04 Results

| Item | Severity | Type | Status |
|---|---|---|---|
| V04-01 Orphaned `CopyToClipboardAsync` | Medium | Dead code (dependency chain) | **Confirmed** (resolvable via Batch A) |
| V04-R1 TSV parsing (`\r\n` handling) | ? | Needs method body read | Open (verification) |
| V04-R2 Per-ColumnType coercion | ? | Needs method body read — **highest-risk re-verify** | Open (verification) |
| V04-R3 Computed/non-editable skip | ? | Needs method body read | Open (verification) |
| V04-R4 Deleted-row skip | ? | Needs method body read | Open (verification) |
| V04-R5 Best-effort error handling | ? | Needs method body read — **highest-risk re-verify** | Open (verification) |
| V04-R6 IsSaving gate | ? | Single-line check | Open (verification) |

**1 confirmed + 6 targeted re-verification items.** The two highest-risk re-verify items are **V04-R2** (per-ColumnType coercion with culture sensitivity) and **V04-R5** (best-effort error handling — "do not write raw string on coercion failure"). Both are correctness concerns that could produce data-corruption-adjacent bugs if implemented wrong.

## Recommended Stage 03 Integration

**V04-01 folds into Batch A:** it's the dead-code half of the V03-06 Ctrl+C gap. Completing Batch A (V03+V07 consolidated) automatically resolves V04-01 as a side effect.

**V04-R2 through V04-R6** should be a **single follow-up sub-audit** — one focused read of `.Editing.cs:246` through the end of the paste method body. Expected outcome: most rules are probably implemented correctly (since the spec was written against this source), but any one of R2/R3/R5 could turn out to be a subtle bug because the paste path is complex and culture/type-coercion logic is easy to get wrong.

## Human Decisions Needed

**Zero.** All V04 findings have clear spec-grounded paths forward.

## Stage 01b Status After V04

- ✅ V03 complete (fire #10/#11) — 8 sub-gaps
- ✅ V07 complete (fire #12) — 6 sub-gaps + 2 cross-refs
- ✅ V02 complete (fire #13) — 1 confirmed + 7 re-verify + 1 cross-ref
- ✅ **V04 complete (this fire)** — 1 confirmed + 6 re-verify
- ⏳ V01, V05, V06, V08, V09, V10 — 6 remaining

**4 of 10 sub-tasks complete. 16 confirmed implementation gaps opened** (V03×8 + V07×6 + V02×1 + V04×1).

**Consolidated Stage 03 batches ready to queue:**
- **Batch A** — Range Selection + Keyboard + A11y (V03+V07 = 14 gaps; resolving also closes V04-01 as a side effect)
- **Batch B** — Dirty Tracking Correctness (V02-01 = 1 gap, standalone)

Both batches remain decision-independent and parallel-safe.

**Re-verification queue (could be a single fire each):**
- V02-R2 through V02-R8 (7 items — can batch into one fire)
- V04-R1 through V04-R6 (6 items — can batch into one fire, ideally paired with a read of `.Editing.cs:246-end`)

Next highest-yield verification: **V01 columns-and-schema** or **V05 bulk-operations-and-saveall**. Both are likely to surface fewer gaps than V03/V07 (which hit the feature-gap jackpot) but more than the polish-oriented V06/V08.
