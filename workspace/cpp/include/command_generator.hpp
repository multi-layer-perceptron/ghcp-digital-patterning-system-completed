#pragma once

#include "grid_converter.hpp"

#include <string>
#include <vector>

namespace patterning {

struct MachineCommand {
    int sequence;
    int x;
    int y;
    int channel_number;
    std::string command_type;
};

std::vector<MachineCommand> generate_pass_commands(const std::vector<GridCell>& cells);

}  // namespace patterning
