using Patterning.Core.Models;
using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class ChannelMappingWorkflowTests
{
    [Fact]
    public void Map_WithExactColor_ReturnsExactMapping()
    {
        var conceptId = Guid.NewGuid();
        var palette = new ColorPalette([new PaletteColor("p1", "#111111", "Black", 100, 10)], 100, "test");
        var channels = DefaultChannelFactory.CreateDefaultChannels(conceptId);
        var mappings = new ChannelMappingService().Map(palette, channels);
        Assert.Equal(MappingStatus.Exact, mappings[0].Status);
    }
}
