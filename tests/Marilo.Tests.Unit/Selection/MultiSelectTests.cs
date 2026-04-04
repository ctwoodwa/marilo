using Bunit;
using Marilo.Components.Forms.Inputs;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

public class MultiSelectTests : MariloTestBase
{
    private record Country(int Id, string Name, string Code);

    private static readonly List<Country> Countries = new()
    {
        new(1, "United States", "US"),
        new(2, "Canada", "CA"),
        new(3, "United Kingdom", "GB"),
    };

    [Fact]
    public void AllowsMultipleSelections()
    {
        IEnumerable<int>? selectedValue = null;

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.ValueChanged, v => selectedValue = v));

        // Open dropdown by clicking the input area
        cut.Find(".mar-multiselect__input-area").Click();

        // Select first item
        var items = cut.FindAll("[role='option']");
        items[0].Click();

        Assert.NotNull(selectedValue);
        Assert.Contains(1, selectedValue);
    }

    [Fact]
    public void ShowsPlaceholderWhenEmpty()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.Placeholder, "Select countries..."));

        Assert.Contains("Select countries...", cut.Markup);
    }

    [Fact]
    public void RendersTagsForSelectedItems()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int> { 1, 3 }));

        var tags = cut.FindAll(".mar-multiselect-tag");
        Assert.Equal(2, tags.Count);
        Assert.Contains("United States", cut.Markup);
        Assert.Contains("United Kingdom", cut.Markup);
    }

    [Fact]
    public void SingleTagModeShowsCount()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int> { 1, 2, 3 })
            .Add(p => p.TagMode, Marilo.Core.Enums.MultiSelectTagMode.Single));

        Assert.Contains("3 items selected", cut.Markup);
    }

    [Fact]
    public void DeselectingItemRemovesIt()
    {
        IEnumerable<int>? selectedValue = null;

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int> { 1, 2 })
            .Add(p => p.ValueChanged, v => selectedValue = v));

        // Open dropdown by clicking the input area
        cut.Find(".mar-multiselect__input-area").Click();

        // Click on first item (already selected) to deselect
        var items = cut.FindAll("[role='option']");
        items[0].Click();

        Assert.NotNull(selectedValue);
        Assert.DoesNotContain(1, selectedValue);
        Assert.Contains(2, selectedValue);
    }

    [Fact]
    public void TagTemplate_RendersCustomContent()
    {
        var cut = Render<MariloMultiSelect<string, string>>(parameters => parameters
            .Add(p => p.Data, new[] { "Alpha", "Beta", "Gamma" })
            .Add(p => p.TextField, "")
            .Add(p => p.ValueField, "")
            .Add(p => p.Value, new List<string> { "Alpha" })
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<List<string>>(this, _ => { }))
            .Add(p => p.TagTemplate, item => builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", "custom-tag");
                builder.AddContent(2, $"TAG:{item}");
                builder.CloseElement();
            }));

        Assert.Contains("custom-tag", cut.Markup);
        Assert.Contains("TAG:Alpha", cut.Markup);
    }

    [Fact]
    public void NoDataTemplate_RendersWhenEmpty()
    {
        var cut = Render<MariloMultiSelect<string, string>>(parameters => parameters
            .Add(p => p.Data, Array.Empty<string>())
            .Add(p => p.TextField, "")
            .Add(p => p.ValueField, "")
            .Add(p => p.Value, new List<string>())
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<List<string>>(this, _ => { }))
            .Add(p => p.NoDataTemplate, builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "custom-no-data");
                builder.AddContent(2, "Nothing here");
                builder.CloseElement();
            }));

        // Open dropdown to see the no-data slot
        cut.Find(".mar-multiselect__input-area").Click();

        Assert.Contains("custom-no-data", cut.Markup);
        Assert.Contains("Nothing here", cut.Markup);
    }

    [Fact]
    public void HeaderTemplate_RendersInPopup()
    {
        var cut = Render<MariloMultiSelect<string, string>>(parameters => parameters
            .Add(p => p.Data, new[] { "Alpha", "Beta" })
            .Add(p => p.TextField, "")
            .Add(p => p.ValueField, "")
            .Add(p => p.Value, new List<string>())
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<List<string>>(this, _ => { }))
            .Add(p => p.HeaderTemplate, builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "custom-header");
                builder.AddContent(2, "My Header");
                builder.CloseElement();
            }));

        // Open dropdown to expose the header
        cut.Find(".mar-multiselect__input-area").Click();

        var header = cut.Find(".mar-multiselect__header");
        Assert.Contains("custom-header", header.InnerHtml);
        Assert.Contains("My Header", header.InnerHtml);
    }

    [Fact]
    public void FooterTemplate_RendersInPopup()
    {
        var cut = Render<MariloMultiSelect<string, string>>(parameters => parameters
            .Add(p => p.Data, new[] { "Alpha", "Beta" })
            .Add(p => p.TextField, "")
            .Add(p => p.ValueField, "")
            .Add(p => p.Value, new List<string>())
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<List<string>>(this, _ => { }))
            .Add(p => p.FooterTemplate, builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "custom-footer");
                builder.AddContent(2, "My Footer");
                builder.CloseElement();
            }));

        // Open dropdown to expose the footer
        cut.Find(".mar-multiselect__input-area").Click();

        var footer = cut.Find(".mar-multiselect__footer");
        Assert.Contains("custom-footer", footer.InnerHtml);
        Assert.Contains("My Footer", footer.InnerHtml);
    }

    [Fact]
    public void SummaryTagTemplate_RendersInSingleMode()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int> { 1, 2, 3 })
            .Add(p => p.TagMode, Marilo.Core.Enums.MultiSelectTagMode.Single)
            .Add(p => p.SummaryTagTemplate, values => builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", "custom-summary");
                builder.AddContent(2, $"{values.Count} chosen");
                builder.CloseElement();
            }));

        Assert.Contains("custom-summary", cut.Markup);
        Assert.Contains("3 chosen", cut.Markup);
    }

    [Fact]
    public void AllowCustom_ShowsCreateOption()
    {
        var cut = Render<MariloMultiSelect<string, string>>(parameters => parameters
            .Add(p => p.Data, new[] { "Alpha", "Beta", "Gamma" })
            .Add(p => p.TextField, "")
            .Add(p => p.ValueField, "")
            .Add(p => p.Value, new List<string>())
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<List<string>>(this, _ => { }))
            .Add(p => p.Filterable, true)
            .Add(p => p.DebounceDelay, 0)
            .Add(p => p.AllowCustom, true));

        // Open dropdown
        cut.Find(".mar-multiselect__input-area").Click();

        // Simulate typing a value that does not match any existing item
        cut.Find(".mar-multiselect__filter-input").Input("NewEntry");

        Assert.Contains("Create: NewEntry", cut.Markup);
        Assert.Contains("mar-multiselect__item--custom", cut.Markup);
    }
}
