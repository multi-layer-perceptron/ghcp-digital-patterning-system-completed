using Patterning.Core.Models;

namespace Patterning.Core.Services;

/// <summary>Maps palette colors to the nearest active channel.</summary>
public sealed class ChannelMappingService
{
    public IReadOnlyList<ChannelMapping> Map(ColorPalette palette, IReadOnlyList<ManufacturingChannel> channels)
    {
        var activeChannels = channels.Where(channel => channel.IsEnabled).ToList();
        if (activeChannels.Count == 0)
        {
            return palette.Colors.Select(color => CreateUnresolved(color, null)).ToList();
        }

        return palette.Colors.Select(color =>
        {
            var best = activeChannels.MinBy(channel => ColorDistance(color.HexColor, channel.HexColor));
            if (best is null)
            {
                return CreateUnresolved(color, null);
            }

            var delta = ColorDistance(color.HexColor, best.HexColor);
            var status = delta == 0 ? MappingStatus.Exact : delta <= 80 ? MappingStatus.Approximate : MappingStatus.Unresolved;
            return new ChannelMapping(Guid.NewGuid(), color.Id, status == MappingStatus.Unresolved ? null : best.Id, status, delta, status == MappingStatus.Unresolved ? "No channel close enough for a stable mapping." : null);
        }).ToList();
    }

    private static ChannelMapping CreateUnresolved(PaletteColor color, ManufacturingChannel? channel) =>
        new(Guid.NewGuid(), color.Id, channel?.Id, MappingStatus.Unresolved, null, "No enabled channel is available.");

    private static double ColorDistance(string leftHex, string rightHex)
    {
        var left = Parse(leftHex);
        var right = Parse(rightHex);
        return Math.Sqrt(Math.Pow(left.r - right.r, 2) + Math.Pow(left.g - right.g, 2) + Math.Pow(left.b - right.b, 2));
    }

    private static (int r, int g, int b) Parse(string hex)
    {
        var value = hex.TrimStart('#');
        return (Convert.ToInt32(value[..2], 16), Convert.ToInt32(value.Substring(2, 2), 16), Convert.ToInt32(value.Substring(4, 2), 16));
    }
}
