#pragma once

#include <string>
#include <vector>

namespace patterning {

struct ChannelColor {
    int channel_number;
    std::string hex;
};

struct ChannelMappingResult {
    std::string palette_hex;
    int channel_number;
    std::string status;
    double delta;
};

std::vector<ChannelMappingResult> map_channels(const std::vector<std::string>& palette, const std::vector<ChannelColor>& channels);

}  // namespace patterning
