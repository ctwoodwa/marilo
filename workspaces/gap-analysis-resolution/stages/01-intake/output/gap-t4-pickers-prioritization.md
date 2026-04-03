# T4 Pickers Prioritization View

**Date:** 2026-04-03
**Source:** GAP_ANALYSIS_RESOLUTION_PLAN.md T4 section

---

## Overview

| Component | Gap Count | High | Medium | Low | Dependencies |
|-----------|-----------|------|--------|-----|-------------|
| MariloColorPicker | 6 | 1 (standalone components) | 2 (views API, CSS) | 3 (AdaptiveMode, etc.) | None |
| MariloDateRangePicker | 8 | 2 (events, multi-view) | 3 (AdaptiveMode, templates, methods) | 3 (CSS, ARIA) | Depends on calendar engine |
| MariloDateTimePicker | 6 | 2 (events, Steps child) | 2 (ValidateOn, typed input) | 2 (CSS, ARIA) | Depends on calendar engine |
| MariloTimePicker | 7 | 1 (PopupClass bug) | 3 (event args, ValidateOn, InputMode) | 3 (ARIA, CSS provider) | None |
| MariloFileUpload | 3 | 0 | 2 (DropZoneId, template context) | 1 (CSS) | JS interop for DropZoneId |
| MariloUpload | 5 | 1 (chunk resume) | 3 (templates, WithCredentials, DropZoneId) | 1 (UploadChunkSettings) | JS interop for DropZoneId |
| MariloMultiSelect | 10 | 3 (events, AllowCustom, templates) | 4 (GroupField, child API, Rebind, ValueMapper) | 3 (AdaptiveMode, ScrollMode, naming) | None |
| **Total** | **45** | **10** | **19** | **16** | |

---

## Cross-Cutting Gaps (affect multiple components)

| Gap | Affected | Severity | Batch |
|-----|----------|----------|-------|
| AdaptiveMode parameter | All 7 pickers | Low | Batch 3 |
| ValidateOn / EditContext | DateTimePicker, TimePicker | Medium | Batch 2 |
| Cancellable OnOpen/OnClose args | DateRangePicker, DateTimePicker, MultiSelect | Medium | Batch 1 |
| role=combobox on inputs | DateRangePicker, DateTimePicker, TimePicker | Low | Batch 3 |
| aria-controls/activedescendant | DateRangePicker, DateTimePicker, TimePicker | Low | Batch 3 |
| CSS provider specificity | DateRangePicker, DateTimePicker | Low | Batch 3 |

---

## Recommended Batches

### Batch 1: Events & Core API (High-severity gaps)
**Scope:** 10 gaps across 5 components
**Focus:** Missing spec-aligned events, cancellable event args, critical bug fixes

| Component | Gaps | Description |
|-----------|------|-------------|
| MariloMultiSelect | OnChange, OnRead, OnOpen, OnClose, OnBlur events | Missing event lifecycle |
| MariloDateTimePicker | OnChange, OnOpen, OnClose, OnBlur, OnCalendarCellRender | All spec events missing |
| MariloDateRangePicker | OnChange, OnOpen, OnClose events | Missing event lifecycle |
| MariloTimePicker | PopupClass bug fix, cancellable OnOpen/OnClose | Bug + event upgrade |
| MariloUpload | Chunk resume fix (paused offset tracking) | Data integrity bug |

**Dependencies:** None (events are additive, no breaking changes)
**Estimated effort:** Medium-High

### Batch 2: Templates & API Completeness (Medium-severity gaps)
**Scope:** 12 gaps across 4 components
**Focus:** Missing template slots, child component APIs, method gaps

| Component | Gaps | Description |
|-----------|------|-------------|
| MariloMultiSelect | 5 template slots, AllowCustom, GroupField | Core API completeness |
| MariloUpload | 3 template slots, WithCredentials | Template rendering + HTTP config |
| MariloDateTimePicker | DateTimePickerSteps child, ValidateOn | Configuration depth |
| MariloColorPicker | ColorPickerViews child API, standalone sub-components | Advanced views configuration |

**Dependencies:** Batch 1 (events needed for template interaction patterns)
**Estimated effort:** Medium

### Batch 3: Cross-Cutting & Polish (Low-severity gaps)
**Scope:** 16+ gaps
**Focus:** AdaptiveMode, ARIA completeness, CSS provider alignment, naming

| Category | Count | Description |
|----------|-------|-------------|
| AdaptiveMode | 7 | Add parameter to all pickers |
| ARIA/combobox | 6 | Align with WAI-ARIA 1.2 combobox pattern |
| CSS provider | 3 | Component-specific provider methods |
| Naming | 1 | MaxVisibleTags vs MaxAllowedTags |

**Dependencies:** Batches 1 and 2 (don't polish before core is complete)
**Estimated effort:** Low-Medium

---

## Recommended First Batch

**Start with Batch 1** — it addresses the highest-severity gaps (events and bugs) and unblocks Batch 2 (templates need events for complete interaction patterns).

Priority within Batch 1:
1. MariloTimePicker PopupClass bug (quick fix, restores functionality)
2. MariloUpload chunk resume (data integrity)
3. MariloMultiSelect events (most gaps, most users)
4. MariloDateTimePicker events
5. MariloDateRangePicker events
