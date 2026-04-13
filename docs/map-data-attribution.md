# Map Data — Attribution & Licensing

This file documents the data sources, tools, and licenses used by the Marilo local basemap pipeline. Any application or demo that serves these tiles must include the required attributions.

---

## Data Sources

### Natural Earth

| Field | Value |
|-------|-------|
| **Source** | [Natural Earth](https://www.naturalearthdata.com/) |
| **License** | **Public domain** — free for any use without restriction |
| **Data used** | `natural_earth_vector.gpkg` (all scales) |
| **Attribution required?** | No (public domain), but credit is appreciated |
| **Suggested credit** | "Made with Natural Earth" |
| **Link** | <https://www.naturalearthdata.com/about/terms-of-use/> |

### OpenStreetMap

| Field | Value |
|-------|-------|
| **Source** | [OpenStreetMap](https://www.openstreetmap.org/) via [Geofabrik](https://download.geofabrik.de/) |
| **License** | **Open Data Commons Open Database License (ODbL) v1.0** |
| **Data used** | Regional `.osm.pbf` extract (default: District of Columbia) |
| **Attribution required?** | **Yes** — must appear in any public-facing use |
| **Required text** | "&copy; OpenStreetMap contributors" |
| **License URL** | <https://www.openstreetmap.org/copyright> |
| **ODbL full text** | <https://opendatacommons.org/licenses/odbl/1-0/> |

**ODbL key obligations:**

- **Attribution:** Credit OpenStreetMap contributors wherever tiles are displayed.
- **Share-alike:** If you create a derivative database, it must also be ODbL.
- **Tile rendering is not a derivative database.** Rendering tiles from OSM data and displaying them does not trigger share-alike for your application code.

### Geofabrik Extracts

| Field | Value |
|-------|-------|
| **Source** | [Geofabrik GmbH](https://www.geofabrik.de/) |
| **License** | Data is OSM (ODbL); Geofabrik's download service is free for reasonable use |
| **Link** | <https://download.geofabrik.de/> |

---

## Tools

### Planetiler

| Field | Value |
|-------|-------|
| **Source** | [onthegomap/planetiler](https://github.com/onthegomap/planetiler) |
| **License** | **Apache License 2.0** |
| **Used for** | Generating PMTiles vector tiles from OSM + Natural Earth data |
| **Container image** | `ghcr.io/onthegomap/planetiler:latest` |
| **Link** | <https://github.com/onthegomap/planetiler/blob/main/LICENSE> |

### PMTiles

| Field | Value |
|-------|-------|
| **Source** | [protomaps/PMTiles](https://github.com/protomaps/PMTiles) |
| **License** | **BSD 3-Clause** |
| **Used for** | Tile archive format; JS protocol adapter for MapLibre |
| **Link** | <https://github.com/protomaps/PMTiles/blob/main/LICENSE> |

### MapLibre GL JS

| Field | Value |
|-------|-------|
| **Source** | [maplibre/maplibre-gl-js](https://github.com/maplibre/maplibre-gl-js) |
| **License** | **BSD 3-Clause** |
| **Used for** | Client-side vector tile rendering (future integration) |
| **Link** | <https://github.com/maplibre/maplibre-gl-js/blob/main/LICENSE.txt> |

---

## Required Attribution for the Marilo Demo

When displaying the local basemap, the map UI must show:

```
© OpenStreetMap contributors | Natural Earth
```

The MapLibre style JSON (`basemap-local.json`) includes `attribution` fields in each source definition. MapLibre GL JS renders these automatically in the bottom-right attribution control.

---

## US Government Data (if used in future)

If USGS, Census TIGER, or other US federal data is added:

- US government works are in the **public domain** (17 U.S.C. 105).
- No attribution is legally required, but credit is standard practice.
- Suggested: "Data from [agency name]."
