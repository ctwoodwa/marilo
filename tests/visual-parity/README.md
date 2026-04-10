# Visual Parity Test Harness

Playwright-based screenshot capture and comparison for Marilo components. Produces structured baselines organized by component, theme, mode, viewport, and scenario.

## Purpose

This harness supports **CDW Stage 03 — Visual Parity**. Each CDW workspace defines a capture matrix (states, themes, modes) and a parity rubric. This harness automates the screenshot capture side of that process.

**CDW stage files are the planning layer. Playwright files are the execution layer.**

The CDW defines *what* to capture. The Playwright specs define *how* to capture it.

## Relationship to CDW 03-visual-parity

```
ICM/workspaces/<component>-delivery/
  stages/03-visual-parity/
    CONTEXT.md                    ← When to enter, process, outputs
    shared/
      capture-matrix.md           ← What states/themes/modes to capture
      parity-score-rubric.md      ← How to score 0-3
      visual-parity-gap-format.md ← How to record gaps
      claude-remediation-template.md ← How to hand off fixes
    output/
      *-visual-parity-plan.md     ← Starter plan with priorities

tests/visual-parity/
  config/
    component-registry.ts         ← Routes, selectors, reference strategy
    themes.ts                     ← Theme/mode definitions
  helpers/
    page-setup.ts                 ← Navigate, apply theme, wait for Blazor
    capture.ts                    ← Screenshot assertion wrappers
    snapshot-name.ts              ← Naming convention builder
  specs/
    datagrid.spec.ts              ← DataGrid first-pass scenarios
    treeview.spec.ts              ← TreeView first-pass scenarios
    scheduler.spec.ts             ← Scheduler first-pass scenarios
    _template.spec.ts             ← Copy this to add a new component
  baselines/                      ← Generated on first run (gitignored initially)
```

## Screenshot Naming Convention

All snapshots follow this structure:

```
baselines/{component}/{theme-mode}/{viewport}/{scenario}.png
```

Examples:
```
baselines/datagrid/fluent-light/desktop/default.png
baselines/datagrid/fluent-dark/desktop/sorted.png
baselines/treeview/bootstrap-light/desktop/with-icons.png
baselines/scheduler/fluent-light/desktop/default.png
```

Dimensions:
- **component**: lowercase slug from `component-registry.ts` (e.g., `datagrid`)
- **theme-mode**: `{provider}-{mode}` (e.g., `fluent-light`, `bootstrap-dark`)
- **viewport**: `desktop` (1280x900) or `narrow` (768x900, future)
- **scenario**: lowercase-hyphenated state name (e.g., `default`, `sorted`, `selected-row`)

## Running Locally

### Prerequisites
- .NET 10 SDK
- Node.js 18+
- Playwright browsers: `npx playwright install chromium`

### Run all visual parity tests
```bash
npx playwright test --config tests/visual-parity/playwright.config.ts
```

### Run tests for a single component
```bash
npx playwright test --config tests/visual-parity/playwright.config.ts datagrid
```

### Run with a specific theme filter
```bash
npx playwright test --config tests/visual-parity/playwright.config.ts -g "fluent-light"
```

### Update baselines (first run or after intentional visual changes)
```bash
npx playwright test --config tests/visual-parity/playwright.config.ts --update-snapshots
```

### View test report
```bash
npx playwright show-report tests/visual-parity/test-results
```

### Using npm scripts
```bash
npm run test:visual           # Run all visual parity tests
npm run test:visual:update    # Update all baselines
```

## Adding a New Component

1. **Add to registry**: Add an entry in `config/component-registry.ts` with the demo route, root CSS selector, and reference strategy.

2. **Copy the template**: Copy `specs/_template.spec.ts` to `specs/<component-slug>.spec.ts`.

3. **Configure scenarios**: Update `COMPONENT_KEY` and `SCENARIOS` in the new spec. Align scenario names with the CDW capture matrix for that component.

4. **Generate baselines**: Run with `--update-snapshots` to create initial baseline images.

5. **Review**: Compare generated baselines against the CDW parity rubric.

## Updating Baselines

When a visual change is intentional:

1. Run `--update-snapshots` to regenerate affected baselines.
2. Review the new baseline images visually.
3. Commit the updated baselines alongside the code change.

When a visual change is unintentional:

1. The test will fail with a diff image in `test-results/`.
2. Investigate the cause — is it a token change, component change, or demo change?
3. Log the gap in the CDW output folder using `visual-parity-gap-format.md`.

## Design Decisions

### Why Chromium only?
Playwright documentation warns that screenshots differ across browsers and platforms. Starting with a single browser reduces false diffs and keeps baselines manageable. Multi-browser support can be added later by defining additional Playwright projects.

### Why desktop viewport only?
The CDW capture matrices define Desktop (1280px) as the primary review viewport. Narrow viewport testing is defined but commented out in the config for future activation.

### Why element-level screenshots?
Element screenshots isolate the component from surrounding demo page chrome (navigation, headers, theme picker). This makes comparisons more stable and focused on the component itself.

### Why `maxDiffPixelRatio: 0.005`?
A 0.5% tolerance absorbs subpixel rendering variance across runs. Tighten this as baselines stabilize on a consistent CI environment.

### Why `animations: 'disabled'`?
CSS transitions and animations cause screenshot timing variance. Disabling them in capture mode produces deterministic baselines.

## Troubleshooting

**Tests fail with "page not found"**: The demo app may not be running. Start it manually with `dotnet run --project samples/Marilo.Demo` or let the `webServer` config in `playwright.config.ts` start it automatically.

**Tests fail with "locator not found"**: The component's CSS class may differ from the registry. Check the actual rendered HTML and update `component-registry.ts`.

**Baselines differ between local and CI**: Platform differences (font rendering, anti-aliasing) cause this. Consider running CI on the same OS or increasing `maxDiffPixelRatio`.

**Theme not applied**: The theme is set via localStorage before navigation. If the page loads with the wrong theme, check that `addInitScript` runs before `goto`.
