# FileManager Phase E — Gap Resolutions

**Workspace:** filemanager-gap-analysis  
**Stage:** 03 — Resolutions  
**Phase:** E (Upload & Preview)  
**Date:** 2026-04-09  
**Author:** Claude (automated)

---

## Resolved Gaps

### SPEC-FM-028 — FileManagerSettings + FileManagerUploadSettings

**Status:** Resolved  
**Priority:** P2

**Resolution:**
- Added `FileManagerUploadSettings` class to `FileManagerModels.cs` with properties:
  - `SaveUrl` (string?) — server endpoint for file uploads
  - `AllowedExtensions` (string[]?) — file type filter; null = all types accepted
  - `MaxFileSize` (long) — maximum bytes; 0 = no limit
  - `Multiple` (bool, default `true`) — allows multi-file selection
- Added `[Parameter] public FileManagerUploadSettings? UploadSettings { get; set; }` to `MariloFileManager<TItem>`
- Added `[Parameter] public RenderFragment? FileManagerSettings { get; set; }` to `MariloFileManager<TItem>` for future child configuration fragments
- When `UploadSettings` is not null, an Upload button (`mar-filemanager__upload-btn`) appears in the default toolbar
- Clicking the button opens a simple file upload dialog (`mar-filemanager__upload-dialog`) containing an `<input type="file">` styled as `mar-filemanager__upload-input`
- Clicking the overlay backdrop or the close button dismisses the dialog
- MariloUpload integration is intentionally excluded (Phase E scope only)

**Approach decision:** Flat `[Parameter]` rather than child-component cascade. The `FileManagerSettings` RenderFragment parameter provides the child-configuration surface spec requires, but upload settings remain a flat parameter to avoid the cascading-value complexity seen in MariloWizard.

---

### SPEC-FM-029 — Preview Pane

**Status:** Resolved  
**Priority:** P2

**Resolution:**
- Added `[Parameter] public bool ShowPreviewPane { get; set; }` — defaults to false; gates the Details button and the pane
- Added `internal bool _previewPaneVisible` — runtime toggle state
- Added `TogglePreviewPane()` — flips `_previewPaneVisible`, calls `StateHasChanged()`
- When `ShowPreviewPane && _previewPaneVisible`, renders `<div class="mar-filemanager__preview">` to the right of the file list
- Preview shows: icon (folder/file), name, type/extension, size (formatted via `FormatSize`), date created, date modified
- Uses existing `GetName`, `GetIsDirectory`, `GetExtension`, `GetSize`, `GetDateModified` accessors; added new `GetDateCreated` accessor
- When no item is selected (`SelectedItem is null`), displays `<p class="mar-filemanager__preview-empty">No item selected</p>`
- `SelectedItem` computed property returns `_selectedItems[0]` or null

---

### Details Toolbar Button (wires toolbar to preview pane)

**Status:** Resolved  
**Priority:** P2 (part of SPEC-FM-029)

**Resolution:**
- A Details toggle button (`mar-filemanager__details-btn`) is added to the default toolbar when `ShowPreviewPane` is true
- When pane is open, button gains the `mar-filemanager__details-btn--active` modifier class
- Clicking calls `TogglePreviewPane()`
- The file list container gains `mar-filemanager__files--with-preview` modifier class when the pane is open, enabling CSS flex layout (list shrinks, preview takes ~250px)

---

## Files Changed

| File | Change |
|------|--------|
| `src/Marilo.Core/Models/FileManagerModels.cs` | Added `FileManagerUploadSettings` class |
| `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor.cs` | Added `UploadSettings`, `ShowPreviewPane`, `FileManagerSettings` parameters; `_previewPaneVisible`, `_uploadDialogVisible` state; `TogglePreviewPane()`, `ShowUploadDialog()`, `CloseUploadDialog()` methods; `GetDateCreated()` accessor; `SelectedItem` computed property |
| `src/Marilo.Components/Forms/Inputs/MariloFileManager.razor` | Upload button, Details toggle button, preview pane markup, upload dialog overlay, `--with-preview` modifier on file list |

---

## Not Implemented (Out of Scope)

- MariloUpload integration — upload is stub only (Phase E scope)
- Sort UI, ARIA labels — Phase F
- Context menu, rename UI — Phase D (already implemented)
