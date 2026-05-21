#pragma once

#include <string>
#include <vector>

namespace patterning {

struct GridCell {
    int x;
    int y;
    int channel_number;
};

std::vector<GridCell> convert_to_grid(const std::vector<int>& channel_sequence, int grid_size);

}  // namespace patterning
