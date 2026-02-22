namespace TileRift.Monetization
{
    public interface IRewardedAdsProvider
    {
        bool IsReady();
        bool ShowForContinue();
        int ShowForCoin(int rewardCoin);
    }
}
