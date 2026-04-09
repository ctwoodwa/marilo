using Bunit;
using Marilo.Components.Forms.Inputs;
using Marilo.Core.Enums;
using Xunit;

namespace Marilo.Tests.Unit.Forms.Inputs;

/// <summary>
/// bUnit tests for the three standalone color components:
/// MariloColorGradient, MariloColorPalette, MariloFlatColorPicker,
/// plus ColorPickerViews child-tag integration on MariloColorPicker.
/// </summary>
public class ColorComponentTests : MariloTestBase
{
    // ══════════════════════════════════════════════════════════════════
    // MariloColorGradient
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void Gradient_Renders_Container()
    {
        var cut = Render<MariloColorGradient>();
        var root = cut.Find("div");
        Assert.Contains("mar-colorgradient", root.GetAttribute("class"));
    }

    [Fact]
    public void Gradient_Shows_Opacity_Slider_By_Default()
    {
        var cut = Render<MariloColorGradient>();
        var opacityContainer = cut.Find(".mar-colorgradient__opacity-container");
        Assert.NotNull(opacityContainer);
    }

    [Fact]
    public void Gradient_Hides_Opacity_When_Disabled()
    {
        var cut = Render<MariloColorGradient>(p => p
            .Add(x => x.ShowOpacityEditor, false));

        Assert.Throws<ElementNotFoundException>(() =>
            cut.Find(".mar-colorgradient__opacity-container"));
    }

    [Fact]
    public void Gradient_Renders_Hex_Input_When_Format_Hex()
    {
        var cut = Render<MariloColorGradient>(p => p
            .Add(x => x.Format, ColorFormat.Hex));

        var hexInput = cut.Find(".mar-colorgradient__hex-input");
        Assert.NotNull(hexInput);
    }

    [Fact]
    public void Gradient_Disabled_State()
    {
        var cut = Render<MariloColorGradient>(p => p
            .Add(x => x.Enabled, false));

        var root = cut.Find("div");
        Assert.Contains("mar-colorgradient--disabled", root.GetAttribute("class"));
        var canvas = cut.Find(".mar-colorgradient__canvas");
        Assert.Equal("-1", canvas.GetAttribute("tabindex"));
    }

    // ══════════════════════════════════════════════════════════════════
    // MariloColorPalette
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void Palette_Renders_Correct_Tile_Count()
    {
        // Default Colors = Office preset = 70 tiles
        var cut = Render<MariloColorPalette>();
        var tiles = cut.FindAll(".mar-colorpalette__tile");
        Assert.Equal(70, tiles.Count);
    }

    [Fact]
    public void Palette_Renders_Custom_Colors()
    {
        var colors = new[] { "#ff0000", "#00ff00", "#0000ff", "#ffff00", "#ff00ff" };
        var cut = Render<MariloColorPalette>(p => p
            .Add(x => x.Colors, colors));

        var tiles = cut.FindAll(".mar-colorpalette__tile");
        Assert.Equal(5, tiles.Count);
    }

    [Fact]
    public void Palette_Columns_Controls_Grid()
    {
        var cut = Render<MariloColorPalette>(p => p
            .Add(x => x.Columns, 5));

        var tilesContainer = cut.Find(".mar-colorpalette__tiles");
        var style = tilesContainer.GetAttribute("style");
        Assert.Contains("repeat(5,", style);
    }

    [Fact]
    public void Palette_Selected_Tile_Has_Class()
    {
        var colors = new[] { "#ff0000", "#00ff00", "#0000ff" };
        var cut = Render<MariloColorPalette>(p => p
            .Add(x => x.Colors, colors)
            .Add(x => x.Value, "#00ff00"));

        var selectedTiles = cut.FindAll(".mar-colorpalette__tile--selected");
        Assert.Single(selectedTiles);
    }

    [Fact]
    public void Palette_Has_Grid_Role()
    {
        var cut = Render<MariloColorPalette>();
        var root = cut.Find("[role='grid']");
        Assert.NotNull(root);
    }

    // ══════════════════════════════════════════════════════════════════
    // MariloFlatColorPicker
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void FlatPicker_Renders_Container()
    {
        var cut = Render<MariloFlatColorPicker>();
        var root = cut.Find("div");
        Assert.Contains("mar-flatcolorpicker", root.GetAttribute("class"));
    }

    [Fact]
    public void FlatPicker_Shows_Both_View_Tabs_By_Default()
    {
        var cut = Render<MariloFlatColorPicker>();
        var viewBtns = cut.FindAll(".mar-flatcolorpicker__view-btn");
        Assert.Equal(2, viewBtns.Count);
    }

    [Fact]
    public void FlatPicker_Shows_Preview_By_Default()
    {
        var cut = Render<MariloFlatColorPicker>();
        var preview = cut.Find(".mar-flatcolorpicker__preview-container");
        Assert.NotNull(preview);
    }

    [Fact]
    public void FlatPicker_Shows_Apply_Cancel_By_Default()
    {
        var cut = Render<MariloFlatColorPicker>();
        var applyBtn = cut.Find(".mar-flatcolorpicker__apply-btn");
        var cancelBtn = cut.Find(".mar-flatcolorpicker__cancel-btn");
        Assert.NotNull(applyBtn);
        Assert.NotNull(cancelBtn);
    }

    [Fact]
    public void FlatPicker_Hides_Buttons_When_ShowButtons_False()
    {
        var cut = Render<MariloFlatColorPicker>(p => p
            .Add(x => x.ShowButtons, false));

        Assert.Throws<ElementNotFoundException>(() =>
            cut.Find(".mar-flatcolorpicker__apply-btn"));
        Assert.Throws<ElementNotFoundException>(() =>
            cut.Find(".mar-flatcolorpicker__cancel-btn"));
    }

    // ══════════════════════════════════════════════════════════════════
    // ColorPickerViews integration (on MariloColorPicker)
    // ══════════════════════════════════════════════════════════════════

    [Fact]
    public void ColorPicker_Default_Shows_Both_Views()
    {
        // Without ColorPickerViews, both gradient and palette view tabs should be present
        // MariloColorPicker shows view toggle only when popup is open — open it via click
        var cut = Render<MariloColorPicker>(p => p
            .Add(x => x.Value, "#ff0000"));

        // Click the trigger button to open the popup
        var triggerBtn = cut.Find(".mar-colorpicker__btn");
        triggerBtn.Click();

        var viewBtns = cut.FindAll(".mar-colorpicker__view-btn");
        Assert.Equal(2, viewBtns.Count);
    }

    [Fact]
    public void ColorPicker_With_Only_Palette_View()
    {
        var cut = Render<MariloColorPicker>(p => p
            .Add(x => x.Value, "#ff0000")
            .Add<ColorPickerPaletteView>(x => x.ColorPickerViews));

        // Open popup
        cut.Find(".mar-colorpicker__btn").Click();

        // Only palette view should be active — no view toggle since single view
        Assert.Throws<ElementNotFoundException>(() =>
            cut.Find(".mar-colorpicker__view-toggle"));

        // Palette view content should render
        var paletteView = cut.Find(".mar-colorpicker__palette-view");
        Assert.NotNull(paletteView);
    }

    [Fact]
    public void ColorPicker_With_Only_Gradient_View()
    {
        var cut = Render<MariloColorPicker>(p => p
            .Add(x => x.Value, "#ff0000")
            .Add<ColorPickerGradientView>(x => x.ColorPickerViews));

        // Open popup
        cut.Find(".mar-colorpicker__btn").Click();

        // Only gradient view should be active — no view toggle since single view
        Assert.Throws<ElementNotFoundException>(() =>
            cut.Find(".mar-colorpicker__view-toggle"));

        // Gradient view content should render
        var gradientView = cut.Find(".mar-colorpicker__gradient-view");
        Assert.NotNull(gradientView);
    }
}
