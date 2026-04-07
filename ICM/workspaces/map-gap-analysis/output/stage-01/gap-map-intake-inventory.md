# Gap Intake Inventory: MariloMap

> Component: MariloMap
> Intake date: 2026-04-03
> Intake mode: Fresh analysis (assess mode) -- no source code exists
> Spec docs: 7 files (overview, events, layers/overview, layers/tile, layers/marker, layers/shape, layers/bubble)
> Demo page: Placeholder only ("Coming soon")

---

## 1. Source Code Status

**No source files exist.** A search of `/workspaces/Marilo/src/Marilo.Components/` found zero Map-related files. This is a **standard greenfield intake**.

## 2. Spec Summary

The spec documents a geographic map component with tile-based rendering:

- **Core parameters:** 12 top-level -- Center, MinZoom, MaxZoom, MinSize, Pannable, WrapAround, Zoom, Zoomable, Class, Width, Height, plus controls (Navigator, Zoom, Attribution positions).
- **Layer types:** 4 types -- Tile (UrlTemplate, Attribution, Subdomains, TileSize), Marker (Data, LocationField, TitleField, Shape, Template, Tooltip), Shape (GeoJSON data binding, fill/stroke styling), Bubble (LocationField, ValueField, MinSize, MaxSize, Symbol, fill/stroke styling).
- **Layer parameters:** ~20 shared MapLayer parameters (Attribution, Data, Extent, LocationField, MaxSize, MaxZoom, MinSize, MinZoom, Opacity, Shape, Subdomains, Symbol, TileSize, TitleField, Type, UrlTemplate, ValueField, ZIndex, etc.).
- **Events:** 5 events -- OnClick (MapClickEventArgs with Location), OnMarkerClick (MapMarkerClickEventArgs with DataItem), OnShapeClick (MapShapeClickEventArgs with DataItem/GeoJsonDataItem), OnZoomEnd (MapZoomEndEventArgs), OnPanEnd (MapPanEndEventArgs).
- **Content Security Policy:** Dual template syntax (legacy inline #= =# and JS function-based CSP-compliant).
- **Methods:** Refresh.
- **Marker customization:** Template (JS function), shapes (Pin, PinTarget), tooltips with RenderFragment Template.

**Estimated parameter/feature count:** ~35 parameters across layers, 5 events, 1 method, 4 layer types.

## 3. Demo Page Status

The demo page at `/workspaces/Marilo/samples/Marilo.Demo/Pages/Components/Map/Overview.razor` is a **placeholder only** -- displays a "Coming soon" alert.

## 4. Rough Gap Count

| Feature Area | Estimated Gaps |
|---|---|
| Core component + JS rendering engine | 4 |
| Tile layer support | 3 |
| Marker layer (data binding, shapes, templates, tooltips) | 6 |
| Shape layer (GeoJSON) | 4 |
| Bubble layer (data binding, sizing, styling) | 4 |
| Pan/zoom interaction | 3 |
| Events (5 total) | 4 |
| Controls (Navigator, Zoom, Attribution) | 3 |
| CSP-compliant template support | 2 |
| Styling/theming | 2 |
| **Total** | **~35** |

## 5. Severity Breakdown Estimate

| Severity | Count | Examples |
|---|---|---|
| Critical | ~12 | Core rendering engine, tile layer with UrlTemplate, basic pan/zoom, marker layer data binding |
| Important | ~14 | Shape layer (GeoJSON), bubble layer, all 5 events, marker tooltips/templates, controls |
| Nice-to-have | ~9 | CSP-compliant JS function templates, WrapAround, custom marker shapes, ZIndex control, Extent filtering |

## 6. Delivery Workspace Recommendation

**YES -- merits its own delivery workspace.** The Map is a JS-interop-heavy component requiring a tile rendering engine, GeoJSON parsing, coordinate systems, and multiple layer types. It has a dedicated external dependency on tile providers. A dedicated `map-delivery/` workspace already exists. Scope: `systematic`.

---

**Next step:** Proceed to Stage 02 (prioritize) with tile layer rendering + pan/zoom as the critical foundation, then marker layer.
