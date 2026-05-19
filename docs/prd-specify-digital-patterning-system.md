# Product Requirements Document: Digital Patterning System

## Document Control

- File name: `docs/prd-specify-digital-patterning-system.md`
- Owner: Floorcovering Development team lead
- Stakeholders: Development Software Engineers, Design team, Manufacturing Operations, IT/DevOps
- Status: Approved
- Created: 2026-05-14
- Last updated: 2026-05-14
- Target release or lab milestone: Workshop Labs 10–14 (FPGA, PLC, C#, SQL, Azure IoT Edge)

---

## Summary

The **Digital Patterning System** is a realistic industrial software reference platform that controls and monitors
digital textile printing machines used in floorcovering manufacturing. The system spans a real-time PLC control layer,
an FPGA-based signal-processing module, a C# / .NET WPF operator dashboard, a TypeScript / React web-based design
portal, and a SQL Server-backed event store — all connected through a TCP/IP fabric and optionally extended to Azure
IoT Edge for remote telemetry and cloud-connected monitoring.

This PRD defines the requirements for the workshop simulation of that system, which serves as hands-on material for a
GitHub Copilot _Agentic DevOps_ deep-dive. The simulation runs locally or in GitHub Codespaces and demonstrates how
Copilot accelerates engineering workflows across C#, FPGA, PLC, SQL, and TypeScript layers.

---

## Problem Statement

Floorcovering development engineers in industrial manufacturing environments routinely context-switch across six or more
programming languages and hardware-interface layers in a single sprint. Onboarding new engineers, maintaining
documentation, and catching cross-layer defects (e.g., a PLC timing constant that disagrees with an FPGA register
offset) are slow and error-prone without AI assistance.

GitHub Copilot's agentic capabilities — prompt files, path-scoped instructions, skills, and agent mode — can materially
reduce this friction. This workshop demonstrates those capabilities using a realistic digital patterning scenario:
_sprint ticket #142: Add dye-head misfire detection to the pattern renderer_.

---

## Goals

- Demonstrate GitHub Copilot Agentic DevOps workflows across the full digital patterning technology stack.
- Give participants hands-on experience with Copilot prompt files, instructions, skills, and agent mode on real
  industrial-software patterns (FPGA signal maps, PLC routines, C# services, T-SQL migrations).
- Produce a reusable reference simulation that can be extended with real C# / .NET WPF and FPGA components without
  replacing the Python/TypeScript simulation core.
- Validate that Copilot accelerates cross-language context switching for small-team industrial-software engineers.

---

## Non-Goals

- Shipping production-quality FPGA bitstreams or PLC programs (simulation stubs only for the workshop).
- Full Azure IoT Edge deployment in every lab (covered in Lab 14 as an optional advanced track).
- Authentication, authorization, or multi-tenant design.
- Machine learning model training (ONNX inference stubs are referenced but not trained in the workshop).

---

## Users and Personas

| Persona | Needs | Success Looks Like |
| --- | --- | --- |
| Development Software Engineer | Explore cross-layer features fast with Copilot | Sprint ticket resolved in <1 day with Copilot assistance |
| Workshop Participant | Learn Copilot Agentic DevOps hands-on | Completes Lab 10–14 without facilitator intervention |
| Workshop Facilitator | Demo and reset the system reliably | One-command reset; all labs reproducible in Codespaces |
| Design Team | Visualize pattern output in real time | Design portal shows live patterning with accurate color rendering |
| Manufacturing Operator | Monitor machine health from WPF dashboard | Dashboard shows dye-head status, misfire alerts, and pattern progress |

---

## Use Cases

### Use Case 1: Dye-Head Misfire Detection (Sprint Ticket #142)

- **Actor:** Development Software Engineer
- **Trigger:** PLC reports a zero-voltage pulse on dye-head channel 7 during a pattern job
- **Preconditions:** Pattern job is running; FPGA signal monitor is active; SQL event store is recording
- **Main flow:**
  1. Engineer opens sprint ticket #142 in GitHub Issues; Copilot Chat summarizes the context.
  2. Engineer uses Copilot agent mode with `10.03.misfire-detection.prompt.md` to generate `DyeHeadMisfireDetector.cs`.
  3. Copilot generates xUnit tests and a T-SQL migration for the `dye_head_events` table.
  4. Engineer runs the PLC simulation harness; validates the signal round-trip against FPGA timing constants.
  5. PR is opened; Copilot writes the PR description and flags a timing-constant inconsistency inline.
  6. Engineer resolves the review comment; CI passes; merge triggers Azure IoT Edge deploy.
- **Alternate flows:**
  - FPGA timing constant is out of range → Copilot Fix suggests corrected value with register-map reference.
  - SQL migration conflicts with existing schema → Copilot generates rollback script.
- **Outcome:** Misfire detection is live; operator dashboard shows misfire alerts; events are stored in SQL.

### Use Case 2: Pattern Rendering Visualization

- **Actor:** Design Team member
- **Trigger:** Design team member uploads a new `.pat` pattern file via the React design portal
- **Preconditions:** Pattern engine service is running; SQL pattern definitions table is seeded
- **Main flow:**
  1. User opens the React design portal and uploads a `.pat` file.
  2. Pattern engine parses the file and generates a pixel-color map via the FPGA signal map module.
  3. Dashboard renders the animated pattern with per-dye-head color overlays.
  4. User adjusts color saturation; change is pushed to the PLC layer and reflected in the dashboard within 200 ms.
- **Alternate flows:**
  - Invalid `.pat` file → API returns 422 with validation error; portal displays inline error.
  - PLC connection timeout → Dashboard shows "Machine Offline" banner; pattern preview still renders from last state.
- **Outcome:** Design team can preview and adjust patterns without a physical machine.

### Use Case 3: Azure IoT Edge Telemetry

- **Actor:** Manufacturing Operations engineer
- **Trigger:** Pattern engine module is deployed to Azure IoT Edge on the factory floor
- **Preconditions:** `azd deploy` completed; IoT Hub is configured; SQL event store is reachable
- **Main flow:**
  1. Operations engineer runs `azd deploy` with `azd env set AZURE_IOT_HUB_CONNECTION_STRING <value>`.
  2. Pattern engine module starts on the edge device and begins publishing MQTT telemetry to IoT Hub.
  3. Telemetry events (pattern start, dye-head status, misfire alerts) are visible in Azure Monitor.
  4. Operations engineer queries event history from the Azure Portal or via the SQL event store.
- **Alternate flows:**
  - Edge device loses connectivity → Module buffers telemetry locally; retries on reconnect.
- **Outcome:** Factory-floor telemetry is visible in Azure without VPN access to the machine network.

---

## Functional Requirements

| ID | Requirement | Priority | Notes |
| --- | --- | --- | --- |
| FR-001 | The system shall simulate a digital pattern engine with configurable dye-head count (default: 16) and pattern width (default: 1024 px) | Must | Simulation stub; FPGA module is a future extension |
| FR-002 | The system shall expose a REST API for pattern job submission, dye-head status, and misfire events | Must | FastAPI + Pydantic validation |
| FR-003 | The system shall stream live pattern state to the dashboard over WebSocket | Must | Same pattern as elevator dispatch workshop |
| FR-004 | The system shall write pattern job start/stop and dye-head misfire events to SQL when `DATABASE_URL` is configured | Must | Optional persistence; in-memory default |
| FR-005 | The system shall provide a C# `PatternRenderer` class with a `RenderFrame(PatternJob job)` method | Must | C# stub generated by Copilot in Lab 12 |
| FR-006 | The system shall provide a C# `DyeHeadMisfireDetector` class that raises `MisfireDetectedEvent` | Must | Generated by Copilot in Lab 10.03 |
| FR-007 | The system shall include a PLC simulation harness that exercises the dye-head control routine | Should | Structured Text stub; Lab 11 |
| FR-008 | The system shall include an FPGA signal-map stub that maps pattern bytes to dye-head pulse widths | Should | VHDL stub; Lab 10 |
| FR-009 | The dashboard shall display per-dye-head status (active / idle / misfired) with animated color overlays | Should | React design portal; Lab 12 |
| FR-010 | The system shall support deployment to Azure Container Apps and Azure IoT Edge via `azd` | Could | Lab 14 advanced track |
| FR-011 | The system shall include a T-SQL migration for `pattern_jobs`, `dye_head_events`, and `misfire_alerts` | Should | Generated by Copilot in Lab 10.04 |
| FR-012 | The system shall include xUnit tests for `PatternRenderer` and `DyeHeadMisfireDetector` | Must | Coverage required for merge |

---

## Non-Functional Requirements

| ID | Category | Requirement | Target |
| --- | --- | --- | --- |
| NFR-001 | Performance | Dashboard WebSocket update latency | ≤ 200 ms end-to-end |
| NFR-002 | Performance | Pattern frame render time (C# stub) | ≤ 50 ms per frame at 1024 px width |
| NFR-003 | Reliability | Simulation uptime in Codespaces without restart | ≥ 4 hours |
| NFR-004 | Reliability | PLC simulation harness must complete signal round-trip without timeout | 100% of test cases |
| NFR-005 | Security | No secrets committed to repository | Enforced by `.gitignore` and secret scanning |
| NFR-006 | Security | SQL parameterized queries only; no string concatenation | Enforced by code review and Copilot instructions |
| NFR-007 | Accessibility | Dashboard readable at 1280×720 and above; keyboard navigable | WCAG 2.1 AA |
| NFR-008 | Maintainability | Each module < 300 lines; single-responsibility | Enforced by Copilot instructions |
| NFR-009 | Portability | Simulation layer runs on Windows, macOS, and Linux | Tested in Codespaces (Ubuntu) |

---

## User Experience Requirements

- **Primary screens:** Live pattern dashboard (TypeScript / React), operator dashboard (C# WPF stub), REST API docs
  (`/docs`)
- **Required states:** Loading, pattern running, pattern paused, misfire alert, machine offline, reset confirmation
- **Content requirements:** Dye-head status grid, pattern progress bar, misfire alert banner, event log table,
  pattern preview canvas
- **Accessibility:** Keyboard focus on all interactive controls; ARIA labels on dye-head status indicators; high-contrast
  misfire alert color
- **Responsive behavior:** Dashboard usable on 1280×720 operator monitor; design portal optimized for 1920×1080

---

## Data Requirements

- **Entities:** `PatternJob`, `DyeHeadStatus`, `MisfireEvent`, `PatternDefinition`, `SimulationRun`
- **Required fields:**
  - `PatternJob`: `id UUID`, `pattern_name VARCHAR(255)`, `started_at TIMESTAMPTZ`, `completed_at TIMESTAMPTZ`,
    `status VARCHAR(50)`
  - `DyeHeadStatus`: `id UUID`, `job_id UUID FK`, `head_index INT`, `status VARCHAR(50)`, `recorded_at TIMESTAMPTZ`
  - `MisfireEvent`: `id UUID`, `job_id UUID FK`, `head_index INT`, `channel INT`, `detected_at TIMESTAMPTZ`,
    `resolved_at TIMESTAMPTZ`
- **Data lifecycle:** Created on job start; updated on misfire; closed on job complete; purged on simulation restart
- **Validation rules:** `head_index` must be 0–15; `channel` must be 0–31; `status` must be one of
  `queued | running | paused | completed | failed`
- **Seed data:** 3 sample pattern definitions included in `workspace/scripts/seed_patterns.sql`
- **Privacy:** No PII; all data is operational machine telemetry

---

## API and Integration Requirements

- **REST endpoints:**
  - `GET /api/state` — current simulation snapshot
  - `POST /api/pattern/start` — start a new pattern job `{ "pattern_name": "str", "dye_head_count": int }`
  - `POST /api/pattern/pause` — pause the running job
  - `POST /api/pattern/restart` — clear state and start fresh
  - `GET /api/misfire/events` — list all misfire events for the current run
  - `POST /api/misfire/resolve/{event_id}` — mark misfire as resolved
- **WebSocket:** `ws://host/ws` — pushes `SimulationStateSnapshot` JSON every 500 ms
- **Internal module boundaries:**
  - `simulation/` owns pattern state; `api/` owns HTTP/WS lifecycle; `ui/` owns rendering
  - C# `PatternRenderer` calls FPGA signal map through a `ISignalMapAdapter` interface
  - PLC routines communicate with C# service via TCP/IP socket (simulated with a loopback stub)
- **External services:** Azure IoT Hub (optional); SQL Server or PostgreSQL (optional); Azure Container Apps
- **Configuration:** `DATABASE_URL`, `AZURE_IOT_HUB_CONNECTION_STRING`, `DYE_HEAD_COUNT`, `PATTERN_WIDTH_PX`
- **Failure handling:** Missing `DATABASE_URL` → in-memory mode; IoT Hub unreachable → local buffer with retry

---

## Technical Approach

The simulation core is a Python FastAPI service (mirroring the elevator dispatch workshop) that stands in for the C#
Pattern Engine service during Copilot Labs 01–09. From Lab 10 onward, participants generate C# stubs alongside the
running simulation to experience multi-language Copilot workflows.

### Proposed Components

| Component | Responsibility | Files or Location |
| --- | --- | --- |
| Pattern Engine (Python sim) | Simulation tick, dye-head state, WebSocket push | `workspace/simulation/` |
| FastAPI Server | REST API, WebSocket lifecycle, request validation | `workspace/api/server.py` |
| SQL Persistence | Optional event writes to `pattern_jobs` / `dye_head_events` | `workspace/api/database.py` |
| Live Dashboard | Real-time pattern visualization | `workspace/ui/` |
| C# Pattern Renderer stub | `PatternRenderer.cs`, `DyeHeadMisfireDetector.cs`, xUnit tests | `workspace/csharp/` (Lab 12) |
| FPGA Signal Map stub | VHDL signal-to-pixel mapping | `workspace/fpga/` (Lab 10) |
| PLC Control stub | Structured Text dye-head control routine | `workspace/plc/` (Lab 11) |
| T-SQL Schema | `pattern_jobs`, `dye_head_events`, `misfire_alerts` tables | `workspace/sql/migrations/` |
| Azure IoT Edge module | Pattern engine container on edge device | `workspace/edge/` (Lab 14) |

### Data Model or Schema

```sql
-- pattern_jobs
CREATE TABLE pattern_jobs (
    id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    pattern_name    NVARCHAR(255) NOT NULL,
    dye_head_count  INT NOT NULL DEFAULT 16,
    pattern_width   INT NOT NULL DEFAULT 1024,
    status          NVARCHAR(50) NOT NULL DEFAULT 'queued',
    started_at      DATETIMEOFFSET,
    completed_at    DATETIMEOFFSET,
    created_at      DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
);

-- dye_head_events
CREATE TABLE dye_head_events (
    id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    job_id          UNIQUEIDENTIFIER NOT NULL REFERENCES pattern_jobs(id),
    head_index      INT NOT NULL,
    event_type      NVARCHAR(50) NOT NULL, -- 'activated' | 'idle' | 'misfired'
    recorded_at     DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET()
);

-- misfire_alerts
CREATE TABLE misfire_alerts (
    id              UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    job_id          UNIQUEIDENTIFIER NOT NULL REFERENCES pattern_jobs(id),
    head_index      INT NOT NULL,
    channel         INT NOT NULL,
    detected_at     DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    resolved_at     DATETIMEOFFSET,
    resolved_by     NVARCHAR(255)
);
```

### Key Flows

```text
Pattern Job Lifecycle:
  POST /api/pattern/start
    → PatternEngine.start_job()
      → FPGA signal map: pattern bytes → pulse widths
        → PLC layer: pulse widths → dye-head activation signals
          → DyeHeadMisfireDetector: monitor for zero-voltage pulses
            → MisfireEvent raised → SQL insert → WebSocket push → dashboard alert

Misfire Detection Flow (C# / Lab 12):
  PLC TCP socket → C# SignalReceiver
    → DyeHeadMisfireDetector.Inspect(signal)
      → if (signal.Voltage < THRESHOLD) → raise MisfireDetectedEvent
        → PatternRenderer.MarkHeadMisfired(headIndex)
          → SQL INSERT dye_head_events + misfire_alerts
            → SignalR push → dashboard banner
```

---

## Acceptance Criteria

- [ ] AC-001: Given a running pattern job, when dye-head channel 7 reports zero voltage, then a `MisfireDetectedEvent`
  is raised within 50 ms and the dashboard shows a misfire alert banner.
- [ ] AC-002: Given a valid `.pat` file upload, when the pattern engine parses it, then the live dashboard renders the
  animated pattern within 200 ms.
- [ ] AC-003: Given `DATABASE_URL` is set, when a pattern job starts, then a row is inserted into `pattern_jobs` and
  dye-head events are written to `dye_head_events`.
- [ ] AC-004: Given `DATABASE_URL` is not set, when the simulation runs, then no database calls are made and the
  simulation operates correctly in memory.
- [ ] AC-005: Given the Copilot prompt `10.03.misfire-detection.prompt.md`, when the engineer runs it in agent mode,
  then `DyeHeadMisfireDetector.cs` and its xUnit tests are generated in `workspace/csharp/`.
- [ ] AC-006: Given a completed C# `PatternRenderer`, when `dotnet test` is run, then all xUnit tests pass.
- [ ] AC-007: Given `azd deploy` is run with a valid Azure environment, when the deployment completes, then
  `GET /api/state` returns 200 from the Container Apps endpoint.
- [ ] AC-008: Given the FPGA signal-map stub in Lab 10, when the simulation tick runs, then signal-to-pixel mapping
  produces a valid color array with no index-out-of-bounds errors.

---

## Metrics and Success Criteria

| Metric | Baseline | Target | Measurement Method |
| --- | --- | --- | --- |
| Copilot prompt completion rate | N/A | 100% of Labs 10–14 generate runnable code | Manual lab completion tracking |
| Test pass rate (C# xUnit) | N/A | 100% pass on first Copilot-generated run | `dotnet test` output |
| Dashboard WebSocket latency | N/A | ≤ 200 ms | Browser DevTools network waterfall |
| Time to complete sprint ticket #142 simulation | N/A | ≤ 90 min with Copilot | Workshop stopwatch |
| Misfire detection false-positive rate | N/A | 0% with simulation harness | PLC harness test log |

---

## Testing Strategy

- **Unit tests:** xUnit tests for `PatternRenderer`, `DyeHeadMisfireDetector`, FPGA signal-map adapter; `unittest`
  tests for Python simulation tick, dispatcher, and API validation
- **Integration tests:** PLC simulation harness signal round-trip; SQL event write / read cycle; WebSocket push-to-UI
  latency
- **Manual validation:** Run simulation in Codespaces; open dashboard; trigger misfire via `POST /api/test/misfire`;
  verify banner and SQL event
- **Test data:** 3 seed pattern definitions; 1 misfire scenario fixture
- **Regression risks:** FPGA timing constant changes that invalidate pulse-width mapping; SQL schema drift between
  Python model and C# Entity Framework model

---

## Rollout and Operations

- **Local / Codespaces:** `docker compose up` starts PostgreSQL sidecar; `uvicorn` starts pattern engine
- **Azure Container Apps:** `azd deploy` from repository root; see `azure.yaml` for service definitions
- **Azure IoT Edge:** `azd deploy --service edge-module` (Lab 14 advanced track)
- **Migration plan:** None for initial launch; T-SQL migrations are additive
- **Backward compatibility:** Python simulation API shape is stable; C# stubs are additive to the existing workspace
- **Observability:** Structured JSON logs from FastAPI; Azure Monitor for Container Apps; IoT Hub telemetry for edge
- **Runbook:** If dashboard shows "No signal", check that `uvicorn` is running and port 7000 is forwarded in Codespaces

---

## Security, Privacy, and Compliance

- **Authentication:** None required for workshop; production would use Azure Managed Identity for IoT Hub and SQL
- **Secrets:** `DATABASE_URL` and `AZURE_IOT_HUB_CONNECTION_STRING` via environment variables only; never committed
- **Data protection:** All data is synthetic machine telemetry; no PII; no GDPR scope
- **Abuse / misuse:** Workshop environment only; no public-facing endpoints in production deployment

---

## Dependencies

- **Internal:** `workspace/simulation/`, `workspace/api/`, `workspace/ui/`, `workspace/tests/`
- **External:** Python 3.10+, FastAPI, Pydantic, SQLAlchemy, asyncpg (PostgreSQL); Node.js, TypeScript, React;
  .NET 8 SDK (C# labs); CODESYS or TwinCAT (PLC harness, local only); ModelSim or Vivado (FPGA, local only)
- **Azure:** Azure Container Apps, Azure Container Registry, Azure Database for PostgreSQL Flexible Server,
  Azure IoT Hub, Azure IoT Edge runtime
- **Team:** Workshop facilitator for lab validation; Design team for `.pat` file format specification

---

## Risks and Mitigations

| Risk | Impact | Likelihood | Mitigation |
| --- | --- | --- | --- |
| FPGA toolchain not available in Codespaces | High | High | Use simulation stubs; FPGA labs are optional or local-only |
| PLC runtime not available in devcontainer | High | High | Use TCP loopback stub; real PLC is Lab 11 advanced track |
| C# .NET SDK not pre-installed in devcontainer | Medium | Medium | Add `.NET 8 SDK` feature to `devcontainer.json` |
| SQL Server not available in Codespaces | Medium | Low | Default to PostgreSQL; Entity Framework supports both |
| Copilot generates non-compiling C# | Medium | Medium | Provide `csharp-wpf.instructions.md` with type-hint conventions |
| Azure IoT Hub cost overrun in workshop | Low | Low | Use free tier; enforce teardown after Lab 14 |

---

## Open Questions

- [ ] Should the FPGA simulation use a VHDL stub or a C++ behavioral model? (Owner: engineering lead)
- [ ] Should the PLC harness use CODESYS or TwinCAT? (Owner: controls engineering lead)
- [ ] Should the design portal use React or a framework-free TypeScript approach? (Owner: UI lead)
- [ ] Is Azure SQL (SQL Server) or Azure Database for PostgreSQL preferred for the workshop persistence layer?
  (Owner: DevOps lead)

---

## Decisions

| Date | Decision | Rationale | Owner |
| --- | --- | --- | --- |
| 2026-05-14 | Use Python FastAPI for simulation core (Labs 01–09) | Aligns with existing elevator dispatch workshop; no new runtime | Workshop facilitator |
| 2026-05-14 | Generate C# stubs from Copilot in Labs 10–14 | Demonstrates cross-language Copilot capability without pre-written C# | Engineering lead |
| 2026-05-14 | Use FPGA VHDL stubs (not behavioral C++) | VHDL is a representative signal-processing language for the workshop scenario | Engineering lead |
| 2026-05-14 | PostgreSQL for workshop persistence; SQL Server schema for C# labs | PostgreSQL is available in devcontainer; SQL Server schema is more realistic for .NET workshops | DevOps lead |

---

## Implementation Plan

1. Extend `workspace/simulation/` with `PatternEngine`, `DyeHeadManager`, and `MisfireDetector` Python modules.
2. Add `POST /api/pattern/start`, `POST /api/pattern/pause`, `GET /api/misfire/events` routes to `api/server.py`.
3. Extend `workspace/api/database.py` with `pattern_jobs`, `dye_head_events`, and `misfire_alerts` write helpers.
4. Update `workspace/ui/` dashboard to show dye-head status grid and misfire alert banner.
5. Write Python `unittest` tests for pattern engine, dispatcher, and API validation.
6. Create `10.00.fpga-signal-map.prompt.md` — Copilot prompt to scaffold VHDL signal-map stub.
7. Create `10.01.plc-dye-head-control.prompt.md` — Copilot prompt to scaffold PLC Structured Text routine.
8. Create `10.02.pattern-renderer-csharp.prompt.md` — Copilot prompt to generate `PatternRenderer.cs` and xUnit tests.
9. Create `10.03.misfire-detection.prompt.md` — Copilot prompt for `DyeHeadMisfireDetector.cs`.
10. Create `10.04.sql-schema-migration.prompt.md` — Copilot prompt for T-SQL migration.
11. Create `10.05.azure-iot-edge-deploy.prompt.md` — Copilot prompt for Azure IoT Edge deploy.
12. Validate all labs end-to-end in Codespaces; record demo walkthrough.

---

## Appendix

- **Related infographic:** [docs/images/digitial-patterning-infographic.svg](images/digitial-patterning-infographic.svg)
- **Root README:** [README.md](../README.md)
- **Elevator dispatch PRD (reference):** [prd-elevator-dispatch.md](prd-elevator-dispatch.md)
- **Copilot instructions:** [.github/copilot-instructions.md](../.github/copilot-instructions.md)
- **Workshop prompts:** [.github/prompts/](../.github/prompts/)
- **Glossary:**
  - **Digital patterning system** — A representative digital textile patterning machine and software stack
  - **Dye head** — An individual ink-jet nozzle assembly that applies a single color channel to the textile
  - **FPGA** — Field-Programmable Gate Array; handles real-time signal processing between the pattern engine and dye heads
  - **PLC** — Programmable Logic Controller; executes machine control logic (conveyor speed, dye-head timing)
  - **IEC 61131-3** — International standard for PLC programming languages; Structured Text is used in this project
  - **OPC-UA** — Open Platform Communications Unified Architecture; used for machine-to-cloud telemetry
  - **MQTT** — Lightweight messaging protocol used by Azure IoT Edge for telemetry
  - **ADR** — Architecture Decision Record; used to capture key design choices
