# Example UX Gap List — MariloResizableContainer

**Stage:** 02-example-ux
**Date:** 2026-04-11
**Demo file:** `/workspaces/Marilo/samples/Marilo.Demo/Pages/Components/ResizableContainer/Overview.razor`
**Spec root:** `/workspaces/Marilo/docs/component-specs/resizable-container/`

---

## Coverage Matrix

| Spec Area | Spec File | Demo Section | Coverage |
|-----------|-----------|--------------|----------|
| Basic sizing + min/max constraints | overview.md | "Basic Bottom-Right Resizing" | COVERED |
| Width / Height parameters | overview.md | all demos | COVERED |
| MinWidth / MinHeight / MaxWidth / MaxHeight | overview.md | "Basic Bottom-Right Resizing" (line 13), "All Edges" (line 140) | COVERED |
| Enabled parameter (disabled state) | appearance.md | MISSING | MISSING |
| ShowHandle parameter | overview.md | MISSING | MISSING |
| ResizeEdges — Right | appearance.md | "Right-Only Resize" (line 124) | COVERED |
| ResizeEdges — Bottom | appearance.md | "Bottom-Only Resize" (line 132) | COVERED |
| ResizeEdges — BottomRight (default) | appearance.md | "Basic Bottom-Right Resizing" (line 13) | COVERED |
| ResizeEdges — All | appearance.md | "All Edges" (line 140) | COVERED |
| ResizeEdges — Top, Left, TopLeft, TopRight, BottomLeft (individual) | appearance.md | MISSING | MISSING |
| UseGhostOutline | overview.md, appearance.md | MISSING | MISSING |
| ClampToParent | overview.md | MISSING | MISSING |
| DisableTextSelection | overview.md | MISSING | MISSING |
| ObserveSizeChanges + OnObservedSizeChanged | overview.md, events.md | "Grid Host Example" (line 26), "Scheduler Host" (line 63), "Chart Host" (line 107) | COVERED |
| PersistSize + PersistKey | overview.md | "Persisted Dimensions" (line 163) | COVERED |
| KeyboardResizeEnabled + keyboard interaction | accessibility/overview.md | "Keyboard Resize" (line 149) | COVERED |
| HandleAriaLabel | accessibility/overview.md | MISSING | MISSING |
| HandleClass + HandleStyle | appearance.md | MISSING | MISSING |
| OnResizeStart event | events.md | MISSING | MISSING |
| OnResizing event | events.md | MISSING | MISSING |
| OnResizeEnd event | events.md | "Basic Bottom-Right Resizing" (line 14) + "Keyboard Resize" (line 151) | PARTIAL |
| Two-way binding (@bind-Width / @bind-Height) | overview.md | MISSING | MISSING |
| SetSizeAsync() public method | overview.md | MISSING | MISSING |
| ResetSizeAsync() public method | overview.md | MISSING | MISSING |
| FocusHandleAsync() public method | overview.md, accessibility/overview.md | MISSING | MISSING |
| CSS class structure / theming | appearance.md | MISSING (no CSS class demo) | MISSING |
| Integration pattern (hosted component guidance) | overview.md | "Grid Host Example", "Scheduler Host", "Chart Host", "Usage Guidance" | COVERED |
| When to use / not to use | overview.md | "Usage Guidance" (line 172) | COVERED |

---

## Summary

| Status | Count |
|--------|-------|
| COVERED | 14 |
| PARTIAL | 1 |
| MISSING | 13 |
| BLOCKED-BY-SOURCE | 0 |

---

## Gaps

### UX-GAP-001 — Disabled State

**Status:** MISSING
**Spec area:** Appearance (appearance.md lines 58–67)
**Description:** The spec documents that when `Enabled="false"`, the container renders in a dimmed state with no active handles. No demo shows this state.
**Suggested scenario:** Side-by-side enabled/disabled containers, or a toggle button that flips `Enabled` to illustrate the visual difference and the absence of handles.
**Demo location to add:** Overview.razor — new DemoSection under "Edge Options" or a dedicated "Disabled State" PageSection.

---

### UX-GAP-002 — ShowHandle Parameter

**Status:** MISSING
**Spec area:** overview.md parameter table (line 48)
**Description:** The spec documents `ShowHandle` (bool, default true) — when false, no handle elements are rendered even if `Enabled` is true. No demo demonstrates this parameter.
**Suggested scenario:** A container with `ShowHandle="false"` where the container is sized only programmatically (e.g., `SetSizeAsync`), or driven by `@bind-Width` / `@bind-Height` from a slider. This also pairs well with the public methods gap (UX-GAP-009).
**Demo location to add:** New DemoSection; could be combined with a programmatic-resize scenario.

---

### UX-GAP-003 — Remaining ResizeEdges Values (Top, Left, TopLeft, TopRight, BottomLeft)

**Status:** MISSING
**Spec area:** appearance.md lines 38–46 (MariloResizeEdges enum values table)
**Description:** The spec documents 9 enum values. The demo covers Right, Bottom, BottomRight, and All. Top, Left, TopLeft, TopRight, and BottomLeft are undemonstrated.
**Suggested scenario:** A compact multi-card layout showing each single-edge/corner variant — Top, Left, TopLeft, TopRight, BottomLeft — as separate small demo boxes. Alternatively, a single "Custom Edges" demo with a bitwise OR example: `ResizeEdges="MariloResizeEdges.Right | MariloResizeEdges.Bottom"`.
**Priority note:** The spec explicitly calls out that `MariloResizeEdges` is a `[Flags]` enum (appearance.md line 46). No demo shows combined values (bitwise OR). This is a notable gap for advanced users.
**Demo location to add:** "Edge Options" PageSection in Overview.razor.

---

### UX-GAP-004 — UseGhostOutline

**Status:** MISSING
**Spec area:** overview.md (line 53), appearance.md (lines 49–56), overview.md performance section (lines 93–94)
**Description:** `UseGhostOutline="true"` shows a dashed outline at the drag target size without live-resizing the content — important for complex hosted content to avoid reflow during drag. No demo shows this behavior.
**Suggested scenario:** A container with heavy content (e.g., the grid host from the existing demo) + `UseGhostOutline="true"`, with a label showing "Ghost mode — content stays stable during drag." Comparing ghost vs. live mode side-by-side would be ideal.
**Demo location to add:** New DemoSection "Ghost Outline Mode" under a "Performance" or "Appearance" PageSection.

---

### UX-GAP-005 — ClampToParent

**Status:** MISSING
**Spec area:** overview.md parameter table (line 54)
**Description:** `ClampToParent="true"` constrains resize within parent element bounds. No demo shows this.
**Suggested scenario:** A container inside a fixed-size parent `div` with `ClampToParent="true"`, demonstrating that dragging cannot exceed the parent boundary.
**Demo location to add:** New DemoSection under a "Behavior" PageSection.

---

### UX-GAP-006 — DisableTextSelection

**Status:** MISSING
**Spec area:** overview.md parameter table (line 55)
**Description:** `DisableTextSelection` (default true) suppresses text selection during drag. Consumers may need `false` for edge cases. No demo shows this parameter — even a note or toggle would help.
**Priority:** P3 — the default is safe; this is an informational gap rather than a functional gap.
**Suggested scenario:** A note in the keyboard/drag demo, or a code-snippet-only demo showing `DisableTextSelection="false"`.
**Demo location to add:** Inline note in existing drag demo, or a separate DemoSection.

---

### UX-GAP-007 — HandleAriaLabel

**Status:** MISSING
**Spec area:** accessibility/overview.md (lines 20–31), overview.md parameter table (line 57)
**Description:** The spec documents that the handle announces itself via `aria-label`, defaults to "Resize", and accepts a custom string via `HandleAriaLabel`. No demo demonstrates customizing this label. Important for teams that embed multiple ResizableContainers on a page (each handle would read "Resize" with no differentiation).
**Suggested scenario:** A demo with `HandleAriaLabel="Resize chart panel"` alongside a note explaining the default and the screen reader behavior.
**Demo location to add:** Accessibility PageSection (new) or expanded keyboard demo.

---

### UX-GAP-008 — HandleClass / HandleStyle

**Status:** MISSING
**Spec area:** appearance.md (lines 69–77), overview.md parameter table (lines 58–59)
**Description:** `HandleClass` and `HandleStyle` allow custom handle appearance. The spec shows a code example (appearance.md lines 71–76). No live demo exists.
**Suggested scenario:** A DemoSection "Custom Handle Styling" with a visually distinct handle (e.g., red background or icon).
**Demo location to add:** "Appearance" PageSection (new or in existing Edge Options section).

---

### UX-GAP-009 — Public Methods (SetSizeAsync, ResetSizeAsync, FocusHandleAsync)

**Status:** MISSING
**Spec area:** overview.md Public Methods table (lines 75–80), accessibility/overview.md (line 66)
**Description:** Three public methods are documented. None are demonstrated. These are important for programmatic control scenarios (e.g., a layout manager resizing panels, a reset button after user customization).
**Suggested scenario:**
- A "Programmatic Resize" demo with a button that calls `SetSizeAsync("600px", "400px")` and a reset button that calls `ResetSizeAsync()`.
- A "Focus Handle" demo or note showing `FocusHandleAsync()` for custom keyboard workflows.
**Demo location to add:** New PageSection "Programmatic Control" in Overview.razor.

---

### UX-GAP-010 — Two-Way Binding (@bind-Width / @bind-Height)

**Status:** MISSING
**Spec area:** overview.md "Two-Way Binding" section (lines 63–70)
**Description:** The spec shows a `@bind-Width` / `@bind-Height` usage pattern. The existing demos display size after resize using `OnResizeEnd` callbacks instead of two-way binding. No demo uses `@bind-Width` / `@bind-Height` directly.
**Suggested scenario:** A demo with `@bind-Width="_width"` and `@bind-Height="_height"` showing live bound state update, plus an input or slider that sets `_width` to drive the container programmatically via binding.
**Demo location to add:** New DemoSection "Two-Way Binding" in the Overview page (could precede or replace the basic demo).

---

### UX-GAP-011 — OnResizeStart Event

**Status:** MISSING
**Spec area:** events.md (lines 13–29)
**Description:** `OnResizeStart` fires when drag begins. No demo handles this event. The events.md spec has a code example for it (lines 18–28) but no live demo illustrates the callback firing.
**Suggested scenario:** A demo that logs or displays "Resize started" on `OnResizeStart`, alongside the existing end-state display.
**Demo location to add:** Expand "Basic Bottom-Right Resizing" demo to also wire `OnResizeStart`, or add an Events-focused DemoSection.

---

### UX-GAP-012 — OnResizing Event

**Status:** MISSING
**Spec area:** events.md (line 33 — very thin spec, see SPEC-resizable-container-008 in Stage 01 output)
**Description:** `OnResizing` fires on each pointer-move frame during drag. No demo subscribes to it. This is also the event most likely to cause performance issues if misused.
**Suggested scenario:** A demo using `OnResizing` to display a real-time size readout (demonstrating per-frame updates) with a caveat note about throttling expensive operations.
**Demo location to add:** Events-focused DemoSection; pair with `UseGhostOutline` to contrast performance approaches.
**Blocked-by-source note:** None — the event is fully implemented (source line 91).

---

### UX-GAP-013 — OnResizeEnd Partial Coverage

**Status:** PARTIAL
**Spec area:** events.md (lines 36–50)
**Description:** `OnResizeEnd` is used in two demos ("Basic Bottom-Right Resizing" and "Keyboard Resize") which correctly capture `args.Width` and `args.Height`. However, the demo does not display or exercise `args.ActiveEdge` or `args.IsUserInitiated`, both of which are documented in the MariloResizeEventArgs table (events.md lines 82–83). Consumers need to see these to understand the full event payload.
**Suggested scenario:** Expand the Basic demo to also display `args.ActiveEdge` in the size readout (e.g., "Last edge: BottomRight").
**Demo location to add:** Modify existing "Basic Bottom-Right Resizing" demo in Overview.razor lines 203–208.

---

## Blocked-by-Source

None. All gaps above are demo gaps only; the underlying source parameters and events are fully implemented.

---

## Notes

- The "Usage Guidance" PageSection (Overview.razor lines 172–196) provides good when-to-use / when-not-to-use content. This directly mirrors overview.md and provides solid doc-parity.
- The integration demos (Grid Host, Scheduler Host, Chart Host) cover `OnObservedSizeChanged` well and provide realistic usage patterns. These are a strength of the current demo page.
- The code snippets embedded in `@code` (lines 268–342) use `MariloDataGrid`, `MariloAllocationScheduler`, and `MariloChart` in the code string samples even though the live demos use inline HTML — this is acceptable (the live demos avoid component dependencies), but a note in the spec or demo could clarify the intent.
