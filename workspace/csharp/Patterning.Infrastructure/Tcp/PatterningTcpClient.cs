using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Patterning.Core.Protocol;

namespace Patterning.Infrastructure.Tcp;

/// <summary>Small JSON Lines TCP client for simulator component boundaries.</summary>
public sealed class PatterningTcpClient
{
    /// <summary>Sends one protocol envelope and returns the response line.</summary>
    public async Task<string> SendAsync(string host, int port, MachineProtocolEnvelope envelope, CancellationToken cancellationToken)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(host, port, cancellationToken).ConfigureAwait(false);
        await using var stream = client.GetStream();
        var json = JsonSerializer.Serialize(envelope, new JsonSerializerOptions(JsonSerializerDefaults.Web)) + "\n";
        var bytes = Encoding.UTF8.GetBytes(json);
        await stream.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
        using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
        return await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
    }
}
