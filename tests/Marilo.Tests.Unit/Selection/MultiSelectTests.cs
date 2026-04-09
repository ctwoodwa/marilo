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

        // Wait for async debounce + filter to complete
        cut.WaitForState(() => cut.Markup.Contains("Create: NewEntry"), TimeSpan.FromSeconds(1));

        Assert.Contains("Create: NewEntry", cut.Markup);
        Assert.Contains("mar-multiselect__item--custom", cut.Markup);
    }

    // ── GroupField (GAP-MSEL-003 / RES-T4B4-01) ───────────────────────

    private record GroupedCountry(int Id, string Name, string Region);

    private static readonly List<GroupedCountry> RegionalCountries = new()
    {
        new(1, "United States", "Americas"),
        new(2, "Canada", "Americas"),
        new(3, "United Kingdom", "Europe"),
        new(4, "Germany", "Europe"),
        new(5, "Japan", "Asia"),
    };

    [Fact]
    public void GroupField_RendersGroupHeadersForEachDistinctValue()
    {
        var cut = Render<MariloMultiSelect<GroupedCountry, int>>(parameters => parameters
            .Add(p => p.Data, RegionalCountries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.GroupField, "Region")
            .Add(p => p.Value, new List<int>()));

        cut.Find(".mar-multiselect__input-area").Click();

        var headers = cut.FindAll(".mar-multiselect__group-header");
        Assert.Equal(3, headers.Count);

        var headerTexts = headers.Select(h => h.TextContent.Trim()).ToList();
        Assert.Contains("Americas", headerTexts);
        Assert.Contains("Europe", headerTexts);
        Assert.Contains("Asia", headerTexts);
    }

    [Fact]
    public void GroupField_HeadersAreOrderedAlphabetically()
    {
        var cut = Render<MariloMultiSelect<GroupedCountry, int>>(parameters => parameters
            .Add(p => p.Data, RegionalCountries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.GroupField, "Region")
            .Add(p => p.Value, new List<int>()));

        cut.Find(".mar-multiselect__input-area").Click();

        var headerTexts = cut.FindAll(".mar-multiselect__group-header")
            .Select(h => h.TextContent.Trim())
            .ToList();

        Assert.Equal(new[] { "Americas", "Asia", "Europe" }, headerTexts);
    }

    [Fact]
    public void GroupField_NotSet_RendersNoGroupHeaders()
    {
        var cut = Render<MariloMultiSelect<GroupedCountry, int>>(parameters => parameters
            .Add(p => p.Data, RegionalCountries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>()));

        cut.Find(".mar-multiselect__input-area").Click();

        Assert.Empty(cut.FindAll(".mar-multiselect__group-header"));
        // All 5 items still rendered as options
        Assert.Equal(5, cut.FindAll("[role='option']").Count);
    }

    [Fact]
    public void GroupField_PreservesAllItemsAcrossGroups()
    {
        var cut = Render<MariloMultiSelect<GroupedCountry, int>>(parameters => parameters
            .Add(p => p.Data, RegionalCountries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.GroupField, "Region")
            .Add(p => p.Value, new List<int>()));

        cut.Find(".mar-multiselect__input-area").Click();

        // 5 underlying items must still render as 5 options regardless of grouping
        Assert.Equal(5, cut.FindAll("[role='option']").Count);
    }

    [Fact]
    public void GroupField_HeaderHasStickyPositioning()
    {
        var cut = Render<MariloMultiSelect<GroupedCountry, int>>(parameters => parameters
            .Add(p => p.Data, RegionalCountries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.GroupField, "Region")
            .Add(p => p.Value, new List<int>()));

        cut.Find(".mar-multiselect__input-area").Click();

        var firstHeader = cut.FindAll(".mar-multiselect__group-header").First();
        var style = firstHeader.GetAttribute("style") ?? string.Empty;
        Assert.Contains("position:sticky", style);
        Assert.Contains("top:0", style);
    }

    // ── OnRead / Rebind / ValueMapper (GAP-MSEL-006 / RES-T4B5-01) ────

    [Fact]
    public void OnRead_InvokedWhenDropdownOpens()
    {
        var readCount = 0;
        MultiSelectReadEventArgs<Country>? capturedArgs = null;

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Value, new List<int>())
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.OnRead, args =>
            {
                readCount++;
                capturedArgs = args;
                args.Data = Countries;
                args.Total = Countries.Count;
            }));

        cut.Find(".mar-multiselect__input-area").Click();

        Assert.Equal(1, readCount);
        Assert.NotNull(capturedArgs);
        Assert.Equal(string.Empty, capturedArgs!.Filter);
        // After OnRead resolved, dropdown shows the items the handler supplied
        cut.WaitForState(() => cut.FindAll("[role='option']").Count == 3, TimeSpan.FromSeconds(1));
        Assert.Equal(3, cut.FindAll("[role='option']").Count);
    }

    [Fact]
    public async Task Rebind_TriggersOnReadAgain()
    {
        var readCount = 0;

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Value, new List<int>())
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.OnRead, args =>
            {
                readCount++;
                args.Data = Countries;
                args.Total = Countries.Count;
            }));

        // First open triggers initial read
        cut.Find(".mar-multiselect__input-area").Click();
        Assert.Equal(1, readCount);

        // Rebind triggers another read
        await cut.InvokeAsync(() => cut.Instance.Rebind());
        Assert.Equal(2, readCount);
    }

    [Fact]
    public async Task Rebind_WithoutOnRead_FallsBackToRefresh()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>()));

        // Should not throw — falls back to Refresh() which re-reads local Data
        await cut.InvokeAsync(() => cut.Instance.Rebind());

        cut.Find(".mar-multiselect__input-area").Click();
        Assert.Equal(3, cut.FindAll("[role='option']").Count);
    }

    [Fact]
    public void ValueMapper_ResolvesPreSelectedRemoteValues()
    {
        // Pre-select id=99 which is NOT in the local Data window —
        // the ValueMapper supplies the matching item so the tag renders.
        var remoteOnly = new Country(99, "Antarctica", "AQ");

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int> { 99 })
            .Add(p => p.ValueMapper, async values =>
            {
                await Task.Yield();
                return values.Contains(99) ? new[] { remoteOnly } : Array.Empty<Country>();
            }));

        // Wait for the async ValueMapper resolution to complete
        cut.WaitForState(() => cut.Markup.Contains("Antarctica"), TimeSpan.FromSeconds(1));

        var tags = cut.FindAll(".mar-multiselect-tag");
        Assert.Single(tags);
        Assert.Contains("Antarctica", cut.Markup);
    }

    [Fact]
    public void OnRead_ReceivesFilterTextWhenUserTypes()
    {
        string? capturedFilter = null;
        var readCount = 0;

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Value, new List<int>())
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Filterable, true)
            .Add(p => p.DebounceDelay, 0)
            .Add(p => p.OnRead, args =>
            {
                readCount++;
                capturedFilter = args.Filter;
                args.Data = Countries.Where(c =>
                    string.IsNullOrEmpty(args.Filter) ||
                    c.Name.Contains(args.Filter, StringComparison.OrdinalIgnoreCase));
                args.Total = Countries.Count;
            }));

        // Open
        cut.Find(".mar-multiselect__input-area").Click();
        Assert.Equal(1, readCount);

        // Type a filter
        cut.Find(".mar-multiselect__filter-input").Input("Can");

        cut.WaitForState(() => readCount >= 2, TimeSpan.FromSeconds(1));
        Assert.Equal("Can", capturedFilter);
    }

    // ── OnChange / OnItemRender (GAP-MSEL-001 final / RES-T4B6-01) ────

    [Fact]
    public void OnChange_FiresWhenUserSelectsItem()
    {
        List<int>? captured = null;
        var changeCount = 0;

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.OnChange, list => { changeCount++; captured = list; }));

        cut.Find(".mar-multiselect__input-area").Click();
        cut.FindAll("[role='option']")[0].Click();

        Assert.Equal(1, changeCount);
        Assert.NotNull(captured);
        Assert.Contains(1, captured!);
    }

    [Fact]
    public void OnChange_FiresOnRemove()
    {
        var changeCount = 0;

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int> { 1 })
            .Add(p => p.OnChange, _ => changeCount++));

        // Click the tag remove button (× inside .mar-multiselect-tag__remove)
        cut.Find(".mar-multiselect-tag__remove").Click();

        Assert.Equal(1, changeCount);
    }

    [Fact]
    public void OnChange_DoesNotFireOnExternalValueSet()
    {
        var changeCount = 0;

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.OnChange, _ => changeCount++));

        // Re-render with externally set Value — must not fire OnChange.
        // bUnit v2: Render() on IRenderedComponent<T> rebinds parameters.
        cut.Render(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.OnChange, _ => changeCount++)
            .Add(p => p.Value, new List<int> { 1, 2 }));

        Assert.Equal(0, changeCount);
    }

    [Fact]
    public void OnItemRender_InvokedOncePerFilteredItem()
    {
        var renderedItems = new List<string>();

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.OnItemRender, args => { renderedItems.Add(args.Item.Name); }));

        cut.Find(".mar-multiselect__input-area").Click();

        // 3 countries, dropdown open → 3 OnItemRender invocations (one per item)
        Assert.Equal(3, renderedItems.Count);
        Assert.Contains("United States", renderedItems);
        Assert.Contains("Canada", renderedItems);
        Assert.Contains("United Kingdom", renderedItems);
    }

    [Fact]
    public void OnItemRender_CssClassAppliedToOption()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.OnItemRender, args =>
            {
                if (args.Item.Code == "CA")
                    args.CssClass = "highlight-canada";
            }));

        cut.Find(".mar-multiselect__input-area").Click();

        // The option element for Canada should carry the custom class
        var canadaOption = cut.FindAll("[role='option']")
            .First(o => o.TextContent.Contains("Canada"));
        Assert.Contains("highlight-canada", canadaOption.GetAttribute("class") ?? "");
    }

    [Fact]
    public void OnItemRender_DisabledItemIsNotSelectable()
    {
        List<int>? captured = null;

        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.ValueChanged, v => captured = v)
            .Add(p => p.OnItemRender, args =>
            {
                if (args.Item.Id == 1) args.IsDisabled = true;
            }));

        cut.Find(".mar-multiselect__input-area").Click();

        // Click the disabled item — selection should not happen
        cut.FindAll("[role='option']")[0].Click();

        Assert.Null(captured);
    }

    [Fact]
    public void OnItemRender_DisabledItemHasAriaDisabled()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.OnItemRender, args =>
            {
                if (args.Item.Id == 1) args.IsDisabled = true;
            }));

        cut.Find(".mar-multiselect__input-area").Click();

        var firstOption = cut.FindAll("[role='option']")[0];
        Assert.Equal("true", firstOption.GetAttribute("aria-disabled"));
    }

    // ── Virtual scroll config (GAP-MSEL-007 / RES-T4B6-02) ─────────────

    [Fact]
    public void ItemHeight_HasDefault32()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>()));

        Assert.Equal(32, cut.Instance.ItemHeight);
    }

    [Fact]
    public void PageSize_HasDefault3()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>()));

        Assert.Equal(3, cut.Instance.PageSize);
    }

    [Fact]
    public void ItemHeight_AcceptsCustomValue()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.EnableVirtualization, true)
            .Add(p => p.ItemHeight, 48));

        Assert.Equal(48, cut.Instance.ItemHeight);
        // Open the dropdown — virtualized container renders without throwing
        cut.Find(".mar-multiselect__input-area").Click();
        Assert.NotEmpty(cut.FindAll(".mar-multiselect__virtual-container"));
    }

    [Fact]
    public void PageSize_AcceptsCustomValue()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.EnableVirtualization, true)
            .Add(p => p.PageSize, 10));

        Assert.Equal(10, cut.Instance.PageSize);
        cut.Find(".mar-multiselect__input-area").Click();
        Assert.NotEmpty(cut.FindAll(".mar-multiselect__virtual-container"));
    }

    // ── MultiSelectSettings / MultiSelectPopupSettings (GAP-MSEL-005 / RES-T4B7-01) ─

    [Fact]
    public void MultiSelectPopupSettings_Height_OverridesParentPopupHeight()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.EnableVirtualization, true)
            .Add(p => p.PopupHeight, "200px")
            .AddChildContent<MultiSelectPopupSettings>(cp => cp
                .Add(s => s.Height, "400px")));

        cut.Find(".mar-multiselect__input-area").Click();

        var container = cut.Find(".mar-multiselect__virtual-container");
        var style = container.GetAttribute("style") ?? string.Empty;
        Assert.Contains("400px", style);
        Assert.DoesNotContain("200px", style);
    }

    [Fact]
    public void MultiSelectPopupSettings_MaxHeight_OverridesParentPopupMaxHeight()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.PopupMaxHeight, "300px")
            .AddChildContent<MultiSelectPopupSettings>(cp => cp
                .Add(s => s.MaxHeight, "500px")));

        cut.Find(".mar-multiselect__input-area").Click();

        var container = cut.Find(".mar-multiselect__list-container");
        var style = container.GetAttribute("style") ?? string.Empty;
        Assert.Contains("500px", style);
        Assert.DoesNotContain("300px", style);
    }

    [Fact]
    public void MultiSelectPopupSettings_Width_AppliedToPopup()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .AddChildContent<MultiSelectPopupSettings>(cp => cp
                .Add(s => s.Width, "320px")));

        cut.Find(".mar-multiselect__input-area").Click();

        var popup = cut.Find("[role='listbox']");
        var style = popup.GetAttribute("style") ?? string.Empty;
        Assert.Contains("width:320px", style);
    }

    [Fact]
    public void MultiSelectPopupSettings_Class_OverridesParentPopupClass()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.PopupClass, "parent-class")
            .AddChildContent<MultiSelectPopupSettings>(cp => cp
                .Add(s => s.Class, "child-class")));

        cut.Find(".mar-multiselect__input-area").Click();

        var popup = cut.Find("[role='listbox']");
        var classAttr = popup.GetAttribute("class") ?? string.Empty;
        Assert.Contains("child-class", classAttr);
        Assert.DoesNotContain("parent-class", classAttr);
    }

    [Fact]
    public void NoSettingsChild_FallsBackToParentParameters()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.EnableVirtualization, true)
            .Add(p => p.PopupHeight, "250px"));

        cut.Find(".mar-multiselect__input-area").Click();

        var container = cut.Find(".mar-multiselect__virtual-container");
        var style = container.GetAttribute("style") ?? string.Empty;
        Assert.Contains("250px", style);
    }

    [Fact]
    public void MultiSelectSettings_AdaptiveMode_OverridesParent()
    {
        var cut = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .Add(p => p.AdaptiveMode, Marilo.Core.Enums.AdaptiveMode.None)
            .AddChildContent<MultiSelectSettings>(cp => cp
                .Add(s => s.AdaptiveMode, Marilo.Core.Enums.AdaptiveMode.Auto)));

        // NOTE: state-only test — EffectiveAdaptiveMode is plumbing until Adaptive rendering is wired up.
        // Parent parameter is NOT mutated — it still returns the declared value.
        Assert.Equal(Marilo.Core.Enums.AdaptiveMode.None, cut.Instance.AdaptiveMode);

        // The effective value (internal, visible via InternalsVisibleTo) reflects the
        // child override — this is the value the component actually consumes.
        Assert.Equal(Marilo.Core.Enums.AdaptiveMode.Auto, cut.Instance.EffectiveAdaptiveMode);
    }

    [Fact]
    public void ChildContent_AcceptsSettingsTagsWithoutVisibleDom()
    {
        // Render WITHOUT settings children
        var baseline = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>()));

        // Render WITH settings children that should produce no DOM
        var withSettings = Render<MariloMultiSelect<Country, int>>(parameters => parameters
            .Add(p => p.Data, Countries)
            .Add(p => p.TextField, "Name")
            .Add(p => p.ValueField, "Id")
            .Add(p => p.Value, new List<int>())
            .AddChildContent<MultiSelectSettings>(cp => cp
                .Add(s => s.AdaptiveMode, Marilo.Core.Enums.AdaptiveMode.Auto))
            .AddChildContent<MultiSelectPopupSettings>(cp => cp
                .Add(s => s.Height, "360px")
                .Add(s => s.Width, "280px")));

        // The settings child components must add no visible DOM. The two renders should
        // produce structurally identical markup because (a) Width/Height/AdaptiveMode only
        // affect the popup's open state which is closed here, and (b) the settings tags
        // themselves render no markup. We compare descendant counts rather than byte-equal
        // markup because the component generates a per-instance GUID for `_listboxId` that
        // is emitted as attribute values, so the two renders' raw markup will differ.
        var baselineDescendantCount = baseline.FindAll("*").Count;
        var withSettingsDescendantCount = withSettings.FindAll("*").Count;
        Assert.Equal(baselineDescendantCount, withSettingsDescendantCount);
    }
}
