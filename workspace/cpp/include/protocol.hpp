#pragma once

#include <chrono>
#include <string>

namespace patterning {

struct ProtocolEnvelope {
    std::string message_id;
    std::string message_type;
    std::string schema_version;
    std::string sent_at;
    std::string payload_json;
};

std::string serialize_json_line(const ProtocolEnvelope& envelope);
ProtocolEnvelope parse_json_line(const std::string& line);

}  // namespace patterning
