using Bunit;
using Marilo.Components.Forms.Inputs;
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

        // Open dropdown
        cut.Find("div[role='listbox']").Click();

        // Select first item
        var items = cut.FindAll("li[role='option']");
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

        // Open dropdown
        cut.Find("div[role='listbox']").Click();

        // Click on first item (already selected) to deselect
        var items = cut.FindAll("li[role='option']");
        items[0].Click();

        Assert.NotNull(selectedValue);
        Assert.DoesNotContain(1, selectedValue);
        Assert.Contains(2, selectedValue);
    }
}
