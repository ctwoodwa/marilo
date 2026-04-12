namespace Marilo.Core.Enums;

/// <summary>
/// Specifies the row selection mode for a TreeList.
/// </summary>
public enum TreeListSelectionMode
{
    /// <summary>No row selection.</summary>
    None,

    /// <summary>Only one row can be selected at a time.</summary>
    Single,

    /// <summary>Multiple rows can be selected simultaneously.</summary>
    Multiple
}

/// <summary>
/// Specifies the filter display mode for a TreeList.
/// </summary>
public enum TreeListFilterMode
{
    /// <summary>No filter UI is displayed.</summary>
    None,

    /// <summary>Filter inputs are displayed in a row below the column headers.</summary>
    FilterRow
}

/// <summary>
/// Specifies the edit mode for a TreeList.
/// </summary>
public enum TreeListEditMode
{
    /// <summary>No editing is enabled.</summary>
    None,

    /// <summary>Inline row editing — double-click a row to edit, with Save/Cancel buttons.</summary>
    Inline
}

/// <summary>
/// Specifies the command type for a TreeList toolbar button.
/// </summary>
public enum TreeListToolbarCommand
{
    /// <summary>Add a new row.</summary>
    Add,

    /// <summary>Save the currently editing row.</summary>
    Save,

    /// <summary>Cancel the current edit.</summary>
    Cancel
}
