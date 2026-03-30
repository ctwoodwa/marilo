---
title: Animation
page_title: Popup Animation
description: Explore the animation settings of the Popup for Blazor. Discover how to adjust the way the Popup appears and disappears on the screen.
slug: popup-animation
tags: marilo,blazor,popup,animation
published: True
position: 35
components: ["popup"]
---
# Popup Animation

This article outlines how to customize the animations when the Popup displays and hides.

## Type

You can change the way the Popup component shows and hides by setting the `AnimationType` parameter to a member of the `AnimationType` enum:

* `None`
* `Fade`
* `PushUp`
* `PushDown`
* `PushLeft`
* `PushRight`
* `RevealVertical`
* `SlideUp`
* `SlideIn`
* `SlideDown` (default)
* `SlideRight`
* `SlideLeft`
* `ZoomIn`
* `ZoomOut`

See the [example](#example) below to customize the available parameters and see how they affect the Popup component.

## Duration

Set the `AnimationDuration` parameter in milliseconds as `int` to control how long the animation will take until the component is fully displayed.

See the [example](#example) below to customize the available parameters and see how they affect the Popup component.

## Example

The following example lets you experiment with the available settings that control the animation in the Popup. It starts with the default component behavior.

````RAZOR
<label>
    Animation Type:
    <MariloDropDownList Data="@AnimationTypes"
                         Value="@SelectedAnimationType"
                         ValueChanged="@( (AnimationType newValue) => OnDropDownValueChanged(newValue) )"
                         Width="160px" />
</label>
<label>
    Animation Duration:
    <MariloNumericTextBox @bind-Value="@SelectedAnimationDuration"
                           Min="0"
                           Max="7000"
                           Width="100px" />
</label>

<MariloButton OnClick="@(() => PopupRef.Show())">Show Popup</MariloButton>
<MariloButton OnClick="@(() => PopupRef.Hide())">Hide Popup</MariloButton>

<MariloPopup @ref="@PopupRef"
              AnchorSelector=".popup-target"
              AnimationType="@SelectedAnimationType"
              AnimationDuration="@SelectedAnimationDuration"
              Width="300px">
    <div style="padding: 1em; color: #fff; background: #282f89; text-align: center;">
        Marilo Blazor Animation Container
    </div>
</MariloPopup>

<div class="popup-target styled-container">
    
</div>

@code {
    private MariloPopup PopupRef { get; set; }

    private List<AnimationType> AnimationTypes { get; set; }

    private AnimationType SelectedAnimationType { get; set; } = AnimationType.SlideDown;

    private int SelectedAnimationDuration { get; set; } = 300;

    private void OnDropDownValueChanged(AnimationType newAnimationType)
    {
        PopupRef.Hide();

        SelectedAnimationType = newAnimationType;

        PopupRef.Show();
    }

    protected override void OnInitialized()
    {
        AnimationTypes = new List<AnimationType>();

        foreach (AnimationType animation in Enum.GetValues(typeof(AnimationType)))
        {
            AnimationTypes.Add(animation);
        }

        base.OnInitialized();
    }
}

<style>
    .styled-container {
        width: 200px;
        height: 30px;
        background-color: yellowgreen;
        margin-top: 20px;
    }
</style>
````

## See Also

* [Live Popup Demos](https://demos.marilo.com/blazor-ui/popup/overview)
* [Popup API Reference](slug:Marilo.Blazor.Components.MariloPopup)
