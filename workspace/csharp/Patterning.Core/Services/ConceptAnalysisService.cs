using Patterning.Core.Models;

namespace Patterning.Core.Services;

/// <summary>Coordinates concept analysis results for dashboard presentation.</summary>
public sealed class ConceptAnalysisService
{
    /// <summary>Builds a complete analyzed concept snapshot.</summary>
    public AnalyzedConcept Analyze(DesignConcept concept, ImageMetadata metadata, ColorPalette palette)
    {
        var analyzed = concept with { AnalysisStatus = AnalysisStatus.Analyzed };
        return new AnalyzedConcept(analyzed, metadata, palette);
    }
}

/// <summary>Analyzed concept data used by the upload workflow.</summary>
public sealed record AnalyzedConcept(DesignConcept Concept, ImageMetadata Metadata, ColorPalette Palette);
