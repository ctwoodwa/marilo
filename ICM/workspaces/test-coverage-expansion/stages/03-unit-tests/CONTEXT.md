# Stage 03 - Unit Tests

Generate unit test scaffolds for all areas assigned to the unit layer in the test strategy.
Each scaffold contains the test class structure, a representative passing test, and commented
placeholders for the remaining cases identified in the audit.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Test strategy | `../../_config/test-strategy.md` | Unit-layer assignments | Defines what to scaffold |
| Source classes | `{{SOLUTION_PATH}}/` | Classes assigned to unit layer | Content source |
| Test patterns | `../../shared/test-patterns.md` | NUnit patterns, AAA structure | Authoring guide |
| Coverage config | `../../_config/coverage-context.md` | Test project path | Output destination |

## Process

1. For each class assigned to the unit layer, read the source class to understand its public API and dependencies.
2. Identify the test cases: happy path, boundary values, null/empty inputs, and error conditions for each public method.
3. Generate a test class:
   - One `[TestFixture]` class per source class, named `[ClassName]Tests`
   - One representative passing `[Test]` method to establish the pattern
   - `// TODO: [test description]` comments for each remaining case
   - `[SetUp]` method with constructor arguments stubbed using value objects (no mocks, no DI container)
4. For solver/calculator classes (high risk): write at least three concrete test cases (not just one + TODOs).
5. Write tests to the existing test project at `{{TEST_PROJECT_PATH}}`.
6. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Unit test files | `{{TEST_PROJECT_PATH}}/Unit/` | C# NUnit test files |
| Unit test scaffold index | `output/[slug]-unit-test-index.md` | Table: test file, source class, tests written, TODOs remaining |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 (first scaffold) | First generated test class | Approve pattern before applying to all remaining classes |
| 5 | Complete scaffold index | Confirm all assigned classes are covered |

## Audit

| Check | Pass Condition |
|---|---|
| All unit-assigned classes scaffolded | Every class in the unit assignment table has a test file |
| Solver classes have 3+ concrete tests | No solver/calculator test class has fewer than three non-TODO tests |
| No infrastructure in unit tests | No `DbContext`, `HttpClient`, or Aspire references in the Unit/ folder |
| Tests follow AAA | Every test has distinct Arrange, Act, Assert sections |
| Tests compile | No unresolved references in generated test files |
