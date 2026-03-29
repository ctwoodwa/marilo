using Marilo.Core.Enums;

namespace Marilo.Core.Data;

public class FilterDescriptor
{
    public string Field { get; set; } = string.Empty;
    public FilterOperator Operator { get; set; }
    public object? Value { get; set; }
}
