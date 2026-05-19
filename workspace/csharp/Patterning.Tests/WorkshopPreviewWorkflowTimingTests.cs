using System.Diagnostics;
using Patterning.Core.Models;
using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class WorkshopPreviewWorkflowTimingTests
{
    [Fact]
    public void UploadAnalyzePreview_CompletesWithinWorkshopBudget()
    {
        var watch = Stopwatch.StartNew();
        var concept = new UploadValidationService().Validate("sample.png", "image/png", 256, "preview");
        var metadata = new ImageMetadata(1, 1, 1, 1, false, null);
        var palette = new ColorPalette([], 100, "test");
        _ = new ConceptAnalysisService().Analyze(concept, metadata, palette);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 1000);
    }
}
