# Gap Analysis Part 2 — Layout Components (Panel through TabStrip)

## 1. MariloPanel

**Spec:** PanelBar — data-driven accordion with `Data` binding, `ExpandedItems`, `ExpandMode`, hierarchical items, templates, icons, navigation, events, `Rebind()` method.

**Current:** Renders a plain `<div>` with `ChildContent`. No data binding, no expand/collapse, no items, no events.

| Gap | Severity |
|-----|----------|
| No `Data` property or item model binding | **[High]** |
| No expand/collapse behavior (`ExpandedItems`, `ExpandMode`) | **[High]** |
| No header/content template support | **[High]** |
| No hierarchical item rendering | **[High]** |
| No icons or navigation support | **[Medium]** |
| No events (item selection, expand/collapse) | **[Medium]** |
| No `Rebind()` method | **[Low]** |

---

## 2. MariloRow

**Spec:** No standalone spec. Appears to be a simple layout primitive.

**Current:** Renders a `<div>` with CSS class from `CssProvider.RowClass()` and `ChildContent`.

| Gap | Severity |
|-----|----------|
| No spec to compare against — likely feature-complete as a simple layout wrapper | **[Low]** |

---

## 3. MariloSplitter

**Spec:** Multiple panes via `<SplitterPane>` children, per-pane `Size`/`Min`/`Max`/`Collapsible`/`Resizable`, `Width`/`Height`, `Orientation`, `AriaLabel`, state management (`GetState`/`SetState`), events, collapse/resize interaction.

**Current:** Hard-coded two-pane layout with `FirstPane`/`SecondPane` RenderFragments. Static `FirstPaneSize`. Non-interactive separator (no drag resize). Uses `StackDirection` enum instead of `SplitterOrientation`.

| Gap | Severity |
|-----|----------|
| Only supports 2 panes; spec supports N panes via child components | **[High]** |
| No interactive resize (drag handle) | **[High]** |
| No collapse/expand functionality per pane | **[High]** |
| No per-pane `Min`/`Max` constraints | **[High]** |
| No `Width`/`Height` parameters | **[Medium]** |
| No state management (`GetState`/`SetState`) | **[Medium]** |
| No events (resize, collapse) | **[Medium]** |
| No `AriaLabel` parameter | **[Low]** |
| Uses `StackDirection` enum instead of `SplitterOrientation` | **[Low]** |

---

## 4. MariloStack

**Spec:** `Orientation` (default Horizontal), `Spacing`, `HorizontalAlign`, `VerticalAlign`, `Width`, `Height`.

**Current:** Has `Direction` (default Vertical) and `Alignment` (single enum). No `Spacing`, `Width`, `Height`, or separate horizontal/vertical alignment.

| Gap | Severity |
|-----|----------|
| No `Spacing` parameter | **[High]** |
| No `Width`/`Height` parameters | **[Medium]** |
| No separate `HorizontalAlign`/`VerticalAlign` — uses single `StackAlignment` enum | **[Medium]** |
| Default orientation differs (Vertical vs spec's Horizontal) | **[Low]** |
| Parameter named `Direction` instead of `Orientation` | **[Low]** |

---

## 5. MariloStep

**Spec:** Part of Stepper. Spec defines `StepperStep` with `Icon`, `Label`, `Disabled`, `Optional`, `Valid`, custom template support.

**Current:** Has only `Title` and `ChildContent`. Registers/unregisters with parent stepper.

| Gap | Severity |
|-----|----------|
| No `Icon` parameter | **[High]** |
| No `Label` parameter (uses `Title` instead — naming mismatch) | **[Low]** |
| No `Disabled` parameter | **[Medium]** |
| No `Optional` or validation (`Valid`) support | **[Medium]** |
| No step template support | **[Medium]** |

---

## 6. MariloStepper

**Spec:** `Value` (current step index with two-way binding), `Orientation`, `Linear` flow, `StepType` (display mode), step validation, templates.

**Current:** Has `ActiveStep` with `ActiveStepChanged`. Renders step indicators with checkmark for completed. No orientation, no linear flow, no display modes.

| Gap | Severity |
|-----|----------|
| No `Orientation` parameter (horizontal/vertical) | **[High]** |
| No `Linear` flow enforcement | **[Medium]** |
| No `StepType` / display mode (labels vs icons vs both) | **[Medium]** |
| No step validation integration | **[Medium]** |
| No step template support | **[Medium]** |
| Step indicators not clickable (no navigation) | **[Medium]** |
| Parameter named `ActiveStep` instead of `Value` | **[Low]** |

---

## 7. MariloTabStrip

**Spec:** `ActiveTabIndex`/`ActiveTabId`, `TabPosition`, `TabAlignment`, `Size`, `PersistTabContent`, `EnableTabReorder`, `OverflowMode`, `ScrollButtonsPosition`/`Visibility`, `Width`/`Height`, `TabStripSuffixTemplate`, state events, `GetState`/`SetState`/`Refresh`.

**Current:** Implements nearly all spec parameters including `ActiveTabId`, `ActiveTabIndex` (obsolete), `TabPosition`, `TabAlignment`, `Size`, `PersistTabContent`, `EnableTabReorder`, `OverflowMode` with menu, `ScrollButtonsPosition`/`Visibility`, `Width`/`Height`, `TabStripSuffixTemplate`, `OnStateInit`/`OnStateChanged`/`OnTabReorder`, `GetState`/`SetState`/`Refresh`.

| Gap | Severity |
|-----|----------|
| `EnableTabReorder` parameter exists but no drag-and-drop implementation in markup | **[High]** |
| Scroll overflow mode declared but no scroll button rendering in template | **[Medium]** |
| No keyboard navigation (arrow keys between tabs) | **[Medium]** |
| Overflow menu uses inline styles instead of CSS classes | **[Low]** |

---

## 8. TabStripTab

**Spec:** `Title`, `Visible`, `Disabled`, `Closeable`, `Pinnable`, `Pinned`, `HeaderTemplate`, `Content`.

**Current:** Has `Id`, `Title`, `Visible`/`VisibleChanged`, `Disabled`, `Closeable`, `Pinnable`, `Pinned`/`PinnedChanged`, `HeaderTemplate`, `Content`, `ChildContent`.

| Gap | Severity |
|-----|----------|
| No context menu for pin/unpin action | **[Medium]** |
| Component is largely feature-complete relative to spec | — |

---

## Summary

| Component | Overall Gap | Key Issue |
|-----------|------------|-----------|
| MariloPanel | **Critical** | Placeholder div; entire PanelBar feature set missing |
| MariloRow | **None** | No spec; appears complete |
| MariloSplitter | **Critical** | Hard-coded 2 panes, no interactivity |
| MariloStack | **Medium** | Missing Spacing, Width/Height, split alignment |
| MariloStep | **Medium** | Missing Icon, Disabled, validation |
| MariloStepper | **High** | Missing orientation, linear flow, clickable steps |
| MariloTabStrip | **Low** | Mostly complete; drag reorder not wired up |
| TabStripTab | **Low** | Mostly complete; missing pin context menu |
