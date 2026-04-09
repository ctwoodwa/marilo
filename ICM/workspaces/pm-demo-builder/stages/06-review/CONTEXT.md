# Stage 06: Review and Polish

Verify acceptance criteria, check for gaps, and polish the implementation.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../05-integration/output/integration-manifest.md` | Full file | What was wired |
| Previous stage | `../04-page-build/output/build-manifest.md` | Full file | What was built |
| Config | `../../_config/pm-demo-context.md` | Full file | Baseline to compare against |
| Reference | `../../shared/implementation-guardrails.md` | Full file | Rules to verify against |

## Process

1. Run `dotnet build` and `dotnet test` — confirm clean build and passing tests.
2. Walk every new route in the sidebar — confirm navigability and correct layout nesting.
3. Verify guardrail compliance:
   a. Only MainLayout renders MariloAppShell.
   b. All event subscribers implement IDisposable.
   c. No page binds to DTOs directly (only VMs).
   d. Demo auth values clearly marked "DEMO ONLY".
   e. All form inputs are Marilo-native.
4. Check for visual polish issues:
   a. Consistent spacing and typography across new pages.
   b. Loading/empty/error states present where expected.
   c. Toast notifications fire for save actions.
5. Check for missing features against the IA plan.
6. Update `_config/pm-demo-context.md` with the new state.
7. Update `samples/Marilo.PmDemo/SETTINGS_STATUS.md` if settings pages were part of this pass.
8. Write the review report to output.

## Audit

| Check | Pass Condition |
|-------|---------------|
| Build + tests | 0 errors, 0 test failures |
| Route coverage | Every route in the IA plan is navigable |
| Guardrail compliance | All 5 guardrails verified |
| Context updated | `_config/pm-demo-context.md` reflects current state |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Review report | `output/review-report.md` | Pass/fail table, gap list, polish notes, updated context reference |
