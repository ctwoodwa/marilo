---
title: Overview
page_title: FloatLabel Overview
description: Overview of the FloatingLabel for Blazor and its features. What is a floating label and how to use it.
slug: floatinglabel-overview
tags: marilo,blazor,floatinglabel,floating,label,overview
published: True
position: 0
components: ["floatinglabel"]
---
# Blazor Floating Label Overview

The <a href="https://www.marilo.com/blazor-ui/floatinglabel" target="_blank">Blazor FloatingLabel component</a> provides additional features and improved user experience, compared to the standard HTML `label`. The floating label displays on top of empty non-focused components and moves above them on focus.


## Benefits

The Marilo FloatingLabel enhances HTML `label` functionality in the following ways:

* built-in animations
* integration with form validation
* integration with the `placeholder` attribute
* association with non-form elements - for example, the MariloDropDownList component renders as a `<span>`


## Creating Blazor FloatingLabel

1. Use the `MariloFloatingLabel` tag.
1. Set the `Text` parameter of the floating label.
1. Place a [compatible Marilo component](#compatibility) inside the floating label.
1. Set an `Id` to the Marilo component. The `Id` value will automatically render as a `for` attribute for the `<label>` element.

>caption How to use a Floating Label

````RAZOR
<MariloFloatingLabel Text="Your Name">
    <MariloTextBox Id="name" @bind-Value="@Name" />
</MariloFloatingLabel>

@code {
    string Name { get; set; }
}
````


## Compatibility

The FloatingLabel integrates with focusable Marilo Blazor components:

* AutoComplete
* ComboBox
* DateInput
* DatePicker
* DateTimePicker
* DropDownList
* MaskedTextBox
* MultiSelect
* NumericTextBox
* TextArea
* TextBox
* TimePicker

The FloatingLabel does not support third-party components and generic HTML inputs.


## Forms and Validation

The FloatingLabel can change its styles, based on validation state. See [Validation](slug:floatinglabel-validation) for details and example.


## Placeholder Behavior

Labels and placeholders serve similar purpose for the user experience.

If a Marilo component has both a `Placeholder` and a floating label, the behavior is:

* When the floating label is **over the component**, then the placeholder is **not rendered**.
* When the floating label is **outside the component** and the focused component has no value, the placeholder **is visible**.

## Integration with Prefix Adornment


## FloatingLabel Parameters

The following table lists the FloatingLabel parameters. Also check the [FloatingLabel API Reference](slug:Marilo.Blazor.Components.MariloFloatingLabel) for a full list of properties, methods and events.


| Attribute | Type | Description |
| --- | --- | --- |
| `Class` | `string` | Renders additional CSS class to the `span.k-floating-label-container` element, which holds the `<label>` child element. Use `Class` to apply custom styles or [override the theme](slug:themes-override). |
| `Id` | `string` | Renders an `id` attribute to the `label.k-label` element. To improve accessibility, set the same string as floating label `Id` and `AriaLabelledBy` value of the associated Marilo component. |


## Next Steps

* [Review Floating Label integration with form validation](slug:floatinglabel-validation)


## See Also

* [Live Floating Label Demos](https://demos.marilo.com/blazor-ui/floatinglabel/overview)
* [Floating Label API Reference](slug:Marilo.Blazor.Components.MariloFloatingLabel)
