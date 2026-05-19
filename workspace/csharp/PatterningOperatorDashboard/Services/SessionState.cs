using System;
using System.Collections.Generic;
using Patterning.Core.Models;
using Patterning.Core.Services;

namespace PatterningOperatorDashboard.Services;

/// <summary>
/// In-memory shared state passed between dashboard tabs (Upload → Channels → Simulation → Reports).
/// Singleton because the dashboard is a single-window operator app.
/// </summary>
public sealed class SessionState
{
    private static readonly Lazy<SessionState> instance = new(() => new SessionState());

    /// <summary>Shared instance.</summary>
    public static SessionState Current => instance.Value;

    private SessionState()
    {
        channels = DefaultChannelFactory.CreateDefaultChannels(Guid.Empty);
    }

    private DesignConcept? concept;
    private ColorPalette? palette;
    private IReadOnlyList<ManufacturingChannel> channels;
    private IReadOnlyList<ChannelMapping> mappings = [];

    /// <summary>Raised whenever the active design palette changes.</summary>
    public event EventHandler? PaletteChanged;

    /// <summary>Raised when channel mappings have been applied from the Channels tab.</summary>
    public event EventHandler? MappingsApplied;

    /// <summary>Active uploaded concept (may be null until upload completes).</summary>
    public DesignConcept? Concept
    {
        get => concept;
        set { concept = value; }
    }

    /// <summary>Active palette extracted from the uploaded design.</summary>
    public ColorPalette? Palette
    {
        get => palette;
        set
        {
            palette = value;
            PaletteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>Editable manufacturing channels for the active concept.</summary>
    public IReadOnlyList<ManufacturingChannel> Channels
    {
        get => channels;
        set { channels = value; }
    }

    /// <summary>Last applied palette-to-channel mappings.</summary>
    public IReadOnlyList<ChannelMapping> Mappings
    {
        get => mappings;
        set
        {
            mappings = value;
            MappingsApplied?.Invoke(this, EventArgs.Empty);
        }
    }
}
