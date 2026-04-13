#!/usr/bin/env bash
# generate-tiles.sh — Run Planetiler via Podman to produce PMTiles from local data.
# Usage: bash tools/map-data/generate-tiles.sh
# Run from the repo root.
#
# Prerequisites:
#   - Podman installed and running (podman --version)
#   - Source data downloaded via download-sources.sh
#
# Planetiler image: ghcr.io/onthegomap/planetiler:latest
# Planetiler docs:  https://github.com/onthegomap/planetiler
set -euo pipefail

# Prevent Git Bash (MSYS2) from mangling Unix paths passed to Podman.
export MSYS_NO_PATHCONV=1

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
INPUT_DIR="${SCRIPT_DIR}/input"
OUTPUT_DIR="${SCRIPT_DIR}/output"
mkdir -p "${OUTPUT_DIR}"

# Paths inside the container (bind-mounted)
CONTAINER_INPUT="/data/input"
CONTAINER_OUTPUT="/data/output"

PLANETILER_IMAGE="ghcr.io/onthegomap/planetiler:latest"

# JVM heap — must fit within the Podman VM memory (check: podman machine inspect).
# The default Podman VM has 2 GB RAM. If you see OOM errors, increase VM memory:
#   podman machine stop && podman machine set --memory 4096 && podman machine start
JAVA_HEAP="1g"

# Limit threads to reduce per-thread memory pressure inside the container.
THREADS="2"

# ---------------------------------------------------------------------------
# Validate source data exists
# ---------------------------------------------------------------------------
OSM_FILE=$(find "${INPUT_DIR}" -name "*.osm.pbf" | head -1)
NE_FILE="${INPUT_DIR}/natural_earth_vector.gpkg"

if [ -z "${OSM_FILE}" ]; then
    echo "[error] No .osm.pbf file found in ${INPUT_DIR}."
    echo "        Run download-sources.sh first."
    exit 1
fi

OSM_BASENAME=$(basename "${OSM_FILE}")
echo "[info] OSM extract:     ${OSM_BASENAME}"
echo "[info] Natural Earth:   $([ -f "${NE_FILE}" ] && echo 'found' || echo 'NOT FOUND — Planetiler will download its own copy')"
echo "[info] Planetiler image: ${PLANETILER_IMAGE}"
echo ""

# ---------------------------------------------------------------------------
# Stage A: Natural Earth-only low-zoom globe tiles (z0–z6)
# ---------------------------------------------------------------------------
NE_OUTPUT="${OUTPUT_DIR}/natural-earth.pmtiles"

echo "=== Stage A: Natural Earth tiles (z0–z6) ==="
podman run --rm \
    -e "JAVA_TOOL_OPTIONS=-Xmx${JAVA_HEAP}" \
    --memory=1500m \
    -v "${INPUT_DIR}:${CONTAINER_INPUT}:ro" \
    -v "${OUTPUT_DIR}:${CONTAINER_OUTPUT}:rw" \
    "${PLANETILER_IMAGE}" \
    --download \
    --osm-path="${CONTAINER_INPUT}/${OSM_BASENAME}" \
    --natural-earth-path="${CONTAINER_INPUT}/natural_earth_vector.gpkg" \
    --output="${CONTAINER_OUTPUT}/natural-earth.pmtiles" \
    --maxzoom=6 \
    --threads="${THREADS}" \
    --nodemap-type=sortedtable \
    --storage=mmap \
    --force

echo "[done] Natural Earth tiles: ${NE_OUTPUT}"
echo ""

# ---------------------------------------------------------------------------
# Stage B: OSM regional extract — street-level tiles (z0–z14)
# ---------------------------------------------------------------------------
OSM_OUTPUT="${OUTPUT_DIR}/osm-region.pmtiles"

echo "=== Stage B: OSM regional tiles (z0–z14) ==="
podman run --rm \
    -e "JAVA_TOOL_OPTIONS=-Xmx${JAVA_HEAP}" \
    --memory=1500m \
    -v "${INPUT_DIR}:${CONTAINER_INPUT}:ro" \
    -v "${OUTPUT_DIR}:${CONTAINER_OUTPUT}:rw" \
    "${PLANETILER_IMAGE}" \
    --download \
    --osm-path="${CONTAINER_INPUT}/${OSM_BASENAME}" \
    --natural-earth-path="${CONTAINER_INPUT}/natural_earth_vector.gpkg" \
    --output="${CONTAINER_OUTPUT}/osm-region.pmtiles" \
    --maxzoom=14 \
    --threads="${THREADS}" \
    --nodemap-type=sortedtable \
    --storage=mmap \
    --force

echo "[done] OSM regional tiles: ${OSM_OUTPUT}"
echo ""

# ---------------------------------------------------------------------------
# Stage C: Copy to demo wwwroot for local serving
# ---------------------------------------------------------------------------
DEMO_MAP_DATA="${SCRIPT_DIR}/../../samples/Marilo.Demo/wwwroot/map-data"
mkdir -p "${DEMO_MAP_DATA}"

echo "=== Stage C: Copying PMTiles to demo wwwroot ==="
cp "${NE_OUTPUT}" "${DEMO_MAP_DATA}/natural-earth.pmtiles"
cp "${OSM_OUTPUT}" "${DEMO_MAP_DATA}/osm-region.pmtiles"
echo "[done] PMTiles copied to: ${DEMO_MAP_DATA}"
ls -lh "${DEMO_MAP_DATA}"

echo ""
echo "=== Pipeline complete ==="
echo "Tile files are ready for local serving by the Blazor demo app."
echo "See docs/map-local-basemap-setup.md for MapLibre integration details."
