using NUnit.Framework;
using TileRift.Monetization;

namespace TileRift.Tests.EditMode
{
    public sealed class MonetizationFacadeTests
    {
        [Test]
        public void RewardedContinue_ReturnsTrue_WhenProviderReady()
        {
            var facade = new MonetizationFacade(
                new RewardedAdsProviderMock(new AdMobServiceMock(), ready: true),
                new InterstitialAdsProviderMock(ready: true),
                new IapProviderMock(new IapServiceMock()),
                new InterstitialPolicy(3));

            Assert.That(facade.TryContinueWithRewarded(), Is.True);
        }

        [Test]
        public void RewardedCoin_ReturnsZero_WhenProviderNotReady()
        {
            var facade = new MonetizationFacade(
                new RewardedAdsProviderMock(new AdMobServiceMock(), ready: false),
                new InterstitialAdsProviderMock(ready: true),
                new IapProviderMock(new IapServiceMock()),
                new InterstitialPolicy(3));

            Assert.That(facade.TryRewardCoin(20), Is.EqualTo(0));
        }

        [Test]
        public void Interstitial_FollowsPolicy()
        {
            var interstitial = new InterstitialAdsProviderMock(ready: true);
            var facade = new MonetizationFacade(
                new RewardedAdsProviderMock(new AdMobServiceMock(), ready: true),
                interstitial,
                new IapProviderMock(new IapServiceMock()),
                new InterstitialPolicy(3));

            Assert.That(facade.TryShowInterstitial(2), Is.False);
            Assert.That(facade.TryShowInterstitial(3), Is.True);
            Assert.That(interstitial.ShowCount, Is.EqualTo(1));
        }

        [Test]
        public void IapPurchase_WorksForKnownProducts()
        {
            var iapService = new IapServiceMock();
            var facade = new MonetizationFacade(
                new RewardedAdsProviderMock(new AdMobServiceMock(), ready: true),
                new InterstitialAdsProviderMock(ready: true),
                new IapProviderMock(iapService),
                new InterstitialPolicy(3));

            Assert.That(facade.TryPurchase("no_ads"), Is.True);
            Assert.That(facade.TryPurchase("coin_pack"), Is.True);
            Assert.That(iapService.NoAdsOwned, Is.True);
            Assert.That(iapService.CoinBalance, Is.EqualTo(500));
        }
    }
}
