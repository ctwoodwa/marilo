# Implementation Log — T4 Pickers Batch 8C

**Batch:** 8C  
**Date:** 2026-04-09  
**Status:** Complete — all 3 gaps resolved, 12 tests pass

---

## RES-T4B8C-001 — FileUpload: Template context type mismatch

### Files changed

| File | Change |
|---|---|
| `src/Marilo.Components/Forms/Inputs/UploadModels.cs` | Added `FileUploadTemplateContext` class (wraps `FileSelectFileInfo` + `ValidationMessage`) |
| `src/Marilo.Components/Forms/Inputs/MariloFileUpload.razor.cs` | Changed `FileTemplate` / `FileInfoTemplate` parameter types to `RenderFragment<FileUploadTemplateContext>?`; added `BuildTemplateContext(FileSelectFileInfo)` helper |
| `src/Marilo.Components/Forms/Inputs/MariloFileUpload.razor` | Updated template invocations to pass `BuildTemplateContext(file)` instead of `file` directly |

### Implementation notes

`FileUploadTemplateContext` is a plain record-like class in `Marilo.Components.Forms.Inputs` (same namespace as the component) rather than `Marilo.Core.Models`, because it references `FileSelectFileInfo` which lives in the component namespace. The class exposes:
- `File : FileSelectFileInfo` — raw file info
- `IsInvalid : bool` — computed from `File.IsInvalid`
- `ValidationMessage : string` — pre-computed human-readable error summary

The `BuildTemplateContext` helper calls the existing `ValidationSummary(file)` method so validation message generation stays in one place.

**Breaking change:** Template parameter types changed from `FileSelectFileInfo` to `FileUploadTemplateContext`. Existing consumers using `@context.Name` must change to `@context.File.Name`. Existing consumers using `@context.IsInvalid` continue working unchanged (same accessor name).

---

## RES-T4B8C-002 — FileUpload: CSS provider delegation for drop-zone

### Files changed

| File | Change |
|---|---|
| `src/Marilo.Core/Contracts/IMariloCssProvider.cs` | Added `string FileUploadDropZoneClass(bool isDragOver, bool isDisabled)` |
| `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs` | Implemented: `mar-file-upload__zone` + `--dragover` / `--disabled` modifiers |
| `src/Marilo.Providers.Bootstrap/BootstrapCssProvider.cs` | Implemented: Bootstrap border/bg classes + `mar-bs-file-upload__zone` + `--dragover` / `--disabled` modifiers |
| `samples/Marilo.Demo/Services/ProviderSwitcher.cs` | Delegated `FileUploadDropZoneClass` to `Css.FileUploadDropZoneClass(...)` |
| `src/Marilo.Components/Forms/Inputs/MariloFileUpload.razor.cs` | Replaced inline `DropZoneCssClass()` string building with `CssProvider.FileUploadDropZoneClass(_isDragOver, !Enabled)` |

### Implementation notes

The FluentUI implementation emits the existing structural BEM class names (`mar-file-upload__zone`, `mar-file-upload__zone--dragover`, `mar-file-upload__zone--disabled`) so no SCSS changes are needed for the FluentUI bridge.

The Bootstrap implementation emits Bootstrap utility classes for the base state plus `mar-bs-file-upload__zone` structural class, following the Bootstrap bridge naming convention (`mar-bs-` prefix for provider-injected classes).

---

## RES-T4B8C-003 — Upload: UploadChunkSettings nested tag API

### Files changed

| File | Change |
|---|---|
| `src/Marilo.Components/Forms/Inputs/MariloUploadChunkSettings.cs` | **New file** — `IUploadChunkSettingsSink` interface + `MariloUploadChunkSettings` component |
| `src/Marilo.Components/Forms/Inputs/MariloUpload.razor` | Added `@implements IUploadChunkSettingsSink`; wrapped component in `<CascadingValue Value="(IUploadChunkSettingsSink)this" IsFixed="true">` |
| `src/Marilo.Components/Forms/Inputs/MariloUpload.razor.cs` | Added `IUploadChunkSettingsSink` implementation to partial class; added `_chunkSettings` field + effective chunk properties; updated chunk upload to use `EffectiveChunkSize`, add `EffectiveMetadataField`, implement auto-retry via `EffectiveAutoRetryAfter`/`EffectiveMaxAutoRetries`; updated `ResumeFile` to respect `EffectiveResumable` |
| `src/Marilo.Components/Forms/Inputs/UploadModels.cs` | Added `internal int ChunkRetryCount` to `UploadFileInfo` |

### Implementation notes

Pattern follows `IMultiSelectSettingsSink` / `MultiSelectSettings` from Batch 7. Key design decisions:

1. **`IUploadChunkSettingsSink`** is `internal` — not public API.
2. **CascadingValue** wraps the entire outer `<div>` (not just `ChildContent`) so the cascade is always active, even when no `ChildContent` is set. This means `<MariloUploadChunkSettings>` can be placed either as part of visual `ChildContent` or as a sibling non-visual child.
3. **`EffectiveResumable`** defaults to `true` (preserves existing behaviour). When `false`, `ResumeFile` resets `UploadedBytes = 0` so the upload restarts from byte 0.
4. **Auto-retry** uses `chunkIndex--; continue` in the chunk loop to retry the same chunk after `EffectiveAutoRetryAfter` ms delay. `ChunkRetryCount` is reset to 0 on each successful chunk.
5. **`MetadataField`** appends a custom form-data field containing `info.Id` to every chunk request. The consumer can use this to correlate chunks server-side.
6. **Flat `ChunkSize` parameter** is the backward-compatible fallback when no child component is registered (`_chunkSettings == null`).
7. **`RegisterChunkSettings` / `UnregisterChunkSettings`** use `InvokeAsync(StateHasChanged)` (dispatcher safety) and `ReferenceEquals` defensive check on unregister.

---

## Build & Test Results

```
src/Marilo.Components: Build succeeded (0 errors, 1 pre-existing warning)
src/Marilo.Providers.FluentUI: Build succeeded (0 errors)
src/Marilo.Providers.Bootstrap: Build succeeded (0 errors)
samples/Marilo.Demo: Build succeeded (0 errors, pre-existing warnings only)
tests/Marilo.Tests.Unit: Build succeeded (0 errors)

Tests: 12/12 passed
  - FileUpload_FileTemplate_ReceivesTemplateContextNotRawFileInfo: PASS
  - FileUpload_FileInfoTemplate_ReceivesTemplateContextNotRawFileInfo: PASS
  - FileUploadTemplateContext_ExposesFileAndValidationMessage: PASS
  - FileUploadTemplateContext_IsInvalid_FalseWhenFileIsValid: PASS
  - FileUpload_DropZone_HasCssProviderClass: PASS
  - FileUpload_DropZone_Disabled_HasDisabledClass: PASS
  - FileUpload_DropZone_Enabled_NoDisabledClass: PASS
  - FileUpload_DropZone_NoDragOver_NoDragoverClass: PASS
  - Upload_ChunkSettings_DefaultChunkSize_FallsBackToFlatParameter: PASS
  - Upload_ChunkSettings_ChildComponent_RegistersWithParent: PASS
  - UploadChunkSettings_Parameters_DefaultToNull: PASS
  - UploadChunkSettings_Parameters_CanBeSet: PASS
```

**Also fixed:** Pre-existing build error in `T4PickerBatch8BTests.cs` line 154 (`EventCallback` missing namespace qualifier → `Microsoft.AspNetCore.Components.EventCallback`).
