import { defineConfig, devices } from '@playwright/test';

/**
 * Playwright configuration for Marilo visual parity tests.
 *
 * Design decisions:
 * - Chromium only: Playwright warns that screenshots differ across browsers
 *   and platforms. We start with a single stable browser to avoid false diffs.
 * - Desktop viewport (1280x900): Matches the CDW capture matrix "Desktop"
 *   primary review viewport.
 * - Animations disabled: Reduces screenshot flakiness from CSS transitions.
 * - snapshotPathTemplate: Organizes baselines by component/theme-mode/viewport
 *   so they're human-browsable and stable as scenarios are added.
 * - Conservative timeouts: Blazor Server apps need time for SignalR connection
 *   and initial render.
 */
export default defineConfig({
  testDir: './specs',
  outputDir: './test-results',

  /* Snapshot storage: {component}/{theme-mode}/{viewport}/{scenario}.png */
  snapshotPathTemplate:
    '{snapshotDir}/{arg}{ext}',

  /* Baselines stored alongside specs in a __screenshots__ folder */
  snapshotDir: './baselines',

  /* Fail fast during first-pass capture; relax later if needed */
  fullyParallel: false,
  retries: 0,
  workers: 1,

  /* Reporter — list to stdout, no HTML server that can hold a port */
  reporter: 'list',

  use: {
    /*
     * Demo app URL — the dev server uses HTTPS with a self-signed cert.
     * Override with MARILO_DEMO_URL env var if running on a different port.
     */
    baseURL: process.env.MARILO_DEMO_URL || 'https://localhost:5301',
    ignoreHTTPSErrors: true,

    /* Screenshot comparison settings */
    screenshot: 'off', // We use explicit toHaveScreenshot() calls
    trace: 'retain-on-failure',

    /* Reduce animation noise */
    actionTimeout: 10_000,
    navigationTimeout: 30_000,
  },

  projects: [
    {
      name: 'visual-parity-desktop',
      use: {
        ...devices['Desktop Chrome'],
        viewport: { width: 1280, height: 900 },
      },
    },
    // Future: uncomment to add narrow viewport project
    // {
    //   name: 'visual-parity-narrow',
    //   use: {
    //     ...devices['Desktop Chrome'],
    //     viewport: { width: 768, height: 900 },
    //   },
    // },
  ],

  /*
   * Start the Marilo demo app if not already running.
   * The first `dotnet run` compiles the full solution, which can take 2+ minutes.
   * If the app is already running on :5301, reuseExistingServer skips the build.
   */
  webServer: {
    command: 'dotnet run --project ../../samples/Marilo.Demo',
    url: 'https://localhost:5301',
    ignoreHTTPSErrors: true,
    reuseExistingServer: true,
    timeout: 180_000,
    stdout: 'ignore',
    stderr: 'pipe',
  },
});
