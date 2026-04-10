import { expect, type Locator, type Page } from '@playwright/test';
import { type SnapshotDimensions, snapshotName } from './snapshot-name';
import { toAnchorId, cssEscape } from './page-setup';

/**
 * Default screenshot options for visual parity captures.
 * Animations are disabled and the caret is hidden to reduce flakiness.
 */
const DEFAULT_SCREENSHOT_OPTIONS = {
  animations: 'disabled' as const,
  caret: 'hide' as const,
  // Allow 0.5% pixel difference to absorb subpixel rendering variance.
  // Tighten this as baselines stabilize.
  maxDiffPixelRatio: 0.005,
};

/**
 * Capture an element-level screenshot and assert against baseline.
 *
 * Preferred method: captures only the component container, excluding
 * surrounding demo chrome.
 *
 * @param locator - Playwright locator for the component element
 * @param dims - Snapshot naming dimensions
 * @param options - Additional screenshot options
 */
export async function captureElement(
  locator: Locator,
  dims: SnapshotDimensions,
  options?: { maxDiffPixelRatio?: number; threshold?: number },
): Promise<void> {
  await expect(locator).toHaveScreenshot(snapshotName(dims), {
    ...DEFAULT_SCREENSHOT_OPTIONS,
    ...options,
  });
}

/**
 * Capture a page-level screenshot and assert against baseline.
 *
 * Use only when the component cannot be meaningfully isolated
 * (e.g., full-page layout components like DockManager).
 *
 * @param page - Playwright page
 * @param dims - Snapshot naming dimensions
 * @param options - Additional screenshot options
 */
export async function capturePage(
  page: Page,
  dims: SnapshotDimensions,
  options?: {
    maxDiffPixelRatio?: number;
    fullPage?: boolean;
    clip?: { x: number; y: number; width: number; height: number };
  },
): Promise<void> {
  await expect(page).toHaveScreenshot(snapshotName(dims), {
    ...DEFAULT_SCREENSHOT_OPTIONS,
    ...options,
  });
}

/**
 * Capture a demo section's preview area.
 * Scrolls the section into view, waits for stability, then captures.
 *
 * @param page - Playwright page
 * @param sectionTitle - Title of the DemoSection
 * @param dims - Snapshot naming dimensions
 */
export async function captureDemoSection(
  page: Page,
  sectionTitle: string,
  dims: SnapshotDimensions,
): Promise<void> {
  const anchorId = toAnchorId(sectionTitle);

  const preview = page.locator(`#${cssEscape(anchorId)} .demo-panel-preview`);
  await preview.scrollIntoViewIfNeeded();
  await page.waitForTimeout(300);

  await expect(preview).toHaveScreenshot(snapshotName(dims), {
    ...DEFAULT_SCREENSHOT_OPTIONS,
  });
}
