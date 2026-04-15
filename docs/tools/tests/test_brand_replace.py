import sys
import os
sys.path.insert(0, os.path.join(os.path.dirname(__file__), '..'))

import unittest
from brand_replace import brand_replace

class TestBrandReplace(unittest.TestCase):

    def test_lowercase_telerik(self):
        self.assertEqual(brand_replace("tags: telerik,blazor"), "tags: marilo,blazor")

    def test_capitalized_telerik(self):
        self.assertEqual(brand_replace("Telerik UI for Blazor"), "Marilo UI for Blazor")

    def test_component_tag(self):
        self.assertEqual(brand_replace("<TelerikChart>"), "<MariloChart>")
        self.assertEqual(brand_replace("</TelerikGrid>"), "</MariloGrid>")

    def test_slug_namespace(self):
        self.assertEqual(
            brand_replace("slug:Telerik.Blazor.Components.ChartSeries"),
            "slug:Marilo.Blazor.Components.ChartSeries"
        )

    def test_slug_namespace_lowercase(self):
        self.assertEqual(
            brand_replace("slug:telerik.blazor.components.sankeylegend"),
            "slug:marilo.blazor.components.sankeylegend"
        )

    def test_demo_url(self):
        self.assertEqual(
            brand_replace("https://demos.telerik.com/blazor-ui/chart/overview"),
            "https://demos.marilo.com/blazor-ui/chart/overview"
        )

    def test_www_url(self):
        self.assertEqual(
            brand_replace("https://www.telerik.com/blazor-ui"),
            "https://www.marilo.com/blazor-ui"
        )

    def test_preserves_non_telerik_content(self):
        self.assertEqual(
            brand_replace("The `<MariloChart>` component"),
            "The `<MariloChart>` component"
        )

    def test_template_line_unchanged(self):
        # Template include lines should not be altered (no telerik in them)
        line = "@[template](/_contentTemplates/common/parameters-table-styles.md#table-layout)"
        self.assertEqual(brand_replace(line), line)

    def test_all_caps_telerik(self):
        self.assertEqual(brand_replace("TELERIK UI"), "MARILO UI")

    def test_idempotent(self):
        inputs = [
            "tags: telerik,blazor",
            "Telerik UI for Blazor",
            "TELERIK",
            "https://demos.telerik.com/blazor-ui/chart/overview",
            "slug:Telerik.Blazor.Components.ChartSeries",
        ]
        for text in inputs:
            with self.subTest(text=text):
                once = brand_replace(text)
                self.assertEqual(brand_replace(once), once)

if __name__ == '__main__':
    unittest.main()
