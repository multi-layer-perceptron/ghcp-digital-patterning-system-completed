using Patterning.Core.Models;
using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class SimulationLifecycleTests
{
    [Fact]
    public void StartPauseResumeReset_FollowsLifecycle()
    {
        var service = new SimulationLifecycleService();
        var run = service.Create(4);
        run = service.Start(run, []);
        Assert.Equal(SimulationStatus.Running, run.Status);
        run = service.Pause(run);
        Assert.Equal(SimulationStatus.Paused, run.Status);
        run = service.Resume(run);
        Assert.Equal(SimulationStatus.Running, run.Status);
        run = service.Reset(run);
        Assert.Equal(SimulationStatus.Reset, run.Status);
    }
}
