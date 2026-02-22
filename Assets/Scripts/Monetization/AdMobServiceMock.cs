namespace TileRift.Monetization
{
    public sealed class AdMobServiceMock
    {
        public int RewardedWatchCount { get; private set; }

        public bool WatchContinueRewarded()
        {
            RewardedWatchCount++;
            return true;
        }

        public int WatchCoinRewarded(int coinReward = 20)
        {
            RewardedWatchCount++;
            return coinReward;
        }
    }
}
