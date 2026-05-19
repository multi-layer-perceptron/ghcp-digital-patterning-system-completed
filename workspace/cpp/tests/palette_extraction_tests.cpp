#include "palette_extractor.hpp"

#include <cassert>

int main() {
    auto palette = patterning::extract_palette({"#112233", "#112233", "#445566"});
    assert(palette.size() == 2);
    return 0;
}
