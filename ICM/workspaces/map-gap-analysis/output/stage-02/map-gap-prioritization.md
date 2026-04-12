# MariloMap Gap Prioritization -- Stage 02

**Date:** 2026-04-12
**Worker:** w-map-gap-analysis
**Input:** `stage-01/map-gap-inventory.md` (23 gaps)
**Sequencing basis:** ADR "Next Steps" section + dependency analysis

---

## Sequencing Rationale

The ADR defines a clear implementation sequence: models -> JS module -> adapter -> component rewrite -> SCSS -> demo -> tests. Gaps are sorted into 4 waves following this sequence. Each wave's gate must pass before the next wave starts, because later waves depend on earlier ones.

**Dependency chain:**
- Wave 1 (Models) produces the types that Wave 2 (JS/Adapter) and Wave 3 (Component) consume.
- Wave 2 (JS/Adapter) produces the interop layer that Wave 3 (Component) calls.
- Wave 3 (Component) produces the working component that Wave 4 (SCSS/Demo/Tests) validates.

---

## Wave 1: Model Expansion

**Goal:** Create all missing model and enum types in `MapModels.cs` so that subsequent waves can reference them.

**Gate:** All types compile. No runtime behavior change -- models only.

| Gap ID | Description | Severity | Effort |
|--------|-------------|----------|--------|
| GAP-MAP-019 | All missing model types (MapBounds, MapLocation, all EventArgs, all enums, all settings types) | Critical | Medium |
| GAP-MAP-002 | MapLayerType enum (Tile, Marker, Shape, Bubble) | Critical | Small |
| GAP-MAP-006 | Zoom type: change `int` to `double` | Important | Small |

**Notes:**
- GAP-MAP-019 is the umbrella; GAP-MAP-002 and GAP-MAP-006 are called out separately because they are independently referenced in the spec gaps list, but the work is done together as a single models expansion pass.
- `MapMarkerShape` enum values: `Pin`, `PinTarget` (from spec).
- `MapControlsPosition` enum values: `TopLeft`, `TopRight`, `BottomLeft`, `BottomRight` (standard MapLibre control positions).
- `MapLayerType` enum values: `Tile`, `Marker`, `Shape`, `Bubble`.
- All EventArgs types should follow Marilo conventions (public class, not record, with settable properties for JS deserialization).

**Files affected:**
- `src/Marilo.Core/Models/MapModels.cs`

**Estimated scope:** ~150-200 lines of model code.

---

## Wave 2: JS Module + MapLibre Adapter

**Goal:** Replace the prototype `marilo-map.js` with the full `maplibre-adapter.js` and create the `IMapEngineAdapter` / `MapLibreAdapter` C# abstraction.

**Gate:** Adapter compiles. JS module exports all required functions. `dotnet build` passes. No component rewrite yet -- adapter is internal and not wired up.

| Gap ID | Description | Severity | Effort |
|--------|-------------|----------|--------|
| GAP-MAP-022 | JS module: `init`, `updateViewport`, `addLayer`, `removeLayer`, `updateLayer`, `destroy`, event forwarding | Critical | Large |
| GAP-MAP-021 | IMapEngineAdapter interface + MapLibreAdapter implementation | Critical | Large |

**Notes:**
- The JS module must handle:
  - MapLibre GL JS lazy loading (from bundled or CDN source)
  - Map instance lifecycle (create, resize, destroy)
  - Layer CRUD (add/remove/update sources and layers)
  - Event listeners -> DotNetObjectReference.InvokeMethodAsync callbacks
  - UrlTemplate `{s}/{z}/{x}/{y}` -> MapLibre source format translation
  - GeoJSON source management for shape/bubble layers
  - Data collection -> GeoJSON feature conversion for marker/bubble layers
- The C# adapter must handle:
  - `MapInitOptions` -> JS init call
  - `MapLayerDescriptor` -> JS addLayer/updateLayer calls
  - `MapViewport` -> JS updateViewport calls
  - JS callback deserialization -> typed EventArgs
  - IAsyncDisposable lifecycle
- Reconnection handling: detect Blazor Server reconnect and re-initialize.

**Files affected:**
- `src/Marilo.Components/wwwroot/js/maplibre-adapter.js` (new, replaces marilo-map.js)
- `src/Marilo.Components/DataDisplay/Map/IMapEngineAdapter.cs` (new)
- `src/Marilo.Components/DataDisplay/Map/MapLibreAdapter.cs` (new)
- `src/Marilo.Components/DataDisplay/Map/MapInitOptions.cs` (new, internal)
- `src/Marilo.Components/DataDisplay/Map/MapLayerDescriptor.cs` (new, internal)
- `src/Marilo.Components/DataDisplay/Map/MapViewport.cs` (new, internal)

**Estimated scope:** ~300-400 lines C#, ~400-500 lines JS.

---

## Wave 3: Component Rewrite

**Goal:** Replace the prototype `MariloMap.razor` with the full component using `MapLayers`/`MapLayer` child registration, viewport binding, all events, and the adapter.

**Gate:** Component renders a tile layer with OSM tiles. Markers display. Events fire. `dotnet build` passes. Demo page updated and functional.

| Gap ID | Description | Severity | Effort |
|--------|-------------|----------|--------|
| GAP-MAP-001 | MapLayers/MapLayer child content architecture | Critical | Large |
| GAP-MAP-003 | Tile layer support (UrlTemplate, Subdomains, Attribution) | Critical | Medium |
| GAP-MAP-004 | Shape/GeoJSON layer support | Critical | Medium |
| GAP-MAP-005 | Bubble layer support | Critical | Medium |
| GAP-MAP-007 | MinZoom parameter | Important | Small |
| GAP-MAP-008 | MaxZoom parameter | Important | Small |
| GAP-MAP-009 | Bounds parameter | Important | Small |
| GAP-MAP-010 | OnClick event | Important | Medium |
| GAP-MAP-011 | OnMarkerClick signature fix (MapMarker -> MapMarkerClickEventArgs) | Important | Small |
| GAP-MAP-012 | OnShapeClick event | Important | Small |
| GAP-MAP-013 | OnZoomEnd event | Important | Small |
| GAP-MAP-014 | OnPanEnd event | Important | Small |
| GAP-MAP-020 | Remove prototype Markers parameter | Important | Small |
| GAP-MAP-018 | OnMapReady escape hatch | Nice-to-have | Small |

**Notes:**
- Layer registration follows `MariloDataGrid` -> `MariloGridColumn` CascadingValue pattern.
- `MapLayers` is a container component; `MapLayer` registers with parent on init, unregisters on dispose.
- `MariloMap` maintains internal `List<MapLayer>` and rebuilds `MapLayerDescriptor` list on parameter changes.
- Event forwarding: JS -> DotNetObjectReference -> adapter -> component -> EventCallback.
- GAP-MAP-020 (remove Markers param) happens naturally as the flat API is replaced by layers.
- The component rewrite is the largest single wave and may benefit from sub-phases:
  - 3a: Core component + tile layer (GAP-MAP-001, 003, 007, 008, 009)
  - 3b: Marker layer + events (GAP-MAP-010, 011, 020)
  - 3c: Shape + bubble layers + shape events (GAP-MAP-004, 005, 012)
  - 3d: Viewport events + escape hatch (GAP-MAP-013, 014, 018)

**Files affected:**
- `src/Marilo.Components/DataDisplay/MariloMap.razor` (rewrite)
- `src/Marilo.Components/DataDisplay/Map/MapLayers.razor` (new)
- `src/Marilo.Components/DataDisplay/Map/MapLayer.razor` (new)
- `src/Marilo.Components/DataDisplay/Map/MapLayerMarkerSettings.razor` (new)
- `src/Marilo.Components/DataDisplay/Map/MapLayerShapeSettings.razor` (new + sub-components)
- `src/Marilo.Components/DataDisplay/Map/MapLayerBubbleSettings.razor` (new + sub-components)

**Estimated scope:** ~400-500 lines Razor/C#.

---

## Wave 4: Provider CSS + Demo Update + Tests

**Goal:** Add theme-aware SCSS for all providers, rewrite the demo page to use the layer-based API, and add bUnit tests for component behavior.

**Gate:** All three provider SCSS files exist and build. Demo page renders real tiles. bUnit tests pass. Full `dotnet build` + `dotnet test` green.

| Gap ID | Description | Severity | Effort |
|--------|-------------|----------|--------|
| GAP-MAP-023 | Provider SCSS (FluentUI P1, Bootstrap P2, Material P3) | Important | Medium |
| GAP-MAP-015 | MapControls (Attribution, Navigator, Zoom) | Nice-to-have | Medium |
| GAP-MAP-016 | Refresh method | Nice-to-have | Small |
| GAP-MAP-017 | Class parameter verification | Nice-to-have | Small |

**Notes:**
- SCSS covers: `.mar-map` container, `.mar-map__controls` positioning, control button styling, marker popup styling, dark mode overrides.
- FluentUI is P1 (primary provider), Bootstrap and Material are P2/P3.
- GAP-MAP-015 (controls) is deferrable within Wave 4 -- MapLibre adds default controls automatically, so this is a polish item for positional customization.
- GAP-MAP-016 (Refresh) wraps a MapLibre `resize()` + `triggerRepaint()` call through the adapter.
- GAP-MAP-017 (Class) may already work via `MariloComponentBase.AdditionalAttributes` -- verify before implementing.
- bUnit tests should cover: layer registration/unregistration, parameter binding to adapter calls, event callback wiring, dispose lifecycle. JS interop calls are mocked via the `IMapEngineAdapter` interface.
- Demo page should demonstrate: tile layer, marker layer with click, shape layer with GeoJSON, bubble layer, zoom/pan events.

**Files affected:**
- `src/Marilo.Providers.FluentUI/Styles/Components/_map.scss` (new)
- `src/Marilo.Providers.Bootstrap/Styles/Components/_map.scss` (new, P2)
- `src/Marilo.Providers.Material/Styles/Components/_map.scss` (new, P3)
- `samples/Marilo.Demo/Pages/Components/Map/Overview.razor` (rewrite)
- `tests/Marilo.Tests.Unit/Components/DataDisplay/MariloMapTests.cs` (new)
- `src/Marilo.Components/DataDisplay/Map/MapControlsAttribution.razor` (new, if GAP-MAP-015 included)
- `src/Marilo.Components/DataDisplay/Map/MapControlsNavigator.razor` (new, if GAP-MAP-015 included)
- `src/Marilo.Components/DataDisplay/Map/MapControlsZoom.razor` (new, if GAP-MAP-015 included)

**Estimated scope:** ~200-300 lines SCSS, ~200 lines demo, ~300 lines tests.

---

## Wave Summary

| Wave | Gaps | Critical | Important | Nice-to-have | Estimated Total Scope |
|------|------|----------|-----------|--------------|----------------------|
| 1: Model Expansion | 3 | 2 | 1 | 0 | ~200 lines |
| 2: JS Module + Adapter | 2 | 2 | 0 | 0 | ~800 lines |
| 3: Component Rewrite | 14 | 4 | 7 | 1 | ~500 lines |
| 4: SCSS + Demo + Tests | 4 | 0 | 1 | 3 | ~800 lines |
| **Total** | **23** | **8** | **9** | **4** | **~2300 lines** |

---

## Dependency Graph

```
Wave 1 (Models)
  │
  ├──▶ Wave 2 (JS + Adapter)
  │      │
  │      └──▶ Wave 3 (Component Rewrite)
  │             │
  │             └──▶ Wave 4 (SCSS + Demo + Tests)
  │
  └──▶ Wave 3 (Component Rewrite)  [also directly depends on models]
```

All waves are strictly sequential. No parallelism between waves. Within Wave 3, sub-phases 3a-3d can be sequential within a single worker. Within Wave 4, FluentUI SCSS, demo, and tests can potentially run in parallel lanes if orchestrated.

---

## Risk Notes

1. **MapLibre GL JS bundle size** (~210KB gzip): Must be lazy-loaded on first map render, not at app startup. Wave 2 JS module must implement this.
2. **Large GeoJSON over SignalR** (Blazor Server): Consider adding `DataUrl` parameter for shape layers in a future wave (not in current scope -- ADR acknowledges this risk).
3. **Blazor Server reconnection**: Wave 2 adapter must handle re-initialization. The `_isInitialized` flag pattern from Editor/AllocationScheduler applies.
4. **Wave 3 scope is large**: 14 gaps in one wave. Sub-phasing (3a-3d) is recommended to keep changes reviewable.
