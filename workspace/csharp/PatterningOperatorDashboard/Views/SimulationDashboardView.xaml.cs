using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using PatterningOperatorDashboard.Services;
using PatterningOperatorDashboard.ViewModels;

namespace PatterningOperatorDashboard.Views;

public partial class SimulationDashboardView : UserControl
{
    private readonly SimulationDashboardViewModel viewModel = new();
    private readonly DispatcherTimer timer;

    public SimulationDashboardView()
    {
        InitializeComponent();
        DataContext = viewModel;

        timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
        timer.Tick += Timer_Tick;

        SessionState.Current.MappingsApplied += (_, _) => Refresh();
        IsVisibleChanged += (_, _) => { if (IsVisible) Refresh(); };

        Refresh();
    }

    private void Refresh()
    {
        timer.Stop();
        viewModel.Refresh(SessionState.Current.Mappings);
        SessionState.Current.ProductionGrid = viewModel.Grid;
        SessionState.Current.LastRun = viewModel.ActiveRun;
    }

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
        viewModel.Start(SessionState.Current.Mappings, SessionState.Current.Channels);
        SessionState.Current.LastRun = viewModel.ActiveRun;
        if (viewModel.CanPause)
        {
            timer.Start();
        }
    }

    private void PauseButton_Click(object sender, RoutedEventArgs e)
    {
        viewModel.Pause();
        timer.Stop();
        SessionState.Current.LastRun = viewModel.ActiveRun;
    }

    private void ResumeButton_Click(object sender, RoutedEventArgs e)
    {
        viewModel.Resume();
        if (viewModel.CanPause)
        {
            timer.Start();
        }
        SessionState.Current.LastRun = viewModel.ActiveRun;
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        timer.Stop();
        viewModel.Reset();
        SessionState.Current.LastRun = viewModel.ActiveRun;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var run = viewModel.Tick(SessionState.Current.Channels, incrementPercent: 2.5m);
        SessionState.Current.LastRun = run;
        if (run is null || run.Status != Patterning.Core.Models.SimulationStatus.Running)
        {
            timer.Stop();
        }
    }
}
