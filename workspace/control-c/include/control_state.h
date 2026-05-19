#ifndef CONTROL_STATE_H
#define CONTROL_STATE_H

typedef enum ControlRunStatus {
    CONTROL_NOT_STARTED = 0,
    CONTROL_RUNNING = 1,
    CONTROL_PAUSED = 2,
    CONTROL_COMPLETED = 3,
    CONTROL_RESET = 4,
    CONTROL_BLOCKED = 5
} ControlRunStatus;

typedef struct ControlState {
    ControlRunStatus status;
    int current_pass;
    int total_passes;
    double progress_percent;
} ControlState;

void control_state_init(ControlState* state, int total_passes);
void control_state_start(ControlState* state);
void control_state_pause(ControlState* state);
void control_state_resume(ControlState* state);
void control_state_reset(ControlState* state);
void control_state_advance(ControlState* state);

#endif
