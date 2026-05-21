namespace Patterning.Core.Validation;

/// <summary>Validates display text for confidentiality-safe workshop use.</summary>
public static class ConfidentialitySafeText
{
    private static readonly string[] BlockedTerms = ["restricted-brand", "customer", "site-specific", "internal-only"];

    /// <summary>Returns true when text avoids blocked or control terms.</summary>
    public static bool IsSafe(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true;
        }

        if (value.Any(char.IsControl))
        {
            return false;
        }

        return !BlockedTerms.Any(term => value.Contains(term, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Throws when text is not confidentiality-safe.</summary>
    public static void EnsureSafe(string? value, string parameterName)
    {
        if (!IsSafe(value))
        {
            throw new ArgumentException("Text must be confidentiality-safe and must not include identifiable terms.", parameterName);
        }
    }
}
