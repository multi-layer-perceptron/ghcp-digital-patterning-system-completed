#include "image_metadata.hpp"

#include <stdexcept>

namespace patterning {

ImageMetadataResult extract_metadata(int width_px, int height_px, int sampled_unique_colors, bool has_transparency) {
    if (width_px <= 0 || height_px <= 0 || width_px > 4096 || height_px > 4096) {
        throw std::invalid_argument("Image dimensions must be between 1 and 4096 pixels");
    }
    return ImageMetadataResult{width_px, height_px, static_cast<double>(width_px) / height_px, sampled_unique_colors, has_transparency};
}

}  // namespace patterning
