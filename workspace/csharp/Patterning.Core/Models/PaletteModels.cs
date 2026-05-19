namespace Patterning.Core.Models;

/// <summary>A representative color extracted from a design.</summary>
public sealed record PaletteColor(string Id, string Hex, string Label, decimal CoveragePercent, int SampleCount);

/// <summary>Ranked palette returned by image analysis.</summary>
public sealed record ColorPalette(IReadOnlyList<PaletteColor> Colors, decimal CoverageTotalPercent, string ExtractionMethod);

/// <summary>A generic editable manufacturing channel for the active concept.</summary>
public sealed record ManufacturingChannel(string Id, string Label, string Hex, int SortOrder);

/// <summary>Relationship between a palette color and a manufacturing channel.</summary>
public sealed record ChannelMapping(string PaletteColorId, string? ChannelId, MappingStatus Status, decimal Delta, string? Notes);
