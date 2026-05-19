using Patterning.Core.Validation;
using Xunit;

namespace Patterning.Tests;

public sealed class ReportConfidentialityTests
{
    [Fact]
    public void IsSafe_ControlCharacter_ReturnsFalse()
    {
        Assert.False(ConfidentialitySafeText.IsSafe("sample\u0001text"));
    }
}
