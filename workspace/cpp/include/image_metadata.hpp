#pragma once

namespace patterning {

struct ImageMetadataResult {
    int width_px;
    int height_px;
    double aspect_ratio;
    int estimated_unique_colors;
    bool has_transparency;
};

ImageMetadataResult extract_metadata(int width_px, int height_px, int sampled_unique_colors, bool has_transparency);

}  // namespace patterning
