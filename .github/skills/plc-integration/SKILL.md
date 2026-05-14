# Skill: PLC Integration

This skill scaffolds and validates the Millitron PLC dye-head control integration layer for the digital patterning
workshop. It generates IEC 61131-3 Structured Text stubs and a Python simulation harness that exercises the PLC
control logic over a TCP loopback socket.

## What This Skill Does

1. Generates a PLC Structured Text program (`DyeHeadControl.st`) with:
   - Dye-head fire sequencing using `TON` timers.
   - Misfire detection on zero-voltage return.
   - E-stop latching with explicit reset.
2. Generates a Python TCP simulation harness (`simulate_harness.py`) that:
   - Sends activation messages to the PLC stub on `localhost:5010`.
   - Injects misfire and E-stop scenarios.
   - Prints a pass/fail report.
3. Validates the harness against 20 activation round-trips, 2 misfire scenarios, and 1 E-stop scenario.

## When to Use This Skill

- Lab 11: Generating the PLC dye-head control routine from `10.01.plc-dye-head-control.prompt.md`.
- When validating signal round-trip timing between the C# `SignalReceiver` and the PLC stub.
- When adding a new dye-head channel or adjusting the misfire threshold.

## Skill Assets

```text
.github/skills/plc-integration/
├── SKILL.md                            # This file
├── templates/
│   └── DyeHeadControl.st.tmpl          # PLC Structured Text template
└── scripts/
    └── validate-plc-harness.sh         # Run the Python simulation harness
```

## Invocation

### From Copilot Agent Mode

```text
Use the plc-integration skill to generate DyeHeadControl.st and run the harness.
Skill path: .github/skills/plc-integration/SKILL.md
```

### From the Command Line

```bash
.github/skills/plc-integration/scripts/validate-plc-harness.sh
```

Expected output:

```
PLC Dye-Head Control Harness
=============================
[PASS] 20/20 activation round-trips
[PASS] 2/2 misfire flags raised
[PASS] 1/1 E-stop latched
All 23 scenarios passed.
```

## PLC Structured Text Notes

The generated `DyeHeadControl.st` uses:
- `R_TRIG` function blocks for rising-edge detection on `ACTIVATE` and `ESTOP`.
- One `TON` timer instance per dye-head channel (16 total), instantiated as an array.
- A `MISFIRE_FLAG` output latched until the `RESET` input is asserted.
- `ESTOP_LATCH` that persists across scan cycles; requires explicit `RESET` to clear.

## TCP Message Protocol (Simulation)

The simulation harness uses a simple JSON-over-TCP protocol on port 5010:

```json
{
  "head_index": 7,
  "pulse_width_us": 128,
  "voltage_return_mv": 45,
  "activate": true
}
```

Response from PLC stub:

```json
{
  "head_active": true,
  "misfire_flag": true,
  "estop_latch": false
}
```

## Extension Points

- Extend `DyeHeadControl.st` to add conveyor-speed PID control (see `10.01.plc-dye-head-control.prompt.md`).
- Replace the TCP loopback with a CODESYS or TwinCAT runtime for hardware-in-the-loop testing.
- Add a vision-alignment routine that reads encoder feedback from the FPGA timing controller.

## Related

- Prompt: [.github/prompts/10.01.plc-dye-head-control.prompt.md](../../prompts/10.01.plc-dye-head-control.prompt.md)
- Instructions: [.github/instructions/digital-patterning.instructions.md](../../instructions/digital-patterning.instructions.md)
- FPGA skill: [.github/skills/fpga-simulation/SKILL.md](../fpga-simulation/SKILL.md)
- PRD: [docs/prd-digital-patterning-system.md](../../../../docs/prd-digital-patterning-system.md)
