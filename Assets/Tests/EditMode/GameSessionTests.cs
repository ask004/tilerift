using System;
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

        [Test]
        public void Session_UndoBooster_RestoresBoard()
        {
            var level = new LevelData
            {
                levelId = 1,
                width = 2,
                height = 1,
                maxMoves = 4,
                initialRows = new[] { "R." }
            };

            var session = new GameSession(level, initialCoin: 0);
            Assert.That(session.TryMove((0, 0), (1, 0)), Is.True);
            Assert.That(session.Board.Get(1, 0), Is.EqualTo(TileRift.Core.TileType.Red));

            Assert.That(session.TryUndo(), Is.True);
            Assert.That(session.Board.Get(0, 0), Is.EqualTo(TileRift.Core.TileType.Red));
            Assert.That(session.Board.Get(1, 0), Is.EqualTo(TileRift.Core.TileType.None));
        }

        [Test]
        public void Session_HintAndShuffleBoosters_Work()
        {
            var level = new LevelData
            {
                levelId = 1,
                width = 2,
                height = 2,
                maxMoves = 6,
                initialRows = new[] { "R.", ".G" }
            };

            var session = new GameSession(level, initialCoin: 0);
            var hint = session.TryHint();
            Assert.That(hint.HasValue, Is.True);

            var before = new[]
            {
                session.Board.Get(0, 0),
                session.Board.Get(1, 0),
                session.Board.Get(0, 1),
                session.Board.Get(1, 1),
            };

            session.Shuffle(seed: 11);

            var after = new[]
            {
                session.Board.Get(0, 0),
                session.Board.Get(1, 0),
                session.Board.Get(0, 1),
                session.Board.Get(1, 1),
            };

            Array.Sort(before);
            Array.Sort(after);
            Assert.That(after, Is.EqualTo(before));
        }
    }
}
