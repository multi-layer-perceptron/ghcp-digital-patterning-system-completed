using System.Diagnostics;
using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class AnalysisPerformanceTests
{
    [Fact]
    public void UploadValidation_CompletesWithinWorkshopBudget()
    {
        var watch = Stopwatch.StartNew();
        _ = new UploadValidationService().Validate("sample.png", "image/png", 256, "preview");
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 500);
    }
}
