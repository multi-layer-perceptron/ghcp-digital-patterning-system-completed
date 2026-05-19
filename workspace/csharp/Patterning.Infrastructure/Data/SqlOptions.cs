namespace Patterning.Infrastructure.Data;

/// <summary>SQL connection settings for simulator persistence.</summary>
public sealed record SqlOptions
{
    /// <summary>SQL Server-compatible connection string.</summary>
    public required string ConnectionString { get; init; }
}
