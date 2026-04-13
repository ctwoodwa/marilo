# DataSheet Gap Inventory — Stage 01 Intake

**Worker:** `w-datasheet-gap-analysis`
**Session:** `marilo-grid-pipeline-2026-04-11-1200`
**Wave:** gap-analysis Stage 01 (intake) — fresh bootstrap
**Stage:** `01-intake` (checkpoint — STOP before prioritization)
**Date:** 2026-04-11
**Component:** MariloDataSheet (`src/Marilo.Components/DataGrid/MariloDataSheet*`)
**Mode:** Assess (no prior datasheet-gap-analysis workspace existed; this is the fresh bootstrap)

## Inputs consumed

1. Wave 4 delivery report: `ICM/workspaces/datasheet-delivery/stages/04-sync-check/output/datasheet-delivery-report.md` (gate: BLOCKED; 7 distinct blockers; 9 AMBER; 6 CLEAR).
2. Tick-8 decisions record: `.claude/orchestration/_orchestrator/decisions/tick-8-2026-04-11-1830.md` — specifically P0-1 `datasheet-theming-architecture` (UD-01) and P0-2 `datasheet-10k-rows` (UD-02), both RESOLVED.
3. Wave 1 spec gap list: `ICM/workspaces/datasheet-delivery/stages/01-spec-review/output/datasheet-spec-gap-list.md` (17 new + 3 carried = 20 spec-review gaps).
4. Wave 2 example-UX gap list: `ICM/workspaces/datasheet-delivery/stages/02-example-ux/output/datasheet-example-ux-gap-list.md` (EU-01 .. EU-08).
5. Wave 3 visual-parity gap list: `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-visual-parity-gaps.md` (VP-datasheet-01 .. 12 + D01/D02/D03 deferrals).
6. Wave 3 parity summary: `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-parity-summary.md`.
7. Read-only source reference: `src/Marilo.Components/DataGrid/MariloDataSheet.*` (7 partials, ~1,600 lines) — DataSheet lives alongside MariloDataGrid under the DataGrid folder.
8. Read-only spec reference: `docs/component-specs/datasheet/` (9 markdown files).

## Priority scheme

- `P0-blocker`     — Holds the delivery gate shut. Either architectural (orchestrator-only) or structural (touches multiple sync areas).
- `P0-arch-only`   — P0 blocker that is `orchestrator-only-implementation`. Workers MUST NOT self-implement.
- `P1-primary`     — Primary quality gap. Not gate-blocking on its own but high user visibility.
- `P2-secondary`   — Behavior drift / partial coverage. Worker-tractable in a later pass.
- `P3-polish`      — Minor wording, naming, cosmetic drift.

## Scope scheme

- `single`                          — One atomic gap, one sync area.
- `batch`                           — Group of related gaps to be dispatched in one lane.
- `architectural`                   — Affects a provider or public API surface.
- `orchestrator-only-implementation` — Marked by tick-8 as orchestrator-only. Workers escalate, do not self-implement.

---

## Gap Records (flat inventory)

### Architectural / orchestrator-only — dispatched separately

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `UD-01` | Wave 4 sync-check (OPEN user-decision, RESOLVED tick 8) | `P0-arch-only` | `orchestrator-only-implementation` | `source`, `spec`, `docs`, `tests`, `gap-plan` (provider contract) | `datasheet-theming-architecture` — extends provider contract via a new `IDataSheetTheme` sub-contract, additive to `IMariloCssProvider`. Resolves SRC-02 (21 hard-coded BEM classes vs. "all styling delegated to provider") and unblocks every datasheet VP remediation lane. Cross-refs: Wave 1 `SRC-02`, Wave 2 `EU-06`, Wave 3 `VP-datasheet-D01` + `VP-datasheet-01..11` (structural) + umbrella parity summary "Primary remediation lanes". | Cite tick-8 decisions record § "P0-1 datasheet-theming-architecture → RESOLVED". **Workers may NOT self-implement** — orchestrator dispatches separately in three lanes: (L1) define `IDataSheetTheme` surface, (L2) FluentUI provider implementation mirroring the DataGrid SCSS layer, (L3) Material provider 5-line stub. Constraint: additive to `IMariloCssProvider` per `marilo.json` `architecture_constants.public_api_stability = additive-only`. Cross-ref `.claude/orchestration/_memory/projects/marilo.json` `orchestrator_only_changes` → "provider contract modifications". |

### P0 blockers — demo / spec threshold

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `UD-02` | Wave 4 sync-check (OPEN user-decision, RESOLVED tick 8) | `P0-blocker` | `batch` | `spec`, `demo` | `datasheet-10k-rows` — cap the DocFX demo at 5,000 rows and add a spec threshold note "10,000 rows supported with `EnableVirtualization=true`; see Phase B roadmap." Unblocks Wave 2 `EU-01` upper threshold and Wave 3 `VP-datasheet-D03` deferral. Cross-refs: `docs/component-specs/datasheet/virtualization-and-performance.md` (Recommended Thresholds table), `BulkOperations.razor` scenario E (currently caps at 500 rows). | Cite tick-8 decisions record § "P0-2 datasheet-10k-rows → RESOLVED". Single small lane, low effort: (a) edit `docs/component-specs/datasheet/virtualization-and-performance.md` to land the exact text `"10,000 rows is supported with EnableVirtualization=true; see Phase B roadmap."`; (b) in `samples/Marilo.Demo/Pages/Components/DataSheet/BulkOperations.razor` (or a new `Virtualization.razor` page), add a 5,000-row scenario and do NOT add a 10k-row scenario. No source changes, no tests impact (no assertion exists on row-count cap). Note: DataGrid GAP_ANALYSIS confirms virtualization (`EnableVirtualization`, `VirtualizeOverscanCount`) is Phase B JS interop — not yet implemented, so the DocFX WASM host cannot absorb 10k rows without visible lag. |

### P0 blockers — source-side behavioral gaps (gate the delivery report)

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `V03` | Wave 1 spec-review (carried from 2026-04-10) | `P0-blocker` | `single` (large) | `source`, `spec`, `demo`, `tests` | `docs/component-specs/datasheet/selection-and-ranges.md:37-114` — rectangular range selection, `DataSheetSelection<TItem>` model, Shift+Click, click-drag, Shift+Arrow, `Ctrl+A`, range-scoped Copy/Paste/Fill Down/Delete — entirely absent from source. Source tracks only `_activeCellRow`/`_activeCellField` (single cell) and `_selectedRows` (row-level `HashSet<TItem>`). Gates Wave 4 delivery checklist 1.2, 2.1 (selection-and-ranges Blocked-by-source), `VP-datasheet-D02` deferral, Wave 2 `EU-07`. | Non-trivial source lane. Introduce `DataSheetSelection<TItem>` model in `src/Marilo.Components/DataGrid/` with anchor/extent fields, keyboard handlers in `MariloDataSheet.Editing.cs`, range-aware `Copy/Paste/FillDown/Delete`. Pair with `EU-07` demo scenario and new bUnit tests. Folds in `V07.4` (`Ctrl+A`). |
| `V07.4` | Wave 1 spec-review (carried) | `P0-blocker` | `single` | `source`, `tests` | `docs/component-specs/datasheet/keyboard-and-accessibility.md:62` — Ctrl+A "Selects all cells in the DataSheet" — not implemented in `MariloDataSheet.Editing.cs HandleKeyDown`. Folds into V03 range-selection lane per Wave 1 audit. | Resolve as part of the `V03` range-selection lane. No separate dispatch. |
| `SA-01` | Wave 1 spec-review (new 2026-04-11) | `P0-blocker` | `single` | `source`, `spec`, `tests` | `docs/component-specs/datasheet/keyboard-and-accessibility.md:74` — grid root `tabindex="0"` documented but absent from `MariloDataSheet.razor:6-14`. Grid is not keyboard-focusable from outside; roving-tabindex focus model cannot engage. Gates `VP-datasheet-12` focus-visible styling. | Add `tabindex="0"` (or data-binding equivalent) to grid root `<div role="grid">`. Add bUnit test asserting root has `tabindex="0"`. Tiny source change; small scope. |
| `SA-13` | Wave 1 spec-review (new 2026-04-11) | `P0-blocker` | `batch` | `source`, `spec`, `demo`, `tests` | `docs/component-specs/datasheet/keyboard-and-accessibility.md:148-154` — three missing `aria-live` announcements: "Saving changes" (start of save), "Save failed. {N} validation errors.", "{N} cells have errors". Source `MariloDataSheet.Data.cs` only announces save-blocked, save-success, reset, and per-commit dirty count. Gates Wave 4 checklist 2.6 (error state demo) + 1.2, and `EU-05`. | Extend `SaveAllAsync` in `MariloDataSheet.Data.cs` with start-of-save, save-failure, and cells-have-errors announcements. Pair with `EU-05` demo scenario (SaveAllAsync failure + retry). Add bUnit test verifying `_ariaAnnouncement` text for each path. |
| `VP-datasheet-01` | Wave 3 visual-parity (umbrella) | `P0-blocker` | `architectural` | (satisfied by UD-01 once dispatched) | Umbrella structural gap — zero `mar-datasheet*` SCSS rules exist in any provider. `ripgrep mar-datasheet` against FluentUI / Bootstrap / Material Styles returns 0 hits; `find _data-sheet*.scss` returns 0 files. Component renders with browser-default `<table>` presentation: no grid lines, no header fill, no selection, no cell-state tinting, no dark-mode overrides. Every primary state scores 0 in all 6 theme/mode cells. | Gated on `UD-01` resolution. Once the `IDataSheetTheme` surface is defined, the remediation splits into 6 parallel lanes (see VP-datasheet-02..11 below). |
| `VP-datasheet-02` | Wave 3 visual-parity | `P0-blocker` | `single` (lane `vp-datasheet-fluent-light`) | `source` (provider SCSS only) | Fluent Light cell grid default — no `_data-sheet.scss` exists in `src/Marilo.Providers.FluentUI/Styles/components/`. No `border-collapse`, no width, no cell borders. Target: 1px `neutralStrokeLayer1` borders, neutral-layer-1 background, 13px segoeui, 32px row height. | Gated on `UD-01`. Lane `vp-datasheet-fluent-light`: author `_data-sheet.scss` in FluentUI provider using tokens from `foundation/_tokens.scss`. |
| `VP-datasheet-03` | Wave 3 visual-parity | `P0-blocker` | `single` (lane `vp-datasheet-fluent-dark`) | `source` (provider SCSS only) | Fluent Dark cell grid default — no dark-mode block for any `mar-datasheet*` selector. 0 SCSS hits in Fluent tree. Target: `neutralLayer2Dark` background, `neutralStrokeDark` 1px borders, dark-mode `--marilo-color-neutral-foreground-rest`. | Gated on `UD-01`. Lane `vp-datasheet-fluent-dark`: add `[data-marilo-theme="dark"]` block inside `_data-sheet.scss`. |
| `VP-datasheet-04` | Wave 3 visual-parity | `P0-blocker` | `single` (lane `vp-datasheet-fluent-selection`) | `source` (provider SCSS only) | Cell selection (single active cell) — `mar-datasheet__cell--active` modifier emitted by `FluentUICssProvider.DataSheetCellClass` but no SCSS rule targets it. Active cell indistinguishable from inactive. Target: 2px Fluent brand-stroke outline inset, `:focus-visible` brand-stroke-focus shadow. | Gated on `UD-01`. Lane `vp-datasheet-fluent-selection`: implement `.mar-datasheet__cell--active` in Fluent `_data-sheet.scss`. |
| `VP-datasheet-05` | Wave 3 visual-parity | `P0-blocker` | `single` (lane `vp-datasheet-fluent-editor`) | `source` (provider SCSS only) | Editable cell (in edit mode) — `Rendering.cs:233/247/264/279` emits `<input class="mar-datasheet__editor-input">` / `<select class="mar-datasheet__editor-select">` but no provider SCSS styles these classes. Browser-default chrome breaks in-place editing illusion. | Gated on `UD-01`. Lane `vp-datasheet-fluent-editor`: add `.mar-datasheet__cell.is-editing > .mar-datasheet__editor-input` / `__editor-select` selectors. Material variant uses underline focus. |
| `VP-datasheet-06` | Wave 3 visual-parity | `P0-blocker` | `single` (lane `vp-datasheet-fluent-validation`) | `source` (provider SCSS) + follow-up `source` (inline-style migration, gated on SRC-02/UD-01) | Validation error cell — `mar-datasheet__cell--invalid` emitted but not styled. Sr-only error uses hard-coded inline `style=` attribute in `Rendering.cs:156-158` (bypasses provider). Users only discover errors via hover tooltip. | Primary: implement `.mar-datasheet__cell--invalid` using red-palette tokens. Secondary follow-up (gated on UD-01 landing): migrate inline sr-only `style=` to provider class `mar-datasheet__sr-only`. |
| `VP-datasheet-07` | Wave 3 visual-parity | `P0-blocker` | `single` (dual — source + SCSS) | `source`, `spec`, `tests` | Frozen column — source emits no freeze-column classes. Spec `columns-and-schema.md` lists frozen-column as in-scope. Dual gap: (a) source-side `Frozen` property + render-time sticky class; (b) provider SCSS `.mar-datasheet__cell--frozen` + `__header-cell--frozen` with `position: sticky`, border-right, shadow. | Two lanes. Source half: add `Frozen` property to `MariloDataSheetColumn.razor`, render sticky class. SCSS half (gated on `UD-01`): add `.mar-datasheet__cell--frozen` selector set across all 3 providers. |
| `VP-datasheet-09` | Wave 3 visual-parity | `P0-blocker` | `single` (lane `vp-datasheet-fluent-selection`) | `source` (provider SCSS only) | Row selection — `mar-datasheet__row--selected` emitted by bulk-checkbox path but no SCSS styles it. `mar-datasheet__bulk-bar--visible` modifier also unstyled. BulkOperations scenario C demo depends on visible distinction. | Gated on `UD-01`. Lane `vp-datasheet-fluent-selection`: implement `.mar-datasheet__row--selected` and `.mar-datasheet__bulk-bar[--visible]`. Merge with VP-04 lane. |
| `VP-datasheet-10` | Wave 3 visual-parity | `P0-blocker` | `single` (lane `vp-datasheet-bootstrap-bridge`) | `source` (provider SCSS only) | Bootstrap bridge — 0 SCSS rules for any `mar-datasheet*` selector. No `_bridge-data-sheet.scss`. Bootstrap provider emits class names but ships zero styles. | Gated on `UD-01`. Lane `vp-datasheet-bootstrap-bridge`: author `_bridge-data-sheet.scss` modeled on existing `_bridge-data.scss`. Use `[data-bs-theme="dark"]` for dark-mode block (not `[data-marilo-theme="dark"]`). |
| `VP-datasheet-11` | Wave 3 visual-parity | `P0-blocker` | `single` (lane `vp-datasheet-material`) | `source` (provider SCSS only) | Material — `_data-sheet.scss` does not exist. Material Density bar (40px row / 48px header) cannot apply without provider SCSS. Secondary dependency: Material runtime provider status (SCSS-only scaffold as of 2026-04-10). | Gated on `UD-01`. Lane `vp-datasheet-material`: author `_data-sheet.scss` in Material provider using Material density tokens. Material 5-line-stub pattern (tick-8 Pattern 5) applies if runtime is not yet implemented — mark SCSS-ready-blocked-on-runtime. |

### P0-workspace — bootstrap gap

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `WS-01` | Wave 4 sync-check | `P0-blocker` | `single` | `gap-plan` | Wave 4 checklist items 3.1 / 3.3 / 3.5 / 5.1 BLOCKED because `datasheet-gap-analysis` workspace did not exist before this tick. Bootstrap begins with this Stage 01 intake; `_config/coverage-summary.md` and `_config/gap-context.md` must be initialized so future delivery sync-checks can evaluate coverage. | This intake run itself partially satisfies WS-01 by creating the `gap-inventory.md` + initializing `gap-context.md`. Full closure requires `coverage-summary.md` to be populated in Stage 02 (prioritize) — out of scope for this checkpoint. |

### P1 primary — demo coverage additions (worker-tractable)

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `EU-01` | Wave 2 example-UX | `P1-primary` | `batch` (with UD-02) | `demo`, `spec` | Add >1,000-row virtualization scenario to BulkOperations scenario E (or new page) with 1k / 5k row toggles. Upper threshold (10k) is out of scope per UD-02. | Merge dispatch with UD-02 — both land the 5k demo + spec threshold note in a single lane. |
| `EU-02` | Wave 2 example-UX | `P1-primary` | `single` | `demo` | Add copy → paste round-trip scenario to BulkOperations scenario D (or new scenario F) — Ctrl+C then Ctrl+V of same data, exercising Format / `data-raw-value` round-trip contract from V04.4. | Single demo edit. No source or spec change. |
| `EU-03` | Wave 2 example-UX | `P1-primary` | `single` | `demo` | Add paste-blocked-during-save scenario. Sets `IsSaving=true` on a timer; user attempts Ctrl+V; paste is rejected. Directly proves spec claim at `bulk-paste-and-clipboard.md:67`. Cross-ref Wave 1 `SA-08` (source currently does NOT check `IsSaving` in `PasteFromClipboard`). | **Depends on SA-08 source fix first** — source must actually honor `IsSaving` before demo can be truthful. Pair with SA-08 or queue after. |

### P1-secondary — partial / wording drift

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `SA-08` | Wave 1 spec-review (new 2026-04-11) | `P1-primary` | `single` | `source`, `tests` | `bulk-paste-and-clipboard.md:67` says paste is disabled when `IsSaving=true`. Source `MariloDataSheet.Editing.cs:425-477 PasteFromClipboard` only checks `AllowBulkPaste`, `_activeCellRow`, `_activeCellField`. A user pasting mid-save mutates rows already flagged `CellState.Saving`. | Add `if (IsSaving) return;` early guard in `PasteFromClipboard`. bUnit test: assert paste is no-op when `IsSaving=true`. Paired with `EU-03` demo. |
| `SA-09` | Wave 1 spec-review | `P1-primary` | `single` | `source`, `spec`, `tests` | `editing-and-validation.md:50` — "Double-click on any cell" enters edit mode. Source `Rendering.cs:122-123` only wires `onclick`; the "click-click-edit" path exists in `Editing.cs:72-100 OnCellClick` but the single-double-click contract is not wired. | Decide spec vs source: either remove "double-click" wording from spec OR add `ondblclick` handler. Orchestrator arbitration recommended (`SA-09` was not in Wave 1 escalation shortlist but it's API-visible). |

### P1-secondary — source drift (escalation candidates from Wave 1)

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `SA-02` | Wave 1 spec-review | `P1-primary` | `single` | `spec` **or** `source`+`tests` (orchestrator arbitrates) | `bulk-operations-and-saveall.md:117` — "Appends the row to the end." Source `MariloDataSheet.razor.cs:208` does `_displayRows.Insert(0, newItem)` (prepend). Either the spec changes to "prepended at the top" or source flips to `Add(newItem)`. | **Orchestrator arbitration required** (Wave 1 escalation candidate). Affects existing demos + any tests. Worker cannot self-decide. |
| `SA-03` | Wave 1 spec-review | `P1-primary` | `single` | `source`, `tests` | `bulk-operations-and-saveall.md:119` — "Active cell moves to first editable column of the new row" after Add Row. Source `AddRowAsync` never calls `ActivateCell`. | Add `ActivateCell` call for new row's first editable column at end of `AddRowAsync`. Unit test. |
| `SA-04` | Wave 1 spec-review | `P1-primary` | `single` | `source`, `tests` | `bulk-operations-and-saveall.md:162` — Reset: "The undo buffer is cleared." `MariloDataSheet.Data.cs:494-522 ResetAsync` never touches `_undoBuffer`; Ctrl+Z after Reset restores stale pre-reset values. | Add `_undoBuffer.Clear();` to `ResetAsync`. Unit test covering Ctrl+Z-after-Reset regression. |
| `SA-05` | Wave 1 spec-review | `P1-primary` | `single` | `spec` **or** `source`+`tests` (orchestrator arbitrates) | `bulk-operations-and-saveall.md:104-107` — cell-state transition table says `Saving → Saved (IsSaving set to false)`. Source drives the transition itself via `Task.Delay(_savedStateDurationMs)`, not keyed off the consumer's `IsSaving`. API-visible consumer-facing wording. | **Orchestrator arbitration required** (Wave 1 escalation candidate). Either rewrite spec to describe component-driven transition OR observe `IsSaving` changes in source. |
| `SA-06` | Wave 1 spec-review | `P2-secondary` | `single` | `demo`, `spec` (demo-wording drift) | `selection-and-ranges.md:88` — Ctrl+D "Copies the value ... down to all cells in the same column within the current selection range. Only editable, non-computed cells are filled." Source iterates `_selectedRows` (row-level) without filtering for `Editable` or `ColumnType != Computed`. Demo page `Keyboard-and-Accessibility.razor:100` also overstates. | Two paths: (a) soften demo wording now (doc-only fix) OR (b) wait for `V03` range-selection to land and then implement the spec-correct behavior. Orchestrator decision. |

### P2 secondary — spec-side wording and documentation fixes

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `SA-07` | Wave 1 spec-review | `P2-secondary` | `single` | `spec` | `bulk-paste-and-clipboard.md:91` — says date coercion uses "`DateTime.TryParse` with the current culture". Source `Editing.cs:569-574 TryParseDateCell` uses `CultureInfo.InvariantCulture` (deliberate V04.4 round-trip choice). Spec wording is wrong. | Update spec to: "InvariantCulture (matches the invariant round-trip used by `data-raw-value`)". |
| `SA-10` | Wave 1 spec-review | `P2-secondary` | `single` | `spec` | Duplicate cross-reference of SA-07 per Wave 1 list. Track for audit-trail, resolved with SA-07. | Resolved with SA-07. Dedup during Stage 02 prioritization. |
| `SA-11` | Wave 1 spec-review | `P2-secondary` | `single` | `spec` | `editing-and-validation.md:139` — "both can produce errors, but only one error message is displayed (the required error takes priority if both fail)". Source `Data.cs:166-192 RunColumnValidation` short-circuits on required failure and never runs `column.Validate`. "Both can produce errors" is never true for a single commit. | Rewrite spec paragraph to describe the actual short-circuit. |
| `SA-12` | Wave 1 spec-review | `P2-secondary` | `single` | `spec` **or** `source` | `editing-and-validation.md:193` — "dirty count indicator does not include invalid-only rows in the count". Source `razor:31,168` counts `_dirtyRows.Count(kv => !kv.Value.IsDeleted && kv.Value.DirtyFields.Count > 0)`. Since invalid commits also add to DirtyFields, invalid-AND-dirty rows ARE counted. No "invalid-only" state exists. | Either drop the clause from the spec OR add a `!kv.Value.ValidationErrors.Any()` filter to the count predicate. Worker-tractable spec fix recommended. |
| `SA-14` | Wave 1 spec-review | `P2-secondary` | `single` | `spec` **or** `source`+`tests` | `columns-and-schema.md:118` — Number Required "rejects `null` or zero when `Required` is set (zero rejection applies only to non-nullable types where `default` is `0`)." Source only checks `value is null`. `decimal` `0m` passes Required validation. | Either drop the "or zero" clause OR add an `IsNumeric && default(T).Equals(value)` branch to `RunColumnValidation`. Spec fix simpler; source fix may surprise callers. |
| `SA-15` | Wave 1 spec-review | `P2-secondary` | `single` | `spec` **or** `source`+`tests` | `columns-and-schema.md:231` — Date Required rejects `null` or `default(DateTime)`. Source only rejects `null`. `DateTime.MinValue` (=`default(DateTime)`) passes. | Mirror resolution of `SA-14`. Prefer spec-side drop unless there's a real need. |
| `SRC-01` | Wave 1 spec-review | `P3-polish` | `single` | `spec` **or** `source` | `MariloDataSheet.razor:78-91` renders a hard-coded 5-row skeleton (`for (var s = 0; s < 5; s++)`). Spec `virtualization-and-performance.md:77` describes viewport-calculated skeleton count. "Minor cosmetic divergence". | Either narrow spec wording to "a fixed number of skeleton rows" OR compute from container height. Spec fix recommended — matches the existing skeleton. |
| `NM-01` | Wave 1 spec-review | `P3-polish` | `single` | `spec` | `overview.md:122-123` tables `Class` and `Style` as top-level MariloDataSheet parameters. Source does not expose these as `[Parameter]`s; they come from `MariloComponentBase` via `AdditionalAttributes`. Spec overstates public surface. | Either remove rows or annotate as "inherited from `MariloComponentBase`". Spec-side doc fix. |

### P2 secondary — demo additions

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `EU-04` | Wave 2 example-UX | `P2-secondary` | `single` | `demo` | Delete-key clears selected cells scenario in `Keyboard-and-Accessibility.razor`. Currently listed in `_keyboard[]` table but not interactive. | Single demo edit. Small footprint. Partially gated on Ctrl+A until V03 lands. |
| `EU-05` | Wave 2 example-UX | `P2-secondary` | `single` | `demo` | SaveAllAsync failure + retry scenario. Simulate server error in `HandleSaveAll`, show aria-live "Save failed" announcement. Cross-refs `SA-13` (source must emit the announcement first). | **Depends on `SA-13` source fix first.** Pair with `SA-13` or queue after. |
| `EU-08` | Wave 2 example-UX | `P2-secondary` | `single` | `demo` | `CellTemplate` custom-rendering scenario. Carried from 2026-04-10 audit. Small footprint, not architecture-blocked. | Single demo edit. Worker-tractable now. |

### P2 secondary — visual parity (non-critical)

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `VP-datasheet-08` | Wave 3 visual-parity | `P2-secondary` | `single` (folded into umbrella) | `source` (provider SCSS only) | Row hover — no `:hover` affordance anywhere. Not capture-matrix primary but listed in wave-3 target set. Severity: Major. | Gated on `UD-01`. Add `.mar-datasheet__row:hover:not(.mar-datasheet__row--selected)` selector. Trivial addition once `_data-sheet.scss` exists; fold into Fluent Light lane. |
| `VP-datasheet-12` | Wave 3 visual-parity | `P2-secondary` | `single` (blocked on SA-01) | `source` (provider SCSS), depends on `SA-01` source fix | Focused cell (keyboard focus distinct from mouse-activated cell) — `mar-datasheet__cell--active` represents "last clicked", not "has keyboard focus". Grid root has no `tabindex=0` (SA-01). No `:focus-visible` target exists. Severity: Major, score 1 (partial). | Two-step: (a) land `SA-01` first, then (b) add `.mar-datasheet__cell:focus-visible` with brand-stroke-focus outline. Classified `DEFERRED-PENDING-SOURCE` by Wave 3. |

### Deferrals carried forward (not gaps to remediate in Stage 05, tracked for record-of-gate)

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `VP-datasheet-D01` | Wave 3 visual-parity | `P0-blocker` | `orchestrator-only-implementation` | — | EU-06 theming side-by-side demo. `DEFERRED-PENDING-ARCHITECTURE`. Was blocked on `UD-01` (now RESOLVED). | Unblocks automatically once UD-01 implementation lands. Re-enters scope in Stage 05 via the `EU-06` demo task. |
| `VP-datasheet-D02` | Wave 3 visual-parity | `P0-blocker` | `single` (dependent on V03) | — | Rectangular range-selection capture. `DEFERRED-PENDING-SOURCE`. Gated on `V03` source model landing. | Unblocks automatically once `V03` lands. |
| `VP-datasheet-D03` | Wave 3 visual-parity | `P0-blocker` | `batch` (with UD-02) | — | 10k-row virtualization capture. `DEFERRED-PENDING-SCOPE`. Was blocked on `UD-02` (now RESOLVED: cap at 5k + tested-but-not-demoed note). | **Resolved by UD-02.** No 10k capture will ever be made. Classified as WONTFIX at capture level; record kept for audit trail. Effective closure coincides with UD-02 implementation. |

### Clear / CLEAR-passed (record-of-gate only, no action)

These items are captured for completeness so Stage 02 prioritization can see the full Wave 4 context. They passed the delivery checklist and do NOT enter the remediation pipeline.

| id | origin_stage | priority | scope | sync_areas | rationale | resolution_hint |
|---|---|---|---|---|---|---|
| `CLEAR-1.5` | Wave 4 sync-check | — | — | — | All events documented and implemented (`OnRowChanged`, `OnSaveAll`, `OnValidate`). | No action. |
| `CLEAR-2.2` | Wave 4 sync-check | — | — | — | Every spec event has at least one demo scenario. | No action. |
| `CLEAR-2.8` | Wave 4 sync-check | — | — | — | Zero Telerik component references in demo pages (verified across 4 pages, ~1,488 lines). | No action. |
| `CLEAR-4.1` | Wave 4 sync-check | — | — | — | Visual parity review completed (Wave 3 static-analysis pass). | No action. |
| `CLEAR-4.3` | Wave 4 sync-check | — | — | — | Parity scores documented for primary states across all active themes (54 scoring points; aggregate 0.22/3 identical across providers because gap is structural). | No action. |
| `CLEAR-5.3` | Wave 4 sync-check | — | — | — | No parameter rename drift between spec and demo. | No action. |

---

## Scope Rollup

### By priority

| Priority | Count |
|---|---:|
| `P0-arch-only` (orchestrator-only-implementation) | 1 (UD-01) |
| `P0-blocker` (actionable) | 16 (UD-02, V03, V07.4, SA-01, SA-13, VP-datasheet-01..07, 09, 10, 11, WS-01) |
| `P0-blocker` (deferrals tracked) | 3 (VP-datasheet-D01/D02/D03) |
| `P1-primary` | 9 (EU-01, EU-02, EU-03, SA-02, SA-03, SA-04, SA-05, SA-08, SA-09) |
| `P2-secondary` | 11 (SA-06, SA-07, SA-10, SA-11, SA-12, SA-14, SA-15, EU-04, EU-05, EU-08, VP-datasheet-08, VP-datasheet-12) |
| `P3-polish` | 2 (SRC-01, NM-01) |
| **Total actionable** | **39** |
| **Total with deferrals** | **42** |
| **CLEAR items (record-only, no action)** | **6** |

Note: SA-06 counted once under P2-secondary despite being cross-listed earlier; SA-10 is a dedup target for SA-07.

### By scope

| Scope | Count | IDs |
|---|---:|---|
| `orchestrator-only-implementation` | 2 | UD-01, VP-datasheet-D01 |
| `architectural` | 1 | VP-datasheet-01 (umbrella, gated on UD-01) |
| `batch` | 3 | UD-02 (+ EU-01, VP-datasheet-D03 pair), SA-13 (+ EU-05 pair), EU-01+UD-02 merge candidate |
| `single` | 33 | everything else |

### By sync_area (what a remediation lane must touch)

| Sync area | Count of gaps that touch it |
|---|---:|
| `source` (component code) | 15 (V03, V07.4, SA-01, SA-02?, SA-03, SA-04, SA-05?, SA-08, SA-09?, SA-12?, SA-13, SA-14?, SA-15?, VP-datasheet-07 source half, UD-01 implementation) |
| `source` (provider SCSS) | 10 (VP-datasheet-01..07, 08, 09, 10, 11) |
| `spec` | 17 (NM-01, SA-01, SA-02, SA-05, SA-07, SA-09, SA-10, SA-11, SA-12, SA-13, SA-14, SA-15, SRC-01, UD-02, V03, VP-datasheet-07, WS-01) |
| `demo` | 11 (EU-01, EU-02, EU-03, EU-04, EU-05, EU-08, SA-06, SA-13, UD-02, V03, VP-datasheet-D03 effective closure) |
| `docs` (DocFX articles) | 1 (UD-01 implementation) |
| `tests` | 13 (V03, SA-01, SA-03, SA-04, SA-08, SA-13, SA-14?, SA-15?, VP-datasheet-07, and UD-01 follow-on tests) |
| `gap-plan` | 1 (WS-01 workspace bootstrap) |

Counts are approximate — gaps marked `?` depend on orchestrator arbitration whether spec or source changes. Stage 02 will firm these up before prioritization.

### Cross-workspace dependencies

| Dependency | Source | Target | Note |
|---|---|---|---|
| UD-01 implementation | `datasheet-gap-analysis` (this workspace) | provider contract (`IMariloCssProvider` + new `IDataSheetTheme`) | Orchestrator-dispatched in 3 separate lanes. NOT in this workspace's remediation pipeline. |
| Pattern 5 (Material 5-line stubs) | tick-8 cross-component pattern | `docs/provider-material/OPEN-STUBS.md` | VP-datasheet-11 (Material) follows the systemic stub pattern. Not expanded in this wave. |
| Pattern 2 (_dark-mode.scss mandatory) | tick-8 cross-component pattern | every `src/Marilo.Providers.FluentUI/Styles/components/<comp>/` folder | Applies to `_data-sheet.scss` when it is authored under UD-01 lanes. |
| Pattern 4 (`#fff` fallback literal) | tick-8 cross-component pattern | all `Styles/**/*.scss` | Any new `_data-sheet.scss` must use `var(--mar-color-surface, #fff)` form, not bare `#fff`. |

---

## Annotations required by inbox (explicit callouts)

### UD-01 — `IDataSheetTheme` sub-contract — ORCHESTRATOR-ONLY-IMPLEMENTATION

- **Scope:** `orchestrator-only-implementation`.
- **Reason:** Per `.claude/rules/orchestration.md` "Architecture-Level Changes" and `.claude/orchestration/_memory/projects/marilo.json` `orchestrator_only_changes` — provider contract modifications and `IMariloCssProvider` public API changes are orchestrator-only. Tick-8 decisions record § "P0-1 datasheet-theming-architecture → RESOLVED" explicitly states: "Implementation work is orchestrator-dispatched only. **Workers may NOT self-implement.**"
- **Flag:** `orchestrator-only-implementation` — Workers may NOT self-implement; dispatched separately by orchestrator once contract surface is defined. Stage 05 of THIS workspace must NOT attempt the contract change. The three lanes (L1 surface definition, L2 FluentUI implementation, L3 Material stub) are orchestrator-dispatched outside the normal gap-analysis pipeline.
- **Resolution hint:** Cite tick-8 decisions record § "P0-1 datasheet-theming-architecture → RESOLVED" — Path A chosen (provider-contract extension via `IDataSheetTheme` sub-contract, additive to `IMariloCssProvider`, mirrors how DataGrid already has its own SCSS layer). Constraint: additive-only per `marilo.json` `architecture_constants.public_api_stability`. Unblocks 5 datasheet sync-check checklist items + every VP remediation lane gated on UD-01.

### UD-02 — 5k row cap + 10k tested-not-demoed note

- **Scope:** `batch`.
- **Priority:** `P0-blocker`.
- **Sync areas:** `spec` + `demo`.
- **Note:** Single small lane, low effort — spec threshold note + demo row cap. No source work. No tests impact.
- **Resolution hint:** Cite tick-8 decisions record § "P0-2 datasheet-10k-rows → RESOLVED". Spec text to land (verbatim): `"10,000 rows is supported with EnableVirtualization=true; see Phase B roadmap."` in `docs/component-specs/datasheet/virtualization-and-performance.md`. Demo cap: 5,000 rows in `samples/Marilo.Demo/Pages/Components/DataSheet/BulkOperations.razor` (scenario E) or a new `Virtualization.razor` page. Auto-unblocks Wave 3 `VP-datasheet-D03` deferral (effective WONTFIX at capture level — no 10k capture will ever be made).

---

## Checkpoint

**STOP — end of Stage 01 intake.**

- Stage 02 (prioritize) is **NOT** executed in this turn.
- Stage 03 (resolution-design) is **NOT** executed.
- `IDataSheetTheme` surface definition is **NOT** authored here — orchestrator-only lane.
- The 5k demo cap is **NOT** implemented here — Stage 05 lane, to be dispatched after Stage 02 prioritization.
- This inventory is intentionally flat and exhaustive. Stage 02 will group, sequence, and remove duplicates (SA-07 / SA-10).

**Output handoff:** Stage 02 worker ingests this file plus `_config/gap-context.md` and emits a prioritized sequence.

**Total record count in this inventory:**
- Actionable gaps: **39** (1 `P0-arch-only` + 16 `P0-blocker` + 9 `P1-primary` + 11 `P2-secondary` + 2 `P3-polish`)
- Tracked deferrals: **3** (VP-datasheet-D01/D02/D03)
- CLEAR record-of-gate items: **6** (CLEAR-1.5, -2.2, -2.8, -4.1, -4.3, -5.3)
- **Grand total records: 48**
