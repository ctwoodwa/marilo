# Resolution Records: MariloTreeView — Node Editing (Inline Rename)

## Summary

Gap 22 covers inline rename of tree nodes via `AllowEditing`. When enabled, a user can activate edit mode on the focused node's title by double-clicking or pressing F2; the title span is replaced with a text input that commits on Enter or blur and cancels on Escape. `OnItemEdit` fires with the new text on commit. This record is reconstructed retroactively; the implementation predates the record.

---

### RES-TREEVIEW-022: AllowEditing parameter and inline rename workflow

**Resolves:** GAP-22 (node-editing — inline rename via AllowEditing)
**Status:** Reconstructed — implementation predates this record

#### Problem Statement

MariloTreeView node labels were read-only. Consumers managing editable data sets (file trees, outline editors, task lists) needed a first-class inline rename interaction without leaving the tree. Requirements were:

1. Edit mode must be activatable by keyboard (F2 on the focused node) and by pointer (double-click on the title).
2. During edit, the title span must be replaced by a text input pre-filled with the current label.
3. Committing (Enter or blur) must fire an `OnItemEdit` event carrying the node ID and new text; empty text after trimming must be silently discarded.
4. Cancelling (Escape) must restore the original label without firing the event.
5. Double-click-to-expand (`ExpandOnDoubleClick`) must be suppressed when editing is enabled, because double-click is already claimed by edit activation.
6. Disabled and ReadOnly states must prevent edit activation.
7. The feature must default to off (`AllowEditing = false`) to avoid breaking existing consumers.

#### Options Considered

**Option A (Selected): Opt-in boolean parameter with component-level editing state, render-time swap, and keyboard integration**

- Approach: Add `AllowEditing` (bool, default false) and `OnItemEdit` (`EventCallback<TreeItemEditEventArgs>`) parameters. Track active editing with two private fields: `_editingNodeId` (string?) and `_editingText` (string?). During `RenderNodes`, when `_editingNodeId` matches the current node, emit `<input type="text">` with `oninput`, `onblur`, `onkeydown`, and `autofocus` in place of the title `<span>`. Wire the double-click activation guard into the existing `ondblclick` attribute emission path, and handle F2 and Escape inside the existing `HandleKeyDown` keyboard handler.
- `TreeItemEditEventArgs` carries `ItemId` (string) and `NewText` (string).
- `CommitEdit` trims `_editingText`; if non-empty it invokes `OnItemEdit` and clears `_editingNodeId`. `CancelEdit` clears both fields without raising the event.
- The `ExpandOnDoubleClick` path already guards on `!AllowEditing` (line 506), so the suppression is inherent to the render logic rather than a separate conditional.
- Pros: Minimal new state (two nullable fields); edit input is rendered inline with no overlay or portal; keyboard path reuses existing `HandleKeyDown`; commit/cancel logic is symmetric and testable in isolation.
- Cons: `autofocus` is set as a static attribute; if the component re-renders mid-edit for an unrelated reason the focus may shift. Consumers who need server-side validation before accepting a rename must handle it via `OnItemEdit` and manually call methods to revert — the component has no built-in async commit.
- Effort: Medium

**Option B (Not chosen): Modal/popover rename dialog**

- Approach: On activation, open a modal dialog containing a text field. Commit closes the modal and fires an event.
- Pros: Easier to style; avoids layout reflow in the tree.
- Cons: Breaks the inline editing UX expectation; adds a dependency on a dialog component; heavier DOM mutation for a simple rename.
- Effort: Large

**Option C (Not chosen): Contenteditable span**

- Approach: Make the title `<span>` `contenteditable` when `AllowEditing` is true and the node is focused.
- Pros: No element swap; preserves inline layout precisely.
- Cons: `contenteditable` has inconsistent browser behaviour around Enter, paste, and IME input; extracting the typed text reliably requires extra sanitisation; Blazor's reconciler can conflict with browser-managed content.
- Effort: Medium (with higher maintenance burden)

#### Decision

**Chosen:** Option A
**Rationale:** The render-time element swap (span → input) is the canonical Blazor pattern for conditional inline editing. The approach keeps all state within the component, avoids contenteditable pitfalls, and integrates cleanly with the existing keyboard handler. The `ExpandOnDoubleClick` suppression falls out naturally from the existing render guard rather than requiring an additional code path.

#### Target Pattern

```razor
<!-- Basic inline editing -->
<MariloTreeView Nodes="@nodes"
                AllowEditing="true"
                OnItemEdit="@HandleItemEdit" />

@code {
    private void HandleItemEdit(TreeItemEditEventArgs args)
    {
        // args.ItemId  — the node being renamed
        // args.NewText — trimmed, non-empty new label
        var node = FindNode(args.ItemId);
        if (node is not null)
            node.Text = args.NewText;
    }
}
```

Parameter signatures (from `MariloTreeView.razor.cs`, lines 131–136):

```csharp
[Parameter] public bool AllowEditing { get; set; }

[Parameter] public EventCallback<TreeItemEditEventArgs> OnItemEdit { get; set; }
```

`TreeItemEditEventArgs` model (from `TreeViewModels.cs`):

```csharp
public class TreeItemEditEventArgs
{
    public string ItemId { get; set; } = string.Empty;
    public string NewText { get; set; } = string.Empty;
}
```

Private editing state (from `MariloTreeView.razor.cs`, lines 22–23):

```csharp
private string? _editingNodeId;
private string? _editingText;
```

Activation — double-click guard (from `MariloTreeView.razor.cs`, lines 599–600):

```csharp
// ondblclick on title span, emitted only when AllowEditing && !Disabled && !ReadOnly
```

Activation — F2 key (from `MariloTreeView.razor.cs`, line 784):

```csharp
// F2 guard: AllowEditing && !ReadOnly && _focusedNodeId != null
```

ExpandOnDoubleClick suppression (from `MariloTreeView.razor.cs`, line 506):

```csharp
if (ExpandOnDoubleClick && !AllowEditing)
    builder.AddAttribute(21, "ondblclick", ...ToggleNodeAsync...);
```

Commit and cancel (from `MariloTreeView.razor.cs`, lines 966–992):

```csharp
// CommitEdit (line 973):
//   trims _editingText; if non-empty fires OnItemEdit; clears _editingNodeId

// CancelEdit (line 985):
//   clears _editingNodeId and _editingText; does NOT fire OnItemEdit
```

#### Consequences

- No breaking change: `AllowEditing` defaults to `false`; existing consumers see no change.
- **ExpandOnDoubleClick suppression is intentional.** When `AllowEditing=true`, the `ondblclick` expand handler is never emitted, even if `ExpandOnDoubleClick=true`. Double-click is unconditionally reserved for edit activation when editing is enabled. Consumers must not rely on `ExpandOnDoubleClick` to function alongside `AllowEditing`. This is a deliberate trade-off — the two features are mutually exclusive on the double-click gesture, and edit activation takes precedence. No warning or error is produced when both are set; the suppression is silent.
- `Disabled=true` prevents both double-click and F2 edit activation. `ReadOnly=true` prevents both activation paths independently of `Disabled`. The guards are distinct conditions, not a combined flag.
- Empty text (after trim) on commit is silently discarded: `OnItemEdit` is not fired and `_editingNodeId` is cleared, reverting to the previous label. Consumers who need to reject an edit for other reasons (e.g., duplicate name) must handle that logic in their `OnItemEdit` handler; there is no built-in re-open-on-error path.
- `autofocus` is applied as a static Blazor attribute on the edit input. Intermediate re-renders while edit mode is active will re-apply `autofocus`, which browsers typically ignore after the initial focus event. This is benign in practice but means focus recovery after a programmatic `StateHasChanged` call is not guaranteed without additional JS interop.
- Escape handling in `HandleKeyDown` checks `_editingNodeId != null` before calling `CancelEdit`, so Escape key presses outside of edit mode are passed through to any other registered Escape handlers without interference.

#### Success Criteria

- [ ] Double-click on title activates edit mode when `AllowEditing=true` (unit test)
- [ ] F2 key activates edit mode on focused node when `AllowEditing=true` (unit test)
- [ ] Edit input replaces title span during editing (unit test)
- [ ] Enter key commits edit and fires `OnItemEdit` with new text (unit test)
- [ ] Escape key cancels edit and restores original text (unit test)
- [ ] Blur commits edit (unit test)
- [ ] Empty text after trim does not fire `OnItemEdit` (unit test)
- [ ] `AllowEditing=false` prevents edit activation (unit test)
- [ ] `Disabled=true` prevents edit activation (unit test)
- [ ] `ReadOnly=true` prevents edit activation (unit test)
- [ ] `ExpandOnDoubleClick` is suppressed when `AllowEditing=true` (unit test)
- [ ] `AllowEditing` defaults to `false` (unit test)

<!-- Reconstructed retroactively — implementation predates this record -->
