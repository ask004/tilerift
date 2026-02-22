using System;
using System.IO;
using NUnit.Framework;
using TileRift.Daily;
using TileRift.Level;

namespace TileRift.Tests.EditMode
{
    public sealed class DailyChallengeSelectorTests
    {
        [Test]
        public void Selector_IsDeterministic_ForSameDate()
        {
            var date = new DateTime(2026, 2, 22, 0, 0, 0, DateTimeKind.Utc);
            var idx1 = DailyChallengeSelector.SelectLevelIndex(date, 60);
            var idx2 = DailyChallengeSelector.SelectLevelIndex(date, 60);

            Assert.That(idx1, Is.EqualTo(idx2));
            Assert.That(idx1, Is.InRange(0, 59));
        }

        [Test]
        public void Selector_PicksLevel_FromCatalog()
        {
            var json = File.ReadAllText("Assets/Data/levels_mvp.json");
            var levels = LevelLoader.LoadMany(json);
            var date = new DateTime(2026, 2, 22, 0, 0, 0, DateTimeKind.Utc);

            var level = DailyChallengeSelector.SelectLevel(date, levels);

            Assert.That(level, Is.Not.Null);
            Assert.That(level.levelId, Is.InRange(1, 60));
        }
    }
}
