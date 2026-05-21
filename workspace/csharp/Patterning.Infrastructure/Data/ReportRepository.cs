namespace Patterning.Infrastructure.Data;

/// <summary>Loads report input data from SQL-backed stores.</summary>
public sealed class ReportRepository(SqlOptions options) : SqlRepositoryBase(options)
{
    public Task<bool> ConceptExistsAsync(Guid conceptId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(conceptId != Guid.Empty);
    }
}
