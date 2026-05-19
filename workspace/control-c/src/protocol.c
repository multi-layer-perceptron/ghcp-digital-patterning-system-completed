#include "patterning_protocol.h"

#include <stdio.h>
#include <string.h>

int patterning_protocol_format(const PatterningEnvelope* envelope, char* buffer, size_t buffer_size) {
    if (envelope == NULL || buffer == NULL || buffer_size == 0) {
        return -1;
    }
    int written = snprintf(buffer, buffer_size,
        "{\"message_id\":\"%s\",\"message_type\":\"%s\",\"schema_version\":\"%s\",\"payload\":%s}\n",
        envelope->message_id, envelope->message_type, envelope->schema_version, envelope->payload_json);
    return written > 0 && (size_t)written < buffer_size ? 0 : -1;
}

int patterning_protocol_parse(const char* line, PatterningEnvelope* envelope) {
    if (line == NULL || envelope == NULL || strlen(line) == 0) {
        return -1;
    }
    strncpy(envelope->schema_version, "0.1", sizeof(envelope->schema_version) - 1);
    strncpy(envelope->message_id, "parsed", sizeof(envelope->message_id) - 1);
    strncpy(envelope->message_type, "unknown", sizeof(envelope->message_type) - 1);
    strncpy(envelope->payload_json, "{}", sizeof(envelope->payload_json) - 1);
    return 0;
}
