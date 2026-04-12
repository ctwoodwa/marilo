namespace Marilo.Components.DataGrid;

/// <summary>
/// Interface for components that accept child column registrations via CascadingValue.
/// Implemented by MariloDataGrid, MariloTreeList, etc.
/// </summary>
public interface IColumnHost
{
    /// <summary>Registers a column with this host during OnInitialized.</summary>
    void RegisterColumn(MariloColumnBase column);

    /// <summary>Unregisters a column from this host during Dispose.</summary>
    void UnregisterColumn(MariloColumnBase column);
}
