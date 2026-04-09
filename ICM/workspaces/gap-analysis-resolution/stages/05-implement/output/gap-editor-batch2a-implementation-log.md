# Implementation Log: Editor Batch 2a — Import/Export with Markdig

> Date: 2026-04-09
> Resolutions: `stages/03-resolution-design/output/gap-editor-batch2-import-export-resolutions.md`
> Components: `MariloEditor`
> Scope: single (one gap; runs 01 → 03 → 05 → 06)
> Human decision: Markdig approved as bounded Markdown adapter (2026-04-09)

---

## RES-EDITOR-B2A-01: Format-agnostic import/export with Markdown + plaintext adapters

### Files created

- `src/Marilo.Components/Editors/EditorFormatConverter.cs` — new file containing:
  - `IEditorFormatConverter` public interface (Format, ToHtml, FromHtml)
  - `MarkdownFormatConverter` internal sealed class (Markdig pipeline for Md→HTML, BasicHtmlToMarkdown for HTML→Md)
  - `PlainTextFormatConverter` internal sealed class (line-to-`<p>` wrapping / tag stripping)
  - `BasicHtmlToMarkdown` internal static class (regex-based HTML→Markdown best-effort converter)
  - `MariloEditorServiceExtensions` public static class (`AddMariloEditorMarkdownSupport()`, `AddMariloEditorPlainTextSupport()`)

### Files modified

- `Directory.Packages.props` — added `<PackageVersion Include="Markdig" Version="0.39.1"/>` (MIT license, CPM)
- `src/Marilo.Components/Marilo.Components.csproj` — added `<PackageReference Include="Markdig" />` (first third-party NuGet on this project)
- `src/Marilo.Components/Editors/MariloEditor.razor` — added `@inject IEnumerable<IEditorFormatConverter> _formatConverters`, `ImportAsync(string, string)`, `ExportAsync(string)`, `ResolveConverter(string)` methods
- `tests/Marilo.Tests.Unit/P1Content/EditorTests.cs` — added 8 new bUnit tests

### Architecture

- **Format-agnostic interface:** `IEditorFormatConverter` defines `Format`, `ToHtml`, `FromHtml`. Registered via DI. The editor resolves converters at runtime by format name (case-insensitive `FirstOrDefault`).
- **Markdown adapter:** `MarkdownFormatConverter` uses `Markdig.MarkdownPipelineBuilder().UseAdvancedExtensions().Build()` for Markdown→HTML. HTML→Markdown uses `BasicHtmlToMarkdown.Convert()` (regex-based best-effort).
- **Plaintext adapter:** `PlainTextFormatConverter` wraps lines in `<p>` tags for import, strips tags + decodes entities for export.
- **DI extension methods:** `AddMariloEditorMarkdownSupport()` and `AddMariloEditorPlainTextSupport()` register the adapters as singletons. Consumers opt in explicitly.
- **No converter → clear error:** `ResolveConverter` throws `InvalidOperationException` with a message listing the registration methods.
- **Spec alignment:** The spec says "allowing the application to decide how to import and export the content makes the package more lightweight." The interface-based approach honors this — Markdig is a dependency but the converters are opt-in via DI.

### Runtime test results

**675/675 tests passing** (8 new + 667 existing). Validated on .NET 10.0.101 with `dotnet test`.

---

## Tests (8 new)

| Test | Purpose |
|------|---------|
| `MarkdownConverter_ToHtml_ConvertsHeadingsAndBold` | Markdig converts `# Hello` to `<h1>` and `**bold**` to `<strong>` |
| `MarkdownConverter_ToHtml_EmptyReturnsEmpty` | Empty/null input returns empty string |
| `MarkdownConverter_FromHtml_ConvertsBoldAndHeadings` | BasicHtmlToMarkdown converts `<h1>` to `# ` and `<strong>` to `**` |
| `PlainTextConverter_ToHtml_WrapsLinesInParagraphs` | Lines wrapped in `<p>` tags |
| `PlainTextConverter_FromHtml_StripsTagsAndDecodes` | Tags stripped, entities decoded |
| `ImportAsync_WithNoConverterRegistered_ThrowsInvalidOperationException` | Clear error when no converter for format |
| `ExportAsync_WithNoConverterRegistered_ThrowsInvalidOperationException` | Clear error when no converter for format |
| `ImportAsync_WithRegisteredConverter_SetsValue` | PlainText converter registered via DI → ImportAsync sets Value with converted HTML |

---

## No opportunistic changes

Every modified file traces directly to GAP-EDITOR-005. The only non-Editor files touched are `Directory.Packages.props` (CPM) and `Marilo.Components.csproj` (Markdig reference) — both required for the Markdig dependency.
