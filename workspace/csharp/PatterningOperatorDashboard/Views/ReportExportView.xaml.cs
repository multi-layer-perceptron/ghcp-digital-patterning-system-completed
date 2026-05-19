using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using PatterningOperatorDashboard.Services;
using PatterningOperatorDashboard.ViewModels;

namespace PatterningOperatorDashboard.Views;

/// <summary>
/// Code-behind for the Reports tab. Assembles a <c>ConceptReport</c> from the shared
/// <see cref="SessionState"/> and exports it as JSON or HTML via <c>SaveFileDialog</c>.
/// </summary>
public partial class ReportExportView : UserControl
{
    private readonly ReportExportViewModel viewModel = new();

    public ReportExportView()
    {
        InitializeComponent();
        DataContext = viewModel;

        SessionState.Current.SimulationCompleted += (_, _) => Refresh();
        SessionState.Current.MappingsApplied += (_, _) => Refresh();
        SessionState.Current.PaletteChanged += (_, _) => Refresh();
        IsVisibleChanged += (_, _) => { if (IsVisible) Refresh(); };
    }

    private void Refresh()
    {
        var session = SessionState.Current;
        viewModel.Generate(
            session.Concept,
            session.Metadata,
            session.Palette,
            session.Channels,
            session.Mappings,
            session.ProductionGrid,
            session.LastRun);
    }

    private void GenerateButton_Click(object sender, RoutedEventArgs e) => Refresh();

    private void ExportJsonButton_Click(object sender, RoutedEventArgs e)
        => ExportTo("Export report as JSON", "JSON files (*.json)|*.json", "report.json", viewModel.ExportJson);

    private void ExportHtmlButton_Click(object sender, RoutedEventArgs e)
        => ExportTo("Export report as HTML", "HTML files (*.html)|*.html", "report.html", viewModel.ExportHtml);

    private void ExportTo(string title, string filter, string defaultName, Func<string> render)
    {
        if (!viewModel.CanExport)
        {
            return;
        }

        var dialog = new SaveFileDialog
        {
            Title = title,
            Filter = filter,
            FileName = defaultName
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            File.WriteAllText(dialog.FileName, render());
            viewModel.LastExportPath = dialog.FileName;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Export failed: {ex.Message}", "Reports", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
