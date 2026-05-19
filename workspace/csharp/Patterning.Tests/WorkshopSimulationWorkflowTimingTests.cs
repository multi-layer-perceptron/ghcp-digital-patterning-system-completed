using System.Diagnostics;
using Patterning.Core.Models;
using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class WorkshopSimulationWorkflowTimingTests
{
    [Fact]
    public void MappingGridSimulation_CompletesWithinWorkshopBudget()
    {
        var watch = Stopwatch.StartNew();
        var palette = new ColorPalette([new PaletteColor("p1", "#111111", "Black", 100, 1)], 100, "test");
        var channels = DefaultChannelFactory.CreateDefaultChannels(Guid.NewGuid());
        var mappings = new ChannelMappingService().Map(palette, channels);
        _ = new ProductionGridService().CreateGrid(ProductionGridSize.Grid64, mappings);
        var run = new SimulationLifecycleService().Create(1);
        _ = new SimulationLifecycleService().Start(run, []);
        watch.Stop();
        Assert.True(watch.ElapsedMilliseconds < 1000);
    }
}
