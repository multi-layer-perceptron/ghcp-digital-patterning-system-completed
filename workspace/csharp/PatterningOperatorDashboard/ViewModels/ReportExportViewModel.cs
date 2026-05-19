using System.ComponentModel;

namespace PatterningOperatorDashboard.ViewModels;

/// <summary>View model for report export commands.</summary>
public sealed class ReportExportViewModel : INotifyPropertyChanged
{
    private string? lastExportPath;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string? LastExportPath
    {
        get => lastExportPath;
        set { lastExportPath = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastExportPath))); }
    }
}
