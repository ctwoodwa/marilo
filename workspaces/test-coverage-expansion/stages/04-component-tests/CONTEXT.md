# Stage 04 - Component Tests

Generate bunit test scaffolds for Blazor pages and components assigned to the component
test layer. Each scaffold tests rendering, user interactions, and service call verification.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Test strategy | `../../_config/test-strategy.md` | Component-layer assignments | Defines what to scaffold |
| Blazor component files | `{{SOLUTION_PATH}}/*Web*/Components/` | Assigned components and pages | Content source |
| Test patterns | `../../shared/test-patterns.md` | bunit patterns | Authoring guide |
| Coverage config | `../../_config/coverage-context.md` | Test project path | Output destination |

## Process

1. For each component or page assigned to the component layer, read the `.razor` and code-behind file to understand:
   - Parameters and cascading parameters
   - Services injected via `@inject`
   - User interactions (buttons, forms, navigation)
   - Conditional rendering logic
2. Generate a bunit test class:
   - One test class per component, named `[ComponentName]Tests`
   - A `TestContext` setup in `[SetUp]` with mocked service registrations using bunit's `Services.AddSingleton`
   - A `renders_correctly` test that verifies the component mounts without error
   - A representative interaction test (e.g., button click triggers service call)
   - `// TODO: [test description]` comments for remaining interaction and rendering cases
3. For pages with forms or data submission: include a test that verifies the form submission path.
4. Write tests to the existing test project at `{{TEST_PROJECT_PATH}}`.
5. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Component test files | `{{TEST_PROJECT_PATH}}/Components/` | C# NUnit + bunit test files |
| Component test scaffold index | `output/[slug]-component-test-index.md` | Table: test file, component, tests written, TODOs remaining |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 2 (first scaffold) | First generated component test class | Approve bunit pattern before applying to remaining components |
| 4 | Complete scaffold index | Confirm all assigned components are covered |

## Audit

| Check | Pass Condition |
|---|---|
| All component-assigned files scaffolded | Every component in the assignment table has a test file |
| Renders-correctly test present | Every test class has at least one rendering test |
| Service dependencies mocked | No test resolves real services from a DI container |
| Tests compile | No unresolved references in generated test files |
| No Playwright or Selenium | Browser-automation tests are not in this workspace |
