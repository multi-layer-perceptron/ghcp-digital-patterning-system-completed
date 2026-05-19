using System.ComponentModel;
using Patterning.Core.Models;

namespace PatterningOperatorDashboard.ViewModels;

/// <summary>View model for channel edits and mapping results.</summary>
public sealed class ChannelMappingViewModel : INotifyPropertyChanged
{
    private IReadOnlyList<ManufacturingChannel> channels = [];
    private IReadOnlyList<ChannelMapping> mappings = [];

    public event PropertyChangedEventHandler? PropertyChanged;

    public IReadOnlyList<ManufacturingChannel> Channels
    {
        get => channels;
        set { channels = value; OnPropertyChanged(nameof(Channels)); }
    }

    public IReadOnlyList<ChannelMapping> Mappings
    {
        get => mappings;
        set { mappings = value; OnPropertyChanged(nameof(Mappings)); }
    }

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
