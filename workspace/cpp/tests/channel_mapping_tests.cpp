#include "channel_mapper.hpp"
#include "color_delta.hpp"

#include <cassert>

int main() {
    assert(patterning::color_delta("#000000", "#000000") == 0.0);
    auto mappings = patterning::map_channels({"#000000", "#FFFFFF"}, {{1, "#000000"}, {2, "#EEEEEE"}});
    assert(mappings.size() == 2);
    assert(mappings[0].status == "exact");
    return 0;
}
