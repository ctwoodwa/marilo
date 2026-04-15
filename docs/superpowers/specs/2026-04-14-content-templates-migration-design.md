# Design: `_contentTemplates/` Migration + Include Restoration

**Date:** 2026-04-14  
**Status:** Approved  
**Confidence:** High — proven approach, all facts verified against live file system

---

## End State

`Marilo/docs/_contentTemplates/` exists with all 59 template files converted to Marilo branding. Every one of the 502 component-spec files that previously had `@[template]` includes stripped now has those lines restored, pointing to the new destination-side templates. Component specs render complete shared content sections (state management, editing, accessibility, CSS table styles, etc.) as intended.

---

## Context & Why

When `blazor-docs/components/` was cloned into `Marilo/docs/component-specs/`, the migration script stripped every `@[template](/_contentTemplates/...)` include line rather than preserving or resolving it. This removed 1,056 include references across 502 files, leaving holes where shared content blocks should appear — CSS table styles, reusable state/editing/databinding sections, shared notes, and more. The `_contentTemplates/` directory (59 files) was never migrated to the destination. Until it is, the component specs are structurally incomplete even though all `.md` files are present.

---

## Success Criteria

- `Marilo/docs/_contentTemplates/` contains all 59 template files with zero remaining `telerik`/`Telerik` brand references
- All 165 unique template anchors referenced in source are resolvable in the migrated template files
- All 1,056 `@[template]` include lines are present in destination component-spec files, pointing to `/_contentTemplates/` paths
- Zero `@[template]` lines reference paths that don't exist in `Marilo/docs/_contentTemplates/`
- No destination component-spec file is otherwise altered (brand replacements and Marilo-specific content are preserved)

**NOT in scope:**
- Migration of non-component directories (`getting-started/`, `knowledge-base/`, etc.)
- Content review or quality audit of the template files beyond brand replacement
- Inlining template content (A2 approach — deferred)

**FAILED if:**
- Any destination component-spec file loses existing Marilo-specific content (API changes, anchor fixes) during the include restoration
- Template files contain remaining `telerik.com` URLs or `Telerik.Blazor.Components` slug references

---

## Assumptions & Validation

- `The @[template] syntax is the live doc-build include format` → VALIDATE BY: confirm with existing working files in source → IMPACT IF WRONG: restore approach may need to target a different syntax
- `All template lines were stripped (none were inlined)` → VALIDATE BY: `grep -r "@\[template\]" Marilo/docs/component-specs/` returns 0 (already confirmed) → IMPACT IF WRONG: some files may already have inlined content that would be duplicated
- `Destination files have no other structural diffs beyond brand/API replacements` → VALIDATE BY: spot-check 5 high-diff files → IMPACT IF WRONG: context-matching for insertion points may fail more often, requiring manual review

---

## Phases

### Phase 1 — Copy and convert `_contentTemplates/`

**Scope:** All 59 `.md` files under `blazor-docs/_contentTemplates/` → `Marilo/docs/_contentTemplates/`

**Transformations applied (same ruleset as component-specs):**
- `telerik` → `marilo`, `Telerik` → `Marilo` (case-preserving)
- Component tags: `<TelerikXxx` → `<MariloXxx`, `</TelerikXxx` → `</MariloXxx`
- `TelerikXxx` class/type names → `MariloXxx`
- URLs: `demos.telerik.com` → `demos.marilo.com`, `www.telerik.com` → `www.marilo.com`
- Slug refs: `slug:Telerik.Blazor.Components.` → `slug:Marilo.Blazor.Components.`
- Tags frontmatter: `telerik,` → `marilo,`

**Deliverable:** `Marilo/docs/_contentTemplates/` with 59 converted files, directory tree preserved exactly.

**Gate:** `grep -ri "telerik" Marilo/docs/_contentTemplates/` returns 0 matches AND file count matches source (59 files).

**Review:** Spot-check 5 template files (parameters-table-styles.md, general-info.md, grid/state.md, scheduler/views.md, treelist/editing.md) — brand replacements correct, structure intact.

---

### Phase 2 — Restore `@[template]` includes in component-spec files

**Scope:** 502 `.md` files in `Marilo/docs/component-specs/` that had template includes stripped.

**Algorithm (per file):**
1. Load source file (`blazor-docs/components/<path>.md`) and destination file (`Marilo/docs/component-specs/<path>.md`)
2. Extract all `@[template]` lines from source with their 0-based line positions
3. For each stripped template line, find the "anchor line" — the non-template line immediately preceding it in source
4. Apply brand transformations to the anchor line text (telerik→marilo) to get the expected destination text
5. Locate that anchor line in the destination file (exact match first, then case-insensitive fuzzy)
6. Insert the `@[template]` line immediately after the anchor line in destination
7. If anchor line not found, fall back to inserting at the proportional line position (source line% × dest line count)
8. Write the patched destination file

**Edge cases:**
- Template line is the first line in file → insert at line 0
- Multiple identical anchor lines → use the one closest to proportional position
- Anchor line itself was modified in destination → apply brand mapping before searching
- Consecutive template lines → insert all, preserving relative order

**Implementation:** Python script (`tools/restore-template-includes.py`) that processes all 502 files, logs each insertion with file path + line number, and reports any files where anchor-matching fell back to proportional positioning for manual review.

**Deliverable:** All 502 files updated; script produces a report listing any fallback cases.

**Gate:** `grep -r "@\[template\]" Marilo/docs/component-specs/ | wc -l` equals 1,056 AND `grep -r "@\[template\]" Marilo/docs/component-specs/ | grep -v "/_contentTemplates/"` returns 0.

**Review:** Open 10 randomly-selected restored files; verify template include lines appear at semantically correct positions (before/after the same surrounding content as in source).

---

### Review Checkpoint (after both phases)

**Gate:** Run a cross-reference check — every `/_contentTemplates/` path referenced in any component-spec file resolves to an actual file in `Marilo/docs/_contentTemplates/`. Script: `tools/validate-template-refs.py`.

---

## Verification

**Automated:**
- `grep -ri "telerik" Marilo/docs/_contentTemplates/` → 0 results
- `grep -r "@\[template\]" Marilo/docs/component-specs/ | wc -l` → 1,056
- Cross-reference validator: 0 broken template paths

**Manual:**
- Spot-check 5 high-reference-count files (grid/state.md, scheduler views, treelist/editing) — template lines in correct positions
- Spot-check 3 destination component-spec files with complex diffs (chart/multiple-axes.md, aiprompt/events.md) — Marilo-specific content (API changes, anchor fixes) is intact

**Ongoing Observability:**
- Re-run the cross-reference validator whenever new component spec files are added

---

## Completion Gate

- [ ] All 59 template files registered in `Marilo/docs/_contentTemplates/` (directory exists, correct count)
- [ ] `anatomy.md` updated with new `_contentTemplates/` directory entry
- [ ] `gap-analysis.md` updated to reflect template infrastructure as resolved
- [ ] No orphaned `@[template]` references (validated by cross-reference script)
- [ ] Restoration script and validator committed to `Marilo/docs/tools/` for future use

---

## Tools Produced

| Tool | Path | Purpose |
|---|---|---|
| Brand-replacement script | `Marilo/docs/tools/convert-templates.py` | Copy + convert `_contentTemplates/` |
| Include restoration script | `Marilo/docs/tools/restore-template-includes.py` | Re-add stripped `@[template]` lines |
| Cross-reference validator | `Marilo/docs/tools/validate-template-refs.py` | Verify all template paths resolve |
