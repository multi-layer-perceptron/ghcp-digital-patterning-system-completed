using Patterning.Core.Models;

namespace PatterningOperatorDashboard.Rendering;

/// <summary>Builds simple preview swatches from mapped channel colors.</summary>
public sealed class MappedPreviewRenderer
{
    public IReadOnlyList<string> RenderSwatches(IReadOnlyList<ChannelMapping> mappings, IReadOnlyList<ManufacturingChannel> channels)
    {
        return mappings.Select(mapping =>
        {
            var channel = mapping.ChannelId is not null ? channels.FirstOrDefault(item => item.Id == mapping.ChannelId) : null;
            return channel?.Hex ?? "#999999";
        }).ToList();
    }
}
