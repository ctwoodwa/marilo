// marilo-map.js — MapLibre GL JS adapter for MariloMap component.
// Loaded as an ESM module via JS interop.
// Depends on maplibregl and pmtiles being available globally (loaded via CDN in App.razor).

const maps = new Map();

/**
 * Initialize a MapLibre map in the given container element.
 * @param {string} containerId - DOM id of the map container div.
 * @param {object} options - Map configuration.
 * @param {string} options.styleUrl - URL to a MapLibre style JSON.
 * @param {number} options.lng - Center longitude.
 * @param {number} options.lat - Center latitude.
 * @param {number} options.zoom - Initial zoom level.
 * @param {boolean} options.zoomable - Whether scroll zoom and zoom controls are enabled.
 * @param {boolean} options.pannable - Whether drag panning is enabled.
 * @param {Array} options.markers - Array of { title, latitude, longitude }.
 * @param {object} dotNetRef - DotNetObjectReference for callbacks.
 */
export function initMap(containerId, options, dotNetRef) {
    // Register PMTiles protocol if not already registered.
    if (typeof pmtiles !== "undefined" && !initMap._pmtilesRegistered) {
        const protocol = new pmtiles.Protocol();
        maplibregl.addProtocol("pmtiles", protocol.tile);
        initMap._pmtilesRegistered = true;
    }

    // Dispose existing map on the same container (hot-reload safety).
    disposeMap(containerId);

    const map = new maplibregl.Map({
        container: containerId,
        style: options.styleUrl || {
            version: 8,
            sources: {},
            layers: [{ id: "background", type: "background", paint: { "background-color": "#f0f0f0" } }]
        },
        center: [options.lng, options.lat],
        zoom: options.zoom,
        scrollZoom: options.zoomable !== false,
        dragPan: options.pannable !== false,
        attributionControl: true
    });

    if (options.zoomable !== false) {
        map.addControl(new maplibregl.NavigationControl(), "top-right");
    }

    // Add markers after the map loads.
    const markerInstances = [];
    map.on("load", () => {
        if (options.markers && options.markers.length > 0) {
            for (const m of options.markers) {
                const el = createMarkerElement();
                const marker = new maplibregl.Marker({ element: el })
                    .setLngLat([m.longitude, m.latitude])
                    .addTo(map);

                if (m.title) {
                    marker.setPopup(new maplibregl.Popup({ offset: 25 }).setText(m.title));
                }

                el.addEventListener("click", () => {
                    if (dotNetRef) {
                        dotNetRef.invokeMethodAsync("OnMarkerClickFromJs", m.title || "", m.latitude, m.longitude);
                    }
                });

                markerInstances.push(marker);
            }
        }
    });

    maps.set(containerId, { map, markers: markerInstances });
}

/**
 * Update markers on an existing map.
 * @param {string} containerId
 * @param {Array} markers
 * @param {object} dotNetRef
 */
export function updateMarkers(containerId, markers, dotNetRef) {
    const entry = maps.get(containerId);
    if (!entry) return;

    // Remove existing markers.
    for (const m of entry.markers) {
        m.remove();
    }
    entry.markers = [];

    // Add new markers.
    for (const m of markers) {
        const el = createMarkerElement();
        const marker = new maplibregl.Marker({ element: el })
            .setLngLat([m.longitude, m.latitude])
            .addTo(entry.map);

        if (m.title) {
            marker.setPopup(new maplibregl.Popup({ offset: 25 }).setText(m.title));
        }

        el.addEventListener("click", () => {
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync("OnMarkerClickFromJs", m.title || "", m.latitude, m.longitude);
            }
        });

        entry.markers.push(marker);
    }
}

/**
 * Fly the map to a new center and zoom.
 * @param {string} containerId
 * @param {number} lng
 * @param {number} lat
 * @param {number} zoom
 */
export function flyTo(containerId, lng, lat, zoom) {
    const entry = maps.get(containerId);
    if (!entry) return;
    entry.map.flyTo({ center: [lng, lat], zoom: zoom, essential: true });
}

/**
 * Dispose a map and free resources.
 * @param {string} containerId
 */
export function disposeMap(containerId) {
    const entry = maps.get(containerId);
    if (!entry) return;
    for (const m of entry.markers) {
        m.remove();
    }
    entry.map.remove();
    maps.delete(containerId);
}

function createMarkerElement() {
    const svgNs = "http://www.w3.org/2000/svg";
    const svg = document.createElementNS(svgNs, "svg");
    svg.setAttribute("width", "24");
    svg.setAttribute("height", "32");
    svg.setAttribute("viewBox", "0 0 24 32");

    const path = document.createElementNS(svgNs, "path");
    path.setAttribute("d", "M12 0C5.4 0 0 5.4 0 12c0 9 12 20 12 20s12-11 12-20C24 5.4 18.6 0 12 0z");
    path.setAttribute("fill", "#d32f2f");
    svg.appendChild(path);

    const circle = document.createElementNS(svgNs, "circle");
    circle.setAttribute("cx", "12");
    circle.setAttribute("cy", "12");
    circle.setAttribute("r", "5");
    circle.setAttribute("fill", "white");
    svg.appendChild(circle);

    const el = document.createElement("div");
    el.style.cursor = "pointer";
    el.appendChild(svg);
    return el;
}
