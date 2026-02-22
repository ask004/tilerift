namespace TileRift.Monetization
{
    public sealed class InterstitialAdsProviderMock : IInterstitialAdsProvider
    {
        private readonly bool _ready;
        public int ShowCount { get; private set; }

        public InterstitialAdsProviderMock(bool ready = true)
        {
            _ready = ready;
        }

        public bool IsReady() => _ready;

        public bool ShowInterstitial()
        {
            if (!_ready)
            {
                return false;
            }

            ShowCount++;
            return true;
        }
    }
}
