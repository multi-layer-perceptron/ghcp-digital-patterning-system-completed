using Microsoft.Data.SqlClient;

namespace Patterning.Infrastructure.Data;

/// <summary>Base class for SQL Server repositories using parameterized commands.</summary>
public abstract class SqlRepositoryBase(SqlOptions options)
{
    protected SqlOptions Options { get; } = options;

    /// <summary>Creates a configured SQL connection.</summary>
    protected SqlConnection CreateConnection() => new(Options.ConnectionString);

    /// <summary>Adds a nullable parameter to a command.</summary>
    protected static void AddParameter(SqlCommand command, string name, object? value)
    {
        command.Parameters.AddWithValue(name, value ?? DBNull.Value);
    }
}
