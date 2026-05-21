#pragma once

#include <string>
#include <vector>

namespace patterning {

struct PaletteColorResult {
    std::string hex;
    double coverage_percent;
    int sample_count;
};

std::vector<PaletteColorResult> extract_palette(const std::vector<std::string>& sampled_hex_colors);

}  // namespace patterning
