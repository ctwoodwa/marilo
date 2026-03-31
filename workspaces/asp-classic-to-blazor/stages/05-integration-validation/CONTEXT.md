# Integration and Validation

Verify functional parity between original ASP pages and new Blazor components, then plan the cutover.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Architecture | `../02-architecture-design/output/{{PROJECT_SLUG}}-architecture.md` | "Auth" and "State Management" sections | Know what to validate |
| Conversion checklist | `../04-page-conversion/output/{{PROJECT_SLUG}}-conversion-checklist.md` | Full file | List of all converted pages and their status |
| Page inventory | `../01-inventory-assessment/output/{{PROJECT_SLUG}}-page-inventory.md` | Full file | Original page list for parity comparison |
| DAB config | `../03-data-layer-migration/output/{{PROJECT_SLUG}}-dab-config.json` | Full file | Validate DAB endpoints match expected entities |

## Process

1. Read the conversion checklist and original inventory. Confirm every page has been converted.
2. For each converted page, verify functional parity:
   - Same data displayed (query results match).
   - Same form submissions accepted (field names and validation rules match).
   - Same navigation paths (redirects land on equivalent pages).
3. Validate Telerik theming consistency: confirm all components use the same theme and that custom CSS does not break component rendering.
4. Test authentication and authorization:
   - Login flow works with {{TARGET_AUTH_METHOD}}.
   - Protected pages redirect unauthenticated users.
   - Role-based access matches original behavior.
5. Validate DAB endpoints: confirm each DAB REST and GraphQL endpoint returns correct data, respects permissions, and handles filtering and pagination. Verify custom API endpoints for complex operations.
6. Confirm data integrity: run key queries against the database and compare results with the original ASP application output.
7. Plan cutover strategy:
   - **Side-by-side**: Use YARP reverse proxy to route old URLs to new Blazor app gradually.
   - **Big-bang**: Switch DNS/IIS bindings in a single maintenance window.
   - Recommend approach based on page count and risk tolerance.
8. Document rollback steps in case the cutover needs to be reversed.
9. Produce the validation report and cutover plan.

## Audit

| Check | Pass Condition |
|-------|---------------|
| All pages converted | Conversion checklist shows no remaining "pending" pages |
| Parity confirmed | Every page has a documented parity check result |
| DAB endpoints verified | Every DAB entity returns correct data with proper permissions |
| Auth tested | Login, logout, and protected-page redirect all work |
| Cutover plan complete | Strategy chosen, steps documented, rollback plan included |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Validation report | `output/{{PROJECT_SLUG}}-validation-report.md` | Sections: Parity Results, Auth Tests, Theme Check, Data Integrity |
| Cutover plan | `output/{{PROJECT_SLUG}}-cutover-plan.md` | Sections: Strategy, Steps, Rollback, Timeline |

<!-- Target: keep this file under 80 lines. -->
