# Integration and Validation

Verify functional parity between original .NET Framework app and new Blazor app, validate DAB endpoints, and plan the cutover.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Architecture | `../02-architecture-design/output/{{PROJECT_SLUG}}-architecture.md` | "Auth", "Migration Strategy", and "Middleware" sections | Know what to validate |
| Conversion checklist | `../04-ui-conversion/output/{{PROJECT_SLUG}}-conversion-checklist.md` | Full file | List of all converted pages and their status |
| Solution inventory | `../01-inventory-assessment/output/{{PROJECT_SLUG}}-solution-inventory.md` | Full file | Original project list for parity comparison |
| DAB config | `../03-data-layer-migration/output/{{PROJECT_SLUG}}-dab-config.json` | Full file | Validate DAB endpoints |

## Process

1. Read the conversion checklist and original inventory. Confirm every page/view/endpoint has been converted.
2. Validate DAB endpoints:
   - Start DAB locally and test REST endpoints for each entity (GET list, GET by PK, POST, PATCH, DELETE).
   - Verify GraphQL queries return expected data.
   - Confirm permissions block unauthorized access.
3. For each converted page, verify functional parity:
   - Same data displayed (query results match original).
   - Same form submissions accepted (field names and validation rules match).
   - Same navigation paths (redirects land on equivalent pages).
4. Validate Telerik theming consistency: confirm all components use the same theme and that custom CSS does not break component rendering.
5. Test authentication and authorization:
   - Login flow works with {{TARGET_AUTH_METHOD}}.
   - Protected pages redirect unauthenticated users.
   - Role-based access matches original behavior.
   - DAB permissions align with app-level auth roles.
6. Confirm data integrity: run key queries against the database and compare results with the original app output.
7. Plan cutover based on {{MIGRATION_STRATEGY}}:
   - **Incremental**: configure YARP reverse proxy to route migrated URLs to Blazor, legacy URLs to Framework app. Document per-route migration schedule.
   - **Big-bang**: plan maintenance window, DNS/IIS binding switch, rollback procedure.
   - **Side-by-side**: configure shared auth (cookie bridging or token exchange), define traffic split rules, monitor parity metrics.
8. Document rollback steps in case the cutover needs to be reversed.
9. Produce the validation report and cutover plan.

## Audit

| Check | Pass Condition |
|-------|---------------|
| All pages converted | Conversion checklist shows no remaining "pending" pages |
| DAB endpoints tested | Every DAB entity has passing CRUD tests |
| Parity confirmed | Every page has a documented parity check result |
| Auth tested | Login, logout, and protected-page redirect all work |
| Cutover plan complete | Strategy steps documented with rollback plan |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Validation report | `output/{{PROJECT_SLUG}}-validation-report.md` | Sections: DAB Endpoint Tests, Parity Results, Auth Tests, Theme Check, Data Integrity |
| Cutover plan | `output/{{PROJECT_SLUG}}-cutover-plan.md` | Sections: Strategy, YARP Config (if incremental), Steps, Rollback, Timeline |

<!-- Target: keep this file under 80 lines. -->
