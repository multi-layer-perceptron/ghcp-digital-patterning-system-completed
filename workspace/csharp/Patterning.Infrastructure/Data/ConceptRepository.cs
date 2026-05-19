using Microsoft.Data.SqlClient;
using Patterning.Core.Models;

namespace Patterning.Infrastructure.Data;

/// <summary>Persists design concepts and palette records.</summary>
public sealed class ConceptRepository(SqlOptions options) : SqlRepositoryBase(options)
{
    /// <summary>Stores the concept summary with parameterized SQL.</summary>
    public async Task SaveConceptAsync(DesignConcept concept, CancellationToken cancellationToken)
    {
        using var connection = CreateConnection();
        using var command = new SqlCommand("INSERT INTO design_concepts (concept_id, source_name, source_type, mime_type, file_size_bytes, analysis_status) VALUES (@concept_id, @source_name, @source_type, @mime_type, @file_size_bytes, @analysis_status)", connection);
        AddParameter(command, "@concept_id", concept.Id);
        AddParameter(command, "@source_name", concept.SourceName);
        AddParameter(command, "@source_type", concept.SourceType.ToString().ToLowerInvariant());
        AddParameter(command, "@mime_type", concept.MimeType == DesignMimeType.Png ? "image/png" : "image/jpeg");
        AddParameter(command, "@file_size_bytes", concept.FileSizeBytes);
        AddParameter(command, "@analysis_status", concept.AnalysisStatus.ToString().ToLowerInvariant());
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
