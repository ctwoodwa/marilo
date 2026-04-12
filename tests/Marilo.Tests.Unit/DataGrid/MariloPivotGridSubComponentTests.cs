using Bunit;
using Marilo.Components.DataGrid;
using Marilo.Core.Models;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class MariloPivotGridSubComponentTests : MariloTestBase
{
    private static readonly List<object> SalesData = new()
    {
        new SalesRecord("North", "Widget A", 1200),
        new SalesRecord("North", "Widget B", 800),
        new SalesRecord("South", "Widget A", 950),
        new SalesRecord("South", "Widget B", 1100),
    };

    private record SalesRecord(string Region, string Product, double Revenue);

    // ── Container ────────────────────────────────────────────────────────

    [Fact]
    public void Container_Renders_With_ChildContent()
    {
        var cut = Render<MariloPivotGridContainer>(parameters => parameters
            .AddChildContent("<div class='test-grid'>Grid here</div>"));

        var container = cut.Find(".mar-pivotgrid-container");
        Assert.NotNull(container);

        var content = cut.Find(".mar-pivotgrid-container__content");
        Assert.Contains("Grid here", content.InnerHtml);
    }

    [Fact]
    public void Container_Hides_Configurator_When_ShowConfigurator_False()
    {
        var cut = Render<MariloPivotGridContainer>(parameters => parameters
            .Add(p => p.ShowConfigurator, false)
            .AddChildContent("<div>Grid</div>")
            .Add(p => p.ConfiguratorContent, (Microsoft.AspNetCore.Components.RenderFragment)(b =>
            {
                b.AddContent(0, "Configurator content");
            })));

        var panels = cut.FindAll(".mar-pivotgrid-container__configurator-panel");
        Assert.Empty(panels);
    }

    [Fact]
    public void Container_Shows_Configurator_When_ShowConfigurator_True()
    {
        var cut = Render<MariloPivotGridContainer>(parameters => parameters
            .Add(p => p.ShowConfigurator, true)
            .AddChildContent("<div>Grid</div>")
            .Add(p => p.ConfiguratorContent, (Microsoft.AspNetCore.Components.RenderFragment)(b =>
            {
                b.AddContent(0, "Configurator content");
            })));

        var panel = cut.Find(".mar-pivotgrid-container__configurator-panel");
        Assert.Contains("Configurator content", panel.InnerHtml);

        // Should have the --with-configurator modifier
        var container = cut.Find(".mar-pivotgrid-container");
        Assert.Contains("mar-pivotgrid-container--with-configurator", container.ClassName);
    }

    // ── Configurator ─────────────────────────────────────────────────────

    [Fact]
    public void Configurator_Renders_Field_Assignments()
    {
        var rowFields = new List<PivotGridField> { new() { Name = "Region", Title = "Region" } };
        var colFields = new List<PivotGridField> { new() { Name = "Product", Title = "Product" } };
        var measureFields = new List<PivotGridField> { new() { Name = "Revenue", Title = "Revenue" } };

        var cut = Render<MariloPivotGridConfigurator>(parameters => parameters
            .Add(p => p.RowFields, rowFields)
            .Add(p => p.ColumnFields, colFields)
            .Add(p => p.MeasureFields, measureFields));

        var root = cut.Find(".mar-pivotgrid-configurator");
        Assert.NotNull(root);

        var sections = cut.FindAll(".mar-pivotgrid-configurator__section");
        Assert.Equal(3, sections.Count);

        // Row field
        var rowItems = cut.FindAll(".mar-pivotgrid-configurator__field-item--row");
        Assert.Single(rowItems);
        Assert.Contains("Region", rowItems[0].TextContent);

        // Column field
        var colItems = cut.FindAll(".mar-pivotgrid-configurator__field-item--column");
        Assert.Single(colItems);
        Assert.Contains("Product", colItems[0].TextContent);

        // Measure field
        var measureItems = cut.FindAll(".mar-pivotgrid-configurator__field-item--measure");
        Assert.Single(measureItems);
        Assert.Contains("Revenue", measureItems[0].TextContent);
    }

    [Fact]
    public void Configurator_Shows_Empty_Hints_When_No_Fields()
    {
        var cut = Render<MariloPivotGridConfigurator>(parameters => parameters
            .Add(p => p.RowFields, (IReadOnlyList<PivotGridField>?)null)
            .Add(p => p.ColumnFields, (IReadOnlyList<PivotGridField>?)null)
            .Add(p => p.MeasureFields, (IReadOnlyList<PivotGridField>?)null));

        var hints = cut.FindAll(".mar-pivotgrid-configurator__empty-hint");
        Assert.Equal(3, hints.Count);
    }

    [Fact]
    public void Configurator_Falls_Back_To_Name_When_Title_Is_Null()
    {
        var rowFields = new List<PivotGridField> { new() { Name = "Region" } };

        var cut = Render<MariloPivotGridConfigurator>(parameters => parameters
            .Add(p => p.RowFields, rowFields)
            .Add(p => p.ColumnFields, (IReadOnlyList<PivotGridField>?)null)
            .Add(p => p.MeasureFields, (IReadOnlyList<PivotGridField>?)null));

        var rowItem = cut.Find(".mar-pivotgrid-configurator__field-item--row");
        Assert.Contains("Region", rowItem.TextContent);
    }

    // ── ConfiguratorButton ───────────────────────────────────────────────

    [Fact]
    public void ConfiguratorButton_Renders_Default_Content()
    {
        var cut = Render<MariloPivotGridConfiguratorButton>(parameters => parameters
            .Add(p => p.IsOpen, false));

        var btn = cut.Find(".mar-pivotgrid-configurator-button");
        Assert.NotNull(btn);
        Assert.Contains("Configure", btn.TextContent);
    }

    [Fact]
    public void ConfiguratorButton_Toggles_IsOpen_On_Click()
    {
        bool currentValue = false;
        var cut = Render<MariloPivotGridConfiguratorButton>(parameters => parameters
            .Add(p => p.IsOpen, currentValue)
            .Add(p => p.IsOpenChanged, (bool val) => currentValue = val));

        var btn = cut.Find(".mar-pivotgrid-configurator-button");
        btn.Click();

        Assert.True(currentValue);
    }

    [Fact]
    public void ConfiguratorButton_Has_Active_Class_When_Open()
    {
        var cut = Render<MariloPivotGridConfiguratorButton>(parameters => parameters
            .Add(p => p.IsOpen, true));

        var btn = cut.Find(".mar-pivotgrid-configurator-button");
        Assert.Contains("mar-pivotgrid-configurator-button--active", btn.ClassName);
    }

    [Fact]
    public void ConfiguratorButton_Renders_Custom_ChildContent()
    {
        var cut = Render<MariloPivotGridConfiguratorButton>(parameters => parameters
            .Add(p => p.IsOpen, false)
            .AddChildContent("<span class='custom'>My Button</span>"));

        var btn = cut.Find(".mar-pivotgrid-configurator-button");
        Assert.Contains("My Button", btn.TextContent);

        // Should not render default icon/text
        var icons = cut.FindAll(".mar-pivotgrid-configurator-button__icon");
        Assert.Empty(icons);
    }

    // ── Integration ──────────────────────────────────────────────────────

    [Fact]
    public void Integration_Container_With_PivotGrid_And_Configurator()
    {
        var rowFields = new List<PivotGridField> { new() { Name = "Region", Title = "Region" } };
        var colFields = new List<PivotGridField> { new() { Name = "Product", Title = "Product" } };
        var measureFields = new List<PivotGridField> { new() { Name = "Revenue", Title = "Revenue" } };

        // Render the container with a PivotGrid as content and a Configurator as the configurator panel
        var cut = Render<MariloPivotGridContainer>(parameters => parameters
            .Add(p => p.ShowConfigurator, true)
            .AddChildContent<MariloPivotGrid>(pg => pg
                .Add(p => p.Data, SalesData)
                .AddChildContent<MariloPivotGridRowField>(f => f
                    .Add(c => c.Field, "Region")
                    .Add(c => c.Title, "Region"))
                .AddChildContent<MariloPivotGridColumnField>(f => f
                    .Add(c => c.Field, "Product")
                    .Add(c => c.Title, "Product"))
                .AddChildContent<MariloPivotGridMeasureField>(f => f
                    .Add(c => c.Field, "Revenue")
                    .Add(c => c.Title, "Revenue")))
            .Add(p => p.ConfiguratorContent, (Microsoft.AspNetCore.Components.RenderFragment)(b =>
            {
                b.OpenComponent<MariloPivotGridConfigurator>(0);
                b.AddAttribute(1, nameof(MariloPivotGridConfigurator.RowFields), (IReadOnlyList<PivotGridField>)rowFields);
                b.AddAttribute(2, nameof(MariloPivotGridConfigurator.ColumnFields), (IReadOnlyList<PivotGridField>)colFields);
                b.AddAttribute(3, nameof(MariloPivotGridConfigurator.MeasureFields), (IReadOnlyList<PivotGridField>)measureFields);
                b.CloseComponent();
            })));

        // Container should have both regions
        var container = cut.Find(".mar-pivotgrid-container");
        Assert.Contains("mar-pivotgrid-container--with-configurator", container.ClassName);

        // Pivot grid should render its table
        var table = cut.Find("table.mar-pivotgrid__table");
        Assert.NotNull(table);

        // Configurator should show field assignments
        var configurator = cut.Find(".mar-pivotgrid-configurator");
        Assert.NotNull(configurator);

        var rowItems = cut.FindAll(".mar-pivotgrid-configurator__field-item--row");
        Assert.Single(rowItems);
    }
}
