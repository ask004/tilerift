using NUnit.Framework;
using TileRift.QA;

namespace TileRift.Tests.EditMode
{
    public sealed class ReleaseRehearsalTests
    {
        [Test]
        public void Compose_ReadyToShip_WhenAllChecksPass()
        {
            var summary = ReleaseRehearsal.Compose(true, true, true, true);
            Assert.That(summary.readyToShip, Is.True);
        }

        [Test]
        public void Compose_NotReady_WhenAnyCheckFails()
        {
            var summary = ReleaseRehearsal.Compose(true, true, false, true);
            Assert.That(summary.readyToShip, Is.False);
        }
    }
}
