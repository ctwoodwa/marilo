# Spec Gap List — MariloResizableContainer

**Stage:** 01-spec-review
**Date:** 2026-04-11
**Source:** `/workspaces/Marilo/src/Marilo.Components/Layout/ResizableContainer/MariloResizableContainer.razor.cs`
**Spec root:** `/workspaces/Marilo/docs/component-specs/resizable-container/`
**Spec files found:**
- `overview.md` (parameters, public methods, two-way binding, integration guidance)
- `appearance.md` (ResizeEdges enum, ghost outline, disabled state, CSS classes)
- `events.md` (OnResizeStart, OnResizing, OnResizeEnd, OnObservedSizeChanged, event args shapes)
- `accessibility/overview.md` (keyboard interactions, ARIA, reduced motion)

**Spec files listed in delivery-context.md but NOT found on disk:**
- `docs/component-specs/resizable-container/panes.md`
- `docs/component-specs/resizable-container/orientation.md`
- `docs/component-specs/resizable-container/state.md`

---

## Summary

| Category | Count |
|----------|-------|
| Undocumented (in source, not in spec) | 4 |
| Spec-ahead (in spec, not in source) | 3 |
| Mismatch (name or type differs) | 1 |
| Missing spec files (delivery-context listed) | 3 |

---

## List 1: Undocumented (in source, not in spec)

### SPEC-resizable-container-001

**ID:** SPEC-resizable-container-001
**Type:** undocumented
**Parameter/Event:** WidthChanged
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | WidthChanged |
| Type | missing | EventCallback\<string\> |
| Default | missing | default |
| Description | missing | Two-way binding callback; fires with the new CSS width string after a drag, keyboard resize, SetSizeAsync call, or persisted-size restore |

**Location in source:** `MariloResizableContainer.razor.cs` line 99
**Notes:** The spec overview.md documents two-way binding via `@bind-Width` (line 67) and the code example is correct, but `WidthChanged` is never listed in the Parameters table (overview.md lines 38–62). Two-way binding callbacks must appear in the parameter table so consumers know the event name and type.
**Recommended action:** Add `WidthChanged` row to overview.md parameters table under a "Two-Way Binding Callbacks" sub-group, or annotate the existing `Width` row to clarify it supports `@bind-Width`.
**Delegated to:** spec update only

---

### SPEC-resizable-container-002

**ID:** SPEC-resizable-container-002
**Type:** undocumented
**Parameter/Event:** HeightChanged
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | HeightChanged |
| Type | missing | EventCallback\<string\> |
| Default | missing | default |
| Description | missing | Two-way binding callback; fires with the new CSS height string after a drag, keyboard resize, SetSizeAsync call, or persisted-size restore |

**Location in source:** `MariloResizableContainer.razor.cs` line 102
**Notes:** Same gap as WidthChanged. The `@bind-Height` example in overview.md is shown, but the backing `HeightChanged` EventCallback is absent from the parameter table.
**Recommended action:** Add `HeightChanged` row alongside WidthChanged.
**Delegated to:** spec update only

---

### SPEC-resizable-container-003

**ID:** SPEC-resizable-container-003
**Type:** undocumented
**Parameter/Event:** Class / Style (inherited)
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Class / Style listed in overview.md table | Inherited via MariloComponentBase; NOT declared as [Parameter] on this class |
| Type | string? / string? | inherited — applied via CombineClasses() / CombineStyles() in computed properties |
| Default | null / null | N/A (inherited) |
| Description | "Extra CSS class for root" / "Extra inline style for root" | Inherited base-class parameters |

**Location in source:** `MariloResizableContainer.razor.cs` lines 122–126 (CombineClasses / CombineStyles usage); `MariloResizableContainer.razor` line 8 (`@attributes="AdditionalAttributes"`)
**Notes:** The spec overview.md parameter table correctly lists `Class` and `Style`, and they work at runtime through base-class plumbing. However, the spec does not acknowledge that these are base-class inherited parameters rather than locally declared ones. No correction needed if all Marilo components share this convention — but the spec should add a note or link to `MariloComponentBase` so consumers understand the inheritance.
**Recommended action:** Add a note to overview.md parameters table (or a new "Base Parameters" section) clarifying that `Class`, `Style`, and `AdditionalAttributes` are inherited from `MariloComponentBase`.
**Delegated to:** spec update only

---

### SPEC-resizable-container-004

**ID:** SPEC-resizable-container-004
**Type:** undocumented
**Parameter/Event:** OnPersistedSizeRestoredFromJs (JSInvokable internal callback)
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | OnPersistedSizeRestoredFromJs |
| Type | missing | [JSInvokable] Task |
| Default | N/A | N/A |
| Description | missing | Called from JS when a previously persisted size is restored on init; updates _currentWidth/_currentHeight and fires WidthChanged/HeightChanged |

**Location in source:** `MariloResizableContainer.razor.cs` lines 292–299
**Notes:** This is an internal JS-bridge method, not a public Blazor parameter. It is correctly not in the parameter table. However, the spec section on `PersistSize` / `PersistKey` does not mention that size restoration fires `WidthChanged` and `HeightChanged` callbacks on load — consumers using `@bind-Width` / `@bind-Height` with persistence need to know their bound variables will be updated on first render.
**Recommended action:** Add a note in overview.md under the `PersistSize` parameter description (or in a new "Persistence Behavior" subsection) explaining that restoring a persisted size also invokes `WidthChanged` and `HeightChanged`.
**Delegated to:** spec update only

---

## List 2: Spec-Ahead (in spec, not in source)

### SPEC-resizable-container-005

**ID:** SPEC-resizable-container-005
**Type:** spec-ahead
**Parameter/Event:** panes.md (full spec file)
**Priority:** P1 (blocking — spec area listed in delivery-context.md but file absent)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | panes — expected at `docs/component-specs/resizable-container/panes.md` | N/A |
| Type | missing | N/A |
| Default | missing | N/A |
| Description | missing | N/A |

**Notes:** `delivery-context.md` line 65 lists `panes` as a spec feature area with status PENDING. The file does not exist. The source component does not have a multi-pane concept (MariloResizableContainer is a single-wrapper component); the spec area may have been pre-allocated in error, or it refers to pane-level configuration that has not been specced yet.
**Recommended action:** Either create `panes.md` (e.g., documenting how `ChildContent` acts as the single pane, contrasting with MariloSplitter) or remove the `panes` entry from delivery-context.md feature areas with a note that it does not apply to this component.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-resizable-container-006

**ID:** SPEC-resizable-container-006
**Type:** spec-ahead
**Parameter/Event:** orientation.md (full spec file)
**Priority:** P2 (this phase — spec area listed but file absent)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | orientation — expected at `docs/component-specs/resizable-container/orientation.md` | N/A |
| Type | missing | N/A |
| Default | missing | N/A |
| Description | missing | N/A |

**Notes:** `delivery-context.md` line 67 lists `orientation` as a feature area. No source parameter named `Orientation` exists. The `ResizeEdges` flags enum controls which edges are active and partially encodes orientation intent. The spec file is either still to be written or was pre-allocated incorrectly.
**Recommended action:** Either create `orientation.md` explaining how `ResizeEdges` covers horizontal/vertical/both resize semantics, or remove the entry from delivery-context.md.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-resizable-container-007

**ID:** SPEC-resizable-container-007
**Type:** spec-ahead
**Parameter/Event:** state.md (full spec file)
**Priority:** P2 (this phase — spec area listed but file absent)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | state — expected at `docs/component-specs/resizable-container/state.md` | N/A |
| Type | missing | N/A |
| Default | missing | N/A |
| Description | missing | N/A |

**Notes:** `delivery-context.md` line 68 lists `state` as a feature area. Source has persistence state (`PersistSize`, `PersistKey`) and internal dragging state (`_isDragging`). A `state.md` covering persistence, two-way binding state flow, and internal drag state would be useful. The file has not been created.
**Recommended action:** Create `state.md` covering: persistence behavior (PersistSize/PersistKey), two-way binding state (Width/Height with WidthChanged/HeightChanged), and the internal drag state that suppresses parameter updates during a drag (source lines 199–203).
**Delegated to:** gap-analysis-resolution intake

---

## List 3: Mismatches (name or shape differs between spec and source)

### SPEC-resizable-container-008

**ID:** SPEC-resizable-container-008
**Type:** mismatch
**Parameter/Event:** OnResizing — event args documentation
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnResizing | OnResizing |
| Type | EventCallback\<MariloResizeEventArgs\> (implied by sibling events) | EventCallback\<MariloResizeEventArgs\> |
| Default | — | default |
| Description | "Fires on each frame during a drag resize. Use for live feedback." (events.md line 33) | Fires per pointermove frame during drag; passes full MariloResizeEventArgs |

**Notes:** The spec entry for `OnResizing` (events.md line 33) has no code example, no event args table call-out, and no mention of the performance implications of a per-frame callback. The sibling events `OnResizeStart` and `OnResizeEnd` both have code examples. The `OnResizing` entry is present but materially incomplete — not fully spec-ahead (the parameter exists in source) and not undocumented (it appears in spec), but its spec coverage is too thin to be useful.
**Recommended action:** Expand the `OnResizing` section in events.md to include: a code example matching OnResizeStart/OnResizeEnd style, the MariloResizeEventArgs table, and a performance note (per-frame — throttle heavy operations; `UseGhostOutline` avoids this).
**Delegated to:** spec update only
