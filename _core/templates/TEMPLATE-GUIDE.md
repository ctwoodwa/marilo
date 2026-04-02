# Template Guide

How to use the workspace templates in `_core/templates/`.

## Using component-delivery-workspace/

Instantiate this template when:
- A component has more than one ICM phase of gap work, OR
- Its demo page needs multiple distinct feature-area sections, OR
- Its API spec has more than 20 parameters.

### Placeholder replacement checklist

| Placeholder | Description | Example |
|-------------|-------------|---------|
| `{{COMPONENT_NAME}}` | Full display name | TreeView |
| `{{COMPONENT_SLUG}}` | Lowercase-hyphenated | treeview |
| `{{ACTIVE_PHASE}}` | Current gap phase | Phase 3 |
| `{{SPEC_PATH}}` | Path to docs/component-specs/[component]/ | /workspaces/Marilo/docs/component-specs/treeview/ |
| `{{DEMO_PATH}}` | Path to samples/Marilo.Demo/Pages/Components/[page] | /workspaces/Marilo/samples/Marilo.Demo/Pages/Components/TreeView/ |
| `{{SOURCE_PATH}}` | Path to src/Marilo.Components/[component]/ | /workspaces/Marilo/src/Marilo.Components/Navigation/ |
| `{{TEST_PATH}}` | Path to tests/ for this component | /workspaces/Marilo/tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs |
| `{{GAP_WORKSPACE_PATH}}` | Path to this component's gap workspace | /workspaces/Marilo/workspaces/gap-analysis-resolution |
| `{{SPEC_VERSION}}` | Current spec document version or "unversioned" | unversioned |
| `{{DEMO_PAGES}}` | Comma-separated list of demo .razor files | TreeView/Overview.razor |
| Dates (`{{LAST_*_DATE}}`) | Leave as placeholder -- agent fills these on first run | -- |

### Complexity routing

- **Simple / single-phase component:** use global gap-analysis-resolution only.
- **Complex / multi-phase component:** instantiate this CDW + a component-specific gap workspace (copy gap-analysis-resolution template).
