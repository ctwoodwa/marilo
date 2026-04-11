# DataSheet Example-UX Gap List

Running ledger of demo vs. spec coverage gaps for MariloDataSheet
(`samples/Marilo.Demo/Pages/Components/DataSheet/*.razor` vs.
`docs/component-specs/datasheet/*.md`). Each dated section is the output of
one example-UX audit pass. Prior sections are preserved verbatim; new
sections append below. The fuller 2026-04-10 pre-refactor audit lives
alongside this file at
`stages/02-example-ux/output/datasheet-example-ux-audit-2026-04-10.md`.

---

## 2026-04-11 orchestrator wave 2 (subagent dispatch)

**Worker:** w-datasheet-delivery (subagent-mode)
**Session:** marilo-grid-pipeline-2026-04-11-1200
**Stage:** 02-example-ux
**Component:** MariloDataSheet (demos under `samples/Marilo.Demo/Pages/Components/DataSheet/`)

### Scope

Inventory every MariloDataSheet demo page and cross-reference against the 9
spec topics (`overview`, `selection-and-ranges`, `editing-and-validation`,
`bulk-paste-and-clipboard`, `keyboard-and-accessibility`,
`bulk-operations-and-saveall`, `columns-and-schema`,
`virtualization-and-performance`, `theming-and-css-provider`). Classify each
topic's demo coverage as Covered / Partial / Missing / Orphan / Blocked-by-source.

MariloDataGrid demos are **excluded** — handled by `w-datagrid-delivery`.

### Demo pages inventoried (4 total)

| # | Demo page | Lines | Scenarios | Route |
|---|-----------|------:|----------:|-------|
| 1 | `Overview.razor` | 208 | 1 (Investment Position Editor) | `/components/DataSheet`, `/components/DataSheet/overview` |
| 2 | `Editing-and-Validation.razor` | 465 | 5 (Required / Column-validator / Cross-row / IsLoading / Reset) | `/components/DataSheet/editing-and-validation` |
| 3 | `BulkOperations.razor` | 443 | 5 (Add+Save / Delete-Undo / Bulk select / Paste TSV / Virtualization side-by-side) | `/components/DataSheet/bulk-operations` |
| 4 | `Keyboard-and-Accessibility.razor` | 372 | 5 (Nav / Edit-mode / Command / Space-checkbox / ARIA info) | `/components/DataSheet/keyboard-and-accessibility` |

**Total:** 4 pages, 16 scenarios, ~1,488 lines of demo code. This matches the
Priority-1 multi-page structure recommended by the 2026-04-10 audit — the
refactor has landed since that report and all four pages compile against
the current `MariloDataSheet<TItem>` API.

### Coverage by spec topic

| # | Spec topic | Status | Demo(s) | Evidence |
|---|------------|--------|---------|----------|
| 1 | `overview.md` | **Covered** | Overview.razor (full scenario) | Lines 12–59 build a 7-column position editor that hits Text / Select / Number / Date / Checkbox / Computed column types in one grid; event log shows `OnRowChanged`, `OnSaveAll`, `OnValidate` wiring. |
| 2 | `selection-and-ranges.md` | **Blocked-by-source** | None (only row-select checkbox in BulkOperations scenario C) | Spec (`selection-and-ranges.md:37-114`) describes rectangular range selection, Shift+Click, click-drag, Shift+Arrow, `DataSheetSelection<TItem>`. Source has no range model — see Wave 1 gap **V03 (carried)** and **V07.4**. Nothing to demo until V03 lands. BulkOperations scenario C demos row-level bulk checkbox, which is a different model. |
| 3 | `editing-and-validation.md` | **Covered** | Editing-and-Validation.razor (scenarios A–E) | Scenario A = Required fields, B = column Validate, C = cross-row OnValidate, D = IsLoading skeleton, E = ResetAsync. All five visible behaviours match spec sections. |
| 4 | `bulk-paste-and-clipboard.md` | **Partial** | BulkOperations.razor scenario D (`Clipboard Paste with Type Coercion`) | Scenario D covers the worked example from `bulk-paste-and-clipboard.md:23` including a ready-to-copy TSV block with deliberately invalid cells (`abc`, `invalid-date`). Still missing, per spec: (i) **copy round-trip** (Ctrl+C → Ctrl+V same data) exercising the `data-raw-value` / Format delegate contract from V04.4; (ii) no scenario demonstrating paste **being disabled** when `IsSaving=true` (SA-08) or interaction with deleted rows (V04.3). Flagging as Partial because Wave 1 called these out as demo evidence obligations. |
| 5 | `keyboard-and-accessibility.md` | **Partial** | Keyboard-and-Accessibility.razor scenarios A–E | Scenarios A (navigation), B (F2/Enter/Escape), C (Ctrl+S/C/V/D/Z), D (Space toggles checkbox) and E (AccessibilityInfo) cover most of the printed spec key table. Missing: (i) **Ctrl+A select-all** — demo page has no "select all cells" story because source does not implement it (Wave 1 V07.4 still open); (ii) **Delete key clears selected cells** — listed in `_keyboard[]` array but no interactive scenario; (iii) **roving `tabindex=0` on grid root** (Wave 1 SA-01) is not demonstrable until source is fixed; (iv) scenario C advertises Ctrl+D as "fill the selected range down from the anchor" but source iterates `_selectedRows` (row-level), so the demo text overstates actual behaviour (Wave 1 SA-06). |
| 6 | `bulk-operations-and-saveall.md` | **Partial** | BulkOperations.razor scenarios A, B, C; Editing-and-Validation.razor scenario E; Overview.razor save flow | Scenarios cover AllowAddRow (A), delete/undo toggle (B), bulk select + multi-row delete (C) and ResetAsync (E). **Missing:** (i) **SaveAllAsync error path** — no scenario surfaces a failed save (the 2026-04-10 Priority-2 "Error handling pattern" gap is still open); (ii) **BulkDeleteAsync / BulkResetAsync programmatic method calls** — referenced in the 2026-04-10 audit as Priority-2, still not demoed; (iii) Wave 1 **SA-04** (Reset clears undo buffer) has no demo evidence because source does not actually clear `_undoBuffer` — demo-blocked until source fix. |
| 7 | `columns-and-schema.md` | **Covered** | Overview.razor columns block; BulkOperations / Editing columns | All 6 `DataSheetColumnType` values (Text, Number, Date, Select, Checkbox, Computed) are exercised across the four pages, as are `Required`, `Editable`, `Width`, `Options`, `Format` and `Validate`. Still missing: **`CellTemplate`** (custom cell rendering — Priority-2 in 2026-04-10 audit) and **`MinWidth`**. Marking Covered overall because the missing two are Priority-2/3 nice-to-haves, not core schema behaviours. Track as follow-up rather than Partial. |
| 8 | `virtualization-and-performance.md` | **Partial** | BulkOperations.razor scenario E ("Virtualization Off vs On") | Scenario E renders two side-by-side grids (50 rows / 500 rows) with `EnableVirtualization=false` and `EnableVirtualization=true`, matching the worked example in the 2026-04-10 audit. **Missing:** (i) no scenario at a threshold **above 1,000 rows** despite spec "Recommended Thresholds" table calling for behaviour at 1k/5k/10k rows; (ii) no demonstration of the `OverscanCount` / sticky header behaviour documented in `virtualization-and-performance.md:47-58`; (iii) no demo showing the virtualization-safe `aria-rowindex` contract that `_keyboard[]` table claims is applied; (iv) **Wave 1 SRC-01** (hard-coded 5-row skeleton count) is a source-side gap but means there is no way to demo the viewport-calculated skeleton row story from `virtualization-and-performance.md:77`. Partial until large-N threshold scenario is added. |
| 9 | `theming-and-css-provider.md` | **Missing** | None (all four pages use default theme) | No demo page shows the same grid under two themes, no `IMariloCssProvider` swap, no CellState visual mapping (Dirty / Invalid / Saving / Saved / Pristine) shown side-by-side, no class-provider override scenario. The 2026-04-10 audit already flagged this as Priority-2 ("Theme switching") and deferred it; Wave 1 **SRC-02** (hard-coded BEM classes vs. "all styling delegated to the provider") makes a faithful theming demo **architecturally blocked** — until the orchestrator decides whether to extend `IMariloCssProvider` or narrow the spec, any theming demo would document an unresolved contract. Flagging **Missing + architecture-blocked**; do not attempt without orchestrator sign-off. |

### Counts

| Classification | Count | Topics |
|---|---:|---|
| Covered | 3 | overview, editing-and-validation, columns-and-schema |
| Partial | 4 | bulk-paste-and-clipboard, keyboard-and-accessibility, bulk-operations-and-saveall, virtualization-and-performance |
| Missing | 1 | theming-and-css-provider (architecture-blocked by SRC-02) |
| Blocked-by-source | 1 | selection-and-ranges (blocked by V03 / V07.4) |
| Orphan demos | 0 | — no demo scenario exists without a corresponding spec topic |
| **Total** | **9 topics / 4 demo pages / 16 scenarios** | |

### Top-priority demo gaps (new this wave)

- **EU-01 (Partial → Priority-1):** Add a **>1,000-row virtualization scenario** to BulkOperations.razor scenario E (or a new `virtualization-and-performance.razor` page) with 1k / 5k row toggles. Spec calls out these thresholds; current demo caps at 500. Tied to Wave 1 virtualization topic.
- **EU-02 (Partial → Priority-1):** Add a **copy → paste round-trip scenario** to BulkOperations.razor scenario D or a new scenario F. Exercises Ctrl+C followed by Ctrl+V of the same data to prove the Format / `data-raw-value` round-trip contract from the spec. This is the specific demo evidence Wave 1 asked for on bulk-paste-and-clipboard.
- **EU-03 (Partial → Priority-1):** Add a **paste-blocked-during-save** scenario to BulkOperations.razor — sets `IsSaving=true` on a timer, user attempts Ctrl+V, paste is rejected. Matches Wave 1 SA-08. Tiny footprint, directly proves spec claim at `bulk-paste-and-clipboard.md:67`.
- **EU-04 (Partial → Priority-2):** Add **Delete-key clears selected cells** scenario to Keyboard-and-Accessibility.razor (currently listed in `_keyboard[]` table but not interactive). Small incremental scenario; blocked only by Ctrl+A until V03 lands.
- **EU-05 (Partial → Priority-2):** Add a **SaveAllAsync failure + retry** scenario to BulkOperations.razor or Editing-and-Validation.razor — simulate server error in `HandleSaveAll`, show aria-live "Save failed" announcement (this crosses over with Wave 1 SA-13 missing announcements). Carries over from the 2026-04-10 Priority-2 list.
- **EU-06 (Missing → architecture-blocked):** Theming side-by-side demo. Do not build until orchestrator decides SRC-02 / contract extension. Log as placeholder; escalate rather than scaffold.
- **EU-07 (Blocked-by-source):** Rectangular range-selection demo for selection-and-ranges.md. Wait on V03 implementation. No demo code should be written until the source model exists.
- **EU-08 (Carry from 2026-04-10 audit, Priority-2, still open):** `CellTemplate` custom-rendering scenario — no page demonstrates the `CellTemplate` column parameter. Small footprint, not architecture-blocked. Worker-tractable.

### Cross-reference to Wave 1 spec-review gaps

Demo-evidence obligations the spec-review worker is counting on:

| Wave 1 gap | Topic | Demo obligation | Status this wave |
|---|---|---|---|
| V03 (carried) | selection-and-ranges | Range-selection demo | Blocked-by-source (unchanged) |
| V07.4 (carried) | keyboard-and-accessibility | Ctrl+A select-all | Blocked-by-source (unchanged) |
| SA-01 | keyboard-and-accessibility | Grid root `tabindex=0` focus model | Blocked-by-source (unchanged) |
| SA-04 | bulk-operations-and-saveall | Reset clears undo buffer | Blocked-by-source — EU cannot demo until source fix |
| SA-06 | keyboard-and-accessibility / selection-and-ranges | Ctrl+D fill down (currently overstated in demo text) | **Demo wording drift** — Keyboard-and-Accessibility.razor line 100 says "fill the selected range down from the anchor" but source acts on row selection. Should be softened until V03 lands. Not urgent — source gap is the real fix. |
| SA-08 | bulk-paste-and-clipboard | Paste disabled during save | Demoable now (EU-03) |
| SA-13 | keyboard-and-accessibility / bulk-operations-and-saveall | Start-of-save + error-count aria-live announcements | Not demoable until source adds announcements |
| SRC-01 | virtualization-and-performance | Viewport-calculated skeleton row count | Blocked-by-source |
| SRC-02 | theming-and-css-provider | Any theming demo | Architecture-blocked |

### Escalation candidates for orchestrator

- **EU-06 (theming demo) and SRC-02** together form a single architecture decision. Until the orchestrator decides whether `IMariloCssProvider` grows per-subregion methods (datasheet button / skeleton / badge / editor / sr-only) or the spec is narrowed, no theming demo can be built without front-running a provider-contract change. Worker cannot proceed.
- Drift between **Keyboard-and-Accessibility.razor Ctrl+D description** and actual source behaviour (SA-06). Deciding whether to (a) soften the demo wording now as a doc-only fix or (b) wait for V03 implementation is an orchestrator call — either path is fine, neither is worker-tractable in the example-UX lane.
- **EU-01 large-N virtualization thresholds** — the spec recommends demoing at 1k / 5k / 10k. A 10k-row in-memory dataset in a shared demo project may affect `Marilo.Demo` startup and per-page render cost. Orchestrator should confirm whether `Marilo.Demo` can absorb a 10k-row demo scenario before the worker scaffolds one.

All other EU-0x items (01 non-10k, 02, 03, 04, 05, 08) are worker-tractable
in a later stage-02 implementation pass without architecture or ownership
conflicts.
