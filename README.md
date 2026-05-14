# Millitron Digital Patterning System

> **GitHub Copilot · Agentic DevOps · Deep Dive Workshop**
> *A fictitious-but-realistic day-in-the-life scenario for a Development Software Engineer on the Floorcovering
> Development (Digital Patterning Systems) team — Spartanburg, SC.*

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Python 3.10+](https://img.shields.io/badge/Python-3.10%2B-green.svg)](https://python.org)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.x-blue.svg)](https://www.typescriptlang.org/)
[![.NET 8](https://img.shields.io/badge/.NET-8-purple.svg)](https://dotnet.microsoft.com/)
[![GitHub Copilot](https://img.shields.io/badge/GitHub%20Copilot-Enabled-black.svg)](https://github.com/features/copilot)

---

## Overview

The **Millitron Digital Patterning System** is a proprietary industrial software platform that drives digital textile
printing machines used in floorcovering manufacturing. Engineers on the Floorcovering Development team architect,
implement, test, and deploy software spanning real-time PLC control, FPGA signal processing, a C# / .NET WPF operator
dashboard, and a TypeScript / React web-based design portal — all connected through a TCP/IP fabric and backed by SQL
Server.

This repository serves as a GitHub Copilot workshop that demonstrates **Agentic DevOps** workflows across the full
Millitron stack. Participants follow a realistic _day-in-the-life_ scenario for a Development Software Engineer:
morning stand-up, feature branch, Copilot-assisted implementation across C# / FPGA / PLC layers, PR review, CI/CD
pipeline, and Azure IoT Edge deployment — all with GitHub Copilot as an active pair-programmer.

### Workshop Infographic

![Millitron System Infographic](docs/images/millitron-infographic.svg)

---

## Day in the Life: Development Software Engineer

| Time | Activity | Copilot Role |
| --- | --- | --- |
| 08:00 | Daily stand-up; pick up sprint ticket #142 — *"Add dye-head misfire detection to pattern renderer"* | Copilot Chat summarizes ticket context |
| 08:30 | Explore `PatternRenderer.cs` and FPGA signal map | Copilot Explain walks through signal-to-pixel mapping |
| 09:00 | Generate `DyeHeadMisfireDetector` class with unit tests | Copilot generates C# class, xUnit tests, and SQL migration |
| 10:30 | Run PLC simulation harness; validate signal round-trip | Copilot suggests edge-case assertions |
| 11:00 | Open PR; Copilot writes PR description from diff | Copilot PR summarizer, inline review comments |
| 13:00 | Address review feedback on FPGA timing constants | Copilot Fix applied to flagged lines |
| 14:00 | CI passes; merge to `main`; Azure IoT Edge deploy starts | GitHub Actions + `azd deploy` |
| 15:00 | Monitor Grafana dashboard in Codespaces port-forward | Copilot explains anomaly in telemetry log |
| 16:00 | Write ADR for misfire-detection algorithm choice | Copilot drafts ADR from discussion thread |
| 16:30 | Demo to design team; update sprint board | Copilot Chat generates demo script |

---

## Technology Stack

| Layer | Technologies |
| --- | --- |
| **Operator Dashboard** | C# · .NET 8 · WPF · XAML |
| **Web Design Portal** | TypeScript · React 18 · Node.js |
| **Backend Services** | C# · .NET 8 Web API · SignalR |
| **Pattern Engine** | C++ · FPGA (VHDL / Verilog) · real-time DSP |
| **Machine Control** | C · PLC (IEC 61131-3 Structured Text) · TCP/IP |
| **Data Store** | SQL Server · T-SQL · Entity Framework Core |
| **DevOps** | GitHub Actions · Azure DevOps · Azure Container Apps |
| **Edge / IoT** | Azure IoT Edge · MQTT · OPC-UA |
| **AI / ML** | GitHub Copilot · Azure ML · ONNX inference |
| **OS / Runtime** | Windows (WPF) · Linux (Edge modules) · RTOS (PLC) |
| **IDE** | VS Code · Visual Studio 2022 |

---

## Project Layout

```text
/
├── workspace/                  # Runnable simulation (FastAPI + TypeScript)
│   ├── api/                    #   FastAPI backend, routes, WebSocket lifecycle
│   ├── simulation/             #   Pattern engine, dispatcher, tick model
│   ├── tests/                  #   unittest regression suite
│   ├── ui/                     #   HTML template, TypeScript source, CSS, JS
│   └── scripts/                #   Convenience wrappers
├── docs/
│   ├── prd-digital-patterning-system.md   # Product Requirements Document
│   ├── prd-elevator-dispatch.md           # Workshop elevator-dispatch PRD
│   └── images/                 #   Architecture diagrams, screenshots, infographic
├── .github/
│   ├── copilot-instructions.md # Global Copilot context
│   ├── prompts/                # Reusable .prompt.md files for Copilot agent mode
│   ├── instructions/           # Path-scoped Copilot instructions
│   ├── skills/                 # Self-contained skill packages (SKILL.md + assets)
│   └── agents/                 # Agent definitions
└── .devcontainer/              # Codespaces / Dev Container configuration
```

---

## Prerequisites

| Setup Path | Requirements |
| --- | --- |
| GitHub Codespaces | Devcontainer installs all dependencies automatically. |
| VS Code Dev Containers | Docker Desktop + Dev Containers extension. |
| Manual local | Python 3.10+, Node.js LTS, npm, Git. |
| Full C# / .NET stack | .NET 8 SDK, Visual Studio 2022 or VS Code with C# Dev Kit. |
| FPGA simulation | ModelSim or Vivado Simulator (local install; not in devcontainer). |
| PLC runtime | CODESYS or TwinCAT SoftPLC (Windows host). |

---

## Quick Start (Simulation Layer)

```bash
# 1. Clone or open in Codespaces
# Note: this is the completed workshop solution repository. A starter template may be available
# from your facilitator. Replace the URL below with your assigned repository if different.
git clone https://github.com/ms-mfg-community/ghcp-digital-patterning-system-completed.git
cd ghcp-digital-patterning-system-completed/workspace

# 2. Create virtual environment and install Python dependencies
python -m venv .venv
source .venv/bin/activate          # Windows: .venv\Scripts\Activate.ps1
pip install -r requirements.txt

# 3. Install TypeScript / Node dependencies
npm install

# 4. Start the simulation API
python -m uvicorn api.server:app --host 0.0.0.0 --reload --port 7000

# 5. Open the dashboard
open http://127.0.0.1:7000
```

### With PostgreSQL Persistence

```bash
DATABASE_URL=postgresql://patterning:patterning@postgres:5432/patterning_db \
  python -m uvicorn api.server:app --host 0.0.0.0 --reload --port 7000
```

---

## Running Tests

```bash
cd workspace
source .venv/bin/activate
python -m compileall api simulation tests
python -m unittest discover -s tests -v
npm run build
```

---

## GitHub Copilot Integration

This repository ships with a complete set of Copilot customizations:

### Custom Prompts (`.github/prompts/`)

| Prompt File | Purpose |
| --- | --- |
| `00.00.meta-prompt.prompt.md` | Meta-prompt for Copilot agent orientation |
| `00.00.update-readme-and-prd.prompt.md` | Update README and PRD from sprint changes |
| `01.00.initialize-project.prompt.md` | Bootstrap the simulation workspace |
| `10.00.fpga-signal-map.prompt.md` | Generate FPGA signal-to-pixel mapping stubs |
| `10.01.plc-dye-head-control.prompt.md` | Scaffold PLC dye-head control routines |
| `10.02.pattern-renderer-csharp.prompt.md` | Generate C# pattern rendering classes |
| `10.03.misfire-detection.prompt.md` | Add misfire detection with unit tests |
| `10.04.sql-schema-migration.prompt.md` | Generate T-SQL migration for pattern events |
| `10.05.azure-iot-edge-deploy.prompt.md` | Deploy pattern engine module to Azure IoT Edge |

### Path-Scoped Instructions (`.github/instructions/`)

| Instruction File | Scope |
| --- | --- |
| `digital-patterning.instructions.md` | All files under `workspace/**` |
| `csharp-wpf.instructions.md` | C# and WPF source files |
| `fpga-vhdl.instructions.md` | VHDL / Verilog source files |
| `unittest-conventions.instructions.md` | Test files under `workspace/tests/` |
| `azure-deployment.instructions.md` | Azure IaC and deployment files |
| `ui-typescript.instructions.md` | TypeScript files under `workspace/ui/` |

### Skills (`.github/skills/`)

| Skill | Description |
| --- | --- |
| `fpga-simulation` | Self-contained skill to scaffold and run FPGA signal simulation stubs |
| `plc-integration` | Skill to generate and validate PLC integration wiring |
| `postgres-devcontainer` | Add PostgreSQL sidecar to devcontainer |
| `postgres-schema-inspection` | Inspect and test the pattern-events schema |
| `postgres-data-persistence` | Wire pattern event writes to PostgreSQL |
| `add-basement-level` | Add sub-floor support to the simulation (elevator workshop reference) |

### Agents (`.github/agents/`)

Custom agent definitions are stored in `.github/agents/`. Agents have access to the repository, devcontainer runtime,
and Copilot coding capabilities. See individual agent YAML files for trigger conditions and tool permissions.

---

## Architecture Overview

```text
┌──────────────────────────────────────────────────────────────────────────────┐
│                        Millitron Digital Patterning System                   │
│                                                                              │
│  ┌─────────────────┐   TCP/IP   ┌──────────────────┐   FPGA Bus             │
│  │  WPF Operator   │◄──────────►│  C# Pattern      │◄──────────►┌──────────┐│
│  │  Dashboard      │            │  Engine Service  │            │  FPGA    ││
│  │  (.NET 8 / WPF) │            │  (.NET 8 Web API) │           │  DSP     ││
│  └─────────────────┘            └──────────────────┘            │  Module  ││
│                                         │                        └──────────┘│
│  ┌─────────────────┐           SignalR  │  SQL Server                        │
│  │  React / TS     │◄──────────────────►│  ┌────────────────────────────┐   │
│  │  Design Portal  │                    │  │  pattern_jobs              │   │
│  │  (Node.js)      │                    │  │  dye_head_events           │   │
│  └─────────────────┘                    │  │  pattern_definitions       │   │
│                                         │  └────────────────────────────┘   │
│  ┌─────────────────────────────────┐    │                                   │
│  │  PLC Control Layer              │    │  Azure IoT Edge                   │
│  │  (IEC 61131-3 Structured Text)  │◄───┘  ┌─────────────────────────────┐  │
│  │  Dye heads · Conveyors · Vision │       │  Pattern Engine Module      │  │
│  └─────────────────────────────────┘       │  Telemetry · MQTT · OPC-UA  │  │
│                                            └─────────────────────────────┘  │
└──────────────────────────────────────────────────────────────────────────────┘
```

---

## Workshop Labs

| Lab | Title | Key Skills |
| --- | --- | --- |
| Lab 01 | Initialize project & run simulation | Python, FastAPI, Copilot Chat |
| Lab 02 | Add PostgreSQL persistence | Docker, SQL, Copilot agent mode |
| Lab 03 | PR review with Copilot | Git, PR workflow, inline review |
| Lab 04 | Migrate to Azure | Azure Container Apps, `azd`, IaC |
| Lab 10 | FPGA signal map scaffolding | FPGA, VHDL, Copilot prompt files |
| Lab 11 | PLC dye-head control | PLC, Structured Text, Copilot skills |
| Lab 12 | C# pattern renderer | C#, .NET, WPF, xUnit |
| Lab 13 | Misfire detection + SQL migration | C#, T-SQL, Entity Framework, tests |
| Lab 14 | Azure IoT Edge deployment | Azure IoT Edge, MQTT, `azd` |

---

## Azure Deployment

The production Millitron pattern engine is deployed to **Azure Container Apps** with an optional **Azure Database for
PostgreSQL Flexible Server** for event persistence. The reference workshop endpoint below is an example — replace it
with your own deployment URL after running `azd deploy`:

```
https://ca-patterning-dev.<your-unique-suffix>.eastus2.azurecontainerapps.io/
```

Deployment commands:

```bash
azd env get-values
azd deploy
az containerapp revision list \
  --name ca-patterning-dev \
  --resource-group rg-patterning \
  --query '[].{name:name,active:properties.active,traffic:properties.trafficWeight}' \
  -o table
```

---

## Extension Points

| Area | Good Workshop Changes |
| --- | --- |
| `simulation/dispatcher.py` | Try alternative pattern dispatch heuristics. |
| `simulation/simulation.py` | Adjust tick lifecycle, dye-head spawning, pause/resume. |
| `api/database.py` | Extend optional pattern-event persistence. |
| `api/server.py` | Add validated endpoints for pattern jobs or dye-head status. |
| `ui/main.ts` / `ui/static/styles.css` | Extend the live patterning dashboard. |
| `tests/` | Add focused `unittest` coverage for pattern and dispatcher changes. |

---

## Contributing

1. Fork the repository or open it in GitHub Codespaces.
2. Create a feature branch: `git checkout -b feature/my-change`
3. Make changes with GitHub Copilot assistance.
4. Run tests: `python -m unittest discover -s workspace/tests -v`
5. Open a pull request — Copilot will summarize your diff automatically.

See [CONTRIBUTING.md](CONTRIBUTING.md) if present, or the workshop facilitator guide in `docs/`.

---

## Role Context: Development Software Engineer

**Location:** Spartanburg, SC · **Team:** Floorcovering Development (Digital Patterning Systems)
**Salary range:** $75K – $95K · **Education:** BS minimum (Computer Engineering preferred); MS/PhD considered

**Responsibilities:** Architect, design, implement, test, install, and document software applications for lab,
industrial, and design environments globally. System-architecture freedom in a small-team, fast-paced environment.

**Stack signals:** C# · C++ · C · Visual Basic · SQL · TCP/IP · Windows · Linux · FPGA · PLCs · Robotics

**Why GitHub Copilot matters here:**
- Proprietary industrial software (Millitron) spans 6+ languages and real-time hardware interfaces.
- Small team; each engineer owns broad vertical slices (FPGA ↔ PLC ↔ C# ↔ SQL ↔ UI).
- Copilot accelerates context switching across languages, generates hardware-interface stubs, and drafts SQL migrations.
- Azure DevOps · Azure IoT Edge · Azure App Service modernization stories map directly to this stack.

---

## License

[MIT](LICENSE) © Milliken & Company (fictitious workshop scenario)

---

## Related Documentation

- [Product Requirements Document](docs/prd-digital-patterning-system.md)
- [Elevator Dispatch Workshop PRD](docs/prd-elevator-dispatch.md)
- [Azure Deployment Instructions](.github/instructions/azure-deployment.instructions.md)
- [Copilot Instructions](.github/copilot-instructions.md)
