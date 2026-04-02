# Closure Report: GAP-17 — Disabled / ReadOnly

**Closure Status:** Resolved
**Phase:** 2 — Enhanced
**Pipeline note:** Reconstructed — code predates formal records
**Validated:** 2026-04-02

## Criteria Verification

| Criterion | Source location | Test name | Status |
|-----------|----------------|-----------|--------|
| `Disabled=true` sets `aria-disabled="true"` on root element | `MariloTreeView.razor:8` — `aria-disabled="@(Disabled ? "true" : null)"` | `TreeView_Disabled_SetsAriaDisabledOnRoot` | ✅ |
| `Disabled=false` omits `aria-disabled` attribute | `MariloTreeView.razor:8` — attribute evaluates to `null` when `Disabled` is `false` | `TreeView_Disabled_False_NoAriaDisabled` | ✅ |
| `Disabled=true` prevents expand/collapse via toggle | `MariloTreeView.razor.cs:ToggleNodeAsync` (line 635) — `if (Disabled \|\| ReadOnly) return;` | `TreeView_Disabled_PreventsExpandCollapseViaToggle` | ✅ |
| `Disabled=true` prevents selection | `MariloTreeView.razor.cs:SelectItem` (line 302) — `if (Disabled \|\| ReadOnly) return;` | `TreeView_Disabled_PreventsSelection` | ✅ |
| `Disabled=true` prevents checkbox changes | `MariloTreeView.razor.cs:ToggleItemChecked` (line 254) — `if (id == null \|\| Disabled \|\| ReadOnly) return;` | `TreeView_Disabled_PreventsCheckboxChanges` | ✅ |
| `Disabled=true` prevents keyboard navigation | `MariloTreeView.razor.cs:HandleKeyDown` (line 705) — `if (Disabled) return;` (ReadOnly intentionally omitted) | `TreeView_Disabled_PreventsKeyboardNavigation` | ✅ |
| `ReadOnly=true` prevents checkbox changes | `MariloTreeView.razor.cs:ToggleItemChecked` (line 254) — `if (id == null \|\| Disabled \|\| ReadOnly) return;` | `TreeView_ReadOnly_PreventsCheckboxChanges` | ✅ |
| `ReadOnly=true` allows keyboard focus movement | `MariloTreeView.razor.cs:HandleKeyDown` (line 705) — guard checks only `Disabled`; `ReadOnly` not included | `TreeView_ReadOnly_AllowsKeyboardFocusMovement` | ✅ |
| Both `Disabled` and `ReadOnly` default to `false` | `MariloTreeView.razor.cs` (lines 115, 118) — `bool` parameters with no explicit initialiser; C# default is `false` | `TreeView_BothDefaultToFalse` | ✅ |

## Evidence

- **Source:** `Navigation/MariloTreeView.razor.cs` (interaction guards on lines 254, 302, 597, 599, 635, 705, 784) + `Navigation/MariloTreeView.razor` (ARIA and input disabled attributes on lines 8, 519, 548)
- **Tests:** `TreeViewTests.cs` — 9 tests, all passing
- **Gap no longer present:** Yes — `Disabled` blocks all interaction and emits `aria-disabled`; `ReadOnly` blocks all mutations while preserving keyboard navigation; both parameters default to `false` with no breaking change for existing consumers

## Enforcement Guardrails

- 9 bUnit tests in `TreeViewTests.cs` cover every resolved criterion; any regression in the per-interaction guards, `aria-disabled` emission, or default parameter values will produce a test failure in CI
- `HandleKeyDown` intentionally omits `ReadOnly` from its early-return guard — this is the deliberate design point that permits keyboard navigation in read-only mode. Code review should treat any addition of `ReadOnly` to that guard as a breaking change requiring explicit sign-off
- `ExpandOnClick` (line 501) and drag-and-drop (line 480) render guards check only `!Disabled`, not `ReadOnly`. A `ReadOnly` tree with `ExpandOnClick=true` or `EnableDragDrop=true` will therefore still allow those interactions; callers enabling those features on a `ReadOnly` tree must handle the resulting events in their own callbacks if they wish to reject the mutations. This is a known design point.

## Follow-up Tasks

Flag the `ReadOnly` / `ExpandOnClick` interaction (and `ReadOnly` / `EnableDragDrop` interaction) for a future explicit decision: either add `ReadOnly` to those guards and document the breaking change, or add an explicit test confirming the "allowed under ReadOnly" behaviour as intentional.
