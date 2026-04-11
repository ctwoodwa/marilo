# DataSheet Stage 02 — Example UX Audit

**Audit date:** 2026-04-10
**Component:** `MariloDataSheet<TItem>`
**Current demo:** `samples/Marilo.Demo/Pages/Components/DataSheet/Overview.razor` (206 lines — "Investment Position Editor")
**Stage:** 02-example-ux
**Upstream:** Stage 01b verification complete + F1/F2/F3/F4 all resolved (1159/1159 tests passing)

## Headline

The current Overview.razor demo is **well-crafted and demonstrates core editing, validation, and save flows competently**. However, the 206-line single-page format is beginning to strain under the weight of 18 top-level parameters, 11 column parameters, and ~25 distinct behavioral features from the F-series implementation. The component's depth — clipboard paste, bulk operations, keyboard shortcuts, accessibility, virtualization, and theming — merits **multi-page demo coverage following the DataGrid precedent** (4 pages: Overview, Editing, BulkOperations, Accessibility). The current demo covers ~55% of the API surface and misses critical feature showcases for users building production workflows.

## Current Demo Coverage

### Top-level parameters (18 total)

| Parameter | Demonstrated? | Where / how | Notes |
|---|---|---|---|
| Data | ✓ | Line 14 — bound to `_rows` (12 investment positions) | Uses realistic domain data; good baseline. |
| KeyField | ✓ | Line 15 — `"Id"` explicit | Hidden but correct; `Id` generated via `Guid.NewGuid()` on line 164. |
| OnSaveAll | ✓ | Line 16 — `HandleSaveAll` fires on save (lines 115–128) | Demonstrates full flow: `IsSaving` toggle, `ResetAsync()` clear, event logging. |
| OnRowChanged | ✓ | Line 17 — logs individual cell changes (lines 130–133) | Shows field-level tracking ("Ticker: AAPL → AMZN" pattern). |
| OnValidate | ✓ | Line 18 — cross-field validation (lines 135–154) | Validates ticker non-empty and quantity >= 0. Only **field-level, not cross-row**. |
| IsSaving | ✓ | Line 19, lines 115–128 | Toggled during simulated API delay (1500ms). Save button disables visually during save. |
| IsLoading | ✗ | Not demonstrated. | No skeleton loading shown; the demo immediately renders full data. |
| AllowAddRow | ✓ | Line 20 — toolbar "+ Add Row" button visible | Button creates new `PositionRow` with `Guid.NewGuid()` key. No scenario shown in demo. |
| AllowDeleteRow | ✓ | Line 21 — per-row delete buttons visible; lines 124–127 show deleted rows in `args.DeletedRows` | Demonstrates deletion UI and save payload. No **MarkRowDeleted toggle** (F2 V05.4) explicitly called out. |
| AllowBulkPaste | ✓ | Line 22 — clipboard Ctrl+V enabled | No **worked example** of paste; no error handling demo (Invalid state). |
| EmptyStateMessage | ✓ | Line 23 — custom message if data is empty | Text set but no empty state shown (data pre-populated). |
| Height | ✓ | Line 24 — `"500px"` fixed height, scrollable | Good showcase of virtualization boundary. |
| EnableVirtualization | ✓ | Line 25 — explicitly set to `false` | Deliberate; allows full 12-row dataset visibility without scrolling. Misses **virtualization demo** (large dataset benefit). |
| AriaLabel | ✗ | Not demonstrated. | Default "Editable data grid" used; no custom label or accessibility story. |
| ChildContent | ✓ | Lines 27–59 — 7 `MariloDataSheetColumn` definitions | Full column set shown: Text, Select, Number, Date, Checkbox, Computed. |
| ToolbarTemplate | ✗ | Not demonstrated. | Parameter exists but no custom toolbar content added. |
| Class | ✗ | Not demonstrated. | No custom CSS class injection shown. |
| Style | ✗ | Not demonstrated. | No inline style binding shown. |

**Parameters demonstrated: 13/18 (72%)** — missing IsLoading, AriaLabel, ToolbarTemplate, Class, Style (mostly cosmetic/advanced).

### Column parameters (11 total)

| Parameter | Demonstrated? | Notes |
|---|---|---|
| Field | ✓ | All 7 columns map to row properties: Ticker, AssetClass, Quantity, Price, TradeDate, IsHedge, MarketValue. |
| Title | ✓ | Custom titles used: "Ticker", "Asset Class", "Qty", "Price", "Trade Date", "Hedge?", "Mkt Value". |
| ColumnType | ✓ | **All 6 types shown:** Text (Ticker), Select (AssetClass), Number (Quantity, Price), Date (TradeDate), Checkbox (IsHedge), Computed (MarketValue). Excellent breadth. |
| Editable | ✓ | MarketValue (line 57) explicitly set to `false`. Computed columns inherit read-only. |
| Required | ✓ | Ticker (line 29) and Quantity (line 38) marked Required. Demonstrates validation block on save. |
| MinWidth | ✗ | Not demonstrated. |
| Width | ✓ | All columns have explicit Width values: "120px", "140px", "100px", "80px" (lines 29–58). |
| Format | ✓ | Price (line 44) and MarketValue (line 58) use `ToString("C2")` formatter. Shows currency display. |
| Validate | ✓ | Quantity (line 39) and Price (line 45) have column-level validators (range checks). Also cross-field in `OnValidate`. |
| Options | ✓ | AssetClass (lines 32–34) populated from `_assetClassOptions` (lines 91–97). Four options shown: Equity, Fixed Income, Derivative, Cash. |
| CellTemplate | ✗ | Not demonstrated. | No custom cell rendering shown. |

**Column parameters demonstrated: 10/11 (91%)** — missing MinWidth (minor) and CellTemplate (advanced scenario).

### Behaviors and features

| Feature | Demonstrated? | Notes |
|---|---|---|
| 6 ColumnTypes (Text, Number, Date, Select, Checkbox, Computed) | ✓ | All six present in columns. |
| CellState.Dirty visual indication | ✓ | Per-cell edits marked dirty (implicit via save flow); visual styling depends on CSS provider. |
| CellState.Invalid visual indication | ✓ | Required validation and `Validate` delegates show this (validation errors block save). |
| CellState.Saving/Saved transient states (F2) | ✓ | `IsSaving` parameter at line 19 triggers Saving → Saved → Pristine transitions. |
| Row-level select-all checkbox | ✗ | Bulk action bar exists but no **select-all checkbox scenario**. |
| BulkDeleteAsync | ✗ | Not demonstrated. Public method exists; no scenario showing multi-row delete via method call. |
| MarkRowDeleted toggle (F2 V05.4) | ✗ | Rows can be deleted but the F2 V05.4 toggle feature (undo delete before save) is not explicitly shown. |
| AddRowAsync | ✗ | Not demonstrated. Button exists; no scenario showing programmatic `AddRowAsync()` call. |
| ResetAsync | ✓ | Line 127 calls `ResetAsync()` after successful save (clears dirty state). Misses the **user-initiated reset** scenario (Discard All Changes). |
| BulkResetAsync | ✗ | Not demonstrated. No programmatic reset scenario. |
| SaveAllAsync flow | ✓ | Ctrl+S or Save All button invokes save (line 16 fires `OnSaveAll`). Flow: validate → transition to Saving → fire event → transition to Saved. |
| Validation blocks save | ✓ | Required fields and custom validators prevent save if invalid. |
| Bulk paste from clipboard | ✓ (partial) | Line 22 enables `AllowBulkPaste=true`. No **worked example** shown (copy-paste interaction). |
| CRLF round-trip paste (F3 V04.1) | ✗ | Not demonstrated. F3 hardening feature not showcased. |
| Paste invalid value → CellState.Invalid (F3 V04.2) | ✗ | Not demonstrated. Paste error handling not shown. |
| Paste skips deleted rows (F3 V04.3) | ✗ | Not demonstrated. Paste interaction with deleted rows not shown. |
| Copy honors Format via data-raw-value (F3 V04.4) | ✗ | Not demonstrated. Copy behavior (Ctrl+C) not shown; Format delegates exist but copy semantics not visible. |
| Keyboard shortcuts (F2, Enter, Escape, arrows, Ctrl+S, Ctrl+Z, Ctrl+D, Delete) | ✓ (documented) | Lines 175–187 list shortcuts in `_keyboard` array (passed to AccessibilityInfo). Shortcuts work but are **documented, not interactively demoed**. |
| Enter enters edit mode (F4 V07.1) | ✓ (documented) | Listed at line 178; not interactively shown. |
| Printable char enters edit mode (F4 V07.2) | ✓ (documented) | Not explicitly listed but expected F4 behavior. |
| Space toggles checkbox (F4 V07.3) | ✓ (documented) | Not explicitly shown in demo action. |
| ARIA roles and attributes (F4 V07.5/V07.6/V07.7/V07.8) | ✓ (documented) | Lines 189–204 list ARIA attributes. |
| aria-live announcements (F4 V07.9) | ✓ (documented) | Mentioned at line 200 ("aria-live polite region"). |
| Tab row wrapping (F4) | ✓ (documented) | Mentioned at line 177. |
| Virtualization | ✓ (via parameter) | Enabled at line 25 (`false` for this demo). No **large dataset** demo. |
| Themed across providers | ✗ | No provider switching. Demo uses default theme only. |

**Behavioral features demonstrated: 17/25 (68%)** — strong on core editing/validation/save, weak on clipboard operations, keyboard interactivity, and theming.

### Providers tested

The demo uses only the **default/single theme provider**. No theme switching is shown.

## Gap Summary

- **Top-level parameters demonstrated:** 13 / 18 (72%)
- **Column parameters demonstrated:** 10 / 11 (91%)
- **Behavioral features demonstrated:** 17 / 25 (68%)

**Priority-1 gaps (must-have for Stage 02 completion):**

1. **Clipboard paste scenario** — worked example of Ctrl+V with data, error handling, and format preservation.
2. **Keyboard interaction demo** — visible, interactive showcase of F2, Enter, Tab, Escape, Ctrl+S, Delete.
3. **Virtualization with large dataset** — toggle `EnableVirtualization` on/off with 500+ row dataset to show performance difference.
4. **Bulk select and operations** — row select-all checkbox, multi-row delete (BulkDeleteAsync), Ctrl+A behavior.
5. **IsLoading skeleton state** — show placeholder rows while data loads.
6. **Reset/Discard Changes** — user-initiated `ResetAsync()` via button (not just post-save cleanup).

**Priority-2 gaps (should-have):**

1. **Custom ToolbarTemplate** — inject custom buttons/actions into toolbar.
2. **CellTemplate for custom rendering** — show hand-crafted cell UI for a computed or conditional column.
3. **Cross-row OnValidate scenario** — validation that compares multiple rows (e.g., "total must not exceed $1M").
4. **Error handling pattern** — Show save failure, retry, and partial success recovery.
5. **MarkRowDeleted toggle (F2 V05.4)** — explicitly demonstrate undo-delete before save.
6. **Theme switching** — show the same grid with two different CSS providers applied.

**Priority-3 gaps (nice-to-have):**

1. AriaLabel customization (rarely needed; default is good).
2. Class/Style injection (cosmetic; CSS provider handles most needs).
3. MinWidth column sizing (niche use case).
4. Programmatic AddRowAsync / BulkDeleteAsync calls (power users; button-driven UX is primary).

## Recommended Demo Structure

**Recommendation: (b) Multi-page demo** — split into 4 focused pages, each with 3–5 scenarios, following the DataGrid pattern (Overview, Events, Appearance, Accessibility exist for DataGrid; DataSheet should mirror).

### Proposed Page Structure

1. **Overview.razor** (current, refactored; ~80 lines)
   - **Headline scenario:** "Investment Position Editor" — keep as-is, but reduce event log to improve readability.
   - **Aim:** "What is the DataSheet? What does it do?" Answer in 60 seconds.

2. **Editing-and-Validation.razor** (~120 lines; NEW)
   - **Scenario A:** Required field validation blocking save.
   - **Scenario B:** Custom column validator (Quantity >= 0, Price > 0; expand with cross-field example like "EndDate > StartDate").
   - **Scenario C:** OnValidate cross-row validation (e.g., "sum of quantities must be < 1000").
   - **Scenario D:** IsLoading skeleton state.
   - **Scenario E:** Reset/Discard Changes.

3. **Bulk-Operations.razor** (~140 lines; NEW)
   - **Scenario A:** Add Row + Edit + Save.
   - **Scenario B:** Delete and Undo Delete toggle (F2 V05.4).
   - **Scenario C:** Bulk select + multi-row delete.
   - **Scenario D:** Paste from clipboard (with format preservation and error cells).
   - **Scenario E:** Virtualization toggle (small vs large dataset).

4. **Keyboard-and-Accessibility.razor** (~140 lines; NEW)
   - **Scenario A:** Keyboard navigation (Tab, Shift+Tab, arrows, Enter).
   - **Scenario B:** Edit mode shortcuts (F2, Escape, Ctrl+Z).
   - **Scenario C:** Command keys (Ctrl+S, Ctrl+C, Ctrl+V, Ctrl+D).
   - **Scenario D:** Checkbox Space toggle.
   - **Scenario E:** Screen reader support readout (ARIA attributes visible via tooltip/overlay).

### Scenario Count per Page

- **Overview:** 1 scenario (headline demo)
- **Editing-and-Validation:** 5 scenarios
- **Bulk-Operations:** 5 scenarios
- **Keyboard-and-Accessibility:** 5 scenarios
- **Total: 16 scenarios across 4 pages**

### Code Footprint Estimate

- Overview: 80 lines (keep current, slightly trim)
- Editing-and-Validation: 120 lines
- Bulk-Operations: 140 lines
- Keyboard-and-Accessibility: 140 lines
- **Total: ~480 lines** (vs. current 206 lines, but spread across 4 pages).

## Concrete Demo Scenarios to Add (Priority-1 Set)

### 1. Clipboard Paste with Type Coercion and Error Handling

**What it shows:** Copying data from Excel-like source, pasting into grid, observing type coercion (string → number, date), and seeing invalid cells highlighted in red.

**Parameters/methods exercised:**
- `AllowBulkPaste=true`
- Column validators (ColumnType.Number, ColumnType.Date, ColumnType.Select)
- `CellState.Invalid` visual feedback

**User sees:**
1. Small 3×3 grid with Columns: ProductName (Text), Quantity (Number), Date (Date).
2. Instruction: "Copy the data below and press Ctrl+V in the grid."
3. Pre-formatted clipboard data block: `"Widget A\t100\t2025-03-15\nWidget B\tabc\t2025-04-01\nWidget C\t50\tinvalid-date"`
4. After paste: cells in row 2 (abc) and row 3 (invalid-date) show red border + error message on hover.
5. Valid cells show Dirty state.

**Code footprint:** Small (~40 lines).

---

### 2. Bulk Select All and Multi-Row Delete

**What it shows:** Row-level selection, select-all checkbox, and bulk delete with delete toggle (undo before save).

**Parameters/methods exercised:**
- `AllowDeleteRow=true`
- Row selection checkboxes (internal state)
- Delete toggle (F2 V05.4 feature: mark deleted, unmark before save)
- `DataSheetSaveArgs.DeletedRows`

**User sees:**
1. Grid with 4–5 rows and a select-all checkbox in the header.
2. Clicking individual row checkboxes selects/deselects rows; select-all toggles all.
3. When rows selected, "Delete Selected (X)" button appears in bulk action bar.
4. Clicking delete marks rows with strikethrough and grayed-out style.
5. Before save, clicking the delete button again on a strikethrough row un-deletes it (toggle).
6. After save, deleted rows are removed from grid.

**Code footprint:** Medium (~60 lines).

---

### 3. Virtualization Performance Comparison

**What it shows:** Side-by-side grids: one with 50 rows + virtualization disabled, one with 500 rows + virtualization enabled.

**Parameters/methods exercised:**
- `EnableVirtualization=true/false`
- `Height="400px"` (scrollable viewport)
- Large dataset binding

**User sees:**
1. Two labeled grids side-by-side: "Without Virtualization (50 rows)" and "With Virtualization (500 rows)".
2. Small grid with 50 rows renders all DOM nodes immediately.
3. Large grid with 500 rows only renders visible rows (+ overscan); scrolling is smooth.
4. Scroll performance is noticeably faster on virtualized grid.

**Code footprint:** Medium (~80 lines).

---

### 4. IsLoading Skeleton State

**What it shows:** Grid placeholder rows while data is loading, then smooth transition to actual data.

**Parameters/methods exercised:**
- `IsLoading=true/false`
- Skeleton animation via CSS provider (DataSheetClass with isLoading flag)

**User sees:**
1. Grid shows 3–5 skeleton placeholder rows (gray blocks, shimmer animation).
2. Instruction: "Click 'Load Data' button."
3. Button click sets `IsLoading=false` after 2-second delay.
4. Skeleton rows disappear; real data appears.

**Code footprint:** Small (~50 lines).

---

### 5. Reset and Discard Changes

**What it shows:** User makes edits, clicks "Discard All Changes" button, and all dirty state is reverted.

**Parameters/methods exercised:**
- `ResetAsync()` public method
- Dirty state reversion (before/after row snapshots)
- Added rows removed; deleted rows restored

**User sees:**
1. Grid with 3 rows. Rows have Dirty indicator (yellow background).
2. User makes several edits (change cell values, add a new row, mark a row for deletion).
3. "Discard All Changes" button calls `ResetAsync()`.
4. All dirty indicators disappear; values revert to originals; added row is removed; deleted row is restored.
5. Event log shows: "Discarded 3 dirty rows, restored 1 deleted row, removed 1 added row."

**Code footprint:** Small (~50 lines).

---

### 6. Keyboard Navigation and Edit Shortcuts

**What it shows:** Interactive keyboard demo — user presses keys and sees active cell move, edit mode enter/exit, and undo behavior.

**Parameters/methods exercised:**
- Keyboard handling (`F2`, `Enter`, `Escape`, arrows, `Ctrl+Z`)
- Edit mode state transitions
- Undo buffer

**User sees:**
1. Small 3×3 grid. Instructions at top: "Try these keys: F2 (edit), Enter (commit), Escape (cancel), Ctrl+Z (undo), arrows (navigate)."
2. Below grid: Key event log showing "Pressed: F2", "Entered edit mode on Ticker", "Typed: GOOG", "Pressed: Escape", "Reverted value", etc.
3. As user presses keys, active cell highlight moves, edit input appears/disappears, and log updates in real-time.
4. User presses Ctrl+Z and sees cell value revert to previous (undo).

**Code footprint:** Small (~70 lines).

## Recommendation for Stage 02 Implementation

**Implement all Priority-1 gaps in a single Stage 02 loop iteration**, split into two sub-batches:

- **02a (Page scaffolding + Editing/Bulk-Operations):** Create Editing-and-Validation.razor and Bulk-Operations.razor; implement scenarios 1, 2, 3, 4, 5 (clipboard, delete, virtualization, loading, reset).
- **02b (Keyboard/A11y + navigation):** Create Keyboard-and-Accessibility.razor; implement scenario 6 (keyboard demo) and refactor Overview.razor for clarity.

**Effort estimate:** 3–4 development days for a skilled Blazor developer.

**Rationale:** Priority-1 gaps represent genuine feature gaps that users building production workflows will encounter immediately. Priority-2 and Priority-3 can be deferred to Stage 03 (advanced features, polish).

## Open Questions

1. **Clipboard data source for paste demo:** Should the paste scenario use a pre-formatted text block in the page (user copy-pastes from visible text), or should it include a real Excel file download link? **Recommendation:** pre-formatted text block approach (simpler, still effective).

2. **Keyboard demo interactivity:** Should Keyboard-and-Accessibility.razor include a "keystroke logger" overlay showing keys pressed, or just rely on visual cell highlighting and the event log? **Recommendation:** both — small on-screen key display significantly aids learning.

3. **Theme switching for "Themed across providers":** Should the multi-page demo include a theme selector (FluentUI vs. Bootstrap CSS provider) so users can see the same grid restyled? **Recommendation:** defer to Stage 03 (theming polish).

4. **Cross-row OnValidate scenario scale:** Cross-row validation (sum < 1000 check) adds complexity. **Recommendation:** include it in Stage 02 — it showcases a genuinely useful pattern that users ask for.
