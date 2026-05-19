using Patterning.Core.Models;

namespace Patterning.Core.Services;

/// <summary>Creates production grids from channel mappings.</summary>
public sealed class ProductionGridService
{
    public ProductionGridModel CreateGrid(ProductionGridSize size, IReadOnlyList<ChannelMapping> mappings)
    {
        var dimension = (int)size;
        var cells = new List<ProductionGridCell>(dimension * dimension);
        for (var y = 0; y < dimension; y++)
        {
            for (var x = 0; x < dimension; x++)
            {
                var mapping = mappings[(y * dimension + x) % mappings.Count];
                cells.Add(new ProductionGridCell(x, y, mapping.ChannelId?.ToString(), "#000000"));
            }
        }

        var coverage = cells.Where(cell => cell.ChannelId is not null)
            .GroupBy(cell => cell.ChannelId!)
            .ToDictionary(group => group.Key, group => (decimal)group.Count() / cells.Count * 100m);
        return new ProductionGridModel(size, cells, coverage, cells.Count, Math.Max(0, mappings.Count - 1), 0.1m);
    }
}
