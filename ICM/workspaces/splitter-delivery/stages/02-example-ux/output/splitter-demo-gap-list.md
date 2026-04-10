# MariloSplitter — Demo Gap List

**Audit date:** 2026-04-10
**Existing demo page:** `samples/Marilo.Demo/Pages/Components/Splitter/Overview.razor`
**Current scenario count:** 9
**Target scenario count:** 9 (no gaps)

---

## Current Coverage

| # | Section | Scenario | Features Covered |
|---|---------|---------|-----------------|
| 1 | Overview | Basic Horizontal | Orientation, SplitterPane, Size |
| 2 | Overview | Vertical Orientation | SplitterOrientation.Vertical, Height |
| 3 | Constraints | Min / Max Size | Min, Max parameters |
| 4 | Constraints | Non-Resizable Pane | Resizable=false |
| 5 | Collapse | Collapsible Panes | Collapsible, OnCollapse, OnExpand, keyboard |
| 6 | Multiple Panes | Three Panes | Multi-pane layout |
| 7 | Multiple Panes | Nested Splitters | Nested horizontal+vertical |
| 8 | Events | Resize Events | OnResizeStart, OnResize, OnResizeEnd |
| 9 | Events/State | State Management | GetState(), SetState(), SplitterState |
| 10 | Styling | Custom Bar Size | CSS variable override |

## Assessment

The Splitter demo is **comprehensive** — all implemented parameters, events, and features are demonstrated with interactive controls. No gaps remain.
