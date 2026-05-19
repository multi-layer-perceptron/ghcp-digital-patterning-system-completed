using Patterning.Core.Protocol;
using System.Text.Json;

namespace Patterning.Infrastructure.Gateways;

/// <summary>Simulation-only FPGA timing gateway stub.</summary>
public sealed class FpgaTimingGatewayStub
{
    public MachineProtocolEnvelope ValidateTiming(MachineProtocolEnvelope command)
    {
        return new MachineProtocolEnvelope(Guid.NewGuid(), "status.update", MachineProtocolEnvelope.CurrentSchemaVersion, DateTimeOffset.UtcNow, JsonSerializer.SerializeToElement(new { source = "fpga", command = command.MessageType, timing_valid = true }));
    }
}
