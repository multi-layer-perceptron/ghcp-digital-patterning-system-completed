using Patterning.Core.Models;

namespace Patterning.Core.Services;

/// <summary>Creates the eight editable generic manufacturing channels.</summary>
public static class DefaultChannelFactory
{
    private static readonly string[] DefaultHexColors = ["#2C6F91", "#B9573F", "#D2A13D", "#7B8F45", "#3E4F63", "#2C6F91", "#B9573F", "#D2A13D"];

    public static IReadOnlyList<ManufacturingChannel> CreateDefaultChannels(Guid conceptId)
    {
        _ = conceptId;
        return Enumerable.Range(0, 8)
            .Select(index => new ManufacturingChannel($"channel-{index + 1}", $"Channel {index + 1}", DefaultHexColors[index], index + 1))
            .ToList();
    }
}
