# Scaffolding Checklist

Use this checklist to verify all artifacts were created correctly after Stage 07 runs.

## Delivery Workspace

- [ ] `workspaces/{slug}-delivery/CLAUDE.md` exists and is under 800 tokens
- [ ] `workspaces/{slug}-delivery/CONTEXT.md` exists with correct routing table
- [ ] `workspaces/{slug}-delivery/_config/delivery-context.md` exists with all fields populated
- [ ] `workspaces/{slug}-delivery/_status/workspace-status.md` exists with current date
- [ ] `workspaces/{slug}-delivery/shared/spec-coverage-format.md` exists with correct ID prefix
- [ ] `workspaces/{slug}-delivery/stages/01-spec-review/CONTEXT.md` exists with correct paths
- [ ] `workspaces/{slug}-delivery/stages/02-example-ux/CONTEXT.md` exists with correct paths
- [ ] `workspaces/{slug}-delivery/stages/02-example-ux/shared/demo-scenario-format.md` exists
- [ ] `workspaces/{slug}-delivery/stages/03-sync-check/CONTEXT.md` exists with correct paths
- [ ] `workspaces/{slug}-delivery/stages/03-sync-check/shared/delivery-checklist.md` exists
- [ ] All `stages/*/output/` directories contain `.gitkeep`

## Gap-Analysis Workspace

- [ ] `workspaces/{slug}-gap-analysis/CLAUDE.md` exists and is under 800 tokens
- [ ] `workspaces/{slug}-gap-analysis/CONTEXT.md` exists with correct routing table
- [ ] `workspaces/{slug}-gap-analysis/_config/gap-context.md` exists with all fields populated
- [ ] `workspaces/{slug}-gap-analysis/_config/coverage-summary.md` exists
- [ ] `workspaces/{slug}-gap-analysis/_status/workspace-status.md` exists with current date
- [ ] `workspaces/{slug}-gap-analysis/setup/questionnaire.md` exists
- [ ] All 6 stage CONTEXT.md files exist with correct paths
- [ ] `workspaces/{slug}-gap-analysis/shared/` contains all 5 shared files
- [ ] All `stages/*/output/` directories contain `.gitkeep`

## Spec Documentation

- [ ] `docs/component-specs/{slug}/overview.md` exists with parameter table from API design
- [ ] `docs/component-specs/{slug}/appearance.md` exists
- [ ] `docs/component-specs/{slug}/events.md` exists with event list from API design
- [ ] `docs/component-specs/{slug}/accessibility.md` exists with ARIA roles from discovery
- [ ] `docs/component-specs/{slug}/toc.yml` exists listing all spec files

## Registration

- [ ] Component appears in `workspaces/shared/workspace-routing.md` routing table
- [ ] Component row added to `gap-analysis-resolution/_config/coverage-summary.md`

## Cross-References

- [ ] Delivery workspace gap link points to `{slug}-gap-analysis`
- [ ] Gap-analysis workspace delivery link points to `{slug}-delivery`
- [ ] Delivery spec path matches `docs/component-specs/{slug}/`
- [ ] Both workspaces reference correct source and test paths
- [ ] Zero `{{` placeholder patterns remain in any scaffolded file
