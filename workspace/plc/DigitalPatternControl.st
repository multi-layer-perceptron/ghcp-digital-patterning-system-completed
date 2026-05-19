FUNCTION_BLOCK DigitalPatternControl
VAR_INPUT
    StartRun : BOOL;
    PauseRun : BOOL;
    ResumeRun : BOOL;
    ResetRun : BOOL;
END_VAR
VAR_OUTPUT
    Running : BOOL;
    Paused : BOOL;
    Reset : BOOL;
END_VAR

IF ResetRun THEN
    Running := FALSE;
    Paused := FALSE;
    Reset := TRUE;
ELSIF StartRun THEN
    Running := TRUE;
    Paused := FALSE;
    Reset := FALSE;
ELSIF PauseRun THEN
    Paused := TRUE;
    Running := FALSE;
ELSIF ResumeRun THEN
    Running := TRUE;
    Paused := FALSE;
END_IF;
END_FUNCTION_BLOCK
