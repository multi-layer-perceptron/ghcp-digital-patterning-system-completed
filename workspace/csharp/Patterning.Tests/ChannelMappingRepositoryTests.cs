using Patterning.Infrastructure.Data;
using Xunit;

namespace Patterning.Tests;

public sealed class ChannelMappingRepositoryTests
{
    [Fact]
    public void Constructor_WithOptions_CreatesRepository()
    {
        var repository = new ChannelMappingRepository(new SqlOptions { ConnectionString = "Server=.;Database=PatterningSimulator;Trusted_Connection=True;" });
        Assert.NotNull(repository);
    }
}
