# Stage 01 — Spec Review: MariloMap
**Date:** 2026-04-11
**Source audited:** `src/Marilo.Components/DataDisplay/MariloMap.razor` (lines 54–103)
**Models audited:** `src/Marilo.Core/Models/MapModels.cs`
**Spec files audited:**
- `docs/component-specs/map/overview.md`
- `docs/component-specs/map/events.md`
- `docs/component-specs/map/layers/overview.md`
- `docs/component-specs/map/layers/tile.md`
- `docs/component-specs/map/layers/marker.md`
- `docs/component-specs/map/layers/bubble.md`
- `docs/component-specs/map/layers/shape.md`

---

## Summary

| Category | Count |
|---|---|
| Undocumented (in source, missing from spec) | 4 |
| Spec-ahead (in spec, missing from source) | 27 |
| Mismatch (name or shape differs) | 3 |

---

## List 1: Undocumented
*In source, not documented in spec.*

---

**ID:** SPEC-map-001
**Type:** undocumented
**Parameter/Event:** `Markers`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `Markers` |
| Type | missing | `List<MapMarker>` |
| Default | missing | `new()` (empty list) |
| Description | missing | Collection of pin markers rendered on the prototype canvas |

**Recommended action:** Add `Markers` parameter to `docs/component-specs/map/overview.md` — Map Parameters table.
**Delegated to:** spec update only

---

**ID:** SPEC-map-002
**Type:** undocumented
**Parameter/Event:** `OnMarkerClick` (source signature)
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnMarkerClick` (events.md) | `OnMarkerClick` |
| Type | `EventCallback` with `MapMarkerClickEventArgs` | `EventCallback<MapMarker>` |
| Default | missing | — |
| Description | events.md describes the event but with a different args type | Source passes the raw `MapMarker` model, not a `MapMarkerClickEventArgs` wrapper |

**Note:** This is also a Mismatch — recorded separately as SPEC-map-M01 below.

---

**ID:** SPEC-map-003
**Type:** undocumented
**Parameter/Event:** `MapCenter` (model type)
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | missing | `MapCenter` class |
| Type | missing | `class { Latitude: double, Longitude: double }` |
| Default | missing | N/A |
| Description | Spec uses `double[]` for Center everywhere; source uses a `MapCenter` object | Source `Center` parameter is `MapCenter?` not `double[]` |

**Recommended action:** Spec uses `double[]` arrays throughout (`overview.md` line 106, all layer examples). Source uses a typed `MapCenter` model. The spec must be updated to reflect the actual model type, or the source must be aligned to `double[]`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-004
**Type:** undocumented
**Parameter/Event:** `Zoom` (type mismatch — also Mismatch SPEC-map-M02)
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Zoom` | `Zoom` |
| Type | `double` | `int` |
| Default | missing | `5` |
| Description | Spec documents Zoom as double; source declares it as int | See SPEC-map-M02 |

---

## List 2: Spec-ahead
*Documented in spec, not implemented in source.*

The current source is a prototype rendering engine (grid canvas with marker pins). The full layer architecture documented in the spec (`MapLayers`, `MapLayer`, `MapLayersType`, child component tree) does not exist in source. All items below are spec-ahead.

---

**ID:** SPEC-map-S01
**Type:** spec-ahead
**Parameter/Event:** `MapLayers` / `MapLayer` child component system
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MapLayers` + `MapLayer` child tags | missing |
| Type | `RenderFragment` / child components | missing |
| Default | missing | N/A |
| Description | The entire layer architecture (Tile, Marker, Bubble, Shape) is spec-documented but not implemented |

**Recommended action:** Implement layer component architecture.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S02
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.Type` (`MapLayersType` enum)
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Type` | missing |
| Type | `MapLayersType` enum (Tile, Marker, Bubble, Shape) | missing |
| Default | missing | N/A |
| Description | Layer type discriminator — not implemented |

**Recommended action:** Implement as part of `MapLayer` component.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S03
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.Attribution`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Attribution` | missing |
| Type | `string` | missing |
| Default | missing | N/A |
| Description | Tile-layer attribution string (copyright notice) |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S04
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.UrlTemplate`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `UrlTemplate` | missing |
| Type | `string` | missing |
| Default | missing | N/A |
| Description | URL template for tile layers; supports both `#= x #` legacy and JS function CSP-compliant syntax |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S05
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.Subdomains`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Subdomains` | missing |
| Type | `string[]` | missing |
| Default | missing | N/A |
| Description | Tile subdomains for parallel tile loading |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S06
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.Data`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Data` | missing |
| Type | `object` | missing |
| Default | missing | N/A |
| Description | Data source for Marker, Bubble, and Shape layers |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S07
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.LocationField`
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `LocationField` | missing |
| Type | `string` | missing |
| Default | missing | N/A |
| Description | Data item field name that holds `[latitude, longitude]` for Marker and Bubble layers |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S08
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.TitleField`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `TitleField` | missing |
| Type | `string` | missing |
| Default | missing | N/A |
| Description | Data item field name for marker title/label |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S09
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.ValueField`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `ValueField` | missing |
| Type | `string` | missing |
| Default | missing | N/A |
| Description | Numeric field for Bubble layer radius calculation |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S10
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.Shape` (`MapMarkersShape` enum)
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Shape` | missing |
| Type | `MapMarkersShape` enum (Pin, PinTarget) | missing |
| Default | `PinTarget` | N/A |
| Description | Visual shape of markers — Pin vs PinTarget |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S11
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.Symbol` (`MapLayersSymbol` enum)
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Symbol` | missing |
| Type | `MapLayersSymbol` enum | missing |
| Default | missing | N/A |
| Description | Default symbol for Bubble layers |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S12
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.Opacity`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Opacity` | missing |
| Type | `double` | missing |
| Default | missing | N/A |
| Description | Layer-level opacity |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S13
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.Extent`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Extent` | missing |
| Type | `double[]` | missing |
| Default | missing | N/A |
| Description | NW/SE bounding box that hides layer when out of view |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S14
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.MaxSize` / `MapLayer.MinSize`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MaxSize` / `MinSize` | missing |
| Type | `double` | missing |
| Default | missing | N/A |
| Description | Bubble symbol size range |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S15
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.MaxZoom` / `MapLayer.MinZoom`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MaxZoom` / `MinZoom` | missing |
| Type | `double` | missing |
| Default | missing | N/A |
| Description | Per-layer zoom visibility range |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S16
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.TileSize`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `TileSize` | missing |
| Type | `double` | missing |
| Default | missing | N/A |
| Description | Tile image size in pixels (default typically 256) |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S17
**Type:** spec-ahead
**Parameter/Event:** `MapLayer.ZIndex`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `ZIndex` | missing |
| Type | `double` | missing |
| Default | missing | N/A |
| Description | Stacking order override for layers |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S18
**Type:** spec-ahead
**Parameter/Event:** `MapLayerMarkerSettings` child component
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MapLayerMarkerSettings` | missing |
| Type | Child component with `Template` and `Tooltip` parameters | missing |
| Default | missing | N/A |
| Description | Container for marker customization (template + tooltip) |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S19
**Type:** spec-ahead
**Parameter/Event:** `MapLayerMarkerSettingsTooltip` / `Template` RenderFragment
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MapLayerMarkerSettingsTooltip.Template` | missing |
| Type | `RenderFragment` | missing |
| Default | missing | N/A |
| Description | Blazor template for marker tooltip content |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S20
**Type:** spec-ahead
**Parameter/Event:** `MapLayerBubbleSettings` / `MapLayerBubbleSettingsStyleFill` / `MapLayerBubbleSettingsStyleStroke`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Bubble style child component tree | missing |
| Type | `Color`, `Opacity` (Fill); `Color`, `DashType`, `Opacity`, `Width` (Stroke) | missing |
| Default | missing | N/A |
| Description | Visual customization for Bubble layer circles |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S21
**Type:** spec-ahead
**Parameter/Event:** `MapLayerShapeSettings` / `MapLayerShapeSettingsStyleFill` / `MapLayerShapeSettingsStyleStroke`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Shape style child component tree | missing |
| Type | `Color`, `Opacity` (Fill); `Color`, `DashType`, `Opacity`, `Width` (Stroke) | missing |
| Default | missing | N/A |
| Description | Visual customization for GeoJSON Shape layer polygons |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S22
**Type:** spec-ahead
**Parameter/Event:** `MapControls` / `MapControlsAttribution` / `MapControlsNavigator` / `MapControlsZoom`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | Controls child component tree | missing |
| Type | `MapControlsPosition` enum for each sub-control | missing |
| Default | missing | N/A |
| Description | Positioning of attribution, navigator, and zoom controls |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S23
**Type:** spec-ahead
**Parameter/Event:** `MinZoom` / `MaxZoom` (MariloMap root parameters)
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MinZoom` / `MaxZoom` | missing |
| Type | `double` | missing |
| Default | missing | N/A |
| Description | Global min/max zoom constraints on the root MariloMap |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S24
**Type:** spec-ahead
**Parameter/Event:** `MinSize` (MariloMap root parameter)
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `MinSize` | missing |
| Type | `double` | missing |
| Default | missing | N/A |
| Description | Map size in pixels at zoom level 0 |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S25
**Type:** spec-ahead
**Parameter/Event:** `WrapAround`
**Priority:** P3 (next phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `WrapAround` | missing |
| Type | `bool` | missing |
| Default | missing | N/A |
| Description | Whether the map wraps around east-west edges |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S26
**Type:** spec-ahead
**Parameter/Event:** `Class`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Class` | missing |
| Type | `string` | missing |
| Default | missing | N/A |
| Description | CSS class for the root DOM element; source uses `CombineClasses()` internally but does not expose a public `Class` parameter |

**Note:** Source uses `@attributes="AdditionalAttributes"` which may allow class pass-through via `MariloComponentBase`, but `Class` is not an explicit declared `[Parameter]`.
**Delegated to:** spec update only (clarify if `AdditionalAttributes` handles this)

---

**ID:** SPEC-map-S27
**Type:** spec-ahead
**Parameter/Event:** `Refresh()` method
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Refresh()` | missing |
| Type | `void` method | missing |
| Default | N/A | N/A |
| Description | Redraws the component after programmatic changes |

**Recommended action:** Implement `Refresh()` method on `MariloMap`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S28
**Type:** spec-ahead
**Parameter/Event:** `OnClick`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnClick` | missing |
| Type | `EventCallback<MapClickEventArgs>` | missing |
| Default | missing | N/A |
| Description | Fires on user click/tap on the map canvas; args expose `Location` (lat/lng) and native `EventArgs` |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S29
**Type:** spec-ahead
**Parameter/Event:** `OnShapeClick`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnShapeClick` | missing |
| Type | `EventCallback<MapShapeClickEventArgs>` | missing |
| Default | missing | N/A |
| Description | Fires when user clicks a shape; args expose `DataItem` and `GeoJsonDataItem` |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S30
**Type:** spec-ahead
**Parameter/Event:** `OnZoomEnd`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnZoomEnd` | missing |
| Type | `EventCallback<MapZoomEndEventArgs>` | missing |
| Default | missing | N/A |
| Description | Fires when zoom gesture completes; args expose `Zoom`, `Center`, `Extent` |

**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-S31
**Type:** spec-ahead
**Parameter/Event:** `OnPanEnd`
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnPanEnd` | missing |
| Type | `EventCallback<MapPanEndEventArgs>` | missing |
| Default | missing | N/A |
| Description | Fires when pan gesture completes; args expose `Center` and `Extent` |

**Delegated to:** gap-analysis-resolution intake

---

## List 3: Mismatches
*Parameter exists in both spec and source but name or shape differs.*

---

**ID:** SPEC-map-M01
**Type:** mismatch
**Parameter/Event:** `OnMarkerClick` — args type
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `OnMarkerClick` | `OnMarkerClick` |
| Type | `EventCallback<MapMarkerClickEventArgs>` | `EventCallback<MapMarker>` |
| Default | missing | — |
| Description | Spec: args wrapper with `DataItem` (object) + `EventArgs` (MouseEventArgs). Source: passes raw `MapMarker` model directly |

**Recommended action:** Either introduce `MapMarkerClickEventArgs` wrapper class in source and update handler, or update spec to match the simpler `EventCallback<MapMarker>` signature.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-M02
**Type:** mismatch
**Parameter/Event:** `Zoom` — parameter type
**Priority:** P2 (this phase)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Zoom` | `Zoom` |
| Type | `double` (overview.md line 112) | `int` (MariloMap.razor line 59) |
| Default | missing in spec | `5` in source |
| Description | Type mismatch: spec says double, source says int |

**Recommended action:** Align to `double` (spec) for consistency with layer `MinZoom`/`MaxZoom` parameters, or update spec to `int`.
**Delegated to:** gap-analysis-resolution intake

---

**ID:** SPEC-map-M03
**Type:** mismatch
**Parameter/Event:** `Center` — parameter type
**Priority:** P1 (blocking)

| Field | In Spec | In Source |
|-------|---------|-----------|
| Name | `Center` | `Center` |
| Type | `double[]` (all spec examples) | `MapCenter?` (MariloMap.razor line 56) |
| Default | missing in spec | `null` in source |
| Description | Spec consistently uses `double[]` with `[Latitude, Longitude]` semantics; source uses a typed `MapCenter` class with separate `Latitude`/`Longitude` properties |

**Recommended action:** Pick one canonical shape. Typed model (`MapCenter`) is better for Blazor parameter binding and IntelliSense. Update all spec examples and overview parameter table to use `MapCenter`.
**Delegated to:** gap-analysis-resolution intake

---

## Notes

- Source file `MariloMap.razor` is explicitly marked as a prototype: line 48 reads `"Prototype - tile provider integration required"`. The layer component tree (`MapLayers`, `MapLayer`, type-discriminated child components) is entirely absent — this accounts for the large spec-ahead count.
- `MapModels.cs` defines only `MapMarker` and `MapCenter`. None of the event args types (`MapClickEventArgs`, `MapMarkerClickEventArgs`, `MapShapeClickEventArgs`, `MapZoomEndEventArgs`, `MapPanEndEventArgs`) exist in source.
- The `AdditionalAttributes` pass-through from `MariloComponentBase` may implicitly support `Class` and style — verify before adding an explicit `[Parameter] public string? Class`.
