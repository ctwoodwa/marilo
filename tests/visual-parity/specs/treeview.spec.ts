import { test } from '@playwright/test';
import { COMPONENTS } from '../config/component-registry';
import { FIRST_PASS_THEMES } from '../config/themes';
import { openComponentPage, locateComponent, scrollToDemoSection, locateDemoPreview } from '../helpers/page-setup';
import { captureElement, captureDemoSection } from '../helpers/capture';
import { type SnapshotDimensions } from '../helpers/snapshot-name';

/**
 * TreeView visual parity — first-pass scenarios.
 *
 * Aligned with CDW treeview-delivery/stages/03-visual-parity/shared/capture-matrix.md
 * which defines 15 state/scenario items. This starter covers P1 and P2 priorities.
 *
 * Scenarios captured:
 *   1. default-hierarchy  — Basic Usage tree with expanded nodes (P1)
 *   2. with-icons         — With Icons section (P2, icon + text alignment)
 *   3. selection          — Selection section (P1, selected node)
 *   4. expand-collapse    — Item Expand / Collapse section (P1)
 *   5. flat-data          — Data-Driven (Flat Data) section (P1, structural baseline)
 *
 * Not yet covered (add in follow-up):
 *   - checkbox states, indeterminate, disabled, load-on-demand, templated, empty
 */

const component = COMPONENTS.treeview;
const viewport = 'desktop';

for (const theme of FIRST_PASS_THEMES) {
  test.describe(`treeview · ${theme.slug}`, () => {
    test.beforeEach(async ({ page }) => {
      await openComponentPage(page, component, theme);
    });

    test('default-hierarchy', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'default-hierarchy',
      };
      // First tree on the page — "Basic Usage" section
      await captureDemoSection(page, 'Basic Usage (ChildContent)', dims);
    });

    test('with-icons', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'with-icons',
      };
      await scrollToDemoSection(page, 'With Icons');
      await captureDemoSection(page, 'With Icons', dims);
    });

    test('selection', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'selection',
      };
      await scrollToDemoSection(page, 'Selection');
      await captureDemoSection(page, 'Selection', dims);
    });

    test('expand-collapse', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'expand-collapse',
      };
      await scrollToDemoSection(page, 'Item Expand / Collapse');
      await captureDemoSection(page, 'Item Expand / Collapse', dims);
    });

    test('flat-data', async ({ page }) => {
      const dims: SnapshotDimensions = {
        component: component.slug,
        theme: theme.slug,
        viewport,
        scenario: 'flat-data',
      };
      await scrollToDemoSection(page, 'Data-Driven (Flat Data)');
      await captureDemoSection(page, 'Data-Driven (Flat Data)', dims);
    });
  });
}
