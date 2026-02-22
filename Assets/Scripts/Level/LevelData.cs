using System;

namespace TileRift.Level
{
    [Serializable]
    public sealed class LevelData
    {
        public int levelId;
        public int width;
        public int height;
        public int maxMoves;
        public string[] initialRows;
    }

    [Serializable]
    public sealed class LevelCatalog
    {
        public LevelData[] levels;
    }
}
