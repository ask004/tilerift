namespace TileRift.Monetization
{
    public interface IInterstitialAdsProvider
    {
        bool IsReady();
        bool ShowInterstitial();
    }
}
