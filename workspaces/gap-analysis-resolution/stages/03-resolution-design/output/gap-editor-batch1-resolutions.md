# Editor Batch 1 — Resolution Records

> Batch scope: Public API closures, validation integration, test expansion
> Date: 2026-04-04
> Stage: 03-resolution-design

---

## RES-EDITOR-001: Close GAP-EDITOR-001 (ExecuteAsync) — Already Resolved

**Resolves:** GAP-EDITOR-001
**Status:** Resolved (no code change needed)

`ExecuteAsync(EditorCommandArgs args)` exists at MariloEditor.razor line 174 as a public method with polymorphic dispatch supporting HtmlCommandArgs, FormatCommandArgs, LinkCommandArgs, ImageCommandArgs, TableCommandArgs, ColorCommandArgs, FontSizeCommandArgs, FontFamilyCommandArgs. Additionally `ExecuteCommandAsync(string command)` exists at line 220. Gap was misidentified during intake.

---

## RES-EDITOR-010: Close GAP-EDITOR-010 (Edit mode) — Already Resolved

**Resolves:** GAP-EDITOR-010
**Status:** Resolved (no code change needed)

`EditorEditMode` enum has Edit, Preview, Source — these three modes match the spec. The "Div" mode mentioned in spec is a paragraph formatting mode, not a separate edit mode. Existing implementation is complete.

---

## RES-EDITOR-007: Validation integration

**Resolves:** GAP-EDITOR-007
**Status:** Proposed

### Target Pattern

Add `EditContext` integration via cascading parameter so the editor participates in form validation:

```csharp
[CascadingParameter] private EditContext? CascadedEditContext { get; set; }

/// <summary>Expression used for form-field identification.</summary>
[Parameter] public Expression<Func<string>>? ValueExpression { get; set; }

private FieldIdentifier? _fieldIdentifier;

protected override void OnParametersSet()
{
    if (ValueExpression != null)
        _fieldIdentifier = FieldIdentifier.Create(ValueExpression);
}

// In the existing value-changed handler (after debounce):
private void NotifyFieldChanged()
{
    if (_fieldIdentifier.HasValue && CascadedEditContext != null)
        CascadedEditContext.NotifyFieldChanged(_fieldIdentifier.Value);
}
```

### Decision

Add `ValueExpression` parameter and EditContext cascading parameter. On each debounced value change, call `NotifyFieldChanged()`. This follows the same pattern as Blazor's built-in `InputBase<T>`.

### Success Criteria
- [ ] Editor notifies EditContext on value change
- [ ] Data annotation validation works with editor in a form
- [ ] No regression when used outside a form (EditContext null)
- [ ] bUnit test verifies field changed notification

---

## RES-EDITOR-008: Sanitization documentation

**Resolves:** GAP-EDITOR-008
**Status:** Proposed

### Decision

Add XML doc comments to the existing `SanitizeHtml()` and `SanitizeAttr()` methods documenting:
- Which tags are allowed/blocked
- Which attributes are stripped
- How paste content is cleaned

Also add a `/// <remarks>` block on the component class documenting the security model.

### Success Criteria
- [ ] SanitizeHtml() has complete XML doc
- [ ] SanitizeAttr() has complete XML doc  
- [ ] Component class has security remarks

---

## RES-EDITOR-009: Custom tool API enhancement

**Resolves:** GAP-EDITOR-009
**Status:** Proposed

### Target Pattern

```csharp
/// <summary>Custom tool definitions beyond the built-in EditorTool set.</summary>
[Parameter] public IEnumerable<EditorCustomTool>? CustomTools { get; set; }
```

Where `EditorCustomTool` is:
```csharp
public class EditorCustomTool
{
    public string Name { get; set; } = "";
    public string? Icon { get; set; }
    public string? Tooltip { get; set; }
    public Func<Task>? OnClick { get; set; }
    public RenderFragment? Template { get; set; }
}
```

Custom tools render after built-in tools in the toolbar. Each has either an OnClick callback or a Template for full customization.

### Decision

Create `EditorCustomTool` class. Add `CustomTools` parameter. Render custom tool buttons in the toolbar after built-in tools. ToolbarTemplate still takes full precedence.

### Success Criteria
- [ ] Custom tools render in toolbar
- [ ] OnClick callback fires
- [ ] Template custom tool renders
- [ ] Built-in tools unaffected
- [ ] ToolbarTemplate still overrides everything

---

## RES-EDITOR-011: Test expansion

**Resolves:** GAP-EDITOR-011
**Status:** Proposed

### Target

Expand from 7 tests to 15+ covering:
- EditMode switching (Edit → Source → Preview)
- ExecuteAsync method (verify it accepts commands without error)
- Disabled state blocks editing
- ValueChanged fires on content change
- OnSelectionChange event
- OnCommand event
- Custom tools rendering
- Validation integration (field changed notification)
- Accessibility (aria attributes)
- Debounce behavior

---

## Summary

| Resolution | Gaps | Effort | Notes |
|------------|------|--------|-------|
| RES-EDITOR-001 | GAP-EDITOR-001 | — | Already resolved (close) |
| RES-EDITOR-010 | GAP-EDITOR-010 | — | Already resolved (close) |
| RES-EDITOR-007 | GAP-EDITOR-007 | M | Validation/EditContext integration |
| RES-EDITOR-008 | GAP-EDITOR-008 | S | Documentation only |
| RES-EDITOR-009 | GAP-EDITOR-009 | M | Custom tool API |
| RES-EDITOR-011 | GAP-EDITOR-011 | L | Test expansion |
