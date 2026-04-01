using Marilo.Core.Enums;
using Marilo.Providers.FluentUI;
using Xunit;

namespace Marilo.Tests.Unit;

public class FluentUICssProviderTests
{
    private readonly FluentUICssProvider _provider = new();

    [Fact]
    public void ButtonClass_Default_ReturnsExpectedClasses()
    {
        var result = _provider.ButtonClass(ButtonVariant.Primary, ButtonSize.Medium, false, false);

        Assert.Contains("mar-button", result);
        Assert.Contains("mar-button--primary", result);
        Assert.Contains("mar-button--medium", result);
    }

    [Fact]
    public void ButtonClass_WithFillMode_ReturnsExpectedClasses()
    {
        var result = _provider.ButtonClass(ButtonVariant.Primary, ButtonSize.Medium, FillMode.Flat, RoundedMode.Full, false);

        Assert.Contains("mar-button", result);
        Assert.Contains("mar-button--fill-flat", result);
        Assert.Contains("mar-button--rounded-full", result);
    }

    [Fact]
    public void DropDownListClass_Open_ReturnsOpenClass()
    {
        var result = _provider.DropDownListClass(true, false, false);

        Assert.Contains("mar-dropdownlist", result);
        Assert.Contains("mar-dropdownlist--open", result);
    }

    [Fact]
    public void ComboBoxClass_Disabled_ReturnsDisabledClass()
    {
        var result = _provider.ComboBoxClass(false, true, false);

        Assert.Contains("mar-combobox", result);
        Assert.Contains("mar-combobox--disabled", result);
    }

    [Fact]
    public void MultiSelectItemClass_Selected_ReturnsSelectedClass()
    {
        var result = _provider.MultiSelectItemClass(false, true);

        Assert.Contains("mar-multiselect-item", result);
        Assert.Contains("mar-multiselect-item--selected", result);
    }

    [Fact]
    public void DataGridHeaderCellClass_Sortable_ReturnsSortableClass()
    {
        var result = _provider.DataGridHeaderCellClass(true, false);

        Assert.Contains("mar-datagrid-header-cell", result);
        Assert.Contains("mar-datagrid-header-cell--sortable", result);
    }

    [Fact]
    public void WindowClass_Modal_ReturnsModalClass()
    {
        var result = _provider.WindowClass(true);

        Assert.Contains("mar-window", result);
        Assert.Contains("mar-window--modal", result);
    }

    [Fact]
    public void SliderClass_Vertical_ReturnsVerticalClass()
    {
        var result = _provider.SliderClass(SliderOrientation.Vertical);

        Assert.Contains("mar-slider", result);
        Assert.Contains("mar-slider--vertical", result);
    }

    [Fact]
    public void SnackbarClass_WithPosition_ReturnsPositionClasses()
    {
        var result = _provider.SnackbarClass(NotificationVerticalPosition.Top, NotificationHorizontalPosition.Right);

        Assert.Contains("mar-snackbar", result);
        Assert.Contains("mar-snackbar--top", result);
        Assert.Contains("mar-snackbar--right", result);
    }

    [Fact]
    public void EditorClass_ReturnsBaseClass()
    {
        var result = _provider.EditorClass();
        Assert.Equal("mar-editor", result);
    }

    [Fact]
    public void UploadDropZoneClass_Active_ReturnsActiveClass()
    {
        var result = _provider.UploadDropZoneClass(true);

        Assert.Contains("mar-upload-dropzone", result);
        Assert.Contains("mar-upload-dropzone--active", result);
    }

    [Fact]
    public void ListViewItemClass_Selected_ReturnsSelectedClass()
    {
        var result = _provider.ListViewItemClass(true);

        Assert.Contains("mar-listview-item", result);
        Assert.Contains("mar-listview-item--selected", result);
    }

    // ── SignalR Status ──────────────────────────────────────────────────

    [Theory]
    [InlineData(AggregateConnectionState.Healthy, "mar-signalr-status--healthy")]
    [InlineData(AggregateConnectionState.Degraded, "mar-signalr-status--degraded")]
    [InlineData(AggregateConnectionState.Offline, "mar-signalr-status--offline")]
    [InlineData(AggregateConnectionState.Partial, "mar-signalr-status--partial")]
    public void SignalRStatusClass_ReturnsStateClass(AggregateConnectionState state, string expected)
    {
        var result = _provider.SignalRStatusClass(state, false);

        Assert.Contains("mar-signalr-status", result);
        Assert.Contains(expected, result);
    }

    [Fact]
    public void SignalRStatusClass_Compact_AddsCompactClass()
    {
        var result = _provider.SignalRStatusClass(AggregateConnectionState.Healthy, true);

        Assert.Contains("mar-signalr-status--compact", result);
    }

    [Fact]
    public void SignalRPopupClass_ReturnsExpectedClass()
    {
        var result = _provider.SignalRPopupClass();

        Assert.Contains("mar-signalr-popup", result);
    }

    [Theory]
    [InlineData(ConnectionHealthState.Healthy, "mar-signalr-row--healthy")]
    [InlineData(ConnectionHealthState.Recovering, "mar-signalr-row--recovering")]
    [InlineData(ConnectionHealthState.Offline, "mar-signalr-row--offline")]
    [InlineData(ConnectionHealthState.Degraded, "mar-signalr-row--degraded")]
    [InlineData(ConnectionHealthState.Connecting, "mar-signalr-row--connecting")]
    public void SignalRRowClass_ReturnsHealthClass(ConnectionHealthState health, string expected)
    {
        var result = _provider.SignalRRowClass(health);

        Assert.Contains("mar-signalr-row", result);
        Assert.Contains(expected, result);
    }

    [Theory]
    [InlineData(ConnectionHealthState.Healthy, "mar-signalr-badge--healthy")]
    [InlineData(ConnectionHealthState.Offline, "mar-signalr-badge--offline")]
    public void SignalRBadgeClass_ReturnsHealthClass(ConnectionHealthState health, string expected)
    {
        var result = _provider.SignalRBadgeClass(health);

        Assert.Contains("mar-signalr-badge", result);
        Assert.Contains(expected, result);
    }
}
