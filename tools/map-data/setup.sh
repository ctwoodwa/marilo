#!/usr/bin/env bash
# setup.sh — One-command basemap pipeline: download sources, generate tiles, copy to demo.
# Usage: bash tools/map-data/setup.sh
# Run from the repo root.
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "============================================"
echo "  Marilo Local Basemap Pipeline"
echo "============================================"
echo ""

echo "--- Step 1/2: Downloading source data ---"
bash "${SCRIPT_DIR}/download-sources.sh"
echo ""

echo "--- Step 2/2: Generating tiles via Planetiler (Podman) ---"
bash "${SCRIPT_DIR}/generate-tiles.sh"
echo ""

echo "============================================"
echo "  Setup complete!"
echo "  Run the Marilo demo app to serve tiles."
echo "============================================"
