using Bunit;
using Marilo.Components.Forms.Inputs;
using Xunit;

namespace Marilo.Tests.Unit.Selection;

public class RadioGroupTests : MariloTestBase
{
    // ── Child content mode ──────────────────────────────────────────

    [Fact]
    public void RendersChildRadiosWithLabels()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha"))
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "b")
                .AddChildContent("Beta")));

        var labels = cut.FindAll("label span");
        Assert.Equal(2, labels.Count);
        Assert.Contains("Alpha", labels[0].TextContent);
        Assert.Contains("Beta", labels[1].TextContent);
    }

    [Fact]
    public void SelectedRadioIsChecked()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "b")
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha"))
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "b")
                .AddChildContent("Beta")));

        var inputs = cut.FindAll("input[type='radio']");
        Assert.False(inputs[0].HasAttribute("checked"));
        Assert.True(inputs[1].HasAttribute("checked"));
    }

    [Fact]
    public void ClickingRadioUpdatesGroupValue()
    {
        string? selected = "a";

        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .Add(p => p.ValueChanged, v => selected = v)
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha"))
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "b")
                .AddChildContent("Beta")));

        var inputs = cut.FindAll("input[type='radio']");
        inputs[1].Change("b");

        Assert.Equal("b", selected);
    }

    [Fact]
    public void AllRadiosShareGroupName()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .Add(p => p.Name, "test-group")
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha"))
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "b")
                .AddChildContent("Beta")));

        var inputs = cut.FindAll("input[type='radio']");
        Assert.All(inputs, input => Assert.Equal("test-group", input.GetAttribute("name")));
    }

    // ── Data-driven mode ────────────────────────────────────────────

    private record RadioOption(string Value, string Text);

    private static readonly List<object> Options = new()
    {
        new RadioOption("r", "Red"),
        new RadioOption("g", "Green"),
        new RadioOption("b", "Blue"),
    };

    [Fact]
    public void DataDrivenRendersRadiosFromData()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Data, Options)
            .Add(p => p.Value, "r"));

        var inputs = cut.FindAll("input[type='radio']");
        Assert.Equal(3, inputs.Count);

        var labels = cut.FindAll("label span");
        Assert.Contains("Red", labels[0].TextContent);
        Assert.Contains("Green", labels[1].TextContent);
        Assert.Contains("Blue", labels[2].TextContent);
    }

    [Fact]
    public void DataDrivenSelectedItemIsChecked()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Data, Options)
            .Add(p => p.Value, "g"));

        var inputs = cut.FindAll("input[type='radio']");
        Assert.False(inputs[0].HasAttribute("checked"));
        Assert.True(inputs[1].HasAttribute("checked"));
        Assert.False(inputs[2].HasAttribute("checked"));
    }

    [Fact]
    public void DataDrivenSelectionFiresValueChanged()
    {
        string? selected = "r";

        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Data, Options)
            .Add(p => p.Value, "r")
            .Add(p => p.ValueChanged, v => selected = v));

        var inputs = cut.FindAll("input[type='radio']");
        inputs[2].Change("b");

        Assert.Equal("b", selected);
    }

    // ── Disabled state ──────────────────────────────────────────────

    [Fact]
    public void DisabledGroupDisablesChildRadios()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .Add(p => p.Enabled, false)
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha"))
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "b")
                .AddChildContent("Beta")));

        var inputs = cut.FindAll("input[type='radio']");
        Assert.All(inputs, input => Assert.True(input.HasAttribute("disabled")));
    }

    [Fact]
    public void DisabledGroupDoesNotFireValueChanged()
    {
        string? selected = "a";

        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .Add(p => p.Enabled, false)
            .Add(p => p.ValueChanged, v => selected = v)
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha"))
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "b")
                .AddChildContent("Beta")));

        // Force the change event even though input is disabled
        var inputs = cut.FindAll("input[type='radio']");
        inputs[1].Change("b");

        // Group's SelectValue guard should prevent the update
        Assert.Equal("a", selected);
    }

    [Fact]
    public void DisabledGroupAppliesDisabledCssClass()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .Add(p => p.Enabled, false)
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha")));

        var group = cut.Find("div[role='radiogroup']");
        Assert.Contains("mar-radio-group--disabled", group.GetAttribute("class"));
    }

    [Fact]
    public void DataDrivenDisabledGroupDisablesAllRadios()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Data, Options)
            .Add(p => p.Value, "r")
            .Add(p => p.Enabled, false));

        var inputs = cut.FindAll("input[type='radio']");
        Assert.All(inputs, input => Assert.True(input.HasAttribute("disabled")));
    }

    // ── Layout ──────────────────────────────────────────────────────

    [Fact]
    public void HorizontalLayoutAppliesCssClass()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .Add(p => p.Layout, "horizontal")
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha")));

        var group = cut.Find("div[role='radiogroup']");
        Assert.Contains("mar-radio-group--horizontal", group.GetAttribute("class"));
    }

    [Fact]
    public void VerticalLayoutDoesNotApplyHorizontalClass()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .Add(p => p.Layout, "vertical")
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha")));

        var group = cut.Find("div[role='radiogroup']");
        Assert.DoesNotContain("mar-radio-group--horizontal", group.GetAttribute("class"));
    }

    // ── Accessibility ───────────────────────────────────────────────

    [Fact]
    public void AriaLabelIsRendered()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .Add(p => p.Label, "Choose an option")
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha")));

        var group = cut.Find("div[role='radiogroup']");
        Assert.Equal("Choose an option", group.GetAttribute("aria-label"));
    }

    [Fact]
    public void GroupRendersRadiogroupRole()
    {
        var cut = Render<MariloRadioGroup>(parameters => parameters
            .Add(p => p.Value, "a")
            .AddChildContent<MariloRadio>(radio => radio
                .Add(r => r.Value, "a")
                .AddChildContent("Alpha")));

        Assert.NotNull(cut.Find("div[role='radiogroup']"));
    }

    // ── Standalone MariloRadio (no group) ───────────────────────────

    [Fact]
    public void StandaloneRadioUsesOwnNameAndSelected()
    {
        var cut = Render<MariloRadio>(parameters => parameters
            .Add(r => r.Value, "solo")
            .Add(r => r.Name, "standalone-name")
            .Add(r => r.IsSelected, true)
            .Add(r => r.Label, "Solo Label"));

        var input = cut.Find("input[type='radio']");
        Assert.Equal("standalone-name", input.GetAttribute("name"));
        Assert.True(input.HasAttribute("checked"));
        Assert.Contains("Solo Label", cut.Markup);
    }

    [Fact]
    public void StandaloneRadioFiresOwnValueChanged()
    {
        string? fired = null;

        var cut = Render<MariloRadio>(parameters => parameters
            .Add(r => r.Value, "x")
            .Add(r => r.Name, "solo")
            .Add(r => r.ValueChanged, v => fired = v));

        cut.Find("input[type='radio']").Change("x");

        Assert.Equal("x", fired);
    }

    [Fact]
    public void StandaloneRadioRendersChildContentOverLabel()
    {
        var cut = Render<MariloRadio>(parameters => parameters
            .Add(r => r.Value, "x")
            .Add(r => r.Label, "Fallback")
            .AddChildContent("Primary Content"));

        var span = cut.Find("label span");
        Assert.Contains("Primary Content", span.TextContent);
        Assert.DoesNotContain("Fallback", cut.Markup);
    }

    [Fact]
    public void StandaloneRadioRendersLabelWhenNoChildContent()
    {
        var cut = Render<MariloRadio>(parameters => parameters
            .Add(r => r.Value, "x")
            .Add(r => r.Label, "Fallback Label"));

        Assert.Contains("Fallback Label", cut.Markup);
    }

    [Fact]
    public void StandaloneRadioRendersNoSpanWhenNoLabelOrChildContent()
    {
        var cut = Render<MariloRadio>(parameters => parameters
            .Add(r => r.Value, "x"));

        Assert.Empty(cut.FindAll("label span"));
    }

    [Fact]
    public void StandaloneDisabledRadioIsDisabled()
    {
        var cut = Render<MariloRadio>(parameters => parameters
            .Add(r => r.Value, "x")
            .Add(r => r.Enabled, false)
            .Add(r => r.Label, "Disabled"));

        Assert.True(cut.Find("input[type='radio']").HasAttribute("disabled"));
    }
}
