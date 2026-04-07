# Closure Report: GAP-21 — CheckboxTemplate (Custom Checkbox Rendering)

**Closure Status:** Resolved
**Phase:** 3 — Advanced
**Pipeline note:** Reconstructed — code predates formal records
**Validated:** 2026-04-02

## Criteria Verification

| Criterion | Source location | Test name | Status |
|-----------|----------------|-----------|--------|
| `CheckboxTemplate` renders custom content instead of default checkbox | `MariloTreeView.razor.cs:529` — `if (CheckboxTemplate != null)` branch; `builder.AddContent(30, CheckboxTemplate(ctx))` | `TreeView_CheckboxTemplate_RendersCustomContent` | ✅ |
| `CheckboxContext.Checked` reflects node checked state | `MariloTreeView.razor.cs:531` — `Checked = checkState == true` | `TreeView_CheckboxTemplate_ProvidesCorrectContext` | ✅ |
| `CheckboxContext.Indeterminate` reflects partial-check state | `MariloTreeView.razor.cs:532` — `Indeterminate = checkState == null` | `TreeView_CheckboxTemplate_ProvidesCorrectContext` | ✅ |
| Default `<input type="checkbox">` renders when `CheckboxTemplate` is null | `MariloTreeView.razor.cs:537+` — else branch of `CheckboxTemplate != null` | `TreeView_CheckboxTemplate_DefaultCheckboxWhenNull` | ✅ |
| `CheckboxContext.Disabled` reflects `Disabled OR ReadOnly` | `MariloTreeView.razor.cs:533` — `Disabled = Disabled \|\| ReadOnly` | code inspection | ⚠️ |
| `CheckboxContext.OnChange` triggers `ToggleItemChecked` when value changes | `MariloTreeView.razor.cs:534` — `OnChange = (val) => { if (val != (checkState == true)) ToggleItemChecked(cbId); }` | code inspection | ⚠️ |

**Note on ⚠️ `CheckboxContext.Disabled`:** bUnit tests do not independently assert the `Disabled` property of the constructed `CheckboxContext` instance. The `Action<bool> OnChange` callback is a delegate captured in a closure at render time — it is not accessible via `GetAttribute` or standard bUnit markup queries. Verified by direct code inspection at `MariloTreeView.razor.cs:533`.

**Note on ⚠️ `CheckboxContext.OnChange`:** `OnChange` is declared as `Action<bool>` rather than `EventCallback<bool>`. Blazor does not automatically schedule a re-render when an `Action<bool>` is invoked. Custom controls driven by `ctx.OnChange` must call `StateHasChanged` themselves if their rendering depends on the change triggering a cycle independently of the tree's own re-render. The change-guard (`if (val != (checkState == true))`) prevents spurious double-toggles on mount. These behaviours are confirmed by code inspection only.

## Evidence

- **Source:** `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` (lines 529–558); `src/Marilo.Core/Models/TreeViewModels.cs` (lines 35–48, `CheckboxContext`)
- **Tests:** `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs` — 3 tests, all passing
- **Gap no longer present:** Yes — consumers can supply a `RenderFragment<CheckboxContext>` to replace the built-in checkbox with full access to logical state; default rendering is fully preserved when the parameter is null

## Enforcement Guardrails

- `CheckboxTemplate` must remain nullable (`RenderFragment<CheckboxContext>?`). The null fall-through to the default checkbox is the backward-compatibility guarantee. Do not make it non-nullable.
- `CheckboxContext.Disabled` must reflect `Disabled || ReadOnly`, not `Disabled` alone. ReadOnly trees must present their checkboxes as non-interactive to custom controls as well. Do not change to `Disabled` only.
- `OnChange` is `Action<bool>`, not `EventCallback<bool>`. This is a deliberate design decision — `EventCallback<bool>` would automatically trigger `StateHasChanged` on the tree for every custom checkbox interaction, which may be undesirable. Do not change to `EventCallback<bool>` without assessing the render-cycle implications for all custom checkbox consumers.
- The change-guard in `OnChange` (`if (val != (checkState == true))`) must be preserved to prevent double-toggle bugs in controls that fire change events on mount.

## Follow-up Tasks

- Consider adding dedicated bUnit tests for `CheckboxContext.Disabled` and `CheckboxContext.OnChange` in a future test-coverage pass. These would require either exposing the context via a test-specific render fragment that captures the `CheckboxContext` instance, or using a wrapper component approach.
