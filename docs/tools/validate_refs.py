#!/usr/bin/env python3
"""
Validate @[template] references in component-spec files.

Scans all .md files under component-specs/ and checks that every
@[template](/_contentTemplates/<path>#<anchor>) reference resolves to
an actual file in _contentTemplates/.

Usage:
    python validate_refs.py          # full report + exit code
    python validate_refs.py --verify # CI-friendly, exits 0/1 without extra output
"""

from pathlib import Path
import re
import sys

SPEC_DIR = Path(__file__).parent.parent / "component-specs"
TEMPLATE_DIR = Path(__file__).parent.parent / "_contentTemplates"
PATTERN = re.compile(r'@\[template\]\(/_contentTemplates/([^)#\n]+)')


def validate(spec_dir=None, template_dir=None):
    """
    Scan all .md files under spec_dir and verify that every @[template]
    reference resolves to a file in template_dir.

    Returns (errors: list[str], total: int).
    """
    if spec_dir is None:
        spec_dir = SPEC_DIR
    if template_dir is None:
        template_dir = TEMPLATE_DIR

    spec_dir = Path(spec_dir)
    template_dir = Path(template_dir)

    errors = []
    total = 0

    for md_file in sorted(spec_dir.rglob("*.md")):
        content = md_file.read_text(encoding="utf-8", errors="replace")
        matches = PATTERN.findall(content)
        for rel_path in matches:
            total += 1
            # Strip any trailing #anchor that slipped past the regex (shouldn't
            # happen given [^)#\n]+, but be defensive).
            rel_path = rel_path.split("#")[0].strip()
            target = template_dir / rel_path
            if not target.is_file():
                errors.append(
                    f"BROKEN: {md_file.relative_to(spec_dir)} "
                    f"→ _contentTemplates/{rel_path} (not found)"
                )

    return errors, total


if __name__ == "__main__":
    verify_mode = "--verify" in sys.argv
    errors, total = validate()

    if errors:
        if not verify_mode:
            for e in errors:
                print(e)
            print(f"FAIL: {len(errors)} broken @[template] reference(s) out of {total} total.")
        sys.exit(1)
    else:
        if not verify_mode:
            print(f"PASS: All {total} @[template] references resolve correctly.")
        sys.exit(0)
