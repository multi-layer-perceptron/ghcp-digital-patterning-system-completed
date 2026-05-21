#include "color_delta.hpp"

#include <cmath>
#include <cstdlib>
#include <stdexcept>
#include <string>

namespace patterning {

namespace {
int component(const std::string& value, std::size_t offset) {
    return static_cast<int>(std::strtol(value.substr(offset, 2).c_str(), nullptr, 16));
}
}

double color_delta(const std::string& left_hex, const std::string& right_hex) {
    if (left_hex.size() != 7 || right_hex.size() != 7 || left_hex[0] != '#' || right_hex[0] != '#') {
        throw std::invalid_argument("Colors must use #RRGGBB format");
    }
    const auto dr = component(left_hex, 1) - component(right_hex, 1);
    const auto dg = component(left_hex, 3) - component(right_hex, 3);
    const auto db = component(left_hex, 5) - component(right_hex, 5);
    return std::sqrt((dr * dr) + (dg * dg) + (db * db));
}

}  // namespace patterning
