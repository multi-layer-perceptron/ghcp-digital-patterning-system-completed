# Digital Patterning Simulator Workspace

This workspace contains the industrial-stack proof-of-concept implementation for the Digital Patterning System Simulator.

## Components

- `csharp/`: C# operator dashboard, gateway host, domain models, infrastructure adapters, and xUnit tests.
- `cpp/`: C++ pattern processor for validation, metadata, palette, mapping, grid conversion, and command generation.
- `control-c/`: C control emulator and TCP/IP protocol helpers.
- `sql/`: SQL Server-compatible schema migrations and validation notes.
- `plc/`: PLC Structured Text stubs and lifecycle scenarios.
- `fpga/`: VHDL signal mapping stubs and testbench.
- `assets/samples/`: Generic confidentiality-safe demonstration assets.

## Validation

Follow `../specs/001-digital-patterning-simulator/quickstart.md` for the full validation flow.
