# Onboarding Questionnaire -- ResizableContainer Gap Analysis

<!-- Agent instructions: Read this file when the user types "setup". Ask ALL questions
     in a single conversational pass. Collect answers and update _config/gap-context.md.
     After all updates, verify no placeholders remain in the workspace. -->

### Q1: What is the target project name?
- Files: `_config/gap-context.md`
- Type: free text
- Default: Marilo.Components

### Q2: What is the path to the target component source?
- Files: `_config/gap-context.md`
- Type: free text
- Default: `src/Marilo.Components/Layout/ResizableContainer/`

### Q3: What is the technology stack?
- Files: `_config/gap-context.md`
- Type: free text
- Default: .NET 10 / Blazor / C# / Razor Class Library

### Q4: What is the repository URL? (optional)
- Files: `_config/gap-context.md`
- Type: free text
- Default: https://github.com/ctwoodwa/Marilo

### Q5: Do you have existing gap analysis files, or do we need to create them?
- Type: selection
- Options: existing, fresh
- If "existing": Ask Q6 and Q7
- If "fresh": Skip Q6/Q7; Stage 01 will assess current state

### Q6: Where are the gap analysis source files? (paths, comma-separated)
- Files: `_config/gap-context.md`
- Type: free text
- Default: (required if Q5 = existing)

### Q7: Is there a gap analysis index file? (path or "none")
- Files: `_config/gap-context.md`
- Type: free text
- Default: none

### Q8: What is the target state you are resolving toward?
- Files: `_config/gap-context.md`
- Type: free text
- Default: MariloResizableContainer fully matches its documented API specification with no missing parameters, events, or methods. All provider CSS classes implemented. All tests passing.

### Q9: Are there any constraints or notes for this resolution run?
- Files: `_config/gap-context.md`
- Type: free text
- Default: Follow Marilo provider-first architecture. SCSS must be rebuilt after style changes. Public methods must be dispatcher-safe.

---

## After Onboarding

The following were configured:
- `_config/gap-context.md` -- populated with project and scope details
- Entry path determined (existing analysis vs. fresh assessment)
- Target state captured

**Next step:** Run Stage 01 (intake). Type `ingest` to start with existing files, or navigate to `stages/01-intake/CONTEXT.md`.
