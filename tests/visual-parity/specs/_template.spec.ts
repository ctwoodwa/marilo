/**
 * TEMPLATE: Visual parity spec for a Marilo component.
 *
 * Copy this file to create a new component's visual parity spec:
 *   1. Copy to specs/<component-slug>.spec.ts
 *   2. Update COMPONENT_KEY to match the key in component-registry.ts
 *   3. Update SCENARIOS to match the CDW capture matrix for this component
 *   4. Adjust section titles to match the actual demo page sections
 *
 * Naming convention (enforced by snapshot-name.ts):
 *   baselines/{component}/{theme-mode}/{viewport}/{scenario}.png
 *
 * Each scenario should map to a row in the CDW capture matrix:
 *   CDW: stages/03-visual-parity/shared/capture-matrix.md
 */

import { test } from '@playwright/test';
import { COMPONENTS } from '../config/component-registry';
import { FIRST_PASS_THEMES } from '../config/themes';
import {
  openComponentPage,
  locateComponent,
  scrollToDemoSection,
} from '../helpers/page-setup';
import { captureElement, captureDemoSection } from '../helpers/capture';
import { type SnapshotDimensions } from '../helpers/snapshot-name';

// ── Configuration ────────────────────────────────────────────────────
// Change these for your component:

const COMPONENT_KEY = 'splitter'; // Must match a key in component-registry.ts

/**
 * Define scenarios to capture. Each entry maps to:
 * - A scenario slug for snapshot naming
 * - A demo section title on the overview page (or null for first-instance capture)
 */
const SCENARIOS: Array<{
  scenario: string;
  demoSection: string | null;
  priority: 'P1' | 'P2' | 'P3' | 'P4';
}> = [
  { scenario: 'default', demoSection: null, priority: 'P1' },
  // Add more scenarios from the CDW capture matrix:
  // { scenario: 'hover', demoSection: 'Hover State', priority: 'P1' },
  // { scenario: 'selected', demoSection: 'Selection', priority: 'P1' },
];

// ── Test Generation ──────────────────────────────────────────────────
// No changes needed below this line for typical use.

const component = COMPONENTS[COMPONENT_KEY];
const viewport = 'desktop';

for (const theme of FIRST_PASS_THEMES) {
  test.describe(`${component.slug} · ${theme.slug}`, () => {
    test.beforeEach(async ({ page }) => {
      await openComponentPage(page, component, theme);
    });

    for (const { scenario, demoSection } of SCENARIOS) {
      test(scenario, async ({ page }) => {
        const dims: SnapshotDimensions = {
          component: component.slug,
          theme: theme.slug,
          viewport,
          scenario,
        };

        if (demoSection) {
          // Capture a specific demo section by title
          await scrollToDemoSection(page, demoSection);
          await captureDemoSection(page, demoSection, dims);
        } else {
          // Capture the first instance of the component on the page
          const el = locateComponent(page, component);
          await captureElement(el, dims);
        }
      });
    }
  });
}
