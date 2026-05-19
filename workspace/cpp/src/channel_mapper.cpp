#include "channel_mapper.hpp"

#include "color_delta.hpp"

#include <limits>

namespace patterning {

std::vector<ChannelMappingResult> map_channels(const std::vector<std::string>& palette, const std::vector<ChannelColor>& channels) {
    std::vector<ChannelMappingResult> results;
    for (const auto& palette_hex : palette) {
        double best_delta = std::numeric_limits<double>::max();
        int best_channel = 0;
        for (const auto& channel : channels) {
            const auto delta = color_delta(palette_hex, channel.hex);
            if (delta < best_delta) {
                best_delta = delta;
                best_channel = channel.channel_number;
            }
        }
        const auto status = best_delta == 0.0 ? "exact" : best_delta <= 80.0 ? "approximate" : "unresolved";
        results.push_back(ChannelMappingResult{palette_hex, status == std::string("unresolved") ? 0 : best_channel, status, best_delta});
    }
    return results;
}

}  // namespace patterning
