#include "image_metadata.hpp"
#include "image_validator.hpp"
#include "grid_converter.hpp"
#include "command_generator.hpp"
#include "palette_extractor.hpp"
#include "protocol.hpp"

#include <iostream>

int main(int argc, char** argv) {
    (void)argc;
    (void)argv;
    const auto cells = patterning::convert_to_grid({1, 2, 3, 4}, 64);
    const auto commands = patterning::generate_pass_commands(cells);
    std::cout << "pattern_processor ready" << std::endl;
    std::cout << "grid.convert commands=" << commands.size() << std::endl;
    return 0;
}
