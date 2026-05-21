using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Patterning.Core.Models;
using Patterning.Core.Reports;
using Patterning.Core.Services;

namespace PatterningOperatorDashboard.ViewModels;

/// <summary>
/// View model for the Reports tab. Assembles a <see cref="ConceptReport"/> from the
/// current session state (concept, metadata, palette, channels, mappings, grid, run)
/// and exposes HTML/JSON exporters plus simple summary properties for the UI.
/// </summary>
public sealed class ReportExportViewModel : INotifyPropertyChanged
{
    private readonly ConceptReportBuilder reportBuilder = new();
    private readonly HtmlReportExporter htmlExporter = new();
    private readonly JsonReportExporter jsonExporter = new();
    private readonly DiagnosticService diagnosticService = new();

    private ConceptReport? report;
    private string statusMessage = "Complete the Upload, Channels, and Simulation tabs to generate a report.";
    private string? lastExportPath;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ConceptReport? Report
    {
        get => report;
        private set
        {
            report = value;
            RaiseAll();
        }
    }

    public string StatusMessage
    {
        get => statusMessage;
        private set { statusMessage = value; Raise(nameof(StatusMessage)); }
    }

    public string? LastExportPath
    {
        get => lastExportPath;
        set { lastExportPath = value; Raise(nameof(LastExportPath)); }
    }

    public bool CanExport => report is not null;

    public string ConceptSummary => report is null
        ? "No concept loaded."
        : $"{report.Concept.SourceName}  ({report.Metadata.WidthPx} \u00d7 {report.Metadata.HeightPx}, {report.Concept.AnalysisStatus})";

    public string PaletteSummary => report is null
        ? "No palette."
        : $"{report.Palette.Colors.Count} colors  \u2022  coverage {report.Palette.CoverageTotalPercent:0.#}%  \u2022  {report.Palette.ExtractionMethod}";

    public string ChannelsSummary => report is null
        ? "No channels."
        : $"{report.Channels.Count} channels  \u2022  {report.Mappings.Count} mappings";

    public string GridSummary
    {
        get
        {
            if (report?.GridSummary is not ProductionGridModel grid)
            {
                return "No grid generated.";
            }

            return $"{(int)grid.GridSize}\u00d7{(int)grid.GridSize}  \u2022  {grid.Cells.Count} cells  \u2022  {grid.EstimatedCommandCount} cmds  \u2022  {grid.ChannelSwitchCount} switches  \u2022  fine-detail {grid.FineDetailScore:0.##}";
        }
    }

    public string SimulationSummary
    {
        get
        {
            if (report?.SimulationSummary is not SimulationRun run)
            {
                return "No simulation run.";
            }

            var duration = run.CompletedAt is { } completed && run.StartedAt is { } started
                ? $"  \u2022  {(completed - started).TotalSeconds:0.0}s"
                : string.Empty;
            return $"{run.Status}  \u2022  pass {run.CurrentPass}/{run.TotalPasses}  \u2022  {run.EventStream.Count} events{duration}";
        }
    }

    public string DiagnosticsSummary
    {
        get
        {
            if (report is null)
            {
                return "0 diagnostics.";
            }

            var errors = report.Diagnostics.Count(d => d.Severity == DiagnosticSeverity.Error);
            var warnings = report.Diagnostics.Count(d => d.Severity == DiagnosticSeverity.Warning);
            return $"{report.Diagnostics.Count} diagnostics  \u2022  {errors} errors  \u2022  {warnings} warnings";
        }
    }

    public IReadOnlyList<PaletteColor> PaletteColors => report?.Palette.Colors ?? Array.Empty<PaletteColor>();

    public IReadOnlyList<ManufacturabilityDiagnostic> Diagnostics => report?.Diagnostics ?? Array.Empty<ManufacturabilityDiagnostic>();

    /// <summary>Rebuild the report from the current session state.</summary>
    public void Generate(
        DesignConcept? concept,
        ImageMetadata? metadata,
        ColorPalette? palette,
        IReadOnlyList<ManufacturingChannel> channels,
        IReadOnlyList<ChannelMapping> mappings,
        ProductionGridModel? grid,
        SimulationRun? simulationRun)
    {
        if (concept is null || metadata is null || palette is null || grid is null)
        {
            Report = null;
            StatusMessage = "Upload a design, apply channel mappings, and run a simulation to enable the report.";
            return;
        }

        var diagnostics = diagnosticService.Evaluate(mappings);
        Report = reportBuilder.Build(concept, metadata, palette, channels, mappings, grid, diagnostics, simulationRun);
        StatusMessage = $"Report ready  \u2022  generated {Report.GeneratedAt.LocalDateTime:HH:mm:ss}  \u2022  id {Report.ReportId.ToString()[..8]}";
    }

    public string ExportHtml() => report is null ? string.Empty : htmlExporter.Export(report);

    public string ExportJson() => report is null ? string.Empty : jsonExporter.Export(report);

    private void RaiseAll()
    {
        Raise(nameof(Report));
        Raise(nameof(CanExport));
        Raise(nameof(ConceptSummary));
        Raise(nameof(PaletteSummary));
        Raise(nameof(ChannelsSummary));
        Raise(nameof(GridSummary));
        Raise(nameof(SimulationSummary));
        Raise(nameof(DiagnosticsSummary));
        Raise(nameof(PaletteColors));
        Raise(nameof(Diagnostics));
    }

    private void Raise(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
