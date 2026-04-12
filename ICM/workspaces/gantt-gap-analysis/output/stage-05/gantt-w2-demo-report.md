# Gantt Wave 2 — Demo Coverage Report (Lane L2)

**Date:** 2026-04-12
**Scope:** W4-INT-08, 09, 10, 11, 12

---

## Pre-Existing Demo Pages (6 pages)

| Page | Route | Content |
|---|---|---|
| Overview.razor | `/components/gantt` | Basic usage with flat data, columns, views |
| Hierarchical.razor | `/components/gantt/hierarchical` | Three-level nesting with ParentId |
| Editing.razor | `/components/gantt/editing` | Inline edit with CUD handlers |
| Templates.razor | `/components/gantt/templates` | TaskTemplate, column Template, GanttToolBarTemplate |
| Views.razor | `/components/gantt/views` | Day/Week/Month/Year view switching with SlotWidth |
| Features.razor | `/components/gantt/features` | Sorting, filtering, dependency lines |

---

## New Demo Pages Created

### W4-INT-08 — Milestone Demo (P1) — NEW

**File:** `samples/Marilo.Demo/Pages/Components/Gantt/Milestones.razor`
**Route:** `/components/gantt/milestones`

Demonstrates zero-duration milestones (Start == End). Data includes:
- 3 project phases with regular tasks
- 3 milestone tasks: "Requirements Sign-Off", "Design Approval" (nested), and "Go-Live" (root-level)
- Explanatory text about diamond rendering
- Code snippet showing how to create a milestone (set Start == End)

**Status: CLOSED**

### W4-INT-09 — Summary Task Demo (P1) — NEW

**File:** `samples/Marilo.Demo/Pages/Components/Gantt/SummaryTasks.razor`
**Route:** `/components/gantt/summary-tasks`

Demonstrates automatic summary aggregation:
- Multi-level hierarchy: Q1 Release > Backend/Frontend/QA phases > leaf tasks
- Second summary group (Q2 Release) to show multiple independent summaries
- Month view default to show full range
- OnUpdate handler for editing leaf tasks
- Explanatory text about automatic Start/End/PercentComplete computation
- Code snippet explaining the `mar-gantt__bar--summary` CSS class

**Status: CLOSED**

### W4-INT-10 — OnStateChanged Demo (P2) — NEW

**File:** `samples/Marilo.Demo/Pages/Components/Gantt/State.razor`
**Route:** `/components/gantt/state`

Demonstrates state change tracking:
- OnStateChanged handler logs PropertyName + detail to a scrollable panel
- Shows actual PropertyName values (SortDescriptor, FilterValues, View, etc.)
- "Reset State" button calls `SetStateAsync(null)` 
- "Apply Preset" button sets a sort + view via `SetStateAsync`
- Sortable + Filterable columns so user can trigger state changes interactively
- Three timeline views (Week, Month, Year) for view-change testing

**Status: CLOSED**

### W4-INT-11 — Refresh Data Demo (P2) — NEW

**File:** `samples/Marilo.Demo/Pages/Components/Gantt/RefreshData.razor`
**Route:** `/components/gantt/refresh-data`

Demonstrates both data refresh mechanisms:
- "Add Task (New Reference)" — creates a new list instance (automatic detection)
- "Add Task (Rebind)" — mutates in place, calls `Rebind()`
- "Remove First Task" — removes task and children, calls `Rebind()`
- Task count and last action display for verification
- Code snippet explaining both approaches

**Status: CLOSED**

### W4-INT-12 — Column Chooser / VisibleColumns Demo (P2) — INCLUDED IN STATE PAGE

**File:** `samples/Marilo.Demo/Pages/Components/Gantt/State.razor` (second DemoSection)
**Route:** `/components/gantt/state#column-chooser-visiblecolumns`

Demonstrates the column chooser:
- `ShowColumnChooser="true"` enables the toolbar button
- 4 columns (Title, PercentComplete, Start, End) that can be toggled
- Code snippet explaining VisibleColumns in GanttState

**Status: CLOSED**

---

## Existing Demo Fix

### Features.razor — Fixed invalid parameter

**File:** `samples/Marilo.Demo/Pages/Components/Gantt/Features.razor`

- Replaced `Filterable="true"` (not a MariloGantt parameter) with `FilterMode="GanttFilterMode.FilterRow"` (the actual parameter)
- Added `Filterable="true"` on individual GanttColumn instances so the filter row is functional
- Removed unused `_filterText` field (eliminated CS0169 warning)
- Renamed demo section title from "Text Filter" to "Filter Row" to match the actual feature

---

## Verification

Build: `dotnet build Marilo.slnx` — 0 errors, 0 new warnings.

All demo pages use parameters verified against the actual source code:
- `MariloGantt.razor.cs`: TItem, Data, IdField, ParentIdField, Height, Width, Sortable, FilterMode, ShowColumnChooser, @bind-View, OnStateChanged, OnUpdate, OnDelete, OnCreate
- `GanttState.cs`: GanttState<TItem>, GanttSortDescriptor, GanttStateEventArgs<TItem>, GanttFilterMode
- `GanttView.cs`: GanttView.Week, GanttView.Month, GanttView.Year

---

## Summary

| Gap ID | Description | Status |
|---|---|---|
| W4-INT-08 | Milestone demo | CLOSED (new page) |
| W4-INT-09 | Summary task demo | CLOSED (new page) |
| W4-INT-10 | OnStateChanged demo | CLOSED (new page) |
| W4-INT-11 | Refresh data demo | CLOSED (new page) |
| W4-INT-12 | Column chooser demo | CLOSED (section in State page) |

**5 of 5 L2 demo coverage gaps closed.**

**Total new demo pages: 4** (Milestones, SummaryTasks, State, RefreshData)
**Total demo pages after Wave 2: 10** (6 pre-existing + 4 new)
