using Bunit;
using Marilo.Components.Forms.Inputs;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

public class DropDownListTests : MariloTestBase
{
    private record Country(int Id, string Name, string Code);

    private static readonly List<Country> Countries = new()
    {
        new(1, "United States", "US"),
        new(2, "Canada", "CA"),
        new(3, "United Kingdom", "GB"),
    };

    [Fact]
    public void RendersItemsFromData()
    {
        var cut = Render<MariloDropDownList<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id"));

        // Click to open the dropdown
        cut.Find("div[role='listbox']").Click();

        var items = cut.FindAll("li[role='option']");
        Assert.Equal(3, items.Count);
        Assert.Contains("United States", items[0].TextContent);
        Assert.Contains("Canada", items[1].TextContent);
        Assert.Contains("United Kingdom", items[2].TextContent);
    }

    [Fact]
    public void ValueSelectionFiresValueChanged()
    {
        int? selectedValue = null;

        var cut = Render<MariloDropDownList<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.ValueChanged, v => selectedValue = v));

        // Open dropdown
        cut.Find("div[role='listbox']").Click();

        // Click on "Canada"
        var items = cut.FindAll("li[role='option']");
        items[1].Click();

        Assert.Equal(2, selectedValue);
    }

    [Fact]
    public void ShowsPlaceholderWhenNoValueSelected()
    {
        var cut = Render<MariloDropDownList<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Placeholder, "Pick a country"));

        Assert.Contains("Pick a country", cut.Markup);
    }

    [Fact]
    public void KeyboardArrowDownOpensDropdown()
    {
        var cut = Render<MariloDropDownList<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id"));

        var root = cut.Find("div[role='listbox']");
        root.KeyDown(key: "ArrowDown");

        // Should now be open with items visible
        var items = cut.FindAll("li[role='option']");
        Assert.Equal(3, items.Count);
    }

    [Fact]
    public void EscapeClosesDropdown()
    {
        var cut = Render<MariloDropDownList<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id"));

        var root = cut.Find("div[role='listbox']");

        // Open
        root.Click();
        Assert.NotEmpty(cut.FindAll("li[role='option']"));

        // Close with Escape
        root.KeyDown(key: "Escape");
        Assert.Empty(cut.FindAll("li[role='option']"));
    }
}
