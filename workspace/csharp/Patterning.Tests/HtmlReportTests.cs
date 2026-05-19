using Patterning.Core.Models;
using Patterning.Core.Reports;
using Xunit;

namespace Patterning.Tests;

public sealed class HtmlReportTests
{
    [Fact]
    public void Export_Report_ReturnsHtmlDocument()
    {
        var concept = new DesignConcept(Guid.NewGuid(), "sample.png", SourceType.Sample, DesignMimeType.Png, 10, "preview", AnalysisStatus.Completed, DateTimeOffset.UtcNow);
        var report = new ConceptReport(Guid.NewGuid(), DateTimeOffset.UtcNow, concept, new ImageMetadata(1, 1, 1, 1, false, null), new ColorPalette([], 100, "test"), [], [], new { size = 64 }, [], null);
        var html = new HtmlReportExporter().Export(report);
        Assert.Contains("<!doctype html>", html);
    }
}
