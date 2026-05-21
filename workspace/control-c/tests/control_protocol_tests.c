#include "control_state.h"
#include "patterning_protocol.h"

#include <assert.h>

int main(void) {
    ControlState state;
    control_state_init(&state, 2);
    control_state_start(&state);
    control_state_advance(&state);
    assert(state.status == CONTROL_RUNNING);
    control_state_advance(&state);
    assert(state.status == CONTROL_COMPLETED);

    PatterningEnvelope envelope = {"1", "run.start", "0.1", "{}"};
    char buffer[256];
    assert(patterning_protocol_format(&envelope, buffer, sizeof(buffer)) == 0);
    return 0;
}
