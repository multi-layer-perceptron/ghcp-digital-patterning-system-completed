using Patterning.Core.Services;
using Xunit;

namespace Patterning.Tests;

public sealed class UploadValidationTests
{
    [Fact]
    public void Validate_SupportedPng_ReturnsUploadedConcept()
    {
        var service = new UploadValidationService();
        var concept = service.Validate("generic.png", "image/png", 128, "data:image/png;base64,");
        Assert.Equal("generic.png", concept.SourceName);
    }

    [Fact]
    public void Validate_UnsupportedMime_ThrowsArgumentException()
    {
        var service = new UploadValidationService();
        Assert.Throws<ArgumentException>(() => service.Validate("generic.gif", "image/gif", 128, ""));
    }
}
