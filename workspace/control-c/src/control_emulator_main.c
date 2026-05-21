#include "control_state.h"

#include <stdio.h>

int main(void) {
    ControlState state;
    control_state_init(&state, 4);
    control_state_start(&state);
    printf("control_emulator ready status=%d\n", state.status);
    return 0;
}
