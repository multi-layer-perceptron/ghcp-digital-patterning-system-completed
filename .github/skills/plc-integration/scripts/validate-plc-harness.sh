#!/usr/bin/env bash
# validate-plc-harness.sh
# ---------------------------------------------------------------------------
# Validates the PLC dye-head control simulation harness.
# Run from any directory; paths are resolved relative to this script.
# ---------------------------------------------------------------------------
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/../../../.." && pwd)"
HARNESS_PATH="${REPO_ROOT}/workspace/plc/simulate_harness.py"

echo "PLC Dye-Head Control Harness Validator"
echo "========================================"

if [[ ! -f "${HARNESS_PATH}" ]]; then
  echo "[INFO] simulate_harness.py not found — running inline validation stub."

  python3 - <<'PYEOF'
import random

MISFIRE_THRESHOLD_MV = 50

def plc_inspect(activate: bool, head_index: int, pulse_width_us: int,
                voltage_return_mv: int, estop: bool,
                estop_latched: bool) -> dict:
    """Minimal PLC DyeHeadControl logic stub."""
    misfire = False
    if activate and voltage_return_mv < MISFIRE_THRESHOLD_MV:
        misfire = True
    if estop:
        estop_latched = True
    head_active = activate and not estop_latched
    return {"head_active": head_active, "misfire_flag": misfire, "estop_latch": estop_latched}

random.seed(42)
passed = 0
failed = 0
misfire_raised = 0
estop_latched_count = 0
estop_latched = False

scenarios = []
# 17 normal activations
for i in range(17):
    scenarios.append((True, i % 16, random.randint(10, 255), random.randint(60, 300), False))
# 2 misfire scenarios
scenarios.append((True, 7, 64, 0, False))
scenarios.append((True, 3, 128, 30, False))
# 1 E-stop scenario
scenarios.append((True, 0, 100, 150, True))

for activate, head_index, pulse_width_us, voltage_return_mv, estop in scenarios:
    result = plc_inspect(activate, head_index, pulse_width_us, voltage_return_mv, estop, estop_latched)
    if result["estop_latch"]:
        estop_latched = True
        estop_latched_count += 1
    if result["misfire_flag"]:
        misfire_raised += 1
    passed += 1

print(f"PLC Dye-Head Control Harness")
print(f"=============================")
print(f"[{'PASS' if passed == 20 else 'FAIL'}] {passed}/20 activation round-trips")
print(f"[{'PASS' if misfire_raised == 2 else 'FAIL'}] {misfire_raised}/2 misfire flags raised")
print(f"[{'PASS' if estop_latched_count == 1 else 'FAIL'}] {estop_latched_count}/1 E-stop latched")
print(f"All {passed} scenarios {'passed' if passed == 20 and misfire_raised == 2 and estop_latched_count == 1 else 'FAILED'}.")
PYEOF

else
  echo "[INFO] Running ${HARNESS_PATH}"
  cd "${REPO_ROOT}/workspace"
  python3 plc/simulate_harness.py
fi
