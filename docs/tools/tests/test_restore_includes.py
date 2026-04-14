import sys
import os
sys.path.insert(0, os.path.join(os.path.dirname(__file__), '..'))

import unittest
from restore_includes import restore_lines

class TestRestoreLines(unittest.TestCase):

    def test_restores_single_template_line(self):
        """Template line deleted from dest is re-inserted after its anchor."""
        src = [
            "Some content\n",
            "@[template](/_contentTemplates/common/foo.md#bar)\n",
            "More content\n",
        ]
        dest = [
            "Some content\n",
            "More content\n",
        ]
        result, count = restore_lines(src, dest)
        self.assertEqual(count, 1)
        self.assertEqual(result, [
            "Some content\n",
            "@[template](/_contentTemplates/common/foo.md#bar)\n",
            "More content\n",
        ])

    def test_restores_template_before_table(self):
        """Template line that appears before a table is placed correctly."""
        src = [
            "Parameters:\n",
            "@[template](/_contentTemplates/common/parameters-table-styles.md#table-layout)\n",
            "| Name | Type |\n",
        ]
        dest = [
            "Parameters:\n",
            "| Name | Type |\n",
        ]
        result, count = restore_lines(src, dest)
        self.assertEqual(count, 1)
        self.assertEqual(result[1], "@[template](/_contentTemplates/common/parameters-table-styles.md#table-layout)\n")

    def test_restores_multiple_template_lines(self):
        """Multiple stripped template lines are all restored."""
        src = [
            "Intro\n",
            "@[template](/_contentTemplates/common/foo.md#a)\n",
            "Middle\n",
            "@[template](/_contentTemplates/common/bar.md#b)\n",
            "End\n",
        ]
        dest = [
            "Intro\n",
            "Middle\n",
            "End\n",
        ]
        result, count = restore_lines(src, dest)
        self.assertEqual(count, 2)
        self.assertIn("@[template](/_contentTemplates/common/foo.md#a)\n", result)
        self.assertIn("@[template](/_contentTemplates/common/bar.md#b)\n", result)

    def test_preserves_dest_only_lines(self):
        """Lines only in dest (Marilo additions) are kept."""
        src = [
            "Shared line\n",
        ]
        dest = [
            "Shared line\n",
            "Marilo-only line\n",
        ]
        result, count = restore_lines(src, dest)
        self.assertEqual(count, 0)
        self.assertIn("Marilo-only line\n", result)

    def test_no_templates_returns_dest_unchanged(self):
        """Files without template lines return dest unchanged."""
        src  = ["Line A\n", "Line B\n"]
        dest = ["Line A\n", "Line B (marilo)\n"]
        result, count = restore_lines(src, dest)
        self.assertEqual(count, 0)
        self.assertEqual(result, dest)

    def test_handles_brand_replaced_anchor(self):
        """Anchor line that was brand-replaced in dest is still found."""
        src = [
            "See <TelerikChart> docs\n",
            "@[template](/_contentTemplates/chart/link-to-basics.md#understand-basics-and-databinding-first)\n",
            "Next section\n",
        ]
        dest = [
            "See <MariloChart> docs\n",   # brand-replaced
            "Next section\n",
        ]
        result, count = restore_lines(src, dest)
        self.assertEqual(count, 1)
        self.assertIn("@[template](/_contentTemplates/chart/link-to-basics.md#understand-basics-and-databinding-first)\n", result)

if __name__ == '__main__':
    unittest.main()
