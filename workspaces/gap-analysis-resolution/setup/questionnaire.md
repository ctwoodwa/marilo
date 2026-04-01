# Onboarding Questionnaire

<!-- Agent instructions: Read this file when the user types "setup". Ask ALL questions
     in a single conversational pass. The user should be able to answer everything in one
     message. Collect answers. Replace placeholders across the specified files. After all
     replacements, verify no {{PLACEHOLDER}} patterns remain in the workspace. -->

### Q1: What is the target project name?
- Placeholder: `{{PROJECT_NAME}}`
- Files: `_config/gap-context.md`
- Type: free text
- Default: (none -- required)

### Q2: What is the path to the target project?
- Placeholder: `{{PROJECT_PATH}}`
- Files: `_config/gap-context.md`
- Type: free text
- Default: (none -- required)

### Q3: What is the technology stack?
- Placeholder: `{{TECH_STACK}}`
- Files: `_config/gap-context.md`
- Type: free text
- Default: .NET / Blazor / C#

### Q4: What is the repository URL? (optional)
- Placeholder: `{{REPO_URL}}`
- Files: `_config/gap-context.md`
- Type: free text
- Default: (leave blank if not applicable)

### Q5: Do you have existing gap analysis files, or do we need to create them?
- Type: selection
- Options: existing, fresh
- Placeholder: `{{ENTRY_PATH}}`
- Files: `_config/gap-context.md`
- If "existing": Ask Q6 and Q7
- If "fresh": Skip Q6/Q7; Stage 01 will assess current state

### Q6: Where are the gap analysis source files? (paths, comma-separated)
- Placeholder: `{{GAP_SOURCE_FILES}}`
- Files: `_config/gap-context.md`
- Type: free text
- Default: (none -- required if Q5 = existing)

### Q7: Is there a gap analysis index file? (path or "none")
- Placeholder: `{{GAP_INDEX_FILE}}`
- Files: `_config/gap-context.md`
- Type: free text
- Default: none

### Q8: What is the target state you are resolving toward?
- Placeholder: `{{TARGET_STATE_DESCRIPTION}}`
- Files: `_config/gap-context.md`
- Type: free text (one paragraph describing what "fully resolved" looks like)
- Default: All components match their documented API specification with no missing parameters, events, or methods.

### Q9: Are there any constraints or notes for this resolution run?
- Placeholder: `{{CONSTRAINTS_AND_NOTES}}`
- Files: `_config/gap-context.md`
- Type: free text
- Default: None.

---

## After Onboarding

The following were configured:
- `_config/gap-context.md` -- populated with project and scope details
- Entry path determined (existing analysis vs. fresh assessment)
- Target state captured

**Next step:** Run Stage 01 (intake). Type `ingest` to start with existing files, or navigate to `stages/01-intake/CONTEXT.md`.

After all replacements, scan the entire workspace for remaining `{{` patterns. If any remain, ask for the missing info.
