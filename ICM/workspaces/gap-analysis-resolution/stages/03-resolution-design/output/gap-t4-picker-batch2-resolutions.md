# T4 Picker Batch 2 — Resolution Records

> Batch scope: Template slots & API completeness across MultiSelect, Upload
> Date: 2026-04-04
> Stage: 03-resolution-design

---

## RES-T4B2-001: MultiSelect template slots (5 templates)

**Resolves:** GAP-MSEL-004 (5 template slots missing)
**Status:** Proposed

### Target Pattern

```csharp
// On MariloMultiSelect<TItem, TValue>:
[Parameter] public RenderFragment<TItem>? TagTemplate { get; set; }
[Parameter] public RenderFragment<List<TValue>>? SummaryTagTemplate { get; set; }
[Parameter] public RenderFragment? HeaderTemplate { get; set; }
[Parameter] public RenderFragment? FooterTemplate { get; set; }
[Parameter] public RenderFragment? NoDataTemplate { get; set; }
```

- **TagTemplate**: Replaces default tag content inside each `span.mar-multiselect-tag`. Falls back to `@GetText(tagItem)`.
- **SummaryTagTemplate**: When `TagMode=Single`, replaces the "N items selected" text. Receives the full selected values list.
- **HeaderTemplate**: Renders inside popup before the item list.
- **FooterTemplate**: Renders inside popup after the item list.
- **NoDataTemplate**: Replaces hardcoded "No items found" when filtered list is empty.

### Decision

Add all 5 as `[Parameter]` declarations. Each renders with fallback to existing hardcoded content when null. This is additive and non-breaking.

### Success Criteria

- [ ] Each template renders when provided
- [ ] Existing rendering works when templates are null
- [ ] bUnit tests verify each template slot

---

## RES-T4B2-002: MultiSelect AllowCustom parameter

**Resolves:** GAP-MSEL-002
**Status:** Proposed

### Target Pattern

```csharp
[Parameter] public bool AllowCustom { get; set; }
```

When `AllowCustom=true` and the filter text doesn't match any item, a "Create: {filterText}" option appears. Selecting it adds the filter text as a custom value.

### Decision

Add parameter. When enabled and `_filteredItems` is empty (or no exact match), insert a synthetic option at the top of the dropdown. On selection, convert filter text to `TValue` and add to the value list.

### Success Criteria

- [ ] AllowCustom=true shows create option when no match
- [ ] Selecting custom option adds value
- [ ] AllowCustom=false (default) behavior unchanged
- [ ] bUnit test validates custom value creation

---

## RES-T4B2-003: Upload template slots (3 templates)

**Resolves:** GAP-UPL-001 (3 template slots missing)
**Status:** Proposed

### Target Pattern

```csharp
[Parameter] public RenderFragment? SelectFilesButtonTemplate { get; set; }
[Parameter] public RenderFragment<UploadFileInfo>? FileTemplate { get; set; }
[Parameter] public RenderFragment<UploadFileInfo>? FileInfoTemplate { get; set; }
```

- **SelectFilesButtonTemplate**: Replaces the `<label class="mar-upload__browse-btn">` content. The `<InputFile>` element is always rendered separately (not inside the template).
- **FileTemplate**: Replaces the entire `<li>` content for each file in the file list.
- **FileInfoTemplate**: Replaces just the `<span class="mar-upload__file-info">` section (file name, size, error) while keeping status/progress/actions.

### Decision

Add all 3 as `[Parameter]` declarations. Each renders with fallback to existing hardcoded content. The `<InputFile>` element must remain outside any template to maintain file selection functionality.

### Success Criteria

- [ ] Each template renders when provided
- [ ] File selection still works with custom button template
- [ ] Existing rendering works when templates are null
- [ ] bUnit tests verify template slots

---

## RES-T4B2-004: Upload WithCredentials fix

**Resolves:** GAP-UPL-002 (WithCredentials declared but inert)
**Status:** Proposed

### Target Pattern

In Blazor WebAssembly, `HttpClient` maps to the browser's `fetch` API. To send credentials (cookies), the request must include `credentials: 'include'`. This is done via `HttpRequestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include)`.

```csharp
if (WithCredentials)
    request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
```

### Decision

Add the `SetBrowserRequestCredentials` call to all three HTTP request paths: `UploadWholeAsync`, `UploadChunkedAsync`, and `RemoveFileInternalAsync`.

### Success Criteria

- [ ] WithCredentials=true adds credentials to upload requests
- [ ] WithCredentials=false (default) leaves requests unchanged
- [ ] No regression on existing upload behavior

---

## Summary

| Resolution | Gaps | Component | Effort |
|------------|------|-----------|--------|
| RES-T4B2-001 | GAP-MSEL-004 | MultiSelect | M |
| RES-T4B2-002 | GAP-MSEL-002 | MultiSelect | M |
| RES-T4B2-003 | GAP-UPL-001 | Upload | M |
| RES-T4B2-004 | GAP-UPL-002 | Upload | S |
