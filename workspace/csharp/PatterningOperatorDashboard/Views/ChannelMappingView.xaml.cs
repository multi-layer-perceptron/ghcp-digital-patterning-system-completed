using System.Windows;
using System.Windows.Controls;
using PatterningOperatorDashboard.Services;
using PatterningOperatorDashboard.ViewModels;

namespace PatterningOperatorDashboard.Views;

/// <summary>
/// Code-behind for the Channels tab. Pulls the active palette from
/// <see cref="SessionState"/>, lets the operator map palette colors to manufacturing
/// channels, validates, and writes the result back to the session for downstream tabs.
/// </summary>
public partial class ChannelMappingView : UserControl
{
    private readonly ChannelMappingViewModel viewModel = new();

    public ChannelMappingView()
    {
        InitializeComponent();
        viewModel.Channels = SessionState.Current.Channels;
        DataContext = viewModel;

        SessionState.Current.PaletteChanged += (_, _) => Refresh();
        IsVisibleChanged += (_, _) => { if (IsVisible) Refresh(); };

        Refresh();
    }

    private void Refresh()
    {
        viewModel.Channels = SessionState.Current.Channels;
        viewModel.LoadPalette(SessionState.Current.Palette);
    }

    private void AutoAssignButton_Click(object sender, RoutedEventArgs e)
        => viewModel.AutoAssign(SessionState.Current.Palette);

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        if (!viewModel.IsValid)
        {
            return;
        }

        var mappings = viewModel.BuildMappings();
        SessionState.Current.Mappings = mappings;
        viewModel.StatusMessage = $"Applied mapping for {mappings.Count} palette color(s). Continue to the Simulation tab.";
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
        => viewModel.LoadPalette(SessionState.Current.Palette);

    private void ChannelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox { DataContext: ChannelMappingRow row })
        {
            viewModel.OnRowChannelChanged(row);
        }
    }
}
