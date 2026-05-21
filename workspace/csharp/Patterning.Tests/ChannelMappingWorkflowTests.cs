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
        var palette = new ColorPalette([new PaletteColor("p1", "#2C6F91", "Blue", 100, 10)], 100, "test");
        var channels = DefaultChannelFactory.CreateDefaultChannels(conceptId);
        var mappings = new ChannelMappingService().Map(palette, channels);
        Assert.Equal(MappingStatus.Exact, mappings[0].Status);
    }

    [Fact]
    public void Map_WithSamplePaletteColors_ReturnsMappingsWithinDeltaTen()
    {
        var conceptId = Guid.NewGuid();
        var palette = new ColorPalette([
            new PaletteColor("p1", "#2C6F91", "Color 1", 20, 51),
            new PaletteColor("p2", "#B9573F", "Color 2", 20, 51),
            new PaletteColor("p3", "#D2A13D", "Color 3", 20, 51),
            new PaletteColor("p4", "#7B8F45", "Color 4", 20, 51),
            new PaletteColor("p5", "#3E4F63", "Color 5", 20, 52)
        ], 100, "sample-swatches");

        var channels = DefaultChannelFactory.CreateDefaultChannels(conceptId);
        var mappings = new ChannelMappingService().Map(palette, channels);

        Assert.All(mappings, mapping => Assert.True(mapping.Delta <= 10m));
        Assert.All(mappings, mapping => Assert.NotEqual(MappingStatus.Unresolved, mapping.Status));
    }
}
