namespace Patterning.Core.Models;

/// <summary>A grid cell assigned to a manufacturing channel.</summary>
public sealed record ProductionGridCell(int X, int Y, string? ChannelId, string SourceColorHex);

/// <summary>Simplified manufacturing-oriented production grid.</summary>
public sealed record ProductionGridModel(
    ProductionGridSize GridSize,
    IReadOnlyList<ProductionGridCell> Cells,
    IReadOnlyDictionary<string, decimal> ChannelCoverage,
    int EstimatedCommandCount,
    int ChannelSwitchCount,
    decimal FineDetailScore);

/// <summary>A blocking error, warning, or informational manufacturability finding.</summary>
public sealed record ManufacturabilityDiagnostic(
    string Id,
    DiagnosticSeverity Severity,
    string Category,
    string Message,
    string? RelatedEntityId,
    bool Blocking);

/// <summary>A simulation event shown in the operator dashboard.</summary>
public sealed record SimulationEvent(int Sequence, DateTimeOffset Timestamp, string EventType, string Message, string? ChannelId);

/// <summary>Lifecycle-controlled production simulation run.</summary>
public sealed record SimulationRun(
    Guid Id,
    SimulationStatus Status,
    decimal ProgressPercent,
    int CurrentPass,
    int TotalPasses,
    IReadOnlyList<string> ActiveChannels,
    IReadOnlyList<SimulationEvent> EventStream,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt);

/// <summary>Portable report model exported as printable HTML and structured JSON.</summary>
public sealed record ConceptReport(
    Guid ReportId,
    DateTimeOffset GeneratedAt,
    DesignConcept Concept,
    ImageMetadata Metadata,
    ColorPalette Palette,
    IReadOnlyList<ManufacturingChannel> Channels,
    IReadOnlyList<ChannelMapping> Mappings,
    object GridSummary,
    IReadOnlyList<ManufacturabilityDiagnostic> Diagnostics,
    object? SimulationSummary);
