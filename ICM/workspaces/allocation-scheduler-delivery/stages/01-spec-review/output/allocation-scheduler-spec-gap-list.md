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
