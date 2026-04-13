# DataSheet Gap Analysis — Stage 01 Intake

**Date:** 2026-04-11
**Source:** `ICM/workspaces/datasheet-delivery/stages/01-spec-review/output/datasheet-spec-gap-list.md`
**Session:** `marilo-grid-pipeline-2026-04-11-1200` (gifted-swanson worktree)

This intake document covers all gaps from the DataSheet spec gap list that require
source or architectural work. Spec-only wording corrections (SA-07, SA-11, SA-12,
NM-01, SRC-01) have been applied in the same delivery pass and are noted below as
RESOLVED. Source behavioral fixes (SA-01, SA-03, SA-04, SA-08, SA-09, SA-13,
SA-14, SA-15) have also been applied and are noted as RESOLVED. Items that require
orchestrator decisions or multi-session implementation remain in intake status.

---

## Resolved this pass (spec-only wording corrections)

| Gap ID | Description | Status | Notes |
|--------|-------------|--------|-------|
| SA-07 | Date coercion: "current culture" → InvariantCulture | RESOLVED | `bulk-paste-and-clipboard.md:91` corrected |
| SA-11 | Validation short-circuit wording corrected | RESOLVED | `editing-and-validation.md:139` corrected |
| SA-12 | Dirty count "does not include invalid-only rows" corrected | RESOLVED | `editing-and-validation.md:193` corrected |
| NM-01 | `Class`/`Style` parameter table rows clarified | RESOLVED | `overview.md:122-123` updated to note base-class origin |
| SRC-01 | Skeleton row count: viewport-calculated → fixed 5 | RESOLVED | `virtualization-and-performance.md:77` corrected |

## Resolved this pass (source behavioral fixes)

| Gap ID | Description | Status | Notes |
|--------|-------------|--------|-------|
| SA-01 | `tabindex="0"` missing on root grid element | RESOLVED | Added to `MariloDataSheet.razor` |
| SA-03 | `AddRowAsync` doesn't activate first editable column | RESOLVED | `ActivateCell` call added after insert |
| SA-04 | `ResetAsync` doesn't clear `_undoBuffer` | RESOLVED | `_undoBuffer.Clear()` added in `Data.cs` |
| SA-08 | `PasteFromClipboard` doesn't check `IsSaving` | RESOLVED | Guard added at top of `PasteFromClipboard` |
| SA-09 | No `ondblclick` handler on cells | RESOLVED | `ondblclick` + `OnCellDoubleClick` added |
| SA-13 | Missing aria-live: "Saving changes", "Save failed. {N} errors", "{N} cells have errors" | RESOLVED | All three announcements added |
| SA-14 | Required check for non-nullable numeric types doesn't reject zero | RESOLVED | Zero-default check added in `RunColumnValidation` |
| SA-15 | Required check for `DateTime` doesn't reject `DateTime.MinValue` | RESOLVED | `DateTime.MinValue` check added in `RunColumnValidation` |

---

## Active intake items (require source or architectural work)

### V03 — DataSheetSelection\<TItem\> (Range Selection)

**Gap location:** `docs/component-specs/datasheet/selection-and-ranges.md:37-114`

**Description:**
Rectangular range selection (anchor/extent, Shift+Click, click-drag, Shift+Arrow),
`Ctrl+A`, range-scoped Copy/Paste/Fill Down/Delete, and `DataSheetSelection<TItem>`
model are entirely absent from source. Source only tracks `_activeCellRow` /
`_activeCellField` (single cell) and `_selectedRows` (row-level `HashSet<TItem>`
for bulk delete).

**Status:** DESIGN COMPLETE at
`ICM/workspaces/datasheet-gap-analysis/stages/03-resolution-design/output/gap-datasheet-v03-selection-ranges-resolution.md`;
ready for Phase A implementation.

**Implementation estimate:** ~3.5 developer-days. Requires its own dedicated
session. Do NOT attempt to implement in a spec-correction or small-fixes pass.

**Blocked items:**
- V07.4 (Ctrl+A select all) — folded into V03 resolution
- SA-06 (Ctrl+D range fill) — depends on V03 range selection
- VP-datasheet-D02 (visual parity for range selection)
- EU-07 (selection-and-ranges demo scenario)

---

### SRC-02 — Hard-coded BEM classes vs. CSS provider contract

**Gap location:** `docs/component-specs/datasheet/theming-and-css-provider.md:34` vs
`MariloDataSheet.razor` + `MariloDataSheet.Rendering.cs`

**Description:**
Source emits 21 hard-coded BEM element classes (`mar-datasheet__add-btn`,
`mar-datasheet__save-btn`, etc.). The spec states "The component does not add its
own hard-coded classes — all styling is delegated to the provider." The current
`IMariloCssProvider` surface exposes only 7 container-level methods and has no hooks
for the 21 element-level classes.

**Status:** BLOCKED pending orchestrator UD-01 decision.

**Decision required:**
- **Path A:** Extend `IMariloCssProvider` with per-subregion methods (button classes,
  skeleton classes, editor classes, badge classes, sr-only class). This is a
  public-API change to all three providers (FluentUI, Material, Bootstrap).
- **Path B:** Narrow `theming-and-css-provider.md` to "container-level classes are
  delegated; BEM element classes below the CSS-provider boundary are
  component-internal."

**Why not worker-fixable:** Provider-contract change (`IMariloCssProvider` surface)
is orchestrator-only per `.claude/rules/orchestration.md` "Architecture-Level
Changes" list.

---

### SA-02 — AddRowAsync prepend vs. append

**Gap location:** `docs/component-specs/datasheet/bulk-operations-and-saveall.md:117`
vs `MariloDataSheet.razor.cs:208`

**Description:**
Spec says "Appends the row to the end of the internal data list." Source calls
`_displayRows.Insert(0, newItem)` (prepend at index 0). Either the spec must say
"prepended at the top" or `AddRowAsync` must switch to `Add(newItem)`.

**Status:** BLOCKED pending orchestrator arbitration — changing from prepend to
append would affect existing consumers (the demo pages show new rows appearing at
the top, and tests may assert index 0).

**Why not worker-fixable:** API-visible behavior change that affects demo behavior
and existing tests. Must be an explicit orchestrator call.

---

### SA-05 — Saving→Saved cell-state transition

**Gap location:** `docs/component-specs/datasheet/bulk-operations-and-saveall.md:104-107`

**Description:**
The spec's cell-state transition table reads "`Saving → Saved (IsSaving set to false)`"
implying the component observes the consumer's `IsSaving` parameter to drive the
transition. Actual source drives the `Saving → Saved → Pristine` transition
internally inside `SaveAllAsync` — it does not observe `IsSaving` for this purpose.

**Status:** BLOCKED pending orchestrator arbitration — consumer-visible API behavior.
Either the spec text needs to describe the actual component-driven transition or the
source needs to be redesigned to observe `IsSaving` changes (which would require
`OnParametersSetAsync` hooks and state machine plumbing).

---

### SA-06 — Ctrl+D range fill (depends on V03)

**Gap location:** `docs/component-specs/datasheet/selection-and-ranges.md:88`

**Description:**
Ctrl+D spec says it copies the active cell value down to all cells in the same column
within the **current selection range**. Source iterates `_selectedRows` (row-level
bulk selection) and does not filter for `column.Editable == true` or non-Computed
columns. Until V03 lands, Ctrl+D is effectively a row-selection feature rather than
a range fill.

**Status:** DEPENDENT on V03 range selection implementation. Will be addressed as
part of V03 Phase A work.

---

## Summary counts

| Category | Count |
|----------|-------|
| Resolved (spec-wording) | 5 (SA-07, SA-11, SA-12, NM-01, SRC-01) |
| Resolved (source behavioral) | 8 (SA-01, SA-03, SA-04, SA-08, SA-09, SA-13, SA-14, SA-15) |
| Design complete, ready for implementation | 1 (V03) |
| Blocked on orchestrator decision | 3 (SRC-02, SA-02, SA-05) |
| Dependent on V03 | 1 (SA-06) |
| **Total intake items** | **13 discrete gaps** |

## Not included in this intake

The following gaps were closed in prior sessions and are not re-listed:
- V01.1, V01.2, V02.1, V02.2, V04.1–V04.4, V05.1–V05.5, V07.1–V07.9
  (confirmed fixed via source comment markers `V*`/`F1.*`/`F2.*`)

Gaps outside source/architectural scope (purely carried spec items):
- V07.4 (Ctrl+A) — folded into V03
- V02.3 (per-row validation layer) — pending spec re-read, not actioned
