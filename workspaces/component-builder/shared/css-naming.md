# CSS Naming Conventions

Authoritative reference for how Marilo CSS classes are named. Used by CSS provider implementations and SCSS files.

---

## Prefix

All Marilo classes use the `mar-` prefix to avoid collisions with third-party CSS.

---

## BEM-Like Structure

| Part | Pattern | Example |
|------|---------|---------|
| Block | `mar-[component]` | `mar-button` |
| Element | `mar-[component]__[element]` | `mar-button__icon` |
| Modifier | `mar-[component]--[modifier]` | `mar-button--primary` |
| State | `mar-[component]--[state]` | `mar-button--disabled` |
| Size | `mar-[component]--[size]` | `mar-button--sm` |

---

## Provider Method Return Values

FluentUI provider returns `mar-` prefixed classes:
```
mar-button mar-button--primary mar-button--md
```

Bootstrap provider returns a mix of Bootstrap utility classes and `mar-` bridge classes:
```
btn btn-primary btn-md mar-button
```

Bridge SCSS files in the Bootstrap provider map `mar-[component]` classes to Bootstrap equivalents where possible and add custom rules where Bootstrap has no match.

---

## SCSS File Naming

| Provider | File Pattern | Import Target |
|----------|-------------|---------------|
| FluentUI | `_[component].scss` | `marilo-fluentui.scss` |
| Bootstrap | `_bridge-[component].scss` | `marilo-bootstrap.scss` |

---

## Design Tokens

FluentUI styles reference design tokens (CSS custom properties) rather than hardcoded values. Token naming follows the pattern `--mar-[category]-[property]` (e.g., `--mar-color-primary`, `--mar-spacing-md`).

Bootstrap styles reference Bootstrap variables and tokens where available (`$primary`, `$spacer`, etc.) and fall back to `mar-` tokens for Marilo-specific values.
