#ifndef PATTERNING_PROTOCOL_H
#define PATTERNING_PROTOCOL_H

#include <stddef.h>

typedef struct PatterningEnvelope {
    char message_id[64];
    char message_type[64];
    char schema_version[16];
    char payload_json[2048];
} PatterningEnvelope;

int patterning_protocol_format(const PatterningEnvelope* envelope, char* buffer, size_t buffer_size);
int patterning_protocol_parse(const char* line, PatterningEnvelope* envelope);

#endif
