using Marilo.Core.Data;

namespace Marilo.Components.DataGrid;

internal class GridState
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public List<SortDescriptor> SortDescriptors { get; set; } = [];
    public List<FilterDescriptor> FilterDescriptors { get; set; } = [];
    public int TotalCount { get; set; }
}
