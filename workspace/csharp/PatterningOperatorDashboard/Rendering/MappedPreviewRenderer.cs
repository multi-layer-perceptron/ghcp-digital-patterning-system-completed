using Patterning.Core.Models;

namespace PatterningOperatorDashboard.Rendering;

/// <summary>Builds simple preview swatches from mapped channel colors.</summary>
public sealed class MappedPreviewRenderer
{
    public IReadOnlyList<string> RenderSwatches(IReadOnlyList<ChannelMapping> mappings, IReadOnlyList<ManufacturingChannel> channels)
    {
        return mappings.Select(mapping =>
        {
            var channel = mapping.ChannelId.HasValue ? channels.FirstOrDefault(item => item.Id == mapping.ChannelId.Value) : null;
            return channel?.HexColor ?? "#999999";
        }).ToList();
    }
}
