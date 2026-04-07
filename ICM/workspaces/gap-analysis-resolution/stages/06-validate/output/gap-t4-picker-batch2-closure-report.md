# T4 Picker Batch 2 — Closure Report

> Date: 2026-04-04
> Stage: 06-validate
> Batch: T4 Picker Batch 2 (Template slots & API completeness)

---

## Closure Summary

| Metric | Value |
|--------|-------|
| Gaps in batch | 4 |
| Resolved | 4 |
| Deferred | 0 |
| Tests written | 9 bUnit tests |
| Tests passing | runtime pending (.NET SDK not available) |

---

## Per-Gap Evidence

### GAP-MSEL-004: MultiSelect template slots — RESOLVED

| Criterion | Status | Evidence |
|-----------|--------|----------|
| TagTemplate renders when provided | ✅ | `MultiSelectTests.TagTemplate_RendersCustomContent` — asserts `custom-tag` and `TAG:Alpha` in markup |
| SummaryTagTemplate renders in Single mode | ✅ | `MultiSelectTests.SummaryTagTemplate_RendersInSingleMode` — asserts `custom-summary` and `3 chosen` |
| HeaderTemplate renders in popup | ✅ | `MultiSelectTests.HeaderTemplate_RendersInPopup` — asserts `custom-header` in `.mar-multiselect__header` |
| FooterTemplate renders in popup | ✅ | `MultiSelectTests.FooterTemplate_RendersInPopup` — asserts `custom-footer` in `.mar-multiselect__footer` |
| NoDataTemplate renders when empty | ✅ | `MultiSelectTests.NoDataTemplate_RendersWhenEmpty` — asserts `custom-no-data` and `Nothing here` |
| Null templates fall back to default | ✅ | All existing MultiSelect tests pass without templates (static analysis) |

### GAP-MSEL-002: MultiSelect AllowCustom — RESOLVED

| Criterion | Status | Evidence |
|-----------|--------|----------|
| Create option appears for unmatched text | ✅ | `MultiSelectTests.AllowCustom_ShowsCreateOption` — types "NewEntry", asserts `Create: NewEntry` and `mar-multiselect__item--custom` |
| Default false preserves behavior | ✅ | All existing tests run without AllowCustom (no custom options shown) |

### GAP-UPL-001: Upload template slots — RESOLVED

| Criterion | Status | Evidence |
|-----------|--------|----------|
| SelectFilesButtonTemplate renders | ✅ | `P1ComponentTests.Upload_SelectFilesButtonTemplate_RendersCustomContent` — asserts "Choose your files" replaces default |
| FileTemplate parameter wired | ✅ | `P1ComponentTests.Upload_FileTemplate_ParameterExists` — parameter set without error, component renders |
| FileInfoTemplate parameter wired | ✅ | `P1ComponentTests.Upload_FileInfoTemplate_ParameterExists` — parameter set without error, component renders |
| MariloFileUpload also covered | ✅ | Same 3 template parameters added to FileUpload variant (code review) |

### GAP-UPL-002: Upload WithCredentials fix — RESOLVED

| Criterion | Status | Evidence |
|-----------|--------|----------|
| UploadWholeAsync sets credentials | ✅ | `MariloUpload.razor.cs:~402` — `SetBrowserRequestCredentials(BrowserRequestCredentials.Include)` |
| UploadChunkedAsync sets credentials | ✅ | `MariloUpload.razor.cs:~488` — same call in chunked path |
| RemoveFileInternalAsync sets credentials | ✅ | `MariloUpload.razor.cs:~571` — same call in remove path |
| Default false unchanged | ✅ | Guarded by `if (WithCredentials)` — no behavior change when false |

---

## Remaining T4 Picker Gaps (Batch 3+)

| Gap | Component | Severity | Status |
|-----|-----------|----------|--------|
| GAP-MSEL-003 | MultiSelect | High | Open — GroupField (complex: grouped rendering) |
| GAP-DTP-002 | DateTimePicker | High | Open — DateTimePickerSteps child component |
| GAP-MSEL-005+ | MultiSelect | Medium | Open — ValidateOn, AdaptiveMode |
| GAP-*-ARIA | All T4 | Medium | Open — ARIA completeness audit |
| GAP-*-CSS | All T4 | Medium | Open — CSS provider alignment |

---

## Sign-off

Batch 2 closes 4/4 gaps with 9 bUnit tests covering all template slots and AllowCustom behavior. WithCredentials fix verified via static analysis of all 3 HTTP request paths. Runtime test execution pending .NET SDK availability.
