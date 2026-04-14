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
    ("telerik.com", "marilo.com"),  # intentional substring — no false positives expected in doc sources
    # Brand name: mixed-case first to prevent the generic lowercase rule
    # from double-firing on output of the capitalised rule.
    ("Telerik", "Marilo"),
    ("telerik", "marilo"),
    # All-caps variant: independent of the above (no ordering dependency).
    ("TELERIK", "MARILO"),
]


def brand_replace(text: str) -> str:
    """Apply all brand replacements to a string. Safe to call multiple times."""
    for old, new in REPLACEMENTS:
        text = text.replace(old, new)
    return text
