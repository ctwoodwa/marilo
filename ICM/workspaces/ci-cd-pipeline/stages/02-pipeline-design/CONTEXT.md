# Stage 02 - Pipeline Design

Design the job structure, trigger strategy, and environment routing for the pipeline.
Produces the blueprint that Stage 03 translates into YAML.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Pipeline config | `../../_config/pipeline-context.md` | Platform, environments, deploy branch map | Drives all design decisions |
| Build audit summary | `../01-build-audit/output/[slug]-build-audit.md` | Key findings | Informs job separation and ordering |
| Ordered build steps | `../../_config/build-steps.md` | Full table | Source for job step content |

## Process

1. Confirm the CI/CD platform from `_config/pipeline-context.md`. Select the matching reference from `shared/`.
2. Define pipeline triggers: PR validation, push-to-branch deploys, tag-based prod release.
3. Design the job graph: CI jobs (build, test, scan) and CD jobs (deploy per environment). Show dependencies.
4. Assign build steps from `_config/build-steps.md` to specific jobs.
5. Define the artifact strategy: what is built, what is published, and how artifacts flow between jobs.
6. Define the secret and variable strategy: which secrets are needed per job and where they come from.
7. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Pipeline blueprint | `output/[slug]-pipeline-blueprint.md` | Job graph (ASCII), triggers, step assignments, artifact flow |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Job graph and trigger list | Approve structure before step assignment |
| 7 | Complete pipeline blueprint | Sign off before Stage 03 generates YAML |

## Audit

| Check | Pass Condition |
|---|---|
| All build steps assigned | Every step from `_config/build-steps.md` appears in a job |
| Test gates deploy | Every deploy job has the test job as a required dependency |
| Secrets documented | Every required secret is named in the blueprint |
| Triggers defined | PR, push, and release triggers are all specified |
