using NUnit.Framework;
using TileRift.Level;
using TileRift.Runtime;
using TileRift.UI;

namespace TileRift.Tests.EditMode
{
    public sealed class GameSessionTests
    {
        [Test]
        public void Session_LoadsBoardAndConsumesMove()
        {
            var level = new LevelData
            {
                levelId = 1,
                width = 2,
                height = 2,
                maxMoves = 3,
                initialRows = new[] { "R.", ".." }
            };

            var session = new GameSession(level, initialCoin: 20);

            Assert.That(session.Hud.MovesLeft, Is.EqualTo(3));
            Assert.That(session.Hud.Coin, Is.EqualTo(20));

            var moved = session.TryMove((0, 0), (1, 0));
            Assert.That(moved, Is.True);
            Assert.That(session.Hud.MovesLeft, Is.EqualTo(2));
        }

        [Test]
        public void Session_LoseState_WhenMovesExhausted()
        {
            var level = new LevelData
            {
                levelId = 1,
                width = 2,
                height = 1,
                maxMoves = 1,
                initialRows = new[] { "R." }
            };

            var session = new GameSession(level, initialCoin: 0);
            var moved = session.TryMove((0, 0), (1, 0));

            Assert.That(moved, Is.True);
            Assert.That(session.Menu.Current, Is.EqualTo(MenuScreen.Fail));
        }

        [Test]
        public void Session_WinState_WhenMatchFormed()
        {
            var level = new LevelData
            {
                levelId = 1,
                width = 4,
                height = 1,
                maxMoves = 3,
                initialRows = new[] { "RR.R" }
            };

            var session = new GameSession(level, initialCoin: 10);
            var moved = session.TryMove((3, 0), (2, 0));

            Assert.That(moved, Is.True);
            Assert.That(session.Menu.Current, Is.EqualTo(MenuScreen.Win));
            Assert.That(session.Hud.Coin, Is.EqualTo(20));
        }
    }
}
