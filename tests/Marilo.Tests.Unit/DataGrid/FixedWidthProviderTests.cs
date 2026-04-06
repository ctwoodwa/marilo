using Marilo.Components.DataGrid.Sizing;
using Xunit;

namespace Marilo.Tests.Unit.DataGrid;

public class FixedWidthProviderTests
{
    [Fact]
    public void Resolve_UsesAutoForUnspecifiedColumnWidths()
    {
        var provider = new FixedWidthProvider();
        var columns = new List<ColumnSizingEntry>
        {
            new("name", null, 50, null, null),
            new("department", "180px", 50, null, null)
        };

        var contract = provider.Resolve(columns);

        Assert.Equal("auto", contract.WidthById["name"]);
        Assert.Equal("180px", contract.WidthById["department"]);
        Assert.Equal("auto 180px", contract.CenterGridTemplate);
    }

    [Fact]
    public void Resolve_ClampsExplicitPixelWidthsToMinAndMax()
    {
        var provider = new FixedWidthProvider();
        var columns = new List<ColumnSizingEntry>
        {
            new("small", "20px", 50, null, null),
            new("large", "500px", 50, 300, null)
        };

        var contract = provider.Resolve(columns);

        Assert.Equal("50px", contract.WidthById["small"]);
        Assert.Equal("300px", contract.WidthById["large"]);
    }
}
