using NUnit.Framework;
using TileRift.Runtime;

namespace TileRift.Tests.EditMode
{
    public sealed class MoveSelectionStateTests
    {
        [Test]
        public void FirstClick_OnlySelectsSource()
        {
            var state = new MoveSelectionState();
            var result = state.Click(1, 2);

            Assert.That(result.ready, Is.False);
        }

        [Test]
        public void SecondClick_ProducesMovePair()
        {
            var state = new MoveSelectionState();
            state.Click(1, 2);
            var result = state.Click(2, 2);

            Assert.That(result.ready, Is.True);
            Assert.That(result.source.x, Is.EqualTo(1));
            Assert.That(result.source.y, Is.EqualTo(2));
            Assert.That(result.target.x, Is.EqualTo(2));
            Assert.That(result.target.y, Is.EqualTo(2));
        }

        [Test]
        public void Reset_ClearsPendingSelection()
        {
            var state = new MoveSelectionState();
            state.Click(0, 0);
            state.Reset();
            var result = state.Click(2, 1);

            Assert.That(result.ready, Is.False);
        }
    }
}
