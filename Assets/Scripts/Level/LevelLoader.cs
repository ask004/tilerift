using System;
using System.Collections.Generic;
using UnityEngine;

namespace TileRift.Level
{
    public static class LevelLoader
    {
        public static IReadOnlyList<LevelData> LoadMany(string json)
        {
            var catalog = JsonUtility.FromJson<LevelCatalog>(json);
            if (catalog == null || catalog.levels == null)
            {
                return Array.Empty<LevelData>();
            }

            return catalog.levels;
        }
    }
}
