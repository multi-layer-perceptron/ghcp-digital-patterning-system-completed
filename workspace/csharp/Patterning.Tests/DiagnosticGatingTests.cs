using Patterning.Core.Models;
using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class DiagnosticGatingTests
{
    [Fact]
    public void Start_WithBlockingDiagnostic_ReturnsBlockedRun()
    {
        var service = new SimulationLifecycleService();
        var run = service.Create(4);
        var diagnostics = new[] { new ManufacturabilityDiagnostic("d1", DiagnosticSeverity.Error, "Mapping", "Blocking", null, true) };
        var blocked = service.Start(run, diagnostics);
        Assert.Equal(SimulationStatus.Blocked, blocked.Status);
    }
}
