#!/usr/bin/env python3
"""
Generate 100 SVG icons for the Nexus Icon Library.
All icons use a consistent 24x24 viewBox, 2px stroke, round caps/joins.
Optimized for web: currentColor, no fill by default, minimal paths.
"""

import os

ICON_DIR = "/home/user/workspace/nexus-icons/icons"

def wrap(inner, vb="0 0 24 24"):
    return f'<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="{vb}" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">{inner}</svg>'

def wrap_filled(inner, vb="0 0 24 24"):
    """For icons that need fill instead of stroke"""
    return f'<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="{vb}" fill="currentColor" stroke="none">{inner}</svg>'

def wrap_mixed(inner, vb="0 0 24 24"):
    """For icons that need both fill and stroke"""
    return f'<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="{vb}" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">{inner}</svg>'

icons = {}

# ============================================================
# UI ACTIONS (25 icons)
# ============================================================

icons["ui-actions/search"] = wrap(
    '<circle cx="11" cy="11" r="8"/><path d="m21 21-4.35-4.35"/>'
)

icons["ui-actions/plus"] = wrap(
    '<path d="M12 5v14"/><path d="M5 12h14"/>'
)

icons["ui-actions/minus"] = wrap(
    '<path d="M5 12h14"/>'
)

icons["ui-actions/x-close"] = wrap(
    '<path d="M18 6 6 18"/><path d="m6 6 12 12"/>'
)

icons["ui-actions/check"] = wrap(
    '<path d="M20 6 9 17l-5-5"/>'
)

icons["ui-actions/edit"] = wrap(
    '<path d="M17 3a2.83 2.83 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5Z"/>'
)

icons["ui-actions/trash"] = wrap(
    '<path d="M3 6h18"/><path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"/><path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"/><path d="M10 11v6"/><path d="M14 11v6"/>'
)

icons["ui-actions/copy"] = wrap(
    '<rect width="14" height="14" x="8" y="8" rx="2" ry="2"/><path d="M4 16c-1.1 0-2-.9-2-2V4c0-1.1.9-2 2-2h10c1.1 0 2 .9 2 2"/>'
)

icons["ui-actions/clipboard"] = wrap(
    '<rect width="8" height="4" x="8" y="2" rx="1" ry="1"/><path d="M16 4h2a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H6a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2h2"/>'
)

icons["ui-actions/save"] = wrap(
    '<path d="M19 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11l5 5v11a2 2 0 0 1-2 2z"/><polyline points="17 21 17 13 7 13 7 21"/><polyline points="7 3 7 8 15 8"/>'
)

icons["ui-actions/download"] = wrap(
    '<path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="7 10 12 15 17 10"/><path d="M12 15V3"/>'
)

icons["ui-actions/upload"] = wrap(
    '<path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><polyline points="17 8 12 3 7 8"/><path d="M12 3v12"/>'
)

icons["ui-actions/refresh"] = wrap(
    '<path d="M3 12a9 9 0 0 1 9-9 9.75 9.75 0 0 1 6.74 2.74L21 8"/><path d="M21 3v5h-5"/><path d="M21 12a9 9 0 0 1-9 9 9.75 9.75 0 0 1-6.74-2.74L3 16"/><path d="M3 21v-5h5"/>'
)

icons["ui-actions/undo"] = wrap(
    '<path d="M3 7v6h6"/><path d="M3 13a9 9 0 0 1 15.36-6.36L21 9"/>'
    # Simpler undo
)

icons["ui-actions/redo"] = wrap(
    '<path d="M21 7v6h-6"/><path d="M21 13a9 9 0 0 0-15.36-6.36L3 9"/>'
)

icons["ui-actions/filter"] = wrap(
    '<polygon points="22 3 2 3 10 12.46 10 19 14 21 14 12.46 22 3"/>'
)

icons["ui-actions/sort"] = wrap(
    '<path d="M11 5h10"/><path d="M11 9h7"/><path d="M11 13h4"/><path d="m3 17 3 3 3-3"/><path d="M6 18V4"/>'
)

icons["ui-actions/settings"] = wrap(
    '<circle cx="12" cy="12" r="3"/><path d="M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 0 1 0 2.83 2 2 0 0 1-2.83 0l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-2 2 2 2 0 0 1-2-2v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 0 1-2.83 0 2 2 0 0 1 0-2.83l.06-.06A1.65 1.65 0 0 0 4.68 15a1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1-2-2 2 2 0 0 1 2-2h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 0 1 0-2.83 2 2 0 0 1 2.83 0l.06.06A1.65 1.65 0 0 0 9 4.68a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 2-2 2 2 0 0 1 2 2v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 0 1 2.83 0 2 2 0 0 1 0 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 2 2 2 2 0 0 1-2 2h-.09a1.65 1.65 0 0 0-1.51 1z"/>'
)

icons["ui-actions/sliders"] = wrap(
    '<path d="M4 21v-7"/><path d="M4 10V3"/><path d="M12 21v-9"/><path d="M12 8V3"/><path d="M20 21v-5"/><path d="M20 12V3"/><path d="M2 14h4"/><path d="M10 8h4"/><path d="M18 16h4"/>'
)

icons["ui-actions/eye"] = wrap(
    '<path d="M2 12s3-7 10-7 10 7 10 7-3 7-10 7-10-7-10-7z"/><circle cx="12" cy="12" r="3"/>'
)

icons["ui-actions/eye-off"] = wrap(
    '<path d="M9.88 9.88a3 3 0 1 0 4.24 4.24"/><path d="M10.73 5.08A10.43 10.43 0 0 1 12 5c7 0 10 7 10 7a13.16 13.16 0 0 1-1.67 2.68"/><path d="M6.61 6.61A13.53 13.53 0 0 0 2 12s3 7 10 7a9.74 9.74 0 0 0 5.39-1.61"/><path d="M2 2l20 20"/>'
)

icons["ui-actions/lock"] = wrap(
    '<rect width="18" height="11" x="3" y="11" rx="2" ry="2"/><path d="M7 11V7a5 5 0 0 1 10 0v4"/>'
)

icons["ui-actions/unlock"] = wrap(
    '<rect width="18" height="11" x="3" y="11" rx="2" ry="2"/><path d="M7 11V7a5 5 0 0 1 9.9-1"/>'
)

icons["ui-actions/link"] = wrap(
    '<path d="M10 13a5 5 0 0 0 7.54.54l3-3a5 5 0 0 0-7.07-7.07l-1.72 1.71"/><path d="M14 11a5 5 0 0 0-7.54-.54l-3 3a5 5 0 0 0 7.07 7.07l1.71-1.71"/>'
)

icons["ui-actions/external-link"] = wrap(
    '<path d="M18 13v6a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h6"/><polyline points="15 3 21 3 21 9"/><path d="M10 14 21 3"/>'
)

# ============================================================
# NAVIGATION (20 icons)
# ============================================================

icons["navigation/arrow-up"] = wrap(
    '<path d="m5 12 7-7 7 7"/><path d="M12 19V5"/>'
)

icons["navigation/arrow-down"] = wrap(
    '<path d="m19 12-7 7-7-7"/><path d="M12 5v14"/>'
)

icons["navigation/arrow-left"] = wrap(
    '<path d="m12 19-7-7 7-7"/><path d="M19 12H5"/>'
)

icons["navigation/arrow-right"] = wrap(
    '<path d="m12 5 7 7-7 7"/><path d="M5 12h14"/>'
)

icons["navigation/chevron-up"] = wrap(
    '<path d="m18 15-6-6-6 6"/>'
)

icons["navigation/chevron-down"] = wrap(
    '<path d="m6 9 6 6 6-6"/>'
)

icons["navigation/chevron-left"] = wrap(
    '<path d="m15 18-6-6 6-6"/>'
)

icons["navigation/chevron-right"] = wrap(
    '<path d="m9 18 6-6-6-6"/>'
)

icons["navigation/chevrons-up"] = wrap(
    '<path d="m17 11-5-5-5 5"/><path d="m17 18-5-5-5 5"/>'
)

icons["navigation/chevrons-down"] = wrap(
    '<path d="m7 6 5 5 5-5"/><path d="m7 13 5 5 5-5"/>'
)

icons["navigation/home"] = wrap(
    '<path d="m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/>'
)

icons["navigation/menu"] = wrap(
    '<path d="M4 12h16"/><path d="M4 6h16"/><path d="M4 18h16"/>'
)

icons["navigation/more-horizontal"] = wrap(
    '<circle cx="12" cy="12" r="1"/><circle cx="19" cy="12" r="1"/><circle cx="5" cy="12" r="1"/>'
)

icons["navigation/more-vertical"] = wrap(
    '<circle cx="12" cy="12" r="1"/><circle cx="12" cy="5" r="1"/><circle cx="12" cy="19" r="1"/>'
)

icons["navigation/grid"] = wrap(
    '<rect width="7" height="7" x="3" y="3" rx="1"/><rect width="7" height="7" x="14" y="3" rx="1"/><rect width="7" height="7" x="3" y="14" rx="1"/><rect width="7" height="7" x="14" y="14" rx="1"/>'
)

icons["navigation/list"] = wrap(
    '<path d="M8 6h13"/><path d="M8 12h13"/><path d="M8 18h13"/><path d="M3 6h.01"/><path d="M3 12h.01"/><path d="M3 18h.01"/>'
)

icons["navigation/sidebar"] = wrap(
    '<rect width="18" height="18" x="3" y="3" rx="2"/><path d="M9 3v18"/>'
)

icons["navigation/maximize"] = wrap(
    '<polyline points="15 3 21 3 21 9"/><polyline points="9 21 3 21 3 15"/><path d="M21 3 14 10"/><path d="M3 21l7-7"/>'
)

icons["navigation/minimize"] = wrap(
    '<polyline points="4 14 10 14 10 20"/><polyline points="20 10 14 10 14 4"/><path d="M14 10l7-7"/><path d="M3 21l7-7"/>'
)

icons["navigation/log-out"] = wrap(
    '<path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><polyline points="16 17 21 12 16 7"/><path d="M21 12H9"/>'
)

# ============================================================
# FILES & DOCUMENTS (15 icons)
# ============================================================

icons["files/file"] = wrap(
    '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/>'
)

icons["files/file-text"] = wrap(
    '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><path d="M16 13H8"/><path d="M16 17H8"/><path d="M10 9H8"/>'
)

icons["files/file-plus"] = wrap(
    '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><path d="M12 18v-6"/><path d="M9 15h6"/>'
)

icons["files/file-code"] = wrap(
    '<path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/><path d="m10 13-2 2 2 2"/><path d="m14 17 2-2-2-2"/>'
)

icons["files/folder"] = wrap(
    '<path d="M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"/>'
)

icons["files/folder-open"] = wrap(
    '<path d="m6 14 1.5-2.9A2 2 0 0 1 9.24 10H20a2 2 0 0 1 1.94 2.5l-1.54 6a2 2 0 0 1-1.95 1.5H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h3.9a2 2 0 0 1 1.69.9l.81 1.2a2 2 0 0 0 1.67.9H18a2 2 0 0 1 2 2v2"/>'
)

icons["files/folder-plus"] = wrap(
    '<path d="M22 19a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h5l2 3h9a2 2 0 0 1 2 2z"/><path d="M12 11v6"/><path d="M9 14h6"/>'
)

icons["files/archive"] = wrap(
    '<path d="m21 8-2-2H5L3 8"/><rect width="20" height="6" x="2" y="8" rx="1"/><path d="M12 14v4"/><path d="M2 14h20"/><path d="M12 14h.01"/>'
)

icons["files/image"] = wrap(
    '<rect width="18" height="18" x="3" y="3" rx="2" ry="2"/><circle cx="8.5" cy="8.5" r="1.5"/><polyline points="21 15 16 10 5 21"/>'
)

icons["files/paperclip"] = wrap(
    '<path d="m21.44 11.05-9.19 9.19a6 6 0 0 1-8.49-8.49l9.19-9.19a4 4 0 0 1 5.66 5.66l-9.2 9.19a2 2 0 0 1-2.83-2.83l8.49-8.48"/>'
)

icons["files/cloud"] = wrap(
    '<path d="M18 10h-1.26A8 8 0 1 0 9 20h9a5 5 0 0 0 0-10z"/>'
)

icons["files/cloud-upload"] = wrap(
    '<path d="M18 10h-1.26A8 8 0 1 0 9 20h9a5 5 0 0 0 0-10z"/><polyline points="16 16 12 12 8 16"/><path d="M12 12v9"/>'
    # Simplified: cloud with up arrow
)

icons["files/cloud-download"] = wrap(
    '<path d="M18 10h-1.26A8 8 0 1 0 9 20h9a5 5 0 0 0 0-10z"/><polyline points="8 16 12 20 16 16"/><path d="M12 12v8"/>'
)

icons["files/database"] = wrap(
    '<ellipse cx="12" cy="5" rx="9" ry="3"/><path d="M21 12c0 1.66-4 3-9 3s-9-1.34-9-3"/><path d="M3 5v14c0 1.66 4 3 9 3s9-1.34 9-3V5"/>'
)

icons["files/hard-drive"] = wrap(
    '<path d="M22 12H2"/><path d="M5.45 5.11 2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11z"/><path d="M6 16h.01"/><path d="M10 16h.01"/>'
)

# ============================================================
# MEDIA & COMMUNICATION (15 icons)
# ============================================================

icons["media/play"] = wrap(
    '<polygon points="5 3 19 12 5 21 5 3"/>'
)

icons["media/pause"] = wrap(
    '<rect width="4" height="16" x="6" y="4" rx="1"/><rect width="4" height="16" x="14" y="4" rx="1"/>'
)

icons["media/stop-circle"] = wrap(
    '<circle cx="12" cy="12" r="10"/><rect width="6" height="6" x="9" y="9" rx="1"/>'
)

icons["media/skip-forward"] = wrap(
    '<polygon points="5 4 15 12 5 20 5 4"/><path d="M19 5v14"/>'
)

icons["media/skip-back"] = wrap(
    '<polygon points="19 20 9 12 19 4 19 20"/><path d="M5 19V5"/>'
)

icons["media/volume"] = wrap(
    '<polygon points="11 5 6 9 2 9 2 15 6 15 11 19 11 5"/><path d="M15.54 8.46a5 5 0 0 1 0 7.07"/><path d="M19.07 4.93a10 10 0 0 1 0 14.14"/>'
)

icons["media/volume-mute"] = wrap(
    '<polygon points="11 5 6 9 2 9 2 15 6 15 11 19 11 5"/><path d="M23 9l-6 6"/><path d="M17 9l6 6"/>'
)

icons["media/mic"] = wrap(
    '<path d="M12 2a3 3 0 0 0-3 3v7a3 3 0 0 0 6 0V5a3 3 0 0 0-3-3z"/><path d="M19 10v2a7 7 0 0 1-14 0v-2"/><path d="M12 19v3"/>'
)

icons["media/camera"] = wrap(
    '<path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z"/><circle cx="12" cy="13" r="4"/>'
)

icons["media/mail"] = wrap(
    '<rect width="20" height="16" x="2" y="4" rx="2"/><path d="m22 7-8.97 5.7a1.94 1.94 0 0 1-2.06 0L2 7"/>'
)

icons["media/send"] = wrap(
    '<path d="m22 2-7 20-4-9-9-4z"/><path d="m22 2-11 11"/>'
)

icons["media/message-circle"] = wrap(
    '<path d="M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z"/>'
)

icons["media/phone"] = wrap(
    '<path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"/>'
)

icons["media/bell"] = wrap(
    '<path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"/><path d="M13.73 21a2 2 0 0 1-3.46 0"/>'
)

icons["media/share"] = wrap(
    '<circle cx="18" cy="5" r="3"/><circle cx="6" cy="12" r="3"/><circle cx="18" cy="19" r="3"/><path d="m8.59 13.51 6.83 3.98"/><path d="m15.41 6.51-6.82 3.98"/>'
)

# ============================================================
# STATUS & INDICATORS (15 icons)
# ============================================================

icons["status/check-circle"] = wrap(
    '<path d="M22 11.08V12a10 10 0 1 1-5.93-9.14"/><polyline points="22 4 12 14.01 9 11.01"/>'
)

icons["status/x-circle"] = wrap(
    '<circle cx="12" cy="12" r="10"/><path d="m15 9-6 6"/><path d="m9 9 6 6"/>'
)

icons["status/alert-circle"] = wrap(
    '<circle cx="12" cy="12" r="10"/><path d="M12 8v4"/><path d="M12 16h.01"/>'
)

icons["status/alert-triangle"] = wrap(
    '<path d="m21.73 18-8-14a2 2 0 0 0-3.48 0l-8 14A2 2 0 0 0 4 21h16a2 2 0 0 0 1.73-3z"/><path d="M12 9v4"/><path d="M12 17h.01"/>'
)

icons["status/info"] = wrap(
    '<circle cx="12" cy="12" r="10"/><path d="M12 16v-4"/><path d="M12 8h.01"/>'
)

icons["status/help-circle"] = wrap(
    '<circle cx="12" cy="12" r="10"/><path d="M9.09 9a3 3 0 0 1 5.83 1c0 2-3 3-3 3"/><path d="M12 17h.01"/>'
)

icons["status/loader"] = wrap(
    '<path d="M12 2v4"/><path d="M12 18v4"/><path d="m4.93 4.93 2.83 2.83"/><path d="m16.24 16.24 2.83 2.83"/><path d="M2 12h4"/><path d="M18 12h4"/><path d="m4.93 19.07 2.83-2.83"/><path d="m16.24 7.76 2.83-2.83"/>'
)

icons["status/clock"] = wrap(
    '<circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/>'
)

icons["status/calendar"] = wrap(
    '<rect width="18" height="18" x="3" y="4" rx="2" ry="2"/><path d="M16 2v4"/><path d="M8 2v4"/><path d="M3 10h18"/>'
)

icons["status/star"] = wrap(
    '<polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"/>'
)

icons["status/heart"] = wrap(
    '<path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/>'
)

icons["status/thumbs-up"] = wrap(
    '<path d="M7 22H4a2 2 0 0 1-2-2v-7a2 2 0 0 1 2-2h3"/><path d="M14 9V5a3 3 0 0 0-3-3l-4 9v11h11.28a2 2 0 0 0 2-1.7l1.38-9a2 2 0 0 0-2-2.3H14z"/>'
)

icons["status/flag"] = wrap(
    '<path d="M4 15s1-1 4-1 5 2 8 2 4-1 4-1V3s-1 1-4 1-5-2-8-2-4 1-4 1z"/><path d="M4 22v-7"/>'
)

icons["status/badge"] = wrap(
    '<path d="M3.85 8.62a4 4 0 0 1 4.78-4.77 4 4 0 0 1 6.74 0 4 4 0 0 1 4.78 4.78 4 4 0 0 1 0 6.74 4 4 0 0 1-4.77 4.78 4 4 0 0 1-6.75 0 4 4 0 0 1-4.78-4.77 4 4 0 0 1 0-6.76z"/><path d="m9 12 2 2 4-4"/>'
)

icons["status/zap"] = wrap(
    '<polygon points="13 2 3 14 12 14 11 22 21 10 12 10 13 2"/>'
)

# ============================================================
# SYSTEM / MISC (10 icons)
# ============================================================

icons["system/user"] = wrap(
    '<path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/>'
)

icons["system/users"] = wrap(
    '<path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/>'
)

icons["system/globe"] = wrap(
    '<circle cx="12" cy="12" r="10"/><path d="M2 12h20"/><path d="M12 2a15.3 15.3 0 0 1 4 10 15.3 15.3 0 0 1-4 10 15.3 15.3 0 0 1-4-10 15.3 15.3 0 0 1 4-10z"/>'
)

icons["system/terminal"] = wrap(
    '<polyline points="4 17 10 11 4 5"/><path d="M12 19h8"/>'
)

icons["system/code"] = wrap(
    '<polyline points="16 18 22 12 16 6"/><polyline points="8 6 2 12 8 18"/>'
)

icons["system/cpu"] = wrap(
    '<rect width="16" height="16" x="4" y="4" rx="2"/><rect width="6" height="6" x="9" y="9" rx="1"/><path d="M15 2v2"/><path d="M15 20v2"/><path d="M9 2v2"/><path d="M9 20v2"/><path d="M2 9h2"/><path d="M2 15h2"/><path d="M20 9h2"/><path d="M20 15h2"/>'
)

icons["system/wifi"] = wrap(
    '<path d="M5 12.55a11 11 0 0 1 14.08 0"/><path d="M1.42 9a16 16 0 0 1 21.16 0"/><path d="M8.53 16.11a6 6 0 0 1 6.95 0"/><circle cx="12" cy="20" r="1"/>'
    # Simplified but distinct
)

icons["system/bluetooth"] = wrap(
    '<polyline points="6.5 6.5 17.5 17.5 12 23 12 1 17.5 6.5 6.5 17.5"/>'
)

icons["system/power"] = wrap(
    '<path d="M18.36 6.64a9 9 0 1 1-12.73 0"/><path d="M12 2v10"/>'
)

icons["system/shield"] = wrap(
    '<path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/>'
)


# ============================================================
# Write all icons to disk
# ============================================================

count = 0
for path, svg in icons.items():
    full_path = os.path.join(ICON_DIR, f"{path}.svg")
    os.makedirs(os.path.dirname(full_path), exist_ok=True)
    with open(full_path, "w") as f:
        f.write(svg)
    count += 1

print(f"Generated {count} icons")

# Print category counts
from collections import Counter
cats = Counter(k.split("/")[0] for k in icons)
for cat, c in sorted(cats.items()):
    print(f"  {cat}: {c}")
