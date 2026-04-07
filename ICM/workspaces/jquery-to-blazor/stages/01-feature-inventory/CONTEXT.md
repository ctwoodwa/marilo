# Feature Inventory

Catalog every jQuery feature, plugin, DOM interaction, and data flow in the source page. This is the foundation for all downstream stages.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Source page | `{{JQUERY_SOURCE_PATH}}` | Full file(s) | The jQuery page(s) to inventory |
| Plugin reference | `../../shared/jquery-plugin-map.md` | "Plugin Inventory Checklist" | Structured list of common jQuery plugins to scan for |

## Process

1. Scan the source page HTML for all `<script>` tags. List every JS file reference (CDN and local), inline script blocks, and their load order.
2. Identify the jQuery version and any jQuery plugins loaded. For each plugin, record: name, version (if available), CDN/local path, and what feature it provides.
3. Catalog every `$()` selector pattern used. Group by type: DOM queries, event binding, AJAX calls, animations, DOM manipulation, utility functions.
4. For each AJAX call (`$.ajax`, `$.get`, `$.post`, `$.getJSON`, `fetch`), record: HTTP method, URL/endpoint, request payload shape, response handling, error handling, and any loading/spinner behavior.
5. Map every DOM event handler. Record: element selector, event type (click, change, keyup, submit, etc.), handler behavior (what it does), and whether it uses event delegation.
6. Catalog all DOM manipulation patterns: element creation, show/hide toggling, class toggling, attribute changes, HTML injection, element removal. Note which are conditional on data state.
7. Identify all form elements and their validation rules: required fields, regex patterns, custom validation functions, error display approach.
8. Catalog any CSS/class manipulation: `.addClass()`, `.removeClass()`, `.toggleClass()`, `.css()`, conditional styling.
9. Identify third-party libraries beyond jQuery: charting libs, date pickers, rich text editors, notification libraries, modal/dialog libraries.
10. Rate each feature by migration complexity: Simple (direct Blazor equivalent exists), Moderate (requires component composition or service), Complex (requires JSInterop or custom solution).
11. Save the complete inventory to output.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 10 | Full feature inventory with complexity ratings | Confirm completeness, flag any missed features, adjust complexity ratings |

## Audit

| Check | Pass Condition |
|-------|---------------|
| No jQuery usage missed | Every `$()`, `jQuery()`, and plugin call in the source is cataloged |
| AJAX endpoints complete | Every endpoint URL has method, payload, and response shape documented |
| Event handlers complete | Every event binding (`.on()`, `.click()`, `.change()`, etc.) is listed |
| Complexity rated | Every feature has a Simple/Moderate/Complex rating |
| Plugin versions noted | Every third-party library has name and version recorded |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Feature inventory | `output/{{PROJECT_SLUG}}-feature-inventory.md` | Structured markdown with tables per category: scripts, plugins, AJAX calls, events, DOM manipulation, forms, third-party libs, complexity summary |
