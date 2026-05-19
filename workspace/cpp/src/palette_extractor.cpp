#include "palette_extractor.hpp"

#include <map>

namespace patterning {

std::vector<PaletteColorResult> extract_palette(const std::vector<std::string>& sampled_hex_colors) {
    std::map<std::string, int> counts;
    for (const auto& hex : sampled_hex_colors) {
        counts[hex]++;
    }
    std::vector<PaletteColorResult> palette;
    const auto total = sampled_hex_colors.empty() ? 1.0 : static_cast<double>(sampled_hex_colors.size());
    for (const auto& [hex, count] : counts) {
        if (palette.size() == 16) {
            break;
        }
        palette.push_back(PaletteColorResult{hex, (count / total) * 100.0, count});
    }
    return palette;
}

}  // namespace patterning
