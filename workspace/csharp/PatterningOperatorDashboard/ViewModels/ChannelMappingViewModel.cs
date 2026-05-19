using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Patterning.Core.Models;
using Patterning.Core.Services;

namespace PatterningOperatorDashboard.ViewModels;

/// <summary>View model for channel edits and palette-to-channel mapping.</summary>
public sealed class ChannelMappingViewModel : INotifyPropertyChanged
{
    private readonly ChannelMappingService mappingService = new();

    private IReadOnlyList<ManufacturingChannel> channels = [];
    private string statusMessage = "Upload a design on the Upload tab to load its palette.";
    private bool isValid;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Editable manufacturing channels available for mapping.</summary>
    public IReadOnlyList<ManufacturingChannel> Channels
    {
        get => channels;
        set { channels = value; OnPropertyChanged(nameof(Channels)); }
    }

    /// <summary>One row per palette color with an operator-selectable channel.</summary>
    public ObservableCollection<ChannelMappingRow> Rows { get; } = new();

    /// <summary>Operator-facing status / validation message.</summary>
    public string StatusMessage
    {
        get => statusMessage;
        set { statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
    }

    /// <summary>True when every row has a channel selection and there are no duplicates.</summary>
    public bool IsValid
    {
        get => isValid;
        private set { isValid = value; OnPropertyChanged(nameof(IsValid)); }
    }

    /// <summary>Rebuild rows from the supplied palette using the current channels.</summary>
    public void LoadPalette(ColorPalette? palette)
    {
        Rows.Clear();
        if (palette is null || palette.Colors.Count == 0)
        {
            StatusMessage = "Upload a design on the Upload tab to load its palette.";
            Revalidate();
            return;
        }

        var suggested = mappingService.Map(palette, Channels);
        foreach (var color in palette.Colors)
        {
            var match = suggested.FirstOrDefault(m => m.PaletteColorId == color.Id);
            Rows.Add(new ChannelMappingRow(color)
            {
                SelectedChannelId = match?.ChannelId,
                Delta = match?.Delta ?? 0m,
                Status = match?.Status ?? MappingStatus.Unresolved
            });
        }

        StatusMessage = $"Loaded {palette.Colors.Count} palette colors. Adjust channels and click Apply.";
        Revalidate();
    }

    /// <summary>Auto-assign each palette color to its nearest channel using the mapping service.</summary>
    public void AutoAssign(ColorPalette? palette)
    {
        if (palette is null || palette.Colors.Count == 0)
        {
            return;
        }

        var suggested = mappingService.Map(palette, Channels);
        foreach (var row in Rows)
        {
            var match = suggested.FirstOrDefault(m => m.PaletteColorId == row.Color.Id);
            row.SelectedChannelId = match?.ChannelId;
            row.Delta = match?.Delta ?? 0m;
            row.Status = match?.Status ?? MappingStatus.Unresolved;
        }
        Revalidate();
    }

    /// <summary>Build the immutable list of mappings from current row selections.</summary>
    public IReadOnlyList<ChannelMapping> BuildMappings()
        => Rows.Select(r => new ChannelMapping(
                r.Color.Id,
                r.SelectedChannelId,
                r.Status,
                r.Delta,
                r.SelectedChannelId is null ? "Operator left this palette color unmapped." : null))
            .ToList();

    /// <summary>Recompute delta/status for a single row after the operator changes its channel.</summary>
    public void OnRowChannelChanged(ChannelMappingRow row)
    {
        var channel = Channels.FirstOrDefault(c => c.Id == row.SelectedChannelId);
        if (channel is null)
        {
            row.Status = MappingStatus.Unresolved;
            row.Delta = 0m;
        }
        else
        {
            var delta = ColorDistance(row.Color.Hex, channel.Hex);
            row.Delta = (decimal)delta;
            row.Status = delta == 0 ? MappingStatus.Exact : delta <= 80 ? MappingStatus.Approximate : MappingStatus.Unresolved;
        }
        Revalidate();
    }

    private void Revalidate()
    {
        if (Rows.Count == 0)
        {
            IsValid = false;
            return;
        }

        var assigned = Rows.Where(r => r.SelectedChannelId is not null).ToList();
        var duplicates = assigned.GroupBy(r => r.SelectedChannelId).Any(g => g.Count() > 1);
        var unmapped = Rows.Count - assigned.Count;

        if (duplicates)
        {
            StatusMessage = "Two or more palette colors share the same channel. Each channel can be used at most once.";
            IsValid = false;
        }
        else if (unmapped > 0)
        {
            StatusMessage = $"{unmapped} palette color(s) still need a channel assignment.";
            IsValid = false;
        }
        else
        {
            StatusMessage = $"All {Rows.Count} palette colors mapped. Ready to apply.";
            IsValid = true;
        }
    }

    private static double ColorDistance(string leftHex, string rightHex)
    {
        var l = Parse(leftHex);
        var r = Parse(rightHex);
        return Math.Sqrt(Math.Pow(l.r - r.r, 2) + Math.Pow(l.g - r.g, 2) + Math.Pow(l.b - r.b, 2));
    }

    private static (int r, int g, int b) Parse(string hex)
    {
        var v = hex.TrimStart('#');
        return (Convert.ToInt32(v[..2], 16), Convert.ToInt32(v.Substring(2, 2), 16), Convert.ToInt32(v.Substring(4, 2), 16));
    }

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

/// <summary>Editable row representing a single palette color and its operator-chosen channel.</summary>
public sealed class ChannelMappingRow : INotifyPropertyChanged
{
    private string? selectedChannelId;
    private decimal delta;
    private MappingStatus status = MappingStatus.Unresolved;

    public ChannelMappingRow(PaletteColor color)
    {
        Color = color;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Underlying palette color (immutable for the row's lifetime).</summary>
    public PaletteColor Color { get; }

    /// <summary>Currently selected channel id (null when unmapped).</summary>
    public string? SelectedChannelId
    {
        get => selectedChannelId;
        set { selectedChannelId = value; OnPropertyChanged(nameof(SelectedChannelId)); }
    }

    /// <summary>Euclidean RGB distance between palette color and chosen channel.</summary>
    public decimal Delta
    {
        get => delta;
        set { delta = value; OnPropertyChanged(nameof(Delta)); }
    }

    /// <summary>Mapping quality classification.</summary>
    public MappingStatus Status
    {
        get => status;
        set { status = value; OnPropertyChanged(nameof(Status)); }
    }

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
