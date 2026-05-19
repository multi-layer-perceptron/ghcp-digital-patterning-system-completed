using System.Net;
using System.Text;
using Patterning.Core.Models;

namespace Patterning.Core.Reports;

/// <summary>Exports concept reports as printable HTML.</summary>
public sealed class HtmlReportExporter
{
    public string Export(ConceptReport report)
    {
        var builder = new StringBuilder();
        builder.AppendLine("<!doctype html><html><head><meta charset=\"utf-8\"><title>Digital Patterning Concept Report</title></head><body>");
        builder.AppendLine($"<h1>{WebUtility.HtmlEncode(report.Concept.SourceName)}</h1>");
        builder.AppendLine($"<p>Status: {report.Concept.AnalysisStatus}</p>");
        builder.AppendLine($"<p>Image: {report.Metadata.WidthPx} x {report.Metadata.HeightPx}</p>");
        builder.AppendLine("<h2>Palette</h2><ul>");
        foreach (var color in report.Palette.Colors)
        {
            builder.AppendLine($"<li>{WebUtility.HtmlEncode(color.Label)} {WebUtility.HtmlEncode(color.Hex)}</li>");
        }
        builder.AppendLine("</ul><h2>Diagnostics</h2><ul>");
        foreach (var diagnostic in report.Diagnostics)
        {
            builder.AppendLine($"<li>{diagnostic.Severity}: {WebUtility.HtmlEncode(diagnostic.Message)}</li>");
        }
        builder.AppendLine("</ul></body></html>");
        return builder.ToString();
    }
}
