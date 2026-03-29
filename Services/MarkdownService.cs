using Markdig;
using Microsoft.AspNetCore.Components;

namespace Marilo.Services;

public sealed class MarkdownService
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdownService()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .DisableHtml()
            .Build();
    }

    public MarkupString ToHtml(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return new MarkupString(string.Empty);

        var html = Markdown.ToHtml(markdown, _pipeline);
        return new MarkupString(html);
    }

    public string ToPlainText(string markdown, int maxLength = 0)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return string.Empty;

        var text = Markdown.ToPlainText(markdown, _pipeline);

        if (maxLength > 0 && text.Length > maxLength)
            return text[..maxLength] + "...";

        return text;
    }
}
