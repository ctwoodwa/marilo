---
title: Appearance
page_title: DateTimePicker Appearance
description: Appearance settings of the DateTimePicker for Blazor.
slug: datetimepicker-appearance
tags: marilo,blazor,datetimepicker,appearance
published: True
position: 35
components: ["datetimepicker"]
---
# Appearance Settings

You can control the appearance of the DateTimePicker by setting the following attribute:

* [Size](#size)
* [Rounded](#rounded)
* [FillMode](#fillmode)


## Size

You can increase or decrease the size of the DateTimePicker by setting the `Size` attribute to a member of the `Marilo.Blazor.ThemeConstants.DateTimePicker.Size` class:

| Class members | Manual declarations |
|------------|--------|
|`Small` |`sm`|
|`Medium`|`md`|
|`Large`|`lg`|

>caption The built-in sizes

````RAZOR
@{
    var fields = typeof(Marilo.Blazor.ThemeConstants.DateTimePicker.Size)
        .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
        | System.Reflection.BindingFlags.FlattenHierarchy)
        .Where(field => field.IsLiteral && !field.IsInitOnly).ToList();

    foreach (var field in fields)
    {
        string size = field.GetValue(null).ToString();

        <div style="float:left; margin: 20px;">
            <MariloDateTimePicker @bind-Value="@DateValue" Size="@size"></MariloDateTimePicker>
        </div>
    }
}

@code {
    private DateTime DateValue { get; set; } = DateTime.Now;
}
````

## Rounded

The `Rounded` attribute applies the `border-radius` CSS rule to the DateTimePicker to achieve curving of the edges. You can set it to a member of the `Marilo.Blazor.ThemeConstants.DateTimePicker.Rounded` class:

| Class members | Manual declarations |
|------------|--------|
|`Small` |`sm`|
|`Medium`|`md`|
|`Large`|`lg`|
|`Full`|`full`|

>caption The built-in values of the Rounded attribute

````RAZOR
@* The built-in values of the Rounded attribute.  *@

@{
    var fields = typeof(Marilo.Blazor.ThemeConstants.DateTimePicker.Rounded)
        .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
        | System.Reflection.BindingFlags.FlattenHierarchy)
        .Where(field => field.IsLiteral && !field.IsInitOnly).ToList();

    foreach (var field in fields)
    {
        string rounded = field.GetValue(null).ToString();

        <div style="float:left; margin: 20px;">
            <MariloDateTimePicker @bind-Value="@DateValue" Rounded="@rounded"></MariloDateTimePicker>
        </div>
    }
}

@code {
    private DateTime DateValue { get; set; } = DateTime.Now;
} 
````

## FillMode

The `FillMode` controls how the MariloDateTimePicker is filled. You can set it to a member of the `Marilo.Blazor.ThemeConstants.DateTimePicker.FillMode` class:

| Class members | Result |
|------------|--------|
|`Solid` <br /> default value|`solid`|
|`Flat`|`flat`|
|`Outline`|`outline`|

>caption The built-in Fill modes

````RAZOR
@* These are all built-in fill modes *@

@{
    var fields = typeof(Marilo.Blazor.ThemeConstants.DateTimePicker.FillMode)
        .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
        | System.Reflection.BindingFlags.FlattenHierarchy)
        .Where(field => field.IsLiteral && !field.IsInitOnly).ToList();

    foreach (var field in fields)
    {
        string fillmode = field.GetValue(null).ToString();

        <div style="float:left; margin: 20px;">
            <MariloDateTimePicker @bind-Value="@DateValue" FillMode="@fillmode"></MariloDateTimePicker>
        </div>
    }
}

@code {
    private DateTime DateValue { get; set; } = DateTime.Now;
}
````


## See Also

* [Live Demo: DateTimePicker - Appearance](https://demos.marilo.com/blazor-ui/datetimepicker/appearance)
