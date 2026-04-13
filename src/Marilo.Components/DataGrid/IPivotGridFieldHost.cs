namespace Marilo.Components.DataGrid;

/// <summary>
/// Interface for PivotGrid components that accept child field registrations via CascadingValue.
/// </summary>
public interface IPivotGridFieldHost
{
    /// <summary>Registers a row field with this host during OnInitialized.</summary>
    void RegisterRowField(MariloPivotGridRowField field);

    /// <summary>Unregisters a row field from this host during Dispose.</summary>
    void UnregisterRowField(MariloPivotGridRowField field);

    /// <summary>Registers a column field with this host during OnInitialized.</summary>
    void RegisterColumnField(MariloPivotGridColumnField field);

    /// <summary>Unregisters a column field from this host during Dispose.</summary>
    void UnregisterColumnField(MariloPivotGridColumnField field);

    /// <summary>Registers a measure field with this host during OnInitialized.</summary>
    void RegisterMeasureField(MariloPivotGridMeasureField field);

    /// <summary>Unregisters a measure field from this host during Dispose.</summary>
    void UnregisterMeasureField(MariloPivotGridMeasureField field);
}
