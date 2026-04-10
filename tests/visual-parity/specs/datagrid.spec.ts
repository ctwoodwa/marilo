import { test } from '@playwright/test';
import { COMPONENTS } from '../config/component-registry';
import { FIRST_PASS_THEMES, type ThemeConfig } from '../config/themes';
import { openComponentPage, locateComponent, scrollToDemoSection, locateDemoPreview } from '../helpers/page-setup';
import { captureElement, captureDemoSection } from '../helpers/capture';
import { type SnapshotDimensions } from '../helpers/snapshot-name';

/**
 * MariloDataGrid visual parity — first-pass scenarios.
 *
 * Aligned with CDW datagrid-delivery/stages/03-visual-parity/shared/capture-matrix.md
 * which defines 18 state/scenario items. This starter covers the P1 and P2
 * priority scenarios from the visual parity plan.
 *
 * Scenarios captured:
 *   1. default          — Basic Usage grid at rest (P1)
 *   2. sorted           — Single-Column Sorting section (P2)
 *   3. selected-row     — Single Selection section (P1)
 *   4. filter-row       — Filter Row section (P2)
 *   5. pager            — Basic Paging section (P2)
 *   6. checkbox-select  — Multiple Selection with Checkboxes (P3)
 *   7. grouped          — Basic Grouping section (P3)
 *
 * Not yet covered (add in follow-up):
 *   - filter-menu, inline-edit, popup-edit, empty, loading, toolbar, virtualization
 */

const component = COMPONENTS.datagrid;
const viewport = 'desktop';

for (const theme of FIRST_PASS_THEMES) {
  test.describe(`datagrid · ${theme.slug}`, () => {
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

    test('sorted', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'sorted',
      };
      // Scroll to "Single-Column Sorting" section and capture
      await scrollToDemoSection(page, 'Single-Column Sorting');
      await captureDemoSection(page, 'Single-Column Sorting', dims);
    });

    test('selected-row', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'selected-row',
      };
      await scrollToDemoSection(page, 'Single Selection');
      await captureDemoSection(page, 'Single Selection', dims);
    });

    test('filter-row', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'filter-row',
      };
      await scrollToDemoSection(page, 'Filter Row');
      await captureDemoSection(page, 'Filter Row', dims);
    });

    test('pager', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'pager',
      };
      await scrollToDemoSection(page, 'Basic Paging');
      await captureDemoSection(page, 'Basic Paging', dims);
    });

    test('checkbox-select', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'checkbox-select',
      };
      await scrollToDemoSection(page, 'Multiple Selection with Checkboxes');
      await captureDemoSection(
        page,
        'Multiple Selection with Checkboxes',
        dims,
      );
    });

    test('grouped', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'grouped',
      };
      await scrollToDemoSection(page, 'Basic Grouping');
      await captureDemoSection(page, 'Basic Grouping', dims);
    });
  });
}
