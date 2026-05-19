namespace Patterning.Core.Models;

/// <summary>Represents an uploaded or sample design in the active session.</summary>
public sealed record DesignConcept(
    Guid Id,
    string SourceName,
    SourceType SourceType,
    DesignMimeType MimeType,
    long FileSizeBytes,
    string PreviewDataUrl,
    AnalysisStatus AnalysisStatus,
    DateTimeOffset CreatedAt);

/// <summary>Non-sensitive image characteristics returned by analysis.</summary>
public sealed record ImageMetadata(
    int WidthPx,
    int HeightPx,
    decimal AspectRatio,
    int EstimatedUniqueColors,
    bool HasTransparency,
    string? BackgroundIndicator);
