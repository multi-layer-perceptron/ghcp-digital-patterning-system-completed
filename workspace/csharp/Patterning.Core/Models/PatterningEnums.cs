namespace Patterning.Core.Models;

/// <summary>Lifecycle states for the active design concept.</summary>
public enum AnalysisStatus { Empty, Uploaded, Analyzed, Mapped, Converted, Running, Paused, Completed, Blocked }

/// <summary>Supported design source types.</summary>
public enum SourceType { Upload, Sample }

/// <summary>Supported image MIME types.</summary>
public enum DesignMimeType { Png, Jpeg }

/// <summary>Palette-to-channel mapping status.</summary>
public enum MappingStatus { Exact, Approximate, Unresolved }

/// <summary>Manufacturability diagnostic severity.</summary>
public enum DiagnosticSeverity { Error, Warning, Info }

/// <summary>Simulation lifecycle state.</summary>
public enum SimulationStatus { NotStarted, Running, Paused, Completed, Blocked, Reset }

/// <summary>Supported production grid dimensions.</summary>
public enum ProductionGridSize { Grid64 = 64, Grid128 = 128, Grid256 = 256 }
