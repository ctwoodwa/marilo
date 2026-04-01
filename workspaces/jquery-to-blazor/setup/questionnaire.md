# Onboarding Questionnaire

<!-- Agent instructions: Read this file when the user types "setup". Ask ALL questions
     in a single conversational pass. The user should be able to answer everything in one
     message. Collect answers. Replace placeholders across the specified files. After all
     replacements, verify no {{PLACEHOLDER}} patterns remain in the workspace. -->

### Q1: What is the path or URL for the jQuery-based source page(s)?
- Placeholder: `{{JQUERY_SOURCE_PATH}}`
- Files: `stages/01-feature-inventory/CONTEXT.md`
- Type: free text
- Default: (no default, required)
- Note: Can be a single .html/.aspx/.cshtml file, a folder of pages, or a URL to a running page

### Q2: What is the path or repo URL for the target Blazor project?
- Placeholder: `{{BLAZOR_PROJECT_PATH}}`
- Files: `stages/02-codebase-alignment/CONTEXT.md`, `stages/04-page-conversion/CONTEXT.md`
- Type: free text
- Default: (no default, required)
- Note: The existing .NET Core Blazor project where converted pages will be added

### Q3: Which Blazor hosting model does the target project use?
- Placeholder: `{{BLAZOR_HOSTING_MODEL}}`
- Files: `stages/02-codebase-alignment/CONTEXT.md`, `stages/04-page-conversion/CONTEXT.md`
- Type: selection
- Options: Blazor Server, Blazor WebAssembly, Blazor Auto (Server + WASM)
- Default: Blazor Server

### Q4: What is the target .NET version?
- Placeholder: `{{DOTNET_VERSION}}`
- Files: `stages/02-codebase-alignment/CONTEXT.md`, `stages/04-page-conversion/CONTEXT.md`
- Type: selection
- Options: .NET 8 (LTS), .NET 9, .NET 10
- Default: .NET 10

### Q5: How does the jQuery page fetch data? (select all that apply)
- Placeholder: `{{JQUERY_DATA_ACCESS}}`
- Files: `stages/01-feature-inventory/CONTEXT.md`
- Type: free text
- Default: AJAX calls to REST API
- Examples: $.ajax to REST API, $.getJSON, fetch API, inline server-rendered data, WebSocket, SignalR

### Q6: What data access pattern does the target Blazor project use?
- Placeholder: `{{BLAZOR_DATA_ACCESS}}`
- Files: `stages/02-codebase-alignment/CONTEXT.md`, `stages/04-page-conversion/CONTEXT.md`
- Type: selection
- Options: HttpClient to REST API, DAB (Data API Builder), EF Core services, GraphQL, Hybrid (DAB + custom API)
- Default: HttpClient to REST API
- Note: The converted page will follow the same data access pattern already established in the project

### Q7: How does the existing Blazor project handle authentication?
- Placeholder: `{{BLAZOR_AUTH_METHOD}}`
- Files: `stages/02-codebase-alignment/CONTEXT.md`, `stages/05-integration-validation/CONTEXT.md`
- Type: free text
- Default: Okta/JWT
- Examples: Okta/JWT, Azure AD/Entra ID, ASP.NET Core Identity, Windows Auth, Cookie-based, None

### Q8: Do you have a Telerik UI for Blazor license?
- Placeholder: `{{TELERIK_LICENSE_STATUS}}`
- Files: `stages/03-component-mapping/CONTEXT.md`, `stages/04-page-conversion/CONTEXT.md`
- Type: selection
- Options: Yes (licensed), Trial (evaluating), No (will use free alternatives)
- Default: Yes (licensed)
- If "No": Agent will note Telerik alternatives in Stage 03 component mapping output

### Q9: Does the jQuery page use any specific jQuery plugins or libraries?
- Placeholder: `{{JQUERY_PLUGINS}}`
- Files: `stages/01-feature-inventory/CONTEXT.md`
- Type: free text
- Default: None identified yet (Stage 01 will discover them)
- Examples: DataTables, jQuery UI, Select2, Chosen, Bootstrap jQuery plugins, jQuery Validate, Toastr, SweetAlert, Moment.js, Lodash

### Q10: What is the slug name for this migration project? (used in output file names)
- Placeholder: `{{PROJECT_SLUG}}`
- Files: all stage CONTEXT.md output tables
- Type: free text
- Default: jquery-migration
- Derived: `{{PROJECT_NAMESPACE}}` -- PascalCase version of the slug for C# namespaces

### Q11: What is the target deployment environment?
- Placeholder: `{{DEPLOYMENT_TARGET}}`
- Files: `stages/05-integration-validation/CONTEXT.md`
- Type: selection
- Options: Azure App Service, IIS on-premises, Docker/Kubernetes, AWS, Other
- Default: IIS on-premises

---

## After Onboarding

Your workspace is configured. Here is what was set up:

- **jQuery source**: `{{JQUERY_SOURCE_PATH}}`
- **Target project**: `{{BLAZOR_PROJECT_PATH}}` ({{BLAZOR_HOSTING_MODEL}} on {{DOTNET_VERSION}})
- **jQuery data access**: {{JQUERY_DATA_ACCESS}}
- **Blazor data access**: {{BLAZOR_DATA_ACCESS}}
- **Auth**: {{BLAZOR_AUTH_METHOD}}
- **Telerik**: {{TELERIK_LICENSE_STATUS}}
- **Known plugins**: {{JQUERY_PLUGINS}}
- **Deploy to**: {{DEPLOYMENT_TARGET}}
- **Output slug**: {{PROJECT_SLUG}}

Start with `inventory` to catalog the jQuery page features, or go directly to Stage 01.

After all replacements, scan the entire workspace for remaining `{{` patterns. If any remain, ask for the missing info.
