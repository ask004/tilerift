using System;
using System.Collections.Generic;
using NUnit.Framework;
using TileRift.Analytics;
using TileRift.Boosters;
using TileRift.Content;
using TileRift.Core;
using TileRift.Daily;
using TileRift.Economy;
using TileRift.Level;
using TileRift.Monetization;
using TileRift.Polish;
using TileRift.QA;
using TileRift.Store;
using TileRift.UI;

namespace TileRift.Tests.EditMode
{
    public sealed class MvpSystemsTests
    {
        [Test]
        public void LevelLoader_AndFlow_Work()
        {
            var json = "{\"levels\":[{\"levelId\":1,\"width\":2,\"height\":2,\"maxMoves\":5,\"initialRows\":[\"R.\",\".G\"]},{\"levelId\":2,\"width\":2,\"height\":2,\"maxMoves\":5,\"initialRows\":[\"..\",\"..\"]}]}";
            var levels = LevelLoader.LoadMany(json);

            var flow = new LevelFlowController(levels);
            Assert.That(flow.Start().levelId, Is.EqualTo(1));
            Assert.That(flow.Next().levelId, Is.EqualTo(2));
            Assert.That(flow.Restart().levelId, Is.EqualTo(2));
        }

        [Test]
        public void UiAndFeedback_Work()
        {
            var hud = new HudState(2, 10);
            hud.ConsumeMove();
            hud.TogglePause();
            Assert.That(hud.MovesLeft, Is.EqualTo(1));
            Assert.That(hud.IsPaused, Is.True);

            var menu = new MenuStateMachine();
            menu.GoFail();
            Assert.That(menu.Current, Is.EqualTo(MenuScreen.Fail));

            var feedback = new FeedbackBus();
            feedback.OnMove();
            feedback.OnSuccess();
            feedback.OnFail();
            Assert.That(feedback.Events.Count, Is.EqualTo(3));
        }

        [Test]
        public void ContentAndDaily_Work()
        {
            var levels = ContentGenerator.Generate60();
            Assert.That(levels.Count, Is.EqualTo(60));
            Assert.That(levels[0].width, Is.EqualTo(4));
            Assert.That(levels[20].width, Is.EqualTo(5));
            Assert.That(levels[40].width, Is.EqualTo(6));

            var day = new DateTime(2026, 2, 22);
            Assert.That(DailyChallengeService.SeedFor(day), Is.EqualTo(DailyChallengeService.SeedFor(day)));

            var streak = new StreakTracker();
            Assert.That(streak.Login(new DateTime(2026, 2, 20)), Is.EqualTo(1));
            Assert.That(streak.Login(new DateTime(2026, 2, 21)), Is.EqualTo(2));
            Assert.That(streak.Login(new DateTime(2026, 2, 23)), Is.EqualTo(1));
        }

        [Test]
        public void EconomyBoostMonetization_Work()
        {
            var wallet = new CoinWallet();
            wallet.Earn(20);
            Assert.That(wallet.Spend(10), Is.True);
            Assert.That(wallet.Spend(100), Is.False);

            var board = new BoardModel(2, 2);
            board.Place(0, 0, TileType.Red);
            board.Place(1, 1, TileType.Green);
            var boosters = new BoosterService(board);
            boosters.Snapshot();
            board.Move((0, 0), (1, 0));
            Assert.That(boosters.Undo(), Is.True);
            Assert.That(boosters.Hint().HasValue, Is.True);
            boosters.Shuffle();

            var rewarded = new AdMobServiceMock();
            Assert.That(rewarded.WatchContinueRewarded(), Is.True);
            Assert.That(rewarded.WatchCoinRewarded(), Is.EqualTo(20));

            var interstitial = new InterstitialPolicy(3);
            Assert.That(interstitial.ShouldShow(2), Is.False);
            Assert.That(interstitial.ShouldShow(3), Is.True);

            var iap = new IapServiceMock();
            Assert.That(iap.Purchase("no_ads"), Is.True);
            Assert.That(iap.Purchase("coin_pack"), Is.True);
            Assert.That(iap.NoAdsOwned, Is.True);
            Assert.That(iap.CoinBalance, Is.EqualTo(500));
        }

        [Test]
        public void AnalyticsQaAndStore_Work()
        {
            var analytics = new AnalyticsService();
            analytics.Log("level_start", new Dictionary<string, string> { { "level_id", "1" } });
            analytics.Log("level_complete", new Dictionary<string, string> { { "level_id", "1" } });
            analytics.Log("ad_watched");
            analytics.Log("iap_purchase");
            Assert.That(analytics.Events.Count, Is.EqualTo(4));

            var build = new BuildValidator();
            Assert.That(build.CanRelease(), Is.True);

            var bugfix = new BugfixBuffer(1, 2);
            bugfix.CloseP0();
            bugfix.CloseP1(2);
            Assert.That(bugfix.IsClear(), Is.True);

            var assets = new StoreAssetsChecklist { IconReady = true, ScreenshotCount = 5, ShortVideoReady = true };
            Assert.That(assets.IsReady(), Is.True);

            var listing = new StoreListingDraft
            {
                Title = "TileRift",
                ShortDescription = "Quick sort puzzle",
                LongDescription = "Sort colorful tiles in short sessions.",
                Keywords = new List<string> { "tile", "sort", "puzzle", "daily", "casual" }
            };
            Assert.That(listing.IsReady(), Is.True);
        }
    }
}
