# Marilo FluentUI Styling Gap Analysis — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Implement SCSS styles for 17 placeholder component files and fill missing variant/state gaps in partially-implemented components so that all Marilo FluentUI components render visually consistent with the Telerik UI for Blazor design specification.

**Architecture:** All styles live in `src/Marilo.Providers.FluentUI/Styles/components/` — one SCSS partial per component. Every selector targets `.mar-*` CSS class names emitted by `FluentUICssProvider.cs`. All color, spacing, radius, and shadow values must come from `--marilo-*` CSS custom properties defined in the `foundation/` layer. The existing Button (`_button.scss`) and TextField (`_text-field.scss`) are the canonical patterns to follow.

**Tech Stack:** SCSS, CSS custom properties (design tokens), Fluent UI token naming conventions, .NET 10 / Blazor, `npm run scss:build:fluentui` to compile.

---

## Gap Summary

**17 placeholder files** (contain only `// TODO: Placeholder — no FluentUI styles defined yet`):
- Input dropdowns: `_dropdown-list.scss`, `_autocomplete.scss`, `_combo-box.scss`, `_multi-select.scss`, `_search-box.scss`
- Date/time: `_date-picker.scss`, `_color-picker.scss`
- Interactive display: `_slider.scss`, `_rating.scss`, `_progress-circle.scss`
- Feedback/status: `_skeleton.scss`, `_snackbar.scss`, `_alert-strip.scss`, `_data-banner.scss`, `_environment-badge.scss`
- Utility: `_icon.scss`, `_link.scss`

**Variant gaps** in existing components:
- `_text-field.scss`: missing size variants (small/medium/large)
- `_text-area.scss`: near-empty, needs full implementation
- `_dialog.scss`: missing size variants

---

## File Structure

Files to modify:
- `src/Marilo.Providers.FluentUI/Styles/components/_dropdown-list.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_autocomplete.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_combo-box.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_multi-select.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_search-box.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_date-picker.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_color-picker.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_slider.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_rating.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_progress-circle.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_skeleton.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_snackbar.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_alert-strip.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_data-banner.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_environment-badge.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_icon.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_link.scss` — replace placeholder
- `src/Marilo.Providers.FluentUI/Styles/components/_text-field.scss` — add size variants
- `src/Marilo.Providers.FluentUI/Styles/components/_text-area.scss` — full implementation
- `src/Marilo.Providers.FluentUI/Styles/components/_dialog.scss` — add size variants

Verification file (read-only reference):
- `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs` — canonical source of all `.mar-*` class names

---

## Phase 1: Build Validation Baseline

Confirm the SCSS compiler is working and the baseline compiles clean before making any changes.

### Task 1: Verify baseline build

**Files:**
- Read: `src/Marilo.Providers.FluentUI/Styles/marilo-fluentui.scss`

- [ ] **Step 1: Run the SCSS build**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0, no errors. Note any warnings.

- [ ] **Step 2: Run dotnet build**

```bash
dotnet build src/Marilo.Providers.FluentUI/Marilo.Providers.FluentUI.csproj
```

Expected: `Build succeeded.`

- [ ] **Step 3: Commit baseline**

```bash
git add --intent-to-add src/Marilo.Providers.FluentUI/Styles/components/
git status
git commit -m "chore(styles): confirm baseline SCSS build before gap-fill"
```

---

## Phase 2: Dropdown Family (dropdown-list, autocomplete, combo-box, multi-select)

These four components share the same dropdown-with-popup visual pattern. Implement them together for consistency. CSS class sources: `FluentUICssProvider.cs` lines 327–376.

### Task 2: Implement `_dropdown-list.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_dropdown-list.scss`

CSS classes needed (from `FluentUICssProvider`):
- `.mar-dropdownlist` — trigger container
- `.mar-dropdownlist--open` — open state
- `.mar-dropdownlist--disabled` — disabled state
- `.mar-dropdownlist--invalid` — validation error state
- `.mar-dropdownlist__value` — selected value display area (from Razor)
- `.mar-dropdownlist__placeholder` — placeholder text (from Razor)
- `.mar-dropdownlist__filter-input` — filter input (from Razor)
- `.mar-dropdownlist-popup` — popup container
- `.mar-dropdownlist-item` — list item
- `.mar-dropdownlist-item--highlighted` — keyboard-focused item
- `.mar-dropdownlist-item--selected` — currently selected item
- `.mar-dropdownlist__group-header` — group label (from Razor)

- [ ] **Step 1: Write the SCSS**

Replace the file content:

```scss
// =============================================================================
// Component: DropDownList
// =============================================================================

// Trigger (closed state)
.mar-dropdownlist {
    display: inline-flex;
    align-items: center;
    justify-content: space-between;
    position: relative;
    min-width: 200px;
    min-height: 32px;
    padding: 0 10px;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    color: var(--marilo-color-on-background);
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border-strong);
    border-radius: var(--marilo-radius-md);
    cursor: pointer;
    user-select: none;
    transition: border-color var(--marilo-transition-fast);

    &::after {
        content: '';
        display: block;
        flex-shrink: 0;
        width: 12px;
        height: 12px;
        margin-left: var(--marilo-space-sm);
        background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 12 12'%3E%3Cpath d='M2.15 4.65a.5.5 0 0 1 .7 0L6 7.79l3.15-3.14a.5.5 0 0 1 .7.7l-3.5 3.5a.5.5 0 0 1-.7 0l-3.5-3.5a.5.5 0 0 1 0-.7z' fill='%231a1a1a'/%3E%3C/svg%3E");
        background-repeat: no-repeat;
        background-size: contain;
        pointer-events: none;
        transition: transform var(--marilo-transition-fast);
    }
}

.mar-dropdownlist:hover {
    border-color: var(--marilo-color-on-background);
}

.mar-dropdownlist:focus {
    outline: none;
    border-color: var(--marilo-color-primary);
    box-shadow: 0 0 0 1px var(--marilo-color-primary);
}

.mar-dropdownlist--open {
    border-color: var(--marilo-color-primary);
    box-shadow: 0 0 0 1px var(--marilo-color-primary);

    &::after {
        transform: rotate(180deg);
    }
}

.mar-dropdownlist--disabled {
    opacity: var(--disabled-opacity, 0.3);
    cursor: not-allowed;
    pointer-events: none;
}

.mar-dropdownlist--invalid {
    border-color: var(--marilo-color-danger);
}

.mar-dropdownlist__value {
    flex: 1;
    min-width: 0;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.mar-dropdownlist__placeholder {
    color: var(--marilo-color-text-secondary);
}

// Popup
.mar-dropdownlist-popup {
    min-width: 200px;
    max-height: 300px;
    overflow-y: auto;
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border);
    border-radius: var(--marilo-radius-md);
    box-shadow: var(--marilo-shadow-md);
    z-index: var(--marilo-z-dropdown);

    ul {
        list-style: none;
        margin: 0;
        padding: var(--marilo-space-xs) 0;
    }
}

.mar-dropdownlist__filter-input {
    display: block;
    width: 100%;
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    border: none;
    border-bottom: 1px solid var(--marilo-color-border);
    outline: none;
    background: var(--marilo-color-surface);
}

.mar-dropdownlist__group-header {
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    font-size: var(--marilo-font-size-xs);
    font-weight: var(--marilo-font-weight-semibold);
    color: var(--marilo-color-text-secondary);
    text-transform: uppercase;
    letter-spacing: 0.05em;
}

// Items
.mar-dropdownlist-item {
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    color: var(--marilo-color-on-background);
    cursor: pointer;
    transition: background var(--marilo-transition-fast);

    &:hover {
        background: var(--marilo-color-surface);
    }
}

.mar-dropdownlist-item--highlighted {
    background: var(--marilo-color-surface);
}

.mar-dropdownlist-item--selected {
    background: var(--marilo-color-primary-light);
    color: var(--marilo-color-primary);
    font-weight: var(--marilo-font-weight-semibold);
}

.mar-dropdownlist-item--selected.mar-dropdownlist-item--highlighted {
    background: var(--marilo-color-primary-light);
}
```

- [ ] **Step 2: Compile SCSS**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_dropdown-list.scss
git commit -m "feat(styles): implement DropDownList SCSS"
```

---

### Task 3: Implement `_autocomplete.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_autocomplete.scss`

CSS classes needed:
- `.mar-autocomplete` — wraps the text input + popup
- `.mar-autocomplete--open` — open state
- `.mar-autocomplete--disabled` — disabled state
- `.mar-autocomplete--invalid` — validation error state
- `.mar-autocomplete-item` — suggestion item
- `.mar-autocomplete-item--highlighted` — keyboard-focused suggestion
- `.mar-autocomplete-item--selected` — currently chosen suggestion

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: Autocomplete
// =============================================================================

.mar-autocomplete {
    display: inline-flex;
    flex-direction: column;
    position: relative;
    min-width: 200px;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
}

// Trigger input — shares .mar-textbox chrome via the inner input
.mar-autocomplete > .mar-textbox {
    width: 100%;
}

// Popup
.mar-autocomplete__popup {
    position: absolute;
    top: calc(100% + 2px);
    left: 0;
    right: 0;
    max-height: 280px;
    overflow-y: auto;
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border);
    border-radius: var(--marilo-radius-md);
    box-shadow: var(--marilo-shadow-md);
    z-index: var(--marilo-z-dropdown);

    ul {
        list-style: none;
        margin: 0;
        padding: var(--marilo-space-xs) 0;
    }
}

.mar-autocomplete--disabled {
    opacity: var(--disabled-opacity, 0.3);
    pointer-events: none;
}

.mar-autocomplete--invalid > .mar-textbox {
    border-color: var(--marilo-color-danger);
}

// Items
.mar-autocomplete-item {
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    cursor: pointer;
    color: var(--marilo-color-on-background);
    transition: background var(--marilo-transition-fast);

    &:hover {
        background: var(--marilo-color-surface);
    }

    // Highlighted match text within suggestion
    mark {
        background: var(--marilo-color-primary-light);
        color: var(--marilo-color-primary);
        font-weight: var(--marilo-font-weight-semibold);
        border-radius: var(--marilo-radius-sm);
        padding: 0 2px;
    }
}

.mar-autocomplete-item--highlighted {
    background: var(--marilo-color-surface);
}

.mar-autocomplete-item--selected {
    background: var(--marilo-color-primary-light);
    color: var(--marilo-color-primary);
    font-weight: var(--marilo-font-weight-semibold);
}
```

- [ ] **Step 2: Compile SCSS**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_autocomplete.scss
git commit -m "feat(styles): implement Autocomplete SCSS"
```

---

### Task 4: Implement `_combo-box.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_combo-box.scss`

CSS classes needed:
- `.mar-combobox` — editable input + dropdown trigger wrapper
- `.mar-combobox--open` — open state
- `.mar-combobox--disabled` — disabled state
- `.mar-combobox--invalid` — validation error state
- `.mar-combobox-popup` — popup container
- `.mar-combobox-item` — list item
- `.mar-combobox-item--highlighted` — keyboard-focused item
- `.mar-combobox-item--selected` — currently selected item

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: ComboBox (editable DropDownList)
// =============================================================================

.mar-combobox {
    display: inline-flex;
    align-items: center;
    position: relative;
    min-width: 200px;
    min-height: 32px;
    border: 1px solid var(--marilo-color-border-strong);
    border-radius: var(--marilo-radius-md);
    background: var(--marilo-color-background);
    transition: border-color var(--marilo-transition-fast), box-shadow var(--marilo-transition-fast);
    overflow: hidden;
}

.mar-combobox:hover {
    border-color: var(--marilo-color-on-background);
}

.mar-combobox:focus-within {
    border-color: var(--marilo-color-primary);
    box-shadow: 0 0 0 1px var(--marilo-color-primary);
}

.mar-combobox--open {
    border-color: var(--marilo-color-primary);
    box-shadow: 0 0 0 1px var(--marilo-color-primary);
}

.mar-combobox--disabled {
    opacity: var(--disabled-opacity, 0.3);
    cursor: not-allowed;
    pointer-events: none;
}

.mar-combobox--invalid {
    border-color: var(--marilo-color-danger);
}

// Inner input field
.mar-combobox input {
    flex: 1;
    min-width: 0;
    padding: 6px 4px 6px 10px;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    color: var(--marilo-color-on-background);
    background: transparent;
    border: none;
    outline: none;

    &::placeholder {
        color: var(--marilo-color-text-secondary);
    }
}

// Chevron button
.mar-combobox__toggle {
    flex-shrink: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 28px;
    height: 100%;
    border: none;
    background: transparent;
    cursor: pointer;
    color: var(--marilo-color-text-secondary);

    svg, .mar-icon {
        width: 12px;
        height: 12px;
        transition: transform var(--marilo-transition-fast);
    }
}

.mar-combobox--open .mar-combobox__toggle svg,
.mar-combobox--open .mar-combobox__toggle .mar-icon {
    transform: rotate(180deg);
}

// Popup
.mar-combobox-popup {
    min-width: 200px;
    max-height: 280px;
    overflow-y: auto;
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border);
    border-radius: var(--marilo-radius-md);
    box-shadow: var(--marilo-shadow-md);
    z-index: var(--marilo-z-dropdown);

    ul {
        list-style: none;
        margin: 0;
        padding: var(--marilo-space-xs) 0;
    }
}

// Items
.mar-combobox-item {
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    cursor: pointer;
    color: var(--marilo-color-on-background);
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    transition: background var(--marilo-transition-fast);

    &:hover {
        background: var(--marilo-color-surface);
    }
}

.mar-combobox-item--highlighted {
    background: var(--marilo-color-surface);
}

.mar-combobox-item--selected {
    background: var(--marilo-color-primary-light);
    color: var(--marilo-color-primary);
    font-weight: var(--marilo-font-weight-semibold);
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_combo-box.scss
git commit -m "feat(styles): implement ComboBox SCSS"
```

---

### Task 5: Implement `_multi-select.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_multi-select.scss`

CSS classes needed:
- `.mar-multiselect` — tag container + input wrapper
- `.mar-multiselect--open` — open state
- `.mar-multiselect--disabled` — disabled state
- `.mar-multiselect--invalid` — validation error state
- `.mar-multiselect-popup` — popup container
- `.mar-multiselect-item` — list item
- `.mar-multiselect-item--highlighted` — keyboard-focused item
- `.mar-multiselect-item--selected` — checked item
- `.mar-multiselect-tag` — selected value chip/tag

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: MultiSelect
// =============================================================================

.mar-multiselect {
    display: flex;
    flex-wrap: wrap;
    align-items: center;
    gap: var(--marilo-space-xs);
    min-width: 200px;
    min-height: 32px;
    padding: 4px 8px;
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border-strong);
    border-radius: var(--marilo-radius-md);
    cursor: text;
    transition: border-color var(--marilo-transition-fast), box-shadow var(--marilo-transition-fast);
}

.mar-multiselect:hover {
    border-color: var(--marilo-color-on-background);
}

.mar-multiselect:focus-within {
    border-color: var(--marilo-color-primary);
    box-shadow: 0 0 0 1px var(--marilo-color-primary);
}

.mar-multiselect--open {
    border-color: var(--marilo-color-primary);
    box-shadow: 0 0 0 1px var(--marilo-color-primary);
}

.mar-multiselect--disabled {
    opacity: var(--disabled-opacity, 0.3);
    cursor: not-allowed;
    pointer-events: none;
}

.mar-multiselect--invalid {
    border-color: var(--marilo-color-danger);
}

// Inner search/filter input
.mar-multiselect input {
    flex: 1;
    min-width: 80px;
    padding: 2px 4px;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    color: var(--marilo-color-on-background);
    background: transparent;
    border: none;
    outline: none;

    &::placeholder {
        color: var(--marilo-color-text-secondary);
    }
}

// Selected value tags
.mar-multiselect-tag {
    display: inline-flex;
    align-items: center;
    gap: var(--marilo-space-xs);
    padding: 2px 8px;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-sm);
    background: var(--marilo-color-primary-light);
    color: var(--marilo-color-primary);
    border: 1px solid var(--marilo-color-primary);
    border-radius: var(--marilo-radius-full);
    line-height: 1.4;
    max-width: 180px;
    overflow: hidden;

    span {
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
    }

    // Remove button
    button {
        display: flex;
        align-items: center;
        justify-content: center;
        flex-shrink: 0;
        width: 14px;
        height: 14px;
        padding: 0;
        background: transparent;
        border: none;
        cursor: pointer;
        color: var(--marilo-color-primary);
        border-radius: 50%;

        &:hover {
            background: var(--marilo-color-primary);
            color: var(--marilo-color-on-primary);
        }
    }
}

// Popup
.mar-multiselect-popup {
    min-width: 200px;
    max-height: 280px;
    overflow-y: auto;
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border);
    border-radius: var(--marilo-radius-md);
    box-shadow: var(--marilo-shadow-md);
    z-index: var(--marilo-z-dropdown);

    ul {
        list-style: none;
        margin: 0;
        padding: var(--marilo-space-xs) 0;
    }
}

// Items
.mar-multiselect-item {
    display: flex;
    align-items: center;
    gap: var(--marilo-space-sm);
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    cursor: pointer;
    color: var(--marilo-color-on-background);
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    transition: background var(--marilo-transition-fast);

    &:hover {
        background: var(--marilo-color-surface);
    }

    // Checkbox indicator
    &::before {
        content: '';
        flex-shrink: 0;
        width: 16px;
        height: 16px;
        border: 1px solid var(--marilo-color-border-strong);
        border-radius: var(--marilo-radius-sm);
        background: var(--marilo-color-background);
        transition: background var(--marilo-transition-fast), border-color var(--marilo-transition-fast);
    }
}

.mar-multiselect-item--highlighted {
    background: var(--marilo-color-surface);
}

.mar-multiselect-item--selected {
    &::before {
        background: var(--marilo-color-primary);
        border-color: var(--marilo-color-primary);
        background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3E%3Cpath d='M13.78 4.22a.75.75 0 0 1 0 1.06l-7.25 7.25a.75.75 0 0 1-1.06 0L2.22 9.28a.75.75 0 0 1 1.06-1.06L6 10.94l6.72-6.72a.75.75 0 0 1 1.06 0z' fill='%23ffffff'/%3E%3C/svg%3E");
        background-repeat: no-repeat;
        background-size: contain;
    }
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_multi-select.scss
git commit -m "feat(styles): implement MultiSelect SCSS"
```

---

### Task 6: Implement `_search-box.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_search-box.scss`

CSS classes needed (from `FluentUICssProvider.SearchBoxClass()`):
- `.mar-search-box` — wrapper (shares most chrome with `.mar-textbox`)
- Also: `.mar-search-box__icon`, `.mar-search-box__clear` (internal elements from Razor)

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: SearchBox
// =============================================================================

.mar-search-box {
    display: flex;
    align-items: center;
    min-height: 32px;
    border: 1px solid var(--marilo-color-border-strong);
    border-radius: var(--marilo-radius-md);
    background: var(--marilo-color-background);
    transition: border-color var(--marilo-transition-fast), box-shadow var(--marilo-transition-fast);

    &:hover {
        border-color: var(--marilo-color-on-background);
    }

    &:focus-within {
        border-color: var(--marilo-color-primary);
        box-shadow: 0 0 0 1px var(--marilo-color-primary);
    }
}

.mar-search-box__icon {
    display: flex;
    align-items: center;
    padding: 0 8px 0 10px;
    color: var(--marilo-color-text-secondary);
    flex-shrink: 0;
}

.mar-search-box input {
    flex: 1;
    min-width: 0;
    padding: 6px 4px;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    line-height: var(--marilo-line-height-base);
    color: var(--marilo-color-on-background);
    background: transparent;
    border: none;
    outline: none;

    &::placeholder {
        color: var(--marilo-color-text-secondary);
    }
}

.mar-search-box__clear {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-shrink: 0;
    width: 28px;
    height: 100%;
    background: transparent;
    border: none;
    cursor: pointer;
    color: var(--marilo-color-text-secondary);
    opacity: 0;
    transition: opacity var(--marilo-transition-fast), color var(--marilo-transition-fast);

    &:hover {
        color: var(--marilo-color-on-background);
    }
}

// Show clear button when there is content
.mar-search-box:has(input:not(:placeholder-shown)) .mar-search-box__clear {
    opacity: 1;
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_search-box.scss
git commit -m "feat(styles): implement SearchBox SCSS"
```

---

## Phase 3: Date-Time Inputs and Color Picker

### Task 7: Implement `_date-picker.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_date-picker.scss`

CSS classes needed (from `FluentUICssProvider`):
- `.mar-datepicker` — date picker wrapper
- `.mar-timepicker` — time picker wrapper (shares same chrome)
- `.mar-timepicker__popup` — time picker popup
- `.mar-date-range-picker` — date range picker wrapper
- `.mar-date-range-picker__popup` — date range popup
- `.mar-datetime-picker` — date+time picker wrapper
- `.mar-datetime-picker__popup` — datetime popup

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: DatePicker / TimePicker / DateRangePicker / DateTimePicker
// =============================================================================

// Shared input-with-icon chrome
.mar-datepicker,
.mar-timepicker,
.mar-date-range-picker,
.mar-datetime-picker {
    display: inline-flex;
    align-items: center;
    position: relative;
    min-width: 200px;
    min-height: 32px;
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border-strong);
    border-radius: var(--marilo-radius-md);
    transition: border-color var(--marilo-transition-fast), box-shadow var(--marilo-transition-fast);

    &:hover {
        border-color: var(--marilo-color-on-background);
    }

    &:focus-within {
        border-color: var(--marilo-color-primary);
        box-shadow: 0 0 0 1px var(--marilo-color-primary);
    }

    input {
        flex: 1;
        min-width: 0;
        padding: 6px 4px 6px 10px;
        font-family: var(--marilo-font-family);
        font-size: var(--marilo-font-size-base);
        line-height: var(--marilo-line-height-base);
        color: var(--marilo-color-on-background);
        background: transparent;
        border: none;
        outline: none;
        cursor: pointer;

        &::placeholder {
            color: var(--marilo-color-text-secondary);
        }
    }
}

// Calendar/clock toggle button
.mar-datepicker__toggle,
.mar-timepicker__toggle,
.mar-date-range-picker__toggle,
.mar-datetime-picker__toggle {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-shrink: 0;
    width: 32px;
    height: 100%;
    background: transparent;
    border: none;
    border-left: 1px solid var(--marilo-color-border);
    cursor: pointer;
    color: var(--marilo-color-text-secondary);
    transition: background var(--marilo-transition-fast), color var(--marilo-transition-fast);

    &:hover {
        background: var(--marilo-color-surface);
        color: var(--marilo-color-on-background);
    }
}

// Popup containers
.mar-timepicker__popup,
.mar-date-range-picker__popup,
.mar-datetime-picker__popup {
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border);
    border-radius: var(--marilo-radius-md);
    box-shadow: var(--marilo-shadow-lg);
    z-index: var(--marilo-z-dropdown);
    overflow: hidden;
}

// Invalid state
.mar-datepicker--invalid,
.mar-timepicker--invalid,
.mar-date-range-picker--invalid,
.mar-datetime-picker--invalid {
    border-color: var(--marilo-color-danger);
}

// Disabled state
.mar-datepicker--disabled,
.mar-timepicker--disabled,
.mar-date-range-picker--disabled,
.mar-datetime-picker--disabled {
    opacity: var(--disabled-opacity, 0.3);
    pointer-events: none;
}

// Calendar header navigation
.mar-calendar-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    border-bottom: 1px solid var(--marilo-color-border);
}

.mar-calendar-title {
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    font-weight: var(--marilo-font-weight-semibold);
    color: var(--marilo-color-on-background);
    background: transparent;
    border: none;
    cursor: pointer;

    &:hover {
        color: var(--marilo-color-primary);
    }
}

.mar-calendar-nav-btn {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 28px;
    height: 28px;
    background: transparent;
    border: none;
    border-radius: var(--marilo-radius-sm);
    cursor: pointer;
    color: var(--marilo-color-text-secondary);

    &:hover {
        background: var(--marilo-color-surface);
        color: var(--marilo-color-on-background);
    }
}

// Calendar grid
.mar-calendar-grid {
    padding: var(--marilo-space-sm);
}

.mar-calendar-weekdays {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    text-align: center;
    padding: var(--marilo-space-xs) 0;
}

.mar-calendar-weekday {
    font-size: var(--marilo-font-size-xs);
    font-weight: var(--marilo-font-weight-semibold);
    color: var(--marilo-color-text-secondary);
    text-align: center;
}

.mar-calendar-days {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    gap: 2px;
}

.mar-calendar-day {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 32px;
    height: 32px;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-sm);
    border: none;
    border-radius: var(--marilo-radius-sm);
    background: transparent;
    cursor: pointer;
    color: var(--marilo-color-on-background);
    transition: background var(--marilo-transition-fast);

    &:hover {
        background: var(--marilo-color-surface);
    }

    &--today {
        font-weight: var(--marilo-font-weight-semibold);
        color: var(--marilo-color-primary);
    }

    &--selected {
        background: var(--marilo-color-primary);
        color: var(--marilo-color-on-primary);

        &:hover {
            background: var(--marilo-color-primary-hover);
        }
    }

    &--in-range {
        background: var(--marilo-color-primary-light);
        border-radius: 0;
    }

    &--other-month {
        color: var(--marilo-color-text-disabled);
    }

    &--disabled {
        opacity: var(--disabled-opacity, 0.3);
        cursor: not-allowed;
    }
}

// Time picker clock list
.mar-timepicker__list {
    display: flex;
    max-height: 200px;
    overflow-y: auto;
}

.mar-timepicker__column {
    flex: 1;
    display: flex;
    flex-direction: column;
    overflow-y: auto;
    border-right: 1px solid var(--marilo-color-border);

    &:last-child {
        border-right: none;
    }
}

.mar-timepicker__item {
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    text-align: center;
    cursor: pointer;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-sm);
    transition: background var(--marilo-transition-fast);

    &:hover {
        background: var(--marilo-color-surface);
    }

    &--selected {
        background: var(--marilo-color-primary);
        color: var(--marilo-color-on-primary);
        font-weight: var(--marilo-font-weight-semibold);
    }
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_date-picker.scss
git commit -m "feat(styles): implement DatePicker / TimePicker / DateRangePicker SCSS"
```

---

### Task 8: Implement `_color-picker.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_color-picker.scss`

CSS classes needed (from `FluentUICssProvider`):
- `.mar-color-picker` — trigger swatch button
- `.mar-color-picker__popup` — popup container
- `.fui-colorgradient` — gradient picker area
- `.fui-colorpalette` — palette grid
- `.fui-flatcolorpicker` — flat (no popup) color picker

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: ColorPicker / ColorGradient / ColorPalette / FlatColorPicker
// =============================================================================

// Trigger button with color swatch
.mar-color-picker {
    display: inline-flex;
    align-items: center;
    gap: var(--marilo-space-sm);
    min-height: 32px;
    padding: 4px 10px;
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border-strong);
    border-radius: var(--marilo-radius-md);
    cursor: pointer;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    color: var(--marilo-color-on-background);
    transition: border-color var(--marilo-transition-fast);

    &:hover {
        border-color: var(--marilo-color-on-background);
    }

    &:focus-visible {
        outline: none;
        box-shadow: var(--marilo-focus-ring);
    }
}

.mar-color-picker__swatch {
    display: inline-block;
    width: 20px;
    height: 20px;
    border-radius: var(--marilo-radius-sm);
    border: 1px solid rgba(0, 0, 0, 0.2);
    flex-shrink: 0;
    // background-color is set inline by the component
}

// Popup panel
.mar-color-picker__popup {
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border);
    border-radius: var(--marilo-radius-md);
    box-shadow: var(--marilo-shadow-lg);
    padding: var(--marilo-space-md);
    z-index: var(--marilo-z-dropdown);
}

// Color gradient — 2D saturation/brightness picker
.fui-colorgradient {
    position: relative;
    width: 220px;
    height: 160px;
    border-radius: var(--marilo-radius-sm);
    overflow: hidden;
    cursor: crosshair;
    user-select: none;

    // Gradient layers overlay each other
    .fui-colorgradient__white {
        position: absolute;
        inset: 0;
        background: linear-gradient(to right, #fff 0%, transparent 100%);
    }

    .fui-colorgradient__black {
        position: absolute;
        inset: 0;
        background: linear-gradient(to bottom, transparent 0%, #000 100%);
    }

    .fui-colorgradient__thumb {
        position: absolute;
        width: 12px;
        height: 12px;
        border: 2px solid #fff;
        border-radius: 50%;
        transform: translate(-50%, -50%);
        box-shadow: 0 0 2px rgba(0, 0, 0, 0.5);
        cursor: grab;

        &:active {
            cursor: grabbing;
        }
    }
}

// Hue slider
.fui-colorgradient__hue {
    position: relative;
    height: 12px;
    border-radius: var(--marilo-radius-full);
    background: linear-gradient(to right,
        hsl(0, 100%, 50%),
        hsl(60, 100%, 50%),
        hsl(120, 100%, 50%),
        hsl(180, 100%, 50%),
        hsl(240, 100%, 50%),
        hsl(300, 100%, 50%),
        hsl(360, 100%, 50%)
    );
    cursor: pointer;
    margin-top: var(--marilo-space-sm);
}

// Alpha slider
.fui-colorgradient__alpha {
    position: relative;
    height: 12px;
    border-radius: var(--marilo-radius-full);
    margin-top: var(--marilo-space-sm);
    cursor: pointer;
    // background set inline by component (alpha gradient over current color)
}

// Color palette grid
.fui-colorpalette {
    display: grid;
    grid-template-columns: repeat(auto-fill, 22px);
    gap: 2px;
    padding: var(--marilo-space-sm);
}

.fui-colorpalette__swatch {
    width: 22px;
    height: 22px;
    border-radius: var(--marilo-radius-sm);
    border: 1px solid rgba(0, 0, 0, 0.1);
    cursor: pointer;
    transition: transform var(--marilo-transition-fast);

    &:hover {
        transform: scale(1.2);
        border-color: rgba(0, 0, 0, 0.3);
    }

    &--selected {
        outline: 2px solid var(--marilo-color-primary);
        outline-offset: 1px;
    }
}

// Flat (always-visible) color picker
.fui-flatcolorpicker {
    background: var(--marilo-color-background);
    border: 1px solid var(--marilo-color-border);
    border-radius: var(--marilo-radius-md);
    padding: var(--marilo-space-md);
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_color-picker.scss
git commit -m "feat(styles): implement ColorPicker / ColorGradient / ColorPalette SCSS"
```

---

## Phase 4: Interactive Display (Slider, Rating, Progress Circle)

### Task 9: Implement `_slider.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_slider.scss`

CSS classes needed (from `FluentUICssProvider` + `MariloSlider.razor`):
- `.mar-slider` — horizontal container
- `.mar-slider--horizontal` — explicit horizontal modifier
- `.mar-slider--vertical` — vertical orientation
- `.mar-slider--disabled` — disabled state
- `.mar-slider__btn` — decrement/increment buttons
- `.mar-slider__ticks` — tick marks container
- `.mar-slider__tick` — individual tick mark
- `.mar-slider__label` — label template container

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: Slider / RangeSlider
// =============================================================================

.mar-slider {
    display: flex;
    align-items: center;
    gap: var(--marilo-space-sm);
    position: relative;
    width: 100%;
}

.mar-slider--vertical {
    flex-direction: column;
    width: auto;
    height: 200px;
}

.mar-slider--disabled {
    opacity: var(--disabled-opacity, 0.3);
    pointer-events: none;
}

// The native range input styled as Fluent
.mar-slider input[type="range"] {
    -webkit-appearance: none;
    appearance: none;
    flex: 1;
    height: 4px;
    background: var(--marilo-color-border);
    border-radius: var(--marilo-radius-full);
    outline: none;
    cursor: pointer;

    // Thumb
    &::-webkit-slider-thumb {
        -webkit-appearance: none;
        appearance: none;
        width: 16px;
        height: 16px;
        background: var(--marilo-color-primary);
        border: 2px solid var(--marilo-color-on-primary);
        border-radius: 50%;
        cursor: pointer;
        box-shadow: var(--marilo-shadow-sm);
        transition: background var(--marilo-transition-fast), transform var(--marilo-transition-fast);

        &:hover {
            background: var(--marilo-color-primary-hover);
            transform: scale(1.2);
        }
    }

    &::-moz-range-thumb {
        width: 16px;
        height: 16px;
        background: var(--marilo-color-primary);
        border: 2px solid var(--marilo-color-on-primary);
        border-radius: 50%;
        cursor: pointer;
        box-shadow: var(--marilo-shadow-sm);
        transition: background var(--marilo-transition-fast), transform var(--marilo-transition-fast);
    }

    // Track fill (Webkit)
    &::-webkit-slider-runnable-track {
        height: 4px;
        background: var(--marilo-color-border);
        border-radius: var(--marilo-radius-full);
    }

    &::-moz-range-track {
        height: 4px;
        background: var(--marilo-color-border);
        border-radius: var(--marilo-radius-full);
    }

    &:focus-visible::-webkit-slider-thumb {
        box-shadow: var(--marilo-focus-ring);
    }
}

// Vertical range input
.mar-slider--vertical input[type="range"] {
    writing-mode: vertical-lr;
    direction: rtl;
    width: 4px;
    height: 100%;
}

// Decrement / Increment buttons
.mar-slider__btn {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 24px;
    height: 24px;
    background: var(--marilo-color-surface);
    border: 1px solid var(--marilo-color-border-strong);
    border-radius: var(--marilo-radius-sm);
    cursor: pointer;
    font-size: var(--marilo-font-size-base);
    font-weight: var(--marilo-font-weight-semibold);
    color: var(--marilo-color-on-background);
    flex-shrink: 0;
    transition: background var(--marilo-transition-fast);

    &:hover {
        background: var(--marilo-color-surface-hover);
    }

    &:disabled {
        opacity: 0.3;
        cursor: not-allowed;
    }
}

// Tick marks
.mar-slider__ticks {
    position: absolute;
    bottom: -16px;
    left: 0;
    right: 0;
    height: 8px;
}

.mar-slider__tick {
    position: absolute;
    width: 1px;
    height: 6px;
    background: var(--marilo-color-border-strong);
    transform: translateX(-50%);
}

// Label
.mar-slider__label {
    position: absolute;
    bottom: -28px;
    left: 0;
    right: 0;
    display: flex;
    justify-content: space-between;
    font-size: var(--marilo-font-size-xs);
    color: var(--marilo-color-text-secondary);
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_slider.scss
git commit -m "feat(styles): implement Slider SCSS"
```

---

### Task 10: Implement `_rating.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_rating.scss`

CSS classes needed (from `FluentUICssProvider.RatingClass()`):
- `.mar-rating` — star container
- `.mar-rating__star` — individual star (from Razor markup)
- `.mar-rating__star--filled` — filled star
- `.mar-rating__star--half` — half-filled star

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: Rating
// =============================================================================

.mar-rating {
    display: inline-flex;
    align-items: center;
    gap: 2px;
    line-height: 1;
}

.mar-rating__star {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    color: var(--marilo-color-border-strong);
    font-size: 1.25rem; // default star size
    transition: color var(--marilo-transition-fast), transform var(--marilo-transition-fast);
    user-select: none;

    &:hover {
        transform: scale(1.1);
    }

    svg {
        width: 1.25em;
        height: 1.25em;
    }
}

.mar-rating__star--filled {
    color: var(--marilo-color-warning);
}

.mar-rating__star--half {
    position: relative;
    color: var(--marilo-color-border-strong);

    &::after {
        content: '';
        position: absolute;
        left: 0;
        top: 0;
        width: 50%;
        height: 100%;
        background: var(--marilo-color-warning);
        clip-path: polygon(0 0, 100% 0, 100% 100%, 0 100%);
    }
}

// Disabled
.mar-rating--disabled {
    pointer-events: none;
    opacity: 0.5;
}

// Read-only (display only)
.mar-rating--readonly {
    pointer-events: none;

    .mar-rating__star {
        cursor: default;

        &:hover {
            transform: none;
        }
    }
}

// Size variants
.mar-rating--small .mar-rating__star { font-size: 1rem; }
.mar-rating--large .mar-rating__star { font-size: 1.75rem; }
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_rating.scss
git commit -m "feat(styles): implement Rating SCSS"
```

---

### Task 11: Implement `_progress-circle.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_progress-circle.scss`

CSS classes needed (from `FluentUICssProvider`):
- `.mar-progress-circle` — wrapper
- `.mar-progress-circle__track` — background circle (from Razor)
- `.mar-progress-circle__fill` — progress arc (from Razor)
- `.mar-progress-circle__label` — center text (from Razor)

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: ProgressCircle (Circular Progress Bar)
// =============================================================================

.mar-progress-circle {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    position: relative;
}

.mar-progress-circle svg {
    display: block;
    transform: rotate(-90deg); // SVG circles start at 3 o'clock; rotate to start at 12 o'clock
}

.mar-progress-circle__track {
    fill: none;
    stroke: var(--marilo-color-border);
}

.mar-progress-circle__fill {
    fill: none;
    stroke: var(--marilo-color-primary);
    stroke-linecap: round;
    transition: stroke-dashoffset var(--marilo-transition-normal, 0.2s) ease;
}

// Theme color variants
.mar-progress-circle--success .mar-progress-circle__fill { stroke: var(--marilo-color-success); }
.mar-progress-circle--danger  .mar-progress-circle__fill { stroke: var(--marilo-color-danger); }
.mar-progress-circle--warning .mar-progress-circle__fill { stroke: var(--marilo-color-warning); }
.mar-progress-circle--info    .mar-progress-circle__fill { stroke: var(--marilo-color-info); }

// Center label
.mar-progress-circle__label {
    position: absolute;
    inset: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-sm);
    font-weight: var(--marilo-font-weight-semibold);
    color: var(--marilo-color-on-background);
}

// Indeterminate / animated state
.mar-progress-circle--indeterminate .mar-progress-circle__fill {
    animation: mar-progress-circle-spin 1.5s linear infinite;
}

@keyframes mar-progress-circle-spin {
    0%   { stroke-dashoffset: 0; }
    100% { stroke-dashoffset: -283; } // circumference of r=45 circle
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_progress-circle.scss
git commit -m "feat(styles): implement ProgressCircle SCSS"
```

---

## Phase 5: Feedback & Status Components

### Task 12: Implement `_skeleton.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_skeleton.scss`

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: Skeleton (Loading Placeholder)
// =============================================================================

.mar-skeleton {
    display: block;
    background: linear-gradient(
        90deg,
        var(--marilo-color-surface) 25%,
        var(--marilo-color-surface-hover) 50%,
        var(--marilo-color-surface) 75%
    );
    background-size: 200% 100%;
    border-radius: var(--marilo-radius-sm);
    animation: mar-skeleton-shimmer 1.5s ease-in-out infinite;
}

@keyframes mar-skeleton-shimmer {
    0%   { background-position: 200% 0; }
    100% { background-position: -200% 0; }
}

// Shape variants
.mar-skeleton--text {
    height: var(--marilo-line-height-base, 1em);
    border-radius: var(--marilo-radius-sm);
}

.mar-skeleton--circle {
    border-radius: 50%;
}

.mar-skeleton--rect {
    border-radius: var(--marilo-radius-md);
}

// Size helpers (width/height set inline by component)
.mar-skeleton--wave {
    // wave animation already applied via keyframes above
}

.mar-skeleton--pulse {
    animation: mar-skeleton-pulse 1.5s ease-in-out infinite;
    background: var(--marilo-color-surface);
}

@keyframes mar-skeleton-pulse {
    0%, 100% { opacity: 1; }
    50%       { opacity: 0.4; }
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_skeleton.scss
git commit -m "feat(styles): implement Skeleton SCSS"
```

---

### Task 13: Implement `_snackbar.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_snackbar.scss`

CSS classes needed:
- `.mar-snackbar` — single snackbar/toast notification
- `.mar-snackbar--success`, `--warning`, `--danger`, `--info` — severity variants
- `.mar-snackbar__message` — message text (from Razor)
- `.mar-snackbar__action` — action button (from Razor)
- `.mar-snackbar__close` — close button (from Razor)

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: Snackbar / Toast Notification
// =============================================================================

.mar-snackbar {
    display: flex;
    align-items: center;
    gap: var(--marilo-space-md);
    min-width: 280px;
    max-width: 480px;
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    background: var(--marilo-color-on-background);
    color: var(--marilo-color-background);
    border-radius: var(--marilo-radius-md);
    box-shadow: var(--marilo-shadow-lg);
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    animation: mar-snackbar-enter 0.2s ease-out;
}

@keyframes mar-snackbar-enter {
    from {
        opacity: 0;
        transform: translateY(8px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}

// Severity variants
.mar-snackbar--success {
    background: var(--marilo-color-success);
    color: var(--marilo-color-on-success);
}

.mar-snackbar--warning {
    background: var(--marilo-color-warning);
    color: var(--marilo-color-on-warning);
}

.mar-snackbar--danger {
    background: var(--marilo-color-danger);
    color: var(--marilo-color-on-danger);
}

.mar-snackbar--info {
    background: var(--marilo-color-info);
    color: var(--marilo-color-on-info);
}

.mar-snackbar__icon {
    display: flex;
    align-items: center;
    flex-shrink: 0;
    font-size: 1.1em;
}

.mar-snackbar__message {
    flex: 1;
    line-height: var(--marilo-line-height-base);
}

.mar-snackbar__action {
    flex-shrink: 0;
    background: transparent;
    border: 1px solid currentColor;
    border-radius: var(--marilo-radius-sm);
    padding: 2px 10px;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-sm);
    font-weight: var(--marilo-font-weight-semibold);
    color: inherit;
    cursor: pointer;
    transition: background var(--marilo-transition-fast);

    &:hover {
        background: rgba(255, 255, 255, 0.15);
    }
}

.mar-snackbar__close {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-shrink: 0;
    width: 24px;
    height: 24px;
    background: transparent;
    border: none;
    border-radius: var(--marilo-radius-sm);
    cursor: pointer;
    color: inherit;
    opacity: 0.7;
    transition: opacity var(--marilo-transition-fast), background var(--marilo-transition-fast);

    &:hover {
        opacity: 1;
        background: rgba(255, 255, 255, 0.15);
    }
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_snackbar.scss
git commit -m "feat(styles): implement Snackbar SCSS"
```

---

### Task 14: Implement `_alert-strip.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_alert-strip.scss`

CSS classes needed (from Razor + CssProvider):
- `.mar-alert` / `.mar-alert-strip` — inline alert bar
- `.mar-alert--success`, `--warning`, `--danger`, `--info` — severity variants
- `.mar-alert__icon`, `.mar-alert__message`, `.mar-alert__close` — sub-elements

- [ ] **Step 1: Write the SCSS**

```scss
// =============================================================================
// Component: Alert / AlertStrip
// =============================================================================

.mar-alert,
.mar-alert-strip {
    display: flex;
    align-items: flex-start;
    gap: var(--marilo-space-sm);
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    border-radius: var(--marilo-radius-md);
    border-left: 4px solid currentColor;
    background: var(--marilo-color-surface);
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-base);
    color: var(--marilo-color-on-background);
}

// Alert strip goes full-width with no border-radius
.mar-alert-strip {
    border-radius: 0;
    border-left-width: 0;
    border-bottom: 1px solid currentColor;
}

// Severity variants
.mar-alert--success,
.mar-alert-strip--success {
    color: var(--marilo-color-success);
    background: var(--marilo-color-success-light);
    border-color: var(--marilo-color-success);
}

.mar-alert--warning,
.mar-alert-strip--warning {
    color: #986400;
    background: var(--marilo-color-warning-light);
    border-color: var(--marilo-color-warning);
}

.mar-alert--danger,
.mar-alert-strip--danger {
    color: var(--marilo-color-danger);
    background: var(--marilo-color-danger-light);
    border-color: var(--marilo-color-danger);
}

.mar-alert--info,
.mar-alert-strip--info {
    color: var(--marilo-color-info);
    background: var(--marilo-color-info-light);
    border-color: var(--marilo-color-info);
}

.mar-alert__icon,
.mar-alert-strip__icon {
    flex-shrink: 0;
    margin-top: 1px;
    font-size: 1em;
}

.mar-alert__message,
.mar-alert-strip__message {
    flex: 1;
    line-height: var(--marilo-line-height-base);
}

.mar-alert__close,
.mar-alert-strip__close {
    flex-shrink: 0;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 20px;
    height: 20px;
    background: transparent;
    border: none;
    cursor: pointer;
    color: inherit;
    opacity: 0.7;
    border-radius: var(--marilo-radius-sm);
    transition: opacity var(--marilo-transition-fast), background var(--marilo-transition-fast);

    &:hover {
        opacity: 1;
        background: rgba(0, 0, 0, 0.08);
    }
}
```

- [ ] **Step 2: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 3: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_alert-strip.scss
git commit -m "feat(styles): implement Alert / AlertStrip SCSS"
```

---

### Task 15: Implement `_data-banner.scss`, `_environment-badge.scss`, `_icon.scss`, `_link.scss`

These are smaller utility components — handle them together in one commit per file.

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_data-banner.scss`
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_environment-badge.scss`
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_icon.scss`
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_link.scss`

CSS classes needed:
- `.mar-data-banner`, `.mar-data-banner__label`, `.mar-data-banner__value` — labeled data display
- `.mar-env-badge`, `.mar-env-badge--development`, `--staging`, `--production` — environment indicator
- `.mar-icon` — icon wrapper sizing
- `.mar-link`, `.mar-link--visited`, `.mar-link--external` — hyperlink (note: base `.mar-link` is also defined in `_button.scss`; define ONLY the additional link-specific modifiers here to avoid duplication)

- [ ] **Step 1: Write `_data-banner.scss`**

```scss
// =============================================================================
// Component: DataBanner
// =============================================================================

.mar-data-banner {
    display: inline-flex;
    flex-direction: column;
    gap: 2px;
    padding: var(--marilo-space-sm) var(--marilo-space-md);
    background: var(--marilo-color-surface);
    border: 1px solid var(--marilo-color-border);
    border-radius: var(--marilo-radius-md);
    font-family: var(--marilo-font-family);
}

.mar-data-banner__label {
    font-size: var(--marilo-font-size-xs);
    font-weight: var(--marilo-font-weight-semibold);
    color: var(--marilo-color-text-secondary);
    text-transform: uppercase;
    letter-spacing: 0.05em;
}

.mar-data-banner__value {
    font-size: var(--marilo-font-size-md);
    font-weight: var(--marilo-font-weight-semibold);
    color: var(--marilo-color-on-background);
}
```

- [ ] **Step 2: Write `_environment-badge.scss`**

```scss
// =============================================================================
// Component: EnvironmentBadge
// =============================================================================

.mar-env-badge {
    display: inline-flex;
    align-items: center;
    gap: var(--marilo-space-xs);
    padding: 2px 10px;
    font-family: var(--marilo-font-family);
    font-size: var(--marilo-font-size-xs);
    font-weight: var(--marilo-font-weight-semibold);
    text-transform: uppercase;
    letter-spacing: 0.06em;
    border-radius: var(--marilo-radius-full);
    border: 1px solid transparent;
}

.mar-env-badge--development {
    background: var(--marilo-color-info-light);
    color: var(--marilo-color-info);
    border-color: var(--marilo-color-info);
}

.mar-env-badge--staging {
    background: var(--marilo-color-warning-light);
    color: #986400;
    border-color: var(--marilo-color-warning);
}

.mar-env-badge--production {
    background: var(--marilo-color-danger-light);
    color: var(--marilo-color-danger);
    border-color: var(--marilo-color-danger);
}

.mar-env-badge--local {
    background: var(--marilo-color-surface);
    color: var(--marilo-color-text-secondary);
    border-color: var(--marilo-color-border);
}
```

- [ ] **Step 3: Write `_icon.scss`**

```scss
// =============================================================================
// Component: Icon
// =============================================================================

.mar-icon {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    flex-shrink: 0;
    // width/height inherit from font-size or inline style
    vertical-align: middle;

    svg {
        width: 1em;
        height: 1em;
        fill: currentColor;
    }
}

// Size variants
.mar-icon--xs { font-size: 0.75rem; }
.mar-icon--sm { font-size: 1rem; }
.mar-icon--md { font-size: 1.25rem; }
.mar-icon--lg { font-size: 1.5rem; }
.mar-icon--xl { font-size: 2rem; }
```

- [ ] **Step 4: Write `_link.scss`**

```scss
// =============================================================================
// Component: Link
// Note: Base .mar-link styles live in _button.scss.
// This file adds link-specific modifier variants only.
// =============================================================================

.mar-link--visited {
    color: var(--marilo-color-primary-active);
}

.mar-link--external::after {
    content: '';
    display: inline-block;
    width: 0.75em;
    height: 0.75em;
    margin-left: 0.2em;
    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3E%3Cpath d='M8.636 3.5a.5.5 0 0 0-.5-.5H1.5A1.5 1.5 0 0 0 0 4.5v10A1.5 1.5 0 0 0 1.5 16h10a1.5 1.5 0 0 0 1.5-1.5V7.864a.5.5 0 0 0-1 0V14.5a.5.5 0 0 1-.5.5h-10a.5.5 0 0 1-.5-.5v-10a.5.5 0 0 1 .5-.5h6.636a.5.5 0 0 0 .5-.5z' fill='currentColor'/%3E%3Cpath d='M16 .5a.5.5 0 0 0-.5-.5h-5a.5.5 0 0 0 0 1h3.793L6.146 9.146a.5.5 0 1 0 .708.708L15 1.707V5.5a.5.5 0 0 0 1 0v-5z' fill='currentColor'/%3E%3C/svg%3E");
    background-repeat: no-repeat;
    background-size: contain;
    vertical-align: middle;
    opacity: 0.7;
}

.mar-link--disabled {
    opacity: 0.5;
    pointer-events: none;
    text-decoration: none;
}
```

- [ ] **Step 5: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 6: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_data-banner.scss \
        src/Marilo.Providers.FluentUI/Styles/components/_environment-badge.scss \
        src/Marilo.Providers.FluentUI/Styles/components/_icon.scss \
        src/Marilo.Providers.FluentUI/Styles/components/_link.scss
git commit -m "feat(styles): implement DataBanner, EnvironmentBadge, Icon, Link SCSS"
```

---

## Phase 6: Variant Gaps in Existing Components

### Task 16: Add size variants to `_text-field.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_text-field.scss`

The `FluentUICssProvider.TextBoxClass()` does not currently emit size modifier classes, but the gap analysis shows they are needed for Telerik parity. These classes will be emitted once the parameter is added to the provider. Add the SCSS now so the provider change doesn't require a separate SCSS commit.

- [ ] **Step 1: Read the current text-field.scss**

```bash
cat src/Marilo.Providers.FluentUI/Styles/components/_text-field.scss
```

- [ ] **Step 2: Append size variants at the end of the file**

Add after the last existing rule in the file:

```scss
/* === TextField Size Variants === */
/* Applied via mar-textbox--small / --medium / --large modifier classes */

.mar-textbox--small {
    min-height: 24px;

    input {
        padding: 3px 8px;
        font-size: var(--marilo-font-size-sm);
    }

    .mar-textbox__prefix,
    .mar-textbox__suffix {
        padding-left: 6px;
        padding-right: 6px;
        font-size: var(--marilo-font-size-sm);
    }
}

.mar-textbox--medium {
    // Already the default — present for explicit use
    min-height: 32px;

    input {
        padding: 6px 10px;
        font-size: var(--marilo-font-size-base);
    }
}

.mar-textbox--large {
    min-height: 40px;

    input {
        padding: 8px 12px;
        font-size: var(--marilo-font-size-md);
    }

    .mar-textbox__prefix,
    .mar-textbox__suffix {
        font-size: var(--marilo-font-size-md);
    }
}
```

- [ ] **Step 3: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 4: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_text-field.scss
git commit -m "feat(styles): add small/medium/large size variants to TextField SCSS"
```

---

### Task 17: Full implementation of `_text-area.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_text-area.scss`

Read the current file first to know what (if anything) is there. CSS classes: `.mar-textarea`, `.mar-textarea--invalid` (from `FluentUICssProvider.TextAreaClass()`).

- [ ] **Step 1: Read current `_text-area.scss`**

```bash
cat src/Marilo.Providers.FluentUI/Styles/components/_text-area.scss
```

- [ ] **Step 2: Replace file with full implementation**

```scss
// =============================================================================
// Component: TextArea
// =============================================================================

.mar-textarea {
    display: flex;
    flex-direction: column;
    border: 1px solid var(--marilo-color-border-strong);
    border-radius: var(--marilo-radius-md);
    background: var(--marilo-color-background);
    transition: border-color var(--marilo-transition-fast), box-shadow var(--marilo-transition-fast);

    &:hover {
        border-color: var(--marilo-color-on-background);
    }

    &:focus-within {
        border-color: var(--marilo-color-primary);
        box-shadow: 0 0 0 1px var(--marilo-color-primary);
    }

    textarea {
        flex: 1;
        min-width: 0;
        width: 100%;
        padding: 8px 10px;
        font-family: var(--marilo-font-family);
        font-size: var(--marilo-font-size-base);
        line-height: var(--marilo-line-height-base);
        color: var(--marilo-color-on-background);
        background: transparent;
        border: none;
        outline: none;
        resize: vertical;
        min-height: 80px;

        &::placeholder {
            color: var(--marilo-color-text-secondary);
        }
    }
}

.mar-textarea--invalid {
    border-color: var(--marilo-color-danger);
    box-shadow: 0 0 0 1px var(--marilo-color-danger);
}

.mar-textarea--disabled {
    opacity: var(--disabled-opacity, 0.3);
    cursor: not-allowed;
    pointer-events: none;
}

.mar-textarea--no-resize textarea {
    resize: none;
}

// Size variants
.mar-textarea--small textarea {
    padding: 4px 8px;
    font-size: var(--marilo-font-size-sm);
    min-height: 56px;
}

.mar-textarea--large textarea {
    padding: 10px 12px;
    font-size: var(--marilo-font-size-md);
    min-height: 120px;
}

// Character counter
.mar-textarea__counter {
    align-self: flex-end;
    padding: 2px 8px 4px;
    font-size: var(--marilo-font-size-xs);
    color: var(--marilo-color-text-secondary);
}

.mar-textarea__counter--over-limit {
    color: var(--marilo-color-danger);
    font-weight: var(--marilo-font-weight-semibold);
}
```

- [ ] **Step 3: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 4: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_text-area.scss
git commit -m "feat(styles): full TextArea SCSS implementation with size variants"
```

---

### Task 18: Add size variants to `_dialog.scss`

**Files:**
- Modify: `src/Marilo.Providers.FluentUI/Styles/components/_dialog.scss`

- [ ] **Step 1: Read current `_dialog.scss`**

```bash
cat src/Marilo.Providers.FluentUI/Styles/components/_dialog.scss
```

- [ ] **Step 2: Append size variants after the last existing rule**

```scss
/* === Dialog Size Variants === */

.mar-dialog--small  .mar-dialog__panel { max-width: 400px; }
.mar-dialog--medium .mar-dialog__panel { max-width: 600px; }   // default
.mar-dialog--large  .mar-dialog__panel { max-width: 900px; }
.mar-dialog--full   .mar-dialog__panel {
    max-width: 100%;
    width: 100%;
    height: 100%;
    border-radius: 0;
}
```

- [ ] **Step 3: Compile**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0.

- [ ] **Step 4: Commit**

```bash
git add src/Marilo.Providers.FluentUI/Styles/components/_dialog.scss
git commit -m "feat(styles): add small/medium/large/full size variants to Dialog SCSS"
```

---

## Phase 7: Final Build and Demo Verification

### Task 19: Full build and dotnet test

- [ ] **Step 1: Full SCSS build**

```bash
cd src/Marilo.Providers.FluentUI && npm run scss:build:fluentui
```

Expected: exits 0, no errors or unresolved variable warnings.

- [ ] **Step 2: dotnet build (solution level)**

```bash
dotnet build Marilo.slnx
```

Expected: `Build succeeded.` with 0 errors.

- [ ] **Step 3: dotnet test**

```bash
dotnet test Marilo.slnx --no-build
```

Expected: 0 failed tests.

- [ ] **Step 4: Run the demo app and verify visually**

```bash
dotnet run --project samples/Marilo.Demo
```

Navigate to each of the following demo pages and confirm the component renders with correct Fluent UI visual style (not unstyled browser defaults):
- `/inputs/textbox` — confirm size variants
- `/inputs/dropdown` — confirm dropdown-list
- `/inputs/autocomplete` — confirm autocomplete suggestions
- `/inputs/combobox` — confirm combobox
- `/inputs/multiselect` — confirm tags + popup
- `/inputs/search` — confirm search box
- `/inputs/datepicker` — confirm datepicker chrome
- `/inputs/slider` — confirm slider track/thumb
- `/inputs/rating` — confirm star rating
- `/feedback/skeleton` — confirm shimmer animation
- `/feedback/snackbar` — confirm toast notification
- `/feedback/alert` — confirm alert strip

- [ ] **Step 5: Final commit if any last-minute fixes were needed**

```bash
git add -p
git commit -m "fix(styles): styling fixes from demo verification"
```

---

## Success Criteria

- **PASS:** All 17 placeholder SCSS files have real implementations (no `// TODO` lines).
- **PASS:** `npm run scss:build:fluentui` exits 0.
- **PASS:** `dotnet build Marilo.slnx` exits 0.
- **PASS:** `dotnet test` exits 0.
- **PASS:** Each implemented component renders with Fluent UI visual style in the demo app (not unstyled).
- **PASS:** No hardcoded hex color values added — all colors reference `--marilo-*` CSS custom properties.
- **FAIL:** Any component SCSS file still contains only `// TODO: Placeholder`.
- **FAIL:** SCSS build produces errors or unresolved variable warnings.
- **FAIL:** `dotnet build` produces errors.

---

## Reference

- Canonical CSS class names: `src/Marilo.Providers.FluentUI/FluentUICssProvider.cs`
- Design tokens: `src/Marilo.Providers.FluentUI/Styles/foundation/`
- Pattern files: `src/Marilo.Providers.FluentUI/Styles/patterns/`
- Button SCSS (canonical pattern for variants): `src/Marilo.Providers.FluentUI/Styles/components/_button.scss`
- TextField SCSS (canonical pattern for inputs): `src/Marilo.Providers.FluentUI/Styles/components/_text-field.scss`
- STYLES_README: `src/Marilo.Providers.FluentUI/STYLES_README.md`
