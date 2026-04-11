# Spec Gap List: MariloAllocationScheduler

**Audit date:** 2026-04-05
**Source:** src/Marilo.Components/DataDisplay/AllocationScheduler/
**Spec:** docs/component-specs/allocation-scheduler/

## Inventory Summary

| Category | Source count | Spec count | Match |
|----------|-------------|------------|-------|
| Parameters (MariloAllocationScheduler) | 32 | 31 | 1 undocumented |
| Parameters (AllocationResourceColumn) | 9 | 9 | Full match |
| Events | 18 | 15 + 3 two-way | Partial (see SPEC-AS-008) |
| Public methods | 8 | 8 | Type mismatches |
| RenderFragment slots | 5 | 4 | 1 undocumented |
| CSS provider methods | 14 | 14 | Full match |
| Enums | 8 | 6 | 2 undocumented sections |
| Models/EventArgs | 21 | 21 | 1 property missing |

## Gap Records

---

### SPEC-AS-001

**ID:** SPEC-AS-001
**Type:** undocumented
**Parameter/Event:** ResourceRowTemplate
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | ResourceRowTemplate |
| Type | missing | `RenderFragment<TResource>?` |
| Default | missing | `null` |
| Description | missing | Fallback template for resource metadata cells when no column-level Template is set |

**Recommended action:** Add to templates.md RenderFragment slots table.
**Delegated to:** spec update only

---

### SPEC-AS-002

**ID:** SPEC-AS-002
**Type:** mismatch
**Parameter/Event:** CellEditedArgs.BucketEnd
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing from events.md properties table | BucketEnd |
| Type | missing | `DateTime` |
| Default | N/A | N/A |
| Description | missing | End of the time bucket for the edited cell |

**Recommended action:** Add BucketEnd property to CellEditedArgs table in events.md.
**Delegated to:** spec update only

---

### SPEC-AS-003

**ID:** SPEC-AS-003
**Type:** mismatch
**Parameter/Event:** Public method return types
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Rebind, Refresh, NavigateTo, NavigateForward, NavigateBack, NavigateToToday, ClearSelection | Same |
| Type | Not specified (overview.md table has no return type column) | `Task` (all are async) |
| Default | N/A | N/A |
| Description | overview.md shows method names and descriptions only | All public methods are `async Task` |

**Recommended action:** Add return type column to the public methods table in overview.md. All seven methods return `Task`.
**Delegated to:** spec update only

---

### SPEC-AS-004

**ID:** SPEC-AS-004
**Type:** mismatch
**Parameter/Event:** GetSelectedCells() return type
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | GetSelectedCells() | GetSelectedCells() |
| Type | `IEnumerable<AllocationCellRef>` | `IReadOnlyList<AllocationCellRef>` |
| Default | N/A | N/A |
| Description | Returns the current selection | Returns the current selection |

**Recommended action:** Update overview.md to show `IReadOnlyList<AllocationCellRef>` return type. The source intentionally uses `IReadOnlyList` to guarantee count and indexer access.
**Delegated to:** spec update only

---

### SPEC-AS-005

**ID:** SPEC-AS-005
**Type:** undocumented
**Parameter/Event:** ShowComparisonPanel, ShowCriticalPath -- missing from parameter tables
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Mentioned in scenario-planning.md narrative only | ShowComparisonPanel, ShowCriticalPath |
| Type | Not in any parameter table | `bool`, `bool` |
| Default | Not specified | `false`, `false` |
| Description | Narrative text in scenario-planning.md | Toggle comparison panel / critical path highlighting |

**Recommended action:** Add both parameters to the overview.md parameter table under "Scenario Planning" section. They already appear in scenario-planning.md narrative but are not in any formal parameter table.
**Delegated to:** spec update only

---

### SPEC-AS-006

**ID:** SPEC-AS-006
**Type:** undocumented
**Parameter/Event:** DeltaDisplayMode enum -- values not in dedicated section
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Referenced in overview.md parameter table | DeltaDisplayMode |
| Type | Mentioned as a parameter type | Enum: Value, Percentage, StatusIcon |
| Default | `Value` documented | `DeltaDisplayMode.Value` |
| Description | No enum values table in any doc | Controls how variance between actuals and targets is shown |

**Recommended action:** Add DeltaDisplayMode enum values table to overview.md or a dedicated enums section.
**Delegated to:** spec update only

---

### SPEC-AS-007

**ID:** SPEC-AS-007
**Type:** undocumented
**Parameter/Event:** AllocationUnit enum -- no formal values table
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Referenced in data-binding.md and business-objects.md | AllocationUnit |
| Type | Used in AllocationRecord.Unit | Enum: Hours, Currency |
| Default | N/A | N/A |
| Description | Appears in code samples but no enum table | The unit of an allocation value |

**Recommended action:** Add AllocationUnit enum values table to data-binding.md alongside AllocationRecord.
**Delegated to:** spec update only

---

### SPEC-AS-008

**ID:** SPEC-AS-008
**Type:** undocumented
**Parameter/Event:** Two-way binding callbacks in events.md
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Documented in data-binding.md only | ViewGrainChanged, VisibleStartChanged, ActiveSetIdChanged |
| Type | Described as two-way binding patterns | `EventCallback<TimeGranularity>`, `EventCallback<DateTime>`, `EventCallback<Guid>` |
| Default | N/A | N/A |
| Description | data-binding.md shows usage, events.md does not list them | Two-way binding callbacks for ViewGrain, VisibleStart, ActiveSetId |

**Recommended action:** Add a "Two-Way Binding Callbacks" section to events.md linking to data-binding.md, or add the three callbacks to the events table.
**Delegated to:** spec update only

---

### SPEC-AS-009

**ID:** SPEC-AS-009
**Type:** undocumented
**Parameter/Event:** Accessibility specification
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | No dedicated accessibility doc | Full ARIA implementation in .razor |
| Type | N/A | role=grid, role=row, role=gridcell, role=columnheader, aria-selected, aria-disabled, aria-readonly, aria-label |
| Default | N/A | N/A |
| Description | Keyboard support mentioned in overview.md, no ARIA spec | Complete ARIA grid pattern with per-cell labeling |

**Recommended action:** Create `docs/component-specs/allocation-scheduler/accessibility.md` with ARIA roles table, keyboard interaction matrix, and screen reader expectations. Source: component-builder Stage 01 requirements (accessibility section).
**Delegated to:** spec update only

---

## Priority Summary

| Priority | Count | Records |
|----------|-------|---------|
| P1 (blocking) | 0 | -- |
| P2 (this phase) | 0 | -- |
| P3 (next phase) | 0 | -- |

## Resolution Log

All 9 gaps resolved on 2026-04-05:

| ID | Resolution |
|----|-----------|
| SPEC-AS-001 | Added `ResourceRowTemplate` to templates.md |
| SPEC-AS-002 | Added full `CellEditedArgs` property table (incl. `BucketEnd`) to events.md |
| SPEC-AS-003 | Added `Return Type` column to methods table in overview.md — all async methods show `Task` |
| SPEC-AS-004 | Corrected `GetSelectedCells()` return type to `IReadOnlyList<AllocationCellRef>` in overview.md |
| SPEC-AS-005 | Added `ShowComparisonPanel` and `ShowCriticalPath` to overview.md parameter table |
| SPEC-AS-006 | Added `DeltaDisplayMode` enum values table to overview.md |
| SPEC-AS-007 | Added `AllocationUnit` enum values table to data-binding.md |
| SPEC-AS-008 | Added "Two-Way Binding Callbacks" section to events.md |
| SPEC-AS-009 | Created `accessibility.md` with ARIA roles, states, keyboard matrix, and screen reader expectations |

## Spec-Ahead Items (documented but not implemented)

None found. All documented parameters have corresponding source implementations.

## Audit Checks

| Check | Status |
|-------|--------|
| All source parameters inventoried | PASS -- 32 MariloAllocationScheduler + 9 AllocationResourceColumn + 18 events + 5 slots + 8 methods |
| All spec parameters inventoried | PASS -- 31 parameters + 15 events + 3 two-way callbacks + 4 slots + 8 methods |
| No gap record missing a type classification | PASS -- all 9 gaps have type (5 undocumented, 4 mismatch) |
| Priority order justified | PASS -- P2 = incomplete or incorrect spec info; P3 = coverage gaps for completeness |
| No spec content duplicated in output | PASS -- output references spec locations, does not copy content |

---

## 2026-04-11 orchestrator wave 1 (subagent dispatch)

**Audit date:** 2026-04-11
**Session:** marilo-grid-pipeline-2026-04-11-1200
**Worker:** w-allocation-scheduler-delivery (subagent)
**Stage:** 01-spec-review, Wave 1
**Source tree audited:** src/Marilo.Components/DataDisplay/AllocationScheduler/ (MariloAllocationScheduler.razor, MariloAllocationScheduler.razor.cs ~1700 LOC, AllocationResourceColumn.razor.cs), plus src/Marilo.Core/Models/AllocationSchedulerModels.cs, src/Marilo.Core/Enums/AllocationSchedulerEnums.cs, src/Marilo.Core/BusinessLogic/Enums/BusinessLogicEnums.cs.
**Spec tree audited:** docs/component-specs/allocation-scheduler/ (16 .md files).
**Cross-reference:** .wolf/cerebrum.md known-source-behavior list.

### Inventory Delta Since 2026-04-05

| Category | Prior baseline | Current source | Notes |
|----------|---------------|----------------|-------|
| MariloAllocationScheduler parameters | 32 | 50+ | Time-column sizing (6), Jump-to-date (1), MinVisibleColumns + obsolete DefaultRangeLength (2), VisibleColumnOverride (1), ShowComparisonPanel/ShowCriticalPath (already logged as SPEC-AS-005) |
| AllocationResourceColumn parameters | 9 | 9 | Pinned is `[Obsolete]` — theming still references `isPinned` parameter |
| Events | 18 | 19 | Adds `OnTimeColumnResized` |
| Enums | 8 | 8 | ScenarioStatus values Draft/Shared/Approved confirmed — spec file uses wrong type name `AllocationScenarioStatus` |
| Models | 21 | 21 | `AllocationSet` has 11 members including DisplayLabel, Description, ParentBaselineId, CreatedBy, CreatedDate, FinalizedDate, IsLocked, Status, Type — partially covered in scenario-planning.md |

### New Gap Records

---

### SPEC-AS-W1-010

**ID:** SPEC-AS-W1-010
**Type:** naming-mismatch
**Parameter/Event:** `ScenarioStatus` enum type name
**Priority:** P1 (blocking — consumers copy-pasting spec code will hit compile errors)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `AllocationScenarioStatus` (scenario-planning.md lines 54, 96, 99, 246, 254) | `ScenarioStatus` |
| Namespace | not specified in spec | `Marilo.Core.BusinessLogic.Enums` |
| Values | `Draft`, `Shared`, `Approved` | `Draft`, `Shared`, `Approved` (match) |
| Usage | `AllocationSet.Status` typed as `AllocationScenarioStatus` in spec code blocks | `AllocationSet.Status` typed as `ScenarioStatus` in source |

**Recommended action:** Rename every occurrence of `AllocationScenarioStatus` in scenario-planning.md to `ScenarioStatus`. Affects 5 lines including two `Status = AllocationScenarioStatus.Shared` / `.Draft` code block examples. The enum values themselves (Draft/Shared/Approved) are correct — the cerebrum-flagged "not Locked" concern is already reflected correctly in the spec values.
**Delegated to:** spec update only (scenario-planning.md)

---

### SPEC-AS-W1-011

**ID:** SPEC-AS-W1-011
**Type:** spec-ahead (stale)
**Parameter/Event:** `AllocationResourceColumn.Pinned` still present as `isPinned` parameter in theming CSS provider method signature
**Priority:** P1 (blocking — documents an obsolete feature as current)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | theming.md line 22: `AllocationSchedulerResourceColumnClass` with parameter `isPinned` | Pinned property is `[Obsolete("Resource column pinning is obsolete. The left pane is the frozen region in the splitter-based layout.")]` on `AllocationResourceColumn<TResource>` |
| Behavior | Implies pinned columns render differently | Pinned is not honored — splitter-based dual-pane layout replaces it (already covered in splitter-layout.md, but theming spec contradicts this) |

**Recommended action:** Remove `isPinned` parameter from the `AllocationSchedulerResourceColumnClass` row in theming.md. Confirm no CSS provider implementation still reads it. If any provider still accepts the parameter, open a follow-up source-side change. Add an explicit note to theming.md calling out that resource column pinning is obsolete in the splitter-based layout.
**Delegated to:** spec update + provider code audit (not in this stage's scope — flag for sync-check stage)

---

### SPEC-AS-W1-012

**ID:** SPEC-AS-W1-012
**Type:** source-ahead
**Parameter/Event:** Time-column sizing API (6 parameters + 1 event)
**Priority:** P1 (blocking — entire feature missing from spec)

| Field | In Spec | In Source |
|-------|---------|-----------|
| `TimeColumnWidth` | not documented | `int`, default `80`, min 40. Fixed width in pixels for every time column. Overview.md parameter table has no row for this. |
| `AllowTimeColumnResize` | not documented | `bool`, default `false`. Enables Excel-style column-header drag resize on time columns. |
| `AutoFitOnDoubleClick` | not documented | `bool`, default `true`. Double-click column header border snaps to widest rendered cell. Only applies when `AllowTimeColumnResize` is true. |
| `MinTimeColumnWidth` | not documented | `int`, default `48`. Minimum enforced during resize drag. |
| `MaxTimeColumnWidth` | not documented | `int`, default `0` (no max). |
| `OnTimeColumnResized` | not documented | `EventCallback<Dictionary<int, int>>`. Fires after user resizes, payload maps column index to new pixel width. |
| JS interop | not documented | `AllocationSchedulerInterop.initTimeColumnResize` is invoked in OnAfterRenderAsync firstRender when `AllowTimeColumnResize` is true. |

**Recommended action:** Add a new "Time Column Sizing" section to overview.md (or create a dedicated `time-column-sizing.md` spec if the behavior warrants). Document all 6 parameters + 1 event, the default behavior, the Excel-style AutoFit gesture, and the JS interop entry point name.
**Delegated to:** spec update only (overview.md or new spec file)

---

### SPEC-AS-W1-013

**ID:** SPEC-AS-W1-013
**Type:** source-ahead
**Parameter/Event:** `MinVisibleColumns` + obsolete `DefaultRangeLength`
**Priority:** P2 (this phase — spec shows obsolete parameter as current)

| Field | In Spec | In Source |
|-------|---------|-----------|
| `MinVisibleColumns` | not documented anywhere | `int`, default `3`. Floor for the dynamic column count computed by the ResizeObserver. |
| `DefaultRangeLength` | overview.md line 215 still lists this as current: `int` default `3` "Number of units for the default visible range" | `[Obsolete("Use MinVisibleColumns instead. DefaultRangeLength now acts as a minimum floor, not an exact count.")]` — now just a pass-through setter/getter to MinVisibleColumns |

**Recommended action:** (a) In overview.md, replace the `DefaultRangeLength` row with `MinVisibleColumns` and note that `DefaultRangeLength` is obsolete and aliased. (b) Explain that the actual column count is now dynamic (ResizeObserver-driven), not fixed, and `MinVisibleColumns` is only a floor.
**Delegated to:** spec update only (overview.md)

---

### SPEC-AS-W1-014

**ID:** SPEC-AS-W1-014
**Type:** source-ahead
**Parameter/Event:** `VisibleColumnOverride`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | not documented | `VisibleColumnOverride` |
| Type | N/A | `int?`, default `null` |
| Purpose | N/A | Forces a fixed number of equally-spaced columns that fill the timeline pane width regardless of view mode. When null, Month/Week use all visible buckets and Day uses fixed 80px columns. |

**Recommended action:** Add to overview.md parameter table, directly after `MinVisibleColumns`. Explain its interaction with the ResizeObserver-driven dynamic column fill.
**Delegated to:** spec update only (overview.md)

---

### SPEC-AS-W1-015

**ID:** SPEC-AS-W1-015
**Type:** source-ahead
**Parameter/Event:** `ShowJumpToDate` + toolbar jump-to-date picker
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | not documented | `ShowJumpToDate` |
| Type | N/A | `bool`, default `true` |
| Behavior | N/A | When true, renders a date picker and Jump button in the toolbar for direct date navigation (skipping NavigateForward/Back stepping). Internal field `_jumpToDate` defaults to `DateTime.Today`. |

**Recommended action:** Add `ShowJumpToDate` to overview.md parameter table under the "Display" / toolbar group. Add a short "Jump to Date" subsection describing the toolbar affordance.
**Delegated to:** spec update only (overview.md)

---

### SPEC-AS-W1-016

**ID:** SPEC-AS-W1-016
**Type:** source-ahead
**Parameter/Event:** Grouped timeline headers (`TimelineHeaderGroup`)
**Priority:** P2 (this phase — visible structural feature not specified)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | not documented | `TimelineHeaderGroup` record, `_headerGroups` internal list, `ComputeTimelineHeaderGroups()` method |
| Rendering rule | not documented | When `HasTimelineHeaderGroups` is true, the resource-column header gets a `rowspan` so its header aligns with the bottom row of the two-row timeline header (e.g. months grouped by year). |
| BEM class | not documented in theming.md | Implied by source; should match `mar-allocation-scheduler__timeline-header-group` pattern per BEM spec |

**Recommended action:** Add a "Grouped Timeline Headers" subsection to overview.md or splitter-layout.md (splitter-layout covers layout math, so it's a natural place). Document the rowspan rule and which grain combinations trigger grouping (e.g. Month view grouped by Year). Add the BEM class to theming.md.
**Delegated to:** spec update only

---

### SPEC-AS-W1-017

**ID:** SPEC-AS-W1-017
**Type:** source-ahead
**Parameter/Event:** Current-period highlight
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | not documented | `IsCurrentPeriod` per-bucket flag + `mar-allocation-scheduler__col-current` CSS class |
| Behavior | not documented | Computed by `GetPeriodStart(DateTime.Today, _currentViewGrain)` matching the bucket's `Start`. The matching column gets the `col-current` class so CSS can highlight today's period. |
| Spec coverage | 0 | — |

**Recommended action:** Add a "Current Period Highlight" subsection to theming.md. Document the `mar-allocation-scheduler__col-current` class and the calculation rule (`GetPeriodStart` on today's date at `_currentViewGrain`). Add the CSS custom property used to color it.
**Delegated to:** spec update only (theming.md)

---

### SPEC-AS-W1-018

**ID:** SPEC-AS-W1-018
**Type:** source-ahead
**Parameter/Event:** Dynamic column fill via JS ResizeObserver + `MinColumnWidthsByGrain`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | partial — refresh-data.md line 72 mentions "internal `ResizeObserver`" in passing | `AllocationSchedulerInterop.initColumnResize` + `MinColumnWidthsByGrain` dictionary (Day=60, Week=80, Month=90, Quarter=100, Year=120) + `_visibleColumnCount` + `_paneObserverActive` + `_rightPaneId` DOM id |
| Behavior | not formally specified | JS ResizeObserver watches the right-pane div, computes `floor(paneWidth / minWidthForGrain)`, clamped to `MinVisibleColumns`, reports via DotNetObjectReference. Column count is dynamic, not a static range length. |

**Recommended action:** Add a "Dynamic Column Fill" subsection to splitter-layout.md (or a new sizing.md) covering the ResizeObserver algorithm, the per-grain minimum width table, the interaction with `MinVisibleColumns`, `VisibleColumnOverride`, and `TimeColumnWidth`, and the JS interop handshake.
**Delegated to:** spec update only

---

### SPEC-AS-W1-019

**ID:** SPEC-AS-W1-019
**Type:** source-ahead
**Parameter/Event:** JS drag-fill interop payload shape
**Priority:** P3 (next phase — lightly documented in keyboard-navigation.md only)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | keyboard-navigation.md line 187 documents payload as `{source: {resourceKey, bucketStart}, targets: [...]}` | Same shape is produced by `AllocationSchedulerInterop.initDragFill` JS module; consumed via `[JSInvokable]` on the component |
| Spec location | Only appears inside keyboard-navigation.md under "Drag-Fill via Keyboard" | Should appear in events.md as well, or in a JS interop reference |

**Recommended action:** Cross-link the payload shape from events.md (OnRangeEdited) back to keyboard-navigation.md, or promote it to a shared "JS Interop Reference" block in editing.md. The cerebrum lists this as a known source convention; making it formal spec content reduces drift risk.
**Delegated to:** spec update only

---

### SPEC-AS-W1-020

**ID:** SPEC-AS-W1-020
**Type:** source-ahead
**Parameter/Event:** `AllocationSet` model fields partially covered
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| `SetId`, `Name`, `Type`, `IsLocked`, `Status`, `FinalizedDate` | documented in scenario-planning.md | present |
| `DisplayLabel` | mentioned in scenario-planning.md narrative (line 284 "Custom label override") | `string?` property |
| `Description` | not mentioned | `string` (default empty) |
| `ParentBaselineId` | not mentioned | `Guid?` — the scenario's parent baseline reference |
| `CreatedBy` | not mentioned | `string` (default empty) |
| `CreatedDate` | not mentioned | `DateTime` |

**Recommended action:** Expand the `AllocationSet` reference block in scenario-planning.md (around lines 42-55) to cover all 11 fields with types, nullability, and lifecycle notes (e.g. CreatedDate vs FinalizedDate).
**Delegated to:** spec update only (scenario-planning.md)

---

### SPEC-AS-W1-021

**ID:** SPEC-AS-W1-021
**Type:** source-ahead
**Parameter/Event:** `ScenarioOverride.OverrideReason`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | not documented | `OverrideReason` |
| Type | N/A | `string` (default empty) |
| Purpose | N/A | Free-text reason stored on a ScenarioOverride for audit / display |

**Recommended action:** Add `OverrideReason` to the `ScenarioOverride` field table in scenario-planning.md.
**Delegated to:** spec update only (scenario-planning.md)

---

### Priority Summary (Wave 1 new records)

| Priority | Count | Records |
|----------|-------|---------|
| P1 (blocking) | 3 | SPEC-AS-W1-010, SPEC-AS-W1-011, SPEC-AS-W1-012 |
| P2 (this phase) | 6 | SPEC-AS-W1-013, SPEC-AS-W1-014, SPEC-AS-W1-015, SPEC-AS-W1-016, SPEC-AS-W1-017, SPEC-AS-W1-018 |
| P3 (next phase) | 3 | SPEC-AS-W1-019, SPEC-AS-W1-020, SPEC-AS-W1-021 |
| **Total new gaps** | **12** | |

### Cerebrum Cross-Reference Table

| Cerebrum-known behavior | Documented? | Gap ID |
|---|---|---|
| Splitter column-derived layout | Yes (splitter-layout.md) | — |
| Width pattern reuses DataGrid.Sizing (IColumnWidthProvider, GridLayoutContract) | Parameter listed in overview.md but no deeper spec | (out-of-scope — internal sizing contract) |
| Time-column fixed pixel sizing (TimeColumnWidth, AllowTimeColumnResize, AutoFitOnDoubleClick) | NO | SPEC-AS-W1-012 |
| Fully controlled component (never mutates own data) | Yes (editing.md "controlled-component contract") | — |
| Active-cell / fill handle rendering rules | Yes (selection.md "Active Cell vs. Selection", editing.md) | — |
| JS interop drag-fill payload shape | Partial (keyboard-navigation.md only) | SPEC-AS-W1-019 |
| Grouped timeline headers via TimelineHeaderGroup | NO | SPEC-AS-W1-016 |
| Dynamic column fill via ResizeObserver + MinColumnWidthsByGrain | Partial mention only (refresh-data.md) | SPEC-AS-W1-018 |
| Current-period highlight (IsCurrentPeriod + col-current class) | NO | SPEC-AS-W1-017 |
| Pinned obsolete on AllocationResourceColumn | Contradicted (theming.md still uses isPinned) | SPEC-AS-W1-011 |
| Resource header rowspan when HasTimelineHeaderGroups | NO | SPEC-AS-W1-016 |
| AllocationTarget uses PeriodStart/PeriodEnd | Correct (analysis-targets.md matches source) | — |
| AllocationRecord uses BucketStart/BucketEnd | Correct (data-binding.md, events.md, selection.md all match source) | — |
| ScenarioStatus enum = Draft/Shared/Approved (not Locked) | Values correct; type name wrong (AllocationScenarioStatus) | SPEC-AS-W1-010 |

### Audit Checks (Wave 1)

| Check | Status |
|-------|--------|
| All 16 topic specs read and cross-referenced | PASS |
| All ~50 source parameters inventoried against spec | PASS |
| All 19 source events inventoried against spec | PASS |
| Cerebrum known-source behaviors cross-checked (14 entries) | PASS |
| Naming-drift scan (BucketStart/PeriodStart, ScenarioStatus, Pinned) | PASS — 2 issues found (SPEC-AS-W1-010, SPEC-AS-W1-011) |
| Gap records follow SA / SRC / NM taxonomy | PASS |
| No source files modified | PASS — read-only audit |
| No spec files modified | PASS — findings only, no fixes applied |

