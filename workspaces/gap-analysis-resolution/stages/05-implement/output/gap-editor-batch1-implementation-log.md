# Editor Batch 1 — Implementation Log

> Date: 2026-04-04
> Stage: 05-implement
> Batch: Editor Batch 1 (6 gaps: 2 already resolved, 4 implemented)

---

## Summary

| Metric | Value |
|--------|-------|
| Gaps addressed | 6 (2 already resolved, 4 implemented) |
| Files created | 1 (EditorCustomTool.cs) |
| Files modified | 2 (MariloEditor.razor, EditorTests.cs) |
| Tests written | 7 new bUnit tests (7 existing → 14 total) |
| Tests passing | runtime pending (.NET SDK not available) |

---

## Already Resolved (No Code Change)

### GAP-EDITOR-001: ExecuteAsync() — exists at MariloEditor.razor:174
Polymorphic dispatch for HtmlCommandArgs, FormatCommandArgs, LinkCommandArgs, etc.

### GAP-EDITOR-010: Edit modes — EditorEditMode enum has Edit/Preview/Source (complete)

---

## RES-EDITOR-007: Validation integration

**Files modified:** `src/Marilo.Components/Editors/MariloEditor.razor`

- Added `CascadedEditContext` cascading parameter
- Added `ValueExpression` parameter (Expression<Func<string>>)
- Added `_fieldIdentifier` field, initialized in OnParametersSet
- NotifyFieldChanged called after debounced value change and direct HTML set

---

## RES-EDITOR-009: Custom tool API

**Files created:** `src/Marilo.Components/Editors/EditorCustomTool.cs`
**Files modified:** `src/Marilo.Components/Editors/MariloEditor.razor`

- EditorCustomTool class: Name, Icon, Tooltip, OnClick (Func<Task>), Template (RenderFragment)
- CustomTools parameter added to MariloEditor
- Custom tools render after built-in tools; each gets a button or renders its Template
- ToolbarTemplate still takes full precedence

---

## RES-EDITOR-008: Sanitization documentation

**Files modified:** `src/Marilo.Components/Editors/MariloEditor.razor`

- Added XML doc comments to SanitizeHtml() and SanitizeAttr()
- Added security remarks on the component class

---

## RES-EDITOR-011: Test expansion

**Files modified:** `tests/Marilo.Tests.Unit/P1Content/EditorTests.cs`

| Test Method | Gap | Status |
|-------------|-----|--------|
| Editor_EditMode_Source_RendersTextarea | GAP-EDITOR-010 | pending |
| Editor_EditMode_Preview_DisablesEditing | GAP-EDITOR-010 | pending |
| Editor_Disabled_RendersDisabledState | GAP-EDITOR-011 | pending |
| Editor_CustomTools_RenderInToolbar | GAP-EDITOR-009 | pending |
| Editor_CustomTools_OnClick_Fires | GAP-EDITOR-009 | pending |
| Editor_AriaAttributes_Present | GAP-EDITOR-011 | pending |
| Editor_ValueExpression_AcceptedWithoutError | GAP-EDITOR-007 | pending |
