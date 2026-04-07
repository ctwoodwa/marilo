# Stage 01 - Build Audit

Document every build step in the solution in the correct execution order. The output of this
stage is the complete inventory that Stage 03 uses to generate accurate pipeline YAML.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Pipeline config | `../../_config/pipeline-context.md` | Solution path, .NET version, npm flag | Scope definition |
| Solution file | `{{SOLUTION_PATH}}` | All projects | Project list |
| Directory.Build.targets | `{{SOLUTION_PATH}}/Directory.Build.targets` | Custom targets | Reveals pre-build steps |
| Web project file(s) | `{{SOLUTION_PATH}}/*Web*/*.csproj` | Build targets | npm/SCSS build steps |
| Test project file | `{{SOLUTION_PATH}}/*Tests*/*.csproj` | Test framework config | Test runner and coverage tooling |
| AppHost project | `{{SOLUTION_PATH}}/*AppHost*/*.csproj` | Aspire config | Orchestration requirements |

## Process

1. Read `_config/pipeline-context.md` and confirm all fields are filled.
2. Walk the solution and list every project: name, type (API, Web, Library, Test, AppHost), and output type.
3. Read `Directory.Build.targets` and any `.csproj` files with custom `<Target>` blocks. Record each custom step with its `BeforeTargets`/`AfterTargets` ordering.
4. Identify the npm dependency: which projects run `npm install` and `npm run build:*` as pre-build steps.
5. Identify Docker targets: which projects have `DockerDefaultTargetOS` or Dockerfile references.
6. Identify the test runner configuration: framework, runner command, coverage tool and report format.
7. Produce the ordered build steps table and dependency graph in `_config/build-steps.md`.
8. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Project inventory | `output/[slug]-project-inventory.md` | Table: project, type, output, custom build steps |
| Ordered build steps | `_config/build-steps.md` | Filled table and dependency graph |
| Build audit summary | `output/[slug]-build-audit.md` | Key findings: npm projects, Docker targets, test config |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 4 | npm-dependent projects with their exact commands | Confirm command strings before they go into YAML |
| 7 | Complete ordered build steps table and dependency graph | Sign off before Stage 02 begins |

## Audit

| Check | Pass Condition |
|---|---|
| All projects listed | Solution project count matches project inventory row count |
| Custom build steps identified | Every `<Target>` in `.targets` and `.csproj` files is in the table |
| npm ordering captured | npm steps appear before their dependent dotnet steps |
| Test config complete | Test framework, runner command, and coverage tool are all recorded |
