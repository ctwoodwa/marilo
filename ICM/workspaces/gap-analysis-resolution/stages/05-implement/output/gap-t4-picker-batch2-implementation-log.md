# T4 Picker Batch 2 — Implementation Log

> Date: 2026-04-04
> Stage: 05-implement
> Batch: T4 Picker Batch 2 (4 gaps across MultiSelect and Upload)

---

## Summary

| Metric | Value |
|--------|-------|
| Gaps addressed | 4 (template slots, AllowCustom, WithCredentials fix) |
| Files created | 0 |
| Files modified | 4 (MultiSelect.razor, Upload.razor, Upload.razor.cs, MariloFileUpload.razor + .razor.cs) |
| Tests written | 9 new bUnit tests (6 MultiSelect + 3 Upload) |
| Tests passing | runtime pending (.NET SDK not available) |
| Build status | static analysis clean |

---

## RES-T4B2-001: MultiSelect template slots (5 templates)

**Resolves:** GAP-MSEL-004
**Files modified:** `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`

**Changes:**
- Added 5 `[Parameter]` declarations: `TagTemplate` (RenderFragment<TItem>), `SummaryTagTemplate` (RenderFragment<List<TValue>>), `HeaderTemplate`, `FooterTemplate`, `NoDataTemplate`
- `TagTemplate`: Renders inside each tag span when provided; falls back to `@GetText(tagItem)`
- `SummaryTagTemplate`: When `TagMode=Single`, replaces "N items selected" text; receives full value list
- `HeaderTemplate`: Renders inside popup before item list, wrapped in `mar-multiselect__header` div
- `FooterTemplate`: Renders inside popup after item list, wrapped in `mar-multiselect__footer` div
- `NoDataTemplate`: Replaces "No items found" when filtered list is empty

---

## RES-T4B2-002: MultiSelect AllowCustom parameter

**Resolves:** GAP-MSEL-002
**Files modified:** `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`

**Changes:**
- Added `[Parameter] public bool AllowCustom { get; set; }` declaration
- When `AllowCustom=true` and filter text has no exact match in items, a "Create: {filterText}" option appears with `mar-multiselect__item--custom` CSS class
- Selecting the custom option adds filter text as a value to the selected list via `HandleAddCustom` method
- Default `AllowCustom=false` preserves existing behavior

---

## RES-T4B2-003: Upload template slots (3 templates)

**Resolves:** GAP-UPL-001
**Files modified:** `src/Marilo.Components/Forms/Inputs/MariloUpload.razor`, `src/Marilo.Components/Forms/Inputs/MariloUpload.razor.cs`

**Changes:**
- Added 3 `[Parameter]` declarations in code-behind: `SelectFilesButtonTemplate` (RenderFragment), `FileTemplate` (RenderFragment<UploadFileInfo>), `FileInfoTemplate` (RenderFragment<UploadFileInfo>)
- `SelectFilesButtonTemplate`: Replaces browse button label content; `<InputFile>` remains outside template
- `FileTemplate`: Replaces entire `<li>` content for each file in file list
- `FileInfoTemplate`: Replaces file info section (name, size, error) while keeping status/progress/actions
- Same templates also applied to `MariloFileUpload.razor` + `.razor.cs` (FileSelect variant) using `FileSelectFileInfo` context type

---

## RES-T4B2-004: Upload WithCredentials fix

**Resolves:** GAP-UPL-002
**Files modified:** `src/Marilo.Components/Forms/Inputs/MariloUpload.razor.cs`

**Changes:**
- Added `request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include)` in three HTTP request paths:
  - `UploadWholeAsync` (line ~402)
  - `UploadChunkedAsync` (line ~488)
  - `RemoveFileInternalAsync` (line ~571)
- All three paths guarded by `if (WithCredentials)` check
- Default `WithCredentials=false` leaves requests unchanged

---

## Tests Written

| Gap | Test File | Test Method | Status |
|-----|-----------|-------------|--------|
| GAP-MSEL-004 | MultiSelectTests.cs | TagTemplate_RendersCustomContent | pending |
| GAP-MSEL-004 | MultiSelectTests.cs | NoDataTemplate_RendersWhenEmpty | pending |
| GAP-MSEL-004 | MultiSelectTests.cs | HeaderTemplate_RendersInPopup | pending |
| GAP-MSEL-004 | MultiSelectTests.cs | FooterTemplate_RendersInPopup | pending |
| GAP-MSEL-004 | MultiSelectTests.cs | SummaryTagTemplate_RendersInSingleMode | pending |
| GAP-MSEL-002 | MultiSelectTests.cs | AllowCustom_ShowsCreateOption | pending |
| GAP-UPL-001 | P1ComponentTests.cs | Upload_SelectFilesButtonTemplate_RendersCustomContent | pending |
| GAP-UPL-001 | P1ComponentTests.cs | Upload_FileTemplate_ParameterExists | pending |
| GAP-UPL-001 | P1ComponentTests.cs | Upload_FileInfoTemplate_ParameterExists | pending |
