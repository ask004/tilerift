namespace TileRift.Monetization
{
    public sealed class MonetizationFacade
    {
        private readonly IRewardedAdsProvider _rewarded;
        private readonly IInterstitialAdsProvider _interstitial;
        private readonly IIapProvider _iap;
        private readonly InterstitialPolicy _policy;

        public MonetizationFacade(
            IRewardedAdsProvider rewarded,
            IInterstitialAdsProvider interstitial,
            IIapProvider iap,
            InterstitialPolicy policy)
        {
            _rewarded = rewarded;
            _interstitial = interstitial;
            _iap = iap;
            _policy = policy;
        }

        public bool TryContinueWithRewarded()
        {
            if (!_rewarded.IsReady())
            {
                return false;
            }

            return _rewarded.ShowForContinue();
        }

        public int TryRewardCoin(int rewardCoin)
        {
            if (!_rewarded.IsReady())
            {
                return 0;
            }

            return _rewarded.ShowForCoin(rewardCoin);
        }

        public bool TryShowInterstitial(int completedLevelCount)
        {
            if (!_policy.ShouldShow(completedLevelCount) || !_interstitial.IsReady())
            {
                return false;
            }

            return _interstitial.ShowInterstitial();
        }

        public bool TryPurchase(string productId)
        {
            if (!_iap.IsInitialized)
            {
                return false;
            }

            return _iap.Purchase(productId);
        }
    }
}
