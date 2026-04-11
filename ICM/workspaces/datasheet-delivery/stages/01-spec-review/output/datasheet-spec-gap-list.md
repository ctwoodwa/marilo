# DataSheet Spec Gap List

Running ledger of spec vs. source gaps for MariloDataSheet
(`src/Marilo.Components/DataGrid/MariloDataSheet.*` + `docs/component-specs/datasheet/`).
Each dated section is the output of one spec-review pass. Prior sections are preserved
verbatim; new sections append below. Sibling dated files under
`stages/01-spec-review/output/` (`datasheet-spec-gaps.md`, `datasheet-spec-gaps-2026-04-10.md`,
`datasheet-spec-gaps-verified-2026-04-10.md`, `datasheet-v0*-*-findings.md`) hold the
fuller per-area write-ups that this file references.

---

## 2026-04-11 orchestrator wave 1 (subagent dispatch)

**Worker:** w-datasheet-delivery (subagent-mode)
**Session:** marilo-grid-pipeline-2026-04-11-1200
**Stage:** 01-spec-review
**Component:** MariloDataSheet (lives under `src/Marilo.Components/DataGrid/`)

**Audit scope**
- Source partials read: `MariloDataSheet.razor`, `MariloDataSheet.razor.cs`,
  `MariloDataSheet.Data.cs`, `MariloDataSheet.Editing.cs`,
  `MariloDataSheet.Rendering.cs`, `MariloDataSheet.Interop.cs`,
  `MariloDataSheetColumn.razor` (7 partials, ~1,600 lines).
- Specs read: all 9 files under `docs/component-specs/datasheet/` (overview,
  columns-and-schema, editing-and-validation, selection-and-ranges,
  bulk-paste-and-clipboard, bulk-operations-and-saveall,
  virtualization-and-performance, keyboard-and-accessibility,
  theming-and-css-provider).

**Method**
- Cross-referenced public surface (parameters, event callbacks, public methods,
  ARIA attributes) against spec tables.
- Verified each prior 2026-04-10 gap (V01.1, V01.2, V02.1, V02.2, V04.1–V04.4,
  V05.1–V05.5, V07.1–V07.9) against current source. The `V*`/`F1.*`/`F2.*`
  comment markers in `MariloDataSheet.Data.cs` / `.Editing.cs` / `.Rendering.cs`
  confirm the fixes landed — they are NOT re-listed below.

### Still-open gaps carried from prior audits

- **V03 (carried, Spec-ahead, large):** `docs/component-specs/datasheet/selection-and-ranges.md:37-114`
  — rectangular range selection (anchor/extent, Shift+Click, click-drag,
  Shift+Arrow), `Ctrl+A`, range-scoped Copy/Paste/Fill Down/Delete, and
  `DataSheetSelection<TItem>` model — still entirely absent from source.
  Source only tracks `_activeCellRow`/`_activeCellField` (single cell) and
  `_selectedRows` (row-level `HashSet<TItem>` for bulk delete). Verified in
  prior session (`datasheet-spec-gaps-verified-2026-04-10.md:59-79`), still
  open. Stage-03 resolution design already exists.
- **V07.4 (carried, Spec-ahead):** `docs/component-specs/datasheet/keyboard-and-accessibility.md:62`
  — Ctrl+A "Selects all cells in the DataSheet" — not implemented in
  `MariloDataSheet.Editing.cs HandleKeyDown`. Folded into V03 resolution.
- **V02.3 (carried, Spec-ahead / ambiguous):** per-row validation layer noted in
  prior audit still pending spec re-read; not actioned this pass.

### New gaps surfaced 2026-04-11

- **SA-01 (Spec-ahead):** `docs/component-specs/datasheet/keyboard-and-accessibility.md:74`
  — "The grid root element (`role=\"grid\"`) has `tabindex=\"0\"` and receives
  initial focus when the user tabs into the component" — missing from
  `src/Marilo.Components/DataGrid/MariloDataSheet.razor:6-14`. The root
  `<div id="@_gridId" class="..." role="grid" ...>` has no `tabindex`
  attribute, so the grid is not keyboard-focusable from outside and the
  "roving tabindex" focus model described in the spec cannot engage until a
  cell has been clicked.
- **SA-02 (Spec-ahead, conflict):** `docs/component-specs/datasheet/bulk-operations-and-saveall.md:117`
  — "Appends the row to the end of the internal data list." Source
  `src/Marilo.Components/DataGrid/MariloDataSheet.razor.cs:208` calls
  `_displayRows.Insert(0, newItem)`, i.e. prepends at index 0. Either the
  spec must say "prepended at the top" or `AddRowAsync` must switch to
  `Add(newItem)`.
- **SA-03 (Spec-ahead):** `docs/component-specs/datasheet/bulk-operations-and-saveall.md:119`
  — "The active cell moves to the first editable column of the new row"
  after Add Row. `MariloDataSheet.razor.cs:204-246 AddRowAsync` never calls
  `ActivateCell` for the new row, so the active cell remains wherever the
  user last clicked (or nowhere).
- **SA-04 (Spec-ahead):** `docs/component-specs/datasheet/bulk-operations-and-saveall.md:162`
  — Reset: "The undo buffer is cleared." `MariloDataSheet.Data.cs:494-522
  ResetAsync` clears `_dirtyRows`, calls `ClearActiveCell`, sets the aria
  announcement, and returns. It never touches the `_undoBuffer` dictionary
  defined in `MariloDataSheet.Editing.cs:24`, so Ctrl+Z after Reset can
  still restore stale pre-reset values on cells that were edited earlier.
- **SA-05 (Spec-ahead, conflict):** `docs/component-specs/datasheet/bulk-operations-and-saveall.md:104-107`
  cell-state transition table: "`Saving → Saved (IsSaving set to false)`".
  Source `MariloDataSheet.Data.cs:257-429 SaveAllAsync` drives the
  `Saving → Saved → Pristine` transition itself — it flips entries to
  `CellState.Saving` in Step 4, awaits `OnSaveAll.InvokeAsync`, then flips
  to `CellState.Saved` in Step 6 and schedules the Pristine cleanup via
  `Task.Delay(_savedStateDurationMs)`. The transition is NOT keyed off the
  consumer's `IsSaving` parameter. Either the spec text needs to be
  rewritten to describe the actual component-driven transition, or the
  source needs to observe `IsSaving` changes.
- **SA-06 (Spec-ahead, conflict):** `docs/component-specs/datasheet/selection-and-ranges.md:88`
  Ctrl+D: "Copies the value of the active cell ... down to all cells in
  the same column within the current selection range. Only editable,
  non-computed cells are filled." Source
  `MariloDataSheet.Editing.cs:141-158 HandleKeyDown "d"` branch iterates
  `_selectedRows` (row-level bulk selection, not a cell range) and does
  NOT filter for `column.Editable == true` or
  `column.ColumnType != DataSheetColumnType.Computed`. Until V03 lands,
  Ctrl+D is effectively a row-selection feature rather than a range fill,
  and it can overwrite computed / read-only cells on selected rows.
- **SA-07 (Spec-ahead, conflict):** `docs/component-specs/datasheet/bulk-paste-and-clipboard.md:91`
  — Date coercion: "`DateTime.TryParse` with the **current culture**".
  Source `MariloDataSheet.Editing.cs:569-574 TryParseDateCell` uses
  `CultureInfo.InvariantCulture` to round-trip the
  V04.4 `data-raw-value` attribute. The spec wording is wrong (the
  deliberate round-trip choice was documented in the F1 batch). Spec must
  be corrected to say "InvariantCulture (matches the invariant round-trip
  used by `data-raw-value`)".
- **SA-08 (Spec-ahead):** `docs/component-specs/datasheet/bulk-paste-and-clipboard.md:67`
  — "Paste is disabled when ... The DataSheet is in a saving state
  (`IsSaving=true`)." Source
  `MariloDataSheet.Editing.cs:425-477 PasteFromClipboard` only checks
  `AllowBulkPaste`, `_activeCellRow`, and `_activeCellField`. `IsSaving`
  is not consulted, so a user pasting into a grid mid-save will mutate
  rows that are already flagged `CellState.Saving` via `TransientState`.
- **SA-09 (Spec-ahead):** `docs/component-specs/datasheet/editing-and-validation.md:50`
  — "**Double-click** on any cell" enters edit mode. Source
  `MariloDataSheet.Rendering.cs:122-123` wires only an `onclick` handler.
  The spec's "click on focused cell" path is implemented by
  `MariloDataSheet.Editing.cs:72-100 OnCellClick` (first click activates,
  second click on the active cell enters edit mode), but the **single
  double-click** contract — click any cell, editor opens immediately — is
  not wired.
- **SA-10 (Spec-ahead, conflict):** `docs/component-specs/datasheet/bulk-paste-and-clipboard.md:91`
  duplicate — resolved by SA-07. Kept for cross-reference.
- **SA-11 (Spec-ahead, wording):** `docs/component-specs/datasheet/editing-and-validation.md:139`
  — "The required check and the custom validate delegate are independent —
  both can produce errors, but only one error message is displayed (the
  required error takes priority if both fail)." Source
  `MariloDataSheet.Data.cs:166-192 RunColumnValidation` short-circuits on
  a required failure and returns **without** running `column.Validate`. So
  only one error is ever computed; "both can produce errors" is never true
  for a single commit. Minor documentation error.
- **SA-12 (Spec-ahead, conflict):** `docs/component-specs/datasheet/editing-and-validation.md:193`
  — "The dirty count indicator in the save footer ... does not include
  invalid-only rows in the count." Source
  `MariloDataSheet.razor:31,168` renders the count via
  `_dirtyRows.Count(kv => !kv.Value.IsDeleted && kv.Value.DirtyFields.Count > 0)`.
  Since `CommitCellEdit` adds the field to `DirtyFields` on invalid
  commits as well (Data.cs:110-113), a row that is dirty-AND-invalid on
  the same field IS counted. There is no "invalid-only" row state in the
  current source, so either the spec wording is outdated or the count
  needs a `!kv.Value.ValidationErrors.Any()` filter.
- **SA-13 (Spec-ahead):** `docs/component-specs/datasheet/keyboard-and-accessibility.md:148-154`
  aria-live announcements — spec lists "Saving changes" (start of save),
  "Save failed. {N} validation errors.", and "{N} cells have errors".
  Source `MariloDataSheet.Data.cs` only sets `_ariaAnnouncement` to
  "Save blocked: fix validation errors first." (line 306),
  "Changes saved successfully." (line 384),
  "All changes have been reset." (line 519), and per-commit dirty-count
  strings in `CommitCellEdit` (lines 155-159). Missing announcements:
  start-of-save, generic save-failure, cells-have-errors count. Only the
  blocked-save case is announced, not the post-commit validation-error
  count.
- **SA-14 (Spec-ahead):** `docs/component-specs/datasheet/columns-and-schema.md:118`
  — Number Required "rejects `null` or zero when `Required` is set (zero
  rejection applies only to non-nullable types where `default` is `0`)."
  Source `MariloDataSheet.Data.cs:166-192 RunColumnValidation` only
  checks `value is null` for non-checkbox non-string values. A
  non-nullable `decimal` column with `Required=true` and a value of `0m`
  passes validation. Either drop the "or zero" clause from the spec or
  add an `IsNumeric && default(T).Equals(value)` branch.
- **SA-15 (Spec-ahead):** `docs/component-specs/datasheet/columns-and-schema.md:231`
  — Date required rejects "`null` or `default(DateTime)`". Source only
  rejects `null`. A `DateTime` (non-nullable) column with a value of
  `DateTime.MinValue` (which is `default(DateTime)`) passes the required
  check because it is neither `null` nor a string.
- **SRC-01 (Source-ahead):** `src/Marilo.Components/DataGrid/MariloDataSheet.razor:78-91`
  renders a hard-coded 5-row skeleton (`for (var s = 0; s < 5; s++)`).
  Spec `docs/component-specs/datasheet/virtualization-and-performance.md:77`
  describes a viewport-calculated skeleton row count. Documented as a
  "minor cosmetic divergence" in the 2026-04-10 verified findings; not
  yet addressed. Either the spec wording should say "a fixed number of
  skeleton rows" or the source should derive the count from the
  container height.
- **SRC-02 (Source-ahead, conflict):** `src/Marilo.Components/DataGrid/MariloDataSheet.razor`
  and `src/Marilo.Components/DataGrid/MariloDataSheet.Rendering.cs` both
  emit many hard-coded BEM classes (`mar-datasheet__add-btn`,
  `mar-datasheet__save-btn`, `mar-datasheet__reset-btn`,
  `mar-datasheet__spinner`, `mar-datasheet__dirty-badge`,
  `mar-datasheet__skeleton`, `mar-datasheet__skeleton-row`,
  `mar-datasheet__skeleton-cell`, `mar-datasheet__loading-text`,
  `mar-datasheet__empty`, `mar-datasheet__select-header`,
  `mar-datasheet__actions-header`, `mar-datasheet__aria-live`,
  `mar-datasheet__select-cell`, `mar-datasheet__actions-cell`,
  `mar-datasheet__delete-btn`, `mar-datasheet__cell-text`,
  `mar-datasheet__editor-input`, `mar-datasheet__editor-select`,
  `mar-datasheet__content`, `mar-datasheet__sr-only`).
  Spec `docs/component-specs/datasheet/theming-and-css-provider.md:34`
  states: "The component does not add its own hard-coded classes — all
  styling is delegated to the provider." This is a real contract
  violation: the CSS provider surface currently exposes only 7 methods
  (root, cell, header, row, toolbar, bulk-bar, save-footer) and has no
  hook for the 21 classes above. Either extend `IMariloCssProvider` with
  per-subregion methods (button classes, skeleton classes, editor
  classes, badge classes, sr-only class) or narrow the spec to
  "container-level classes are delegated; BEM element classes below the
  CSS-provider boundary are component-internal." This is an architectural
  decision — not worker-fixable without orchestrator sign-off.
- **NM-01 (Naming mismatch / minor):** `docs/component-specs/datasheet/overview.md:122-123`
  tables `Class` and `Style` as top-level MariloDataSheet parameters.
  Source `MariloDataSheet.razor.cs` does NOT expose `[Parameter] public string? Class`
  or `[Parameter] public string? Style` properties; the razor instead
  uses `@attributes="AdditionalAttributes"` plus `CombineClasses` /
  `CombineStyles` helpers from `MariloComponentBase`. If `Class`/`Style`
  are conventional base-class parameters, the overview table should say
  so (and point to base). If they are not parameters at all, the rows
  should be removed. Either way, the current table overstates the
  component's public surface.

### Counts

| Category | Count |
|---|---|
| Carried Spec-ahead | 3 (V03, V07.4, V02.3) |
| New Spec-ahead | 15 (SA-01 through SA-15; SA-10 is a cross-ref dup of SA-07) |
| New Source-ahead | 2 (SRC-01, SRC-02) |
| New Naming-mismatch | 1 (NM-01) |
| **Total new** | **17 discrete + 3 carried** |

### Escalation candidates for orchestrator

- **SRC-02** (hard-coded BEM classes vs. "all styling delegated to the
  provider") is a provider-contract / public-API change and must be
  decided by the orchestrator — extending `IMariloCssProvider` with new
  methods is out of scope for a spec-review worker.
- **SA-05** (cell-state `Saving → Saved` transition) affects documented
  consumer behavior. Either the spec or the source must change; this is
  an API-visible decision the orchestrator should arbitrate.
- **SA-02** (Add Row position) likewise affects existing consumers of
  `AllowAddRow` — switching from prepend to append would change demo
  behavior and tests.

All other gaps are documentation-side or worker-tractable fixes suitable
for later delivery stages.
