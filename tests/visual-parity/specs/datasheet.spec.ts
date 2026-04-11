import { test } from '@playwright/test';
import { COMPONENTS } from '../config/component-registry';
import { FIRST_PASS_THEMES, type ThemeConfig } from '../config/themes';
import { applyTheme, openComponentPage, locateComponent, locateDemoPreview, scrollToDemoSection } from '../helpers/page-setup';
import { captureElement } from '../helpers/capture';
import { type SnapshotDimensions } from '../helpers/snapshot-name';

/**
 * MariloDataSheet visual parity — Stage 03a + 03b scenarios.
 *
 * Aligned with the Stage 03 plan at
 *   ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/
 *     datasheet-visual-parity-plan-2026-04-11.md
 * which splits DataSheet visual parity into sub-batches. This spec
 * covers Stage 03a (primary cell-grid states) and Stage 03b (secondary
 * states) against the internal Marilo delivery-quality baseline.
 *
 * Scenarios captured:
 *   Stage 03a — primary states
 *     1. default        — Cell grid at rest on the Overview page.
 *     2. selected-cell  — Single data cell focused (active) by click.
 *     3. cell-editing   — Active cell promoted to inline edit mode (F2).
 *   Stage 03b — secondary states
 *     4. header-cells   — Column header row at rest (shares the default
 *                         grid container; the header region is covered
 *                         by the grid-level capture since the header is
 *                         always rendered inside `.mar-datasheet`).
 *     5. dirty-row      — Row in its post-commit dirty state after an
 *                         inline edit is committed with Enter.
 *     6. invalid-cell   — Cell carrying a validation error after a
 *                         commit that violates the column-level
 *                         `Validate` callback.
 *     7. empty-state    — Grid rendering its `EmptyStateMessage` on
 *                         the Editing-and-Validation scenario D demo
 *                         (IsLoading skeleton grid starts empty).
 *
 * Out of scope (tracked in the Stage 03 plan, iteration 15 resolutions):
 *   - Frozen rows/columns (NOT IMPLEMENTED in DataSheet)
 *   - Cell range selection (V03 feature, deferred)
 *   - Formula bar / sheet tabs (NOT IMPLEMENTED)
 *   - Theme matrix expansion beyond FIRST_PASS_THEMES
 *   - 03c scoring against the baseline
 *   - 03d parity handoff artifacts
 */

const component = COMPONENTS.datasheet;
const viewport = 'desktop';

for (const theme of FIRST_PASS_THEMES) {
  test.describe(`datasheet · ${theme.slug}`, () => {
    test.beforeEach(async ({ page }) => {
      await openComponentPage(page, component, theme);
    });

    test('default', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'default',
      };
      const grid = locateComponent(page, component);
      await captureElement(grid, dims);
    });

    test('selected-cell', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'selected-cell',
      };
      const grid = locateComponent(page, component);

      // Focus a single data cell by clicking it. The MariloDataSheet
      // click handler calls ActivateCell on the first click, which
      // applies `mar-datasheet__cell--active` to the target cell but
      // does not enter edit mode. We target the Ticker cell on the
      // second data row (index 1 among `.mar-datasheet__cell` nodes
      // for that row) — stable and unambiguous on the Overview demo.
      const secondRowTickerCell = grid
        .locator('.mar-datasheet__row')
        .nth(1)
        .locator('.mar-datasheet__cell')
        .first();
      await secondRowTickerCell.click();
      await page.waitForTimeout(200);

      await captureElement(grid, dims);
    });

    test('cell-editing', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'cell-editing',
      };
      const grid = locateComponent(page, component);

      // Activate the same Ticker cell, then press F2 to promote it
      // into inline edit mode. The MariloDataSheet keyboard handler
      // maps F2 to EnterEditMode for the active cell.
      const secondRowTickerCell = grid
        .locator('.mar-datasheet__row')
        .nth(1)
        .locator('.mar-datasheet__cell')
        .first();
      await secondRowTickerCell.click();
      await page.waitForTimeout(150);
      await page.keyboard.press('F2');
      await page.waitForTimeout(200);

      await captureElement(grid, dims);
    });

    test('header-cells', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'header-cells',
      };
      const grid = locateComponent(page, component);

      // The Overview grid always renders its header row
      // (Ticker / Asset Class / Qty / Price / Trade Date / Hedge? /
      // Mkt Value) inside `.mar-datasheet__header-cell` nodes at the
      // top of the grid container. Wait for at least one header cell
      // to be attached so the capture is not racing initial render,
      // then capture the full grid — the header row is the top band
      // of that capture and is what gets compared across themes.
      await grid.locator('.mar-datasheet__header-cell').first().waitFor();
      await captureElement(grid, dims);
    });

    test('dirty-row', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'dirty-row',
      };
      const grid = locateComponent(page, component);

      // Edit the Ticker cell on the second data row and commit with
      // Enter. CommitCellEdit adds the field to DirtyFields and, since
      // the new value differs from the original, the row transitions
      // to CellState.Dirty — FluentUICssProvider then emits
      // `mar-datasheet__row--dirty` on the row and
      // `mar-datasheet__cell--dirty` on the committed cell.
      const secondRowTickerCell = grid
        .locator('.mar-datasheet__row')
        .nth(1)
        .locator('.mar-datasheet__cell')
        .first();
      await secondRowTickerCell.click();
      await page.waitForTimeout(150);
      await page.keyboard.press('F2');
      await page.waitForTimeout(150);

      // `fill` replaces the existing editor value so the commit is
      // guaranteed to differ from the original and mark the row dirty.
      const editor = grid.locator('.mar-datasheet__editor-input').first();
      await editor.fill('ZZZZ');
      await page.keyboard.press('Enter');

      // Wait for the dirty class to actually apply before capturing —
      // StateHasChanged runs inside CommitCellEdit so this is fast,
      // but we still wait deterministically.
      await grid.locator('.mar-datasheet__row--dirty').first().waitFor();
      await page.waitForTimeout(150);

      await captureElement(grid, dims);
    });

    test('invalid-cell', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'invalid-cell',
      };
      const grid = locateComponent(page, component);

      // The Quantity column on the Overview demo carries a column-level
      // Validate callback: `r => r.Quantity < 0 ? "Must be >= 0" : null`.
      // CommitCellEdit runs RunColumnValidation immediately on commit, so
      // a negative value surfaces CellState.Invalid without needing the
      // user to click Save All. That emits `mar-datasheet__cell--invalid`
      // plus `aria-invalid="true"` on the cell, which is what we capture.
      //
      // Quantity is the 3rd column (index 2 among .mar-datasheet__cell
      // nodes on a data row: Ticker, AssetClass, Quantity, ...).
      const secondRowQtyCell = grid
        .locator('.mar-datasheet__row')
        .nth(1)
        .locator('.mar-datasheet__cell')
        .nth(2);
      await secondRowQtyCell.click();
      await page.waitForTimeout(150);
      await page.keyboard.press('F2');
      await page.waitForTimeout(150);

      const editor = grid.locator('.mar-datasheet__editor-input').first();
      await editor.fill('-100');
      await page.keyboard.press('Enter');

      // Wait for the invalid class to attach to the cell before capturing.
      await grid.locator('.mar-datasheet__cell--invalid').first().waitFor();
      await page.waitForTimeout(150);

      await captureElement(grid, dims);
    });

    test('empty-state', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'empty-state',
      };

      // The Overview demo boots with 12 rows, so it never renders the
      // empty state. Scenario D of the Editing-and-Validation demo wires
      // a separate MariloDataSheet with `_loadingRows` initialized to an
      // empty list and `EmptyStateMessage="No data yet. Click Load Data
      // to fetch."` — before the user clicks Load Data, that grid emits
      // the `.mar-datasheet__empty` cell we want to baseline.
      //
      // We re-use `applyTheme` to navigate to the alternate route with
      // the same theme already applied (matching the beforeEach pattern)
      // and then scope the capture to the scenario-D demo section via
      // `locateDemoPreview` so other MariloDataSheet instances on the
      // page don't bleed into the snapshot.
      await applyTheme(
        page,
        theme,
        '/components/DataSheet/editing-and-validation',
      );
      await scrollToDemoSection(page, 'IsLoading skeleton rows');

      const preview = locateDemoPreview(page, 'IsLoading skeleton rows');
      const emptyGrid = preview.locator('.mar-datasheet').first();
      await emptyGrid.locator('.mar-datasheet__empty').waitFor();
      await captureElement(emptyGrid, dims);
    });
  });
}
