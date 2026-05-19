using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using Patterning.Core.Models;
using ImageMetadata = Patterning.Core.Models.ImageMetadata;

namespace PatterningOperatorDashboard.ViewModels;

/// <summary>View model for design upload and preview.</summary>
public sealed class UploadDesignViewModel : INotifyPropertyChanged
{
    private DesignConcept? concept;
    private ImageMetadata? metadata;
    private ColorPalette? palette;
    private ImageSource? previewImage;
    private string statusMessage = "Select a PNG/JPEG file or load the bundled sample to begin.";
    private bool isBusy;

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
        set
        {
            palette = value;
            PaletteColors.Clear();
            if (value is not null)
            {
                foreach (var c in value.Colors)
                {
                    PaletteColors.Add(c);
                }
            }
            OnPropertyChanged(nameof(Palette));
        }
    }

    /// <summary>Observable palette colors for the swatch list.</summary>
    public ObservableCollection<PaletteColor> PaletteColors { get; } = new();

    /// <summary>Preview image source rendered in the dashboard.</summary>
    public ImageSource? PreviewImage
    {
        get => previewImage;
        set { previewImage = value; OnPropertyChanged(nameof(PreviewImage)); }
    }

    /// <summary>Status / instructional text shown above the preview.</summary>
    public string StatusMessage
    {
        get => statusMessage;
        set { statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
    }

    /// <summary>True while an upload/analysis is running.</summary>
    public bool IsBusy
    {
        get => isBusy;
        set { isBusy = value; OnPropertyChanged(nameof(IsBusy)); }
    }

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
