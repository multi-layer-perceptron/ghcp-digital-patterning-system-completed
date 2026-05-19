using Patterning.Core.Models;
using Patterning.Core.Validation;

namespace Patterning.Core.Services;

/// <summary>Applies user edits to manufacturing channels.</summary>
public sealed class ManufacturingChannelService
{
    public ManufacturingChannel Rename(ManufacturingChannel channel, string label)
    {
        ConfidentialitySafeText.EnsureSafe(label, nameof(label));
        if (string.IsNullOrWhiteSpace(label))
        {
            throw new ArgumentException("Channel label is required.", nameof(label));
        }
        return channel with { Label = label };
    }

    public ManufacturingChannel Recolor(ManufacturingChannel channel, string hexColor)
    {
        if (!hexColor.StartsWith('#') || hexColor.Length != 7)
        {
            throw new ArgumentException("Channel color must be a 6-digit hex color.", nameof(hexColor));
        }
        return channel with { HexColor = hexColor.ToUpperInvariant() };
    }
}
