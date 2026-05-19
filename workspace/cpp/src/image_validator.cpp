#include "image_validator.hpp"

namespace patterning {

bool is_supported_image(const std::string& mime_type, std::size_t file_size_bytes, int width_px, int height_px) {
    const bool supported_type = mime_type == "image/png" || mime_type == "image/jpeg";
    return supported_type && file_size_bytes > 0 && file_size_bytes <= 10485760 &&
           width_px > 0 && height_px > 0 && width_px <= 4096 && height_px <= 4096;
}

}  // namespace patterning
