using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.P2Enhancements;

public class AutoCompleteTests : MariloTestBase
{
    public record Country(string Code, string Name);

    [Fact]
    public void AutoComplete_RendersItemsFromGenericData()
    {
        var countries = new List<Country>
        {
            new("US", "United States"),
            new("GB", "United Kingdom"),
            new("FR", "France"),
        };

        var cut = Render<MariloAutocomplete<Country>>(parameters => parameters
            .Add(p => p.Data, countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Code")
            .Add(p => p.Value, "United")
            .Add(p => p.MinLength, 1));

        // Simulate focus to open dropdown
        var input = cut.Find("input");
        input.Focus();

        // Should show filtered items matching "United"
        var items = cut.FindAll("[role='option']");
        Assert.Equal(2, items.Count); // United States, United Kingdom
    }

    [Fact]
    public void AutoComplete_FiltersWithStartsWithOperator()
    {
        var fruits = new[] { "Apple", "Apricot", "Banana", "Blueberry" };

        var cut = Render<MariloAutocomplete<string>>(parameters => parameters
            .Add(p => p.Items, fruits)
            .Add(p => p.Value, "Bl")
            .Add(p => p.FilterOperator, FilterOperator.StartsWith)
            .Add(p => p.MinLength, 1));

        var input = cut.Find("input");
        input.Focus();

        var items = cut.FindAll("[role='option']");
        Assert.Single(items);
        Assert.Contains("Blueberry", items[0].TextContent);
    }

    [Fact]
    public void AutoComplete_ShowsClearButton()
    {
        var cut = Render<MariloAutocomplete<string>>(parameters => parameters
            .Add(p => p.Items, new[] { "Apple", "Banana" })
            .Add(p => p.Value, "Apple")
            .Add(p => p.ShowClearButton, true));

        var clearBtn = cut.Find(".mar-autocomplete__clear");
        Assert.NotNull(clearBtn);
    }

    [Fact]
    public void AutoComplete_ClearButtonHiddenWhenValueEmpty()
    {
        var cut = Render<MariloAutocomplete<string>>(parameters => parameters
            .Add(p => p.Items, new[] { "Apple", "Banana" })
            .Add(p => p.Value, "")
            .Add(p => p.ShowClearButton, true));

        var clearBtns = cut.FindAll(".mar-autocomplete__clear");
        Assert.Empty(clearBtns);
    }

    [Fact]
    public void AutoComplete_MinLengthPreventsFiltering()
    {
        var cut = Render<MariloAutocomplete<string>>(parameters => parameters
            .Add(p => p.Items, new[] { "Apple", "Banana" })
            .Add(p => p.Value, "A")
            .Add(p => p.MinLength, 3));

        var input = cut.Find("input");
        input.Focus();

        // Should not show items since "A" is less than MinLength 3
        var items = cut.FindAll("[role='option']");
        Assert.Empty(items);
    }

    [Fact]
    public void AutoComplete_ReadOnlyPreventsInteraction()
    {
        var cut = Render<MariloAutocomplete<string>>(parameters => parameters
            .Add(p => p.Items, new[] { "Apple", "Banana" })
            .Add(p => p.Value, "")
            .Add(p => p.ReadOnly, true));

        var input = cut.Find("input");
        Assert.True(input.HasAttribute("readonly"));
    }
}
