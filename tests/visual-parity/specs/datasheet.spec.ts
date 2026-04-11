import { test } from '@playwright/test';
import { COMPONENTS } from '../config/component-registry';
import { FIRST_PASS_THEMES, type ThemeConfig } from '../config/themes';
import { openComponentPage, locateComponent } from '../helpers/page-setup';
import { captureElement } from '../helpers/capture';
import { type SnapshotDimensions } from '../helpers/snapshot-name';

/**
 * MariloDataSheet visual parity — Stage 03a primary-state scenarios.
 *
 * Aligned with the Stage 03 plan at
 *   ICM/workspaces/datasheet-delivery/stages/03-visual-parity/output/
 *     datasheet-visual-parity-plan-2026-04-11.md
 * which splits DataSheet visual parity into sub-batches. This spec
 * covers Stage 03a only — the three primary cell-grid states captured
 * against the internal Marilo delivery-quality baseline.
 *
 * Scenarios captured (Stage 03a):
 *   1. default        — Cell grid at rest on the Overview page.
 *   2. selected-cell  — Single data cell focused (active) by click.
 *   3. cell-editing   — Active cell promoted to inline edit mode (F2).
 *
 * Out of scope (later Stage 03 sub-batches):
 *   - 03b: header cells, frozen/range/dirty/invalid/empty secondary states
 *   - 03c: scoring against the baseline
 *   - 03d: parity handoff artifacts
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
  });
}
