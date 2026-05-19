using Patterning.Core.Models;
using Patterning.Core.Validation;

namespace Patterning.Core.Services;

/// <summary>Validates uploaded PNG and JPEG design inputs.</summary>
public sealed class UploadValidationService
{
    public const long MaxFileSizeBytes = 10_485_760;
    public const int MaxDimensionPx = 4096;

    /// <summary>Validates basic upload properties and returns a design concept.</summary>
    public DesignConcept Validate(string sourceName, string mimeType, long fileSizeBytes, string previewDataUrl)
    {
        ConfidentialitySafeText.EnsureSafe(sourceName, nameof(sourceName));
        if (fileSizeBytes is <= 0 or > MaxFileSizeBytes)
        {
            throw new ArgumentException("File size must be between 1 byte and 10 MB.", nameof(fileSizeBytes));
        }

        var designMimeType = mimeType switch
        {
            "image/png" => DesignMimeType.Png,
            "image/jpeg" => DesignMimeType.Jpeg,
            _ => throw new ArgumentException("Only PNG and JPEG images are supported.", nameof(mimeType))
        };

        return new DesignConcept(Guid.NewGuid(), sourceName, SourceType.Upload, designMimeType, fileSizeBytes, previewDataUrl, AnalysisStatus.Uploaded, DateTimeOffset.UtcNow);
    }
}
