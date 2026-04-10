---
uid: common-features-loading-indicators
title: Loading Indicators
description: Using spinners, skeletons, and progress indicators in Marilo applications.
---

# Loading Indicators

Marilo provides a set of components for communicating async work and content loading state to users: spinners, skeleton placeholders, and progress indicators. Data components such as DataGrid also have built-in loading states controlled through a `Loading` parameter.

## MariloSpinner

`MariloSpinner` is an animated circular indicator for short, indeterminate async operations such as form submission or data refresh.

```razor
@if (isLoading)
{
    <MariloSpinner Size="SpinnerSize.Medium" />
}
```

### Parameters

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Size` | `SpinnerSize` | `SpinnerSize.Medium` | Predefined size of the spinner. |
| `Color` | `string?` | `null` | CSS color for the spinner arc. Defaults to the primary color token. |
| `Label` | `string?` | `null` | Screen-reader text. Defaults to `"Loading..."`. |
| `Class` | `string?` | `null` | Additional CSS classes. |
| `Style` | `string?` | `null` | Inline styles. |

### Sizes

| Value | Approximate diameter |
|---|---|
| `SpinnerSize.Small` | 16 px |
| `SpinnerSize.Medium` | 24 px |
| `SpinnerSize.Large` | 36 px |

### Example: Button with Loading State

```razor
<MariloButton OnClick="SaveAsync" Disabled="@isSaving">
    @if (isSaving)
    {
        <MariloSpinner Size="SpinnerSize.Small" />
        <span>Saving...</span>
    }
    else
    {
        <span>Save</span>
    }
</MariloButton>

@code {
    private bool isSaving;

    private async Task SaveAsync()
    {
        isSaving = true;
        await DataService.SaveAsync(model);
        isSaving = false;
    }
}
```

## MariloSkeleton

`MariloSkeleton` renders an animated shimmer placeholder while content is loading. Use it to match the approximate shape of the content that will appear, reducing perceived wait time.

```razor
@if (isLoading)
{
    <MariloSkeleton Variant="SkeletonVariant.Text" Width="200px" />
    <MariloSkeleton Variant="SkeletonVariant.Text" Width="160px" />
    <MariloSkeleton Variant="SkeletonVariant.Rectangle" Width="100%" Height="120px" />
}
else
{
    <h2>@article.Title</h2>
    <p>@article.Subtitle</p>
    <img src="@article.ThumbnailUrl" />
}
```

### Parameters

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Variant` | `SkeletonVariant` | `SkeletonVariant.Text` | Shape of the placeholder. |
| `Width` | `string?` | `"100%"` | CSS width. |
| `Height` | `string?` | `null` | CSS height. Defaults to a value appropriate for the variant. |
| `Animated` | `bool` | `true` | Whether the shimmer animation plays. |
| `Class` | `string?` | `null` | Additional CSS classes. |
| `Style` | `string?` | `null` | Inline styles. |

### Variants

| Value | Use for |
|---|---|
| `SkeletonVariant.Text` | Single lines of text. Height defaults to the current line-height. |
| `SkeletonVariant.Circle` | Avatars, icons, circular badges. Width and height should match. |
| `SkeletonVariant.Rectangle` | Images, cards, banners, or any rectangular block content. |

### Example: Card Skeleton

```razor
<MariloCard Style="padding: 16px; display: flex; gap: 12px;">
    <MariloSkeleton Variant="SkeletonVariant.Circle" Width="48px" Height="48px" />
    <div style="flex: 1; display: flex; flex-direction: column; gap: 8px;">
        <MariloSkeleton Variant="SkeletonVariant.Text" Width="60%" />
        <MariloSkeleton Variant="SkeletonVariant.Text" Width="40%" />
    </div>
</MariloCard>
```

## MariloProgressBar

`MariloProgressBar` displays a horizontal bar that fills to indicate progress. It supports both determinate (known percentage) and indeterminate (unknown duration) modes.

```razor
<!-- Determinate: 65% complete -->
<MariloProgressBar Value="65" Max="100" />

<!-- Indeterminate: unknown duration -->
<MariloProgressBar Indeterminate="true" />

<!-- With label -->
<MariloProgressBar Value="@uploadedBytes" Max="@totalBytes" ShowLabel="true" />
```

### Parameters

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Value` | `double` | `0` | Current progress value. |
| `Max` | `double` | `100` | Maximum value (100% full). |
| `Indeterminate` | `bool` | `false` | Plays an animated sweep when the total duration is unknown. |
| `ShowLabel` | `bool` | `false` | Displays the percentage as text inside or above the bar. |
| `Color` | `string?` | `null` | CSS color for the filled track. Defaults to the primary color token. |
| `Height` | `string?` | `null` | CSS height of the bar. |
| `Class` | `string?` | `null` | Additional CSS classes. |
| `Style` | `string?` | `null` | Inline styles. |

### Example: File Upload Progress

```razor
<MariloProgressBar Value="@bytesUploaded" Max="@fileSize" ShowLabel="true" />
<p>@((bytesUploaded / (double)fileSize * 100):F0)% uploaded</p>

@code {
    private long bytesUploaded;
    private long fileSize;
}
```

## MariloProgressCircle

`MariloProgressCircle` is a circular variant of the progress indicator. Use it when horizontal space is limited or a radial representation suits the context better (for example, dashboard tiles or compact card headers).

```razor
<!-- Determinate -->
<MariloProgressCircle Value="72" Max="100" ShowLabel="true" />

<!-- Indeterminate -->
<MariloProgressCircle Indeterminate="true" Size="ProgressCircleSize.Large" />
```

### Parameters

| Parameter | Type | Default | Description |
|---|---|---|---|
| `Value` | `double` | `0` | Current progress value. |
| `Max` | `double` | `100` | Maximum value. |
| `Indeterminate` | `bool` | `false` | Animated sweep for unknown duration. |
| `Size` | `ProgressCircleSize` | `ProgressCircleSize.Medium` | Predefined size of the circle. |
| `ShowLabel` | `bool` | `false` | Renders the percentage in the center of the circle. |
| `Color` | `string?` | `null` | CSS color for the progress arc. |
| `Class` | `string?` | `null` | Additional CSS classes. |
| `Style` | `string?` | `null` | Inline styles. |

## Component Built-in Loading States

Data-heavy components manage their own loading UI when you set their `Loading` parameter. The component renders a built-in spinner overlay on top of its content area and disables user interaction until loading is complete.

### DataGrid

```razor
<MariloDataGrid TItem="Order"
                OnRead="@LoadOrders"
                Loading="@isLoading">
    <MariloGridColumn Field="@nameof(Order.Id)" Title="Order #" />
    <MariloGridColumn Field="@nameof(Order.Total)" Title="Total" />
</MariloDataGrid>

@code {
    private bool isLoading;

    private async Task LoadOrders(GridReadEventArgs<Order> args)
    {
        isLoading = true;
        // ... fetch data ...
        isLoading = false;
    }
}
```

When using `OnRead`, the DataGrid sets its own internal loading indicator automatically for each read operation. Set `Loading` explicitly only when you need to control the state independently — for example, during an initial page load before the first `OnRead` fires.

### ListView

`MariloListView` supports the same `Loading` parameter. The built-in spinner is centered within the list's content area.

```razor
<MariloListView TItem="Article"
                Data="@articles"
                Loading="@isFetching" />
```

### Combining Loading Indicators

For full-page or section loading states, wrap the content area in a container and overlay a `MariloSpinner` or `MariloSkeleton` using CSS positioning:

```razor
<div style="position: relative; min-height: 200px;">
    @if (isLoading)
    {
        <div style="position: absolute; inset: 0; display: flex; align-items: center; justify-content: center;">
            <MariloSpinner Size="SpinnerSize.Large" />
        </div>
    }
    else
    {
        <ArticleList Items="@articles" />
    }
</div>
```
