using Microsoft.Data.SqlClient;
using Patterning.Core.Models;

namespace Patterning.Infrastructure.Data;

/// <summary>Persists channel settings and palette mappings.</summary>
public sealed class ChannelMappingRepository(SqlOptions options) : SqlRepositoryBase(options)
{
    public async Task SaveChannelAsync(ManufacturingChannel channel, CancellationToken cancellationToken)
    {
        using var connection = CreateConnection();
        using var command = new SqlCommand("INSERT INTO manufacturing_channels (channel_id, concept_id, channel_number, label, hex_color, is_enabled) VALUES (@channel_id, @concept_id, @channel_number, @label, @hex_color, @is_enabled)", connection);
        AddParameter(command, "@channel_id", channel.Id);
        AddParameter(command, "@concept_id", channel.ConceptId);
        AddParameter(command, "@channel_number", channel.ChannelNumber);
        AddParameter(command, "@label", channel.Label);
        AddParameter(command, "@hex_color", channel.HexColor);
        AddParameter(command, "@is_enabled", channel.IsEnabled);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
