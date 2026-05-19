using Patterning.Core.Models;

namespace Patterning.Core.Services;

/// <summary>Creates the eight editable generic manufacturing channels.</summary>
public static class DefaultChannelFactory
{
    private static readonly string[] DefaultHexColors = ["#111111", "#E53935", "#1E88E5", "#43A047", "#FDD835", "#8E24AA", "#00ACC1", "#F4511E"];

    public static IReadOnlyList<ManufacturingChannel> CreateDefaultChannels(Guid conceptId)
    {
        return Enumerable.Range(0, 8)
            .Select(index => new ManufacturingChannel(Guid.NewGuid(), conceptId, index + 1, $"Channel {index + 1}", DefaultHexColors[index], true))
            .ToList();
    }
}
