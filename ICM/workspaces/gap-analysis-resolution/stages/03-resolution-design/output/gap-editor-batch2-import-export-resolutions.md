# Resolution Records: Editor Batch 2a — Import/Export with Markdig

> Date: 2026-04-09
> Source: `stages/01-intake/output/gap-editor-inventory.md` GAP-EDITOR-005
> Components: MariloEditor
> Scope: single (one isolated gap; runs 01 → 03 → 05 → 06)

This batch resolves only GAP-EDITOR-005 (import/export). The other two Batch 2 gaps (GAP-EDITOR-002 adaptive toolbar, GAP-EDITOR-004 table/image resize) remain blocked on JS interop.

Human decision (2026-04-09): *"Markdig approved as a bounded Markdown adapter for Editor, not as the editor's core model."*

---

## RES-EDITOR-B2A-01: Format-agnostic import/export with built-in Markdown adapter

**Resolves:** GAP-EDITOR-005
**Status:** Ready for implementation

### Target Pattern

The spec says: *"Allowing the application to decide how to import and export the content makes the Marilo UI for Blazor package more lightweight."* — this means import/export is NOT embedded in the editor's core render path; it's a helper API.

Three layers:

**Layer 1: Format converter interface** (in `Marilo.Components.Editors`)

```csharp
/// <summary>
/// Converts editor content between HTML and other formats.
/// Register implementations via DI. The editor resolves these at runtime.
/// </summary>
public interface IEditorFormatConverter
{
    /// <summary>The format this converter handles (e.g., "markdown", "plaintext").</summary>
    string Format { get; }

    /// <summary>Convert from the specified format to HTML.</summary>
    string ToHtml(string content);

    /// <summary>Convert from HTML to the specified format.</summary>
    string FromHtml(string html);
}
```

**Layer 2: Public methods on MariloEditor**

```csharp
/// <summary>
/// Imports content in the specified format, converting it to HTML via the
/// registered IEditorFormatConverter for that format, then sets Value.
/// </summary>
public async Task ImportAsync(string content, string format)
{
    var converter = ResolveConverter(format);
    var html = converter.ToHtml(content);
    Value = html;
    await ValueChanged.InvokeAsync(Value);
    if (_jsModule is not null)
        await _jsModule.InvokeVoidAsync("setContent", Value);
}

/// <summary>
/// Exports the current HTML value in the specified format via the registered
/// IEditorFormatConverter for that format.
/// </summary>
public async Task<string> ExportAsync(string format)
{
    var html = await GetHtmlAsync();
    var converter = ResolveConverter(format);
    return converter.FromHtml(html);
}
```

`ResolveConverter(format)` resolves from DI: `IEnumerable<IEditorFormatConverter>` injected at construction, searched by `Format` property match (case-insensitive). Throws `InvalidOperationException` with a clear message if no converter registered for the requested format.

**Layer 3: Markdig implementation** (in `Marilo.Components.Editors`)

```csharp
/// <summary>
/// Markdown ↔ HTML converter backed by Markdig. Register via
/// <c>services.AddMariloEditorMarkdownSupport()</c>.
/// </summary>
internal class MarkdownFormatConverter : IEditorFormatConverter
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    public string Format => "markdown";

    public string ToHtml(string content)
    {
        if (string.IsNullOrEmpty(content)) return string.Empty;
        return Markdig.Markdown.ToHtml(content, _pipeline);
    }

    public string FromHtml(string html)
    {
        if (string.IsNullOrEmpty(html)) return string.Empty;
        // Markdig does not have a built-in HTML → Markdown converter.
        // Use ReverseMarkdown (MIT) or a simple tag-stripping approach.
        // For v1: use a basic HTML → Markdown conversion via a lightweight helper.
        // This is the "bounded adapter" — not a full round-trip guarantee.
        return HtmlToMarkdownHelper.Convert(html);
    }
}
```

Wait — Markdig converts Markdown → HTML but NOT HTML → Markdown. The reverse direction needs a different library (ReverseMarkdown, MIT licensed) or a custom implementation. Let me scope this properly.

**Revised scope:** v1 import/export supports:
- **Import from Markdown → HTML:** Markdig. Full fidelity.
- **Export HTML → Markdown:** Use the MIT-licensed `ReverseMarkdown` package (NuGet: `ReverseMarkdown`), OR implement a basic `Html.Agility` approach. Given the "bounded adapter" constraint, `ReverseMarkdown` is the pragmatic choice.
- **Import from plaintext:** trivial (`<p>` wrapping). Built-in, no dependency.
- **Export to plaintext:** trivial (strip HTML tags). Built-in, no dependency.

**DI registration extension method:**

```csharp
public static class MariloEditorServiceExtensions
{
    /// <summary>
    /// Registers the Markdown format converter for MariloEditor import/export.
    /// Adds Markdig (MIT) for Markdown → HTML and basic HTML → Markdown conversion.
    /// </summary>
    public static IServiceCollection AddMariloEditorMarkdownSupport(
        this IServiceCollection services)
    {
        services.AddSingleton<IEditorFormatConverter, MarkdownFormatConverter>();
        return services;
    }
}
```

### Decision

**Chosen:** Format-agnostic interface with built-in Markdown + plaintext adapters. Markdig added as a package dependency to `Marilo.Components.csproj` but dormant unless activated via DI. This follows the spec's philosophy ("application decides") while providing the common Markdown case out of the box.

**Rationale:**
- The spec explicitly pushes import/export to consumers — the interface-based approach honors this.
- Markdig is MIT-licensed and the most widely-used .NET Markdown library.
- HTML → Markdown conversion is inherently lossy. Rather than shipping a dependency on `ReverseMarkdown`, v1 will use a **basic built-in helper** that strips tags to approximate Markdown. Full round-trip fidelity is a v2 concern.
- Plaintext import/export is trivial and doesn't need Markdig — built-in.
- The `AddMariloEditorMarkdownSupport()` extension method makes the Markdig dependency opt-in at the DI level even though the package is always referenced.

### Success Criteria

- [ ] `IEditorFormatConverter` interface exists in `Marilo.Components.Editors`.
- [ ] `MariloEditor` has `ImportAsync(string content, string format)` and `ExportAsync(string format)` public methods.
- [ ] `ImportAsync` resolves a converter from DI, converts content to HTML, sets `Value`, syncs JS.
- [ ] `ExportAsync` gets HTML, resolves a converter from DI, returns converted content.
- [ ] Missing converter throws `InvalidOperationException` with clear message.
- [ ] `MarkdownFormatConverter` exists, is `internal`, uses Markdig for Markdown → HTML.
- [ ] `PlainTextFormatConverter` exists, is `internal`, handles plaintext ↔ HTML via simple tag stripping/wrapping.
- [ ] `AddMariloEditorMarkdownSupport()` extension method registers the Markdown converter.
- [ ] Markdig NuGet package added to `Marilo.Components.csproj`.
- [ ] bUnit tests cover: import Markdown → HTML, export HTML → plaintext, missing converter throws, ImportAsync syncs value.
- [ ] Existing tests unchanged.
