# Stage 02: Information Architecture and Shell Planning

Decide navigation placement, route structure, and layout nesting for new feature areas.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../01-current-state/output/baseline-inventory.md` | Full file | Know what exists |
| Config | `../../_config/domain-expansion.md` | Full file | New feature areas to place |
| Reference | `../../shared/shell-and-ia.md` | Full file | Shell conventions and layout rules |

## Process

1. Read baseline inventory — understand current sidebar groups and route patterns.
2. Read domain expansion scope — understand each new feature area's navigation needs.
3. Propose sidebar navigation additions. Consider: where do Assets, Inspections, Deficiencies land? New group? Under Governance? Standalone?
4. Propose route structure for all new feature areas. Follow the existing `/entity` → `/entity/{id}` pattern.
5. Decide layout nesting: do asset detail pages use a sub-layout (like SettingsLayout) or stay in MainLayout?
6. Identify shared UI patterns: entity detail shell, register grid, activity timeline, status badges.
7. **[Checkpoint]** — Present the proposed navigation map and route table. Get human approval before proceeding.
8. Write the IA plan to output.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 7 | Navigation map, route table, layout nesting plan, shared pattern list | Whether placement and grouping are correct |

## Audit

| Check | Pass Condition |
|-------|---------------|
| No orphan routes | Every proposed page has a sidebar or sub-nav entry |
| Consistent patterns | New routes follow existing `/entity` → `/entity/{id}` convention |
| Layout clarity | Each page's layout parent is explicitly named |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| IA plan | `output/ia-plan.md` | Navigation map, route table, layout decisions, shared patterns |
