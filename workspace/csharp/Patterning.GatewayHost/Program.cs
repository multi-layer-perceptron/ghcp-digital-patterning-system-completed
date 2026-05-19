using Patterning.Core.Protocol;
using Patterning.Infrastructure.Gateways;
using System.Text.Json;

var gateway = args.Contains("--gateway") ? args[Array.IndexOf(args, "--gateway") + 1] : "plc";
var command = new MachineProtocolEnvelope(Guid.NewGuid(), MachineMessageTypes.RunStart, MachineProtocolEnvelope.CurrentSchemaVersion, DateTimeOffset.UtcNow, JsonSerializer.SerializeToElement(new { }));
var response = gateway.Equals("fpga", StringComparison.OrdinalIgnoreCase)
    ? new FpgaTimingGatewayStub().ValidateTiming(command)
    : new PlcGatewayStub().Handle(command);
Console.WriteLine($"{response.MessageType}:{gateway}:ready");
