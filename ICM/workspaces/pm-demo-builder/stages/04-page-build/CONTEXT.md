# Stage 04: Page Build

Implement Razor pages, layouts, and components for the PM demo.

## Inputs

| Source | File/Location | Section/Scope | Why |
|--------|--------------|---------------|-----|
| Previous stage | `../03-domain-modeling/output/domain-models.md` | Full file | Entity shapes and service contracts |
| Previous stage | `../03-domain-modeling/output/dynamic-form-schema.md` | Full file | Form rendering strategy |
| Previous stage | `../02-ia-and-shell/output/ia-plan.md` | Route table, layout decisions | Where pages go and how they nest |
| Reference | `../../shared/component-inventory.md` | Full file | Available Marilo components |
| Reference | `../../shared/implementation-guardrails.md` | Full file | Coding rules |

## Process

1. Read the IA plan route table — this is the build list.
2. Read domain models — these define the data each page works with.
3. Build pages in priority order. For each page:
   a. Create the `.razor` file with `@page` directive at the correct route.
   b. Set `@layout` to the appropriate layout (MainLayout or SettingsLayout).
   c. Inject the relevant service interface.
   d. Implement the page using Marilo-native components from the inventory.
   e. Use `MariloField`, `MariloForm`, and existing input components — do not create custom inputs unless the inventory confirms a gap.
4. Build shared components identified in the IA plan (e.g., EntityDetailShell, ActivityTimeline, DynamicForm).
5. Build the `DynamicForm.razor` component if inspections/deficiencies are in scope for this pass.
6. **[Checkpoint]** — Demo the built pages. Get human feedback on UX and completeness.
7. Write a build manifest listing every file created or modified.

## Checkpoints

| After Step | Agent Presents | Human Decides |
|------------|---------------|---------------|
| 6 | Built pages running in the demo | UX feedback, missing features, polish requests |

## Audit

| Check | Pass Condition |
|-------|---------------|
| No MariloAppShell duplication | Only MainLayout renders MariloAppShell |
| Service injection | Every page injects its service; no inline data |
| IDisposable | Every component subscribing to events implements IDisposable |
| Marilo-native inputs | No non-Marilo input components used |
| Route reachability | Every page is navigable from sidebar or sub-nav |

## Outputs

| Artifact | Location | Format |
|----------|----------|--------|
| Build manifest | `output/build-manifest.md` | Table: file path, action (created/modified), purpose |
