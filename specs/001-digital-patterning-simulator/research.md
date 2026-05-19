# Research: Digital Patterning System Simulator

## Decision: Use C# For The Operator Dashboard, Orchestration, And Report Export

**Rationale**: C# is the strongest fit for Windows operator-facing tooling, typed domain models, SQL access, report generation, and TCP/IP orchestration. The proof-of-concept can expose a WPF dashboard or a C# desktop-hosted operator shell while keeping domain models testable with xUnit. C# also provides a clear bridge between design-facing workflow, machine-control protocol messages, and persisted run history.

**Alternatives considered**:
- Browser-only TypeScript dashboard: rejected because the requested target stack emphasizes C#, Windows, TCP/IP, SQL, and industrial operator tooling.
- Python/FastAPI orchestration: rejected because the user explicitly requested the industrial stack instead of repo-local choices.
- Native C++ dashboard: rejected because C++ is better reserved for performance-sensitive pattern processing and hardware-adjacent simulation.

## Decision: Use C++ For Image Normalization, Palette Reduction, Grid Conversion, And Command Generation

**Rationale**: C++ is appropriate for deterministic, performance-sensitive processing of PNG/JPEG-derived raster data into manufacturing-oriented grids and command streams. It keeps image sampling, color bucketing, channel mapping, and pass-by-pass command generation close to production-style systems without introducing external services.

**Alternatives considered**:
- C# image processing only: rejected because it does not demonstrate the requested C++ layer and can blur orchestration and compute responsibilities.
- FPGA-first image processing: rejected for the proof-of-concept because the FPGA layer should remain a signal-processing or command-timing stub, not the primary image decoder.
- External image processing service: rejected because the simulator must remain local and workshop-friendly.

## Decision: Use C For Control Emulation And TCP/IP Machine Protocol Stubs

**Rationale**: C is a realistic choice for low-level control emulation, deterministic command parsing, and simple TCP/IP adapters used by PLC-facing components. The C control emulator can receive command frames from C# or C++, simulate actuator/channel switching, and return status/event frames that the dashboard displays.

**Alternatives considered**:
- Implement control emulation in C#: rejected because it would not represent the lower-level control boundary in the requested stack.
- Implement control emulation in PLC only: rejected because a local C emulator is easier to build and validate in a desktop/Linux development environment while PLC stubs remain portable artifacts.

## Decision: Use SQL For Run History, Diagnostics, And Report Source Data

**Rationale**: The simulator needs repeatable run-history, diagnostics, and concept-report data. A SQL schema for concepts, palette colors, channel mappings, production grids, diagnostics, simulation runs, and command events gives the proof-of-concept an industrial data backbone. SQL Server is the preferred Windows/C# target; PostgreSQL-compatible DDL can be deferred unless the implementation environment requires it.

**Alternatives considered**:
- In-memory-only state: rejected because the requested stack includes SQL and the report/run-history area benefits from durable data.
- File-only JSON persistence: rejected because it does not exercise SQL schema design, queries, or run-history inspection.

## Decision: Use TCP/IP As The Integration Boundary Between Pattern Engine, Control Emulator, PLC Stub, And Dashboard

**Rationale**: TCP/IP is explicitly part of the requested stack and is a natural boundary for industrial software simulation. The command protocol can carry pattern pass commands, channel activation frames, lifecycle commands, status messages, and diagnostics between components without binding the design to a single process.

**Alternatives considered**:
- In-process method calls only: rejected because they hide integration risks and do not demonstrate machine-network boundaries.
- Message queues or cloud brokers: rejected because they add services outside the requested stack.

## Decision: Use PLC Structured Text Stubs For Machine Control Logic

**Rationale**: PLCs are part of the requested stack and should appear as explicit artifacts. Structured Text stubs can model start, pause, resume, reset, channel activation, actuator switching, interlocks, and diagnostic latch behavior while remaining portable across common PLC tooling.

**Alternatives considered**:
- Real PLC runtime dependency: rejected for the proof-of-concept because it would make the workshop hard to reproduce.
- No PLC artifact: rejected because the requested stack calls out PLCs directly.

## Decision: Use FPGA VHDL Stubs For Signal Timing And Channel Output Mapping

**Rationale**: FPGA involvement should be represented by portable VHDL stubs that translate production-grid commands or channel frames into timing-oriented signals. This demonstrates the hardware-adjacent layer without requiring synthesis or device access.

**Alternatives considered**:
- Production FPGA bitstream: rejected as out of scope for a proof-of-concept.
- C++ replacement for FPGA logic: rejected because the requested stack explicitly includes FPGA.

## Decision: Target Windows Operator Use With Linux-Compatible Processing And Control Builds

**Rationale**: Windows is the natural target for C# operator tooling and PLC engineering workflows. Linux compatibility matters for C++ processing, C control emulation, CI validation, and containerized workshop environments. The plan should keep cross-platform boundaries explicit: C# dashboard on Windows, C++/C command-line services buildable on Windows and Linux where possible.

**Alternatives considered**:
- Windows-only implementation: rejected because the requested stack includes Linux and the repo runs in a Linux dev container.
- Linux-only implementation: rejected because operator and PLC-adjacent workflows often require Windows.

## Decision: Reports Export As Printable HTML And Structured JSON From C# Domain Models

**Rationale**: Printable HTML supports stakeholder review. JSON supports automated validation and downstream tooling. Generating both from C# report models keeps the output aligned with SQL run history and avoids PDF dependencies.

**Alternatives considered**:
- PDF generation: rejected because it adds rendering dependencies without being required by the clarified spec.
- JSON only: rejected because stakeholders need a readable report artifact.

## Decision: Diagnostics Severity Controls Lifecycle Gating

**Rationale**: Clarification established that blocking errors prevent simulation while warnings and informational findings do not. This rule should be enforced in the C# orchestration layer before start commands are sent over TCP/IP to the control emulator, PLC stub, or FPGA simulation layer.

**Alternatives considered**:
- Always allow simulation: rejected because blocking validation would have no enforcement.
- Block warnings too: rejected because warnings should remain advisory and exportable.
