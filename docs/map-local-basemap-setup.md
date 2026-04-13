# Local Basemap Pipeline — Setup Guide

This guide documents how to build and serve a fully local, open-source basemap for the Marilo demo site. No external tile APIs or tokens are required at runtime.

## Overview

| Stage | Data source | Output | Zoom levels |
|-------|------------|--------|-------------|
| A | Natural Earth (public domain) | `natural-earth.pmtiles` | z0–z6 |
| B | OpenStreetMap extract via Geofabrik (ODbL) | `osm-region.pmtiles` | z0–z14 |
| C | — | Files copied to `wwwroot/map-data/` | — |

Tiles are generated locally with [Planetiler](https://github.com/onthegomap/planetiler) (Apache-2.0) running inside a Podman container. Output format is [PMTiles](https://github.com/protomaps/PMTiles), which can be served as a static file and consumed by MapLibre GL JS via the `pmtiles` protocol adapter.

## Prerequisites

| Tool | Version | Notes |
|------|---------|-------|
| **Podman** | 5.x+ | `podman --version` — used to run Planetiler container |
| **curl** | any | Downloading source data |
| **unzip** | any | Extracting Natural Earth archive |
| **bash** | 4+ | Scripts use `set -euo pipefail` |

> **Java is NOT required.** Planetiler runs inside the container image `ghcr.io/onthegomap/planetiler:latest`, which bundles its own JDK.

### First-time Podman setup (Windows)

If Podman's machine isn't initialized yet:

```bash
podman machine init
podman machine start
```

## Data Sources

### Natural Earth

- **URL:** <https://naciscdn.org/naturalearth/packages/natural_earth_vector.gpkg.zip>
- **License:** Public domain
- **Size:** ~100 MB (compressed)
- **Content:** Coastlines, country/state boundaries, lakes, land polygons — suitable for z0–z6 globe/world views.

### OpenStreetMap (Geofabrik extract)

- **URL:** <https://download.geofabrik.de/north-america/us/district-of-columbia-latest.osm.pbf>
- **License:** ODbL — see [map-data-attribution.md](map-data-attribution.md)
- **Size:** ~30 MB
- **Content:** Full street-level data for Washington, D.C. — roads, buildings, POIs, land use.

To use a different region, edit `OSM_REGION` and `OSM_URL` in `tools/map-data/download-sources.sh`. Browse regions at <https://download.geofabrik.de/>.

## Folder Layout

```
tools/map-data/
├── download-sources.sh     # Downloads Natural Earth + OSM extract
├── generate-tiles.sh       # Runs Planetiler via Podman, copies to wwwroot
├── setup.sh                # One-command wrapper (download + generate)
├── input/                  # (git-ignored) Downloaded source data
│   ├── natural_earth_vector.gpkg
│   └── district-of-columbia-latest.osm.pbf
└── output/                 # (git-ignored) Generated PMTiles
    ├── natural-earth.pmtiles
    └── osm-region.pmtiles

samples/Marilo.Demo/wwwroot/
├── map-data/               # (PMTiles git-ignored) Served as static files
│   ├── natural-earth.pmtiles
│   └── osm-region.pmtiles
└── map-styles/
    └── basemap-local.json  # MapLibre style JSON referencing local tiles
```

## Running the Pipeline

### One-command setup

From the repo root:

```bash
bash tools/map-data/setup.sh
```

This runs both stages sequentially: download, then generate + copy.

### Step-by-step

```bash
# 1. Download source data (~130 MB total)
bash tools/map-data/download-sources.sh

# 2. Generate tiles and copy to demo wwwroot
bash tools/map-data/generate-tiles.sh
```

### What Planetiler does

The `generate-tiles.sh` script runs two Podman invocations. Key flags:

- **`--nodemap-type=sortedtable`** — most memory-efficient node location storage; uses disk-backed sorting.
- **`--storage=mmap`** — temporary data uses memory-mapped files instead of JVM heap.
- **`--threads=2`** — limits parallelism to reduce per-thread memory pressure.
- **`--download`** — lets Planetiler fetch supplementary data (water polygons, lake centerlines) inside the container.
- **`-Xmx1g`** — JVM heap limit; 1 GB is sufficient for small regional extracts with `sortedtable`.

```bash
# Stage A: Natural Earth only, z0–z6
podman run --rm \
    -e "JAVA_TOOL_OPTIONS=-Xmx1g" --memory=1500m \
    -v "$PWD/tools/map-data/input:/data/input:ro" \
    -v "$PWD/tools/map-data/output:/data/output:rw" \
    ghcr.io/onthegomap/planetiler:latest \
    --download \
    --osm-path=/data/input/district-of-columbia-latest.osm.pbf \
    --natural-earth-path=/data/input/natural_earth_vector.gpkg \
    --output=/data/output/natural-earth.pmtiles \
    --maxzoom=6 --threads=2 --nodemap-type=sortedtable --storage=mmap --force

# Stage B: OSM regional extract, z0–z14 (~60 seconds for DC)
podman run --rm \
    -e "JAVA_TOOL_OPTIONS=-Xmx1g" --memory=1500m \
    -v "$PWD/tools/map-data/input:/data/input:ro" \
    -v "$PWD/tools/map-data/output:/data/output:rw" \
    ghcr.io/onthegomap/planetiler:latest \
    --download \
    --osm-path=/data/input/district-of-columbia-latest.osm.pbf \
    --natural-earth-path=/data/input/natural_earth_vector.gpkg \
    --output=/data/output/osm-region.pmtiles \
    --maxzoom=14 --threads=2 --nodemap-type=sortedtable --storage=mmap --force
```

### Podman VM memory

The Podman VM defaults to 2 GB RAM, which is too little for Planetiler. Ensure at least 4 GB:

```bash
podman machine stop
podman machine set --memory 4096
podman machine start
```

### Git Bash path mangling

On Windows with Git Bash, `MSYS_NO_PATHCONV=1` is set in the script to prevent MSYS2 from converting Unix container paths (like `/data/input`) into Windows paths. This is handled automatically — no action needed.

## Demo App Integration

### MapLibre style

The style file at `wwwroot/map-styles/basemap-local.json` defines two vector tile sources:

- `natural-earth` — low-zoom coastlines and boundaries (z0–z6)
- `osm-region` — street-level detail (z0–z14)

Both use the `pmtiles://` protocol prefix, which MapLibre resolves via the [pmtiles JS adapter](https://github.com/protomaps/PMTiles/tree/main/js).

### Wiring into MariloMap (future)

When the MariloMap component gains MapLibre integration, point it at the local style:

```razor
<MariloMap StyleUrl="/map-styles/basemap-local.json"
           Center="new MapCenter { Latitude = 38.9072, Longitude = -77.0369 }"
           Zoom="11"
           Height="500px" />
```

The Blazor static-file middleware serves `wwwroot/map-data/*.pmtiles` and `wwwroot/map-styles/*.json` automatically — no extra server configuration needed.

### PMTiles protocol setup (in maplibre-adapter.js)

The future JS adapter will need to register the PMTiles protocol before creating the map:

```js
import { Protocol } from 'pmtiles';

const protocol = new Protocol();
maplibregl.addProtocol('pmtiles', protocol.tile);

const map = new maplibregl.Map({
    container: containerEl,
    style: '/map-styles/basemap-local.json',
    center: [lng, lat],
    zoom: zoom
});
```

## Known Limitations

1. **Single region only.** The default pipeline covers Washington, D.C. For broader coverage, add more Geofabrik extracts or use a larger region (e.g., a full US state).
2. **No globe-optimized low-zoom raster.** Natural Earth vector tiles work well for flat maps at z0–z6 but lack the rich shading of raster hillshade tiles. For globe demos, consider adding a raster hillshade source later.
3. **Glyphs served externally.** The style JSON currently points to `demotiles.maplibre.org` for font glyphs. For fully offline use, self-host glyph PBFs (see [maplibre/demotiles](https://github.com/maplibre/demotiles)).
4. **No sprite icons.** The style does not reference a sprite sheet. Add one if POI icons or shields are needed.
5. **PMTiles file size.** The DC extract produces small files (~10–30 MB). Larger regions (full US state) will produce much larger files — adjust `--maxzoom` accordingly.

## Next Steps

- [ ] Integrate PMTiles protocol adapter into `maplibre-adapter.js`
- [ ] Add more regions or allow region selection via script argument
- [ ] Self-host glyph PBFs for fully offline demos
- [ ] Add raster hillshade source for globe/terrain views
- [ ] Create a Marilo-themed style (brand colors, typography) instead of the basic cartographic style
- [ ] Consider a `dotnet tool` or npm script wrapper for cross-platform convenience

## See Also

- [Map component specs](component-specs/map/overview.md)
- [Tile layer spec](component-specs/map/layers/tile.md)
- [Architecture decision — tile engine](component-specs/map/architecture-decision-tile-engine.md)
- [Data attribution](map-data-attribution.md)
