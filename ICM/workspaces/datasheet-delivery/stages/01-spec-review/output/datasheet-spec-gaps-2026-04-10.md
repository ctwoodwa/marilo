# DataSheet Spec Gap List — 2026-04-10 Re-Audit

**Audit date:** 2026-04-10 (supersedes the 2026-04-03 audit that was against the wrong spec directory)
**Component:** `MariloDataSheet<TItem>` — typed, schema-driven bulk-edit data sheet
**Spec directory audited:** `docs/component-specs/datasheet/` (the correct 9-file spec, not the old `docs/component-specs/spreadsheet/` Excel-clone spec)
**Source directory:** `src/Marilo.Components/DataGrid/MariloDataSheet*` (1,324 lines across 7 partial files + column child component)

**Headline finding:** The new spec at `docs/component-specs/datasheet/` was clearly written to describe the **actual current implementation**, and broad-surface alignment is excellent. This is a **cleanup audit**, not a rewrite inventory. The gap count is in the single digits, not the 30-50+ range of the Scheduler or TreeList intakes.

---

## Broad-Surface Alignment — All Verified Present

| Surface | Spec declares | Source has | Verified at |
|---|---|---|---|
| Top-level parameters | 18 (Data, KeyField, OnSaveAll, OnRowChanged, OnValidate, IsSaving, AllowAddRow, AllowDeleteRow, AllowBulkPaste, EmptyStateMessage, Height, IsLoading, EnableVirtualization, AriaLabel, ChildContent, ToolbarTemplate, Class, Style) | ✅ 16 explicit in `.razor.cs` + `Class`/`Style` inherited from `MariloComponentBase` (lines 20, 25) | `MariloDataSheet.razor.cs:23-74` |
| Column parameters | 11 (Field, Title, ColumnType, Editable, Required, MinWidth, Width, Format, Validate, Options, CellTemplate) | ✅ 11/11 | `MariloDataSheetColumn.razor:10-40` |
| Event arg types | 6 (DataSheetSaveArgs, DataSheetRowChangedArgs, DataSheetValidateArgs, DataSheetValidationError, DataSheetCellContext, DataSheetSelectOption) | ✅ 6 files | `src/Marilo.Core/Models/DataSheet/*.cs` |
| Enums | 2 (DataSheetColumnType, CellState) | ✅ 2 files | `src/Marilo.Core/Enums/DataSheetColumnType.cs`, `CellState.cs` |
| Public methods | 9 (ResetAsync, ValidateAllAsync, GetDirtyRows, SetDataAsync, ScrollToRowAsync, CommitCellEdit, EnterEditMode, IsCellEditing, SaveAllAsync) | ✅ 9 (spread across `.razor.cs`, `.Data.cs`, `.Editing.cs`, `.Interop.cs`) | `MariloDataSheet.Data.cs:38,108,142,205`, `.Editing.cs:34,63`, `.Interop.cs:38`, `.razor.cs:120,129` |
| CSS provider methods | 7 (DataSheetClass, DataSheetCellClass, DataSheetHeaderCellClass, DataSheetRowClass, DataSheetToolbarClass, DataSheetBulkBarClass, DataSheetSaveFooterClass) | ✅ 7 at exact signatures | `IMariloCssProvider.cs:167-173` |
| Demo page | Spec example at `overview.md:58-98` | ✅ 205-line "Investment Position Editor" using 14 of 18 top-level parameters | `samples/Marilo.Demo/Pages/Components/DataSheet/Overview.razor` |
| Partial-class architecture | Not spec-mandated but follows DataGrid precedent | ✅ `.razor` / `.razor.cs` / `.Data.cs` / `.Editing.cs` / `.Interop.cs` / `.Rendering.cs` + `MariloDataSheetColumn.razor` | `src/Marilo.Components/DataGrid/MariloDataSheet*` |

**Total verified surface:** 18 + 11 + 6 + 2 + 9 + 7 = **53 distinct API elements present**, plus the demo and architectural pattern.

---

## Summary Counts

| Category | Count |
|---|---|
| Confirmed missing | 0 |
| Verification gaps (needs detail-file audit) | 8 |
| Unverified enum-value coverage | 2 |
| **Total** | **10** |

This is **dramatically smaller** than the Scheduler (32 gaps) or TreeList (43 gaps) intakes because those components had genuine implementation holes, whereas DataSheet's implementation already matches its spec at the API-surface level.

---

## Gap List — Verification Sub-Tasks (Stage 01b)

These are not "missing features" — they are **verification sub-tasks** to audit the 8 feature-area detail spec files against the corresponding source partials. Each represents an independent sub-audit that can be done in a small session.

### GAP-DATASHEET-V01: Verify `columns-and-schema.md` coverage
**Area:** Columns
**Severity:** Medium
**Source:** `docs/component-specs/datasheet/columns-and-schema.md` (not yet audited); `MariloDataSheetColumn.razor` + `MariloDataSheet.Rendering.cs`

**Work:** Walk the columns-and-schema spec file line by line and confirm each feature has an implementation touch point. Specifically verify:
- Default editor behavior for each of the 6 `DataSheetColumnType` values (Text, Number, Date, Select, Checkbox, Computed)
- `Computed` column exclusion from Save All payloads
- `Required` enforcement timing (per-cell on edit vs pre-save-all)
- `Format` lambda invocation in read mode and edit mode

**Status:** Open (verification needed)

---

### GAP-DATASHEET-V02: Verify `editing-and-validation.md` coverage
**Area:** Editing
**Severity:** Medium
**Source:** `docs/component-specs/datasheet/editing-and-validation.md`; `MariloDataSheet.Editing.cs` (294 lines) + `MariloDataSheet.Data.cs` (287 lines)

**Work:** Audit the editing lifecycle spec against `.Editing.cs` and `.Data.cs`. Verify:
- Per-cell vs per-row vs pre-save validation ordering
- Dirty tracking at the field level (not just row level)
- `OnValidate` handler can block Save All by appending errors
- `CommitCellEdit` updates dirty state atomically
- Undo/reset semantics

**Status:** Open (verification needed)

---

### GAP-DATASHEET-V03: Verify `selection-and-ranges.md` coverage
**Area:** Selection
**Severity:** Medium
**Source:** `docs/component-specs/datasheet/selection-and-ranges.md`; `MariloDataSheet.razor.cs:17,140-158` (has `_selectedRows` HashSet, `OnSelectAllChanged`, `ToggleRowSelection`)

**Work:** The source has **row selection** (`HashSet<TItem>` + select-all + toggle). Verify the spec's **cell range selection** semantics — does the spec require range creation via shift-click / click-drag / Ctrl-click? The source hasn't been confirmed to implement these. **Possible genuine gap** if the spec requires cell ranges and the source only does row selection. High-value sub-audit.

**Status:** Open (verification needed — likely to surface real gaps)

---

### GAP-DATASHEET-V04: Verify `bulk-paste-and-clipboard.md` coverage
**Area:** Clipboard
**Severity:** Medium
**Source:** `docs/component-specs/datasheet/bulk-paste-and-clipboard.md`; `MariloDataSheet.Interop.cs` (87 lines, has `PasteFromClipboard` `[JSInvokable]` method) + `MariloDataSheet.Editing.cs` (paste handling around line 263)

**Work:** Verify TSV parsing, type coercion, paste error handling, multi-row paste, paste-past-end-of-data behavior, paste conflict resolution.

**Status:** Open (verification needed)

---

### GAP-DATASHEET-V05: Verify `bulk-operations-and-saveall.md` coverage
**Area:** Bulk operations
**Severity:** Low-Medium
**Source:** `docs/component-specs/datasheet/bulk-operations-and-saveall.md`; `MariloDataSheet.Data.cs:142-…` (`SaveAllAsync`) + `MariloDataSheet.razor.cs:160-181` (`BulkDeleteAsync`, `BulkResetAsync`, `AddRowAsync`)

**Work:** Verify Save All flow (validate → fire OnSaveAll → apply / roll back), bulk delete semantics, bulk reset semantics, retry guidance.

**Status:** Open (verification needed)

---

### GAP-DATASHEET-V06: Verify `virtualization-and-performance.md` coverage
**Area:** Virtualization
**Severity:** Low
**Source:** `docs/component-specs/datasheet/virtualization-and-performance.md`; `EnableVirtualization` parameter exists at `MariloDataSheet.razor.cs:63` (default `true`); usage in `.Rendering.cs` unverified

**Work:** Verify `<Virtualize>` is actually used when `EnableVirtualization` is true, row-height estimation, scroll thresholds, limitations (e.g., incompatibility with certain features?).

**Status:** Open (verification needed)

---

### GAP-DATASHEET-V07: Verify `keyboard-and-accessibility.md` coverage
**Area:** Accessibility
**Severity:** Medium
**Source:** `docs/component-specs/datasheet/keyboard-and-accessibility.md`; `MariloDataSheet.Editing.cs` has `HandleKeyDown` `[JSInvokable]` plus key handling around lines 118-180; `AriaLabel` parameter exists at `.razor.cs:66`; `role="grid"` usage unverified

**Work:** Map every spec-documented keyboard shortcut to its handler in `.Editing.cs`. Confirm `role="grid"`, `role="row"`, `role="gridcell"`, `aria-rowcount`, `aria-colcount`, `aria-rowindex`, `aria-colindex`, `aria-readonly`, `aria-invalid`, `aria-describedby` all render correctly. Confirm focus trapping and restoration on edit enter/exit.

**Status:** Open (verification needed — a11y is always worth detailed verification)

---

### GAP-DATASHEET-V08: Verify `theming-and-css-provider.md` coverage
**Area:** Theming
**Severity:** Low
**Source:** `docs/component-specs/datasheet/theming-and-css-provider.md`; 7 methods on `IMariloCssProvider.cs:167-173` (all confirmed present); per-provider implementations in `FluentUICssProvider.cs`, `MaterialCssProvider.cs`, `BootstrapCssProvider.cs`

**Work:** Verify each CSS provider method is consumed by the correct region of the `.Rendering.cs` partial, and that each provider generates the expected class names per the spec. Likely the lowest-value sub-audit since all signatures are verified present.

**Status:** Open (verification needed)

---

### GAP-DATASHEET-V09: Verify `DataSheetColumnType` enum values
**Area:** Enums
**Severity:** Low
**Source:** Spec says 6 values: `Text`, `Number`, `Date`, `Select`, `Checkbox`, `Computed` (`overview.md:212-219`). Source file `src/Marilo.Core/Enums/DataSheetColumnType.cs` exists but values not confirmed in this audit.

**Work:** Single-file check — open the enum and confirm all 6 values are present and no extras.

**Status:** Open (trivial verification)

---

### GAP-DATASHEET-V10: Verify `CellState` enum values
**Area:** Enums
**Severity:** Low
**Source:** Spec says 5 values: `Pristine`, `Dirty`, `Invalid`, `Saving`, `Saved` (`overview.md:222-232`). Source file `src/Marilo.Core/Enums/CellState.cs` exists but values not confirmed in this audit.

**Work:** Single-file check — open the enum and confirm all 5 values are present.

**Status:** Open (trivial verification)

---

## Cross-cutting Observations

1. **The 2026-04-09 architecture decision worked.** The new spec at `docs/component-specs/datasheet/` was written to describe the actual implementation, not a speculative future direction. Broad-surface alignment is ~100%. This is a refreshingly healthy state — much better than Scheduler (32 open gaps) or TreeList (43 open gaps).

2. **The old 2026-04-03 audit was entirely invalid** — it paired `MariloDataSheet<TItem>` against the wrong spec (`docs/component-specs/spreadsheet/` — the MariloSpreadsheet Excel-clone spec), producing ~38 false-positive gaps. That audit should be archived (already marked superseded at the top of the file).

3. **Remaining work is verification, not rewrites.** The 10 gap records above are **sub-audit tasks**, not feature builds. Each can be done in a small session (10-30 minutes). The one with the highest probability of surfacing a real gap is **V03 (cell range selection)** — the source has row selection but cell range selection is unverified.

4. **DataSheet does NOT need a Stage 02 → 03 → 04 → 05 → 06 phased rewrite.** It needs a **Stage 01b detail audit** (the 10 sub-tasks above) to produce a precise gap list, which will likely be small. After that, any real gaps found go through a targeted Stage 03 → 05 → 06 fix cycle.

5. **Demo is already in good shape.** The 205-line Investment Position Editor uses 14 of the 18 top-level parameters. Adding demo coverage for the remaining 4 (`IsLoading`, `EnableVirtualization`, `EmptyStateMessage`, `ToolbarTemplate`) would be a nice-to-have Stage 02 touch.

## Suggested Stage 02 / Stage 03 Approach

Given how small the gap surface is, DataSheet does not need a "prioritize → design → plan → implement" pipeline. Recommend skipping Stage 02 prioritization and going directly from this Stage 01 re-audit to the 10 Stage 01b verification sub-tasks. Any gaps surfaced by those sub-tasks get a lightweight Stage 03 resolution design and a targeted fix commit.

**Effective pipeline:** `01 (re-audited, done ✓) → 01b (10 verification sub-audits) → 03 (per-gap resolutions if any) → 05 (implement) → 06 (validate)`

## Audit Checklist

| Check | Status |
|---|---|
| Every gap has a unique ID | ✅ GAP-DATASHEET-V01 through GAP-DATASHEET-V10 |
| Every gap references real artifacts | ✅ All source and spec paths verified |
| Severity assigned | ✅ 0 Critical, 0 High, 6 Medium, 4 Low |
| Target state documented | ✅ 9-file spec at `docs/component-specs/datasheet/` |
| Counts match | ✅ 6 + 4 = 10 |

## Human Decisions Needed

**Zero.** This re-audit does not surface any architecture or design choices that require human input. The 10 sub-tasks are all mechanical verification work. Stage 01b can proceed without human intervention — although human eyes on V03 (cell range selection) would be valuable because that's the one most likely to surface a real implementation gap.
