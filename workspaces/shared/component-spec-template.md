# Component Spec Documentation -- Template

Use when scaffolding API specification documentation for a new Marilo component.
Target time to instantiate: under 3 minutes.

## When to Use

Every component built through `component-builder` gets a spec documentation structure. The spec serves as the authoritative contract for what the component's public API should be, and is the primary input for delivery workspace spec reviews.

## How to Instantiate

1. Create directory: `docs/component-specs/{{component-slug}}/`
2. Create the files listed below using the templates
3. Fill all `{{placeholder}}` fields
4. The spec starts as a skeleton; Stage 05 (demos-and-docs) populates detail from the API design

## Folder Structure

```
docs/component-specs/{{component-slug}}/
├── overview.md
├── appearance.md
├── events.md
├── accessibility.md
└── toc.yml
```

Complex components may add feature-area subdirectories:

```
docs/component-specs/{{component-slug}}/
├── overview.md
├── appearance.md
├── events.md
├── accessibility.md
├── toc.yml
├── columns/          (if grid-like)
├── editing/          (if editable)
├── filter/           (if filterable)
├── selection/        (if selectable)
└── templates/        (if templatable)
```

## Placeholder Table

| Placeholder | Fill in | Example |
|-------------|---------|---------|
| `{{component-name}}` | PascalCase display name | `DataGrid` |
| `{{component-slug}}` | lowercase directory slug | `datagrid` |
| `{{component-tag}}` | Razor tag name | `MariloDataGrid` |
| `{{category}}` | component category | `DataDisplay` |
| `{{description}}` | one-line component purpose | `A data grid for tabular data display and editing` |

---

## overview.md Template

```markdown
---
title: {{component-name}} Overview
page_title: {{component-name}} Overview
slug: {{component-slug}}-overview
tags: {{component-slug}}, overview
published: true
position: 0
---

# {{component-name}} Overview

{{description}}

## {{component-name}} Parameters

The `<{{component-tag}}>` component exposes the following parameters:

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
<!-- Populated from API design output -->

## {{component-name}} Events

| Event | Type | Description |
|-------|------|-------------|
<!-- Populated from API design output -->

## {{component-name}} Methods

| Method | Returns | Description |
|--------|---------|-------------|
<!-- Populated from API design output -->

## Basic Usage

\`\`\`razor
<{{component-tag}}>
    <!-- Basic usage example -->
</{{component-tag}}>
\`\`\`

## See Also

- [Appearance](appearance)
- [Events](events)
- [Accessibility](accessibility)
```

---

## appearance.md Template

```markdown
---
title: {{component-name}} Appearance
page_title: {{component-name}} Appearance
slug: {{component-slug}}-appearance
tags: {{component-slug}}, appearance, styling
published: true
position: 1
---

# {{component-name}} Appearance

Customize the look and feel of the {{component-name}} component.

## Size

<!-- Document Size parameter and enum values if applicable -->

## FillMode

<!-- Document FillMode parameter if applicable -->

## Rounded

<!-- Document Rounded parameter if applicable -->

## ThemeColor

<!-- Document theme color options -->

## CSS Classes

The {{component-name}} uses the following CSS class structure:

| Class | Element | Description |
|-------|---------|-------------|
| `mar-{{component-slug}}` | Root | Container element |
<!-- Add component-specific classes -->

## Custom Styling

Use CSS variables to customize the component appearance:

| Variable | Default | Description |
|----------|---------|-------------|
<!-- Add design tokens -->
```

---

## events.md Template

```markdown
---
title: {{component-name}} Events
page_title: {{component-name}} Events
slug: {{component-slug}}-events
tags: {{component-slug}}, events
published: true
position: 2
---

# {{component-name}} Events

Handle user interactions and component lifecycle events.

## Event List

| Event | Type | Bubbles | Description |
|-------|------|---------|-------------|
<!-- Populated from API design output -->

## Event Examples

<!-- One example per event showing handler signature and common usage -->
```

---

## accessibility.md Template

```markdown
---
title: {{component-name}} Accessibility
page_title: {{component-name}} Accessibility
slug: {{component-slug}}-accessibility
tags: {{component-slug}}, accessibility, a11y
published: true
position: 3
---

# {{component-name}} Accessibility

The {{component-name}} component follows WAI-ARIA guidelines.

## ARIA Roles

| Element | Role | Description |
|---------|------|-------------|
<!-- Populated from discovery output accessibility section -->

## Keyboard Navigation

| Key | Action |
|-----|--------|
<!-- Populated from discovery output -->

## Screen Reader Support

<!-- Document screen reader announcements and live regions -->

## Focus Management

<!-- Document focus behavior on open, close, navigation -->
```

---

## toc.yml Template

```yaml
- name: Overview
  href: overview.md
- name: Appearance
  href: appearance.md
- name: Events
  href: events.md
- name: Accessibility
  href: accessibility.md
```
