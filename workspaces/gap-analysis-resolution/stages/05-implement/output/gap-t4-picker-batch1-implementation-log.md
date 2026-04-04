# T4 Picker Batch 1 — Implementation Log

> Date: 2026-04-03
> Stage: 05-implement
> Batch: T4 Picker Batch 1 (10 high-severity gaps across 5 components)

---

## Summary

| Metric | Value |
|--------|-------|
| Gaps addressed | 10 (events, bug fixes, cancellation support) |
| Files created | 2 (PopupEventArgs.cs, CalendarCellRenderEventArgs.cs) |
| Files modified | 5 (TimePicker, MultiSelect, DateTimePicker, DateRangePicker, Upload) |
| Tests written | 17 new bUnit tests |
| Tests passing | 406/406 (17 new + 389 existing, zero regressions) |
| Build status | 0 errors, 2 pre-existing warnings |

---

## RES-T4B1-001: Shared PopupEventArgs

**Files created:**
- `src/Marilo.Core/Models/PopupEventArgs.cs` — `PopupEventArgs` class with `IsCancelled` property

**Consumed by:** MariloTimePicker, MariloMultiSelect, MariloDateTimePicker, MariloDateRangePicker

---

## RES-T4B1-003: DateTimePicker CalendarCellRenderEventArgs

**Files created:**
- `src/Marilo.Core/Models/CalendarCellRenderEventArgs.cs` — `CalendarCellRenderEventArgs` with `Date`, `CssClass`, `IsDisabled`

---

## RES-T4B1-005: TimePicker PopupClass bug fix + cancellable events

**Files modified:** `src/Marilo.Components/Forms/Inputs/MariloTimePicker.razor`

**Changes:**
- Line 58: Added `@PopupClass` to popup div class attribute (was missing — bug fix)
- Parameters: `OnOpen` changed from `EventCallback` to `EventCallback<PopupEventArgs>`, same for `OnClose`
- `OpenDropdown()` now async, fires `OnOpen` with cancellation support before opening
- `Commit()` fires `OnClose` with cancellation before closing
- `Cancel()` fires `OnClose` with cancellation before closing
- All callers (Open, Close, OnInputFocus, OnKeyDown, ToggleDropdown) updated to async/await

---

## RES-T4B1-002: MultiSelect core events

**Files modified:** `src/Marilo.Components/Forms/Inputs/MariloMultiSelect.razor`

**Changes:**
- Added 3 new parameters: `OnOpen` (EventCallback<PopupEventArgs>), `OnClose` (EventCallback<PopupEventArgs>), `OnBlur` (EventCallback)
- `OpenDropdown()` now async with cancellation via PopupEventArgs
- `CloseDropdown()` now async with cancellation via PopupEventArgs
- Added `@onfocusout="HandleBlur"` on outer div
- All callers updated to await async methods
- Public `Open()`/`Close()` now return Task

---

## RES-T4B1-003: DateTimePicker spec events

**Files modified:** `src/Marilo.Components/Forms/Inputs/MariloDateTimePicker.razor`

**Changes:**
- Added 4 new parameters: `OnOpen`, `OnClose`, `OnBlur`, `OnCalendarCellRender`
- `OpenPopup()` now async with cancellation + triggers `RebuildCellRenderCache()`
- `ClosePopup()` now async with cancellation
- Added `@onblur="HandleBlur"` on input element
- Calendar cell render integration: `_cellRenderCache` populated on popup open and month change
- Public `Open()`/`Close()` now return Task

---

## RES-T4B1-004: DateRangePicker OnOpen/OnClose

**Files modified:** `src/Marilo.Components/Forms/Inputs/MariloDateRangePicker.razor`

**Changes:**
- Added 2 new parameters: `OnOpen` (EventCallback<PopupEventArgs>), `OnClose` (EventCallback<PopupEventArgs>)
- `OpenPopup()` now async with cancellation
- `ClosePopup()` now async with cancellation
- All callers (SelectDay, ClearDates, OnKeyDown) updated to await
- Public `Open()`/`Close()` now return Task

---

## RES-T4B1-006: Upload chunk resume fix

**Files modified:**
- `src/Marilo.Components/Forms/Inputs/UploadModels.cs` — Added `UploadedBytes` property to `UploadFileInfo`
- `src/Marilo.Components/Forms/Inputs/MariloUpload.razor.cs` — Resume from `UploadedBytes` offset, skip already-uploaded chunks, update progress tracking after each chunk, reset on retry

---

### Tests Written

| Gap | Test File | Test Method | Status |
|-----|-----------|-------------|--------|
| GAP-TP-003 | T4PickerBatch1Tests.cs | TimePicker_PopupClass_AppliedToPopupDiv | ✅ PASS |
| GAP-TP-003 | T4PickerBatch1Tests.cs | TimePicker_PopupClass_Null_NoExtraClass | ✅ PASS |
| GAP-TP-001 | T4PickerBatch1Tests.cs | TimePicker_OnOpen_Fires_WithPopupEventArgs | ✅ PASS |
| GAP-TP-001 | T4PickerBatch1Tests.cs | TimePicker_OnOpen_Cancelled_PopupStaysClosed | ✅ PASS |
| GAP-TP-001 | T4PickerBatch1Tests.cs | TimePicker_OnClose_Fires_OnCommit | ✅ PASS |
| GAP-TP-001 | T4PickerBatch1Tests.cs | TimePicker_OnClose_Cancelled_PopupStaysOpen | ✅ PASS |
| GAP-TP-001 | T4PickerBatch1Tests.cs | TimePicker_OnBlur_Fires | ✅ PASS |
| GAP-MSEL-001 | T4PickerBatch1Tests.cs | MultiSelect_OnOpen_Fires | ✅ PASS |
| GAP-MSEL-001 | T4PickerBatch1Tests.cs | MultiSelect_OnOpen_Cancelled_PopupStaysClosed | ✅ PASS |
| GAP-MSEL-001 | T4PickerBatch1Tests.cs | MultiSelect_OnClose_Fires_ViaToggle | ✅ PASS |
| GAP-MSEL-001 | T4PickerBatch1Tests.cs | MultiSelect_OnBlur_Fires | ✅ PASS |
| GAP-DTP-001 | T4PickerBatch1Tests.cs | DateTimePicker_OnOpen_Fires | ✅ PASS |
| GAP-DTP-001 | T4PickerBatch1Tests.cs | DateTimePicker_OnOpen_Cancelled_PopupStaysClosed | ✅ PASS |
| GAP-DTP-001 | T4PickerBatch1Tests.cs | DateTimePicker_OnBlur_Fires | ✅ PASS |
| GAP-DTP-001 | T4PickerBatch1Tests.cs | DateTimePicker_OnCalendarCellRender_Fires | ✅ PASS |
| GAP-DRP-002 | T4PickerBatch1Tests.cs | DateRangePicker_OnOpen_Fires | ✅ PASS |
| GAP-DRP-002 | T4PickerBatch1Tests.cs | DateRangePicker_OnOpen_Cancelled_PopupStaysClosed | ✅ PASS |
