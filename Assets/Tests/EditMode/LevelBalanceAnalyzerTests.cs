using System.IO;
using NUnit.Framework;
using TileRift.Content;
using TileRift.Level;

namespace TileRift.Tests.EditMode
{
    public sealed class LevelBalanceAnalyzerTests
    {
        [Test]
        public void Analyzer_ProducesReportForLevelCatalog()
        {
            var json = File.ReadAllText("Assets/Data/levels_mvp.json");
            var levels = LevelLoader.LoadMany(json);
            var report = LevelBalanceAnalyzer.Analyze(levels);

            Assert.That(report.samples.Count, Is.EqualTo(60));
            Assert.That(report.averageScoreAll, Is.GreaterThan(0f));
            Assert.That(report.averageScoreAll, Is.LessThan(1f));
        }

        [Test]
        public void Analyzer_First15NotHardBlocked_ForCurrentCatalog()
        {
            var json = File.ReadAllText("Assets/Data/levels_mvp.json");
            var levels = LevelLoader.LoadMany(json);
            var report = LevelBalanceAnalyzer.Analyze(levels);

            Assert.That(report.hasEarlyHardBlock, Is.False);
            Assert.That(report.averageScoreFirst15, Is.LessThan(0.78f));
        }
    }
}
