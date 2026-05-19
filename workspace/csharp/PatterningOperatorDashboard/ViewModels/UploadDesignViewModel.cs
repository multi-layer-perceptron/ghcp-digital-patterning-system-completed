using System.ComponentModel;
using Patterning.Core.Models;

namespace PatterningOperatorDashboard.ViewModels;

/// <summary>View model for design upload and preview.</summary>
public sealed class UploadDesignViewModel : INotifyPropertyChanged
{
    private DesignConcept? concept;
    private ImageMetadata? metadata;
    private ColorPalette? palette;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>Current design concept.</summary>
    public DesignConcept? Concept
    {
        get => concept;
        set { concept = value; OnPropertyChanged(nameof(Concept)); }
    }

    /// <summary>Current image metadata.</summary>
    public ImageMetadata? Metadata
    {
        get => metadata;
        set { metadata = value; OnPropertyChanged(nameof(Metadata)); }
    }

    /// <summary>Current extracted palette.</summary>
    public ColorPalette? Palette
    {
        get => palette;
        set { palette = value; OnPropertyChanged(nameof(Palette)); }
    }

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
