using Marilo.Core.Enums;
using Microsoft.AspNetCore.Components;

namespace Marilo.Components.Forms.Inputs;

/// <summary>
/// Internal cascade sink used by <see cref="MariloMultiSelect{TItem, TValue}"/> to
/// receive registration callbacks from non-generic settings child components. The
/// sink decouples the generic parent type from the non-generic children so the
/// children can be authored without TItem/TValue type parameters.
/// </summary>
internal interface IMultiSelectSettingsSink
{
    void RegisterSettings(MultiSelectSettings settings);
    void UnregisterSettings(MultiSelectSettings settings);
    void RegisterPopupSettings(MultiSelectPopupSettings settings);
    void UnregisterPopupSettings(MultiSelectPopupSettings settings);
}

/// <summary>
/// Declarative child component that overrides general <see cref="MariloMultiSelect{TItem, TValue}"/>
/// settings via cascading parameter. Produces no markup; parameters left null fall
/// through to the parent component's own parameter values.
/// </summary>
/// <example>
/// <code>
/// &lt;MariloMultiSelect TItem="Country" TValue="int"&gt;
///     &lt;MultiSelectSettings AdaptiveMode="AdaptiveMode.Auto" /&gt;
/// &lt;/MariloMultiSelect&gt;
/// </code>
/// </example>
public class MultiSelectSettings : ComponentBase, IDisposable
{
    [CascadingParameter] internal IMultiSelectSettingsSink? ParentSink { get; set; }

    /// <summary>
    /// Overrides the parent <see cref="MariloMultiSelect{TItem, TValue}.AdaptiveMode"/>
    /// when set. Null means fall through to the parent parameter.
    /// </summary>
    [Parameter] public AdaptiveMode? AdaptiveMode { get; set; }

    protected override void OnInitialized()
    {
        ParentSink?.RegisterSettings(this);
    }

    public void Dispose()
    {
        ParentSink?.UnregisterSettings(this);
    }
}

/// <summary>
/// Declarative child component that overrides <see cref="MariloMultiSelect{TItem, TValue}"/>
/// popup settings via cascading parameter. Produces no markup; parameters left null
/// fall through to the parent component's own parameter values.
/// </summary>
/// <example>
/// <code>
/// &lt;MariloMultiSelect TItem="Country" TValue="int"&gt;
///     &lt;MultiSelectPopupSettings Height="400px" Width="320px" Class="my-popup" /&gt;
/// &lt;/MariloMultiSelect&gt;
/// </code>
/// </example>
public class MultiSelectPopupSettings : ComponentBase, IDisposable
{
    [CascadingParameter] internal IMultiSelectSettingsSink? ParentSink { get; set; }

    /// <summary>
    /// Overrides the parent <see cref="MariloMultiSelect{TItem, TValue}.PopupHeight"/>.
    /// Null means fall through to the parent parameter.
    /// </summary>
    [Parameter] public string? Height { get; set; }

    /// <summary>
    /// Overrides the parent <see cref="MariloMultiSelect{TItem, TValue}.PopupMaxHeight"/>.
    /// Null means fall through to the parent parameter.
    /// </summary>
    [Parameter] public string? MaxHeight { get; set; }

    /// <summary>
    /// Sets the popup width via inline style. There is no parent equivalent — this
    /// parameter is only available through the child settings API.
    /// </summary>
    [Parameter] public string? Width { get; set; }

    /// <summary>
    /// Overrides the parent <see cref="MariloMultiSelect{TItem, TValue}.PopupClass"/>.
    /// Null means fall through to the parent parameter.
    /// </summary>
    [Parameter] public string? Class { get; set; }

    protected override void OnInitialized()
    {
        ParentSink?.RegisterPopupSettings(this);
    }

    public void Dispose()
    {
        ParentSink?.UnregisterPopupSettings(this);
    }
}
