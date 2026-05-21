# C# Components

The C# solution contains the operator dashboard, core domain services, SQL/TCP infrastructure, tests, and gateway host.

## Projects

- `Patterning.Core`: domain models, workflow services, protocol contracts, and report exporters.
- `Patterning.Infrastructure`: SQL repositories, TCP client, and PLC/FPGA simulation gateways.
- `PatterningOperatorDashboard`: WPF operator experience for upload, mapping, simulation, and report export.
- `Patterning.GatewayHost`: command-line host for gateway proof stubs.
- `Patterning.Tests`: xUnit tests for workflows and report behavior.

## Validation

When the .NET SDK is available, run:

```bash
dotnet restore PatterningSimulator.sln
dotnet test PatterningSimulator.sln
```
