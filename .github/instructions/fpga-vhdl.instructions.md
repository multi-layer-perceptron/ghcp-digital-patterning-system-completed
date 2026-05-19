---
applyTo: "workspace/fpga/**"
---

# FPGA / VHDL Conventions — Digital Patterning Signal Processing

These instructions apply to all VHDL and Verilog files under `workspace/fpga/`. Follow these conventions when
generating, reviewing, or editing HDL code for the digital patterning FPGA signal-processing module.

## VHDL Standard

- Use VHDL-2008. Include `use ieee.std_logic_1164.all;` and `use ieee.numeric_std.all;`.
- Do not use `std_logic_arith` or `std_logic_unsigned` (deprecated; replaced by `numeric_std`).
- Do not use vendor-specific libraries (e.g., Xilinx `UNISIM`, Altera `altera_mf`). Keep stubs portable.

## File Structure

```text
workspace/fpga/
├── signal_map.vhd           # Signal-to-pixel mapping entity and architecture
├── signal_map_tb.vhd        # Testbench for signal_map
├── timing_control.vhd       # Pulse-width timing controller
├── dma_buffer.vhd           # DMA pattern buffer interface stub
└── README.md                # Simulation instructions
```

## Entity and Port Naming

- Entity names: `PascalCase` (e.g., `SignalMap`, `TimingControl`).
- Port names: `UPPER_SNAKE_CASE` (e.g., `PATTERN_BYTE`, `PULSE_WIDTH_US`).
- Generic names: `UPPER_SNAKE_CASE` with a type suffix if helpful (e.g., `PIXEL_COUNT`, `DYE_HEAD_COUNT`).
- Signal names (internal): `lower_snake_case` (e.g., `pulse_width_reg`, `head_select_int`).

## Coding Style

- Use `process (clk)` with a single synchronous reset (`if reset = '1' then`).
- Prefer registered outputs (`clk`-triggered) over combinational-only outputs for timing safety.
- Use constants for magic numbers: `constant MISFIRE_THRESHOLD : integer := 50;`.
- Define LUTs as `constant` arrays of a named type, not as `case` statements.
- Keep each architecture under 100 lines; use separate entities for sub-modules.

## Testbenches

- Testbench entity name: `{EntityName}_TB` (e.g., `SignalMap_TB`).
- Use `std.textio` for test result reporting if ModelSim/GHDL is available.
- Assert outputs with `assert <condition> report "<message>" severity failure;`.
- Cover: reset behavior, minimum input, maximum input, boundary values, invalid-range behavior.

## Comments

- Add a comment header block at the top of every file:

```vhdl
-- ============================================================================
-- Module  : SignalMap
-- Purpose : Map pattern pixel bytes to dye-head pulse-width values
-- Author  : Digital Patterning Workshop Team
-- Modified: 2026-05-14
-- ============================================================================
```

- Add an inline comment on every port explaining units (e.g., `-- microseconds, 0-255`).

## Simulation Notes

- The workshop FPGA stubs are simulation-only (no synthesis target).
- Use GHDL or ModelSim for simulation; Vivado Simulator is acceptable but not required.
- Include a `Makefile` or `README.md` in `workspace/fpga/` with simulation commands.
