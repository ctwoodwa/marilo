# Gap Resolution Design — T4 Pickers Batch 8C

**Batch:** 8C  
**Component scope:** MariloFileUpload, MariloUpload  
**Date:** 2026-04-09  
**Author:** AI (automated gap-analysis-resolution pipeline)

---

## RES-T4B8C-001 — FileUpload: Template context type mismatch

| Field | Value |
|---|---|
| Gap ID | RES-T4B8C-001 |
| Component | MariloFileUpload |
| Type | api-contract |
| Priority | P2 |
| Status | Resolved |

### Problem
`FileTemplate` and `FileInfoTemplate` pass raw `FileSelectFileInfo` objects as their `RenderFragment<T>` context. The spec documents a richer wrapper type that includes upload status, progress, and validation state visible to template authors.

### Resolution
Add a `FileUploadTemplateContext` class to `UploadModels.cs` (namespace `Marilo.Components.Forms.Inputs`) that wraps `FileSelectFileInfo` with:
- `File : FileSelectFileInfo` — the underlying file
- `IsInvalid : bool` — convenience accessor
- `ValidationMessage : string` — human-readable validation summary

Change `FileTemplate` and `FileInfoTemplate` parameter types from `RenderFragment<FileSelectFileInfo>?` to `RenderFragment<FileUploadTemplateContext>?`.

Update `MariloFileUpload.razor` to build a `FileUploadTemplateContext` per file and pass it as the template context.

**Backward compatibility:** `FileUploadTemplateContext` exposes `File` so consumers can access raw `FileSelectFileInfo` properties. Adding a new wrapper type is a minor breaking change for existing template consumers but required for spec compliance.

---

## RES-T4B8C-002 — FileUpload: CSS provider delegation for drop-zone

| Field | Value |
|---|---|
| Gap ID | RES-T4B8C-002 |
| Component | MariloFileUpload |
| Type | css-provider |
| Priority | P2 |
| Status | Resolved |

### Problem
`MariloFileUpload.DropZoneCssClass()` builds the drop-zone CSS class string inline in the codebehind (hardcoded `"mar-file-upload__zone"` + state modifiers). This bypasses the CSS provider pattern used by every other component. `IMariloCssProvider` has no `FileUploadDropZoneClass()` method.

### Resolution
1. Add `string FileUploadDropZoneClass(bool isDragOver, bool isDisabled)` to `IMariloCssProvider`.
2. Implement in `FluentUICssProvider`: emit `mar-file-upload__zone`, `mar-file-upload__zone--dragover` (when active), `mar-file-upload__zone--disabled` (when disabled).
3. Implement in `BootstrapCssProvider`: emit Bootstrap border/bg classes (`border border-2 rounded-3 p-4 text-center mar-bs-file-upload__zone`) plus state modifiers.
4. Delegate in `ProviderSwitcher`.
5. Update `MariloFileUpload.razor.cs` `DropZoneCssClass()` to call `CssProvider.FileUploadDropZoneClass(_isDragOver, !Enabled)`.

Structural BEM modifiers (`--dragover`, `--disabled`) are produced by the provider to allow both design systems to share the same base SCSS selectors while adding their own bridge classes.

---

## RES-T4B8C-003 — Upload: UploadChunkSettings nested tag API

| Field | Value |
|---|---|
| Gap ID | RES-T4B8C-003 |
| Component | MariloUpload |
| Type | api — nested-tag |
| Priority | P2 |
| Status | Resolved |

### Problem
`MariloUpload` exposes only a flat `ChunkSize` parameter. The spec requires a `<UploadChunkSettings>` child component with: `AutoRetryAfter` (int, ms delay before auto-retry), `MaxAutoRetries` (int), `MetadataField` (string, server-side metadata field name), `Resumable` (bool).

### Resolution
Apply the canonical interface-decoupled cascade pattern (matches `MultiSelectSettings` / `IMultiSelectSettingsSink` from Batch 7):

1. **`IUploadChunkSettingsSink`** — internal interface on `MariloUpload` with `RegisterChunkSettings` / `UnregisterChunkSettings` methods.
2. **`MariloUploadChunkSettings`** — `ComponentBase` child component with `[CascadingParameter] internal IUploadChunkSettingsSink?` and parameters: `ChunkSize` (long?), `AutoRetryAfter` (int? ms), `MaxAutoRetries` (int?), `MetadataField` (string?), `Resumable` (bool?). All nullable — null means fall through to parent flat parameter.
3. **`MariloUpload`** implements `IUploadChunkSettingsSink`. Wraps `@ChildContent` in `<CascadingValue Value="(IUploadChunkSettingsSink)this" IsFixed="true">`. `RegisterChunkSettings` / `UnregisterChunkSettings` use `InvokeAsync(StateHasChanged)`.
4. **Effective chunk properties** computed via helper properties that prefer child-component values over flat parameters (e.g., `EffectiveChunkSize`, `EffectiveAutoRetryAfter`, etc.).
5. Flat `ChunkSize` parameter is kept as the backward-compatible fallback when no child component is present.
6. `UploadChunkedAsync` uses `EffectiveAutoRetryAfter` / `EffectiveMaxAutoRetries` to implement auto-retry on chunk failure. Adds `MetadataField` custom form field to chunk requests when set. `Resumable` gates whether `UploadedBytes` is tracked across pause/resume (already present; `Resumable=false` resets to 0 on resume).
