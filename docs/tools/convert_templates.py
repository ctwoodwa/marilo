#!/usr/bin/env python3
"""
Phase 1: Copy _contentTemplates from blazor-docs to Marilo/docs with brand replacements.

Source:  C:/Projects/blazor-docs/_contentTemplates/
Dest:    C:/Projects/Marilo/docs/_contentTemplates/

Usage:
    python convert_templates.py           # run conversion
    python convert_templates.py --verify  # verify results only (no write)
"""
import sys
import shutil
from pathlib import Path

# Adjust these paths if running from a different working directory
SOURCE = Path(r"C:/Projects/blazor-docs/_contentTemplates")
DEST   = Path(r"C:/Projects/Marilo/docs/_contentTemplates")

sys.path.insert(0, str(Path(__file__).parent))
from brand_replace import brand_replace


def convert(dry_run: bool = False) -> int:
    """Copy and convert all template files. Returns count of files processed."""
    if not SOURCE.exists():
        print(f"ERROR: Source not found: {SOURCE}")
        return 0

    if not dry_run and DEST.exists():
        # Wipe DEST so stale files from prior runs don't accumulate
        shutil.rmtree(DEST)

    count = 0
    for src_file in sorted(SOURCE.rglob("*.md")):
        rel = src_file.relative_to(SOURCE)
        dest_file = DEST / rel

        text = src_file.read_text(encoding="utf-8", errors="replace")
        converted = brand_replace(text)

        if not dry_run:
            dest_file.parent.mkdir(parents=True, exist_ok=True)
            dest_file.write_text(converted, encoding="utf-8")

        count += 1
        print(f"  {'[dry] ' if dry_run else ''}converted: {rel}")

    print(f"\n{'[dry] ' if dry_run else ''}Done. {count} files.")
    return count


def verify() -> bool:
    """Verify destination has correct file count and no remaining telerik refs."""
    if not DEST.exists():
        print("FAIL: _contentTemplates/ does not exist in destination.")
        return False

    dest_files = list(DEST.rglob("*.md"))
    src_files  = list(SOURCE.rglob("*.md")) if SOURCE.exists() else []

    print(f"Source files : {len(src_files)}")
    print(f"Dest files   : {len(dest_files)}")

    if len(dest_files) != len(src_files):
        print(f"FAIL: File count mismatch ({len(src_files)} expected, {len(dest_files)} found).")
        return False

    hits = []
    for f in dest_files:
        text = f.read_text(encoding="utf-8", errors="replace")
        if "telerik" in text.lower():
            lines = [f"  {f.relative_to(DEST)}:{i+1}: {l.rstrip()}"
                     for i, l in enumerate(text.splitlines()) if "telerik" in l.lower()]
            hits.extend(lines[:3])  # show first 3 hits per file

    if hits:
        print(f"FAIL: Remaining 'telerik' references found:")
        for h in hits:
            print(h)
        return False

    print("PASS: All files converted, no remaining telerik references.")
    return True


if __name__ == "__main__":
    if "--verify" in sys.argv:
        sys.exit(0 if verify() else 1)
    else:
        dry_run = "--dry-run" in sys.argv
        convert(dry_run=dry_run)
        if not dry_run:
            print()
            verify()
