# Step 05 — Expand Test Coverage

## Context

Four critical TreeView scenarios have no bUnit test coverage.
Zero integration tests exist across the entire component library.
This prompt fills the TreeView coverage gaps and establishes an integration test baseline.

**Missing bUnit scenarios (TreeView):**
1. Virtualization with 10,000+ nodes — no test that virtual scrolling renders correctly
2. Drag-and-drop reorder — no test for node position after drop
3. Multi-select with checkboxes + programmatic `CheckAllAsync` — no coverage
4. `OnNodeRender` template override — no test that custom templates render

**Integration test baseline:**
No integration test project exists. The minimum viable baseline is:
- A Playwright or bUnit integration test that mounts a full Blazor Server test app
- Covers: initial render, one user interaction, one data update cycle
- Components targeted first: SignalRConnectionStatus (only fully-delivered component), TreeView

---

## Your Task

You are a Claude agent working in the `enterprise-test-coverage` ICM workspace at:
`/workspaces/Marilo/workspaces/enterprise-test-coverage/`

Read `CLAUDE.md` and `CONTEXT.md` first.

### Task A — Fill Missing TreeView bUnit Tests

For each of the 4 missing scenarios, write a bUnit test class in
`tests/Marilo.Components.Tests/TreeView/`:

**1. Virtualization**
File: `TreeViewVirtualizationTests.cs`
- `Virtualization_Renders_InitialViewport_With_LargeDataset`
  - 10,000 node flat list, virtualization enabled
  - Assert: only ~20 DOM nodes rendered (not 10,000)
- `Virtualization_ScrollDown_RendersNewNodes`
  - Simulate scroll, assert new nodes appear in DOM

**2. Drag and Drop**
File: `TreeViewDragDropTests.cs`
- `DragDrop_MoveNode_UpdatesDataOrder`
  - Drag node B from position 2 to position 1
  - Assert `OnDrop` callback fires with correct args
  - Assert rendered order reflects new position
- `DragDrop_DropOntoChild_IsRejected`
  - Attempt to drag parent onto its own child
  - Assert `OnDrop` does not fire (circular dependency prevention)

**3. Multi-select + CheckAllAsync**
File: `TreeViewCheckAllTests.cs`
- `CheckAllAsync_ChecksAllVisibleNodes`
- `CheckAllAsync_WithFilter_ChecksOnlyFilteredNodes`
- `MultiSelect_Checkbox_And_Programmatic_CheckAll_ProduceConsistentState`

**4. OnNodeRender Template**
File: `TreeViewTemplateTests.cs`
- `OnNodeRender_CustomTemplate_RendersCorrectly`
  - Provide a custom `NodeTemplate` that renders a `<span class="custom">` wrapper
  - Assert: all nodes contain `<span class="custom">`
- `OnNodeRender_NullTemplate_FallsBackToDefault`

---

### Task B — Integration Test Baseline

1. Check if a test app project exists at `tests/Marilo.Components.TestApp/` or similar.
   - If it exists: read its structure and note the entry point.
   - If it does not exist: create a minimal Blazor Server test app at `tests/Marilo.Components.TestApp/`
     with a single `TestPage.razor` that hosts `<MariloSignalRConnectionStatus>` and `<MariloTreeView>`.

2. Check if a Playwright or bUnit integration test project exists.
   - If Playwright exists: add tests to it.
   - If only bUnit exists: add integration-style bUnit tests using `TestContext` with a full render cycle.
   - If neither exists: create `tests/Marilo.Components.IntegrationTests/` with bUnit integration tests.

3. Write the following integration tests in `MariloComponentsIntegrationTests.cs`:
   - `SignalRConnectionStatus_InitialRender_ShowsDisconnectedState`
     - Mount the component, assert the disconnected indicator renders
   - `TreeView_InitialRender_WithData_ShowsRootNodes`
     - Mount with a 3-node flat list, assert 3 `<li>` elements render
   - `TreeView_ExpandNode_RevealChildren`
     - Mount with a 2-level tree, click the expand button, assert children appear

4. Ensure `dotnet build` and `dotnet test` both pass after all additions.

5. Write `output/test-coverage-expansion-report.md`:

```markdown
# Test Coverage Expansion Report

## bUnit Tests Added
| File | Tests added | Scenarios covered |
|------|-------------|-------------------|

## Integration Test Baseline
- Test app created/found: [path]
- Integration test project: [path]
- Tests added: [list]
- dotnet test result: PASSING / FAILING (with error)

## Remaining Coverage Gaps
[Any scenarios still untested after this run]
```

---

## Constraints

- Write tests that test behavior, not implementation details. Assert rendered output and callback invocations.
- Do not use `Thread.Sleep` or arbitrary `await Task.Delay`. Use bUnit's async helpers.
- Do not create a new test project if one already exists that can host these tests.
- If `dotnet test` fails on any new test, fix the test (not the component source). If the component has a genuine bug exposed by the test, document it in the output report and leave the test failing with a `// TODO: Bug #[number]` comment.
