# Resolution Records: MariloTreeView — Disabled / ReadOnly

## Summary

Gap 17 covers the two access-state parameters that prevent user interaction with the tree. Both `Disabled` and `ReadOnly` are fully implemented. This record is reconstructed retroactively; the implementation predates the record.

---

### RES-TREEVIEW-017: Disabled and ReadOnly parameters

**Resolves:** GAP-17 (Disabled / ReadOnly access states)
**Status:** Reconstructed — implementation predates this record

#### Problem Statement

MariloTreeView required two distinct modes that restrict user interaction:

1. **Disabled** — the tree is inert in every respect. All pointer events, keyboard events, drag-and-drop, checkbox changes, selection, and inline editing must be blocked. Screen readers must be informed via `aria-disabled`.
2. **ReadOnly** — the tree displays its current state faithfully but refuses to mutate it. Expand/collapse, check/uncheck, selection, and editing are all blocked. Keyboard navigation (focus movement only) is intentionally **not** blocked, because keyboard users still need to traverse the tree to read it.

The two modes overlap in most guards but diverge on keyboard navigation: `Disabled` blocks it entirely; `ReadOnly` does not.

#### Options Considered

**Option A (Selected): Two independent boolean parameters with per-interaction guards**

- Approach: Add `Disabled` and `ReadOnly` as separate `[Parameter]` properties. Each interactive code path checks the relevant flag(s) before mutating state or emitting interactive attributes. ARIA state is emitted on the root `<ul>` element.
- `Disabled` blocks everything — mutations and navigation alike.
- `ReadOnly` blocks mutations only; keyboard navigation remains live so that keyboard users can still read the tree.
- Both parameters default to `false`, ensuring no behaviour change for existing consumers.
- Pros: The distinction between "cannot interact at all" and "can read but not write" is semantically precise. Guards are co-located with the interaction code they protect, making audits straightforward.
- Cons: Consumers could set both `true` simultaneously, which is redundant but harmless (the more restrictive rules subsume the less restrictive ones).
- Effort: Small per interaction point; systematic across the codebase.

**Option B (Not chosen): Single `Disabled` parameter; ReadOnly expressed via CSS or external guard**

- Approach: Only expose `Disabled`. Callers wanting a read-only state would style the component externally or disable individual controls.
- Pros: Simpler API surface.
- Cons: Cannot cleanly preserve keyboard navigation for read-only trees. Does not meet the documented gap requirement for an explicit `ReadOnly` parameter. Puts coordination burden on consumers.
- Effort: Smaller initially; larger consumer burden.

#### Decision

**Chosen:** Option A
**Rationale:** The two modes serve distinct accessibility use-cases. `Disabled` maps to the ARIA concept of `aria-disabled` and implies complete inertness. `ReadOnly` maps to the concept of a non-editable but navigable control. Providing both as first-class parameters is the only way to honour keyboard navigation for read-only trees without placing that logic on consumers.

#### Key Behavioural Distinction

| Interaction | Disabled | ReadOnly |
|---|---|---|
| Expand / collapse (toggle button) | Blocked | Blocked |
| Expand / collapse (ExpandOnClick) | Blocked | Allowed (ExpandOnClick guard only checks `!Disabled`) |
| Selection | Blocked | Blocked |
| Checkbox changes | Blocked | Blocked |
| Keyboard navigation (focus movement) | Blocked | **Allowed** |
| Inline editing | Blocked | Blocked |
| Drag and drop | Blocked | Allowed (drag guard only checks `!Disabled`) |
| `aria-disabled` on root `<ul>` | `"true"` | Not set |

> **Undocumented behaviour found (1):** `ExpandOnClick` and drag-and-drop guards check only `!Disabled`, not `ReadOnly`. A `ReadOnly` tree with `ExpandOnClick=true` or `EnableDragDrop=true` will therefore still allow expansion via click and drag-and-drop reordering respectively. This is likely intentional — drag-and-drop is a structural mutation, and the current implementation leaves it to the caller's event handlers to reject — but it is not stated in the gap description and warrants a future decision or explicit test.

#### Target Pattern

```razor
<!-- Fully disabled tree — all interaction blocked, aria-disabled set -->
<MariloTreeView Nodes="@nodes"
                Disabled="true" />

<!-- Read-only tree — state visible, mutations blocked, keyboard navigation live -->
<MariloTreeView Nodes="@nodes"
                ReadOnly="true" />
```

Parameter signatures (from `MariloTreeView.razor.cs`):

```csharp
// Line 115
[Parameter] public bool Disabled { get; set; }

// Line 118
[Parameter] public bool ReadOnly { get; set; }
```

#### Implementation Evidence

Guards present in `MariloTreeView.razor.cs`:

```csharp
// ToggleNodeAsync — line 635
if (Disabled || ReadOnly) return;

// ToggleItemChecked — line 254
if (id == null || Disabled || ReadOnly) return;

// SelectItem — line 302
if (Disabled || ReadOnly) return;

// HandleKeyDown — line 705 (ReadOnly intentionally omitted — navigation remains live)
if (Disabled) return;

// ExpandOnClick render guard — line 501 (ReadOnly intentionally omitted)
if (hasKids && !Disabled)

// DragDrop render guard — line 480 (ReadOnly intentionally omitted)
if (EnableDragDrop && !Disabled)

// Title click guard — line 597
if (!Disabled)

// Inline edit on click — line 599
if (AllowEditing && !Disabled && !ReadOnly)

// F2 inline edit — line 784
if (AllowEditing && !ReadOnly && _focusedNodeId != null)
```

Rendered attribute guards in `MariloTreeView.razor`:

```razor
<!-- Toggle button — line 519 -->
disabled="@Disabled"

<!-- Checkbox — line 548 -->
disabled="@(Disabled || ReadOnly)"

<!-- Root <ul> ARIA — line 8 -->
aria-disabled="@(Disabled ? "true" : null)"
```

#### Consequences

- No breaking change: both parameters default to `false`.
- Setting both `Disabled=true` and `ReadOnly=true` simultaneously is supported; the more restrictive rules subsume the less restrictive ones.
- `aria-disabled` is only set for `Disabled`. `ReadOnly` does not emit an ARIA attribute on the root element; individual inputs (`disabled` on checkboxes) convey the read-only state to assistive technologies.
- `ExpandOnClick` and drag-and-drop remain live under `ReadOnly`. Consumers enabling those features on a `ReadOnly` tree should handle the resulting events in their own callbacks and choose whether to reject the resulting mutations.

#### Success Criteria

- [ ] `Disabled=true` prevents expand/collapse via toggle button (unit test)
- [ ] `Disabled=true` prevents selection (unit test)
- [ ] `Disabled=true` prevents checkbox changes (unit test)
- [ ] `Disabled=true` prevents keyboard navigation (unit test)
- [ ] `Disabled=true` sets `aria-disabled="true"` on root element (unit test)
- [ ] `ReadOnly=true` prevents expand/collapse via `ToggleNodeAsync` (unit test)
- [ ] `ReadOnly=true` prevents checkbox changes (unit test)
- [ ] `ReadOnly=true` allows keyboard navigation — focus movement is not blocked (unit test)
- [ ] Both `Disabled` and `ReadOnly` default to `false` (unit test)

<!-- Reconstructed retroactively — implementation predates this record -->
