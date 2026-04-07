# Stage 03 - Pipeline Scaffold

Generate complete, runnable pipeline YAML from the approved blueprint.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Pipeline blueprint | `../02-pipeline-design/output/[slug]-pipeline-blueprint.md` | Full blueprint | Primary source for YAML generation |
| Ordered build steps | `../../_config/build-steps.md` | Exact commands | Command strings go directly into YAML steps |
| Pipeline config | `../../_config/pipeline-context.md` | Platform, .NET version, Node version | Tool version pins |
| Platform reference | `../../shared/[platform]-reference.md` | Syntax patterns | YAML authoring guide |

## Process

1. Open `shared/[platform]-reference.md` for the configured platform.
2. Generate the CI workflow file: build job, test job with coverage upload, optional security scan job.
3. Generate the CD workflow file(s): one deploy job per environment, each requiring the test job.
4. For any step that requires a secret, use the correct platform syntax for secret references.
5. Add a TODO comment for any step that requires environment-specific manual configuration (e.g., deployment target URL).
6. Validate the YAML structure — no syntax errors, no duplicate job IDs, correct dependency syntax.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| CI workflow | `output/[slug]-ci.yml` | Pipeline YAML |
| CD workflow(s) | `output/[slug]-cd-[env].yml` | Pipeline YAML (one per environment) |
| Pipeline setup guide | `output/[slug]-pipeline-setup.md` | Steps to install the pipeline: where to put files, secrets to register, first-run notes |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | CI workflow YAML | Approve before CD workflows are generated |
| 6 | All generated YAML files | Final review; list all TODOs that need human action |

## Audit

| Check | Pass Condition |
|---|---|
| No literal secrets | All secret values referenced by name only |
| npm before dotnet | npm install and build steps precede dotnet build in any job that includes both |
| Test gates deploy | CD workflow jobs declare CI test job as a dependency |
| TODOs documented | Every TODO comment has a specific instruction, not just "fill this in" |
| YAML is valid | No syntax errors (verify structure manually or with a linter) |
