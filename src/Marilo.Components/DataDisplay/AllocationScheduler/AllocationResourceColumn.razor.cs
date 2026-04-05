using Microsoft.AspNetCore.Components;

namespace Marilo.Components.DataDisplay;

/// <summary>
/// Defines a resource metadata column in the AllocationScheduler's left-side resource grid.
/// Must be placed inside an AllocationResourceColumns RenderFragment.
/// </summary>
public partial class AllocationResourceColumn<TResource> : ComponentBase, IDisposable
{
    [CascadingParameter]
    private MariloAllocationScheduler<TResource>? Parent { get; set; }

    /// <summary>Property name on TResource to display in this column.</summary>
    [Parameter] public string Field { get; set; } = string.Empty;

    /// <summary>Column header text.</summary>
    [Parameter] public string Title { get; set; } = string.Empty;

    /// <summary>Column width CSS value.</summary>
    [Parameter] public string Width { get; set; } = "auto";

    /// <summary>Custom cell template for this column.</summary>
    [Parameter] public RenderFragment<TResource>? Template { get; set; }

    /// <summary>Custom header template.</summary>
    [Parameter] public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>Enable sorting on this column.</summary>
    [Parameter] public bool Sortable { get; set; }

    /// <summary>Enable filtering on this column.</summary>
    [Parameter] public bool Filterable { get; set; }

    /// <summary>Show or hide this column.</summary>
    [Parameter] public bool Visible { get; set; } = true;

    /// <summary>Pin column to left edge during horizontal scroll.</summary>
    [Parameter] public bool Pinned { get; set; }

    protected override void OnInitialized()
    {
        Parent?.AddColumn(this);
    }

    public void Dispose()
    {
        Parent?.RemoveColumn(this);
    }
}
