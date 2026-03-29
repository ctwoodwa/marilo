using Marilo.Core.Enums;

namespace Marilo.Core.Data;

public class SortDescriptor
{
    public string Field { get; set; } = string.Empty;
    public SortDirection Direction { get; set; }
}
