# DataSheet Parity Summary

Wave 3 (03-visual-parity) summary for `MariloDataSheet` produced by
`w-datasheet-delivery` under orchestrator session
`marilo-grid-pipeline-2026-04-11-1200`.

**Method:** Static analysis only — no browser capture. Sources reviewed:
`src/Marilo.Components/DataGrid/MariloDataSheet*`, all three provider
`FluentUICssProvider.cs` / `MaterialCssProvider.cs` / `BootstrapCssProvider.cs`
files, each provider's `Styles/components/*.scss` tree, the four DataSheet
demo pages, and `docs/component-specs/datasheet/theming-and-css-provider.md`.
Build verification: `dotnet build Marilo.slnx` exit **0** (0 warnings, 0
errors) at 2026-04-11T17:40Z (this turn).

## Headline finding

**All `mar-datasheet*` BEM classes are emitted by all three CSS providers but
none are defined in any provider SCSS.** Razor/RenderTreeBuilder hard-codes
these class names (`MariloDataSheet.razor` 14 hits; `MariloDataSheet.Rendering.cs`
9 hits) and the providers implement `DataSheet*Class()` methods in their C#
files (`FluentUICssProvider.cs:544-588`), but no
`_data-sheet.scss` or `_bridge-data-sheet.scss` exists in any provider's
`Styles/` tree. `ripgrep` confirms: 0 hits for `mar-datasheet` in
`src/Marilo.Providers.FluentUI/Styles`, 0 in `src/Marilo.Providers.Bootstrap/Styles`,
0 in `src/Marilo.Providers.Material/Styles`.

The practical consequence: DataSheet currently renders as a browser-default
`<table>` in every theme × mode × state combination. No grid lines, no header
fill, no cell-state tinting, no row hover/selection, no frozen-column
separator, no focus treatment, no dark-mode overrides, no density control.
This single structural gap is the root of every VP record except VP-12 (which
also depends on Wave 1 SA-01 on the source side).

## Parity score matrix

State/scenario scored (0–3) against the internal Marilo delivery-quality
baseline. **D** = Deferred (classification below). Dashes are states that
collapse into their umbrella record.

| State | Fluent L | Fluent D | Bootstrap L | Bootstrap D | Material L | Material D |
|---|---|---|---|---|---|---|
| 1. Cell grid default | 0 | 0 | 0 | 0 | 0 | 0 |
| 2. Header row | 0 | 0 | 0 | 0 | 0 | 0 |
| 3. Row hover | 0 | 0 | 0 | 0 | 0 | 0 |
| 4. Cell selection (single) | 0 | 0 | 0 | 0 | 0 | 0 |
| 5. Row selection | 0 | 0 | 0 | 0 | 0 | 0 |
| 6. Focused cell | 1 | 1 | 1 | 1 | 1 | 1 |
| 7. Editable cell | 0 | 0 | 0 | 0 | 0 | 0 |
| 8. Validation error | 1 | 1 | 1 | 1 | 1 | 1 |
| 9. Frozen column | 0 | 0 | 0 | 0 | 0 | 0 |
| 6b. Theming side-by-side | D | D | D | D | D | D |
| 5b. Rectangular range sel. | D | D | D | D | D | D |
| 1b. Virtualization @10k | D | D | D | D | D | D |

Rows 6b / 5b / 1b are the deferrals (VP-datasheet-D01 / D02 / D03 in the gap
list); they are valid output classifications per inbox instruction and are
**not** scoring failures.

### Aggregate

| Provider | Average score (scored states only) | Critical gap count |
|---|---:|---:|
| Fluent Light | 0.22 | 6 |
| Fluent Dark | 0.22 | 6 |
| Bootstrap Light | 0.22 | 5 |
| Bootstrap Dark | 0.22 | 5 |
| Material Light | 0.22 | 5 |
| Material Dark | 0.22 | 5 |

All themes score identically because the gap is structural (no SCSS exists).
Remediation work is parallelizable by provider once the
`datasheet-theming-architecture` contract decision lands.

## Severity rollup (per shared/parity-score-rubric.md)

- **Critical:** 8 gap records — rows 1, 2, 4, 5, 7 on primary states in any
  theme, plus VP-datasheet-01 (umbrella structural gap) and VP-datasheet-06
  (validation error, which scored 1 on a primary state in every cell).
- **Major:** 3 gap records — row hover (VP-08), focused cell (VP-12), frozen
  column (VP-07).
- **Minor:** 0.
- **Polish:** 0.
- **Deferred:** 3 — D01 (theming), D02 (range selection), D03 (10k rows).

## Deferrals (must remain deferred — inbox constraint)

| Deferral ID | State | Classification | Blocker | Source |
|---|---|---|---|---|
| VP-datasheet-D01 | Theming side-by-side (EU-06) | `DEFERRED-PENDING-ARCHITECTURE` | `datasheet-theming-architecture` user-decision OPEN | Wave 2 EU-06; Wave 1 SRC-02 |
| VP-datasheet-D02 | Rectangular range selection (EU-07) | `DEFERRED-PENDING-SOURCE` | No `DataSheetSelection<TItem>` source model | Wave 1 V03 |
| VP-datasheet-D03 | 10k-row virtualization | `DEFERRED-PENDING-SCOPE` | `datasheet-10k-rows` user-decision OPEN | Wave 2 EU-01 upper threshold |

These three deferrals are the **correct output** for their scenarios under
wave 3 constraints and are not re-escalated per inbox instruction.

## Primary remediation lanes (for 04-sync-check + gap-analysis intake)

1. **`_data-sheet.scss` foundation lane (umbrella VP-01)** — author the file
   in each provider, wire into `_index.scss`, define grid container, header,
   cells, borders, density. Blocker: `datasheet-theming-architecture`
   decision. Parallelizes by provider once the contract is set.
2. **Fluent selection / editor / validation lane (VP-04, VP-05, VP-06, VP-09)**
   — selector-level work once (1) lands. No additional blockers beyond (1).
3. **Bootstrap bridge lane (VP-10)** — `_bridge-data-sheet.scss` modeled on
   `_bridge-data.scss`. Dark mode uses `[data-bs-theme="dark"]`.
4. **Material provider lane (VP-11)** — depends on Material runtime provider
   implementation status in addition to (1).
5. **Frozen column dual lane (VP-07)** — source-side freeze model + SCSS
   separator. Source half is a gap-analysis intake item.
6. **Focused-cell / SA-01 waiting lane (VP-12)** — blocked on Wave 1 SA-01
   until grid root `tabindex=0` and roving-tabindex are implemented.

## Known blockers carried into 04-sync-check

- `datasheet-theming-architecture` (OPEN) — gates every VP-datasheet-01 through
  VP-datasheet-11 remediation lane.
- Wave 1 SA-01 — source-side focus root, gates VP-datasheet-12.
- Wave 1 V03 — range selection source model, gates VP-datasheet-D02.
- `datasheet-10k-rows` (OPEN) — gates VP-datasheet-D03.
- Material runtime provider status — secondary gate on VP-datasheet-11.

None are re-escalated by this wave (already surfaced upstream).

## Ready-to-read files

- `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-visual-parity-gaps.md` — all 12 gap records + 3 deferrals
- `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-parity-summary.md` — this file
- `ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/datasheet-visual-parity-plan.md` — plan seeded 2026-04-10, updated this turn with wave 3 findings-informed execution notes
