using System;
using System.Collections.Generic;
using TileRift.Level;
using UnityEngine;

namespace TileRift.Content
{
    [Serializable]
    public sealed class LevelDifficultySample
    {
        public int levelId;
        public float density;
        public float mobility;
        public float score;
    }

    [Serializable]
    public sealed class BalanceReport
    {
        public bool hasEarlyHardBlock;
        public float averageScoreFirst15;
        public float averageScoreAll;
        public List<LevelDifficultySample> samples = new();
    }

    public static class LevelBalanceAnalyzer
    {
        public static BalanceReport Analyze(IReadOnlyList<LevelData> levels)
        {
            var report = new BalanceReport();
            if (levels == null || levels.Count == 0)
            {
                report.hasEarlyHardBlock = true;
                return report;
            }

            float sumAll = 0f;
            float sumFirst = 0f;
            var firstCount = Math.Min(15, levels.Count);

            for (var i = 0; i < levels.Count; i++)
            {
                var level = levels[i];
                var sample = AnalyzeLevel(level);
                report.samples.Add(sample);
                sumAll += sample.score;
                if (i < firstCount)
                {
                    sumFirst += sample.score;
                }
            }

            report.averageScoreAll = sumAll / levels.Count;
            report.averageScoreFirst15 = sumFirst / firstCount;
            report.hasEarlyHardBlock = report.averageScoreFirst15 > 0.78f;
            return report;
        }

        private static LevelDifficultySample AnalyzeLevel(LevelData level)
        {
            var filled = 0;
            var total = level.width * level.height;
            var empty = 0;

            for (var y = 0; y < level.height; y++)
            {
                var row = level.initialRows[y];
                for (var x = 0; x < level.width; x++)
                {
                    if (row[x] == '.')
                    {
                        empty++;
                    }
                    else
                    {
                        filled++;
                    }
                }
            }

            var density = total == 0 ? 1f : (float)filled / total;
            var mobility = total == 0 ? 0f : (float)empty / total;

            var baseScore = (density * 0.65f) + ((1f - mobility) * 0.35f);
            var moveRelief = Math.Min(1f, (float)level.maxMoves / (level.width + level.height + 8));
            var score = Mathf.Clamp01(baseScore - (moveRelief * 0.15f));

            return new LevelDifficultySample
            {
                levelId = level.levelId,
                density = density,
                mobility = mobility,
                score = score,
            };
        }
    }
}
