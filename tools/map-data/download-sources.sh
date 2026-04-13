#!/usr/bin/env bash
# download-sources.sh — Download Natural Earth + OSM data for local basemap generation.
# Usage: bash tools/map-data/download-sources.sh
# Run from the repo root (C:\Projects\Marilo or /workspaces/Marilo).
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
INPUT_DIR="${SCRIPT_DIR}/input"
mkdir -p "${INPUT_DIR}"

# ---------------------------------------------------------------------------
# 1. Natural Earth — vector data (public domain)
#    Used for low-zoom coastlines, country boundaries, lakes, land polygons.
#    ~100 MB download; unpacks to shapefiles.
# ---------------------------------------------------------------------------
NE_URL="https://naciscdn.org/naturalearth/packages/natural_earth_vector.gpkg.zip"
NE_ZIP="${INPUT_DIR}/natural_earth_vector.gpkg.zip"
NE_GPKG="${INPUT_DIR}/natural_earth_vector.gpkg"

if [ -f "${NE_GPKG}" ]; then
    echo "[skip] Natural Earth GeoPackage already exists: ${NE_GPKG}"
else
    echo "[download] Natural Earth vector GeoPackage (~100 MB)..."
    curl -L --progress-bar -o "${NE_ZIP}" "${NE_URL}"
    echo "[extract] Unzipping..."
    unzip -o -q "${NE_ZIP}" -d "${INPUT_DIR}"
    # The zip contains packages/natural_earth_vector.gpkg; move it up if nested.
    if [ -f "${INPUT_DIR}/packages/natural_earth_vector.gpkg" ]; then
        mv "${INPUT_DIR}/packages/natural_earth_vector.gpkg" "${NE_GPKG}"
        rm -rf "${INPUT_DIR}/packages"
    fi
    rm -f "${NE_ZIP}"
    echo "[done] Natural Earth GeoPackage: ${NE_GPKG}"
fi

# ---------------------------------------------------------------------------
# 2. OpenStreetMap extract — small region (.osm.pbf)
#    Default: District of Columbia (~30 MB). Change the URL for other regions.
#    Source: Geofabrik (ODbL). See https://download.geofabrik.de/
# ---------------------------------------------------------------------------
OSM_REGION="district-of-columbia"
OSM_URL="https://download.geofabrik.de/north-america/us/${OSM_REGION}-latest.osm.pbf"
OSM_FILE="${INPUT_DIR}/${OSM_REGION}-latest.osm.pbf"

if [ -f "${OSM_FILE}" ]; then
    echo "[skip] OSM extract already exists: ${OSM_FILE}"
else
    echo "[download] OSM extract: ${OSM_REGION} (~30 MB)..."
    curl -L --progress-bar -o "${OSM_FILE}" "${OSM_URL}"
    echo "[done] OSM extract: ${OSM_FILE}"
fi

echo ""
echo "=== All source data downloaded to ${INPUT_DIR} ==="
ls -lh "${INPUT_DIR}"
