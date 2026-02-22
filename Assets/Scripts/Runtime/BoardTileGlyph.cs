using TileRift.Core;

namespace TileRift.Runtime
{
    public static class BoardTileGlyph
    {
        public static string ToLabel(TileType tile)
        {
            return tile switch
            {
                TileType.Red => "R",
                TileType.Green => "G",
                TileType.Blue => "B",
                TileType.Yellow => "Y",
                _ => ".",
            };
        }
    }
}
