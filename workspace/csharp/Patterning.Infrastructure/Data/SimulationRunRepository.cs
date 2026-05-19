using Microsoft.Data.SqlClient;
using Patterning.Core.Models;

namespace Patterning.Infrastructure.Data;

/// <summary>Persists simulation run state.</summary>
public sealed class SimulationRunRepository(SqlOptions options) : SqlRepositoryBase(options)
{
    public async Task SaveRunAsync(Guid gridId, SimulationRun run, CancellationToken cancellationToken)
    {
        using var connection = CreateConnection();
        using var command = new SqlCommand("INSERT INTO simulation_runs (run_id, grid_id, status, progress_percent, current_pass, total_passes) VALUES (@run_id, @grid_id, @status, @progress_percent, @current_pass, @total_passes)", connection);
        AddParameter(command, "@run_id", run.Id);
        AddParameter(command, "@grid_id", gridId);
        AddParameter(command, "@status", run.Status.ToString().ToLowerInvariant());
        AddParameter(command, "@progress_percent", run.ProgressPercent);
        AddParameter(command, "@current_pass", run.CurrentPass);
        AddParameter(command, "@total_passes", run.TotalPasses);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
