# Design Language Reference

**Version:** 1.0
**Status:** Technology-agnostic reference for internal applications

---

## 1. Purpose and Scope

This document defines a unified visual and interaction language for internal applications,
independent of any specific technology stack (Blazor, React, Bootstrap, etc.).
It ensures that products feel cohesive and professional while allowing engineering teams
to choose implementation details.

This reference covers:
- Visual foundations (colors, typography, spacing, elevation, iconography).
- Shell and layout patterns (Explorer, Workspace, Admin Hub).
- Core component behavior (cards, tables, forms, buttons, pills, charts).
- Governance and extension rules for products and tech stacks.

Stack-specific details belong in separate implementation guides that consume these rules.

---

## 2. Design Philosophy

Optimized for data-rich, task-oriented tools used by experts.

- System first, stack second - the visual language is defined once, implemented per stack.
- Consistent shell - same top bar, navigation, breadcrumb, and page structure across apps.
- Medium density - more content per screen than consumer UIs, less cramped than legacy tools.
- Low noise - neutral palette, subtle borders, and whitespace over heavy shadows.
- Progressive disclosure - defaults visible; advanced options behind expanders or panels.
- Token-driven - colors, spacing, typography, and radii defined as tokens, not literals.

---

## 3. Visual Foundations (Tokens)

### 3.1 Color Tokens

| Token | Purpose |
|---|---|
| color-bg-page | Page background |
| color-bg-surface | Card, panel, dialog backgrounds |
| color-border-subtle | Card borders, table dividers, separators |
| color-text-primary | Main text and headings |
| color-text-muted | Secondary text, labels, helper copy |
| color-accent | Primary calls to action, links, focused states |
| color-success | Success states and positive indicators |
| color-warning | Cautionary states, soft alerts |
| color-error | Errors and critical alerts |

Rules:
- Use semantic tokens in code, not hard-coded hex values.
- Reuse status colors consistently across icons, pills, and charts.
- Prefer neutral surfaces; use accent colors sparingly for emphasis.

### 3.2 Typography Tokens

| Token | Purpose |
|---|---|
| font-family-base | Primary UI font stack |
| font-size-h1/h2/h3 | Heading sizes |
| font-size-body | Body and form input text |
| font-size-caption | Secondary text and helper messages |
| font-weight-regular | Default weight |
| font-weight-semibold | Emphasized labels and headings |

Hierarchy: H1 = page title, H2 = section, H3 = subsection, Body = content, Caption = secondary.
One H1 per screen. Do not skip heading levels. Labels are short and descriptive.

### 3.3 Spacing Scale (4px base)

| Token | Value |
|---|---|
| space-1 | 4px |
| space-2 | 8px |
| space-3 | 12px |
| space-4 | 16px |
| space-5 | 24px |
| space-6 | 32px |

Rules: Use spacing tokens, not arbitrary pixel values. Vertical rhythm in multiples of 4px.

### 3.4 Radii, Borders, Elevation

| Token | Use |
|---|---|
| radius-sm | Small controls (2px) |
| radius-md | Cards and panels (4px) |
| radius-lg | Larger surfaces or callouts (8px) |
| shadow-subtle | Cards and panels |
| shadow-modal | Dialogs and popovers |

### 3.5 Iconography

- Brand/story icons: hero areas, dashboards, explanatory content.
- UI system icons: navigation, buttons, controls. Single consistent style throughout the app.
- Default color: color-text-muted. Active/primary: color-accent.
- Sizes: 16px inline, 20px buttons/toolbars, 24px navigation.

---

## 7. Token and Utility Conventions

Naming rules:
- Colors: color-*
- Spacing: space-*
- Typography: font-size-*, font-weight-*, line-height-*
- Radii: radius-*
- Shadows: shadow-*

Every implementation provides: single source of truth for tokens, utility layer for layout/spacing/text.
The exact mechanism (SCSS, CSS variables, JS theme objects) is stack-specific. Tokens are shared.

---

## 8. Theming (Light and Dark)

- One set of semantic tokens (color-bg-page, color-text-primary, etc.).
- Separate light and dark value sets for those tokens.
- Runtime switch via CSS variables under data-theme=light / dark.
- Layout, spacing, and component structure identical between themes.
- Contrast ratios must meet accessibility requirements in each theme.

---

## 9. Governance, Versioning, and Extensions

- A small design/UX group owns this reference.
- Changes to tokens, core patterns, or interaction rules require review.
- Product teams propose additions with rationale and examples.
- Each stack implementation states which version it targets.
- Products may define additional tokens and component variants as long as they
  do not conflict with this system and remain visually coherent.

---

## 10. Quick Reference

| Category | Defined here | Defined in stack guides |
|---|---|---|
| Colors | Semantic tokens and usage rules | Mapping to theme variables / CSS vars |
| Typography | Roles, hierarchy, tone | Concrete font stacks, component defaults |
| Spacing | 4px scale and usage | Layout utilities and helpers |
| Shell patterns | Top bar, navigation, layouts | Layout components and routing |
| Components | Visual and behavioral expectations | Concrete widgets (grid, form, chart) |
| Theming | Light/dark token model | Theme switch implementation per stack |
| Governance | Ownership, versioning, extensions | Local adoption and review practices |