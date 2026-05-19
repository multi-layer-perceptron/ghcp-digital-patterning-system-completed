using Patterning.Core.Protocol;
using System.Text.Json;

namespace Patterning.Infrastructure.Gateways;

/// <summary>Simulation-only PLC gateway stub for lifecycle commands.</summary>
public sealed class PlcGatewayStub
{
    public MachineProtocolEnvelope Handle(MachineProtocolEnvelope command)
    {
        return new MachineProtocolEnvelope(Guid.NewGuid(), "status.update", MachineProtocolEnvelope.CurrentSchemaVersion, DateTimeOffset.UtcNow, JsonSerializer.SerializeToElement(new { source = "plc", command = command.MessageType, status = "accepted" }));
    }
}
