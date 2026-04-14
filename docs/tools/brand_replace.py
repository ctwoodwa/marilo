"""
Shared brand-replacement utility for Marilo doc migration scripts.
Applies all telerik→marilo transformations in the correct order.
"""

# Order matters: most specific patterns first to avoid partial matches.
REPLACEMENTS = [
    # Slug namespaces (must precede generic Telerik→Marilo)
    ("slug:Telerik.Blazor.Components.", "slug:Marilo.Blazor.Components."),
    ("slug:telerik.blazor.components.", "slug:marilo.blazor.components."),
    # URLs (most specific subdomains first)
    ("demos.telerik.com", "demos.marilo.com"),
    ("www.telerik.com/blazor-ui", "www.marilo.com/blazor-ui"),
    ("www.telerik.com", "www.marilo.com"),
    ("telerik.com", "marilo.com"),
    # Brand name — capitalized before lowercase to avoid double-replacing
    ("Telerik", "Marilo"),
    ("telerik", "marilo"),
    ("TELERIK", "MARILO"),
]


def brand_replace(text: str) -> str:
    """Apply all brand replacements to a string. Safe to call multiple times."""
    for old, new in REPLACEMENTS:
        text = text.replace(old, new)
    return text
