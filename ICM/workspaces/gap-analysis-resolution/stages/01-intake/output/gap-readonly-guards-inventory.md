# Gap Inventory: GAP-readonly-guards

**Gap ID:** GAP-readonly-guards
**Title:** ReadOnly parameter missing from ExpandOnClick, DragDrop, and title-click interaction guards
**Phase:** 2.5 (post-Phase 2 fix, pre-Phase 3)
**Scope:** single
**Severity:** Medium
**Discovered:** 2026-04-02 — during Phase 2 pipeline reconstruction (gap-disabled closure report)
**Affected files:** `Navigation/MariloTreeView.razor.cs`, `Navigation/MariloTreeView.razor`, `Navigation/MariloTreeItem.razor`, `Navigation/MariloTreeItem.razor.cs`

---

## Problem Statement

The `Disabled` and `ReadOnly` parameters on `MariloTreeView` are intended to serve distinct purposes:
- **Disabled** — blocks ALL user interaction; component is inert
- **ReadOnly** — allows viewing and keyboard navigation but blocks all state mutations (expand/collapse, selection, checking, editing, drag-drop)

However, several interaction guards check only `Disabled` and omit `ReadOnly`. This means a `ReadOnly=true` tree still allows expand/collapse via ExpandOnClick, drag-and-drop reordering, and title-click selection in certain code paths.

---

## Complete Guard Audit

### Guards that correctly check BOTH Disabled and ReadOnly ✅

| Location | Line | Guard | Interaction blocked |
|----------|:----:|-------|---------------------|
| `ToggleNodeAsync()` | 635 | `if (Disabled \|\| ReadOnly) return;` | Expand/collapse via toggle button |
| `ToggleItemChecked()` | 254 | `if (id == null \|\| Disabled \|\| ReadOnly) return;` | Checkbox state changes |
| `SelectItem()` | 302 | `if (Disabled \|\| ReadOnly) return;` | Item selection |
| Inline edit start (title dblclick) | 599 | `if (AllowEditing && !Disabled && !ReadOnly)` | Double-click edit activation |
| Inline edit start (F2 key) | 784 | `if (AllowEditing && !ReadOnly && ...)` | Keyboard edit activation |
| Checkbox `disabled` attr | 548 | `disabled="@(Disabled \|\| ReadOnly)"` | HTML checkbox disabled state |
| CheckboxTemplate context | 535 | `Disabled = Disabled \|\| ReadOnly` | Custom checkbox template disabled flag |
| MariloTreeItem `ToggleExpanded()` | .razor.cs:49 | `if (TreeView?.Disabled == true \|\| TreeView?.ReadOnly == true) return;` | Manual TreeItem expand/collapse |
| MariloTreeItem checkbox `disabled` | .razor:27 | `disabled="@(TreeView.Disabled \|\| TreeView.ReadOnly)"` | TreeItem checkbox HTML disabled |

### Guards that check ONLY Disabled — ReadOnly NOT checked ⚠️

| Location | Line | Guard | Interaction allowed when ReadOnly=true | Risk |
|----------|:----:|-------|----------------------------------------|------|
| **ExpandOnClick handler attachment** | 501 | `if (hasKids && !Disabled)` | Header click expands/collapses nodes | **Medium** — ReadOnly tree can have nodes expanded/collapsed via click |
| **DragDrop handler attachment** | 480 | `if (EnableDragDrop && !Disabled)` | Drag-and-drop events fire; `OnItemDrop` callback invoked | **Medium** — ReadOnly tree fires drop events; consumer must defensively ignore |
| **Toggle button `disabled` attr** | 519 | `disabled="@Disabled"` | Toggle button is clickable when ReadOnly=true | **Low** — `ToggleNodeAsync` at line 635 guards with `ReadOnly`, so the click is rejected at the method level. But the button appears enabled (no `disabled` HTML attr) |
| **Title click handler attachment** | 597 | `if (!Disabled)` | Title `onclick` handler attached; `SelectItem()` is called | **Low** — `SelectItem()` at line 302 guards with `ReadOnly`, so selection is blocked. But the click event still fires. |
| **HandleKeyDown early return** | 705 | `if (Disabled) return;` | All keyboard navigation proceeds | **Intentional** — ReadOnly deliberately allows keyboard focus movement. Individual mutation actions within the handler (select, checkbox) are guarded separately at their own methods. |
| MariloTreeItem toggle `disabled` | .razor:16 | `disabled="@(TreeView?.Disabled == true)"` | Toggle button appears enabled when ReadOnly=true | **Low** — same as line 519 above |

### Guard that checks ONLY ReadOnly — Disabled NOT checked

None found. Every `ReadOnly` check also includes or is paired with a `Disabled` check.

---

## Risk Assessment

### High-impact guards (state mutation occurs when ReadOnly=true)

1. **ExpandOnClick (line 501)** — `ExpandOnClick=true` + `ReadOnly=true`: clicking a node header calls `ToggleNodeAsync()`. However, `ToggleNodeAsync()` at line 635 checks `Disabled || ReadOnly` and returns early. So **the state mutation is actually blocked**. The issue is that the `onclick` handler is attached to the DOM when it doesn't need to be, creating:
   - Unnecessary DOM event handlers
   - Potential confusion in testing/debugging
   - A cursor/hover state suggesting interactivity

2. **DragDrop (line 480)** — `EnableDragDrop=true` + `ReadOnly=true`: drag event handlers are attached. Unlike ExpandOnClick, `HandleDrop()` at line 692 does NOT check `Disabled || ReadOnly` — it only checks `_draggedNodeId != null && _draggedNodeId != targetId`. **The `OnItemDrop` callback WILL fire on a ReadOnly tree.** This is the only true state-mutation leak.

3. **Toggle button disabled attribute (line 519)** — The HTML `disabled` attribute only reflects `Disabled`, not `ReadOnly`. A ReadOnly tree shows enabled-looking toggle buttons. The actual click is blocked at `ToggleNodeAsync`, but the visual inconsistency may confuse users.

### Summary

| Guard location | ReadOnly checked? | Actual mutation blocked? | Visual issue? | Action needed |
|----------------|:-:|:-:|:-:|---|
| ExpandOnClick handler (501) | ❌ | ✅ (at ToggleNodeAsync) | ⚠️ cursor | Cosmetic — add ReadOnly check |
| DragDrop handler (480) | ❌ | **❌ — HandleDrop fires** | ⚠️ drag cursor | **Fix required** — add ReadOnly guard |
| HandleDrop method (692) | ❌ | **❌ — OnItemDrop fires** | N/A | **Fix required** — add ReadOnly guard |
| Toggle button disabled (519) | ❌ | ✅ (at ToggleNodeAsync) | ⚠️ enabled look | Cosmetic — add ReadOnly to disabled attr |
| Title click handler (597) | ❌ | ✅ (at SelectItem) | Minor | Cosmetic — add ReadOnly check |
| HandleKeyDown (705) | ❌ (intentional) | N/A | None | No change needed |
| TreeItem toggle disabled (.razor:16) | ❌ | ✅ (at ToggleExpanded) | ⚠️ enabled look | Cosmetic — add ReadOnly |

---

## Recommended Resolution

### Must fix (1 item)
- **HandleDrop + DragDrop guard**: Add `ReadOnly` check to both line 480 (`if (EnableDragDrop && !Disabled && !ReadOnly)`) and line 692 (`HandleDrop` — add early return if `ReadOnly`). This is the only path where a state-mutating callback (`OnItemDrop`) fires on a ReadOnly tree.

### Should fix (3 items)
- **ExpandOnClick guard (line 501)**: Change to `if (hasKids && !Disabled && !ReadOnly)` — prevents attaching unnecessary onclick handlers.
- **Toggle button disabled attr (line 519 + TreeItem .razor:16)**: Change to `disabled="@(Disabled || ReadOnly)"` — correct visual state.
- **Title click guard (line 597)**: Change to `if (!Disabled && !ReadOnly)` — prevents attaching unnecessary onclick handler.

### No change needed (1 item)
- **HandleKeyDown (line 705)**: `ReadOnly` omission is intentional — allows keyboard navigation for accessibility.

---

## Stage Routing

| Stage | Action |
|-------|--------|
| 01-intake | ✅ This document |
| 02-prioritize | Skip (single scope, clear priority) |
| 03-resolution-design | Design the guard changes; write Success Criteria |
| 04-remediation-plan | Skip (single scope) |
| 05-implement | Apply guard fixes + write tests |
| 06-validate | Verify all guards consistent; no regressions |

---

## Cross-References

- Discovered in: `stages/06-validate/output/gap-disabled-closure-report.md` (Follow-up Tasks section)
- Plan entry: `GAP_ANALYSIS_RESOLUTION_PLAN.md` → Deferred / Partial Coverage section
- Related resolution: `stages/03-resolution-design/output/gap-disabled-resolutions.md`
