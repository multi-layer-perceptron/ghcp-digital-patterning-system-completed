using Patterning.Core.Models;

namespace Patterning.Core.Services;

/// <summary>Controls production simulation lifecycle transitions.</summary>
public sealed class SimulationLifecycleService
{
    public SimulationRun Create(int totalPasses) => new(Guid.NewGuid(), SimulationStatus.NotStarted, 0, 0, totalPasses, [], [], null, null);

    public SimulationRun Start(SimulationRun run, IReadOnlyList<ManufacturabilityDiagnostic> diagnostics)
    {
        if (diagnostics.Any(diagnostic => diagnostic.Blocking))
        {
            return run with { Status = SimulationStatus.Blocked };
        }
        return run with { Status = SimulationStatus.Running, StartedAt = DateTimeOffset.UtcNow };
    }

    public SimulationRun Pause(SimulationRun run) => run.Status == SimulationStatus.Running ? run with { Status = SimulationStatus.Paused } : run;

    public SimulationRun Resume(SimulationRun run) => run.Status == SimulationStatus.Paused ? run with { Status = SimulationStatus.Running } : run;

    public SimulationRun Reset(SimulationRun run) => run with { Status = SimulationStatus.Reset, ProgressPercent = 0, CurrentPass = 0, ActiveChannels = [] };
}
