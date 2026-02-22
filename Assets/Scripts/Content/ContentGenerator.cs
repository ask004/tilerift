using System;
using System.Collections.Generic;
using TileRift.Level;

namespace TileRift.Content
{
    public static class ContentGenerator
    {
        public static List<LevelData> Generate60(int seed = 7)
        {
            var rng = new Random(seed);
            var levels = new List<LevelData>(60);

            for (var id = 1; id <= 60; id++)
            {
                var width = id <= 20 ? 4 : id <= 40 ? 5 : 6;
                var height = width;
                var maxMoves = id <= 20 ? 14 : id <= 40 ? 16 : 18;

                var rows = new string[height];
                for (var y = 0; y < height; y++)
                {
                    var chars = new char[width];
                    for (var x = 0; x < width; x++)
                    {
                        chars[x] = "RGB."[rng.Next(0, 4)];
                    }

                    rows[y] = new string(chars);
                }

                levels.Add(new LevelData
                {
                    levelId = id,
                    width = width,
                    height = height,
                    maxMoves = maxMoves,
                    initialRows = rows
                });
            }

            return levels;
        }
    }
}
