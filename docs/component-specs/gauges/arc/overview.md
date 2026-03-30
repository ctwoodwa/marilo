---
title: Overview
page_title: Arc Gauge Overview
description: Overview of the Arc Gauge for Blazor.
slug: arc-gauge-overview
tags: marilo,blazor,arcgauge,arc,gauge,overview
published: True
position: 0
components: ["arcgauge"]
---
# Blazor Arc Gauge Overview

The <a href = "https://www.marilo.com/blazor-ui/arc-gauge" target="_blank">Marilo Arc Gauge for Blazor</a> represents [numerical values](slug:arc-gauge-pointers) on an arc [scale](slug:arc-gauge-scale).

## Creating Blazor Arc Gauge

1. Add the `<MariloArcGauge>` tag.
1. Add an instance of the `<ArcGaugePointer>` to the `<ArcGaugePointers>` collection.
1. Provide a `Value` for the `<ArcGaugePointer>`.
1. (optional) Add a [Center Label Template](slug:arc-gauge-labels#center-template)


````RAZOR
@* Setup a basic arc gauge *@

<MariloArcGauge>
    <ArcGaugePointers>
        <ArcGaugePointer Value="@GaugeValue" />
    </ArcGaugePointers>
</MariloArcGauge>

@code {
    private double GaugeValue { get; set; } = 40;
}
````

## Scale

The scale of the Arc Gauge renders the values of the [pointers](slug:arc-gauge-pointers) and [labels](slug:arc-gauge-labels). See the [Scale](slug:arc-gauge-scale) article for more information on how to customize the scale of the component.

## Pointers

The pointers indicate the values on the scale of the component. See the [Pointers](slug:arc-gauge-pointers) article for more information on how to customize the pointers of the component.

## Labels

The labels are rendered on the scale of the component to give information to the users. See the [Labels](slug:arc-gauge-labels) article for more information on how to customize the labels on the scale of the component.

## Arc Gauge Parameters

| Parameter | Type and Default Value | Description |
| --- | --- | --- |
| `Class` | `string` | Renders a custom CSS class to the `<div class="k-arcgauge">` element. |
| `Width` | `string` | Controls the width of the Arc Gauge. You can read more information in the [Dimensions](slug:common-features/dimensions) article.|
| `Height` | `string` | Controls the height of the Arc Gauge. You can read more information in the [Dimensions](slug:common-features/dimensions) article. |
| `Transitions` | `bool?` | Controls if the Arc Gauge uses animations for its value changes. |
| `RenderAs` | `RenderingMode?` <br /> (`SVG`) | Controls if the gauge renders as `SVG` or `Canvas`. |

## Arc Gauge Reference and Methods
 
| Method | Description |
| --- | --- |
| `Refresh` | Programatically re-render the Arc Gauge. |

>caption Get a reference to the Arc Gauge and use the Refresh method

````RAZOR
@* Change the Width of the component *@

<MariloButton OnClick="@ChangeTheHeight">Change the Width of the component</MariloButton>

<MariloArcGauge @ref="@ArcGaugeRef" Width="@Width">
    <ArcGaugePointers>

        <ArcGaugePointer Value="30" />

    </ArcGaugePointers>
</MariloArcGauge>

@code {
    Marilo.Blazor.Components.MariloArcGauge ArcGaugeRef { get; set; }

    public string Width { get; set; } = "300px";

    private void ChangeTheHeight()
    {
        Width = "450px";

        ArcGaugeRef.Refresh();
    }
}
````

## Next Steps

* [Explore the Arc Gauge Scale](slug:arc-gauge-scale)
* [Learn more about the Arc Gauge Pointers](slug:arc-gauge-pointers)

## See Also

* [Live Demo: Arc Gauge](https://demos.marilo.com/blazor-ui/arcgauge/overview)