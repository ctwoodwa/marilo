# Closure Report — T4 Pickers Batch 8C

**Batch:** 8C  
**Component scope:** MariloFileUpload, MariloUpload  
**Date:** 2026-04-09  
**Status:** CLOSED

---

## Gap Closure Summary

| Gap ID | Title | Status | Tests |
|---|---|---|---|
| RES-T4B8C-001 | FileUpload template context type mismatch | Closed | 4 |
| RES-T4B8C-002 | FileUpload drop-zone CSS provider delegation | Closed | 4 |
| RES-T4B8C-003 | Upload UploadChunkSettings nested tag API | Closed | 4 |

**Total: 3/3 gaps closed. 12/12 tests pass.**

---

## RES-T4B8C-001 — FileUpload template context type mismatch

### Acceptance criteria
- [x] A `FileUploadTemplateContext` wrapper type exists with `File`, `IsInvalid`, and `ValidationMessage` members
- [x] `FileTemplate` parameter type is `RenderFragment<FileUploadTemplateContext>?`
- [x] `FileInfoTemplate` parameter type is `RenderFragment<FileUploadTemplateContext>?`
- [x] Templates receive the wrapper at render time (not raw `FileSelectFileInfo`)
- [x] `ValidationMessage` is pre-populated with human-readable validation summary

### Breaking change note
Consumers using `FileTemplate` or `FileInfoTemplate` must update template bindings: `@context.Name` → `@context.File.Name`. Properties that existed on both types (`IsInvalid`) continue to work unchanged.

---

## RES-T4B8C-002 — FileUpload drop-zone CSS provider delegation

### Acceptance criteria
- [x] `IMariloCssProvider.FileUploadDropZoneClass(bool isDragOver, bool isDisabled)` exists
- [x] FluentUI provider implements it with BEM structural classes
- [x] Bootstrap provider implements it with Bootstrap bridge classes
- [x] `ProviderSwitcher` delegates to the underlying provider
- [x] `MariloFileUpload.DropZoneCssClass()` calls the provider instead of hardcoding strings
- [x] Drop zone has `mar-file-upload__zone` class in tests (FluentUI provider)
- [x] `--disabled` modifier present when `Enabled=false`
- [x] `--disabled` modifier absent when `Enabled=true`
- [x] `--dragover` modifier absent on initial render

---

## RES-T4B8C-003 — Upload UploadChunkSettings nested tag API

### Acceptance criteria
- [x] `IUploadChunkSettingsSink` interface exists (internal)
- [x] `MariloUploadChunkSettings` component exists with parameters: `ChunkSize`, `AutoRetryAfter`, `MaxAutoRetries`, `MetadataField`, `Resumable` (all nullable)
- [x] `MariloUpload` wraps content in `<CascadingValue Value="(IUploadChunkSettingsSink)this">` 
- [x] `MariloUpload` implements `IUploadChunkSettingsSink` with `RegisterChunkSettings` / `UnregisterChunkSettings`
- [x] Registration uses `InvokeAsync(StateHasChanged)` (dispatcher-safe)
- [x] Unregistration uses `ReferenceEquals` guard
- [x] Effective chunk size prefers child settings over flat `ChunkSize` parameter
- [x] `EffectiveResumable=false` causes `ResumeFile` to restart from byte 0
- [x] `EffectiveMetadataField` appends a custom field to chunk requests when set
- [x] Auto-retry fires on chunk failure when `AutoRetryAfter` is set and retry count < `MaxAutoRetries`
- [x] Flat `ChunkSize` parameter is the backward-compatible fallback when no child component is present
- [x] `MariloUploadChunkSettings` can be rendered standalone without a parent (graceful null check on `ParentSink`)

---

## Additional fix in batch

**T4PickerBatch8BTests.cs** had a pre-existing build error (`EventCallback` missing namespace qualifier). Fixed by fully qualifying as `Microsoft.AspNetCore.Components.EventCallback`.
