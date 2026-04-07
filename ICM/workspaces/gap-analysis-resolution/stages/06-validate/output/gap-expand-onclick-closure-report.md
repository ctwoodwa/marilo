# Closure Report: GAP-12 — ExpandOnClick / ExpandOnDoubleClick

**Closure Status:** Resolved
**Phase:** 2 — Enhanced
**Pipeline note:** Reconstructed — code predates formal records
**Validated:** 2026-04-02

## Criteria Verification

| Criterion | Source location | Test name | Status |
|-----------|----------------|-----------|--------|
| `ExpandOnClick=true` attaches `onclick` to parent node header and toggles expand | MariloTreeView.razor.cs:80, 504 | `TreeView_ExpandOnClick_True_TogglesExpandOnHeaderClick` | ✅ |
| `ExpandOnClick=false` does not attach `onclick` handler | MariloTreeView.razor.cs:504 | `TreeView_ExpandOnClick_False_DoesNotAttachOnClickToHeader` | ✅ |
| `ExpandOnDoubleClick=true` attaches `ondblclick` and toggles expand | MariloTreeView.razor.cs:83, 506 | `TreeView_ExpandOnDoubleClick_True_ExpandsOnDoubleClick` | ✅ |
| `ExpandOnDoubleClick=true` with `AllowEditing=true` does not attach `ondblclick` | MariloTreeView.razor.cs:506 | `TreeView_ExpandOnDoubleClick_SuppressedWhenAllowEditing` | ✅ |
| `Disabled=true` prevents handler attachment regardless of parameter value | MariloTreeView.razor.cs:500 | `TreeView_ExpandOnClick_Disabled_PreventsHandlerAttachment` | ✅ |
| Both parameters default to `false` | MariloTreeView.razor.cs:80, 83 | `TreeView_BothDefaultToFalse` | ✅ |

## Evidence

- **Source:** Navigation/MariloTreeView.razor.cs
- **Tests:** TreeViewTests.cs — 6 tests covering Gap 12, all passing
- **Gap no longer present:** Yes — `ExpandOnClick` and `ExpandOnDoubleClick` parameters exist and emit event attributes conditionally at render time, gated by `hasKids`, `!Disabled`, and (for double-click) `!AllowEditing`

## Enforcement Guardrails

- Code review: verify the `hasKids && !Disabled` guard remains in place before any `ExpandOnClick`/`ExpandOnDoubleClick` attribute emission — removing it would attach handlers to leaf nodes and disabled trees
- Code review: verify `ExpandOnDoubleClick && !AllowEditing` remains a single compound condition — splitting or reordering it risks re-introducing the double-click conflict with inline editing

## Follow-up Tasks

None
