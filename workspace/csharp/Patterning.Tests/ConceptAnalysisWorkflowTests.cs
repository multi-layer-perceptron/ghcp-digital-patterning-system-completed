using Patterning.Core.Models;
using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class ConceptAnalysisWorkflowTests
{
    [Fact]
    public void Analyze_ValidInputs_ReturnsAnalyzedConcept()
    {
        var concept = new DesignConcept(Guid.NewGuid(), "sample.png", SourceType.Sample, DesignMimeType.Png, 128, "preview", AnalysisStatus.Uploaded, DateTimeOffset.UtcNow);
        var metadata = new ImageMetadata(16, 16, 1, 4, false, null);
        var palette = new ColorPalette([new PaletteColor("p1", "#112233", "Color 1", 100, 16)], 100, "sampled-rgb-buckets");
        var result = new ConceptAnalysisService().Analyze(concept, metadata, palette);
        Assert.Equal(AnalysisStatus.Analyzed, result.Concept.AnalysisStatus);
    }
}
