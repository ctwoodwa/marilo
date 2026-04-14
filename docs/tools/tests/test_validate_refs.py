import sys
import os
sys.path.insert(0, os.path.join(os.path.dirname(__file__), '..'))

import unittest
import tempfile
from pathlib import Path
from validate_refs import validate


class TestValidateRefs(unittest.TestCase):

    def _make_dirs(self, tmp: Path):
        """Create spec_dir and template_dir under tmp."""
        spec_dir = tmp / "component-specs"
        template_dir = tmp / "_contentTemplates"
        spec_dir.mkdir()
        template_dir.mkdir()
        return spec_dir, template_dir

    def test_valid_reference_passes(self):
        """A spec file with a valid template reference → no errors."""
        with tempfile.TemporaryDirectory() as td:
            tmp = Path(td)
            spec_dir, template_dir = self._make_dirs(tmp)

            # Create the template file
            (template_dir / "grid").mkdir()
            (template_dir / "grid" / "state.md").write_text("# State\n")

            # Create a spec file referencing it
            (spec_dir / "grid.md").write_text(
                "@[template](/_contentTemplates/grid/state.md#some-anchor)\n"
            )

            errors, total = validate(spec_dir=spec_dir, template_dir=template_dir)

            self.assertEqual(errors, [])
            self.assertEqual(total, 1)

    def test_broken_reference_fails(self):
        """A spec file with a reference to a non-existent template → has errors."""
        with tempfile.TemporaryDirectory() as td:
            tmp = Path(td)
            spec_dir, template_dir = self._make_dirs(tmp)

            # No template file created
            (spec_dir / "grid.md").write_text(
                "@[template](/_contentTemplates/grid/missing.md#anchor)\n"
            )

            errors, total = validate(spec_dir=spec_dir, template_dir=template_dir)

            self.assertEqual(len(errors), 1)
            self.assertEqual(total, 1)

    def test_anchor_stripped_from_path(self):
        """The #anchor fragment is stripped before checking file existence.

        Verifies that a reference like grid/state.md#table-layout-anchor resolves
        to grid/state.md on disk.  If anchor stripping were removed, the script
        would look for a file literally named 'state.md#table-layout-anchor', which
        cannot exist on any filesystem — so the test would report a broken ref.
        """
        with tempfile.TemporaryDirectory() as td:
            tmp = Path(td)
            spec_dir, template_dir = self._make_dirs(tmp)

            # Only the bare file exists — no '#…' suffix in its name.
            (template_dir / "grid").mkdir()
            (template_dir / "grid" / "state.md").write_text("# State\n")

            # Reference uses a multi-segment anchor to make the intent explicit.
            (spec_dir / "spec.md").write_text(
                "@[template](/_contentTemplates/grid/state.md#table-layout-anchor)\n"
            )

            errors, total = validate(spec_dir=spec_dir, template_dir=template_dir)

            # If anchor stripping works correctly, the file is found.
            self.assertEqual(errors, [])
            self.assertEqual(total, 1)

            # Confirm that a path WITH the hash fragment is NOT a valid file —
            # showing why the strip is necessary for the lookup to succeed.
            anchored_path = template_dir / "grid" / "state.md#table-layout-anchor"
            self.assertFalse(
                anchored_path.exists(),
                "A file with a '#' in its name should not exist on this filesystem; "
                "anchor stripping is required for the reference to resolve.",
            )

    def test_multiple_references_in_one_file(self):
        """Multiple valid template refs in one file → all pass, total count correct."""
        with tempfile.TemporaryDirectory() as td:
            tmp = Path(td)
            spec_dir, template_dir = self._make_dirs(tmp)

            (template_dir / "common").mkdir()
            (template_dir / "common" / "foo.md").write_text("# Foo\n")
            (template_dir / "common" / "bar.md").write_text("# Bar\n")

            (spec_dir / "spec.md").write_text(
                "@[template](/_contentTemplates/common/foo.md#anchor-a)\n"
                "@[template](/_contentTemplates/common/bar.md#anchor-b)\n"
            )

            errors, total = validate(spec_dir=spec_dir, template_dir=template_dir)

            self.assertEqual(errors, [])
            self.assertEqual(total, 2)

    def test_returns_list_of_errors(self):
        """Broken refs are returned as a list of descriptive strings."""
        with tempfile.TemporaryDirectory() as td:
            tmp = Path(td)
            spec_dir, template_dir = self._make_dirs(tmp)

            (spec_dir / "spec.md").write_text(
                "@[template](/_contentTemplates/missing/file.md#anchor)\n"
            )

            errors, total = validate(spec_dir=spec_dir, template_dir=template_dir)

            self.assertEqual(len(errors), 1)
            self.assertIsInstance(errors[0], str)
            self.assertIn("missing/file.md", errors[0])

    def test_missing_spec_dir_raises(self):
        """validate() raises FileNotFoundError when spec_dir does not exist."""
        with tempfile.TemporaryDirectory() as td:
            tmp = Path(td)
            missing_spec = tmp / "nonexistent-specs"
            template_dir = tmp / "_contentTemplates"
            template_dir.mkdir()

            with self.assertRaises(FileNotFoundError):
                validate(spec_dir=missing_spec, template_dir=template_dir)

    def test_no_references_returns_zero(self):
        """A spec file with no template refs → no errors, total 0."""
        with tempfile.TemporaryDirectory() as td:
            tmp = Path(td)
            spec_dir, template_dir = self._make_dirs(tmp)

            (spec_dir / "spec.md").write_text("# Just a heading\nSome text.\n")

            errors, total = validate(spec_dir=spec_dir, template_dir=template_dir)

            self.assertEqual(errors, [])
            self.assertEqual(total, 0)

    def test_scans_subdirectories_recursively(self):
        """Spec files in nested subdirectories are also scanned."""
        with tempfile.TemporaryDirectory() as td:
            tmp = Path(td)
            spec_dir, template_dir = self._make_dirs(tmp)

            (template_dir / "common").mkdir()
            (template_dir / "common" / "foo.md").write_text("# Foo\n")

            nested = spec_dir / "grid" / "details"
            nested.mkdir(parents=True)
            (nested / "spec.md").write_text(
                "@[template](/_contentTemplates/common/foo.md#anchor)\n"
            )

            errors, total = validate(spec_dir=spec_dir, template_dir=template_dir)

            self.assertEqual(errors, [])
            self.assertEqual(total, 1)


if __name__ == '__main__':
    unittest.main()
