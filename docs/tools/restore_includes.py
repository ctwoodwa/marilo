#!/usr/bin/env python3
"""
Phase 2: Restore @[template] includes stripped from component-spec files.

Uses SequenceMatcher to align brand-replaced source with destination,
then re-inserts any @[template] lines that appear in source but not dest.

Source:  C:/Projects/blazor-docs/components/
Dest:    C:/Projects/Marilo/docs/component-specs/

Usage:
    python restore_includes.py             # run restoration
    python restore_includes.py --dry-run   # preview only
"""
import sys
import difflib
from pathlib import Path
from typing import Tuple

SOURCE = Path(r"C:/Projects/blazor-docs/components")
DEST   = Path(r"C:/Projects/Marilo/docs/component-specs")

sys.path.insert(0, str(Path(__file__).parent))
from brand_replace import brand_replace


def restore_lines(
    src_lines: list,
    dest_lines: list,
) -> Tuple[list, int]:
    """
    Given source lines (may contain @[template] lines) and destination lines
    (template lines stripped, content brand-replaced), return a new list of
    destination lines with the template lines re-inserted.

    Returns (result_lines, count_of_templates_restored).

    Note: Only restores *standalone* @[template] lines (lines that start with the include
    directive). Inline template references (where @[template] is embedded mid-sentence,
    e.g. "The component @[template](...) [read more]") are not restored — those 48
    references across 33 files were removed as full sentences in the destination and
    require manual review.
    """
    # Brand-replace source so it aligns with dest during diffing
    src_branded = [brand_replace(line) for line in src_lines]

    matcher = difflib.SequenceMatcher(
        None,
        [l.strip() for l in src_branded],
        [l.strip() for l in dest_lines],
        autojunk=False,
    )

    result: list = []
    templates_added = 0

    for tag, i1, i2, j1, j2 in matcher.get_opcodes():
        if tag == "equal":
            # Identical blocks — keep dest lines (preserve dest encoding/whitespace)
            result.extend(dest_lines[j1:j2])

        elif tag == "insert":
            # Lines only in dest (Marilo-specific additions) — keep them
            result.extend(dest_lines[j1:j2])

        elif tag == "delete":
            # Lines only in src (stripped during migration) — re-add templates only
            for line in src_branded[i1:i2]:
                if line.strip().startswith("@[template]"):  # standalone only; inline refs skipped (see docstring)
                    result.append(line)
                    templates_added += 1

        elif tag == "replace":
            # Mixed divergence: insert any template lines from src, then dest lines
            # (template lines come before dest lines — correct for most cases where
            #  templates appear before parameter tables or section content)
            for line in src_branded[i1:i2]:
                if line.strip().startswith("@[template]"):
                    result.append(line)
                    templates_added += 1
            result.extend(dest_lines[j1:j2])

    return result, templates_added


def restore_file(src_path: Path, dest_path: Path, dry_run: bool = False) -> int:
    """Process a single file pair. Returns number of template lines restored."""
    src_text  = src_path.read_text(encoding="utf-8", errors="replace")
    dest_text = dest_path.read_text(encoding="utf-8", errors="replace")

    # Fast path: no templates in source
    if "@[template]" not in src_text:
        return 0

    src_lines  = src_text.splitlines(keepends=True)
    dest_lines = dest_text.splitlines(keepends=True)

    result_lines, count = restore_lines(src_lines, dest_lines)

    if count > 0 and not dry_run:
        dest_path.write_text("".join(result_lines), encoding="utf-8")

    return count


def restore_all(dry_run: bool = False) -> None:
    total_files = 0
    total_templates = 0
    missing = []

    for src_file in sorted(SOURCE.rglob("*.md")):
        rel       = src_file.relative_to(SOURCE)
        dest_file = DEST / rel

        if not dest_file.exists():
            missing.append(str(rel))
            continue

        added = restore_file(src_file, dest_file, dry_run)
        if added > 0:
            total_files    += 1
            total_templates += added
            prefix = "[dry] " if dry_run else ""
            print(f"  {prefix}+{added} includes in: {rel}")

    if missing:
        print(f"\nWARNING: {len(missing)} source files had no destination match:")
        for m in missing[:10]:
            print(f"  {m}")

    mode = " (DRY RUN)" if dry_run else ""
    print(f"\nDone{mode}. Restored {total_templates} template includes across {total_files} files.")
    print("Note: inline @[template] refs (embedded mid-sentence) are not handled — see docstring.")


if __name__ == "__main__":
    dry_run = "--dry-run" in sys.argv
    if dry_run:
        print("DRY RUN — no files will be written\n")
    restore_all(dry_run)
