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

### GAP-DATASHEET-V01: ~~Verify `columns-and-schema.md` coverage~~ → **COMPLETE 2026-04-10** — 1 confirmed-pending gap + 8 re-verify
**Area:** Columns
**Severity:** ~~Medium~~ → **High** (V01-01 is a likely Tab-navigation bug)
**Findings:** [datasheet-v01-columns-findings.md](./datasheet-v01-columns-findings.md)

**Result:** Column-type handling is **substantially present and well-architected**. All 6 `DataSheetColumnType` values dispatched in `.Rendering.cs:129-234` render switch; `Format` delegate invoked at both read mode and computed columns with correct `ToString()` fallback; computed read-only enforced in 4 locations; Required + Validate delegates fire correctly in `.Data.cs:112-131`; `GetDefaultValue` switch provides type-specific defaults for 5 types. **One confirmed-pending gap:** V01-01 — `.Editing.cs:190` has `_columns.Where(c => c.Editable || c.ColumnType == DataSheetColumnType.Computed).ToList()` assigned to a variable named `editableColumns`. If this filter drives Tab navigation (likely given the name), it's a spec-compliance bug because Tab should skip computed cells per spec `keyboard-and-accessibility.md:38`. Needs one-line verification of how `editableColumns` is consumed by the Tab branch at `.Editing.cs:210-236`. 8 targeted re-verification items flagged for correctness details (Required rejecting zero for non-nullable Number, whitespace for Text, default(DateTime) for Date; Select preserving unmatched values; Required error message format; culture-sensitive Number parsing cross-ref V04-R2; GetDefaultValue Computed fallback; Select Value/Label rendering).

**V01-01 integration:** Folds into **Batch A** (Range Selection + Keyboard + A11y) since it touches the same `.Editing.cs:186-236` keyboard handler region as V03-04 / V07-05 / V07-06 / V07-07.

**Status:** ✅ Verification complete; 1 confirmed-pending gap + 8 re-verification items

---

### GAP-DATASHEET-V02: ~~Verify `editing-and-validation.md` coverage~~ → **COMPLETE 2026-04-10** — 1 confirmed gap + 7 re-verification items
**Area:** Editing & Validation
**Severity:** ~~Medium~~ → **High** (V02-01 is a correctness bug with user-visible consequence)
**Findings:** [datasheet-v02-editing-validation-findings.md](./datasheet-v02-editing-validation-findings.md)

**Result:** The dirty tracking architecture is **substantially present and well-architected** (`_dirtyRows` Dictionary with `DirtyRowEntry<TItem>` containing DirtyFields/ValidationErrors/IsDeleted, `OverallState` computed property, `ValidateAllAsync` + `SaveAllAsync` flow with OnValidate integration, deleted-row separation, all public query helpers present). **One clear structural gap:** V02-01 — `.razor.cs:107-115` OnParametersSet uses `Data.ToList()` (shallow copy), so there's **no deep-cloned original snapshot** for dirty comparison. This breaks the spec's "edit back to original value → field removed from dirty set" contract (correctness bug with visible user consequence: reverting a cell value doesn't clean it from the save payload). Seven additional items (V02-R2 through V02-R8) flagged for targeted re-verification — each is a small single-method sub-audit, not an independent investigation.

**New Stage 03 batch:** **Batch B — Dirty Tracking Correctness** (V02-01, standalone, 1-2 commits, 5-8 tests, independent of V03+V07's Batch A).

**Status:** ✅ Verification complete; 1 confirmed gap opened + 7 re-verification items + 1 cross-ref to V07-05

---

### GAP-DATASHEET-V03: ~~Verify `selection-and-ranges.md` coverage~~ → **COMPLETE 2026-04-10** — surfaced 8 real gaps
**Area:** Selection
**Severity:** ~~Medium~~ → **High** (4 High + 4 Medium sub-gaps confirmed)
**Findings:** [datasheet-v03-range-selection-findings.md](./datasheet-v03-range-selection-findings.md)

**Result:** As predicted, V03 surfaced the highest concentration of real implementation gaps of all the Stage 01b sub-tasks. Source has active-cell tracking and row selection (both working) but **zero cell-range selection**, and the existing `Ctrl+D` Fill Down command uses the wrong scope (row selection instead of cell range — a latent bug). 8 confirmed gaps (V03-01 through V03-08) promoted from verification sub-tasks to Stage 03 implementation work. Recommended as a single "DataSheet cell range selection" batch in 3 sub-phases (Foundation → Input → Operations). Zero human decisions needed — all 8 have clear recommended directions grounded in spec text.

**Status:** ✅ Verification complete; 8 implementation gaps opened, ready for Stage 03 resolution design

---

### GAP-DATASHEET-V04: ~~Verify `bulk-paste-and-clipboard.md` coverage~~ → **COMPLETE 2026-04-10** — 1 confirmed gap + 6 re-verify items
**Area:** Clipboard
**Severity:** Medium
**Findings:** [datasheet-v04-clipboard-findings.md](./datasheet-v04-clipboard-findings.md)

**Result:** JS interop infrastructure is substantially present (`IJSObjectReference` module load from `marilo-datasheet.js`, keydown handler registration, `IAsyncDisposable` cleanup, `ScrollToRowAsync`/`CopyToClipboardAsync`/`FocusCellAsync` bridge methods all present at expected signatures). **One confirmed gap:** V04-01 — `CopyToClipboardAsync` is **orphaned dead code** with no call sites anywhere in the component, because V03-06 confirmed no Ctrl+C keyboard handler AND no "build TSV from range" method exists (blocked by V03-01 range state absence). The copy path is entirely unreachable. **6 re-verification items** flagged for a focused read of the paste method body (`.Editing.cs:246-~320`): TSV parsing with `\r\n` handling, per-`ColumnType` type coercion with culture sensitivity (highest-risk), computed/non-editable column skipping, deleted-row skipping, best-effort error handling ("do not write raw string on coercion failure" — highest-risk), and `IsSaving` gate. All are method-body correctness concerns, not architectural.

**V04-01 integration:** Resolves automatically as a side effect of **Batch A — Range Selection + Keyboard + A11y** (V03+V07 consolidated). No separate batch needed.

**Status:** ✅ Verification complete; 1 confirmed gap (folds into Batch A) + 6 targeted re-verification items for future fire

---

### GAP-DATASHEET-V05: ~~Verify `bulk-operations-and-saveall.md` coverage~~ → **COMPLETE 2026-04-10** — 8 confirmed gaps + invalidates V02-01 + downgrades V07-08
**Area:** Save All Lifecycle
**Severity:** ~~Low-Medium~~ → **High** (includes a user-visible state bug + a data-loss risk)
**Findings:** [datasheet-v05-saveall-findings.md](./datasheet-v05-saveall-findings.md)

**Result:** Save All flow structure is correct (ValidateAllAsync → OnValidate → OnSaveAll), deleted rows correctly separated from DirtyRows, OnValidate errors mapped back to cells — but **8 confirmed gaps in post-save lifecycle**: CellState.Saving never set (V05-01), no post-save state transitions (V05-02), **deleted rows not removed from `_displayRows` after save (V05-03 — user-visible bug)**, original snapshots not refreshed after save (V05-04), AddRowAsync inserts at top instead of end (V05-05), **new row not marked dirty so silently doesn't save (V05-06 — data loss risk)**, AddRowAsync doesn't move active cell (V05-07), ResetAsync doesn't remove added rows (V05-08).

**Major corrections to prior findings:**
- **V02-01 INVALIDATED (false positive).** The deep-clone snapshot DOES exist — it happens lazily in `CommitCellEdit` at `.Data.cs:49` (`Original = GridReflectionHelper.DeepClone(row)`), not in `OnParametersSet` as my V02 audit expected. The dirty-detection logic at lines 64-72 correctly compares against `entry.Original`. The "edit back to original → remove from dirty set" contract IS implemented. V02 fire #13 grep missed this because my regex didn't include `DeepClone` or `Original = ` patterns.
- **V07-08 DOWNGRADED.** The `_ariaAnnouncement` field IS wired at `.Data.cs:208/228/252` for "save blocked", "saved successfully", and "reset" messages — 2 of 5 spec-required announcements are present. Still missing: dirty count change, saving start, validation errors appear. Downgraded from "structural wiring absent" to "partially wired (2/5)".

**New Stage 03 batch:** **Batch C — Save All Lifecycle Correctness** (V05-01 through V05-08 = 8 gaps, ~15-20 tests, 4 phases). Batch B (Dirty Tracking Correctness) is **DROPPED** since V02-01 is invalidated.

**Status:** ✅ Verification complete; 8 confirmed gaps + 2 re-verify items; 1 spec-match closed (V05-R3 mutual exclusion of deleted+dirty); 2 prior findings corrected

---

### GAP-DATASHEET-V06: Verify `virtualization-and-performance.md` coverage
**Area:** Virtualization
**Severity:** Low
**Source:** `docs/component-specs/datasheet/virtualization-and-performance.md`; `EnableVirtualization` parameter exists at `MariloDataSheet.razor.cs:63` (default `true`); usage in `.Rendering.cs` unverified

**Work:** Verify `<Virtualize>` is actually used when `EnableVirtualization` is true, row-height estimation, scroll thresholds, limitations (e.g., incompatibility with certain features?).

**Status:** Open (verification needed)

---

### GAP-DATASHEET-V07: ~~Verify `keyboard-and-accessibility.md` coverage~~ → **COMPLETE 2026-04-10** — surfaced 6 new gaps (+2 cross-refs)
**Area:** Accessibility + Keyboard
**Severity:** ~~Medium~~ → **High** (3 High + 4 Medium + 1 Low sub-gaps confirmed)
**Findings:** [datasheet-v07-keyboard-a11y-findings.md](./datasheet-v07-keyboard-a11y-findings.md)

**Result:** Root-level ARIA coverage is good (role="grid", aria-label, aria-rowcount, aria-colcount, toolbar landmarks, aria-live regions all present), but **data rows and cells lack `role="row"` and `role="gridcell"`** — a WCAG 2.1 grid-pattern conformance failure. Also found: `aria-busy` only reacts to `IsLoading` not `IsSaving` (one-line fix), missing Ctrl+S shortcut, missing "printable character → edit mode" handler, missing Space-toggles-checkbox, unverified Enter behavior, and the `_ariaAnnouncement` field exists but isn't wired to produce the spec-required screen reader announcements. 6 new sub-gaps + 2 cross-references to V03 (Ctrl+A, Ctrl+C). Consolidated batch layout proposed that combines V03 + V07 into a single "DataSheet Range Selection + Keyboard + A11y" batch across 6 phases. Zero human decisions needed.

**Status:** ✅ Verification complete; 6 new implementation gaps + 2 cross-refs to V03; combined V03+V07 batch = **14 total confirmed gaps** ready for Stage 03 resolution design

---

### GAP-DATASHEET-V08: Verify `theming-and-css-provider.md` coverage
**Area:** Theming
**Severity:** Low
**Source:** `docs/component-specs/datasheet/theming-and-css-provider.md`; 7 methods on `IMariloCssProvider.cs:167-173` (all confirmed present); per-provider implementations in `FluentUICssProvider.cs`, `MaterialCssProvider.cs`, `BootstrapCssProvider.cs`

**Work:** Verify each CSS provider method is consumed by the correct region of the `.Rendering.cs` partial, and that each provider generates the expected class names per the spec. Likely the lowest-value sub-audit since all signatures are verified present.

**Status:** Open (verification needed)

---

### GAP-DATASHEET-V09: ~~Verify `DataSheetColumnType` enum values~~ → ✅ **COMPLETE 2026-04-10 — clean pass, zero gaps**
**Area:** Enums **Severity:** Low
**Source:** `src/Marilo.Core/Enums/DataSheetColumnType.cs` (25 lines)

**Result:** All 6 spec-declared values present in exact spec order with matching XML doc comment semantics: `Text` ✓ `Number` ✓ `Date` ✓ `Select` ✓ `Checkbox` ✓ `Computed` ✓. No extras, no renames, no gaps.

**Status:** ✅ Closed — zero gaps

---

### GAP-DATASHEET-V10: ~~Verify `CellState` enum values~~ → ✅ **COMPLETE 2026-04-10 — clean pass, zero gaps**
**Area:** Enums **Severity:** Low
**Source:** `src/Marilo.Core/Enums/CellState.cs` (23 lines)

**Result:** All 5 spec-declared values present in exact spec order with matching XML doc comment semantics: `Pristine` ✓ `Dirty` ✓ `Invalid` ✓ `Saving` ✓ `Saved` ✓. No extras, no renames, no gaps.

**Status:** ✅ Closed — zero gaps

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
