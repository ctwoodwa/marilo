# MariloMap Gap Inventory -- Stage 01 (Intake)

**Date:** 2026-04-12
**Worker:** w-map-gap-analysis
**Intake mode:** Import from delivery audit (map-delivery stages 01+04) + ADR cross-reference
**Source inputs:**
- `map-delivery/stages/01-spec-review/output/map-spec-gaps.md` (20 gaps)
- `map-delivery/stages/04-sync-check/output/map-delivery-report.md`
- `docs/component-specs/map/architecture-decision-tile-engine.md`
- Current source: `MariloMap.razor` (prototype), `MapModels.cs` (2 types only)

---

## Context

MariloMap is a prototype component. The architecture decision (MapLibre GL JS) is complete and specs have been revised to target the MapLibre-based design. The source is a placeholder that renders a container div with basic JS interop for a MapLibre style URL, positioned markers, and a single `OnMarkerClick` callback. The entire layer system, typed events, model types, and provider SCSS are missing.

The delivery audit identified 20 spec gaps. This inventory imports those and adds 3 additional gaps discovered by cross-referencing the ADR's internal design against the current source, for a total of **23 gaps**.

---

## Gap Inventory

### GAP-MAP-001: MapLayers/MapLayer Child Content Architecture

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-001 |
| **Source** | SPEC-map-001 |
| **Description** | Spec and ADR describe a CascadingValue-based child-content architecture with `<MapLayers>` container and individual `<MapLayer>` children. Source uses a flat `List<MapMarker>` parameter with no layer system. |
| **Severity** | Critical |
| **Type** | spec-ahead |
| **Blocking** | Yes -- blocks all layer types (tile, marker, shape, bubble) |
| **ADR Reference** | Layer Registration section; follows MariloDataGrid CascadingValue pattern |

---

### GAP-MAP-002: MapLayerType Enum

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-002 |
| **Source** | SPEC-map-002 |
| **Description** | `MapLayerType` enum (`Tile`, `Marker`, `Shape`, `Bubble`) is referenced throughout spec and ADR but does not exist in `MapModels.cs`. |
| **Severity** | Critical |
| **Type** | spec-ahead |
| **Blocking** | Yes -- required by MapLayer.Type parameter |

---

### GAP-MAP-003: Tile Layer Support

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-003 |
| **Source** | SPEC-map-003 |
| **Description** | Tile layer with `UrlTemplate` (`{s}/{z}/{x}/{y}` syntax), `Subdomains`, `Attribution`, `TileSize`, `Opacity`, `MinZoom`, `MaxZoom` parameters. Source has no tile rendering -- the prototype shows a static grid. |
| **Severity** | Critical |
| **Type** | spec-ahead |
| **Blocking** | Yes -- core map functionality |

---

### GAP-MAP-004: Shape/GeoJSON Layer Support

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-004 |
| **Source** | SPEC-map-004 |
| **Description** | Shape layer accepting GeoJSON `Data` string, with `MapLayerShapeSettings` for fill/stroke styling. Source has no shape layer. |
| **Severity** | Critical |
| **Type** | spec-ahead |
| **Blocking** | Yes -- blocks GeoJSON rendering |

---

### GAP-MAP-005: Bubble Layer Support

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-005 |
| **Source** | SPEC-map-005 |
| **Description** | Bubble layer with `LocationField`, `ValueField`, `MinSize`, `MaxSize`, data-driven circle sizing via `MapLayerBubbleSettings`. Source has no bubble layer. |
| **Severity** | Critical |
| **Type** | spec-ahead |
| **Blocking** | Yes -- blocks data-visualization layer |

---

### GAP-MAP-006: Zoom Type Mismatch

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-006 |
| **Source** | SPEC-map-006 |
| **Description** | Spec and ADR declare `Zoom` as `double` for fractional zoom levels. Source uses `int`. MapLibre natively uses float zoom. |
| **Severity** | Important |
| **Type** | mismatch |
| **Blocking** | No -- functional but imprecise |

---

### GAP-MAP-007: MinZoom Parameter

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-007 |
| **Source** | SPEC-map-007 |
| **Description** | `MinZoom` parameter (`double`) is in spec and ADR but missing from source. |
| **Severity** | Important |
| **Type** | spec-ahead |
| **Blocking** | No |

---

### GAP-MAP-008: MaxZoom Parameter

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-008 |
| **Source** | SPEC-map-008 |
| **Description** | `MaxZoom` parameter (`double`) is in spec and ADR but missing from source. |
| **Severity** | Important |
| **Type** | spec-ahead |
| **Blocking** | No |

---

### GAP-MAP-009: Bounds Parameter

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-009 |
| **Source** | SPEC-map-009 |
| **Description** | `Bounds` parameter (`MapBounds?` with NorthEast/SouthWest `MapCenter`) is in spec and ADR but missing from source. Requires `MapBounds` model. |
| **Severity** | Important |
| **Type** | spec-ahead |
| **Blocking** | No |

---

### GAP-MAP-010: OnClick Event

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-010 |
| **Source** | SPEC-map-010 |
| **Description** | `OnClick` event (`EventCallback<MapClickEventArgs>`) with `Location` (MapLocation lat/lng) and browser `EventArgs`. Not implemented in source. Requires JS-to-.NET event forwarding. |
| **Severity** | Important |
| **Type** | spec-ahead |
| **Blocking** | No -- but needed for interactive scenarios |

---

### GAP-MAP-011: OnMarkerClick Event Signature Mismatch

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-011 |
| **Source** | SPEC-map-011 |
| **Description** | Source has `EventCallback<MapMarker>`. Spec requires `EventCallback<MapMarkerClickEventArgs>` with `DataItem` (object) and `EventArgs`. The prototype callback passes a raw `MapMarker`; the target API uses typed args with the bound data item. |
| **Severity** | Important |
| **Type** | mismatch |
| **Blocking** | No -- functional but wrong signature |

---

### GAP-MAP-012: OnShapeClick Event

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-012 |
| **Source** | SPEC-map-012 |
| **Description** | `OnShapeClick` event (`EventCallback<MapShapeClickEventArgs>`) with `DataItem` (for bubbles) or `GeoJsonDataItem` (for shapes). Not implemented. |
| **Severity** | Important |
| **Type** | spec-ahead |
| **Blocking** | No -- depends on shape/bubble layers |

---

### GAP-MAP-013: OnZoomEnd Event

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-013 |
| **Source** | SPEC-map-013 |
| **Description** | `OnZoomEnd` event (`EventCallback<MapZoomEndEventArgs>`) with `Zoom`, `Center`, `Extent` (MapBounds). Not implemented. Requires JS event forwarding. |
| **Severity** | Important |
| **Type** | spec-ahead |
| **Blocking** | No |

---

### GAP-MAP-014: OnPanEnd Event

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-014 |
| **Source** | SPEC-map-014 |
| **Description** | `OnPanEnd` event (`EventCallback<MapPanEndEventArgs>`) with `Center`, `Extent` (MapBounds). Not implemented. Requires JS event forwarding. |
| **Severity** | Important |
| **Type** | spec-ahead |
| **Blocking** | No |

---

### GAP-MAP-015: MapControls (Attribution, Navigator, Zoom)

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-015 |
| **Source** | SPEC-map-015 |
| **Description** | `MapControlsAttribution`, `MapControlsNavigator`, `MapControlsZoom` child components with `Position` (`MapControlsPosition` enum). Wraps MapLibre native controls with positional configuration. Not implemented. |
| **Severity** | Nice-to-have |
| **Type** | spec-ahead |
| **Blocking** | No -- MapLibre adds default controls automatically |

---

### GAP-MAP-016: Refresh Method

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-016 |
| **Source** | SPEC-map-016 |
| **Description** | Public `Refresh()` method on component ref to force MapLibre re-render after programmatic changes. Not implemented. |
| **Severity** | Nice-to-have |
| **Type** | spec-ahead |
| **Blocking** | No |

---

### GAP-MAP-017: Class Parameter

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-017 |
| **Source** | SPEC-map-017 |
| **Description** | Spec lists explicit `Class` parameter. Source uses `CombineClasses("mar-map")` from base but no explicit `[Parameter] public string? Class`. May be handled by `MariloComponentBase` via `AdditionalAttributes`. Needs verification. |
| **Severity** | Nice-to-have |
| **Type** | spec-ahead |
| **Blocking** | No |

---

### GAP-MAP-018: OnMapReady Escape Hatch

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-018 |
| **Source** | SPEC-map-018 |
| **Description** | `Func<IJSObjectReference, Task>?` callback that provides direct access to the underlying MapLibre instance for advanced consumers. Defined in ADR as escape hatch policy. Not implemented. |
| **Severity** | Nice-to-have |
| **Type** | spec-ahead |
| **Blocking** | No |

---

### GAP-MAP-019: Missing Model Types

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-019 |
| **Source** | SPEC-map-019 |
| **Description** | `MapModels.cs` contains only `MapMarker` and `MapCenter`. The following types are referenced in spec/ADR but do not exist: |
| **Severity** | Critical |
| **Type** | spec-ahead |
| **Blocking** | Yes -- blocks all typed APIs |

**Missing types:**
- `MapBounds` (NorthEast, SouthWest as MapCenter)
- `MapLocation` (Latitude, Longitude -- used by MapClickEventArgs)
- `MapClickEventArgs`
- `MapMarkerClickEventArgs` (DataItem, EventArgs)
- `MapShapeClickEventArgs` (DataItem, GeoJsonDataItem, EventArgs)
- `MapZoomEndEventArgs` (Zoom, Center, Extent)
- `MapPanEndEventArgs` (Center, Extent)
- `MapLayerType` enum (Tile, Marker, Shape, Bubble)
- `MapMarkerShape` enum (Pin, PinTarget)
- `MapControlsPosition` enum
- `MapLayerShapeSettings` + sub-types (Fill, Stroke styling)
- `MapLayerBubbleSettings` + sub-types (Fill styling)
- `MapLayerMarkerSettings` (Template, Tooltip)

---

### GAP-MAP-020: Prototype Markers Parameter (Deprecation)

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-020 |
| **Source** | SPEC-map-020 |
| **Description** | Source has `[Parameter] public List<MapMarker> Markers` -- a flat marker list from the prototype. Not in spec. Should be removed when layer-based marker support (GAP-MAP-001) is implemented. |
| **Severity** | Important |
| **Type** | source-ahead / undocumented |
| **Blocking** | No -- but must be cleaned up |

---

### GAP-MAP-021: IMapEngineAdapter + MapLibreAdapter (Internal)

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-021 |
| **Source** | ADR internal design |
| **Description** | ADR defines `IMapEngineAdapter` (internal interface) and `MapLibreAdapter` (internal class owning `IJSObjectReference`). Source has inline JS interop in the component with no adapter abstraction. The adapter boundary enables future engine swaps (Leaflet, OpenLayers) and testability via mocking. |
| **Severity** | Critical |
| **Type** | spec-ahead (ADR-driven) |
| **Blocking** | Yes -- all JS interop should route through adapter |

---

### GAP-MAP-022: JS Module (maplibre-adapter.js)

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-022 |
| **Source** | ADR internal design |
| **Description** | ADR specifies `maplibre-adapter.js` exporting `init`, `updateViewport`, `addLayer`, `removeLayer`, `updateLayer`, `destroy`. Source has a basic `marilo-map.js` with `initMap` and `disposeMap` only. The full JS module must handle layer management, event forwarding via `DotNetObjectReference`, and MapLibre lifecycle. |
| **Severity** | Critical |
| **Type** | spec-ahead (ADR-driven) |
| **Blocking** | Yes -- core engine integration |

---

### GAP-MAP-023: Provider SCSS (FluentUI, Bootstrap, Material)

| Field | Value |
|-------|-------|
| **ID** | GAP-MAP-023 |
| **Source** | Delivery report VP-map-001/005/006 |
| **Description** | No provider SCSS exists for the map component in any theme. Need `_map.scss` in FluentUI (P1), Bootstrap (P2), and Material (P3) providers for container styling, control button styling, marker popup styling, and dark mode support. |
| **Severity** | Important |
| **Type** | missing |
| **Blocking** | No -- MapLibre provides default styling, but provider integration needed for theme consistency |

---

## Summary Counts

| Category | Count |
|----------|-------|
| Critical | 8 (GAP-MAP-001, 002, 003, 004, 005, 019, 021, 022) |
| Important | 11 (GAP-MAP-006-014, 020, 023) |
| Nice-to-have | 4 (GAP-MAP-015, 016, 017, 018) |
| **Total** | **23** |

| Type | Count |
|------|-------|
| spec-ahead | 17 |
| mismatch | 2 |
| source-ahead / undocumented | 1 |
| missing (provider) | 1 |
| ADR-driven (internal design) | 2 |

| Blocking | Count |
|----------|-------|
| Yes | 8 |
| No | 15 |
