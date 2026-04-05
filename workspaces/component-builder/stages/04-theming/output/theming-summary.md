# Theming Summary: MariloAllocationScheduler

## Provider Implementations

### FluentUI Provider (FluentUICssProvider.cs)

14 methods added, all using `CssClassBuilder` with `mar-allocation-scheduler` BEM prefix:

| Method | Classes returned |
|---|---|
| AllocationSchedulerClass | `mar-allocation-scheduler` |
| AllocationSchedulerToolbarClass | `mar-allocation-scheduler__toolbar` |
| AllocationSchedulerResourceColumnClass | `mar-allocation-scheduler__resource-col` `--pinned` |
| AllocationSchedulerTimeHeaderClass | `mar-allocation-scheduler__time-header` `--[grain]` |
| AllocationSchedulerRowClass | `mar-allocation-scheduler__row` `--selected` `--over-allocated` |
| AllocationSchedulerCellClass | `mar-allocation-scheduler__cell` `--editable` `--selected` `--conflict` `--disabled` `--drag-target` |
| AllocationSchedulerCellValueClass | `mar-allocation-scheduler__cell-value` `--hours`/`--currency` |
| AllocationSchedulerDeltaClass | `mar-allocation-scheduler__delta` `--over`/`--under` |
| AllocationSchedulerScenarioStripClass | `mar-allocation-scheduler__scenario-strip` |
| AllocationSchedulerScenarioChipClass | `mar-allocation-scheduler__scenario-chip` `--active` `--locked` |
| AllocationSchedulerGhostBarClass | `mar-allocation-scheduler__ghost-bar` |
| AllocationSchedulerContextMenuClass | `mar-allocation-scheduler__context-menu` |
| AllocationSchedulerEmptyClass | `mar-allocation-scheduler__empty` |
| AllocationSchedulerLoaderClass | `mar-allocation-scheduler__loader` |

### Bootstrap Provider (BootstrapCssProvider.cs)

14 methods added, mapping to Bootstrap utilities with `mar-bs-allocation-scheduler` bridge classes:

| Method | Bootstrap classes used |
|---|---|
| AllocationSchedulerClass | `table-responsive mar-bs-allocation-scheduler` |
| AllocationSchedulerToolbarClass | `d-flex gap-2 mb-2` |
| AllocationSchedulerRowClass | `table-active` (selected), `table-danger` (over-allocated) |
| AllocationSchedulerCellClass | `table-primary` (selected), `table-danger` (conflict), `text-muted bg-light` (disabled) |
| AllocationSchedulerDeltaClass | `text-danger` (over), `text-warning` (under), `small` |
| AllocationSchedulerScenarioChipClass | `badge rounded-pill bg-primary`/`bg-secondary` |
| AllocationSchedulerGhostBarClass | `text-muted opacity-50` |
| AllocationSchedulerEmptyClass | `text-center text-muted p-4` |

## SCSS Files Created

| File | Path | Lines |
|---|---|---|
| FluentUI SCSS | `src/Marilo.Providers.FluentUI/Styles/_allocation-scheduler.scss` | ~200 |
| Bootstrap SCSS | `src/Marilo.Providers.Bootstrap/Styles/_bridge-allocation-scheduler.scss` | ~120 |

## SCSS Imports Updated

| File | Change |
|---|---|
| `src/Marilo.Providers.FluentUI/Styles/marilo-fluentui.scss` | Added `@forward 'allocation-scheduler'` |
| `src/Marilo.Providers.Bootstrap/Styles/marilo-bootstrap.scss` | Added `@import "bridge-allocation-scheduler"` |

## Design Token Usage (FluentUI)

| Token | Usage |
|---|---|
| `--marilo-color-border` | Cell borders, row separators, scenario chip borders |
| `--marilo-color-primary` | Selected cell highlight, active scenario chip, drag target |
| `--marilo-color-danger` | Over-allocated rows, conflict cells, over-delta indicator |
| `--marilo-color-warning` | Under-delta indicator |
| `--marilo-color-success` | Currency value text color |
| `--marilo-color-surface` | Cell background, context menu background |
| `--marilo-color-subtle-background` | Toolbar, scenario strip, header row backgrounds |
| `--marilo-color-disabled-background` | Disabled cell background |
| `--marilo-color-disabled-text` | Disabled cell text, empty state text, ghost bar text |

## Audit Checks

| Check | Status |
|---|---|
| Method parity | PASS -- both providers implement all 14 method signatures |
| CSS class coverage | PASS -- all states (selected, conflict, disabled, etc.) have corresponding rules |
| Token usage | PASS -- FluentUI uses design tokens, no hardcoded colors |
| Bridge correctness | PASS -- Bootstrap maps to native utilities where available |
| SCSS imports | PASS -- both main entry points updated |
