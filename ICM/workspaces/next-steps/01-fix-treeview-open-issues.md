# Step 01 — Fix TreeView Open Issues

## Context

TreeView is at 87% completion with 2 confirmed open bugs blocking final sign-off.
This prompt resolves both issues using the `treeview-gap-analysis` workspace.

**Issue 1 — High Severity:** `ExpandAllAsync` + `LazyLoad` combination causes infinite async loop
- Reproducer: Call `ExpandAllAsync()` on a tree with `LazyLoad="true"` and a slow data source
- Expected: Each node loads its children once and resolves
- Actual: Load callback fires in a loop, state never settles, component eventually crashes

**Issue 2 — Medium Severity:** ReadOnly mode does not guard all mutation paths
- `Checked` state can be changed via keyboard (Space key) even when `ReadOnly="true"`
- `Drag` can initiate even when `ReadOnly="true"` if `Draggable` is also set
- Expected: All state mutation is blocked when `ReadOnly="true"`

---

## Your Task

You are a Claude agent working in the `treeview-gap-analysis` workspace at:
`/workspaces/Marilo/workspaces/treeview-gap-analysis/`

Read `CLAUDE.md` and `CONTEXT.md` first to understand the workspace contract.

### Fix 1 — ExpandAllAsync + LazyLoad Infinite Loop

1. Read the current `ExpandAllAsync` implementation in `src/Marilo.Components/TreeView/MariloTreeView.Expand.cs`.
2. Read the lazy load path in `MariloTreeView.Data.cs` to understand how `OnExpand` triggers data loads.
3. Identify the guard condition that is missing: `ExpandAllAsync` must check if a node already has loaded children before triggering the `OnExpand` callback. If `node.IsLoaded == true`, skip the load callback for that node.
4. Write a fix that:
   - Adds a `IsLoaded` tracking field per node (or verify one already exists)
   - Guards the `ExpandAllAsync` loop so it only calls the load callback for nodes where `IsLoaded == false`
   - Sets `IsLoaded = true` on a node after its callback completes
   - Ensures `StateHasChanged()` is called after all nodes are expanded, not inside the loop
5. Write the fix to the source file.
6. Write a bUnit regression test in `tests/Marilo.Components.Tests/TreeView/` named `TreeViewExpandAllLazyLoadTests.cs` with:
   - Test: `ExpandAllAsync_WithLazyLoad_DoesNotLoopInfinitely`
   - Test: `ExpandAllAsync_WithLazyLoad_LoadsEachNodeOnce`
   - Test: `ExpandAllAsync_WithLazyLoad_CompletesAndRendersAllNodes`

### Fix 2 — ReadOnly Mutation Guards

1. Read `MariloTreeView.Interaction.cs` and `MariloTreeView.Keyboard.cs`.
2. Find all keyboard event handlers that mutate state (check, drag initiation, selection, rename).
3. Add `if (ReadOnly) return;` guards at the top of each mutation handler.
4. Find the drag initiation path and add the same guard.
5. Write the fix to the appropriate source files.
6. Write bUnit regression tests in `TreeViewReadOnlyTests.cs`:
   - Test: `ReadOnly_Space_DoesNotToggleChecked`
   - Test: `ReadOnly_DragStart_DoesNotInitiate`
   - Test: `ReadOnly_Rename_DoesNotActivate`

---

## Output

After completing both fixes, write `output/treeview-open-issues-resolved.md` with:

```markdown
# TreeView Open Issues — Resolution Report

## Issue 1: ExpandAllAsync + LazyLoad
- Status: FIXED
- File(s) changed: [list]
- Approach: [one-line summary]
- Tests added: [test names]

## Issue 2: ReadOnly Guards
- Status: FIXED
- File(s) changed: [list]
- Approach: [one-line summary]
- Tests added: [test names]

## TreeView Completion Status
- Previously: 87%
- After this fix: [estimated %]
- Remaining blockers: [none / list]
```

---

## Constraints

- Do not change any component API (parameters, events, methods). Bug fixes only.
- Do not modify any files outside `src/Marilo.Components/TreeView/` and `tests/Marilo.Components.Tests/TreeView/`.
- If you cannot determine the correct fix from reading the source, output a `BLOCKED` report in `output/` describing what human input is needed, and stop.
- Run `dotnet build` after each fix. Do not proceed to Fix 2 if Fix 1 does not compile.
