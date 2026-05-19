using Patterning.Core.Models;

namespace Patterning.Core.Services;

/// <summary>Evaluates manufacturability diagnostics and blocking gates.</summary>
public sealed class DiagnosticService
{
    public IReadOnlyList<ManufacturabilityDiagnostic> Evaluate(IReadOnlyList<ChannelMapping> mappings)
    {
        return mappings.Where(mapping => mapping.Status == MappingStatus.Unresolved)
            .Select(mapping => new ManufacturabilityDiagnostic($"diag-{mapping.PaletteColorId}", DiagnosticSeverity.Error, "Mapping", "Unresolved palette colors must be mapped before simulation.", mapping.PaletteColorId, true))
            .ToList();
    }

    public bool HasBlockingErrors(IReadOnlyList<ManufacturabilityDiagnostic> diagnostics) => diagnostics.Any(diagnostic => diagnostic.Blocking);
}
