using Patterning.Core.Models;

namespace Patterning.Core.Reports;

/// <summary>Assembles complete concept report models.</summary>
public sealed class ConceptReportBuilder
{
    public ConceptReport Build(DesignConcept concept, ImageMetadata metadata, ColorPalette palette, IReadOnlyList<ManufacturingChannel> channels, IReadOnlyList<ChannelMapping> mappings, ProductionGridModel grid, IReadOnlyList<ManufacturabilityDiagnostic> diagnostics, SimulationRun? simulationRun)
    {
        return new ConceptReport(Guid.NewGuid(), DateTimeOffset.UtcNow, concept, metadata, palette, channels, mappings, grid, diagnostics, simulationRun);
    }
}
