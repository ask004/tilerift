using System.IO;
using NUnit.Framework;
using TileRift.Level;

namespace TileRift.Tests.EditMode
{
    public sealed class LevelCatalogFileTests
    {
        [Test]
        public void LevelCatalog_Has60Levels()
        {
            var json = File.ReadAllText("Assets/Data/levels_mvp.json");
            var levels = LevelLoader.LoadMany(json);

            Assert.That(levels.Count, Is.EqualTo(60));
            Assert.That(levels[0].levelId, Is.EqualTo(1));
            Assert.That(levels[59].levelId, Is.EqualTo(60));
        }

        [Test]
        public void LevelCatalog_DifficultyBands_AreConfigured()
        {
            var json = File.ReadAllText("Assets/Data/levels_mvp.json");
            var levels = LevelLoader.LoadMany(json);

            Assert.That(levels[0].width, Is.EqualTo(4));
            Assert.That(levels[19].width, Is.EqualTo(4));
            Assert.That(levels[20].width, Is.EqualTo(5));
            Assert.That(levels[39].width, Is.EqualTo(5));
            Assert.That(levels[40].width, Is.EqualTo(6));
            Assert.That(levels[59].width, Is.EqualTo(6));
        }
    }
}
