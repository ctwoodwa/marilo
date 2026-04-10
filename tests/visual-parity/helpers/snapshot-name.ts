/**
 * Snapshot naming convention for Marilo visual parity tests.
 *
 * Convention:
 *   {component}/{theme-mode}/{viewport}/{scenario}.png
 *
 * Examples:
 *   datagrid/fluent-light/desktop/default.png
 *   datagrid/fluent-dark/desktop/sorted-ascending.png
 *   treeview/bootstrap-light/desktop/expanded.png
 *   scheduler/fluent-light/desktop/week-view.png
 *
 * This produces a browsable folder tree under baselines/:
 *   baselines/
 *     datagrid/
 *       fluent-light/
 *         desktop/
 *           default.png
 *           sorted-ascending.png
 *       fluent-dark/
 *         desktop/
 *           default.png
 *     treeview/
 *       ...
 *
 * All segments are lowercase-hyphenated. No spaces, no camelCase.
 */

export interface SnapshotDimensions {
  component: string;
  theme: string;   // e.g., "fluent-light", "bootstrap-dark"
  viewport: string; // e.g., "desktop", "narrow"
  scenario: string; // e.g., "default", "sorted-ascending", "filter-open"
}

/**
 * Build a snapshot name array for Playwright's toHaveScreenshot().
 *
 * Playwright's `name` parameter accepts a string array that maps to
 * path segments, which integrates with snapshotPathTemplate to produce
 * the final file path.
 *
 * @example
 *   await expect(locator).toHaveScreenshot(
 *     snapshotName({ component: 'datagrid', theme: 'fluent-light', viewport: 'desktop', scenario: 'default' })
 *   );
 *   // Produces: baselines/datagrid/fluent-light/desktop/default.png
 */
export function snapshotName(dims: SnapshotDimensions): string[] {
  return [
    dims.component,
    dims.theme,
    dims.viewport,
    `${dims.scenario}.png`,
  ];
}

/**
 * Build a human-readable test title from dimensions.
 * Used in test.describe/test() blocks for consistent naming.
 *
 * @example
 *   testTitle({ component: 'datagrid', theme: 'fluent-light', viewport: 'desktop', scenario: 'default' })
 *   // => "datagrid · fluent-light · desktop · default"
 */
export function testTitle(dims: SnapshotDimensions): string {
  return `${dims.component} · ${dims.theme} · ${dims.viewport} · ${dims.scenario}`;
}
