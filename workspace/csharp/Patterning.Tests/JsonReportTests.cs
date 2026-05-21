using Patterning.Core.Reports;
using Xunit;

namespace Patterning.Tests;

public sealed class JsonReportTests
{
    [Fact]
    public void Export_NullReport_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new JsonReportExporter().Export(null!));
    }
}
