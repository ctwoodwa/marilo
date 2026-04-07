# Integration Validation

Verify functional parity between the original jQuery page and the converted Blazor page. Plan regression testing and cutover.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Converted pages | `../04-page-conversion/output/` | All output files | The converted Blazor components to validate |
| Codebase patterns | `../02-codebase-alignment/output/{{PROJECT_SLUG}}-codebase-patterns.md` | Full file | Verify pattern compliance |
| Feature inventory | `../01-feature-inventory/output/{{PROJECT_SLUG}}-feature-inventory.md` | Full file | Checklist for functional parity |
| Source page | `{{JQUERY_SOURCE_PATH}}` | Full file | The original page to compare against |

## Process

1. Build a functional parity checklist from the Stage 01 feature inventory. For each cataloged feature, define: what to test, expected behavior, and pass/fail criteria.
2. Verify data operations: every AJAX call in the original page has a working service call in the converted page. Test list, detail, create, update, delete flows.
3. Verify event handling: every jQuery event handler has a working Blazor equivalent. Test click handlers, change handlers, keyboard handlers, form submissions.
4. Verify UI interactions: show/hide toggles, modal dialogs, tab switching, accordion expand/collapse, dropdown behavior, tooltip display.
5. Verify form validation: every validation rule from the original page fires correctly. Test required fields, format validation, custom rules, error message display.
6. Verify authentication integration: the converted page respects the project's auth configuration ({{BLAZOR_AUTH_METHOD}}). Test authorized and unauthorized access.
7. Verify styling and layout: the converted page visually matches the original page at common viewport widths. Note any intentional improvements.
8. Verify Telerik theme consistency: the converted page uses the same Telerik theme as the rest of the project. No visual mismatch between new and existing pages.
9. Identify any JSInterop bridge items and document their testing requirements separately.
10. Produce a test report with pass/fail status for every item.
11. Produce a cutover plan: file deployment steps, service registration changes, route configuration, and rollback procedure.

## Audit

| Check | Pass Condition |
|-------|---------------|
| Parity achieved | Every feature in the Stage 01 inventory has a passing test |
| Auth works | Page correctly requires authentication and respects role-based access |
| Theme consistent | No visual mismatch between converted page and existing project pages |
| No console errors | Browser console shows no JavaScript errors or Blazor circuit errors |
| Performance acceptable | Page load and interaction times are comparable to the original |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Test report | `output/{{PROJECT_SLUG}}-test-report.md` | Markdown with pass/fail table for every feature |
| Cutover plan | `output/{{PROJECT_SLUG}}-cutover-plan.md` | Markdown with deployment steps, registration changes, rollback procedure |
