#include "image_metadata.hpp"

#include <cassert>

int main() {
    auto metadata = patterning::extract_metadata(128, 64, 12, false);
    assert(metadata.width_px == 128);
    assert(metadata.height_px == 64);
    assert(metadata.aspect_ratio == 2.0);
    return 0;
}
