using System.Diagnostics;
using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class LifecycleLatencyTests
{
    [Fact]
    public void LifecycleCommands_CompleteWithinInteractiveBudget()
    {
        var service = new SimulationLifecycleService();
        var run = service.Create(4);
        var watch = Stopwatch.StartNew();
        run = service.Start(run, []);
        run = service.Pause(run);
        run = service.Resume(run);
        _ = service.Reset(run);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 500);
    }
}
