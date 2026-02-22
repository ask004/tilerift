using UnityEngine;

namespace TileRift.Monetization
{
    // Placeholder adapter: replace body with real SDK calls.
    public sealed class UnityAdsAdapterStub : IRewardedAdsProvider, IInterstitialAdsProvider
    {
        private bool _ready;

        public void SetReady(bool ready)
        {
            _ready = ready;
        }

        public bool IsReady() => _ready;

        public bool ShowForContinue()
        {
            if (!_ready)
            {
                return false;
            }

            Debug.Log("Rewarded continue shown (stub).");
            return true;
        }

        public int ShowForCoin(int rewardCoin)
        {
            if (!_ready)
            {
                return 0;
            }

            Debug.Log("Rewarded coin shown (stub).");
            return rewardCoin;
        }

        public bool ShowInterstitial()
        {
            if (!_ready)
            {
                return false;
            }

            Debug.Log("Interstitial shown (stub).");
            return true;
        }
    }
}
