# Quickstart: Digital Patterning System Simulator

This quickstart describes the intended validation flow after implementation using the requested industrial stack: C#, C++, C, SQL, TCP/IP, Windows, Linux, FPGA, and PLCs.

## 1. Prepare Tooling

Recommended developer environment:

- Windows for the C# operator dashboard and PLC engineering tools.
- Linux or Windows for C++ pattern processing and C control emulator builds.
- SQL Server Developer Edition or a local SQL Server container for run-history validation.
- Optional GHDL, ModelSim, or Vivado Simulator for FPGA stub validation.
- Optional CODESYS or TwinCAT for PLC Structured Text validation.

## 2. Build C# Dashboard And Orchestration Layer

```bash
dotnet restore workspace/csharp/PatterningSimulator.sln
dotnet build workspace/csharp/PatterningSimulator.sln --configuration Debug
dotnet test workspace/csharp/PatterningSimulator.sln --configuration Debug
```

Expected result: dashboard/orchestration projects compile, xUnit tests pass, report-generation tests pass, and SQL repository tests use parameterized commands.

## 3. Build C++ Pattern Processing Layer

```bash
cmake -S workspace/cpp -B workspace/cpp/build
cmake --build workspace/cpp/build
ctest --test-dir workspace/cpp/build --output-on-failure
```

Expected result: PNG/JPEG validation, metadata extraction, palette reduction, channel mapping, grid conversion, and command-generation tests pass for 64, 128, and 256 grid sizes.

## 4. Build C Control Emulator

```bash
cmake -S workspace/control-c -B workspace/control-c/build
cmake --build workspace/control-c/build
ctest --test-dir workspace/control-c/build --output-on-failure
```

Expected result: TCP/IP command parser, lifecycle command handling, channel activation, pause/resume/reset, and status response tests pass.

## 5. Apply SQL Schema

```bash
sqlcmd -S localhost -d PatterningSimulator -E -i specs/001-digital-patterning-simulator/contracts/sql-schema.sql
sqlcmd -S localhost -d PatterningSimulator -E -Q "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME"
```

Expected result: design concepts, palette, channels, mappings, production grids, diagnostics, simulation runs, and simulation events tables are present.

## 6. Start Local TCP/IP Services

Run each component in separate terminals:

```bash
workspace/cpp/build/pattern_processor --port 5100
workspace/control-c/build/control_emulator --port 5110
dotnet run --project workspace/csharp/Patterning.GatewayHost -- --gateway plc --port 5120 --scenarios workspace/plc/scenarios/basic-run.json
dotnet run --project workspace/csharp/Patterning.GatewayHost -- --gateway fpga --port 5130 --signal-map workspace/fpga/signal_map.vhd
dotnet run --project workspace/csharp/PatterningOperatorDashboard
```

Expected result: the C# dashboard connects to each TCP/IP endpoint and shows machine status as ready.

## 7. Validate Upload And Analysis

1. Select the built-in generic sample design.
2. Confirm the original preview appears in the C# dashboard.
3. Confirm the C++ service returns metadata: dimensions, aspect ratio, estimated color count, file/source summary, and transparency/background indicators when available.
4. Upload a PNG or JPEG under 10 MB and no larger than 4096 x 4096 pixels.
5. Confirm unsupported types, files over 10 MB, and images over 4096 x 4096 pixels show validation errors and preserve the previous concept state.

## 8. Validate Palette, Channels, And Grid Conversion

1. Confirm palette extraction produces 4 to 16 representative swatches for standard samples.
2. Confirm coverage totals are within 2 percentage points of 100% for standard samples.
3. Confirm the default 8 generic manufacturing channels are visible.
4. Edit at least one channel label and color in the C# dashboard.
5. Confirm mappings update and each palette color is marked `exact`, `approximate`, or `unresolved`.
6. Convert the mapped design with 64 x 64, 128 x 128, and 256 x 256 grid choices.
7. Confirm grid dimensions, cell assignments, channel coverage, command estimates, channel-switch count, and fine-detail score are persisted to SQL.

## 9. Validate Diagnostics And Lifecycle Gating

1. Run a concept with unresolved mappings and confirm blocking diagnostics are generated.
2. Attempt to start simulation and confirm the C# orchestration layer does not send `run.start` over TCP/IP.
3. Run a warning-only concept and confirm `run.start` is sent to the C control emulator.
4. Confirm warning and info diagnostics remain visible and exportable.

## 10. Validate Simulation Lifecycle Over TCP/IP

1. Start the simulation from the dashboard.
2. Confirm line-by-line or pass-by-pass progress appears.
3. Confirm active channels, completed grid regions, command/event stream, machine status, rendered output, and diagnostics update during the run.
4. Pause and resume the simulation; each action should visibly update within 1 second.
5. Reset the simulation and confirm run progress clears while the concept, mapping, grid, and diagnostics remain.
6. Restart the simulator and confirm the active concept returns to initial sample-ready state.

## 11. Validate FPGA And PLC Stubs

FPGA stub validation:

```bash
ghdl -a workspace/fpga/signal_map.vhd workspace/fpga/signal_map_tb.vhd
ghdl -e SignalMap_TB
ghdl -r SignalMap_TB
```

PLC stub validation:

```bash
dotnet test workspace/csharp/PatterningSimulator.sln --configuration Debug --filter PlcGateway
dotnet run --project workspace/csharp/Patterning.GatewayHost -- --gateway plc --port 5120 --scenarios workspace/plc/scenarios/basic-run.json
```

FPGA gateway validation:

```bash
dotnet test workspace/csharp/PatterningSimulator.sln --configuration Debug --filter FpgaTimingGateway
dotnet run --project workspace/csharp/Patterning.GatewayHost -- --gateway fpga --port 5130 --signal-map workspace/fpga/signal_map.vhd
```

Expected result: signal timing maps valid channel frames, PLC lifecycle/interlock scenarios pass, and both stubs produce status or diagnostic messages compatible with the TCP/IP protocol.

## 12. Validate Report Export

1. Export the printable HTML report from the C# dashboard.
2. Export the structured JSON report data.
3. Confirm both outputs include source summary, preview, metadata, palette, channel mapping, grid summary, diagnostics, SQL-backed run history, and simulation results when a run exists.
4. Confirm both outputs avoid customer, site, restricted, and identifying names.
