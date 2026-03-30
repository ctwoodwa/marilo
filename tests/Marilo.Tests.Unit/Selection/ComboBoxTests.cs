using Bunit;
using Marilo.Components.Forms.Inputs;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

public class ComboBoxTests : MariloTestBase
{
    private record Country(int Id, string Name, string Code);

    private static readonly List<Country> Countries = new()
    {
        new(1, "United States", "US"),
        new(2, "Canada", "CA"),
        new(3, "United Kingdom", "GB"),
        new(4, "Germany", "DE"),
    };

    [Fact]
    public void FiltersItemsOnTextInput()
    {
        var cut = Render<MariloComboBox<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id"));

        var input = cut.Find("input[role='combobox']");

        // Focus to open
        input.Focus();

        // Type "can" to filter
        input.Input("Can");

        var items = cut.FindAll("li[role='option']");
        Assert.Single(items);
        Assert.Contains("Canada", items[0].TextContent);
    }

    [Fact]
    public void SelectingItemUpdatesValue()
    {
        int? selectedValue = null;

        var cut = Render<MariloComboBox<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.ValueChanged, v => selectedValue = v));

        var input = cut.Find("input[role='combobox']");
        input.Focus();

        var items = cut.FindAll("li[role='option']");
        items[2].MouseDown(); // United Kingdom

        Assert.Equal(3, selectedValue);
    }

    [Fact]
    public void StartsWithFilterModeWorks()
    {
        var cut = Render<MariloComboBox<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.FilterMode, Marilo.Core.Enums.ComboBoxFilterMode.StartsWith));

        var input = cut.Find("input[role='combobox']");
        input.Focus();

        // "United" should match "United States" and "United Kingdom"
        input.Input("United");

        var items = cut.FindAll("li[role='option']");
        Assert.Equal(2, items.Count);
    }
}
