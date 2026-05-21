# AGENTS.md

This file is the shared operating guide for coding agents working in this repository. It complements
[.github/copilot-instructions.md](.github/copilot-instructions.md), which remains the detailed source of repository
conventions.

## Project Context

This repository is a GitHub Copilot workshop reference implementation for an industrial digital patterning simulator.
The application code lives under `workspace/`; repository-level documents, prompts, skills, PRDs, and Spec Kit artifacts
live outside `workspace/`.

The simulator demonstrates a workflow from design upload through palette extraction, manufacturing-channel mapping,
production-grid conversion, lifecycle simulation, gateway stubs, and report export.

## Agent Rules

- Keep generated application code under `workspace/` unless the user explicitly asks to change repository-level docs,
  prompt files, specs, or configuration.
- Preserve the educational workshop style: small modules, clear names, explicit state transitions, and simple heuristics
  that participants can understand.
- Do not add authentication, queues, cloud services, or mandatory persistent storage unless the user asks.
- Keep live simulator state in memory. Database support is optional and should only persist run metadata and events when
  configured.
- Keep C# domain behavior in `workspace/csharp/Patterning.Core/` and WPF dashboard behavior in
  `workspace/csharp/PatterningOperatorDashboard/`.
- Keep changes focused on the current request. Avoid unrelated refactors and generated-file churn.
- Do not modify files under `completed/` unless the user explicitly asks.
- Add or update tests when changing domain behavior, mapping rules, report output, gateway contracts, or dashboard state
  transitions.

## Key Areas

| Area | Responsibility |
| --- | --- |
| `workspace/csharp/Patterning.Core/` | Domain models, services, validation, diagnostics, simulation lifecycle, reports. |
| `workspace/csharp/PatterningOperatorDashboard/` | WPF operator dashboard tabs: Upload, Channels, Simulation, Reports. |
| `workspace/csharp/Patterning.Infrastructure/` | SQL repositories, TCP clients, PLC/FPGA gateway adapters. |
| `workspace/csharp/Patterning.GatewayHost/` | CLI host for PLC and FPGA gateway proof stubs. |
| `workspace/cpp/` | C++ image metadata, palette, grid, and command-processing examples. |
| `workspace/control-c/` | C lifecycle/control emulator and protocol helpers. |
| `workspace/fpga/` | VHDL signal-map model and GHDL testbench. |
| `workspace/plc/` | Structured Text lifecycle stub and scenario fixtures. |
| `workspace/sql/` | SQL contract, migrations, SQLite validation runner. |
| `docs/` | PRDs and explanatory workshop documentation. |
| `specs/` | Spec Kit-generated feature specs, plans, tasks, contracts, and checklists. |

## Validation Commands

Run commands from the repository root unless noted otherwise.

```powershell
dotnet build workspace/csharp/PatterningSimulator.sln -nologo -v minimal
dotnet test workspace/csharp/PatterningSimulator.sln --configuration Debug
```

For native Windows validation from a long OneDrive path, prefer short external CMake build directories:

```powershell
cmake -S workspace/cpp -B C:/temp/ghcp-digital-patterning-system-completed/cpp-build
cmake --build C:/temp/ghcp-digital-patterning-system-completed/cpp-build --config Debug
ctest --test-dir C:/temp/ghcp-digital-patterning-system-completed/cpp-build -C Debug --output-on-failure

cmake -S workspace/control-c -B C:/temp/ghcp-digital-patterning-system-completed/control-c-build
cmake --build C:/temp/ghcp-digital-patterning-system-completed/control-c-build --config Debug
ctest --test-dir C:/temp/ghcp-digital-patterning-system-completed/control-c-build -C Debug --output-on-failure
```

To run the WPF dashboard on Windows:

```powershell
dotnet run --project workspace/csharp/PatterningOperatorDashboard
```

## Multi-Agent Routing

Use these roles when coordinating feature work across agents:

| Agent Role | Use For |
| --- | --- |
| Product/spec agent | PRDs, Spec Kit specs, user stories, acceptance criteria, open questions. |
| C# domain agent | `Patterning.Core` models, services, diagnostics, reports, and xUnit coverage. |
| WPF agent | Dashboard XAML, view models, tab workflows, and operator UX behavior. |
| Native agent | C++, C, CMake, grid conversion, command generation, protocol helper tests. |
| FPGA/PLC agent | VHDL, Structured Text, gateway scenarios, timing and lifecycle contracts. |
| SQL agent | SQL schema contracts, migrations, SQLite validation, optional persistence boundaries. |
| Docs agent | README, PRDs, tutorials, glossary, diagrams, workshop facilitator notes. |
| Review agent | Diff review, behavioral regressions, missing tests, risk analysis. |

## Recommended Feature Workflow

For broad changes, use this sequence:

1. Capture product intent in `docs/prd-<feature-name>.md`.
2. Run `/speckit.specify` against the PRD to generate `specs/NNN-<feature-name>/spec.md`.
3. Run `/speckit.clarify` if the spec contains unresolved questions.
4. Run `/speckit.plan`, then `/speckit.tasks`.
5. Implement with the appropriate domain, WPF, test, and docs agents.
6. Run targeted validation commands.
7. Ask a review agent to inspect the diff before opening a pull request.

## Current Demo Scenario

A useful multi-agent workshop scenario is channel preset orchestration:

1. Diagnose unresolved palette-to-channel mappings in the WPF Channels tab.
2. Specify a channel naming/preset feature with enumeration values such as `physical-material`, `machine-port`,
   `role-function`, `industrial-standard`, and `hybrid-channel-color`.
3. Implement preset models and services in `Patterning.Core`.
4. Add a preset selector to the Channels tab.
5. Validate that the sample floorcovering palette can map to low-delta or exact channels.
6. Update README, PRD, and reports so the feature is explainable to workshop participants.
