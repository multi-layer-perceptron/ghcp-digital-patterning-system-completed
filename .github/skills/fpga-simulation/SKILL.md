# Skill: FPGA Signal Map Simulation

This skill scaffolds and validates the Millitron FPGA signal-to-pixel mapping module for the digital patterning
workshop. It is a self-contained unit that can be invoked from Copilot agent mode or run standalone.

## What This Skill Does

1. Generates a VHDL entity (`signal_map.vhd`) for the pixel-to-pulse-width LUT.
2. Generates a VHDL testbench (`signal_map_tb.vhd`) with boundary and error-case assertions.
3. Provides a Python simulation stub (`simulate_signal_map.py`) that mimics the VHDL behavior for rapid
   verification without an HDL simulator.
4. Validates the Python stub against 256 LUT entries and 3 edge cases.

## When to Use This Skill

- Lab 10: Generating the FPGA signal-map stub from `10.00.fpga-signal-map.prompt.md`.
- When a developer needs to verify that pixel-to-pulse-width mapping is correct before synthesizing.
- When adding a new dye-head count or pattern-width variant.

## Skill Assets

```text
.github/skills/fpga-simulation/
├── SKILL.md                            # This file
├── templates/
│   ├── signal_map.vhd.tmpl             # VHDL entity + architecture template
│   └── signal_map_tb.vhd.tmpl          # VHDL testbench template
└── scripts/
    └── simulate-signal-map.sh          # Run Python simulation stub validation
```

## Invocation

### From Copilot Agent Mode

Reference this skill in your prompt or instructions:

```text
Use the fpga-simulation skill to generate signal_map.vhd and validate it.
Skill path: .github/skills/fpga-simulation/SKILL.md
```

### From the Command Line

```bash
.github/skills/fpga-simulation/scripts/simulate-signal-map.sh
```

Expected output:

```
FPGA Signal Map Simulation — 256 LUT entries validated
Edge cases: min=0 → pulse=0 ✓  |  max=255 → pulse=255 ✓  |  mid=128 → pulse=128 ✓
All 259 assertions passed.
```

## VHDL Template Notes

The generated `signal_map.vhd` uses:
- A 256-entry integer LUT (`constant PULSE_LUT : lut_t`) where `PULSE_LUT(i) = i` (identity for the stub).
- A registered `head_select` output driven by `pixel_index mod DYE_HEAD_COUNT`.
- A single-cycle `valid` pulse asserted when `pixel_index < PIXEL_COUNT`.

In production, the LUT values are derived from ink-density calibration data loaded at machine startup.

## Python Simulation Stub

The Python stub (`simulate_signal_map.py`) is generated in `workspace/fpga/` by the Lab 10 prompt. It implements the
same combinational logic as the VHDL entity and is used for fast unit testing without HDL tools.

```python
def map_pixel_to_activation(pixel_byte: int, pixel_index: int, dye_head_count: int = 16):
    """Mirror of VHDL SignalMap combinational logic."""
    pulse_width_us = PULSE_LUT[pixel_byte]
    head_select = pixel_index % dye_head_count
    valid = 0 <= pixel_index < 1024
    return pulse_width_us, head_select, valid
```

## Extension Points

- Replace the identity LUT with a calibration-loaded LUT from `workspace/sql/seeds/lut_calibration.sql`.
- Add a `timing_control.vhd` module that gates `head_select` output with the pulse-width timer.
- Extend the testbench to cover DMA buffer overrun and reset-during-frame edge cases.

## Related

- Prompt: [.github/prompts/10.00.fpga-signal-map.prompt.md](../../prompts/10.00.fpga-signal-map.prompt.md)
- Instructions: [.github/instructions/fpga-vhdl.instructions.md](../../instructions/fpga-vhdl.instructions.md)
- PRD: [docs/prd-digital-patterning-system.md](../../../../docs/prd-digital-patterning-system.md)
