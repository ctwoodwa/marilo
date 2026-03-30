---
uid: component-card-overview
title: Card
description: The MariloCard component provides a content container composed with CardHeader, CardBody, and CardActions.
---

# Card

## Overview

The `MariloCard` component renders a styled container typically used to group related content and actions. It is a composition component -- you build its structure by nesting `MariloCardHeader`, `MariloCardBody`, and `MariloCardActions` inside it.

## Creating a Card

````razor
<MariloCard>
    <MariloCardHeader>
        <h3>Card Title</h3>
    </MariloCardHeader>
    <MariloCardBody>
        <p>This is the card body content.</p>
    </MariloCardBody>
    <MariloCardActions>
        <MariloButton Variant="ButtonVariant.Primary">Action</MariloButton>
    </MariloCardActions>
</MariloCard>
````

## Features

- **Composable structure** -- Combine `MariloCardHeader`, `MariloCardBody`, and `MariloCardActions` in any order, or use only the parts you need.
- **Provider-driven styling** -- CSS classes are resolved via `IMariloCssProvider.CardClass()`, `CardHeaderClass()`, `CardBodyClass()`, and `CardActionsClass()`.
- **Flexible content** -- Each section accepts arbitrary Razor content through `ChildContent`.

## Parameters

| Name | Type | Default | Description |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | `null` | The content of the card, typically composed of `MariloCardHeader`, `MariloCardBody`, and `MariloCardActions`. |

### MariloCardHeader Parameters

| Name | Type | Default | Description |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | `null` | Header content (title, subtitle, avatar). |

### MariloCardBody Parameters

| Name | Type | Default | Description |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | `null` | Main body content of the card. |

### MariloCardActions Parameters

| Name | Type | Default | Description |
|---|---|---|---|
| `ChildContent` | `RenderFragment?` | `null` | Action buttons or links displayed at the bottom of the card. |

## See Also

- [API Reference](xref:Marilo.Components.DataDisplay.MariloCard)
