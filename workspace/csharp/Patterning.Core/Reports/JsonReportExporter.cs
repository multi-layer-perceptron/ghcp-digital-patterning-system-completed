using System.Text.Json;
using Patterning.Core.Models;

namespace Patterning.Core.Reports;

/// <summary>Exports concept reports as structured JSON.</summary>
public sealed class JsonReportExporter
{
    public string Export(ConceptReport report)
    {
        ArgumentNullException.ThrowIfNull(report);
        return JsonSerializer.Serialize(report, new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true });
    }
}
