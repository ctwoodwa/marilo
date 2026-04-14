---
title: Appearance
page_title: Button Appearance
description: Appearance settings of the Button for Blazor.
slug: button-appearance
tags: marilo,blazor,button,appearance
published: True
position: 35
components: ["button"]
---
# Appearance Settings

The Marilo Button component for Blazor provides several ways to control its appearance. This article discusses the following component parameters:

* [FillMode](#fillmode)
* [Rounded](#rounded)
* [Size](#size)
* [ThemeColor](#themecolor)

You can use all of them together to achieve the desired appearance.

## FillMode

The `FillMode` parameter changes or removes the background and border of the Button. Set the parameter to a member of the static class `Marilo.Blazor.ThemeConstants.Button.FillMode`. When setting the `FillMode` parameter value inline, start with an `@` character, otherwise the predefined string constant cannot be resolved.

| FillMode Class Member | Actual String Value |
| --- | --- |
| `Solid` | `solid` (default) |
| `Flat` | `flat` |
| `Outline` | `outline` |
| `Link` | `link` |
| `Clear` | `clear` |

>caption Built-in Button FillMode Values

````RAZOR
<MariloButton FillMode="@ThemeConstants.Button.FillMode.Clear">Clear</MariloButton>
<MariloButton FillMode="@ThemeConstants.Button.FillMode.Flat">Flat</MariloButton>
<MariloButton FillMode="@ThemeConstants.Button.FillMode.Link">Link</MariloButton>
<MariloButton FillMode="@ThemeConstants.Button.FillMode.Outline">Outline</MariloButton>
<MariloButton FillMode="@ThemeConstants.Button.FillMode.Solid">Solid (Default)</MariloButton>
````

## Rounded

The `Rounded` parameter applies a `border-radius` CSS style to the Button to curve the corners. Set the parameter to a member of the static class `Marilo.Blazor.ThemeConstants.Button.Rounded`. When setting the `Rounded` parameter value inline, start with a `@` character, otherwise the predefined string constant cannot be resolved.

| Rounded Class Member | Actual Value |
| --- | --- |
| `Small` | `sm` |
| `Medium` | `md` (default) |
| `Large` | `lg` |
| `Full` | `full` |

>caption Built-in Button Rounded Values

````RAZOR
<MariloButton Rounded="@ThemeConstants.Button.Rounded.Full">Full</MariloButton>
<MariloButton Rounded="@ThemeConstants.Button.Rounded.Large">Large</MariloButton>
<MariloButton Rounded="@ThemeConstants.Button.Rounded.Medium">Medium (Default)</MariloButton>
<MariloButton Rounded="@ThemeConstants.Button.Rounded.Small">Small</MariloButton>
````

## Size

The `Size` parameter affects the Button dimensions, paddings, and font size. Set the parameter to a member of the static class `Marilo.Blazor.ThemeConstants.Button.Size`. When setting the `Size` parameter value inline, start with a `@` character, otherwise the predefined string constant cannot be resolved.

| Size Class Member | Actual Value |
| --- | --- |
| `Small` | `sm` |
| `Medium` | `md` (default) |
| `Large` | `lg` |

>caption Built-in Button Size Values

````RAZOR
<MariloButton Size="@ThemeConstants.Button.Size.Large">Large</MariloButton>
<MariloButton Size="@ThemeConstants.Button.Size.Medium">Medium (Default)</MariloButton>
<MariloButton Size="@ThemeConstants.Button.Size.Small">Small</MariloButton>
````

## ThemeColor

The `ThemeColor` parameter controls the Button background, text, and border color. Set the parameter to a member of the static class `Marilo.Blazor.ThemeConstants.Button.ThemeColor`. When setting the `ThemeColor` parameter value inline, start with a `@` character, otherwise the predefined string constant cannot be resolved.

| ThemeColor Class Member | Actual String Value |
| --- | --- |
| `Base` | `base` (default) |
| `Primary` | `primary` |
| `Secondary` | `secondary` |
| `Tertiary` | `tertiary` |
| `Info` | `info` |
| `Success` | `success` |
| `Warning` | `warning` |
| `Error` | `error` |
| `Dark` | `dark` |
| `Light` | `light` |
| `Inverse` | `inverse` |


>caption Built-in Button ThemeColor Values

````RAZOR
<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Base">Base (Default)</MariloButton>

<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Primary">Primary</MariloButton>
<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Secondary">Secondary</MariloButton>
<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Tertiary">Tertiary</MariloButton>

<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Info">Info</MariloButton>
<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Success">Success</MariloButton>
<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Warning">Warning</MariloButton>
<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Error">Error</MariloButton>

<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Dark">Dark</MariloButton>
<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Light">Light</MariloButton>
<MariloButton ThemeColor="@ThemeConstants.Button.ThemeColor.Inverse">Inverse</MariloButton>
````

@[template](/_contentTemplates/common/themebuilder-section.md#appearance-themebuilder)
