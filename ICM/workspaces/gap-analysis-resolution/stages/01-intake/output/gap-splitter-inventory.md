# Gap Inventory: MariloSplitter

> Imported: 2026-04-03
> Analysis mode: Reconstructed (code exists before gap analysis)
> Total gaps: ~10 (3 Critical, 4 High, 3 Medium)

---

## Component Inventory

| Attribute | Value |
|-----------|-------|
| **Source files** | `MariloSplitter.razor` (500 lines), `MariloSplitterPane.razor` (68 lines) |
| **Code-behind partials** | None |
| **Public parameters (Splitter)** | 14 (Orientation, Width, Height, Collapsible, AriaLabel, OnResize, OnResizeStart, OnResizeEnd, OnCollapse, OnExpand, FirstPaneSize, FirstPane, SecondPane, ChildContent) |
| **Public parameters (Pane)** | 8 |
| **Tests** | None found |
| **Demos** | No demo pages found |
| **Spec** | `docs/component-specs/splitter/overview.md` |

---

## Gap Summary

The spec describes a Splitter with SplitterPanes child wrapper, per-pane Size/Min/Max/Collapsible/Collapsed/Resizable parameters, horizontal and vertical orientation, state save/restore (GetState/SetState), events (resize, collapse/expand), and Class parameter. The implementation is relatively mature at 500 lines with pane registration, keyboard resize, collapse toggle, drag overlay, and aria attributes. Key gaps are in the pane wrapper API shape, state management methods, missing tests, and missing demos.

### GAP-SPLITTER-001: Missing SplitterPanes wrapper component

**Area:** MariloSplitter
**Severity:** Critical
**Theme:** api-surface-mismatch
**Source:** splitter/overview.md -- code examples show `<SplitterPanes>` wrapper

**Target behavior:** `SplitterPanes` wrapper tag contains `SplitterPane` children per spec API shape.
**Current behavior:** Panes register via CascadingValue directly through ChildContent; no SplitterPanes wrapper.
**Impact:** Consumer code does not match documented API structure.
**Recommended direction:** Add SplitterPanes as a pass-through wrapper or document the current approach as the supported API.
**Status:** Open

---

### GAP-SPLITTER-002: Missing GetState/SetState methods

**Area:** MariloSplitter
**Severity:** Critical
**Theme:** missing-public-method
**Source:** splitter/overview.md -- Splitter Reference and Methods

**Target behavior:** `GetState()` returns current pane sizes/collapsed state; `SetState()` restores a saved state.
**Current behavior:** No public state management methods visible in the 500-line source.
**Impact:** Cannot persist or restore splitter layout across sessions.
**Recommended direction:** Add GetState/SetState methods returning/accepting a SplitterState object.
**Status:** Open

---

### GAP-SPLITTER-003: Missing Class parameter

**Area:** MariloSplitter
**Severity:** High
**Theme:** missing-css-class-param
**Source:** splitter/overview.md -- Parameters table

**Target behavior:** `Class` parameter renders custom CSS class on the splitter container.
**Current behavior:** Not explicitly listed; may inherit from MariloComponentBase.
**Impact:** Cannot apply custom styling classes to specific splitter instances.
**Recommended direction:** Verify base class provides Class; if not, add it.
**Status:** Open

---

### GAP-SPLITTER-004: Missing SplitterOrientation enum alignment

**Area:** MariloSplitter
**Severity:** High
**Theme:** api-surface-mismatch
**Source:** splitter/overview.md -- uses `SplitterOrientation.Horizontal`

**Target behavior:** `Orientation` parameter uses `SplitterOrientation` enum.
**Current behavior:** Uses `StackDirection` enum instead.
**Impact:** Consumer code using spec-documented enum name will not compile.
**Recommended direction:** Add SplitterOrientation enum or alias, or document StackDirection as the supported type.
**Status:** Open

---

### GAP-SPLITTER-005: Missing per-pane Min/Max parameters

**Area:** MariloSplitterPane
**Severity:** High
**Theme:** missing-parameter
**Source:** splitter/overview.md -- "set their Size, Min, Max, Collapsible, and Resizable parameters"

**Target behavior:** Each pane has `Min` and `Max` parameters constraining resize range.
**Current behavior:** MariloSplitterPane has 8 parameters; Min/Max need verification.
**Impact:** Cannot constrain pane resize boundaries.
**Recommended direction:** Verify pane parameters; add Min/Max if absent.
**Status:** Open

---

### GAP-SPLITTER-006: No test coverage

**Area:** MariloSplitter
**Severity:** Critical
**Theme:** missing-tests
**Source:** No test files found in tests/

**Target behavior:** Tests covering resize, collapse, keyboard interaction, pane registration, state.
**Current behavior:** Zero tests.
**Impact:** All functionality untested; regressions will go undetected.
**Recommended direction:** Create SplitterTests.cs with coverage of core scenarios.
**Status:** Open

---

### GAP-SPLITTER-007: No demo pages

**Area:** MariloSplitter
**Severity:** High
**Theme:** missing-demos
**Source:** samples/Marilo.Demo/Pages/Components/Splitter/ (directory absent)

**Target behavior:** Demo pages showing horizontal/vertical splitters, collapsible panes, state persistence.
**Current behavior:** No demo directory or pages exist.
**Impact:** No way to preview or validate splitter functionality.
**Recommended direction:** Create demo pages for core splitter scenarios.
**Status:** Open

---

### GAP-SPLITTER-008: Missing per-pane Resizable parameter verification

**Area:** MariloSplitterPane
**Severity:** Medium
**Theme:** missing-parameter
**Source:** splitter/overview.md -- pane parameters include Resizable

**Target behavior:** `Resizable` (bool) parameter per pane to disable resize for specific panes.
**Current behavior:** Pane has 8 parameters; Resizable presence needs verification.
**Impact:** Cannot lock individual pane sizes if missing.
**Recommended direction:** Audit pane parameters; add if absent.
**Status:** Open

---

### GAP-SPLITTER-009: Missing 100%-height layout guidance

**Area:** MariloSplitter
**Severity:** Medium
**Theme:** missing-documentation
**Source:** splitter/overview.md -- tip about 100% viewport layout

**Target behavior:** Documented pattern for full-viewport splitter layouts with header/footer/sidebar.
**Current behavior:** No demo or documentation for this pattern.
**Impact:** Common layout scenario undocumented.
**Recommended direction:** Add full-viewport demo example.
**Status:** Open

---

### GAP-SPLITTER-010: Missing nested splitter support verification

**Area:** MariloSplitter
**Severity:** Medium
**Theme:** missing-feature-verification
**Source:** Implied by spec (common Splitter pattern)

**Target behavior:** Splitters can be nested inside panes for complex layouts.
**Current behavior:** CascadingValue with IsFixed=true should support nesting but untested.
**Impact:** Complex layouts may fail without testing.
**Recommended direction:** Verify nesting works; add a nested splitter test.
**Status:** Open

---

## Severity Breakdown

| Severity | Count |
|----------|-------|
| Critical | 3 |
| High | 4 |
| Medium | 3 |
| Low | 0 |
| **Total** | **10** |
