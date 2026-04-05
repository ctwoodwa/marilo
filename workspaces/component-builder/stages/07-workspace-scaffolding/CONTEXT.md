# Workspace Scaffolding

Scaffold all supporting workspaces, spec documentation, and registration needed for the new component to participate in the full ICM pipeline.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Stage 01 output | stages/01-discovery/output/component-requirements.md | Component name, category, complexity | Identity fields for templates |
| Stage 02 output | stages/02-api-design/output/api-design.md | Parameters, events, enums, CSS methods | Populate spec skeleton |
| Stage 03 output | stages/03-implementation/output/implementation-summary.md | File paths, source subfolder | Artifact path placeholders |
| Stage 06 output | stages/06-testing/output/test-summary.md | Test file path | Test path placeholder |
| Delivery template | /workspaces/Marilo/workspaces/shared/component-delivery-template.md | Full file | CDW scaffold source |
| Gap-analysis template | /workspaces/Marilo/workspaces/shared/component-gap-analysis-template.md | Full file | Gap workspace scaffold source |
| Spec template | /workspaces/Marilo/workspaces/shared/component-spec-template.md | Full file | Spec docs scaffold source |
| Workspace routing | /workspaces/Marilo/workspaces/shared/workspace-routing.md | Graduation criteria | Determine complexity tier |

## Process

1. **Extract identity fields** from Stage 01-06 outputs:
   - `component-name`: PascalCase name (from Stage 01)
   - `component-slug`: lowercase-hyphenated (derived from name)
   - `component-tag`: `Marilo{component-name}` (from Stage 03)
   - `category`: component category (from Stage 01)
   - `source-subfolder`: subfolder under Marilo.Components/ (from Stage 03)
   - `test-path`: subfolder under Marilo.Tests.Unit/ (from Stage 06)
   - `complexity-tier`: evaluate against workspace-routing.md graduation criteria
   - `active-phase`: `Phase 1 (initial build)`
   - `date`: today's date in ISO format
   - `description`: one-line purpose (from Stage 01)

2. **Scaffold the delivery workspace** at `workspaces/{component-slug}-delivery/`:
   - Read `component-delivery-template.md`
   - Create all files from the template, replacing all `{{placeholder}}` tokens
   - Create `.gitkeep` files in all `output/` directories
   - Verify: all `{{` patterns resolved, no template artifacts remain

3. **Scaffold the gap-analysis workspace** at `workspaces/{component-slug}-gap-analysis/`:
   - Read `component-gap-analysis-template.md`
   - Create all files from the template, replacing all `{{placeholder}}` tokens
   - Copy shared files from `gap-analysis-resolution/shared/` (adapting ID prefixes)
   - Copy stage CONTEXT.md files from `gap-analysis-resolution/stages/*/CONTEXT.md` (adapting paths)
   - Create `.gitkeep` files in all `output/` directories
   - Verify: all `{{` patterns resolved

4. **Scaffold spec documentation** at `docs/component-specs/{component-slug}/`:
   - Read `component-spec-template.md`
   - Create all files from the template, replacing placeholders
   - Populate the overview.md parameter and event tables from Stage 02 API design output
   - Populate accessibility.md from Stage 01 discovery accessibility section
   - Create toc.yml with all spec files listed

5. **Register the component in shared workspace routing:**
   - Read `workspaces/shared/workspace-routing.md`
   - Add the new component to the routing quick-reference table if not already present
   - Update `workspaces/shared/component-delivery-template.md` is not needed (it's a template)

6. **Update coverage summary in gap-analysis-resolution:**
   - Read `workspaces/gap-analysis-resolution/_config/coverage-summary.md`
   - Add a row for the new component with initial zero counts

7. **[CHECKPOINT]** Present scaffolding summary to user for review.

8. Run the Audit checklist.

9. Write output/workspace-scaffolding-summary.md.

## Checkpoint

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| Step 7 | List of all created directories and files, placeholder resolution report, any issues found | Approve scaffolding, request changes, or re-run with corrections |

## Audit

| Check | Pass Condition |
|-------|----------------|
| Delivery workspace complete | All files from template exist with correct content |
| Gap-analysis workspace complete | All files from template exist, all 6 stage CONTEXT.md files present |
| Spec documentation complete | overview.md, appearance.md, events.md, accessibility.md, toc.yml all exist |
| No unresolved placeholders | `grep -r '{{' workspaces/{slug}-delivery/ workspaces/{slug}-gap-analysis/ docs/component-specs/{slug}/` returns zero matches |
| Delivery workspace CLAUDE.md under 800 tokens | Word count check |
| Gap-analysis workspace CLAUDE.md under 800 tokens | Word count check |
| Workspace routing updated | New component appears in routing table |
| Coverage summary updated | New component row exists in gap-analysis-resolution coverage |
| All output directories have .gitkeep | Find check in both workspaces |
| Cross-references valid | Delivery workspace gap link points to correct gap-analysis workspace; gap-analysis delivery link points to correct delivery workspace |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Scaffolding summary | output/workspace-scaffolding-summary.md | Structured report |
| Delivery workspace | /workspaces/Marilo/workspaces/{component-slug}-delivery/ | Full workspace |
| Gap-analysis workspace | /workspaces/Marilo/workspaces/{component-slug}-gap-analysis/ | Full workspace |
| Spec documentation | /workspaces/Marilo/docs/component-specs/{component-slug}/ | Spec files |
