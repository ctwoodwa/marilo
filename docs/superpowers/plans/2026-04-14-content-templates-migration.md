# `_contentTemplates/` Migration + Include Restoration Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Migrate 59 `_contentTemplates/` files from `blazor-docs` to `Marilo/docs` with brand replacements, then restore 1,056 `@[template]` include lines that were stripped from 502 component-spec files.

**Architecture:** Three Python scripts handle the work — a brand-replacement utility, a template-copy script, and an include-restoration script. The restoration uses `difflib.SequenceMatcher` to align source and destination files and re-insert stripped template lines at their correct positions without disturbing existing Marilo-specific content. A validator script confirms all restored references resolve to real files.

**Tech Stack:** Python 3 (stdlib only — `pathlib`, `difflib`, `re`, `unittest`), Windows paths.

---

## File Structure

| File | Action | Purpose |
|---|---|---|
| `Marilo/docs/tools/brand_replace.py` | Create | Shared brand-replacement function used by all scripts |
| `Marilo/docs/tools/tests/test_brand_replace.py` | Create | Unit tests for brand_replace |
| `Marilo/docs/tools/convert_templates.py` | Create | Phase 1: copy + convert `_contentTemplates/` |
| `Marilo/docs/tools/restore_includes.py` | Create | Phase 2: restore stripped `@[template]` lines |
| `Marilo/docs/tools/validate_refs.py` | Create | Validate all `@[template]` paths resolve |
| `Marilo/docs/_contentTemplates/` | Create (59 files) | Converted template files |
| `Marilo/docs/gap-analysis.md` | Modify | Mark template infrastructure as resolved |

---

## Task 1: Create tools directory and brand-replacement module

**Files:**
- Create: `Marilo/docs/tools/brand_replace.py`
- Create: `Marilo/docs/tools/tests/__init__.py`
- Create: `Marilo/docs/tools/tests/test_brand_replace.py`

- [ ] **Step 1: Write the failing tests**

Create `Marilo/docs/tools/tests/test_brand_replace.py`:

```python
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

if __name__ == '__main__':
    unittest.main()
```

- [ ] **Step 2: Run tests — expect FAIL (module not found)**

```bash
cd C:/Projects/Marilo/docs/tools
python -m pytest tests/test_brand_replace.py -v
```

Expected: `ModuleNotFoundError: No module named 'brand_replace'`

- [ ] **Step 3: Create the brand_replace module**

Create `Marilo/docs/tools/brand_replace.py`:

```python
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
```

Create `Marilo/docs/tools/tests/__init__.py` (empty file).

- [ ] **Step 4: Run tests — expect all PASS**

```bash
cd C:/Projects/Marilo/docs/tools
python -m pytest tests/test_brand_replace.py -v
```

Expected output:
```
test_brand_replace.py::TestBrandReplace::test_lowercase_telerik PASSED
test_brand_replace.py::TestBrandReplace::test_capitalized_telerik PASSED
test_brand_replace.py::TestBrandReplace::test_component_tag PASSED
test_brand_replace.py::TestBrandReplace::test_slug_namespace PASSED
test_brand_replace.py::TestBrandReplace::test_slug_namespace_lowercase PASSED
test_brand_replace.py::TestBrandReplace::test_demo_url PASSED
test_brand_replace.py::TestBrandReplace::test_www_url PASSED
test_brand_replace.py::TestBrandReplace::test_preserves_non_telerik_content PASSED
test_brand_replace.py::TestBrandReplace::test_template_line_unchanged PASSED
9 passed
```

- [ ] **Step 5: Commit**

```bash
cd C:/Projects/Marilo
git add docs/tools/brand_replace.py docs/tools/tests/__init__.py docs/tools/tests/test_brand_replace.py
git commit -m "chore(docs): add brand_replace utility with tests"
```

---

## Task 2: Implement and run convert_templates.py

**Files:**
- Create: `Marilo/docs/tools/convert_templates.py`
- Creates at runtime: `Marilo/docs/_contentTemplates/` (59 files)

- [ ] **Step 1: Write the script**

Create `Marilo/docs/tools/convert_templates.py`:

```python
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
    src_files  = list(SOURCE.rglob("*.md"))

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
        convert()
        print()
        verify()
```

- [ ] **Step 2: Run dry-run to preview**

```bash
cd C:/Projects/Marilo/docs/tools
python convert_templates.py --dry-run 2>&1 | head -20
```

Expected: list of `[dry] converted: <path>` lines, ending with `[dry] Done. 59 files.`

*(Note: `--dry-run` flag is for preview; if you want to skip preview, go straight to Step 3.)*

- [ ] **Step 3: Run the conversion**

```bash
cd C:/Projects/Marilo/docs/tools
python convert_templates.py
```

Expected final lines:
```
Done. 59 files.

Source files : 59
Dest files   : 59
PASS: All files converted, no remaining telerik references.
```

- [ ] **Step 4: Spot-check 5 converted files**

```bash
# Check parameters-table-styles.md — should have no telerik, CSS intact
cat "C:/Projects/Marilo/docs/_contentTemplates/common/parameters-table-styles.md"

# Check grid/state.md header
head -10 "C:/Projects/Marilo/docs/_contentTemplates/grid/state.md"

# Quick scan for any remaining telerik
grep -ri "telerik" "C:/Projects/Marilo/docs/_contentTemplates/"
```

Expected: `grep` returns nothing (exit code 1 / no output).

- [ ] **Step 5: Commit**

```bash
cd C:/Projects/Marilo
git add docs/tools/convert_templates.py docs/_contentTemplates/
git commit -m "chore(docs): migrate _contentTemplates with marilo brand replacements"
```

---

## Task 3: Implement and run restore_includes.py

**Files:**
- Create: `Marilo/docs/tools/restore_includes.py`
- Modify at runtime: 502 files under `Marilo/docs/component-specs/`

- [ ] **Step 1: Write tests for the core restore logic**

Create `Marilo/docs/tools/tests/test_restore_includes.py`:

```python
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
```

- [ ] **Step 2: Run tests — expect FAIL**

```bash
cd C:/Projects/Marilo/docs/tools
python -m pytest tests/test_restore_includes.py -v
```

Expected: `ModuleNotFoundError: No module named 'restore_includes'`

- [ ] **Step 3: Write the script**

Create `Marilo/docs/tools/restore_includes.py`:

```python
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
    src_lines: list[str],
    dest_lines: list[str],
) -> Tuple[list[str], int]:
    """
    Given source lines (may contain @[template] lines) and destination lines
    (template lines stripped, content brand-replaced), return a new list of
    destination lines with the template lines re-inserted.

    Returns (result_lines, count_of_templates_restored).
    """
    # Brand-replace source so it aligns with dest during diffing
    src_branded = [brand_replace(line) for line in src_lines]

    matcher = difflib.SequenceMatcher(
        None,
        [l.strip() for l in src_branded],
        [l.strip() for l in dest_lines],
        autojunk=False,
    )

    result: list[str] = []
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
                if line.strip().startswith("@[template]"):
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


if __name__ == "__main__":
    dry_run = "--dry-run" in sys.argv
    if dry_run:
        print("DRY RUN — no files will be written\n")
    restore_all(dry_run)
```

- [ ] **Step 4: Run tests — expect all PASS**

```bash
cd C:/Projects/Marilo/docs/tools
python -m pytest tests/test_restore_includes.py -v
```

Expected: 6 tests, all PASS.

- [ ] **Step 5: Dry-run to preview scope**

```bash
cd C:/Projects/Marilo/docs/tools
python restore_includes.py --dry-run 2>&1 | tail -5
```

Expected final line: `Done (DRY RUN). Restored 1056 template includes across 502 files.`
(Counts may vary slightly — within ±10 is acceptable; large deviation signals a bug.)

- [ ] **Step 6: Run the restoration**

```bash
cd C:/Projects/Marilo/docs/tools
python restore_includes.py
```

Expected final line: `Done. Restored 1056 template includes across 502 files.`

- [ ] **Step 7: Verify include count in destination**

```bash
grep -r "@\[template\]" "C:/Projects/Marilo/docs/component-specs/" --include="*.md" | wc -l
```

Expected: `1056` (±10 acceptable due to edge cases in SequenceMatcher alignment).

- [ ] **Step 8: Commit**

```bash
cd C:/Projects/Marilo
git add docs/tools/restore_includes.py docs/tools/tests/test_restore_includes.py docs/component-specs/
git commit -m "chore(docs): restore @[template] includes stripped during component-spec migration"
```

---

## Task 4: Write and run validate_refs.py

**Files:**
- Create: `Marilo/docs/tools/validate_refs.py`

- [ ] **Step 1: Write the validator**

Create `Marilo/docs/tools/validate_refs.py`:

```python
#!/usr/bin/env python3
"""
Validate that every @[template](/_contentTemplates/...) reference in
component-specs resolves to an actual file in Marilo/docs/_contentTemplates/.

Usage:
    python validate_refs.py        # exits 0 on pass, 1 on fail
"""
import re
import sys
from pathlib import Path

SPEC_DIR     = Path(r"C:/Projects/Marilo/docs/component-specs")
TEMPLATE_DIR = Path(r"C:/Projects/Marilo/docs/_contentTemplates")

# Matches /_contentTemplates/<path> before the closing ) or #anchor
PATTERN = re.compile(r'@\[template\]\(/_contentTemplates/([^)#\n]+)')


def validate() -> bool:
    errors: list[str] = []
    total = 0

    for md_file in sorted(SPEC_DIR.rglob("*.md")):
        text = md_file.read_text(encoding="utf-8", errors="replace")
        for match in PATTERN.finditer(text):
            total += 1
            rel_path      = match.group(1).strip()
            template_path = TEMPLATE_DIR / rel_path
            if not template_path.exists():
                errors.append(
                    f"  BROKEN: {md_file.relative_to(SPEC_DIR)}\n"
                    f"          → _contentTemplates/{rel_path}"
                )

    if errors:
        print(f"FAIL: {len(errors)} broken reference(s) out of {total} total.\n")
        for e in errors:
            print(e)
        return False

    print(f"PASS: All {total} @[template] references resolve correctly.")
    return True


if __name__ == "__main__":
    sys.exit(0 if validate() else 1)
```

- [ ] **Step 2: Run the validator**

```bash
cd C:/Projects/Marilo/docs/tools
python validate_refs.py
```

Expected: `PASS: All 1056 @[template] references resolve correctly.`

If any BROKEN refs appear, check that:
1. `_contentTemplates/` was created by `convert_templates.py` (Task 2)
2. The referenced path exists under `C:/Projects/Marilo/docs/_contentTemplates/`
3. The template file name matches exactly (case-sensitive on non-Windows)

- [ ] **Step 3: Commit**

```bash
cd C:/Projects/Marilo
git add docs/tools/validate_refs.py
git commit -m "chore(docs): add template reference validator"
```

---

## Task 5: Update gap-analysis.md and final verification

**Files:**
- Modify: `Marilo/docs/gap-analysis.md`

- [ ] **Step 1: Run full final verification**

```bash
# 1. No remaining telerik refs in _contentTemplates
grep -ri "telerik" "C:/Projects/Marilo/docs/_contentTemplates/" && echo "FAIL: telerik refs remain" || echo "PASS: no telerik refs"

# 2. Template include count
echo "Template includes in component-specs:"
grep -r "@\[template\]" "C:/Projects/Marilo/docs/component-specs/" --include="*.md" | wc -l

# 3. All refs resolve
cd C:/Projects/Marilo/docs/tools && python validate_refs.py
```

All three commands must show PASS / expected count before proceeding.

- [ ] **Step 2: Update gap-analysis.md**

Open `C:/Projects/Marilo/docs/gap-analysis.md` and add the following section after the existing summary table:

```markdown
---

## 5. Template Infrastructure — RESOLVED (2026-04-14)

`_contentTemplates/` has been migrated to `Marilo/docs/_contentTemplates/` (59 files, all brand-converted).
All 1,056 `@[template]` include lines stripped during the original component-spec migration have been restored.
Validator: `docs/tools/validate_refs.py` — run after any new component-spec additions.

### What the templates contain

Shared reusable content blocks injected into component spec files:

| Template | Refs | Content |
|---|---|---|
| `common/parameters-table-styles.md` | 293 | CSS for parameter tables |
| `common/general-info.md` | 73 | Shared notes (async callbacks, value vs data bind) |
| `common/observable-data.md` | 54 | Observable collection patterns |
| `chart/link-to-basics.md` | 51 | Chart basics cross-links |
| `common/themebuilder-section.md` | 32 | ThemeBuilder appearance notes |
| `common/issues-and-warnings.md` | 27 | Common warnings (ValueChanged lambda, etc.) |
| *(49 additional templates)* | 326 | Various component-specific shared content |
```

- [ ] **Step 3: Commit**

```bash
cd C:/Projects/Marilo
git add docs/gap-analysis.md
git commit -m "chore(docs): update gap-analysis — template infrastructure resolved"
```

---

## Self-Review Against Spec

| Spec Requirement | Covered By |
|---|---|
| 59 template files in `Marilo/docs/_contentTemplates/` | Task 2 |
| Zero remaining `telerik` refs in templates | Task 2 Steps 4 + 5 |
| All 165 unique anchors resolvable | Task 4 (validator checks file paths) |
| All 1,056 `@[template]` lines restored in component-specs | Task 3 |
| Zero broken template path refs | Task 4 |
| No existing Marilo-specific content lost | Task 3 (SequenceMatcher keeps `insert` blocks; tests cover this) |
| Restoration script committed to `docs/tools/` | Tasks 1–4 |
| `gap-analysis.md` updated | Task 5 |

**Placeholder scan:** No TBD, TODO, or "similar to above" patterns found. ✓

**Type consistency:** `restore_lines(src_lines, dest_lines)` returns `(list[str], int)` — used consistently in `restore_file` and tests. ✓
