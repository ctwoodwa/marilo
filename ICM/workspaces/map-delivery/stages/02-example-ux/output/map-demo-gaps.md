# MariloMap Demo Gaps -- Stage 02 Output

**Date:** 2026-04-12
**Worker:** w-map-delivery
**Component:** MariloMap
**Demo page:** `samples/Marilo.Demo/Pages/Components/Map/Overview.razor`

---

## Summary

The demo page (`Overview.razor`) exists and is functional for the **prototype** API. It demonstrates:
- Basic map with markers
- Interactive marker click handling
- Custom zoom level via range slider

However, the demo uses the **prototype flat-marker API** (`Markers` parameter with `List<MapMarker>`), not the spec's layer-based API (`MapLayers`/`MapLayer` child content). Since the spec has been revised for the MapLibre architecture, the entire demo will need to be rewritten when the layer system is implemented.

---

## Current Demo Scenarios

| # | Scenario | Status | Notes |
|---|----------|--------|-------|
| 1 | Basic map with markers | PRESENT | Uses prototype `Markers` parameter, not spec API |
| 2 | Interactive marker click | PRESENT | Uses `EventCallback<MapMarker>`, not spec's `MapMarkerClickEventArgs` |
| 3 | Custom zoom level | PRESENT | Uses `int` zoom, spec says `double` |

---

## Missing Demo Scenarios (Required by Spec)

### DEMO-map-001: Tile Layer Demo

**Priority:** P1 (blocking -- core functionality)
**Spec reference:** `docs/component-specs/map/layers/tile.md`
**What's needed:** Demo showing `MapLayer Type="MapLayerType.Tile"` with UrlTemplate, Subdomains, Attribution.
**Blocked by:** No tile rendering implementation. Requires MapLibre JS adapter (SPEC-map-003).

---

### DEMO-map-002: Marker Layer Demo (Layer-Based)

**Priority:** P1 (blocking -- replaces current prototype markers)
**Spec reference:** `docs/component-specs/map/layers/marker.md`
**What's needed:** Demo showing `MapLayer Type="MapLayerType.Marker"` with Data, LocationField, TitleField. Should include marker shapes (Pin, PinTarget) and tooltip templates.
**Blocked by:** No layer registration system (SPEC-map-001). Current prototype uses flat `Markers` list.

---

### DEMO-map-003: Shape/GeoJSON Layer Demo

**Priority:** P1 (blocking)
**Spec reference:** `docs/component-specs/map/layers/shape.md`
**What's needed:** Demo showing `MapLayer Type="MapLayerType.Shape"` with GeoJSON data and `MapLayerShapeSettingsStyle` (fill/stroke).
**Blocked by:** No shape layer implementation (SPEC-map-004). Requires MapLibre GeoJSON source support.

---

### DEMO-map-004: Bubble Layer Demo

**Priority:** P1 (blocking)
**Spec reference:** `docs/component-specs/map/layers/bubble.md`
**What's needed:** Demo showing `MapLayer Type="MapLayerType.Bubble"` with LocationField, ValueField, bubble styling.
**Blocked by:** No bubble layer implementation (SPEC-map-005). Requires MapLibre circle layer.

---

### DEMO-map-005: OnClick Event Demo

**Priority:** P2
**Spec reference:** `docs/component-specs/map/events.md` (OnClick section)
**What's needed:** Demo showing map click returning lat/lng location.
**Blocked by:** No OnClick event (SPEC-map-010). Requires JS interop.

---

### DEMO-map-006: OnMarkerClick Event Demo (Typed Args)

**Priority:** P2
**Spec reference:** `docs/component-specs/map/events.md` (OnMarkerClick section)
**What's needed:** Demo showing marker click with `MapMarkerClickEventArgs` (DataItem + EventArgs).
**Current state:** Existing demo uses the prototype `EventCallback<MapMarker>` -- different signature from spec.
**Blocked by:** Event args type mismatch (SPEC-map-011).

---

### DEMO-map-007: OnShapeClick Event Demo

**Priority:** P2
**Spec reference:** `docs/component-specs/map/events.md` (OnShapeClick section)
**What's needed:** Demo showing shape/bubble click with `MapShapeClickEventArgs`.
**Blocked by:** No shape layer or event (SPEC-map-012).

---

### DEMO-map-008: OnZoomEnd Event Demo

**Priority:** P2
**Spec reference:** `docs/component-specs/map/events.md` (OnZoomEnd section)
**What's needed:** Demo showing zoom-end event returning new zoom level, center, and extent.
**Blocked by:** No OnZoomEnd event (SPEC-map-013). Requires JS interop.

---

### DEMO-map-009: OnPanEnd Event Demo

**Priority:** P2
**Spec reference:** `docs/component-specs/map/events.md` (OnPanEnd section)
**What's needed:** Demo showing pan-end event returning center and extent.
**Blocked by:** No OnPanEnd event (SPEC-map-014). Requires JS interop.

---

### DEMO-map-010: Multiple Layers Composed

**Priority:** P2
**Spec reference:** Multiple spec files show tile + marker + bubble layers composed together.
**What's needed:** Demo showing 2-3 layer types stacked together (e.g., tile base + markers + bubbles).
**Blocked by:** No layer system (SPEC-map-001).

---

### DEMO-map-011: Map Controls Demo

**Priority:** P3
**Spec reference:** `docs/component-specs/map/overview.md` (MapControls section)
**What's needed:** Demo showing attribution, navigator, and zoom controls with position configuration.
**Blocked by:** No controls implementation (SPEC-map-015).

---

### DEMO-map-012: Refresh Method Demo

**Priority:** P3
**Spec reference:** `docs/component-specs/map/overview.md` (Map Reference and Methods section)
**What's needed:** Demo showing programmatic map refresh after parameter changes.
**Blocked by:** No Refresh method (SPEC-map-016).

---

### DEMO-map-013: Marker Custom Template Demo

**Priority:** P3
**Spec reference:** `docs/component-specs/map/layers/marker.md` (Customizing Marker Appearance section)
**What's needed:** Demo showing custom marker templates via `MapLayerMarkerSettings Template`.
**Blocked by:** No layer system or template support (SPEC-map-001).

---

### DEMO-map-014: Marker Tooltip Demo

**Priority:** P3
**Spec reference:** `docs/component-specs/map/layers/marker.md` (Setting Marker Tooltips section)
**What's needed:** Demo showing `MapLayerMarkerSettingsTooltip` with template.
**Blocked by:** No tooltip implementation.

---

## Missing State Demos

| State | Status | Notes |
|-------|--------|-------|
| Empty/no-data | MISSING | Map with no layers or markers |
| Pannable=false | MISSING | Panning disabled state |
| Zoomable=false | MISSING | Zooming disabled state |
| Bounds-constrained | MISSING | Map with Bounds parameter limiting viewport |

---

## Summary Counts

| Category | Count |
|----------|-------|
| Present scenarios | 3 (all prototype API) |
| Missing P1 scenarios | 4 (all blocked by MapLibre adapter) |
| Missing P2 scenarios | 5 (blocked by source implementation) |
| Missing P3 scenarios | 5 (deferred features) |
| Missing state demos | 4 |
| **Total missing** | **18** |
