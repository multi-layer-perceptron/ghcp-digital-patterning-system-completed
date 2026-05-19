#pragma once

#include <cstddef>
#include <string>

namespace patterning {

bool is_supported_image(const std::string& mime_type, std::size_t file_size_bytes, int width_px, int height_px);

}  // namespace patterning
