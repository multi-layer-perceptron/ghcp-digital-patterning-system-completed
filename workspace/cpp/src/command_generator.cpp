#include "command_generator.hpp"

namespace patterning {

std::vector<MachineCommand> generate_pass_commands(const std::vector<GridCell>& cells) {
    std::vector<MachineCommand> commands;
    commands.reserve(cells.size());
    int sequence = 1;
    for (const auto& cell : cells) {
        commands.push_back(MachineCommand{sequence++, cell.x, cell.y, cell.channel_number, "place"});
    }
    return commands;
}

}  // namespace patterning
