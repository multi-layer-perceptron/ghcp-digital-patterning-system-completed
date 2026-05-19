using System.ComponentModel;
using Patterning.Core.Models;

namespace PatterningOperatorDashboard.ViewModels;

/// <summary>View model for production simulation status.</summary>
public sealed class SimulationDashboardViewModel : INotifyPropertyChanged
{
    private SimulationRun? activeRun;
    private IReadOnlyList<ManufacturabilityDiagnostic> diagnostics = [];

    public event PropertyChangedEventHandler? PropertyChanged;

    public SimulationRun? ActiveRun
    {
        get => activeRun;
        set { activeRun = value; OnPropertyChanged(nameof(ActiveRun)); }
    }

    public IReadOnlyList<ManufacturabilityDiagnostic> Diagnostics
    {
        get => diagnostics;
        set { diagnostics = value; OnPropertyChanged(nameof(Diagnostics)); }
    }

    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
