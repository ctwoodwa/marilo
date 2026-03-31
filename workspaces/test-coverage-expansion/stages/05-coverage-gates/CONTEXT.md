# Stage 05 - Coverage Gates

Configure coverage collection, reporting, and enforcement thresholds in the build and CI pipeline.
Produces the definition of done for test coverage.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Test strategy | `../../_config/test-strategy.md` | Coverage targets per layer | Threshold values |
| Coverage config | `../../_config/coverage-context.md` | Coverage tool, report format | Tooling choices |
| Test project file | `{{TEST_PROJECT_PATH}}/*.csproj` | Current references | What to add Coverlet to |
| CI workflow | (from ci-cd-pipeline workspace if available) | Test job | Where to add coverage upload step |

## Process

1. Add Coverlet to the test project if not already present: `coverlet.collector` NuGet package.
2. Generate the `dotnet test` command with coverage collection flags:
   ```
   dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
   ```
3. Generate the `reportgenerator` command to produce an HTML report from the coverage XML.
4. Define the threshold enforcement command using Coverlet's `--threshold` flag per layer.
5. Document how to integrate the coverage gate into the CI pipeline test job.
6. Define the definition of done: the combination of threshold values, report artifacts, and CI gate that a sprint or release must satisfy.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Coverage commands | `output/[slug]-coverage-commands.md` | Shell commands for collection, reporting, thresholds |
| CI coverage integration | `output/[slug]-ci-coverage-integration.md` | Steps to add to the test job in the CI pipeline |
| Definition of done | `output/[slug]-definition-of-done.md` | Coverage thresholds, report artifacts, gate conditions |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 4 | Threshold enforcement commands | Confirm threshold values match agreed targets from Stage 02 |

## Audit

| Check | Pass Condition |
|---|---|
| Coverlet configured | `coverlet.collector` is referenced in the test project |
| Thresholds match strategy | Gate values equal the targets agreed in `_config/test-strategy.md` |
| Report format specified | Coverage report format matches `_config/coverage-context.md` |
| CI integration documented | Steps are specific enough to add directly to the test job YAML |
| Definition of done complete | Threshold, artifact, and gate conditions are all stated |
