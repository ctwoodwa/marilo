---
uid: globalization-overview
title: Globalization Overview
description: Internationalization and localization support in Marilo components.
---

# Globalization Overview

Marilo components respect the current .NET culture for formatting and support localization of built-in labels. This article describes how culture settings flow through the component library and how to customize localized text.

## Culture-Aware Formatting

Marilo's input and display components read `CultureInfo.CurrentCulture` to format and parse values. No additional configuration is required beyond setting the culture at the application level.

### Number Formatting

`MariloNumericInput` uses culture-specific conventions for:

- **Decimal separator** — comma (`,`) in many European locales, period (`.`) in English locales.
- **Thousands grouping separator** — space or comma depending on the culture.
- **Negative sign** — prefix or parentheses, as defined by the culture's `NumberFormatInfo`.

When the user types in a numeric input, the component parses input using `CultureInfo.CurrentCulture` as the format provider. When displaying a value, it applies the configured `Format` string (e.g., `"N2"`, `"C"`) with the same culture.

To override the culture for a specific component instance, pass a `CultureInfo` to the `Culture` parameter:

```razor
<MariloNumericInput @bind-Value="_price"
                    Format="C"
                    Culture="@(new CultureInfo("fr-FR"))" />
```

### Date and Time Formatting

`MariloDatePicker`, `MariloTimePicker`, and `MariloDateTimePicker` apply `CultureInfo.CurrentCulture` for:

- **Date patterns** — short date (`dd/MM/yyyy` in `en-GB`, `M/d/yyyy` in `en-US`).
- **Time patterns** — 12-hour vs 24-hour clock, AM/PM designators.
- **First day of week** — the calendar grid's starting column is derived from `CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek`.
- **Month and day names** — displayed in the culture's language.

To display a date picker in a specific culture regardless of the application culture:

```razor
<MariloDatePicker @bind-Value="_date"
                  Culture="@(new CultureInfo("de-DE"))" />
```

## Localization

Marilo's built-in UI labels (navigation arrows, close buttons, placeholder text, etc.) are expressed as component parameters with sensible English defaults. Override any label directly on the component:

```razor
<MariloDatePicker @bind-Value="_date"
                  PreviousMonthLabel="Mois précédent"
                  NextMonthLabel="Mois suivant"
                  TodayButtonLabel="Aujourd'hui"
                  ClearButtonLabel="Effacer" />
```

```razor
<MariloDialog Title="Confirmer" CloseButtonAriaLabel="Fermer la boîte de dialogue">
    @ChildContent
</MariloDialog>
```

There is no satellite resource assembly or `.resx` file system for Marilo labels in the current release. All localization is done through parameters. If your application targets multiple languages, create a helper or wrapper component that injects translated strings from your own localization infrastructure (e.g., `IStringLocalizer<T>` from `Microsoft.Extensions.Localization`).

## Setting Application Culture

For Blazor Server, set the culture in `Program.cs` or from user preference stored in a cookie:

```csharp
app.UseRequestLocalization(options =>
{
    var supportedCultures = new[] { "en-US", "fr-FR", "de-DE", "ar-SA" };
    options.SetDefaultCulture("en-US")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});
```

For Blazor WebAssembly, set the culture in `Program.cs` before building the host:

```csharp
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("fr-FR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("fr-FR");
```

## See Also

- [RTL Support](xref:globalization-rtl)
