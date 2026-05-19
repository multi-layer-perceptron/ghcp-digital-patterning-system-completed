# C Control Emulator

The C11 control emulator models lifecycle state transitions and JSON Lines protocol helpers for the simulated machine boundary.

## Validation

When CMake is available, build `control_emulator` and run `ctest`. With only `gcc`, compile `tests/control_protocol_tests.c` with `src/control_emulator.c` and `src/protocol.c`.
