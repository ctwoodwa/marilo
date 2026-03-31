# Onboarding Questionnaire

<!-- Agent instructions: Read this file when the user types "setup". Ask ALL questions
     in a single conversational pass. The user should be able to answer everything in one
     message. Collect answers. Replace placeholders across the specified files. After all
     replacements, verify no {{PLACEHOLDER}} patterns remain in the workspace. -->

### Q1: What is the path or repo URL for the ASP Classic source project?
- Placeholder: `{{ASP_SOURCE_PATH}}`
- Files: `stages/01-inventory-assessment/CONTEXT.md`
- Type: free text
- Default: (no default, required)

### Q2: Which Blazor hosting model should the target project use?
- Placeholder: `{{BLAZOR_HOSTING_MODEL}}`
- Files: `stages/02-architecture-design/CONTEXT.md`, `stages/04-page-conversion/CONTEXT.md`
- Type: selection
- Options: Blazor Server, Blazor WebAssembly, Blazor Auto (Server + WASM)
- Default: Blazor Server (simplest migration path from server-rendered ASP)

### Q3: What database does the ASP Classic project connect to?
- Placeholder: `{{DATABASE_TYPE}}`
- Files: `stages/03-data-layer-migration/CONTEXT.md`
- Type: selection
- Options: SQL Server, Oracle, MySQL, MS Access, Other
- Default: SQL Server
- Derived: `{{CONNECTION_STRING_PATTERN}}` -- agent fills in the typical connection string format based on the database type

### Q4: Should the project use Microsoft Data API Builder (DAB) for CRUD endpoints?
- Placeholder: `{{DAB_ENABLED}}`
- Files: `stages/03-data-layer-migration/CONTEXT.md`
- Type: selection
- Options: Yes (DAB for CRUD, custom API for complex logic), No (all endpoints via EF Core/Dapper)
- Default: Yes
- If "Yes": DAB auto-generates REST and GraphQL endpoints for standard CRUD. The custom API handles only complex business logic.
- If "No": workspace falls back to pure EF Core/Dapper for all data access; DAB-related steps in Stage 03 are skipped.

### Q5: What data access approach should the custom API service use?
- Placeholder: `{{DATA_ACCESS_APPROACH}}`
- Files: `stages/03-data-layer-migration/CONTEXT.md`
- Type: selection
- Options: EF Core (full ORM), Dapper (micro-ORM), Hybrid (EF Core for CRUD + Dapper for complex queries)
- Default: Hybrid
- Note: When DAB is enabled, this applies only to the custom API service for complex operations. DAB handles standard CRUD independently.

### Q6: How does the current ASP Classic project handle authentication?
- Placeholder: `{{CURRENT_AUTH_METHOD}}`
- Files: `stages/02-architecture-design/CONTEXT.md`, `stages/05-integration-validation/CONTEXT.md`
- Type: free text
- Default: None (anonymous access)
- Examples: Windows/IIS auth, custom login page with Session, cookie-based, none

### Q7: What authentication approach should the new Blazor project use?
- Placeholder: `{{TARGET_AUTH_METHOD}}`
- Files: `stages/02-architecture-design/CONTEXT.md`, `stages/05-integration-validation/CONTEXT.md`
- Type: selection
- Options: ASP.NET Core Identity, Azure AD/Entra ID, Windows Authentication, Cookie-based custom, None
- Default: ASP.NET Core Identity

### Q8: Do you have a Telerik UI for Blazor license?
- Placeholder: `{{TELERIK_LICENSE_STATUS}}`
- Files: `stages/04-page-conversion/CONTEXT.md`
- Type: selection
- Options: Yes (licensed), Trial (evaluating), No (will use free alternatives)
- Default: Yes (licensed)
- If "No": Agent will note Telerik alternatives in Stage 02 architecture output

### Q9: What is the target deployment environment?
- Placeholder: `{{DEPLOYMENT_TARGET}}`
- Files: `stages/05-integration-validation/CONTEXT.md`
- Type: selection
- Options: Azure App Service, IIS on-premises, Docker/Kubernetes, AWS, Other
- Default: Azure App Service

### Q10: What is the target .NET version?
- Placeholder: `{{DOTNET_VERSION}}`
- Files: `stages/02-architecture-design/CONTEXT.md`, `stages/04-page-conversion/CONTEXT.md`
- Type: selection
- Options: .NET 8 (LTS), .NET 9
- Default: .NET 8 (LTS)

### Q11: What is the slug name for this migration project? (used in output file names)
- Placeholder: `{{PROJECT_SLUG}}`
- Files: all stage CONTEXT.md output tables
- Type: free text
- Default: asp-migration
- Derived: `{{PROJECT_NAMESPACE}}` -- PascalCase version of the slug for C# namespaces

---

## After Onboarding

Your workspace is configured. Here is what was set up:

- **Source project**: `{{ASP_SOURCE_PATH}}`
- **Target**: {{BLAZOR_HOSTING_MODEL}} on {{DOTNET_VERSION}}
- **DAB**: {{DAB_ENABLED}}
- **Database**: {{DATABASE_TYPE}} with {{DATA_ACCESS_APPROACH}} (custom API)
- **Auth**: {{CURRENT_AUTH_METHOD}} migrating to {{TARGET_AUTH_METHOD}}
- **Telerik**: {{TELERIK_LICENSE_STATUS}}
- **Deploy to**: {{DEPLOYMENT_TARGET}}
- **Output slug**: {{PROJECT_SLUG}}

Start with `inventory` to catalog your ASP Classic pages, or go directly to Stage 01.

After all replacements, scan the entire workspace for remaining `{{` patterns. If any remain, ask for the missing info.
