using System;
using System.IO;
using NUnit.Framework;
using TileRift.Daily;
using TileRift.Runtime;

namespace TileRift.Tests.EditMode
{
    public sealed class ProgressAndDailyTests
    {
        [Test]
        public void ProgressRepository_SaveAndLoad_Works()
        {
            var path = Path.GetTempFileName();
            try
            {
                var repo = new ProgressRepository(path);
                var data = new PlayerProgress
                {
                    coin = 42,
                    streak = 3,
                    lastLoginUtc = "2026-02-21",
                };

                repo.Save(data);
                var loaded = repo.Load();

                Assert.That(loaded.coin, Is.EqualTo(42));
                Assert.That(loaded.streak, Is.EqualTo(3));
                Assert.That(loaded.lastLoginUtc, Is.EqualTo("2026-02-21"));
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public void DailyReward_FirstLogin_GrantsBaseReward()
        {
            var progress = PlayerProgress.Default();
            DailyRewardService.ApplyToProgress(progress, new DateTime(2026, 2, 22));

            Assert.That(progress.streak, Is.EqualTo(1));
            Assert.That(progress.coin, Is.EqualTo(10));
            Assert.That(progress.lastLoginUtc, Is.EqualTo("2026-02-22"));
        }

        [Test]
        public void DailyReward_ConsecutiveLogin_IncreasesStreakAndReward()
        {
            var progress = new PlayerProgress
            {
                coin = 10,
                streak = 1,
                lastLoginUtc = "2026-02-22",
            };

            DailyRewardService.ApplyToProgress(progress, new DateTime(2026, 2, 23));

            Assert.That(progress.streak, Is.EqualTo(2));
            Assert.That(progress.coin, Is.EqualTo(22));
        }
    }
}
