using System;

namespace TileRift.QA
{
    [Serializable]
    public sealed class ReleaseRehearsalSummary
    {
        public bool androidConfigured;
        public bool readinessGreen;
        public bool balanceHealthy;
        public bool buildDryRunPassed;
        public bool readyToShip;
    }

    public static class ReleaseRehearsal
    {
        public static ReleaseRehearsalSummary Compose(
            bool androidConfigured,
            bool readinessGreen,
            bool balanceHealthy,
            bool buildDryRunPassed)
        {
            return new ReleaseRehearsalSummary
            {
                androidConfigured = androidConfigured,
                readinessGreen = readinessGreen,
                balanceHealthy = balanceHealthy,
                buildDryRunPassed = buildDryRunPassed,
                readyToShip = androidConfigured && readinessGreen && balanceHealthy && buildDryRunPassed,
            };
        }
    }
}
