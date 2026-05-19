# Implementation Plan: Digital Patterning System Simulator

**Branch**: `001-digital-patterning-simulator` | **Date**: 2026-05-19 | **Spec**: `specs/001-digital-patterning-simulator/spec.md`

**Input**: Feature specification from `specs/001-digital-patterning-simulator/spec.md`

## Summary

Build a confidentiality-safe proof-of-concept simulator that accepts generic floorcovering design images, extracts metadata and palettes, maps colors to editable manufacturing channels, converts mapped designs into production grids, simulates production progress, and exports printable HTML plus structured JSON reports.

The implementation uses the requested industrial stack: C# for the operator dashboard, orchestration, SQL access, TCP/IP coordination, gateway hosting, and report export; C++ for image analysis, palette reduction, channel mapping, grid conversion, and command generation; C for the control emulator and low-level TCP/IP protocol handling; SQL Server-compatible DDL for run history and diagnostics; PLC Structured Text and FPGA VHDL stubs for industrial integration demonstrations.

## Technical Context

**Language/Version**: C#/.NET, C++17 or later, C11, SQL Server-compatible SQL, PLC Structured Text, VHDL

**Primary Dependencies**: xUnit, CMake/CTest, SQL Server tooling, TCP/IP JSON Lines protocol, GHDL or equivalent VHDL validation tooling

**Storage**: SQL Server-compatible relational schema for concepts, palettes, mappings, grids, diagnostics, simulation runs, and events

**Testing**: xUnit for C#, CTest for C++/C, SQL schema validation, PLC stub validation, FPGA VHDL testbench validation

**Target Platform**: Windows operator dashboard; Windows/Linux-compatible C++ and C services; Linux dev container validation

**Project Type**: Multi-component industrial simulator proof-of-concept

**Performance Goals**: Upload analysis visible within 5 seconds for standard samples; lifecycle UI feedback within 1 second; report export within 30 seconds

**Constraints**: Generic/confidentiality-safe terminology; no production equipment control; local TCP/IP simulation only; PNG/JPEG up to 10 MB and 4096 x 4096 pixels

**Scale/Scope**: Workshop proof-of-concept with one active concept session, one sample design set, 8 editable channels, and 64/128/256 grid sizes

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Confidentiality-safe artifacts: PASS. Spec, plan, tasks, samples, reports, and generated text must avoid restricted names, site names, identifying references, and production-sensitive details.
- Industrial stack alignment: PASS. Implementation tasks target C#, C++, C, SQL, TCP/IP, Windows, Linux, FPGA, and PLCs only.
- Testable independent stories: PASS. Tasks include story-scoped tests before implementation tasks and checkpoints after each story.
- Simulated control boundaries: PASS. TCP/IP, PLC, and FPGA integrations are local proof-of-concept stubs or emulators and must not control production equipment.
- Explicit quality gates: PASS. Tasks include build, unit, contract/protocol, SQL, PLC, FPGA, timing, and confidentiality validation.

## Project Structure

### Documentation (this feature)

```text
specs/001-digital-patterning-simulator/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   ├── sql-schema.sql
│   └── tcp-command-protocol.md
└── tasks.md
```

### Source Code (repository root)

```text
workspace/
├── csharp/
│   ├── PatterningSimulator.sln
│   ├── PatterningOperatorDashboard/
│   ├── Patterning.GatewayHost/
│   ├── Patterning.Core/
│   ├── Patterning.Infrastructure/
│   └── Patterning.Tests/
├── cpp/
│   ├── include/
│   ├── src/
│   └── tests/
├── control-c/
│   ├── include/
│   ├── src/
│   └── tests/
├── sql/
│   └── migrations/
├── plc/
├── fpga/
└── assets/
    └── samples/
```

**Structure Decision**: Use a multi-component workspace rooted under `workspace/` so each industrial layer can be built and validated independently while sharing the contracts in `specs/001-digital-patterning-simulator/contracts/`.

## Technical Approach

### Component Responsibilities

| Component | Responsibility | Primary Paths |
| --- | --- | --- |
| C# operator dashboard | Upload workflow, session orchestration, channel editing, simulation dashboard, report export | `workspace/csharp/PatterningOperatorDashboard/` |
| C# gateway host | Executable local TCP/IP host for PLC and FPGA gateway stubs | `workspace/csharp/Patterning.GatewayHost/` |
| C# core and infrastructure | Domain models, validation, SQL repositories, TCP/IP clients, report builders | `workspace/csharp/Patterning.Core/`, `workspace/csharp/Patterning.Infrastructure/` |
| C++ pattern processor | Image validation, metadata extraction, palette extraction, channel mapping, grid conversion, command generation | `workspace/cpp/` |
| C control emulator | TCP/IP command server, lifecycle state machine, channel activation events, machine status frames | `workspace/control-c/` |
| SQL schema | Concepts, palettes, channels, mappings, grids, diagnostics, runs, and events | `workspace/sql/`, `specs/001-digital-patterning-simulator/contracts/sql-schema.sql` |
| PLC stub | Structured Text control logic and C# gateway stub for validation scenarios | `workspace/plc/` |
| FPGA stub | VHDL signal map and C# timing gateway stub for validation scenarios | `workspace/fpga/` |
| Sample assets | Generic, confidentiality-safe demonstration inputs | `workspace/assets/samples/` |

### Data Model

Use the entities defined in `data-model.md`: `DesignConcept`, `ImageMetadata`, `ColorPalette`, `ManufacturingChannel`, `ChannelMapping`, `ProductionGridModel`, `ManufacturabilityDiagnostic`, `SimulationRun`, and `ConceptReport`.

Persist run-history and report-source data through the SQL schema contract. Keep active operator state in the C# orchestration layer during the session and write concept, mapping, diagnostic, grid, run, and event records when they become report-relevant.

### Protocol Model

Use newline-delimited JSON over local TCP/IP for proof-of-concept service boundaries. The C# dashboard/orchestrator coordinates with the C++ pattern processor, C control emulator, PLC gateway stub, and FPGA timing gateway stub through `tcp-command-protocol.md`.

The local ports are:

- `5100`: C++ pattern processor
- `5110`: C control emulator
- `5120`: PLC gateway stub
- `5130`: FPGA timing gateway stub

### Key Flows

1. Upload or select a generic sample image in the C# dashboard.
2. Validate PNG/JPEG constraints in C# and C++, then send `concept.analyze` to the C++ pattern processor.
3. Return image metadata, palette entries, and source summary to the dashboard.
4. Edit or accept the 8 generic manufacturing channels, then calculate exact, approximate, or unresolved mappings.
5. Convert the mapped design to a 64, 128, or 256 production grid and persist grid summaries to SQL.
6. Generate diagnostics; block simulation start when blocking errors are present.
7. Start warning-only simulations through TCP/IP command frames and update dashboard progress from emulator/gateway status frames.
8. Export printable HTML and structured JSON reports from SQL-backed concept, mapping, diagnostic, grid, and run data.

## Phase Plan

### Phase 0: Research

Research decisions are captured in `research.md` and lock the requested industrial stack, TCP/IP boundaries, SQL persistence approach, report format, and diagnostics gating behavior.

### Phase 1: Design

Design outputs include `data-model.md`, `contracts/sql-schema.sql`, `contracts/tcp-command-protocol.md`, and `quickstart.md`.

### Phase 2: Tasks

Tasks are generated in `tasks.md` and organized by setup, foundation, each independently testable user story, and polish/validation.

## Quality Gates

- C# solution restores, builds, and passes xUnit tests, including the executable gateway host project.
- C++ and C CMake projects build and pass CTest suites.
- SQL schema applies cleanly and exposes required tables.
- TCP/IP protocol frames match the contract across C#, C++, C, PLC gateway, and FPGA timing gateway components.
- PLC Structured Text and FPGA VHDL stubs validate with the selected local tooling.
- Standard sample analysis appears within 5 seconds, lifecycle actions reflect within 1 second, and report export completes within 30 seconds.
- Confidentiality scan reports zero restricted names, site names, or identifying references.

## Complexity Tracking

No constitution violations are expected. The multi-component structure is necessary because the user explicitly selected an industrial stack spanning C#, C++, C, SQL, TCP/IP, Windows, Linux, FPGA, and PLCs.
