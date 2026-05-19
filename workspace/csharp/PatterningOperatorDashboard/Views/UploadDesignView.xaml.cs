using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Patterning.Core.Models;
using Patterning.Core.Services;
using PatterningOperatorDashboard.ViewModels;
using ImageMetadata = Patterning.Core.Models.ImageMetadata;

namespace PatterningOperatorDashboard.Views;

/// <summary>
/// Code-behind for the Upload tab. Wires Browse / Load Sample / Reset actions to
/// <see cref="UploadValidationService"/> and <see cref="ConceptAnalysisService"/>
/// and binds results to <see cref="UploadDesignViewModel"/>.
/// </summary>
public partial class UploadDesignView : UserControl
{
    private readonly UploadDesignViewModel viewModel = new();
    private readonly UploadValidationService uploadValidator = new();
    private readonly ConceptAnalysisService conceptAnalyzer = new();

    public UploadDesignView()
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select a design image",
            Filter = "PNG/JPEG images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            viewModel.IsBusy = true;
            viewModel.StatusMessage = $"Validating {Path.GetFileName(dialog.FileName)}...";

            var fileInfo = new FileInfo(dialog.FileName);
            var mimeType = MimeFromExtension(fileInfo.Extension);
            var previewBitmap = LoadBitmap(dialog.FileName);

            var concept = uploadValidator.Validate(
                sourceName: fileInfo.Name,
                mimeType: mimeType,
                fileSizeBytes: fileInfo.Length,
                previewDataUrl: new Uri(fileInfo.FullName).AbsoluteUri);

            var metadata = ExtractMetadata(previewBitmap);
            var palette = ExtractPalette(previewBitmap);
            var analyzed = conceptAnalyzer.Analyze(concept, metadata, palette);

            viewModel.PreviewImage = previewBitmap;
            viewModel.Concept = analyzed.Concept;
            viewModel.Metadata = analyzed.Metadata;
            viewModel.Palette = analyzed.Palette;
            viewModel.StatusMessage = $"Analyzed {fileInfo.Name} — {metadata.WidthPx} × {metadata.HeightPx}, {palette.Colors.Count} palette colors.";
        }
        catch (Exception ex)
        {
            viewModel.StatusMessage = $"Upload failed: {ex.Message}";
        }
        finally
        {
            viewModel.IsBusy = false;
        }
    }

    private void SampleButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            viewModel.IsBusy = true;
            viewModel.StatusMessage = "Loading bundled sample...";

            var samplePath = ResolveSamplePath("generic-floorcovering-sample.json");
            if (samplePath is null)
            {
                viewModel.StatusMessage = "Sample manifest not found under workspace/assets/samples/.";
                return;
            }

            using var stream = File.OpenRead(samplePath);
            using var doc = JsonDocument.Parse(stream);
            var root = doc.RootElement;

            var sourceName = root.GetProperty("source_name").GetString() ?? "sample.png";
            var mimeType = root.GetProperty("mime_type").GetString() ?? "image/png";
            var fileSize = root.GetProperty("file_size_bytes").GetInt64();
            var widthPx = root.GetProperty("width_px").GetInt32();
            var heightPx = root.GetProperty("height_px").GetInt32();

            var concept = uploadValidator.Validate(
                sourceName: sourceName,
                mimeType: mimeType,
                fileSizeBytes: fileSize,
                previewDataUrl: new Uri(samplePath).AbsoluteUri);

            var aspect = heightPx == 0 ? 1m : Math.Round((decimal)widthPx / heightPx, 3);
            var metadata = new ImageMetadata(widthPx, heightPx, aspect, EstimatedUniqueColors: 4, HasTransparency: false, BackgroundIndicator: null);
            var palette = BuildSyntheticPalette();
            var analyzed = conceptAnalyzer.Analyze(concept, metadata, palette);

            viewModel.PreviewImage = BuildSyntheticPreview(palette, widthPx, heightPx);
            viewModel.Concept = analyzed.Concept;
            viewModel.Metadata = analyzed.Metadata;
            viewModel.Palette = analyzed.Palette;
            viewModel.StatusMessage = $"Loaded sample '{sourceName}' — {widthPx} × {heightPx}, {palette.Colors.Count} synthetic palette colors.";
        }
        catch (Exception ex)
        {
            viewModel.StatusMessage = $"Sample load failed: {ex.Message}";
        }
        finally
        {
            viewModel.IsBusy = false;
        }
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        viewModel.Concept = null;
        viewModel.Metadata = null;
        viewModel.Palette = null;
        viewModel.PreviewImage = null;
        viewModel.StatusMessage = "Select a PNG/JPEG file or load the bundled sample to begin.";
    }

    private static string MimeFromExtension(string extension) => extension.ToLowerInvariant() switch
    {
        ".png" => "image/png",
        ".jpg" or ".jpeg" => "image/jpeg",
        _ => throw new InvalidOperationException($"Unsupported file extension: {extension}")
    };

    private static BitmapImage LoadBitmap(string path)
    {
        var bmp = new BitmapImage();
        bmp.BeginInit();
        bmp.CacheOption = BitmapCacheOption.OnLoad;
        bmp.UriSource = new Uri(path);
        bmp.EndInit();
        bmp.Freeze();
        return bmp;
    }

    private static ImageMetadata ExtractMetadata(BitmapSource bitmap)
    {
        var width = bitmap.PixelWidth;
        var height = bitmap.PixelHeight;
        var aspect = height == 0 ? 1m : Math.Round((decimal)width / height, 3);
        var hasTransparency = bitmap.Format == PixelFormats.Bgra32 || bitmap.Format == PixelFormats.Pbgra32;
        var pixels = SamplePixels(bitmap, maxSamples: 4096);
        var uniqueQuantized = pixels.Select(p => QuantizeKey(p)).Distinct().Count();
        return new ImageMetadata(width, height, aspect, uniqueQuantized, hasTransparency, BackgroundIndicator: null);
    }

    private static ColorPalette ExtractPalette(BitmapSource bitmap)
    {
        var pixels = SamplePixels(bitmap, maxSamples: 4096);
        if (pixels.Count == 0)
        {
            return new ColorPalette(Array.Empty<PaletteColor>(), 0m, "sampled-rgb-buckets");
        }

        var buckets = pixels.GroupBy(QuantizeKey)
            .Select(g => new
            {
                Count = g.Count(),
                AvgR = (byte)g.Average(p => p.R),
                AvgG = (byte)g.Average(p => p.G),
                AvgB = (byte)g.Average(p => p.B)
            })
            .OrderByDescending(b => b.Count)
            .Take(5)
            .ToList();

        var total = pixels.Count;
        var coverageTotal = 0m;
        var colors = new List<PaletteColor>(buckets.Count);
        for (var i = 0; i < buckets.Count; i++)
        {
            var b = buckets[i];
            var hex = $"#{b.AvgR:X2}{b.AvgG:X2}{b.AvgB:X2}";
            var coverage = Math.Round((decimal)b.Count * 100m / total, 2);
            coverageTotal += coverage;
            colors.Add(new PaletteColor($"p{i + 1}", hex, $"Color {i + 1}", coverage, b.Count));
        }
        return new ColorPalette(colors, coverageTotal, "sampled-rgb-buckets");
    }

    private static IReadOnlyList<(byte R, byte G, byte B)> SamplePixels(BitmapSource bitmap, int maxSamples)
    {
        var converted = bitmap.Format == PixelFormats.Bgra32
            ? bitmap
            : new FormatConvertedBitmap(bitmap, PixelFormats.Bgra32, null, 0);

        var width = converted.PixelWidth;
        var height = converted.PixelHeight;
        if (width == 0 || height == 0)
        {
            return Array.Empty<(byte, byte, byte)>();
        }

        var stride = width * 4;
        var pixelBytes = new byte[stride * height];
        converted.CopyPixels(pixelBytes, stride, 0);

        var totalPixels = width * height;
        var step = Math.Max(1, totalPixels / maxSamples);
        var samples = new List<(byte R, byte G, byte B)>(Math.Min(maxSamples, totalPixels));
        for (var i = 0; i < totalPixels; i += step)
        {
            var offset = i * 4;
            samples.Add((pixelBytes[offset + 2], pixelBytes[offset + 1], pixelBytes[offset]));
        }
        return samples;
    }

    private static (byte R, byte G, byte B) QuantizeKey((byte R, byte G, byte B) pixel)
        => ((byte)(pixel.R & 0xF0), (byte)(pixel.G & 0xF0), (byte)(pixel.B & 0xF0));

    private static ColorPalette BuildSyntheticPalette()
    {
        var colors = new List<PaletteColor>
        {
            new("p1", "#1F4E79", "Color 1", 40m, 102),
            new("p2", "#E8E2D5", "Color 2", 30m, 77),
            new("p3", "#A8443A", "Color 3", 20m, 51),
            new("p4", "#3C7A4E", "Color 4", 10m, 26)
        };
        return new ColorPalette(colors, 100m, "sample-manifest");
    }

    private static BitmapSource BuildSyntheticPreview(ColorPalette palette, int widthPx, int heightPx)
    {
        var width = Math.Max(widthPx, 64);
        var height = Math.Max(heightPx, 64);
        var stride = width * 4;
        var pixels = new byte[stride * height];
        var colors = palette.Colors.Count > 0 ? palette.Colors : new[] { new PaletteColor("p1", "#808080", "Color 1", 100m, 1) };
        var bandHeight = Math.Max(1, height / colors.Count);

        for (var y = 0; y < height; y++)
        {
            var idx = Math.Min(colors.Count - 1, y / bandHeight);
            var color = (Color)ColorConverter.ConvertFromString(colors[idx].Hex)!;
            for (var x = 0; x < width; x++)
            {
                var offset = y * stride + x * 4;
                pixels[offset + 0] = color.B;
                pixels[offset + 1] = color.G;
                pixels[offset + 2] = color.R;
                pixels[offset + 3] = 255;
            }
        }

        var bmp = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgra32, null, pixels, stride);
        bmp.Freeze();
        return bmp;
    }

    private static string? ResolveSamplePath(string fileName)
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "workspace", "assets", "samples", fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }
            var siblingCandidate = Path.Combine(current.FullName, "assets", "samples", fileName);
            if (File.Exists(siblingCandidate))
            {
                return siblingCandidate;
            }
            current = current.Parent;
        }
        return null;
    }
}
