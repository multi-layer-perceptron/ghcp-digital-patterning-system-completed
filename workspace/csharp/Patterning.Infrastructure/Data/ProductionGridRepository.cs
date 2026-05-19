using Microsoft.Data.SqlClient;
using Patterning.Core.Models;

namespace Patterning.Infrastructure.Data;

/// <summary>Persists generated production grid summaries.</summary>
public sealed class ProductionGridRepository(SqlOptions options) : SqlRepositoryBase(options)
{
    public async Task SaveGridAsync(Guid conceptId, ProductionGridModel grid, CancellationToken cancellationToken)
    {
        using var connection = CreateConnection();
        using var command = new SqlCommand("INSERT INTO production_grids (grid_id, concept_id, grid_size, estimated_command_count, channel_switch_count, fine_detail_score) VALUES (@grid_id, @concept_id, @grid_size, @estimated_command_count, @channel_switch_count, @fine_detail_score)", connection);
        AddParameter(command, "@grid_id", Guid.NewGuid());
        AddParameter(command, "@concept_id", conceptId);
        AddParameter(command, "@grid_size", (int)grid.GridSize);
        AddParameter(command, "@estimated_command_count", grid.EstimatedCommandCount);
        AddParameter(command, "@channel_switch_count", grid.ChannelSwitchCount);
        AddParameter(command, "@fine_detail_score", grid.FineDetailScore);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
