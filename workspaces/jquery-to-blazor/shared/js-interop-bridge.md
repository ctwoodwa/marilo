# JSInterop Bridge Patterns

Patterns for when a jQuery feature has no native Blazor or Telerik equivalent and must be bridged via JavaScript Interop. JSInterop is the last resort. Exhaust all native Blazor and Telerik options first.

## When JSInterop Is Acceptable

| Scenario | Why JSInterop | Future Native Path |
|----------|--------------|-------------------|
| Clipboard access | No Blazor API for `navigator.clipboard` | Watch for `Clipboard` API in future Blazor releases |
| Browser geolocation | No Blazor wrapper for `navigator.geolocation` | Wrap in a Blazor service for isolation |
| Complex canvas drawing | Blazor has no canvas API | Consider Blazor WASM + SkiaSharp for future |
| PDF viewer embedding | No Telerik PDF viewer in Blazor yet | Monitor Telerik roadmap |
| Third-party JS library with no .NET port | Library only exists in JavaScript | Track .NET ports or contribute one |
| `window.print()` | No Blazor print API | Unlikely to change; JSInterop is the long-term solution |
| Browser resize observer | Blazor `@onresize` is not widely supported | `TelerikMediaQuery` handles breakpoint detection |
| Scroll to element | No native Blazor scroll API | JSInterop is acceptable here |

## Standard JSInterop Pattern

### JavaScript Side

Create a module file in `wwwroot/js/` following the project's JS file convention:

```javascript
// wwwroot/js/clipboard-interop.js
export function copyToClipboard(text) {
    return navigator.clipboard.writeText(text);
}

export function readFromClipboard() {
    return navigator.clipboard.readText();
}
```

### C# Wrapper Service

Create a service class that wraps the JSInterop calls:

```csharp
// Services/ClipboardService.cs
public class ClipboardService : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public ClipboardService(IJSRuntime jsRuntime)
    {
        _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./js/clipboard-interop.js").AsTask());
    }

    public async ValueTask CopyToClipboardAsync(string text)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("copyToClipboard", text);
    }

    public async ValueTask<string> ReadFromClipboardAsync()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<string>("readFromClipboard");
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
```

### Registration

```csharp
// Program.cs
builder.Services.AddScoped<ClipboardService>();
```

### Usage in Component

```razor
@inject ClipboardService Clipboard

<TelerikButton OnClick="@CopyText">Copy</TelerikButton>

@code {
    private async Task CopyText()
    {
        await Clipboard.CopyToClipboardAsync("text to copy");
    }
}
```

## Rules for JSInterop Usage

1. Never call `IJSRuntime` directly from a component. Always wrap in a service.
2. Use JS module imports (`import`), not global `<script>` tags, for isolation.
3. Every JSInterop service must implement `IAsyncDisposable` to clean up module references.
4. Register JSInterop services as `Scoped` (not Singleton) for Blazor Server circuit safety.
5. Document every JSInterop usage with: what it does, why native Blazor cannot do it, and the plan to replace it.
6. Do not use JSInterop for DOM manipulation that Blazor should own. If you are calling `document.getElementById` to change a class or style, you are doing it wrong. Use component state.
7. Keep JS files minimal. They should be thin wrappers around browser APIs, not business logic.
8. Handle JS exceptions: wrap interop calls in try/catch on the C# side.

## Scroll to Element Pattern

```javascript
// wwwroot/js/scroll-interop.js
export function scrollToElement(elementId) {
    const el = document.getElementById(elementId);
    if (el) {
        el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
}
```

## Focus Element Pattern

```javascript
// wwwroot/js/focus-interop.js
export function focusElement(elementRef) {
    if (elementRef) {
        elementRef.focus();
    }
}
```

Usage with `@ref`:

```razor
<TelerikTextBox @ref="@searchBox" />
<TelerikButton OnClick="@FocusSearch">Search</TelerikButton>

@code {
    private TelerikTextBox searchBox;

    private async Task FocusSearch()
    {
        await searchBox.FocusAsync(); // Telerik components have built-in FocusAsync
    }
}
```

Note: Many Telerik components expose `FocusAsync()` natively. Check the Telerik API before writing custom JSInterop for focus management.
