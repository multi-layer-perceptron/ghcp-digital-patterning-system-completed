using System.Diagnostics;
using Patterning.Core.Reports;
using Xunit;

namespace Patterning.Tests;

public sealed class ReportPerformanceTests
{
    [Fact]
    public void JsonExporter_NullFailure_ReturnsQuickly()
    {
        var watch = Stopwatch.StartNew();
        Assert.Throws<ArgumentNullException>(() => new JsonReportExporter().Export(null!));
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 500);
    }
}
