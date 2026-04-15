# ~~CSS Token Rename: --marilo-* → --k-*~~ — VOIDED

> **VOIDED:** Both `mar-*` class names and `--marilo-*` CSS custom properties are correct and intentional. Renaming them to `k-*`/`--k-*` would violate library independence and licensing constraints. No action needed.

---

# CSS Token Rename: --marilo-* → --k-* Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Rename all CSS custom property tokens from `--marilo-*` to `--k-*` across all three provider SCSS trees to comply with the Telerik/Kendo UI token naming convention.

**Architecture:** This is a mechanical find-and-replace of `--marilo-` with `--k-` across SCSS files and any Razor files that use inline `style="..."` attributes consuming these tokens. CSS class names (`.mar-*`, `class="mar-*"`, CssProvider string literals) are **not changed** — they stay `mar-*`. Only CSS custom property definitions and usages change.

**Tech Stack:** SCSS, `sed` for bulk rename, `npm run scss:build:*` per provider, `dotnet build` for C# validation.

> ⚠️ **Prerequisite for the styling gap-fill plan** (`2026-04-14-styling-gap-analysis.md`): complete this rename first. The gap-fill plan's SCSS code blocks also need their `--marilo-*` token references updated (Task 4 of this plan).

---

## Scope

| What changes | What does NOT change |
|---|---|
| `--marilo-*` CSS custom property definitions in foundation SCSS | `.mar-*` CSS class names |
| `--marilo-*` usages in component/pattern/bridge SCSS | `"mar-*"` strings in CssProvider `.cs` files |
| `--marilo-*` usages in inline Razor `style=""` attributes | `class="mar-*"` attributes in Razor files |
| `--marilo-*` references in the gap-fill plan doc | Fluent UI system tokens (`--neutral-fill-*`, `--accent-fill-*`, `--body-font`, etc.) |

**Files affected:**
```
src/Marilo.Providers.FluentUI/Styles/**/*.scss    (79 files)
src/Marilo.Providers.Bootstrap/Styles/**/*.scss   (bridge files referencing tokens)
src/Marilo.Providers.Material/Styles/**/*.scss
src/Marilo.Components/**/*.razor                  (17 files with inline styles — verify only)
docs/superpowers/plans/2026-04-14-styling-gap-analysis.md
```

---

## Phase 1: Verify Baseline

### Task 1: Confirm clean baseline

- [ ] **Step 1: Verify SCSS builds clean**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 2: Verify dotnet builds clean**

```bash
dotnet build Marilo.slnx
```

Expected: `Build succeeded.` 0 errors.

- [ ] **Step 3: Record rollback SHA**

```bash
git rev-parse HEAD
```

Save this SHA. If something goes wrong: `git reset --hard <SHA>` returns to baseline.

---

## Phase 2: Rename `--marilo-*` Tokens in SCSS

### Task 2: Rename tokens in FluentUI SCSS

**Files:**
- Modify: all files under `src/Marilo.Providers.FluentUI/Styles/`

- [ ] **Step 1: Run the rename**

```bash
find src/Marilo.Providers.FluentUI/Styles -name "*.scss" \
  -exec sed -i 's/--marilo-/--k-/g' {} \;
```

- [ ] **Step 2: Verify zero `--marilo-` references remain**

```bash
grep -r "\-\-marilo-" src/Marilo.Providers.FluentUI/Styles/
```

Expected: no output.

- [ ] **Step 3: Compile FluentUI SCSS**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0. If there are unresolved variable errors, the sed missed a variant — check the error line and correct manually.

- [ ] **Step 4: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/
git commit -m "refactor(styles): rename --marilo- tokens to --k- in FluentUI SCSS"
```

---

### Task 3: Rename tokens in Bootstrap and Material SCSS

**Files:**
- Modify: `src/Marilo.Providers.Bootstrap/Styles/**/*.scss`
- Modify: `src/Marilo.Providers.Material/Styles/**/*.scss`

- [ ] **Step 1: Rename in Bootstrap SCSS**

```bash
find src/Marilo.Providers.Bootstrap/Styles -name "*.scss" \
  -exec sed -i 's/--marilo-/--k-/g' {} \;
```

- [ ] **Step 2: Verify zero `--marilo-` in Bootstrap SCSS**

```bash
grep -r "\-\-marilo-" src/Marilo.Providers.Bootstrap/Styles/
```

Expected: no output.

- [ ] **Step 3: Build Bootstrap SCSS**

```bash
cd src/Marilo.Providers.Bootstrap && npm run scss:build:bootstrap
```

Expected: exits 0.

- [ ] **Step 4: Rename in Material SCSS**

```bash
find src/Marilo.Providers.Material/Styles -name "*.scss" \
  -exec sed -i 's/--marilo-/--k-/g' {} \;
```

- [ ] **Step 5: Verify zero `--marilo-` in Material SCSS**

```bash
grep -r "\-\-marilo-" src/Marilo.Providers.Material/Styles/
```

Expected: no output.

- [ ] **Step 6: Build Material SCSS**

```bash
cd src/Marilo.Providers.Material && npm run scss:build:material
```

Expected: exits 0.

- [ ] **Step 7: Commit**

```bash
git add src/Marilo.Providers.Bootstrap/Styles/ \
        src/Marilo.Providers.Material/Styles/
git commit -m "refactor(styles): rename --marilo- tokens to --k- in Bootstrap and Material SCSS"
```

---

## Phase 3: Check Razor Inline Styles

### Task 4: Verify and fix any `--marilo-*` in Razor inline styles

**Files:**
- Check: `src/Marilo.Components/**/*.razor`

- [ ] **Step 1: Find all Razor files using `--marilo-` in inline styles**

```bash
grep -rn "\-\-marilo-" src/Marilo.Components --include="*.razor"
```

- [ ] **Step 2: Rename any hits**

If Step 1 returned results, run:

```bash
find src/Marilo.Components -name "*.razor" \
  -exec sed -i 's/--marilo-/--k-/g' {} \;
```

Then re-run the grep to confirm zero matches.

- [ ] **Step 3: dotnet build**

```bash
dotnet build Marilo.slnx
```

Expected: `Build succeeded.` 0 errors.

- [ ] **Step 4: Commit if any files changed**

```bash
git diff --name-only src/Marilo.Components/
# only commit if the above shows changed files
git add src/Marilo.Components/
git commit -m "refactor(components): rename --marilo- token references to --k- in inline styles"
```

---

## Phase 4: Update the Styling Gap-Fill Plan

### Task 5: Update `--marilo-*` token references in the gap-fill plan

**Files:**
- Modify: `docs/superpowers/plans/2026-04-14-styling-gap-analysis.md`

The gap-fill plan's SCSS code blocks reference `--marilo-*` tokens. Update them to `--k-*` so implementing agents write correct code.

- [ ] **Step 1: Run the rename in the plan document**

```bash
sed -i 's/--marilo-/--k-/g' docs/superpowers/plans/2026-04-14-styling-gap-analysis.md
```

- [ ] **Step 2: Verify**

```bash
grep -n "\-\-marilo-" docs/superpowers/plans/2026-04-14-styling-gap-analysis.md
```

Expected: zero matches in SCSS code blocks. (Prose text mentioning "marilo" as a word is fine.)

- [ ] **Step 3: Commit**

```bash
git add docs/superpowers/plans/2026-04-14-styling-gap-analysis.md
git commit -m "chore(docs): update styling gap-fill plan to use --k- token names"
```

---

## Phase 5: Final Verification

### Task 6: Full build, test, and spot-check

- [ ] **Step 1: Rebuild all SCSS bundles**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui && cd ../..
cd src/Marilo.Providers.Bootstrap && npm run scss:build:bootstrap && cd ../..
cd src/Marilo.Providers.Material  && npm run scss:build:material  && cd ../..
```

Expected: all three exit 0.

- [ ] **Step 2: dotnet build**

```bash
dotnet build Marilo.slnx
```

Expected: `Build succeeded.` 0 errors.

- [ ] **Step 3: dotnet test**

```bash
dotnet test Marilo.slnx --no-build
```

Expected: 0 failed tests.

- [ ] **Step 4: Browser spot-check**

```bash
dotnet run --project samples/Marilo.Demo
```

Open DevTools → Elements panel → inspect `:root`. Confirm:
- CSS custom properties use `--k-color-primary`, `--k-space-*`, `--k-radius-*` etc.
- No `--marilo-*` custom properties appear
- CSS class names on Marilo components still read `mar-button`, `mar-textbox` etc. (unchanged — this is correct)

- [ ] **Step 5: Commit regenerated compiled CSS**

```bash
git add src/Marilo.Providers.FluentUI/wwwroot/ \
        src/Marilo.Providers.Bootstrap/wwwroot/ \
        src/Marilo.Providers.Material/wwwroot/
git commit -m "chore(styles): regenerate compiled CSS bundles after --k- token rename"
```

---

## Success Criteria

- **PASS:** `grep -r "\-\-marilo-" src/` → zero matches
- **PASS:** All three SCSS builds exit 0
- **PASS:** `dotnet build` exits 0
- **PASS:** `dotnet test` exits 0
- **PASS:** Browser DevTools `:root` shows `--k-*` tokens
- **PASS:** DOM still shows `class="mar-button"` etc. — class names unchanged
- **FAIL:** Any `--marilo-` reference remains in any SCSS file
- **FAIL:** Any `.mar-*` class was accidentally renamed (check `git diff` — `.k-` selectors in SCSS are a mistake)

## Rollback

```bash
git reset --hard <SHA from Task 1 Step 3>
```

---

## Reference

- Token definitions: `src/Marilo.Providers.FluentUI/Styles/foundation/`
- SCSS architecture: `src/Marilo.Providers.FluentUI/STYLES_README.md`
- Telerik Kendo token convention: CSS custom properties use `--k-*` prefix
