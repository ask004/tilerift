using NUnit.Framework;
using TileRift.QA;

namespace TileRift.Tests.EditMode
{
    public sealed class QaSystemsTests
    {
        [Test]
        public void ReadinessEvaluator_ReturnsReleaseReady_WhenAllChecksPass()
        {
            var input = new BuildReadinessInput
            {
                hasMainScene = true,
                targetAndroid = true,
                il2CppEnabled = true,
                minSdkConfigured = true,
                bundleIdentifierSet = true,
                criticalCrashCount = 0,
            };

            var report = BuildReadinessEvaluator.Evaluate(input);
            Assert.That(report.canRelease, Is.True);
            Assert.That(report.failed.Count, Is.EqualTo(0));
            Assert.That(report.passed.Count, Is.EqualTo(6));
        }

        [Test]
        public void ReadinessEvaluator_Fails_WhenCriticalCrashExists()
        {
            var input = new BuildReadinessInput
            {
                hasMainScene = true,
                targetAndroid = true,
                il2CppEnabled = true,
                minSdkConfigured = true,
                bundleIdentifierSet = true,
                criticalCrashCount = 1,
            };

            var report = BuildReadinessEvaluator.Evaluate(input);
            Assert.That(report.canRelease, Is.False);
            Assert.That(report.failed.Count, Is.EqualTo(1));
        }

        [Test]
        public void BugTriageBoard_ReleaseClearLogic_Works()
        {
            var board = new BugTriageBoard();
            board.Add("P0-1", "Critical crash", BugSeverity.P0);
            board.Add("P1-1", "Flow blocker", BugSeverity.P1);
            board.Add("P2-1", "Minor UI", BugSeverity.P2);

            Assert.That(board.IsReleaseClear(), Is.False);
            Assert.That(board.Close("P0-1"), Is.True);
            Assert.That(board.IsReleaseClear(), Is.False);
            Assert.That(board.Close("P1-1"), Is.True);
            Assert.That(board.IsReleaseClear(), Is.True);
        }
    }
}
