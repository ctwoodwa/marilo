# PM Demo Builder — Onboarding

Read this file when the user types "setup". Ask all questions in one conversational pass.

## Questions

**Q1.** What is the scope of this build pass?
- Placeholder: `{{BUILD_SCOPE}}`
- Files: `stages/04-page-build/CONTEXT.md`
- Type: selection
- Options: settings-pages-only | asset-management | dynamic-forms | inspections-and-deficiencies | full-expansion | custom
- Default: settings-pages-only

**Q2.** Which settings pages should be built in this pass (if settings are in scope)?
- Placeholder: `{{SETTINGS_PAGES}}`
- Files: `stages/04-page-build/CONTEXT.md`
- Type: free text (comma-separated page names)
- Default: AccountDetails, PreferencesPage, NotificationsPage

**Q3.** Should DAB/GraphQL entities be created for new data models, or use in-memory mock services only?
- Placeholder: `{{DATA_STRATEGY}}`
- Files: `stages/03-domain-modeling/CONTEXT.md`, `stages/05-integration/CONTEXT.md`
- Type: selection
- Options: dab-entities | mock-only | mock-first-then-dab
- Default: mock-first-then-dab

**Q4.** Are dynamic forms in scope for this pass?
- Placeholder: `{{DYNAMIC_FORMS_IN_SCOPE}}`
- Files: `stages/03-domain-modeling/CONTEXT.md`
- Type: yes/no
- Default: no
- If NO: Skip dynamic form schema design in Stage 03, skip DynamicForm component in Stage 04.

**Q5.** Which asset management features are in scope (if any)?
- Placeholder: `{{ASSET_FEATURES}}`
- Files: `stages/02-ia-and-shell/CONTEXT.md`, `stages/03-domain-modeling/CONTEXT.md`
- Type: free text (comma-separated)
- Default: none
- Options: asset-register, asset-detail, inspections, deficiencies, conditions, remodeling

**Q6.** Should the SettingsLayout be created in this pass?
- Placeholder: `{{CREATE_SETTINGS_LAYOUT}}`
- Files: `stages/04-page-build/CONTEXT.md`
- Type: yes/no
- Default: yes
- Derived from: Q1 — if BUILD_SCOPE includes settings, default yes.

**Q7.** Should ICurrentUserContext be created in this pass?
- Placeholder: `{{CREATE_USER_CONTEXT}}`
- Files: `stages/05-integration/CONTEXT.md`
- Type: yes/no
- Default: yes

## After Onboarding

Tell the user:
- Build scope: `{{BUILD_SCOPE}}`
- Data strategy: `{{DATA_STRATEGY}}`
- Settings pages: `{{SETTINGS_PAGES}}`
- Asset features: `{{ASSET_FEATURES}}`
- Dynamic forms: `{{DYNAMIC_FORMS_IN_SCOPE}}`

Then: "Start with Stage 01 to assess current state, or jump to Stage 02 if you've already run the assessment recently. Your workspace is at `ICM/workspaces/pm-demo-builder/`."
