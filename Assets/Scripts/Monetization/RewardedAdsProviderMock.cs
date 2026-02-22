namespace TileRift.Monetization
{
    public sealed class RewardedAdsProviderMock : IRewardedAdsProvider
    {
        private readonly AdMobServiceMock _service;
        private readonly bool _ready;

        public RewardedAdsProviderMock(AdMobServiceMock service, bool ready = true)
        {
            _service = service;
            _ready = ready;
        }

        public bool IsReady() => _ready;

        public bool ShowForContinue()
        {
            if (!_ready)
            {
                return false;
            }

            return _service.WatchContinueRewarded();
        }

        public int ShowForCoin(int rewardCoin)
        {
            if (!_ready)
            {
                return 0;
            }

            return _service.WatchCoinRewarded(rewardCoin);
        }
    }
}
