# Implementation Log: GAP-22 — Node Editing (Inline Rename)

**Scope:** single
**Phase:** 3 — Complex
**Status:** Reconstructed — implementation predates this log; tests written 2026-04-02

## Summary

Gap 22 introduced first-class inline rename of tree nodes via the `AllowEditing` parameter on `MariloTreeView`. When enabled, a user can activate edit mode on any node's title by double-clicking it or pressing F2 while that node has keyboard focus. During edit, the title `<span>` is replaced in-place by an `<input type="text">` pre-filled with the node's current label. Committing (Enter or blur) fires `OnItemEdit` with the node ID and trimmed new text; empty text is silently discarded. Cancelling (Escape) restores the original label without firing the event. `ExpandOnDoubleClick` is unconditionally suppressed when `AllowEditing=true` because double-click is reserved for edit activation; this is an intentional and silent trade-off. `Disabled` and `ReadOnly` each independently prevent activation via their own render guards.

## Source Files (read-only — no changes made)

| File | Relevant section |
|------|-----------------|
| `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` | `AllowEditing` parameter (line 133), `OnItemEdit` parameter (line 136) |
| `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` | Private editing state: `_editingNodeId` (line 22), `_editingText` (line 23) |
| `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` | Edit input render path (lines 571–587): `<input type="text" class="mar-tree-item__edit-input">` with `oninput`, `onblur`, `onkeydown`, `autofocus` |
| `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` | Double-click activation guard on title span (line 599): `if (AllowEditing && !Disabled && !ReadOnly)` |
| `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` | `ExpandOnDoubleClick` suppression (line 506): `if (ExpandOnDoubleClick && !AllowEditing)` |
| `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` | F2 activation in `HandleKeyDown` (line 782–790): guard `AllowEditing && !ReadOnly && _focusedNodeId != null` |
| `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` | Escape in `HandleKeyDown` (line 793–795): calls `CancelEdit` when `_editingNodeId != null` |
| `src/Marilo.Components/Navigation/MariloTreeView.razor.cs` | `StartEdit` (line 966), `CommitEdit` (line 973), `CancelEdit` (line 985), `IsEditing` (line 992) |
| `src/Marilo.Core/Models/TreeViewModels.cs` | `TreeItemEditEventArgs` model: `ItemId` (string), `NewText` (string) |

## Tests Written

All tests appended to: `tests/Marilo.Tests.Unit/P2Enhancements/TreeViewTests.cs`

| Test name | Criterion covered |
|-----------|------------------|
| `TreeView_NodeEditing_AllowEditingDefaultsFalse` | `AllowEditing` defaults to `false`; title span rendered, no edit input, no `ondblclick` on title |
| `TreeView_NodeEditing_DoubleClickActivatesEditMode` | Double-click on title activates edit mode when `AllowEditing=true` |
| `TreeView_NodeEditing_EditInputReplacesTitle` | During edit, `<input>` is present and pre-filled with current label; title `<span>` is absent |
| `TreeView_NodeEditing_EnterCommitsEdit` | Enter key commits edit and fires `OnItemEdit` with `ItemId` and `NewText`; edit input removed |
| `TreeView_NodeEditing_EscapeCancelsEdit` | Escape cancels edit; `OnItemEdit` not fired; original label restored |
| `TreeView_NodeEditing_EmptyTextDoesNotFireCallback` | Whitespace-only text on commit does not fire `OnItemEdit`; edit mode exits cleanly |
| `TreeView_NodeEditing_BlurCommitsEdit` | Blur commits edit and fires `OnItemEdit` |
| `TreeView_NodeEditing_AllowEditingFalse_PreventsActivation` | `AllowEditing=false` means no `ondblclick` on title; no edit input present |
| `TreeView_NodeEditing_DisabledPreventsActivation` | `Disabled=true` prevents `ondblclick` handler on title; no edit input |
| `TreeView_NodeEditing_ReadOnlyPreventsActivation` | `ReadOnly=true` prevents `ondblclick` handler on title; no edit input |
| `TreeView_NodeEditing_F2ActivatesEditMode` | F2 key activates edit on focused node when `AllowEditing=true` |
| `TreeView_NodeEditing_SuppressesExpandOnDoubleClick` | `ExpandOnDoubleClick=true` + `AllowEditing=true` → no `ondblclick` on header; children not expanded |

**Total Gap 22 tests:** 12
**Test runner result:** All 12 pass; 67/67 total TreeViewTests pass.

## bUnit Technique Notes

- Activation via `TriggerEvent("ondblclick", new MouseEventArgs())` rather than `.DoubleClick()`. The Blazor render builder attaches the handler under the `ondblclick` event name directly; `TriggerEvent` dispatches to the registered handler without requiring an actual DOM double-click simulation. `.DoubleClick()` was not required for the tests to work and `TriggerEvent` is more explicit about which Blazor event binding is exercised.
- `oninput` triggered with `TriggerEvent("oninput", new ChangeEventArgs { Value = "..." })`. This updates `_editingText` via the lambda registered at render time (`e => _editingText = e.Value?.ToString() ?? ""`).
- `onkeydown` on the edit input triggered with `TriggerEvent("onkeydown", new KeyboardEventArgs { Key = "Enter|Escape" })`. The Enter path calls `CommitEdit(editId)` and Escape calls `CancelEdit()`.
- Escape via `HandleKeyDown` triggered with `.KeyDown(new KeyboardEventArgs { Key = "Escape" })` on the `[role='tree']` element. This exercises the HandleKeyDown code path rather than the input's inline `onkeydown`, which is the canonical Escape path when `_editingNodeId != null`.
- `onblur` triggered with `TriggerEvent("onblur", new FocusEventArgs())`.
- `using Microsoft.AspNetCore.Components;` added to the test file to resolve `ChangeEventArgs`.

## ExpandOnDoubleClick Suppression (explicit note)

The resolution record (RES-TREEVIEW-022, line 506 of source) documents that `ExpandOnDoubleClick` is suppressed when `AllowEditing=true` via the render guard:

```csharp
if (ExpandOnDoubleClick && !AllowEditing)
    builder.AddAttribute(21, "ondblclick", ...ToggleNodeAsync...);
```

This means the two features are **mutually exclusive on the double-click gesture**. Edit activation takes unconditional precedence. No warning or error is produced when both parameters are set simultaneously; the suppression is silent. The test `TreeView_NodeEditing_SuppressesExpandOnDoubleClick` verifies both the absence of the `ondblclick` attribute on the header div and the consequent non-expansion of children.

## Phase Exit Criteria

| # | Criterion | Test | Status |
|---|-----------|------|--------|
| 1 | Double-click on title activates edit mode when `AllowEditing=true` | `TreeView_NodeEditing_DoubleClickActivatesEditMode` | ✅ passing |
| 2 | F2 key activates edit mode on focused node when `AllowEditing=true` | `TreeView_NodeEditing_F2ActivatesEditMode` | ✅ passing |
| 3 | Edit input replaces title span during editing | `TreeView_NodeEditing_EditInputReplacesTitle` | ✅ passing |
| 4 | Enter key commits edit and fires `OnItemEdit` with new text | `TreeView_NodeEditing_EnterCommitsEdit` | ✅ passing |
| 5 | Escape key cancels edit and restores original text | `TreeView_NodeEditing_EscapeCancelsEdit` | ✅ passing |
| 6 | Blur commits edit | `TreeView_NodeEditing_BlurCommitsEdit` | ✅ passing |
| 7 | Empty text after trim does not fire `OnItemEdit` | `TreeView_NodeEditing_EmptyTextDoesNotFireCallback` | ✅ passing |
| 8 | `AllowEditing=false` prevents edit activation | `TreeView_NodeEditing_AllowEditingFalse_PreventsActivation` | ✅ passing |
| 9 | `Disabled=true` prevents edit activation | `TreeView_NodeEditing_DisabledPreventsActivation` | ✅ passing |
| 10 | `ReadOnly=true` prevents edit activation | `TreeView_NodeEditing_ReadOnlyPreventsActivation` | ✅ passing |
| 11 | `ExpandOnDoubleClick` is suppressed when `AllowEditing=true` | `TreeView_NodeEditing_SuppressesExpandOnDoubleClick` | ✅ passing |
| 12 | `AllowEditing` defaults to `false` | `TreeView_NodeEditing_AllowEditingDefaultsFalse` | ✅ passing |

**All 12 success criteria satisfied. No ⚠️ criteria.**

## Edge Cases Observed During Testing

1. **`Disabled=true` does not suppress the title span itself** — the title `<span class="mar-tree-item__title">` remains in the DOM when `Disabled=true`; only the `onclick` and `ondblclick` attributes are omitted. The test for `DisabledPreventsActivation` therefore uses `FindAll` with a null-check on `ondblclick` rather than asserting the span is absent.

2. **Escape handling is two-path** — Escape during edit can arrive via (a) the tree's `HandleKeyDown` (`[role='tree']` element), which checks `_editingNodeId != null` before calling `CancelEdit`, or (b) the edit input's own `onkeydown` handler. The tests exercise path (a) which is the dominant path when the tree retains keyboard focus. Both paths call the same `CancelEdit()` method so the observable outcome is identical.

3. **`autofocus` is a static attribute** — the edit input carries `autofocus` as a static Blazor attribute. bUnit does not simulate browser focus events from `autofocus`; tests verify presence of the edit input element rather than any focus state.

4. **`CommitEdit` is `async void`** — the implementation signature is `private async void CommitEdit(string nodeId)`. When triggered synchronously via `TriggerEvent("onkeydown", ...)`, the async continuation (which calls `StateHasChanged`) runs within the same bUnit synchronisation context, so the markup is up to date immediately after `TriggerEvent` returns. No `WaitForState` or `InvokeAsync` was needed.
