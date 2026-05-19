using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class ManufacturingChannelTests
{
    [Fact]
    public void CreateDefaultChannels_ReturnsEightChannels()
    {
        var channels = DefaultChannelFactory.CreateDefaultChannels(Guid.NewGuid());
        Assert.Equal(8, channels.Count);
    }

    [Fact]
    public void Rename_ValidLabel_UpdatesLabel()
    {
        var channel = DefaultChannelFactory.CreateDefaultChannels(Guid.NewGuid())[0];
        var edited = new ManufacturingChannelService().Rename(channel, "Base Layer");
        Assert.Equal("Base Layer", edited.Label);
    }
}
