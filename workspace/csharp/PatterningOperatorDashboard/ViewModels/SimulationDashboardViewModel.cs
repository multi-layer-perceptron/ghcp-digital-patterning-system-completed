using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Patterning.Core.Models;
using Patterning.Core.Services;

namespace PatterningOperatorDashboard.ViewModels;

/// <summary>View model for the production simulation tab.</summary>
public sealed class SimulationDashboardViewModel : INotifyPropertyChanged
{
    private const int TotalPassesDefault = 20;

    private readonly DiagnosticService diagnosticService = new();
    private readonly SimulationLifecycleService lifecycleService = new();
    private readonly ProductionGridService gridService = new();

    private SimulationRun? activeRun;
    private IReadOnlyList<ManufacturabilityDiagnostic> diagnostics = [];
    private ProductionGridModel? grid;
    private string statusMessage = "Apply a channel mapping to enable simulation.";
    private decimal progressPercent;
    private int currentPass;
    private int totalPasses = TotalPassesDefault;
    private bool canStart;
    private bool canPause;
    private bool canResume;
    private bool canReset;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<SimulationEvent> Events { get; } = [];

    public IReadOnlyList<ManufacturabilityDiagnostic> Diagnostics
    {
        get => diagnostics;
        private set { diagnostics = value; OnPropertyChanged(nameof(Diagnostics)); OnPropertyChanged(nameof(DiagnosticsSummary)); }
    }

    public SimulationRun? ActiveRun
    {
        get => activeRun;
        private set { activeRun = value; OnPropertyChanged(nameof(ActiveRun)); OnPropertyChanged(nameof(StatusBadge)); }
    }

    public ProductionGridModel? Grid
    {
        get => grid;
        private set { grid = value; OnPropertyChanged(nameof(Grid)); OnPropertyChanged(nameof(GridSummary)); }
    }

    public string StatusMessage
    {
        get => statusMessage;
        private set { statusMessage = value; OnPropertyChanged(nameof(StatusMessage)); }
    }

    public decimal ProgressPercent
    {
        get => progressPercent;
        private set { progressPercent = value; OnPropertyChanged(nameof(ProgressPercent)); }
    }

    public int CurrentPass
    {
        get => currentPass;
        private set { currentPass = value; OnPropertyChanged(nameof(CurrentPass)); OnPropertyChanged(nameof(PassLabel)); }
    }

    public int TotalPasses
    {
        get => totalPasses;
        private set { totalPasses = value; OnPropertyChanged(nameof(TotalPasses)); OnPropertyChanged(nameof(PassLabel)); }
    }

    public bool CanStart { get => canStart; private set { canStart = value; OnPropertyChanged(nameof(CanStart)); } }
    public bool CanPause { get => canPause; private set { canPause = value; OnPropertyChanged(nameof(CanPause)); } }
    public bool CanResume { get => canResume; private set { canResume = value; OnPropertyChanged(nameof(CanResume)); } }
    public bool CanReset { get => canReset; private set { canReset = value; OnPropertyChanged(nameof(CanReset)); } }

    public string StatusBadge => ActiveRun is null ? "Not Started" : ActiveRun.Status.ToString();

    public string PassLabel => $"Pass {CurrentPass} / {TotalPasses}";

    public string DiagnosticsSummary => Diagnostics.Count == 0
        ? "No manufacturability issues detected."
        : $"{Diagnostics.Count(d => d.Blocking)} blocking, {Diagnostics.Count(d => !d.Blocking)} advisory.";

    public string GridSummary => Grid is null
        ? "Production grid not generated yet."
        : $"Grid {(int)Grid.GridSize}×{(int)Grid.GridSize} · {Grid.EstimatedCommandCount} commands · {Grid.ChannelSwitchCount} channel switches";

    /// <summary>Re-evaluates diagnostics and resets simulation state for the current mappings.</summary>
    public void Refresh(IReadOnlyList<ChannelMapping> mappings)
    {
        Events.Clear();
        ProgressPercent = 0;
        CurrentPass = 0;

        if (mappings.Count == 0)
        {
            Diagnostics = [];
            Grid = null;
            ActiveRun = null;
            StatusMessage = "Apply a channel mapping on the Channels tab to enable simulation.";
            UpdateCommands(running: false);
            return;
        }

        Diagnostics = diagnosticService.Evaluate(mappings);
        Grid = gridService.CreateGrid(ProductionGridSize.Grid64, mappings);
        ActiveRun = lifecycleService.Create(TotalPasses);
        StatusMessage = diagnosticService.HasBlockingErrors(Diagnostics)
            ? "Blocking diagnostics present — resolve unmapped colors before starting."
            : "Ready. Press Start to begin simulating production.";
        UpdateCommands(running: false);
    }

    public void Start(IReadOnlyList<ChannelMapping> mappings, IReadOnlyList<ManufacturingChannel> channels)
    {
        if (ActiveRun is null) return;
        var started = lifecycleService.Start(ActiveRun, Diagnostics);
        ActiveRun = started;
        if (started.Status == SimulationStatus.Blocked)
        {
            StatusMessage = "Simulation blocked — fix the diagnostics listed below.";
            AppendEvent("Blocked", "Start aborted because blocking diagnostics were detected.", null);
            UpdateCommands(running: false);
            return;
        }
        StatusMessage = "Running…";
        AppendEvent("Start", $"Simulation started against {mappings.Count} mapped color(s) on {channels.Count} channel(s).", null);
        UpdateCommands(running: true);
    }

    public void Pause()
    {
        if (ActiveRun is null) return;
        ActiveRun = lifecycleService.Pause(ActiveRun);
        if (ActiveRun.Status == SimulationStatus.Paused)
        {
            StatusMessage = "Paused.";
            AppendEvent("Pause", "Operator paused the simulation.", null);
            UpdateCommands(running: false, paused: true);
        }
    }

    public void Resume()
    {
        if (ActiveRun is null) return;
        ActiveRun = lifecycleService.Resume(ActiveRun);
        if (ActiveRun.Status == SimulationStatus.Running)
        {
            StatusMessage = "Running…";
            AppendEvent("Resume", "Operator resumed the simulation.", null);
            UpdateCommands(running: true);
        }
    }

    public void Reset()
    {
        if (ActiveRun is null) return;
        ActiveRun = lifecycleService.Reset(ActiveRun);
        ProgressPercent = 0;
        CurrentPass = 0;
        Events.Clear();
        StatusMessage = diagnosticService.HasBlockingErrors(Diagnostics)
            ? "Reset. Blocking diagnostics still present."
            : "Reset. Press Start to begin again.";
        AppendEvent("Reset", "Simulation state cleared.", null);
        UpdateCommands(running: false);
    }

    /// <summary>Advance the simulation by one tick. Returns the run after the tick.</summary>
    public SimulationRun? Tick(IReadOnlyList<ManufacturingChannel> channels, decimal incrementPercent)
    {
        if (ActiveRun is null || ActiveRun.Status != SimulationStatus.Running) return ActiveRun;

        var newProgress = Math.Min(100m, ActiveRun.ProgressPercent + incrementPercent);
        var newPass = Math.Min(TotalPasses, (int)Math.Floor(newProgress / 100m * TotalPasses));
        var active = channels.Count == 0
            ? Array.Empty<string>()
            : new[] { channels[newPass % channels.Count].Label };

        if (newPass > ActiveRun.CurrentPass)
        {
            AppendEvent("Pass", $"Completed pass {newPass} of {TotalPasses}.", active.FirstOrDefault());
        }

        var status = newProgress >= 100m ? SimulationStatus.Completed : SimulationStatus.Running;
        var completedAt = status == SimulationStatus.Completed ? DateTimeOffset.UtcNow : ActiveRun.CompletedAt;

        ActiveRun = ActiveRun with
        {
            ProgressPercent = newProgress,
            CurrentPass = newPass,
            ActiveChannels = active,
            Status = status,
            CompletedAt = completedAt,
            EventStream = Events.ToList(),
        };
        ProgressPercent = newProgress;
        CurrentPass = newPass;

        if (status == SimulationStatus.Completed)
        {
            StatusMessage = "Simulation complete. Continue to the Reports tab.";
            AppendEvent("Complete", "All passes finished.", null);
            UpdateCommands(running: false, completed: true);
        }

        return ActiveRun;
    }

    private void AppendEvent(string type, string message, string? channelId)
    {
        Events.Add(new SimulationEvent(Events.Count + 1, DateTimeOffset.Now, type, message, channelId));
    }

    private void UpdateCommands(bool running, bool paused = false, bool completed = false)
    {
        var hasRun = ActiveRun is not null;
        var blocked = diagnosticService.HasBlockingErrors(Diagnostics);
        CanStart = hasRun && !running && !paused && !completed && !blocked;
        CanPause = running;
        CanResume = paused;
        CanReset = hasRun && (running || paused || completed || ActiveRun?.Status == SimulationStatus.Blocked);
    }

    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

