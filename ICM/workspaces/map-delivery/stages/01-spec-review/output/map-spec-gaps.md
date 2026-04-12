# MariloMap Spec Review -- Stage 01 Output

**Date:** 2026-04-12
**Worker:** w-map-delivery
**Component:** MariloMap
**Build status:** dotnet build Marilo.slnx -- 0 errors, 0 warnings

---

## Summary

The spec has been significantly revised (2026-04-12) to align with the MapLibre architecture decision. The current source (`MariloMap.razor`) is a **prototype** with a flat marker list and no layer system, no JS interop, no tile rendering. The spec describes a full layered component with child `MapLayers`/`MapLayer` registration, typed events, and multiple layer types (Tile, Marker, Shape, Bubble). Nearly the entire spec is **spec-ahead** of the source.

**Models file** (`MapModels.cs`) contains only `MapMarker` and `MapCenter`. The spec references many additional types that do not exist yet.

---

## Spec-Ahead Gaps (spec declares it, source does not have it)

### SPEC-map-001: MapLayers/MapLayer Child Content Architecture

**ID:** SPEC-map-001
**Type:** spec-ahead
**Parameter/Event:** MapLayers / MapLayer child registration
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `<MapLayers>` + `<MapLayer>` child content | missing |
| Type | CascadingValue-based layer registration | flat `List<MapMarker>` parameter |
| Description | Spec describes a child-content architecture with `MapLayers` container and individual `MapLayer` children for each layer type | Source uses a flat `Markers` parameter of `List<MapMarker>` -- no layer system |

**Recommended action:** Implement full layer registration system per architecture decision.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-002: MapLayerType Enum

**ID:** SPEC-map-002
**Type:** spec-ahead
**Parameter/Event:** MapLayerType
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MapLayerType` | missing |
| Type | enum (`Tile`, `Marker`, `Shape`, `Bubble`) | missing |

**Recommended action:** Create `MapLayerType` enum in `MapModels.cs`.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-003: Tile Layer Support

**ID:** SPEC-map-003
**Type:** spec-ahead
**Parameter/Event:** MapLayer (Type=Tile)
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Tile layer with UrlTemplate, Subdomains, Attribution | missing |
| Type | MapLayer parameters | no tile rendering at all |
| Description | Spec describes full tile layer with `{s}/{z}/{x}/{y}` URL template, subdomain rotation, attribution display | Source shows a static grid background with "Prototype - tile provider integration required" notice |

**Recommended action:** Implement MapLibre JS adapter and tile layer rendering per architecture decision.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-004: Shape/GeoJSON Layer Support

**ID:** SPEC-map-004
**Type:** spec-ahead
**Parameter/Event:** MapLayer (Type=Shape)
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Shape layer with GeoJSON `Data` parameter | missing |
| Type | string (GeoJSON) data, MapLayerShapeSettings styling | missing entirely |

**Recommended action:** Implement shape layer with GeoJSON support via MapLibre adapter.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-005: Bubble Layer Support

**ID:** SPEC-map-005
**Type:** spec-ahead
**Parameter/Event:** MapLayer (Type=Bubble)
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Bubble layer with LocationField, ValueField, MinSize, MaxSize | missing |
| Type | Data-driven circle sizing with styling | missing entirely |

**Recommended action:** Implement bubble layer via MapLibre circle layers.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-006: Zoom Type Mismatch

**ID:** SPEC-map-006
**Type:** mismatch
**Parameter/Event:** Zoom
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Zoom | Zoom |
| Type | `double` | `int` |
| Default | unspecified | 5 |
| Description | Spec says `double` for fractional zoom levels (standard for web maps) | Source uses `int` |

**Recommended action:** Change source type to `double` to match spec and MapLibre convention.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-007: MinZoom Parameter

**ID:** SPEC-map-007
**Type:** spec-ahead
**Parameter/Event:** MinZoom
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | MinZoom | missing |
| Type | `double` | missing |

**Recommended action:** Add `MinZoom` parameter.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-008: MaxZoom Parameter

**ID:** SPEC-map-008
**Type:** spec-ahead
**Parameter/Event:** MaxZoom
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | MaxZoom | missing |
| Type | `double` | missing |

**Recommended action:** Add `MaxZoom` parameter.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-009: Bounds Parameter

**ID:** SPEC-map-009
**Type:** spec-ahead
**Parameter/Event:** Bounds
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Bounds | missing |
| Type | `MapBounds?` (NorthEast/SouthWest MapCenter) | missing |

**Recommended action:** Add `MapBounds` model and `Bounds` parameter.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-010: OnClick Event

**ID:** SPEC-map-010
**Type:** spec-ahead
**Parameter/Event:** OnClick
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnClick | missing |
| Type | `EventCallback<MapClickEventArgs>` | missing |
| Description | Spec describes map click with Location (lat/lng) and browser EventArgs | not implemented |

**Recommended action:** Implement via JS interop event forwarding.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-011: OnMarkerClick Event Signature Mismatch

**ID:** SPEC-map-011
**Type:** mismatch
**Parameter/Event:** OnMarkerClick
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnMarkerClick | OnMarkerClick |
| Type | `EventCallback<MapMarkerClickEventArgs>` (with DataItem + EventArgs) | `EventCallback<MapMarker>` |
| Description | Spec uses typed args with DataItem object + native EventArgs | Source passes the raw MapMarker object |

**Recommended action:** Create `MapMarkerClickEventArgs` and update callback signature.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-012: OnShapeClick Event

**ID:** SPEC-map-012
**Type:** spec-ahead
**Parameter/Event:** OnShapeClick
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnShapeClick | missing |
| Type | `EventCallback<MapShapeClickEventArgs>` | missing |

**Recommended action:** Implement with shape/bubble layer support.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-013: OnZoomEnd Event

**ID:** SPEC-map-013
**Type:** spec-ahead
**Parameter/Event:** OnZoomEnd
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnZoomEnd | missing |
| Type | `EventCallback<MapZoomEndEventArgs>` (Zoom, Center, Extent) | missing |

**Recommended action:** Implement via JS interop event forwarding.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-014: OnPanEnd Event

**ID:** SPEC-map-014
**Type:** spec-ahead
**Parameter/Event:** OnPanEnd
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnPanEnd | missing |
| Type | `EventCallback<MapPanEndEventArgs>` (Center, Extent) | missing |

**Recommended action:** Implement via JS interop event forwarding.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-015: MapControls (Attribution, Navigator, Zoom)

**ID:** SPEC-map-015
**Type:** spec-ahead
**Parameter/Event:** MapControlsAttribution, MapControlsNavigator, MapControlsZoom
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | MapControlsAttribution, MapControlsNavigator, MapControlsZoom with Position parameters | missing |
| Type | Child components with Position enum | missing |

**Recommended action:** Implement as MapLibre control wrappers.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-016: Refresh Method

**ID:** SPEC-map-016
**Type:** spec-ahead
**Parameter/Event:** Refresh()
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Refresh() | missing |
| Type | public method on component ref | missing |

**Recommended action:** Add Refresh method that re-renders MapLibre instance.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-017: Class Parameter

**ID:** SPEC-map-017
**Type:** spec-ahead
**Parameter/Event:** Class
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Class | missing (base class may handle via AdditionalAttributes) |
| Type | `string` | AdditionalAttributes captures arbitrary attributes |
| Description | Spec lists explicit Class parameter | Source uses `CombineClasses("mar-map")` from base, but no explicit `[Parameter] public string? Class` |

**Recommended action:** Verify if `MariloComponentBase` handles Class via AdditionalAttributes or add explicit parameter.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-018: OnMapReady Escape Hatch

**ID:** SPEC-map-018
**Type:** spec-ahead
**Parameter/Event:** OnMapReady
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | OnMapReady (from architecture decision) | missing |
| Type | `Func<IJSObjectReference, Task>?` | missing |

**Recommended action:** Implement as part of MapLibre adapter lifecycle.
**Delegated to:** gap-analysis-resolution intake

---

### SPEC-map-019: Missing Model Types

**ID:** SPEC-map-019
**Type:** spec-ahead
**Parameter/Event:** Multiple model types
**Priority:** P1 (blocking)

Types referenced in spec but missing from `MapModels.cs`:
- `MapBounds` (NorthEast, SouthWest)
- `MapLocation` (Latitude, Longitude -- used by MapClickEventArgs)
- `MapClickEventArgs`
- `MapMarkerClickEventArgs`
- `MapShapeClickEventArgs`
- `MapZoomEndEventArgs`
- `MapPanEndEventArgs`
- `MapLayerType` enum
- `MapMarkerShape` enum
- `MapControlsPosition` enum
- `MapLayersSymbol` enum
- `DashType` enum (may exist elsewhere)

**Recommended action:** Create all model types in MapModels.cs.
**Delegated to:** gap-analysis-resolution intake

---

## Source-Ahead Gaps (source has it, spec does not mention it)

### SPEC-map-020: Markers Flat Parameter

**ID:** SPEC-map-020
**Type:** undocumented
**Parameter/Event:** Markers
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing from spec | `Markers` |
| Type | missing | `List<MapMarker>` |
| Default | N/A | `new()` |
| Description | N/A | Flat list of markers rendered directly -- prototype API |

**Recommended action:** Remove when layer-based marker support is implemented. This is a prototype convenience parameter.
**Delegated to:** gap-analysis-resolution intake (deprecate/remove during layer implementation)

---

## Summary Counts

| Category | Count |
|----------|-------|
| Spec-ahead (P1 blocking) | 6 |
| Spec-ahead (P2 this phase) | 7 |
| Spec-ahead (P3 next phase) | 4 |
| Mismatch | 2 |
| Source-ahead / undocumented | 1 |
| **Total gaps** | **20** |
