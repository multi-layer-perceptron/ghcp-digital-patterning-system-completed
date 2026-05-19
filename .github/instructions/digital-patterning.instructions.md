---
applyTo: "workspace/**"
---

# Digital Patterning System — Domain Instructions

These instructions apply to all files under `workspace/` in the Digital Patterning System repository.
GitHub Copilot should follow these conventions when generating, reviewing, or editing code in this workspace.

## Domain Context

This repository simulates a Digital Patterning System — a representative industrial platform used in
floorcovering manufacturing. The simulation core is Python / FastAPI, but the production system uses:
- C# / .NET 8 for the Pattern Engine service and WPF operator dashboard
- VHDL / Verilog on FPGA for real-time signal processing
- IEC 61131-3 Structured Text on PLCs for machine control
- TypeScript / React for the design portal
- T-SQL / SQL Server for event persistence

## Module Responsibilities

| Module | Responsibility |
| --- | --- |
| `simulation/` | Pattern engine tick, dye-head state machine, misfire detection simulation |
| `api/` | REST API, WebSocket lifecycle, request validation with Pydantic |
| `ui/` | Live dashboard: pattern canvas, dye-head status grid, misfire alert banner |
| `tests/` | `unittest` regression suite for simulation and API behavior |

## Python Conventions

- Use Python 3.10+ type hints on all function signatures and dataclasses.
- Prefer `dataclasses.dataclass` with `frozen=True` for immutable domain objects.
- Use `asyncio`-safe patterns; never block the event loop with synchronous I/O.
- Raise `ValueError` with a clear message for invalid dye-head indices (0–15) or pulse widths (0–255).
- All database access must be guarded by `if db is not None:` — the database is always optional.
- Keep each module under 300 lines; extract helpers into separate files when approaching this limit.

## Simulation Conventions

- The default scenario is 16 dye heads, 1024 pixels per frame, 500 ms tick interval.
- Dye-head states are: `idle`, `active`, `misfired`. Transitions must follow this state machine:
  - `idle → active` on job start
  - `active → misfired` on zero-voltage return
  - `misfired → idle` on operator reset
- Pattern frames are represented as `list[DyeHeadActivation]` where each entry corresponds to one pixel column.
- The pattern engine must emit a `WebSocket` snapshot after every tick.

## API Conventions

- All request bodies use Pydantic `BaseModel` with explicit field validators.
- Return `HTTP 422` for validation errors with a `{ "detail": [...] }` body.
- Return `HTTP 409` for state-machine violations (e.g., start a job when one is already running).
- Use `async def` for all route handlers; use `await engine.method()` for simulation state mutations.
- Do not access `engine._state` directly from routes; use public `engine.*` methods only.

## Security

- Never accept raw SQL from request bodies. All database writes must use parameterized queries.
- Never log full stack traces to the WebSocket or HTTP response bodies in production mode.
- Rotate any secrets if they appear in logs, commit history, or test fixtures.

## Testing

- Write `unittest.TestCase` subclasses; never use `pytest` (not in `requirements.txt`).
- Use `unittest.mock.patch` and `MagicMock` for dependency injection in tests.
- Test files must match `test_*.py`; test methods must start with `test_`.
- Cover: dye-head state transitions, misfire detection, API validation, WebSocket snapshot shape.

## Related Instructions

- C# / WPF conventions: [csharp-wpf.instructions.md](csharp-wpf.instructions.md)
- FPGA / VHDL conventions: [fpga-vhdl.instructions.md](fpga-vhdl.instructions.md)
- Azure deployment: [azure-deployment.instructions.md](azure-deployment.instructions.md)
- UI TypeScript: [ui-typescript.instructions.md](ui-typescript.instructions.md)
- Unit test conventions: [unittest-conventions.instructions.md](unittest-conventions.instructions.md)
