#include "grid_converter.hpp"

#include <cassert>

int main() {
    auto cells = patterning::convert_to_grid({1, 2, 3}, 64);
    assert(cells.size() == 4096);
    assert(cells[0].channel_number == 1);
    return 0;
}
