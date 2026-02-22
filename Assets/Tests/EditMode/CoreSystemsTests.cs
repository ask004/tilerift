using NUnit.Framework;
using TileRift.Core;

namespace TileRift.Tests.EditMode
{
    public sealed class CoreSystemsTests
    {
        [Test]
        public void BoardModel_PlaceAndMove_Works()
        {
            var board = new BoardModel(3, 3);

            Assert.That(board.Place(0, 0, TileType.Red), Is.True);
            Assert.That(board.Place(0, 0, TileType.Green), Is.False);
            Assert.That(board.Move((0, 0), (1, 0)), Is.True);
            Assert.That(board.Get(1, 0), Is.EqualTo(TileType.Red));
            Assert.That(board.OccupiedCount(), Is.EqualTo(1));
        }

        [Test]
        public void DragDrop_SelectAndDrop_Works()
        {
            var board = new BoardModel(2, 1);
            board.Place(0, 0, TileType.Blue);
            var dnd = new DragDropSystem(board);

            Assert.That(dnd.Select(0, 0), Is.True);
            Assert.That(dnd.Drop(1, 0), Is.True);
            Assert.That(board.Get(1, 0), Is.EqualTo(TileType.Blue));
        }

        [Test]
        public void RuleEngine_MatchAndValidations_Work()
        {
            var board = new BoardModel(3, 1);
            board.Set(0, 0, TileType.Red);
            board.Set(1, 0, TileType.Red);
            board.Set(2, 0, TileType.Red);

            var rules = new RuleEngine(board);
            Assert.That(rules.HasMatch(), Is.True);
            Assert.That(rules.IsValidMove((1, 0), (2, 0)), Is.False);
        }

        [Test]
        public void GameState_WinLoseLogic_Works()
        {
            var state = new GameStateManager(targetMatches: 1, maxMoves: 2);
            state.RecordMove();
            state.RecordMatch();

            Assert.That(state.IsWin(), Is.True);
            Assert.That(state.IsLose(), Is.False);
        }
    }
}
