#include "control_state.h"

void control_state_init(ControlState* state, int total_passes) {
    state->status = CONTROL_NOT_STARTED;
    state->current_pass = 0;
    state->total_passes = total_passes;
    state->progress_percent = 0.0;
}

void control_state_start(ControlState* state) {
    state->status = CONTROL_RUNNING;
}

void control_state_pause(ControlState* state) {
    if (state->status == CONTROL_RUNNING) {
        state->status = CONTROL_PAUSED;
    }
}

void control_state_resume(ControlState* state) {
    if (state->status == CONTROL_PAUSED) {
        state->status = CONTROL_RUNNING;
    }
}

void control_state_reset(ControlState* state) {
    state->status = CONTROL_RESET;
    state->current_pass = 0;
    state->progress_percent = 0.0;
}

void control_state_advance(ControlState* state) {
    if (state->status != CONTROL_RUNNING || state->total_passes <= 0) {
        return;
    }
    state->current_pass += 1;
    state->progress_percent = (100.0 * state->current_pass) / state->total_passes;
    if (state->current_pass >= state->total_passes) {
        state->status = CONTROL_COMPLETED;
        state->progress_percent = 100.0;
    }
}
