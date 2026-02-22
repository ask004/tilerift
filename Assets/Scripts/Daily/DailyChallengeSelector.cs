using System;
using System.Collections.Generic;
using TileRift.Level;

namespace TileRift.Daily
{
    public static class DailyChallengeSelector
    {
        public static int SelectLevelIndex(DateTime dayUtc, int levelCount)
        {
            if (levelCount <= 0)
            {
                return 0;
            }

            var seed = DailyChallengeService.SeedFor(dayUtc.Date);
            return seed % levelCount;
        }

        public static LevelData SelectLevel(DateTime dayUtc, IReadOnlyList<LevelData> levels)
        {
            if (levels == null || levels.Count == 0)
            {
                return null;
            }

            var index = SelectLevelIndex(dayUtc, levels.Count);
            return levels[index];
        }
    }
}
