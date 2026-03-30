---
title: Appearance
page_title: AppBar Appearance
description: Appearance settings of the AppBar for Blazor.
slug: appbar-appearance
tags: marilo,blazor,appbar,navbar,appearance
published: True
position: 35
components: ["appbar"]
---
# Appearance Settings

This article outlines the available AppBar parameters, which control its appearance.

## ThemeColor

You can change the color of the AppBar by setting the `ThemeColor` parameter to a member of the `Marilo.Blazor.ThemeConstants.AppBar.ThemeColor` class:

| Class members | Manual declarations |
|---------------|--------|
| `Base` | `base`   |
| `Primary` | `primary`|
| `Secondary` | `secondary`|
| `Tertiary` | `tertiary`|
| `Info` | `info`   |
| `Success` | `success`|
| `Warning` | `warning`|
| `Error` | `error`  |
| `Dark` | `dark`   |
| `Light` | `light`  |
| `Inverse` | `inverse`|

>caption The built-in AppBar colors

````RAZOR
<MariloDropDownList Data="@ThemeColors" @bind-Value="@SelectedColor" Width="150px"></MariloDropDownList>

<MariloAppBar ThemeColor="@SelectedColor">
    <AppBarSection>
        <span>Our Logo</span>
    </AppBarSection>

    <AppBarSpacer Size="25%"></AppBarSpacer>

    <AppBarSection>
        <span>Our Products</span>
    </AppBarSection>

    <AppBarSpacer Size="50px"></AppBarSpacer>

    <AppBarSection>
        <span>Our Mission</span>
    </AppBarSection>

    <AppBarSpacer></AppBarSpacer>

    <AppBarSection>
        <MariloSvgIcon Icon="@SvgIcon.User"></MariloSvgIcon>
    </AppBarSection>

    <AppBarSeparator></AppBarSeparator>

    <AppBarSection>
        <MariloSvgIcon Icon="@SvgIcon.Logout"></MariloSvgIcon>
    </AppBarSection>
</MariloAppBar>

@code {
    private string SelectedColor { get; set; } = "base";

    private List<string> ThemeColors { get; set; } = new List<string>()
    {
        "base",
        "primary",
        "secondary",
        "tertiary",
        "info",
        "success",
        "warning",
        "error",
        "dark",
        "light",
        "inverse"
    };
}
````