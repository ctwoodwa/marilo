---
title: Icons
page_title: ToggleButton - Icon
description: Icons and images in the ToggleButton for Blazor.
slug: togglebutton-icons
tags: marilo,blazor,Toggle,button,icon,sprite,image
published: True
position: 2
components: ["togglebutton"]
---
# ToggleButton Icons

You can put a Font or Svg Icon in the toggle button to illustrate its purpose for your end users.

## Parameters


| Parameter | Type and Default Value | Description |
|---|---|---|
| `Icon`| `object` | Use it to display a [Marilo Font and SVG Icons](slug:common-features-icons). |

The following example shows how to use SVG and Font icons. If you don't add text to the button, the button will center the icon on all sides.


>caption How to use icons in the Marilo Toggle Button

````RAZOR
@* This sample shows how you can use conditional logic to show different icons in the different states.*@

<MariloToggleButton Icon="@( SvgSelected ? SvgIcon.VolumeUp : SvgIcon.VolumeDown )"
                     @bind-Selected="@SvgSelected">Svg Icon</MariloToggleButton>

<MariloToggleButton Icon="@( FontSelected ? FontIcon.VolumeUp : FontIcon.VolumeDown )"
                     @bind-Selected="@FontSelected">Font Icon</MariloToggleButton>


@code {
    private bool SvgSelected { get; set; }

    private bool FontSelected { get; set; }
}
````


## Icon Size

Images and icons should generally be small enough to fit in a line of text - the button is an inline element and is not designed for large images. If you want to use big icon buttons, consider one of the following options:

* Define a `Class` for the Toggle Button that set `height` and `width` styles. The width and height can be set in `px` sufficient to accommodate the icon or to `auto`;
* Attach an `@onclick` handler to a `span` or `img` element instead of using a button;
* Add your own HTML inside the button, something like:
    `<MariloButton><img style="width: 400px; height: 400px;" src="my-icon.svg" />some text</MariloButton>`

## See Also

* [ToggleButton Overview](slug:togglebutton-overview)
