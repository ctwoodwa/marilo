# Stage 01 - Coverage Audit

Map every testable class, endpoint, and component in the solution. Score the untested
surface by risk level. Produce the baseline that drives all later stages.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Coverage config | `../../_config/coverage-context.md` | Solution path, test project path | Defines scan scope |
| Source projects | `{{SOLUTION_PATH}}/src/` | All public types | Subject of the audit |
| Existing test project | `{{TEST_PROJECT_PATH}}` | All test files | Current coverage baseline |
| Risk scoring guide | `../../shared/risk-matrix.md` | All rules | Scoring rubric |

## Process

1. Read `_config/coverage-context.md` and confirm all paths are filled.
2. For each source project, list all public classes and their public methods. Group by project.
3. For each existing test file, record: what it tests (class or feature), test count, and a quality note (e.g., "integration only", "happy path only", "empty test stubs").
4. For each public class with no test coverage, assign a risk score using `shared/risk-matrix.md`.
5. Produce the untested surface table, sorted by risk (high first).
6. Fill `_config/coverage-audit.md` — summary table and both detail tables.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Coverage audit report | `output/[slug]-coverage-audit-report.md` | Detailed findings per project |
| Coverage audit config | `_config/coverage-audit.md` | Filled summary and untested surface table |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 4 | Untested surface table with risk scores | Review and adjust risk scores before they drive Stage 02 priorities |

## Audit

| Check | Pass Condition |
|---|---|
| All source projects covered | Every project in the solution has a section in the audit report |
| All public types inventoried | No public class is missing from the coverage tables |
| Risk scores assigned | Every untested class has a risk score (high / medium / low) |
| Existing tests documented | Quality notes are recorded for every existing test file |
