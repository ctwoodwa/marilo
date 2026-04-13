# Stage 02 — Example UX Gap List: MariloMap
**Date:** 2026-04-11
**Demo file audited:** `samples/Marilo.Demo/Pages/Components/Map/Overview.razor`
**Spec areas audited:**
- `docs/component-specs/map/overview.md` — Map Parameters, MapControls, Refresh method
- `docs/component-specs/map/events.md` — OnClick, OnMarkerClick, OnShapeClick, OnZoomEnd, OnPanEnd
- `docs/component-specs/map/layers/overview.md` — MapLayer parameters
- `docs/component-specs/map/layers/tile.md`
- `docs/component-specs/map/layers/marker.md`
- `docs/component-specs/map/layers/bubble.md`
- `docs/component-specs/map/layers/shape.md`

---

## Summary

| Status | Count |
|---|---|
| Covered | 4 |
| Missing | 28 |
| Partial | 3 |
| Blocked-by-source | 24 |

---

## Covered Scenarios
*Spec areas that have at least one adequate demo scenario in the current demo page.*

| Spec Area | Demo Section | Notes |
|---|---|---|
| `Center` parameter (prototype model) | "Basic Usage" | Uses `MapCenter` object — covered for prototype |
| `Zoom` parameter | "Custom Zoom Level" | Range slider controls zoom in real time — satisfies user-controllable input requirement |
| `Markers` parameter | "Basic Usage", "Interactive Markers" | Both use `List<MapMarker>`; multiple geographic datasets shown |
| `OnMarkerClick` event (prototype signature) | "Interactive Markers" | Click handler updates selected-marker display — event is triggered and result is visible |

---

## Missing Scenarios
*Spec areas not covered by any demo scenario in the current demo page. Grouped by spec section.*

### Overview — Map Parameters

**GAP-UX-001: `Pannable` parameter**
- Status: Missing
- Spec ref: `docs/component-specs/map/overview.md` line 110
- No scenario demonstrates enabling/disabling panning.
- Recommended: Add a toggle demo that sets `Pannable="false"` and explains the use case (locked reference map).

**GAP-UX-002: `Zoomable` parameter**
- Status: Missing
- Spec ref: `docs/component-specs/map/overview.md` line 113
- No scenario demonstrates disabling user zoom control.
- Recommended: Add a toggle demo paired with `Pannable` — both are lockdown parameters and belong together.

**GAP-UX-003: `Width` parameter**
- Status: Missing
- Spec ref: `docs/component-specs/map/overview.md` line 115
- Demo always uses default or hardcoded pixel heights; `Width` is never set or discussed.
- Recommended: Add a scenario showing responsive width (`Width="100%"`) vs. fixed (`Width="600px"`).

**GAP-UX-004: `Height` parameter (partial — see Partial list)**
- Status: Partial (see GAP-UX-P01)

**GAP-UX-005: `MinZoom` / `MaxZoom` (root parameters)**
- Status: Blocked-by-source (parameters not implemented)
- Spec ref: `docs/component-specs/map/overview.md` lines 107–108
- Cannot demo until implemented.

**GAP-UX-006: `MinSize` parameter**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/overview.md` line 109
- Cannot demo until implemented.

**GAP-UX-007: `WrapAround` parameter**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/overview.md` line 111
- Cannot demo until implemented.

**GAP-UX-008: `Class` parameter**
- Status: Missing
- Spec ref: `docs/component-specs/map/overview.md` line 114
- No scenario shows custom CSS class on the root element.
- Recommended: Add brief note or scenario showing `Class="my-map"` for custom theming.

**GAP-UX-009: `Refresh()` method**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/overview.md` lines 140–174
- Method is not implemented in source. Cannot demo until `Refresh()` exists.
- Recommended once unblocked: Show a button that programmatically changes `Zoom` and calls `MapRef.Refresh()`.

### Overview — MapControls

**GAP-UX-010: `MapControlsAttribution.Position`**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/overview.md` lines 118–125
- Controls sub-components not implemented.

**GAP-UX-011: `MapControlsNavigator.Position`**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/overview.md` lines 127–129
- Controls sub-components not implemented.

**GAP-UX-012: `MapControlsZoom.Position`**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/overview.md` lines 131–133
- Controls sub-components not implemented.

### Events

**GAP-UX-013: `OnClick` event**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/events.md` lines 22–124
- `OnClick` (`MapClickEventArgs`) is not implemented in source.
- Recommended once unblocked: Show click coordinates appearing in a readout below the map.

**GAP-UX-014: `OnMarkerClick` (spec-compliant args type)**
- Status: Partial (see GAP-UX-P02)
- Current demo uses `EventCallback<MapMarker>` (prototype), not `MapMarkerClickEventArgs`.

**GAP-UX-015: `OnShapeClick` event**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/events.md` lines 233–338
- Not implemented. Requires Shape layer implementation.

**GAP-UX-016: `OnZoomEnd` event**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/events.md` lines 340–446
- Not implemented.

**GAP-UX-017: `OnPanEnd` event**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/events.md` lines 448–553
- Not implemented.

### Layers — Tile

**GAP-UX-018: Tile layer basic configuration**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/layers/tile.md`
- `MapLayers` / `MapLayer Type="Tile"` component tree not implemented. The demo uses a prototype canvas, not a real tile layer.
- Recommended once unblocked: Show the basic tile layer with `UrlTemplate`, `Attribution`, `Subdomains`.

**GAP-UX-019: Tile layer CSP-compliant `UrlTemplate` (JS function)**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/overview.md` lines 63–97
- Cannot demo until tile layer exists.

### Layers — Marker (data-bound)

**GAP-UX-020: Data-bound marker layer (`MapLayer Type="Marker"`, `Data`, `LocationField`, `TitleField`)**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/layers/marker.md` lines 24–80
- Current demo uses the prototype `Markers` parameter (flat `List<MapMarker>`), not the spec's data-bound layer architecture.

**GAP-UX-021: Marker layer — custom template**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/layers/marker.md` lines 83–191
- `MapLayerMarkerSettings.Template` not implemented.

**GAP-UX-022: Marker layer — shape (`MapMarkersShape` enum)**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/layers/marker.md` lines 193–258
- `MapLayer.Shape` not implemented.

**GAP-UX-023: Marker layer — tooltip template**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/layers/marker.md` lines 260–318
- `MapLayerMarkerSettingsTooltip` not implemented.

### Layers — Bubble

**GAP-UX-024: Bubble layer basic configuration**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/layers/bubble.md`
- `MapLayer Type="Bubble"` not implemented.

**GAP-UX-025: Bubble layer styling (fill color, stroke color)**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/layers/overview.md` lines 59–77
- `MapLayerBubbleSettingsStyleFill` / `MapLayerBubbleSettingsStyleStroke` not implemented.

### Layers — Shape

**GAP-UX-026: Shape layer (GeoJSON binding)**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/layers/shape.md`
- `MapLayer Type="Shape"` not implemented.

**GAP-UX-027: Shape layer styling**
- Status: Blocked-by-source
- Spec ref: `docs/component-specs/map/layers/overview.md` lines 79–97
- `MapLayerShapeSettingsStyleFill` / `MapLayerShapeSettingsStyleStroke` not implemented.

### Required Demo Page Completeness (per demo-scenario-format.md)

**GAP-UX-028: Disabled / no-data / error states**
- Status: Missing
- The demo page has no scenario for an empty `Markers` list, no disabled state, and no error state.
- Recommended: Add an "Empty State" section with `Markers="new()"` to show the default map with no pins.

---

## Partial Scenarios
*Demo exists but is incomplete relative to the demo-scenario-format.md requirements.*

**GAP-UX-P01: `Height` parameter**
- Demo sections: "Basic Usage" (`Height="400px"`), "Custom Zoom Level" (`Height="350px"`)
- Gap: Height is used in every demo but is never the **primary focus** of its own scenario. No description explains when to choose pixel vs. percentage height. No user-controllable input lets the developer change it live.
- Recommended: Either promote `Height` + `Width` into a dedicated "Sizing" scenario with live controls, or add `Width`/`Height` to the parameter table in the "Basic Usage" scenario with a note.

**GAP-UX-P02: `OnMarkerClick` event**
- Demo section: "Interactive Markers"
- Gap: The callback signature uses the prototype `EventCallback<MapMarker>` rather than the spec's `MapMarkerClickEventArgs`. When the args type is corrected, the demo handler (`HandleMarkerClick`) must be updated to match. Also missing: the scenario has no visible confirmation that `EventArgs` (mouse coordinates) are accessible.
- Recommended: Update handler to accept `MapMarkerClickEventArgs` once the event type is aligned; add a second readout line showing `clientX`/`clientY`.

**GAP-UX-P03: `Zoom` scenario — parameter table and spec link**
- Demo section: "Custom Zoom Level"
- Gap: The scenario has a live control (range input) and a code snippet, but is missing the parameter table (`| Parameter | Value | Notes |`) and a spec anchor link, both required by demo-scenario-format.md.
- Recommended: Add the parameter table and a reference link to `docs/component-specs/map/overview.md#map-parameters`.

---

## Notes

1. **Root cause of most gaps:** The source is a prototype canvas component (`MariloMap.razor`) without the layer sub-component architecture. The 24 "Blocked-by-source" items will become demable once the full `MapLayers` / `MapLayer` component tree is implemented (tracked in Stage 01 as SPEC-map-S01 through SPEC-map-S21).

2. **Demo page is clean for the prototype surface:** The three existing demo sections ("Basic Usage", "Interactive Markers", "Custom Zoom Level") accurately represent the actual implemented API. No stale snippets were found.

3. **Priority order for demo additions once source gaps close:**
   - P1: Tile layer basic, OnClick, OnMarkerClick (corrected args), data-bound marker layer
   - P2: Pannable/Zoomable toggles, Bubble layer, OnZoomEnd/OnPanEnd, empty state
   - P3: Shape layer, marker templates/tooltips, controls positioning, Refresh method
