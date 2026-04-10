import { test } from '@playwright/test';
import { COMPONENTS } from '../config/component-registry';
import { FIRST_PASS_THEMES } from '../config/themes';
import { openComponentPage, locateComponent } from '../helpers/page-setup';
import { captureElement, captureDemoSection } from '../helpers/capture';
import { type SnapshotDimensions } from '../helpers/snapshot-name';

/**
 * MariloScheduler visual parity — first-pass scenarios.
 *
 * Aligned with CDW scheduler-delivery/stages/03-visual-parity/shared/capture-matrix.md
 * which defines 17 state/scenario items. This starter covers P1 priorities.
 *
 * The Scheduler demo page currently has a single "Basic Usage" section
 * with a default scheduler instance. Additional scenarios will require
 * either expanded demo sections or programmatic state manipulation.
 *
 * Scenarios captured:
 *   1. default — Basic Usage scheduler at rest (P1)
 *
 * Not yet covered (add when demo scenarios expand):
 *   - day-view, week-view, month-view, timeline-view
 *   - appointment-hover, appointment-selected, overlapping
 *   - popup-editor, drag-preview, resize-affordance
 *   - current-time-indicator, view-switcher, empty-timeslot
 *
 * NOTE: The Scheduler demo page is minimal (single section). As the
 * demo expands with more scenarios, add corresponding test cases here.
 */

const component = COMPONENTS.scheduler;
const viewport = 'desktop';

for (const theme of FIRST_PASS_THEMES) {
  test.describe(`scheduler · ${theme.slug}`, () => {
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
      // The scheduler demo currently has one section — capture the component
      const scheduler = locateComponent(page, component);
      // If the component doesn't match, fall back to the demo section
      if ((await scheduler.count()) > 0) {
        await captureElement(scheduler, dims);
      } else {
        await captureDemoSection(page, 'Basic Usage', dims);
      }
    });
  });
}
