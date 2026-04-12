# DataSheet Coverage Summary

Last updated: 2026-04-12 (S05 Wave 1)

## Parameter Coverage

| Parameter | bUnit test? | Demo scenario? | Spec documented? | Notes |
|---|---|---|---|---|
| Data | Yes | Yes | Yes | |
| KeyField | Yes | Yes | Yes | Defaults to "Id" |
| OnSaveAll | Yes | Yes | Yes | |
| OnRowChanged | Yes | Yes | Yes | |
| OnValidate | Yes | Yes | Yes | |
| IsSaving | Yes | Yes | Yes | W1: paste-during-save guard added (SA-08) |
| AllowAddRow | Yes | Yes | Yes | W1: ActivateCell on first editable col added (SA-03) |
| AllowDeleteRow | Yes | Yes | Yes | |
| AllowBulkPaste | Yes | Yes | Yes | |
| EmptyStateMessage | Yes | Yes | Yes | |
| Height | No | Yes | Yes | Visual-only; no bUnit assertion |
| IsLoading | Yes | Yes | Yes | |
| EnableVirtualization | Partial | Yes | Yes | W1: threshold note + new Virtualization demo (UD-02) |
| AriaLabel | Yes | Yes | Yes | |
| ChildContent | Yes | Yes | Yes | Column definitions |
| ToolbarTemplate | No | No | Yes | No dedicated bUnit test or demo |

## Event Coverage

| Event | bUnit test? | Demo scenario? | Spec documented? | Notes |
|---|---|---|---|---|
| OnSaveAll | Yes | Yes | Yes | |
| OnRowChanged | Yes | Yes | Yes | |
| OnValidate | Yes | Yes | Yes | |

## Keyboard Shortcut Coverage

| Shortcut | bUnit test? | Demo scenario? | Spec documented? | Notes |
|---|---|---|---|---|
| Arrow Up | Yes | Yes | Yes | |
| Arrow Down | Yes | Yes | Yes | |
| Arrow Left | Yes | Yes | Yes | |
| Arrow Right | Yes | Yes | Yes | |
| Tab | Yes | Yes | Yes | V07 row-wrap + grid-exit |
| Shift+Tab | Yes | Yes | Yes | V07 reverse wrap |
| Enter (edit mode) | Yes | Yes | Yes | Commit + move down |
| Enter (not editing) | Yes | Yes | Yes | V07.1 enters edit mode |
| F2 | Yes | Yes | Yes | |
| Escape | Yes | Yes | Yes | |
| Delete | Yes | Partial | Yes | Single-cell only; range-Delete deferred to V03 |
| Ctrl+S | Yes | Yes | Yes | |
| Ctrl+Z | Yes | Yes | Yes | W1: undo buffer cleared on reset (SA-04) |
| Ctrl+C | No | Yes | Yes | JS interop -- not bUnit-testable |
| Ctrl+V | Yes | Yes | Yes | W1: paste-during-save guard (SA-08), round-trip demo (EU-02) |
| Ctrl+D | Yes | Partial | Yes | Row-level only; range fill-down deferred to V03 |
| Ctrl+A | No | No | Yes | Deferred to V03 (TASK-DS-014) |
| Space (checkbox) | Yes | Yes | Yes | V07.3 |
| Printable char | Yes | Yes | Yes | V07.2 |

## Cell State Lifecycle Coverage

| State | bUnit test? | Demo scenario? | Spec documented? | Notes |
|---|---|---|---|---|
| Pristine | Yes | Yes | Yes | |
| Dirty | Yes | Yes | Yes | |
| Invalid | Yes | Yes | Yes | |
| Saving | Yes | Yes | Yes | V02.2/V05.1 transient state |
| Saved | Yes | Yes | Yes | V02.2/V05.1 transient state + delay |

## aria-live Announcement Coverage

| Trigger | bUnit test? | Spec documented? | Notes |
|---|---|---|---|
| Dirty count change | Yes | Yes | V07.9 |
| Save All initiated | Yes | Yes | W1: "Saving changes." added (SA-13) |
| Save All completed | Yes | Yes | "Changes saved successfully." |
| Save blocked (validation) | Yes | Yes | W1: error count added (SA-13) |
| Save failed (exception) | Yes | Yes | W1: "Save failed. An error occurred." added (SA-13) |
| Reset | Yes | Yes | "All changes have been reset." |

## Gap Records Resolved in Wave 1

| Gap ID | Task | Status |
|---|---|---|
| WS-01 | TASK-DS-001 | Resolved (this file) |
| SA-01 | TASK-DS-002 | Resolved (tabindex="0") |
| UD-02, EU-01 | TASK-DS-003 | Resolved (spec threshold + Virtualization demo) |
| SA-08, EU-03 | TASK-DS-004 | Resolved (IsSaving paste guard + test) |
| SA-13, EU-05 | TASK-DS-005 | Resolved (3 aria-live announcements + tests) |
| SA-03 | TASK-DS-006 | Resolved (AddRow ActivateCell + test) |
| SA-04 | TASK-DS-007 | Resolved (undo buffer cleared on reset + test) |
| EU-02 | TASK-DS-008 | Resolved (copy-paste round-trip demo) |
