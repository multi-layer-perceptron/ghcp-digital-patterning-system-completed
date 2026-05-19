using Microsoft.Data.SqlClient;
using Patterning.Core.Models;

namespace Patterning.Infrastructure.Data;

/// <summary>Persists channel settings and palette mappings.</summary>
public sealed class ChannelMappingRepository(SqlOptions options) : SqlRepositoryBase(options)
{
    public async Task SaveChannelAsync(Guid conceptId, ManufacturingChannel channel, CancellationToken cancellationToken)
    {
        using var connection = CreateConnection();
        using var command = new SqlCommand("INSERT INTO manufacturing_channels (channel_id, concept_id, channel_key, display_label, hex_color, sort_order) VALUES (@channel_id, @concept_id, @channel_key, @display_label, @hex_color, @sort_order)", connection);
        AddParameter(command, "@channel_id", channel.Id);
        AddParameter(command, "@concept_id", conceptId);
        AddParameter(command, "@channel_key", channel.Id);
        AddParameter(command, "@display_label", channel.Label);
        AddParameter(command, "@hex_color", channel.Hex);
        AddParameter(command, "@sort_order", channel.SortOrder);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
