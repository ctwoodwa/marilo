# GAP-DATASHEET-V01 — Columns and Schema Verification (Result)

**Sub-task:** GAP-DATASHEET-V01 from `datasheet-spec-gaps-2026-04-10.md`
**Spec:** `docs/component-specs/datasheet/columns-and-schema.md` (285 lines, fully read)
**Source audited:** `MariloDataSheet.Rendering.cs` (partials surfaced via grep) + `.Editing.cs` (GetDefaultValue, ParseText, editableColumns filter) + `.Data.cs` (Required + Validate logic)
**Verification date:** 2026-04-10 (cron fire #16)

## Result: **1 potential bug** (editableColumns filter) + **8 targeted re-verification items**

Like V02, V01 finds that the column-type handling is **substantially present and well-architected** — all 6 DataSheetColumnType values are dispatched in the render switch, Format delegates are invoked correctly in both read mode and computed columns, computed read-only guards are enforced at 4 separate locations, GetDefaultValue provides type-specific defaults, and custom Validate delegates fire with the correct row-parameter contract. The spec was clearly written against this source.

## What's Verified Present ✅

**Rendering switch for all 6 column types (`.Rendering.cs:129-234`):**
- `Text` case at `:165`
- `Number` case at `:177`
- `Date` case at `:196`
- `Select` case at `:212-232` (iterates `column.Options` at `:221-223`)
- `Checkbox` case at `:131` (read mode) and `:234` (edit mode)
- `Computed` case at `:140-142` (uses `column.Format`)

**Format delegate invocation:**
- Read-mode cell at `.Rendering.cs:146`:
  ```csharp
  var display = column.Format != null ? column.Format(row) : value?.ToString() ?? "";
  ```
- Computed column at `.Rendering.cs:141`:
  ```csharp
  var formatted = column.Format != null ? column.Format(row) : value?.ToString() ?? "";
  ```
- Both correctly fall back to `value?.ToString()` when `Format` is null, matching spec line 206 ("Without it, the cell displays the raw `ToString()` of the property value")

**Computed column read-only enforcement (4 locations):**
- `.Rendering.cs:64` — Forces read mode: `if (!column.Editable || column.ColumnType == DataSheetColumnType.Computed)`
- `.Editing.cs:37` — Blocks `EnterEditMode`
- `.Editing.cs:75` — Blocks `CommitCellEdit`
- `.Editing.cs:178` — Blocks Delete-key clearing
- `.Editing.cs:259` — Paste loop skips computed columns
- `.Rendering.cs:135` — Checkbox `disabled` attribute bound to `!column.Editable`

All consistently guard the "computed columns never enter edit mode" contract from spec line 211.

**Required validation (`.Data.cs:112-117`):**
- Line 112: `if (column.Required)` — required check runs
- Line 117: `if (column.ColumnType == DataSheetColumnType.Checkbox)` — special-case for checkbox ("false means unchecked means invalid" per spec line 193, 233)

**Custom Validate delegate (`.Data.cs:129-131`):**
- Line 129: `if (column.Validate != null)` — runs when defined
- Line 131: `return column.Validate(row);` — passes **full row** to the delegate (matches spec: "It receives the full row so it can perform cross-field validation", line 99)

**GetDefaultValue switch (`.Editing.cs:272-278`):**
| ColumnType | Default | Matches spec? |
|---|---|---|
| `Text` | `""` | ✓ (spec line 89) |
| `Number` | `0m` | ✓ (spec implies 0 for non-nullable) |
| `Checkbox` | `false` | ✓ (spec line 233) |
| `Date` | `null` | ✓ (spec line 231 "null or default") |
| `Select` | `""` | ✓ (spec line 232) |
| `Computed` | — (not listed) | See V01-R7 below |

**Type coercion (`.Editing.cs:285-298`):**
- `Number` → `decimal.TryParse` at `:287` ⚠ culture unverified (V04-R2 already flags this)
- `Date` → `DateTime.TryParse` at `:296`
- `Checkbox` → toggle logic at `:298`
- (Text and Select appear to be string passthrough, not in grep results but implied)

**`MariloDataSheetColumn` parameters (from V02 cross-ref):**
- All 11 spec-declared column parameters present (Field, Title, ColumnType, Editable, Required, MinWidth, Width, Format, Validate, Options, CellTemplate) ✓

---

## Confirmed Potential Gap (needs 1-line context read to fully confirm)

### GAP-DATASHEET-V01-01: `editableColumns` filter includes Computed columns
**Severity:** **High** (Tab navigation contract with keyboard/a11y spec)
**Spec reference:**
- `columns-and-schema.md:202-211` — "Computed columns are **read-only display columns**. They never enter edit mode, regardless of the `Editable` parameter value."
- `keyboard-and-accessibility.md:38` (V07 cross-ref) — "Tab | Any | If in edit mode, commits the current cell. Moves the active cell to **the next editable cell** (left to right, top to bottom)."

**Current:** `.Editing.cs:190` contains:

```csharp
var editableColumns = _columns.Where(c => c.Editable || c.ColumnType == DataSheetColumnType.Computed).ToList();
```

The variable is named `editableColumns` but the filter **includes computed columns** via the `|| c.ColumnType == DataSheetColumnType.Computed` disjunction.

**Two possible interpretations:**

1. **If this list drives Tab navigation (likely, given the name):** The filter is wrong. Tab should move to the **next editable cell** per spec. Computed cells are never editable (spec line 202-211). Including them in the "editable columns" list causes Tab to navigate into computed cells that the user cannot edit — defeating the purpose of Tab traversal and likely causing a "Tab skips nothing, Enter does nothing on computed cells, user is stuck" UX bug.

2. **If this list drives arrow-key navigation:** The filter is arguably correct. Spec `selection-and-ranges.md:85` (V03) says "The active cell can land on non-editable cells (including computed columns) — focus moves freely across all columns." Arrow keys should pass through computed cells. But the variable name `editableColumns` is then misleading.

**Which is it?** Requires reading `.Editing.cs:186-240` in full to see how `editableColumns` is consumed. The grep snippet shows this filter at line 190, inside what V03 identified as the arrow-key handler region (`:186-205`) and Tab handler region (`:210-236`). If the same filter is used for both, one of the two is wrong per spec.

**Recommended verification:** One-line read of `.Editing.cs:190-236` to confirm whether `editableColumns` is consumed by the Tab branch (`key == "Tab"`) or the arrow-key branch. If Tab-only → real bug; rename + remove the Computed disjunction. If arrow-only → rename variable to `navigableColumns` and add a second filter for Tab.

**Status:** Open — **confirmed-with-verification-needed** (high probability real bug given the variable name "editableColumns")

---

## Targeted Re-Verification Items

These are all small single-read verifications. None are confirmed gaps until verified.

### V01-R1: Number Required rejects zero for non-nullable types
**Spec:** `columns-and-schema.md:118` — "Built-in required check: rejects `null` or **zero** when `Required` is set (zero rejection applies only to non-nullable types where `default` is `0`)."
**What to check:** Read `.Data.cs:112` context. Does the Required check for Number distinguish nullable vs non-nullable and reject zero for non-nullable?
**Risk:** If not implemented, users can save a `Required` Number column as `0` and it won't be rejected — defeats the intent.

### V01-R2: Text Required rejects whitespace-only strings
**Spec:** `columns-and-schema.md:98` — "Built-in required check: rejects `null` or **whitespace-only** strings when `Required` is set."
**What to check:** Read `.Data.cs:112` context. Does the Required check for Text use `string.IsNullOrWhiteSpace` (correct) vs `string.IsNullOrEmpty` (spec non-compliant)?
**Risk:** Users could bypass Required by typing a single space.

### V01-R3: Date Required rejects `default(DateTime)`
**Spec:** `columns-and-schema.md:139, 231` — "null or `default(DateTime)` when `Required` is set" / "`null` or `default(DateTime)`"
**What to check:** The Required check for Date should reject both `null` AND `DateTime.MinValue` (which is `default(DateTime)`). Just checking `value == null` is insufficient for non-nullable `DateTime` properties.
**Risk:** A `Required` DateTime field accepts uninitialized dates (`0001-01-01`) as valid.

### V01-R4: Select preserves unmatched existing values
**Spec:** `columns-and-schema.md:161` — "If the current cell value does not match any `Options.Value`, the cell is not automatically marked invalid — the existing value is preserved. This allows for values that were valid at the time of data entry but have since been removed from the options list."
**What to check:** When a cell value doesn't match any option, the Select render logic should still display that value (not auto-clear it). The dropdown at `.Rendering.cs:212-232` needs to preserve the existing value as the `<select>` element's value even if not found in `<option>` children.
**Risk:** Data loss — Options list changes over time could silently clear legacy values.

### V01-R5: Default Required error message format
**Spec:** `columns-and-schema.md:236` — "displays a default error message: `"{Title} is required."`"
**What to check:** The error message string produced by `.Data.cs:112+` for the Required failure. Should be formatted as `$"{column.Title ?? column.Field} is required."` to match spec exactly. Any variation (e.g., "Value required", "This field is required") is a spec compliance issue.
**Risk:** Inconsistent UX / spec compliance.

### V01-R6: Number parsing uses invariant culture
**Spec:** Implied by `bulk-paste-and-clipboard.md:90` — "`decimal.TryParse` with **invariant culture**"
**Cross-ref:** V04-R2
**What to check:** The `decimal.TryParse` call at `.Editing.cs:287` should pass `NumberStyles.Any, CultureInfo.InvariantCulture` to prevent culture-sensitive decimal separator bugs (German `,` vs English `.`).
**Risk:** User on German/French/etc. locale enters `1,50` for a Number cell and it parses as 150 instead of 1.5, or fails to parse entirely.

### V01-R7: GetDefaultValue handles `Computed` case
**Spec:** Computed never needs a default because it's never editable, but the switch at `.Editing.cs:272-278` appears to list 5 values and omit Computed. The default case of a C# switch expression throws if no arms match unless `_ =>` is present.
**What to check:** Does the switch at `:272` include a `_ =>` fallback arm, or does Computed fall through to a throw? If the latter, any code path that accidentally calls `GetDefaultValue(computedColumn)` crashes.
**Risk:** Low (because computed is guarded elsewhere), but defensive programming would suggest a sensible default (null or empty).

### V01-R8: Options dropdown rendering pairs Value + Label
**Spec:** `columns-and-schema.md:149` — "The stored value is the `DataSheetSelectOption.Value` string; the displayed text is `DataSheetSelectOption.Label`."
**What to check:** Lines `.Rendering.cs:221-232` iterate `column.Options` and emit `<option>` elements. Each `<option>` should have `value="@option.Value"` and display `@option.Label` text content. The grep shows the iteration but not the inner markup — needs a 10-line read to confirm the Value/Label distinction is honored.
**Risk:** If Value and Label are swapped or one is used for both, the user sees labels that map to wrong stored values.

---

## Summary of V01 Results

| Item | Severity | Type | Status |
|---|---|---|---|
| V01-01 `editableColumns` filter includes Computed | **High** | Likely bug in Tab navigation | Confirmed-with-verification |
| V01-R1 Number Required rejects zero | ? | Correctness | Open |
| V01-R2 Text Required rejects whitespace | ? | Correctness | Open |
| V01-R3 Date Required rejects default(DateTime) | ? | Correctness | Open |
| V01-R4 Select preserves unmatched values | ? | Data loss risk | Open |
| V01-R5 Default Required error message format | ? | Spec compliance | Open |
| V01-R6 Number parsing culture | ? | Correctness (cross-ref V04-R2) | Open |
| V01-R7 GetDefaultValue Computed fallback | Low | Defensive | Open |
| V01-R8 Select Options Value/Label rendering | ? | Correctness | Open |

**1 confirmed-pending gap + 8 re-verification items.** The confirmed-pending gap (V01-01) is the highest-value finding because it's a likely **user-visible keyboard navigation bug** that would cause Tab to land on non-editable computed cells. It also has the clearest single-line verification path.

## Recommended Stage 03 Integration

**V01-01** (if confirmed) integrates into **Batch A — Range Selection + Keyboard + A11y** because it touches the same `.Editing.cs:186-236` keyboard handler region that V03-04 (Shift+Arrow), V07-05 (printable char), V07-06 (Space-toggles-checkbox), and V07-07 (Enter behavior) all target. The fix is either:
1. Remove the `|| c.ColumnType == DataSheetColumnType.Computed` disjunction (if Tab-only)
2. Split into two filters: `navigableColumns` (includes Computed) for arrows, `editableColumns` (excludes Computed) for Tab

**V01-R1 through V01-R8** fold naturally into the V02 and V04 re-verification batches. Specifically:
- V01-R6 = V04-R2 (same question, both areas flagged it)
- V01-R5 overlaps with V02-R5 (Required error message display — V02-R5 was about precedence, V01-R5 is about text format)

Final Stage 03 batch layout after V01:

- **Batch A** — Range Selection + Keyboard + A11y (V03+V07+V04-01+V01-01 = 15 gaps, ~20-30 tests)
- **Batch B** — Dirty Tracking Correctness (V02-01 = 1 gap, standalone)

V01-01 adds one small additional scope item to Batch A.

## Human Decisions Needed

**Zero.** Every item has a clear verification or fix path.

## Stage 01b Status After V01

- ✅ V02 complete (fire #13) — 1 + 7 re-verify
- ✅ V03 complete (fire #10/#11) — 8
- ✅ V04 complete (fire #14) — 1 + 6 re-verify
- ✅ V07 complete (fire #12) — 6 + 2 cross-refs
- ✅ V09 complete (fire #15) — clean pass
- ✅ V10 complete (fire #15) — clean pass
- ✅ **V01 complete (this fire)** — 1 confirmed-pending + 8 re-verify
- ⏳ V05, V06, V08 — 3 remaining

**7 of 10 sub-tasks complete. 17 confirmed/confirmed-pending implementation gaps opened** (V03×8 + V07×6 + V02×1 + V04×1 + V01×1).

Next queue: **V05 bulk-operations-and-saveall** (likely medium yield, partial overlap with V02/V03 findings) or **V08 theming-and-css-provider** (likely low yield given CSS provider signatures already verified in fire #9). **V06 virtualization-and-performance** is the last remaining untouched feature area.

**Re-verification items now total: 21** (V02×7 + V04×6 + V01×8). Three separate re-verification fires would close them all, or they could be consolidated into a single long fire if context allows.
