# Stage 06: Testing

Write unit tests, CSS provider tests, and validate the complete component.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| API design | `../02-api-design/output/api-design.md` | Full file | Parameters and events to test |
| Implementation | `../03-implementation/output/implementation-summary.md` | Full file | File paths and implementation details |
| Reference | `../../shared/component-patterns.md` | "Test Patterns" section | bUnit test base class and conventions |

## Process

1. Read the API design and implementation summary
2. Create component test file in `tests/Marilo.Tests.Unit/`:
   - Place in the appropriate category subfolder
   - Inherit from `MariloTestBase`
   - Test default rendering (component renders without errors)
   - Test each parameter affects output (CSS classes, attributes, content)
   - Test event callbacks fire correctly
   - Test disabled state behavior
   - Test accessibility attributes (ARIA role, aria-* attributes)
3. Create CSS provider tests:
   - Test each provider method returns expected class strings
   - Test all variant/size/state combinations
   - Test both FluentUI and Bootstrap providers
4. If the component has JS interop, add integration test stubs
5. Run all tests: `dotnet test`
6. Run the audit checks below. If any fail, fix before saving.
7. Write the test summary

## Audit

| Check | Pass Condition |
|-------|---------------|
| Render test | Component renders without exceptions in default state |
| Parameter coverage | Every public parameter has at least one test |
| Event coverage | Every EventCallback has a test verifying it fires |
| Provider coverage | CSS provider methods tested for both FluentUI and Bootstrap |
| Tests pass | `dotnet test` completes with zero failures |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Test summary | `output/test-summary.md` | List of test files, test count, pass/fail results |
