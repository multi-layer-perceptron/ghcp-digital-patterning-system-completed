#include "command_generator.hpp"

#include <cassert>

int main() {
    auto commands = patterning::generate_pass_commands({{0, 0, 1}, {1, 0, 2}});
    assert(commands.size() == 2);
    assert(commands[0].sequence == 1);
    return 0;
}
