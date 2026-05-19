#include "grid_converter.hpp"

#include <stdexcept>

namespace patterning {

std::vector<GridCell> convert_to_grid(const std::vector<int>& channel_sequence, int grid_size) {
    if (grid_size != 64 && grid_size != 128 && grid_size != 256) {
        throw std::invalid_argument("Grid size must be 64, 128, or 256");
    }
    std::vector<GridCell> cells;
    cells.reserve(static_cast<std::size_t>(grid_size * grid_size));
    for (int y = 0; y < grid_size; ++y) {
        for (int x = 0; x < grid_size; ++x) {
            const auto index = static_cast<std::size_t>((y * grid_size + x) % static_cast<int>(channel_sequence.size()));
            cells.push_back(GridCell{x, y, channel_sequence[index]});
        }
    }
    return cells;
}

}  // namespace patterning
