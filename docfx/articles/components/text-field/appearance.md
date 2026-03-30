---
uid: component-textfield-appearance
title: Text Field Appearance
description: Customize MariloTextField with prefix/suffix content, separators, and validation styling.
---

# Text Field Appearance

## Prefix and Suffix

Use the `Prefix` and `Suffix` render fragments to add icons, labels, or action buttons alongside the input:

```razor
<MariloTextField @bind-Value="url" Placeholder="https://example.com">
    <Prefix>
        <MariloIcon Name="globe" Size="IconSize.Small" />
    </Prefix>
    <Suffix>
        <MariloIcon Name="check" Size="IconSize.Small" />
    </Suffix>
</MariloTextField>
```

## Separators

Enable visual dividers between the prefix/suffix and the input:

```razor
<MariloTextField @bind-Value="amount" ShowPrefixSeparator="true">
    <Prefix>$</Prefix>
</MariloTextField>
```

## Validation state

Set `IsInvalid` to apply error styling:

```razor
<MariloTextField @bind-Value="email" IsInvalid="@(!isValid)" Placeholder="Email" />
```

## See Also

- [Text Field Overview](xref:component-textfield-overview)
