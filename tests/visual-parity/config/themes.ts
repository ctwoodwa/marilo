/**
 * Theme and mode definitions for visual parity capture.
 *
 * Matches the CDW 03-visual-parity theme/mode matrix:
 *   Fluent (light/dark), Bootstrap (light/dark), Material (light/dark)
 *
 * The provider-switcher JS module in the demo app uses localStorage keys:
 *   marilo:provider  — "fluentui" | "bootstrap" | "material"
 *   marilo:darkmode  — "0" | "1"
 *
 * Provider switching triggers location.reload(), so theme changes
 * require a full page navigation cycle.
 */

export type ThemeProvider = 'fluentui' | 'bootstrap' | 'material';
export type ThemeMode = 'light' | 'dark';

export interface ThemeConfig {
  provider: ThemeProvider;
  mode: ThemeMode;
  /** Slug used in snapshot naming: "fluent-light", "bootstrap-dark", etc. */
  slug: string;
}

/** All six theme/mode combinations from the CDW capture matrix. */
export const ALL_THEMES: ThemeConfig[] = [
  { provider: 'fluentui', mode: 'light', slug: 'fluent-light' },
  { provider: 'fluentui', mode: 'dark', slug: 'fluent-dark' },
  { provider: 'bootstrap', mode: 'light', slug: 'bootstrap-light' },
  { provider: 'bootstrap', mode: 'dark', slug: 'bootstrap-dark' },
  { provider: 'material', mode: 'light', slug: 'material-light' },
  { provider: 'material', mode: 'dark', slug: 'material-dark' },
];

/**
 * First-pass review order from CDW visual parity plans.
 * Start with Fluent Light (most mature) and work outward.
 */
export const FIRST_PASS_THEMES: ThemeConfig[] = [
  ALL_THEMES[0], // fluent-light
  ALL_THEMES[1], // fluent-dark
];

/** Default viewport for desktop capture. */
export const DESKTOP_VIEWPORT = { width: 1280, height: 900 };

/** Narrow viewport for responsive/overflow scenarios. */
export const NARROW_VIEWPORT = { width: 768, height: 900 };
