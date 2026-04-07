# Stage 05 - Release Checklist

Produce the pre-release gate checklist, rollback procedure, and on-call runbook.
These artifacts are the operational contract for every production deployment.

## Inputs

| Source | File/Location | Section/Scope | Why |
|---|---|---|---|
| Environment matrix | `../../_config/environment-matrix.md` | Full matrix | Defines what must be verified per env |
| Pipeline setup guide | `../03-pipeline-scaffold/output/[slug]-pipeline-setup.md` | Setup steps | Informs the deployment runbook |
| Aspire manifest guide | `../04-environment-strategy/output/[slug]-aspire-manifest.md` | Deployment process | Runbook input |

## Process

1. Define the pre-release gate: the ordered checklist a release engineer completes before approving a prod deployment.
2. Define the rollback procedure: step-by-step instructions to revert a bad production release.
3. Define the on-call runbook: what to check first when a deployment causes an incident.
4. Define the smoke test suite: the minimum set of manual or automated checks to confirm a deployment succeeded.
5. Save outputs.

## Outputs

| Artifact | Location | Format |
|---|---|---|
| Pre-release checklist | `output/[slug]-pre-release-checklist.md` | Ordered checklist with pass criteria |
| Rollback procedure | `output/[slug]-rollback.md` | Numbered steps with commands |
| On-call runbook | `output/[slug]-oncall-runbook.md` | Triage steps, key dashboards, escalation path |
| Smoke test suite | `output/[slug]-smoke-tests.md` | Checklist of endpoints/features to verify post-deploy |

## Checkpoints

| After Step | Agent Presents | Human Decides |
|---|---|---|
| 3 | Pre-release checklist and rollback procedure | Approve before runbook is finalized |

## Audit

| Check | Pass Condition |
|---|---|
| Pre-release checklist is ordered | Steps are numbered with an explicit pass condition per item |
| Rollback procedure has commands | Every rollback step includes the exact command or action to take |
| On-call runbook links dashboards | Key observability links (OTEL, logs) are referenced |
| Smoke tests cover critical paths | Auth flow, key API endpoints, and UI home page are all included |
