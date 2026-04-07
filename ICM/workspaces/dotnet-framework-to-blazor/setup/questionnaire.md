# Onboarding Questionnaire

<!-- Agent instructions: Read this file when the user types "setup". Ask ALL questions
     in a single conversational pass. The user should be able to answer everything in one
     message. Collect answers. Replace placeholders across the specified files. After all
     replacements, verify no {{PLACEHOLDER}} patterns remain in the workspace. -->

### Q1: What is the path or repo URL for the .NET Framework solution?
- Placeholder: `{{SOLUTION_PATH}}`
- Files: `stages/01-inventory-assessment/CONTEXT.md`
- Type: free text
- Default: (no default, required)

### Q2: Which .NET Framework version is the source project targeting?
- Placeholder: `{{SOURCE_FX_VERSION}}`
- Files: `stages/01-inventory-assessment/CONTEXT.md`, `stages/02-architecture-design/CONTEXT.md`
- Type: selection
- Options: .NET Framework 4.5, .NET Framework 4.6, .NET Framework 4.7, .NET Framework 4.8
- Default: .NET Framework 4.8

### Q3: What type of .NET Framework project is this?
- Placeholder: `{{PROJECT_TYPE}}`
- Files: `stages/01-inventory-assessment/CONTEXT.md`, `stages/04-ui-conversion/CONTEXT.md`
- Type: multi-select
- Options: ASP.NET WebForms, ASP.NET MVC, ASP.NET Web API, WCF Services, Windows Services, Class Libraries
- Default: ASP.NET MVC

### Q4: Which Blazor hosting model should the target project use?
- Placeholder: `{{BLAZOR_HOSTING_MODEL}}`
- Files: `stages/02-architecture-design/CONTEXT.md`, `stages/04-ui-conversion/CONTEXT.md`
- Type: selection
- Options: Blazor Server, Blazor WebAssembly, Blazor Auto (Server + WASM)
- Default: Blazor Server (simplest migration path from server-rendered Framework apps)

### Q5: What database does the source project connect to?
- Placeholder: `{{DATABASE_TYPE}}`
- Files: `stages/03-data-layer-migration/CONTEXT.md`
- Type: selection
- Options: SQL Server, Oracle, MySQL, PostgreSQL, Other
- Default: SQL Server

### Q6: What data access approach does the source project use?
- Placeholder: `{{SOURCE_DATA_ACCESS}}`
- Files: `stages/01-inventory-assessment/CONTEXT.md`, `stages/03-data-layer-migration/CONTEXT.md`
- Type: multi-select
- Options: Entity Framework 6, ADO.NET (SqlConnection/SqlCommand), Dapper, NHibernate, Stored Procedures, LINQ to SQL
- Default: Entity Framework 6

### Q7: What data access approach should the new project use?
- Placeholder: `{{TARGET_DATA_ACCESS}}`
- Files: `stages/03-data-layer-migration/CONTEXT.md`
- Type: selection
- Options: DAB + EF Core (DAB for CRUD, EF Core for complex logic), DAB only (pure REST/GraphQL), EF Core only (full ORM), Hybrid DAB + EF Core + Dapper
- Default: DAB + EF Core (DAB for CRUD, EF Core for complex logic)

### Q8: How does the current project handle authentication?
- Placeholder: `{{CURRENT_AUTH_METHOD}}`
- Files: `stages/02-architecture-design/CONTEXT.md`, `stages/05-integration-validation/CONTEXT.md`
- Type: free text
- Default: Forms Authentication
- Examples: Forms Authentication, Windows/IIS auth, OWIN/OAuth, custom token-based, none

### Q9: What authentication approach should the new Blazor project use?
- Placeholder: `{{TARGET_AUTH_METHOD}}`
- Files: `stages/02-architecture-design/CONTEXT.md`, `stages/05-integration-validation/CONTEXT.md`
- Type: selection
- Options: ASP.NET Core Identity, Azure AD/Entra ID, Okta/OIDC, Windows Authentication, Cookie-based custom, None
- Default: ASP.NET Core Identity

### Q10: Do you have a Telerik UI for Blazor license?
- Placeholder: `{{TELERIK_LICENSE_STATUS}}`
- Files: `stages/04-ui-conversion/CONTEXT.md`
- Type: selection
- Options: Yes (licensed), Trial (evaluating), No (will use free alternatives)
- Default: Yes (licensed)
- If "No": Agent will note Telerik alternatives in Stage 02 architecture output

### Q11: What is the target deployment environment?
- Placeholder: `{{DEPLOYMENT_TARGET}}`
- Files: `stages/05-integration-validation/CONTEXT.md`
- Type: selection
- Options: Azure App Service, IIS on-premises, Docker/Kubernetes, AWS, Other
- Default: IIS on-premises

### Q12: What is the target .NET version?
- Placeholder: `{{DOTNET_VERSION}}`
- Files: `stages/02-architecture-design/CONTEXT.md`, `stages/04-ui-conversion/CONTEXT.md`
- Type: selection
- Options: .NET 8 (LTS), .NET 9, .NET 10
- Default: .NET 8 (LTS)

### Q13: What migration strategy do you prefer?
- Placeholder: `{{MIGRATION_STRATEGY}}`
- Files: `stages/02-architecture-design/CONTEXT.md`, `stages/05-integration-validation/CONTEXT.md`
- Type: selection
- Options: Incremental (YARP proxy, migrate page-by-page alongside Framework app), Big-bang (full rewrite, deploy as replacement), Side-by-side (both apps running, gradual traffic shift)
- Default: Incremental (YARP proxy, migrate page-by-page alongside Framework app)

### Q14: What is the slug name for this migration project? (used in output file names)
- Placeholder: `{{PROJECT_SLUG}}`
- Files: all stage CONTEXT.md output tables
- Type: free text
- Default: fx-migration
- Derived: `{{PROJECT_NAMESPACE}}` -- PascalCase version of the slug for C# namespaces

---

## After Onboarding

Your workspace is configured. Here is what was set up:

- **Source**: `{{SOLUTION_PATH}}` ({{SOURCE_FX_VERSION}}, {{PROJECT_TYPE}})
- **Target**: {{BLAZOR_HOSTING_MODEL}} on {{DOTNET_VERSION}}
- **Database**: {{DATABASE_TYPE}}
- **Data access**: {{SOURCE_DATA_ACCESS}} migrating to {{TARGET_DATA_ACCESS}}
- **Auth**: {{CURRENT_AUTH_METHOD}} migrating to {{TARGET_AUTH_METHOD}}
- **Telerik**: {{TELERIK_LICENSE_STATUS}}
- **Deploy to**: {{DEPLOYMENT_TARGET}}
- **Strategy**: {{MIGRATION_STRATEGY}}
- **Output slug**: {{PROJECT_SLUG}}

Start with `inventory` to catalog your .NET Framework solution, or go directly to Stage 01.

After all replacements, scan the entire workspace for remaining `{{` patterns. If any remain, ask for the missing info.
