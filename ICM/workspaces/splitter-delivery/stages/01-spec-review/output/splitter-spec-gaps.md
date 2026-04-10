# MariloSplitter Spec Gap List

**Date:** 2026-04-10
**Auditor:** Claude (Stage 01 - Spec Review)
**Source files:** `src/Marilo.Components/Layout/MariloSplitter.razor`, `MariloSplitterPane.razor`, `MariloSplitterPanes.razor`, `SplitterTypes.cs`
**Spec files:** `docs/component-specs/splitter/` (overview, events, panes, orientation, state, accessibility)

---

## Source Inventory

### MariloSplitter Parameters

| Parameter | Type | Default | In Spec? |
|-----------|------|---------|----------|
| `Orientation` | `SplitterOrientation` | `Horizontal` | Yes |
| `Width` | `string?` | `null` | Yes |
| `Height` | `string?` | `null` | Yes |
| `Collapsible` | `bool` | `false` | No |
| `AriaLabel` | `string?` | `null` | Yes |
| `FirstPaneSize` | `string?` | `"50%"` | No |
| `FirstPane` | `RenderFragment?` | `null` | No |
| `SecondPane` | `RenderFragment?` | `null` | No |
| `ChildContent` | `RenderFragment?` | `null` | No (implicit) |

### MariloSplitter Events

| Event | Args Type | In Spec? |
|-------|-----------|----------|
| `OnResize` | `SplitterResizeEventArgs` | Yes (shape mismatch) |
| `OnResizeStart` | `SplitterResizeEventArgs` | No |
| `OnResizeEnd` | `SplitterResizeEventArgs` | No |
| `OnCollapse` | `SplitterCollapseEventArgs` | Yes (shape mismatch) |
| `OnExpand` | `SplitterCollapseEventArgs` | Yes (args type mismatch) |

### MariloSplitter Methods

| Method | Returns | In Spec? |
|--------|---------|----------|
| `GetState()` | `SplitterState` | Yes (shape mismatch) |
| `SetState(SplitterState)` | `void` | Yes (shape mismatch) |

### MariloSplitterPane Parameters

| Parameter | Type | Default | In Spec? |
|-----------|------|---------|----------|
| `Size` | `string?` | `null` | Yes |
| `Min` | `string?` | `null` | Yes |
| `Max` | `string?` | `null` | Yes |
| `Collapsed` | `bool` | `false` | Yes |
| `Collapsible` | `bool` | `true` | Yes (default mismatch) |
| `Resizable` | `bool` | `true` | Yes |
| `ChildContent` | `RenderFragment?` | `null` | No (implicit) |

### SplitterTypes.cs

| Type | Members | In Spec? |
|------|---------|----------|
| `SplitterResizeEventArgs` | `PaneIndex`, `NewSize`, `AdjacentSize` | Partial (spec uses `Index`, `Size`) |
| `SplitterCollapseEventArgs` | `PaneIndex` | Partial (spec uses `Index`) |
| `SplitterState` | `PaneSizes: List<string>`, `CollapsedPanes: List<bool>` | Mismatch (spec uses `Panes: List<SplitterPaneState>`) |

### MariloSplitterPanes

| Item | Notes | In Spec? |
|------|-------|----------|
| Pass-through wrapper | Renders `ChildContent` only | Yes (spec uses `<SplitterPanes>`) |

---

## Gap Records

### A. Undocumented (in source, not in spec)

#### A-1. `Collapsible` parameter on MariloSplitter (component-level)
- **Priority:** P2
- **Source:** `MariloSplitter.razor` line 98 -- `[Parameter] public bool Collapsible { get; set; }`
- **Details:** The source has a top-level `Collapsible` parameter that enables collapse on all panes globally. The spec only documents `Collapsible` on individual `SplitterPane` instances. This is a useful convenience parameter that should be documented.
- **Action:** Add to overview.md Splitter Parameters table.

#### A-2. `OnResizeStart` event
- **Priority:** P2
- **Source:** `MariloSplitter.razor` line 101 -- `EventCallback<SplitterResizeEventArgs> OnResizeStart`
- **Details:** Fires when drag-to-resize begins. Not mentioned anywhere in the spec events.md.
- **Action:** Add to events.md with example.

#### A-3. `OnResizeEnd` event
- **Priority:** P2
- **Source:** `MariloSplitter.razor` line 102 -- `EventCallback<SplitterResizeEventArgs> OnResizeEnd`
- **Details:** Fires when drag-to-resize completes (mouse up). Not mentioned in the spec. The spec's `OnResize` description says "fires after the user has finished resizing" which semantically overlaps with `OnResizeEnd`, but the source fires `OnResize` on every move and `OnResizeEnd` on completion -- the spec description is wrong about `OnResize` timing.
- **Action:** Add to events.md. Correct `OnResize` description to indicate it fires continuously during drag.

#### A-4. Legacy 2-pane parameters (`FirstPaneSize`, `FirstPane`, `SecondPane`)
- **Priority:** P3
- **Source:** `MariloSplitter.razor` lines 107-109
- **Details:** Legacy API for simple 2-pane usage without `SplitterPanes` wrapper. Not documented in spec. Low priority as the `SplitterPanes` pattern is the canonical API.
- **Action:** Document as legacy/convenience API in overview.md, or mark as `[Obsolete]` in source.

#### A-5. `AdjacentSize` property on `SplitterResizeEventArgs`
- **Priority:** P2
- **Source:** `SplitterTypes.cs` line 14 -- `public string AdjacentSize { get; set; }`
- **Details:** The source event args include the adjacent pane's size. The spec events.md only mentions `Index` and `Size` (as `args.Index`, `args.Size`).
- **Action:** Document in events.md.

#### A-6. Double-click to collapse on separator
- **Priority:** P2
- **Source:** `MariloSplitter.razor` line 39 -- `@ondblclick="() => HandleSeparatorDoubleClick(idx)"`
- **Details:** Double-clicking a separator toggles collapse on collapsible panes. Not documented in the spec.
- **Action:** Document in panes.md or a new "Keyboard and Mouse Interaction" section.

#### A-7. Keyboard resize with Shift modifier (5x step)
- **Priority:** P3
- **Source:** `MariloSplitter.razor` line 374 -- `e.ShiftKey ? KeyboardResizeStep * 5 : KeyboardResizeStep`
- **Details:** Holding Shift during arrow-key resize multiplies the step by 5 (50px vs 10px). Not documented.
- **Action:** Document in accessibility section or keyboard navigation notes.

#### A-8. Enter key toggles collapse on separator
- **Priority:** P3
- **Source:** `MariloSplitter.razor` lines 396-399 -- Enter key handler
- **Details:** When focused on a separator, pressing Enter toggles collapse on collapsible panes. Not documented in spec.
- **Action:** Document in accessibility/keyboard navigation section.

---

### B. Spec-Ahead (in spec, not in source)

#### B-1. `SizeChanged` event on SplitterPane
- **Priority:** P1
- **Source:** Not implemented
- **Spec:** `events.md` lines 146-186 -- Documented with full example showing two-way binding `@bind-Size`
- **Details:** The spec documents `SizeChanged` as an `EventCallback<string>` on `SplitterPane` enabling two-way binding for `Size`. The source `MariloSplitterPane.razor` has no `SizeChanged` parameter. The parent splitter mutates size via internal `SetSize()` but never fires a pane-level callback.
- **Action:** Implement `[Parameter] public EventCallback<string> SizeChanged` on `MariloSplitterPane`. Fire it from the parent when size changes.

#### B-2. `CollapsedChanged` event on SplitterPane
- **Priority:** P1
- **Source:** Not implemented
- **Spec:** `events.md` lines 189-232 -- Documented with full example showing two-way binding `@bind-Collapsed`
- **Details:** The spec documents `CollapsedChanged` as an `EventCallback<bool>` enabling two-way binding for `Collapsed`. Not present in source.
- **Action:** Implement `[Parameter] public EventCallback<bool> CollapsedChanged` on `MariloSplitterPane`. Fire it from the parent when collapse state changes.

#### B-3. `Scrollable` parameter on SplitterPane
- **Priority:** P2
- **Source:** Not implemented
- **Spec:** `panes.md` line 29 -- `Scrollable | bool | Whether the browser automatically shows scrollbars in panes which do not fit their current content.`
- **Details:** Documented in the pane parameters table. Not present in `MariloSplitterPane.razor`.
- **Action:** Implement as `[Parameter] public bool Scrollable { get; set; }`. Apply `overflow:auto` to pane div when true.

#### B-4. `Visible` parameter on SplitterPane
- **Priority:** P2
- **Source:** Not implemented
- **Spec:** `panes.md` line 31 -- `Visible | bool | Defines if the pane element and splitbar render or not.`
- **Details:** Documented in the pane parameters table. Not present in source.
- **Action:** Implement as `[Parameter] public bool Visible { get; set; } = true`. Skip rendering pane and adjacent separator when false.

#### B-5. `Class` parameter on SplitterPane
- **Priority:** P2
- **Source:** Not explicitly on SplitterPane (inherited from `MariloComponentBase` as standard `Class`)
- **Spec:** `panes.md` line 23 -- `Class | string | The custom CSS class that renders on the pane element`
- **Details:** The spec documents `Class` on the pane, but the pane's content is rendered by the parent `MariloSplitter`, not by the pane itself. The parent does not read or apply any `Class` from the pane to the rendered `<div class="mar-splitter__pane">`. Even if `MariloComponentBase` provides `Class`, it is not wired through.
- **Action:** Either expose `Class` on `MariloSplitterPane` and have the parent apply it to the pane div, or confirm it flows through `MariloComponentBase` and wire it in the parent markup.

#### B-6. `SplitterPaneState` model and `Panes` property on `SplitterState`
- **Priority:** P1
- **Source:** `SplitterState` uses flat `List<string> PaneSizes` + `List<bool> CollapsedPanes`
- **Spec:** `state.md` lines 17-21 -- `SplitterState` has `Panes: List<SplitterPaneState>` where each `SplitterPaneState` has `Size` and `Collapsed`
- **Details:** The spec's state model is structured differently from the source. The spec uses a nested `SplitterPaneState` object per pane; the source uses two parallel lists.
- **Action:** Align source to spec. Replace `PaneSizes`/`CollapsedPanes` with `Panes: List<SplitterPaneState>`. Add `SplitterPaneState` class.

#### B-7. `role=group` on pane elements
- **Priority:** P2
- **Source:** Pane divs have no `role` attribute
- **Spec:** `wai-aria-support.md` line 29 -- `.k-pane` should have `role=group`
- **Details:** The WAI-ARIA spec says pane elements should have `role="group"`. The source renders `<div class="mar-splitter__pane">` without any role.
- **Action:** Add `role="group"` to pane divs in `MariloSplitter.razor`.

#### B-8. `aria-keyshortcuts` on separators
- **Priority:** P3
- **Source:** Not implemented
- **Spec:** `wai-aria-support.md` line 40 -- `aria-keyshortcuts=ArrowLeft ArrowRight ArrowUp ArrowDown`
- **Details:** The spec says splitbar elements should have `aria-keyshortcuts`. Source separators have `role`, `tabindex`, `aria-orientation`, `aria-label`, `aria-valuenow` but not `aria-keyshortcuts`.
- **Action:** Add `aria-keyshortcuts` attribute to separator handles.

---

### C. Mismatch (both exist but differ)

#### C-1. `OnResize` event semantics
- **Priority:** P1
- **Source:** Fires on every mouse move during drag (`ApplyDrag` -> `NotifyResize`)
- **Spec:** `events.md` line 107 -- "fires after the user has finished resizing a pane (after the mouse button is released)"
- **Details:** Critical semantic mismatch. The source fires `OnResize` continuously during drag; the spec says it fires only on completion. The source has a separate `OnResizeEnd` for completion, which the spec does not document.
- **Action:** Either change source to match spec (fire `OnResize` only on drag end, remove `OnResizeEnd`), or update spec to match source (OnResize = continuous, OnResizeEnd = completion). Recommend updating spec since the source pattern is more flexible.

#### C-2. `SplitterResizeEventArgs` property names
- **Priority:** P1
- **Source:** Properties: `PaneIndex`, `NewSize`, `AdjacentSize`
- **Spec:** `events.md` line 141 -- `args.Index`, `args.Size`
- **Details:** The spec uses `Index` and `Size`; the source uses `PaneIndex` and `NewSize`. Consumers writing code from the spec will get compile errors.
- **Action:** Align naming. Recommend changing source to `Index`/`Size` to match spec (shorter, conventional). Add `AdjacentSize` to spec.

#### C-3. `SplitterCollapseEventArgs` property name
- **Priority:** P1
- **Source:** Property: `PaneIndex`
- **Spec:** `events.md` line 57 -- `args.Index`
- **Details:** Same naming mismatch as C-2.
- **Action:** Rename source `PaneIndex` to `Index`, or update spec.

#### C-4. `OnExpand` event args type
- **Priority:** P2
- **Source:** Uses `SplitterCollapseEventArgs` (same type as `OnCollapse`)
- **Spec:** `events.md` line 98 -- Uses `SplitterExpandEventArgs` (separate type)
- **Details:** Spec implies a dedicated `SplitterExpandEventArgs` type. Source reuses `SplitterCollapseEventArgs` for both events. Functionally equivalent since both only carry `PaneIndex`/`Index`.
- **Action:** Either create `SplitterExpandEventArgs` in source for spec compliance, or simplify spec to document `SplitterCollapseEventArgs` for both. Recommend keeping one type (rename to `SplitterPaneEventArgs`) for both events.

#### C-5. `SplitterState` model shape
- **Priority:** P1
- **Source:** `PaneSizes: List<string>`, `CollapsedPanes: List<bool>` (flat lists)
- **Spec:** `Panes: List<SplitterPaneState>` (nested objects)
- **Details:** See B-6. This is both a spec-ahead gap (missing type) and a mismatch (existing type has wrong shape).
- **Action:** Align to spec shape. This is a breaking change to the state API.

#### C-6. `SplitterPane` tag name in spec vs source
- **Priority:** P2
- **Source:** Component is `MariloSplitterPane`
- **Spec:** All examples use `<SplitterPane>` (no prefix)
- **Details:** The spec consistently uses `<SplitterPane>` and `<SplitterPanes>`. The source components are `MariloSplitterPane` and `MariloSplitterPanes`. Consumers following spec examples will get tag-not-found errors unless there are `@using` aliases.
- **Action:** Either add tag aliases / `@using` directives, or update all spec examples to use `MariloSplitterPane` / `MariloSplitterPanes` prefixed names.

#### C-7. `SplitterPane.Collapsible` default value
- **Priority:** P2
- **Source:** Defaults to `true` (`MariloSplitterPane.razor` line 21)
- **Spec:** `panes.md` line 25 -- Documents `Collapsible | bool` with no explicit default; overview.md line 21 states "By default, Splitter panes are resizable, but not collapsible"
- **Details:** The spec says panes are NOT collapsible by default; the source defaults `Collapsible` to `true`.
- **Action:** Either change source default to `false` to match spec, or update spec. Recommend changing source to `false` since making all panes collapsible by default is unusual.

#### C-8. `aria-valuenow` on separators
- **Priority:** P2
- **Source:** Separators have `aria-valuenow="@GetPaneSizePercent(idx)"` with `aria-valuemin="0"` and `aria-valuemax="100"`
- **Spec:** `wai-aria-support.md` lines 42-43 -- Explicitly states that setting `value-now` is "not applicable" and recommends against it
- **Details:** The source implements what the spec explicitly recommends against. The spec notes from WAI-ARIA discussions that valuenow is not meaningful for complex multi-pane splitters.
- **Action:** Either remove `aria-valuenow`/`aria-valuemin`/`aria-valuemax` from source to match spec guidance, or document why the implementation diverges. For simple 2-pane splitters valuenow is useful; for multi-pane it is ambiguous.

---

## Summary

| Category | Count | P1 | P2 | P3 |
|----------|-------|----|----|-----|
| A. Undocumented | 8 | 0 | 4 | 4 |
| B. Spec-Ahead | 8 | 3 | 4 | 1 |
| C. Mismatch | 8 | 4 | 4 | 0 |
| **Total** | **24** | **7** | **12** | **5** |

### P1 Blockers (must resolve before handoff)

1. **B-1** `SizeChanged` event not implemented (blocks two-way binding)
2. **B-2** `CollapsedChanged` event not implemented (blocks two-way binding)
3. **B-6 / C-5** `SplitterState` model shape mismatch (breaking API difference)
4. **C-1** `OnResize` semantic mismatch (continuous vs on-completion)
5. **C-2** `SplitterResizeEventArgs` property name mismatch (`PaneIndex`/`NewSize` vs `Index`/`Size`)
6. **C-3** `SplitterCollapseEventArgs` property name mismatch (`PaneIndex` vs `Index`)
7. **C-6** Tag name mismatch (`MariloSplitterPane` vs `<SplitterPane>` in spec examples)
