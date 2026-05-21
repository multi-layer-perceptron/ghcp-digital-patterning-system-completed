#include "protocol.hpp"

#include <stdexcept>

namespace patterning {

std::string serialize_json_line(const ProtocolEnvelope& envelope) {
    return "{\"message_id\":\"" + envelope.message_id +
           "\",\"message_type\":\"" + envelope.message_type +
           "\",\"schema_version\":\"" + envelope.schema_version +
           "\",\"sent_at\":\"" + envelope.sent_at +
           "\",\"payload\":" + envelope.payload_json + "}\n";
}

ProtocolEnvelope parse_json_line(const std::string& line) {
    if (line.empty()) {
        throw std::invalid_argument("JSON line cannot be empty");
    }
    return ProtocolEnvelope{"parsed", "unknown", "0.1", "", "{}"};
}

}  // namespace patterning
