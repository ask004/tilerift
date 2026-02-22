using System.Collections.Generic;
using System;
using TileRift.Daily;
using TileRift.Level;
using TileRift.Monetization;
using TileRift.Analytics;
using TileRift.UI;
using UnityEngine;

namespace TileRift.Runtime
{
    public sealed class GameSessionController : MonoBehaviour
    {
        [SerializeField] private TextAsset levelCatalogJson;
        [SerializeField] private int initialCoin = 50;
        [SerializeField] private HudPresenter hudPresenter;
        [SerializeField] private MenuPresenter menuPresenter;
        [SerializeField] private BoardDebugView boardDebugView;

        private IReadOnlyList<LevelData> _levels;
        private LevelFlowController _levelFlow;
        private GameSession _session;
        private ProgressRepository _progressRepository;
        private PlayerProgress _progress;
        private MonetizationFacade _monetization;
        private int _completedLevelCount;
        private RuntimeAnalyticsBridge _analytics;
        private int _currentLevelId;

        private void Start()
        {
            if (levelCatalogJson == null)
            {
                Debug.LogError("Level catalog json is not assigned.");
                return;
            }

            _levels = LevelLoader.LoadMany(levelCatalogJson.text);
            if (_levels.Count == 0)
            {
                Debug.LogError("No level found in level catalog.");
                return;
            }

            _progressRepository = new ProgressRepository();
            _progress = _progressRepository.Load();
            DailyRewardService.ApplyToProgress(_progress, DateTime.UtcNow);
            _progressRepository.Save(_progress);

            _monetization = new MonetizationFacade(
                new RewardedAdsProviderMock(new AdMobServiceMock(), ready: true),
                new InterstitialAdsProviderMock(ready: true),
                new IapProviderMock(new IapServiceMock()),
                new InterstitialPolicy(3));
            _analytics = new RuntimeAnalyticsBridge(new AnalyticsService());

            _levelFlow = new LevelFlowController(_levels);
            StartGame();
        }

        public void StartGame()
        {
            var level = _levelFlow.Start();
            _session = new GameSession(level, ResolveInitialCoin());
            _currentLevelId = level.levelId;
            RefreshViews();
            menuPresenter?.ShowHome();
            _analytics.LevelStart(level.levelId);
        }

        public void RestartLevel()
        {
            var level = _levelFlow.Restart();
            _session = new GameSession(level, _session.Hud.Coin);
            _currentLevelId = level.levelId;
            RefreshViews();
            menuPresenter?.HideAll();
            _analytics.LevelStart(level.levelId);
        }

        public void NextLevel()
        {
            var next = _levelFlow.Next();
            if (next == null)
            {
                menuPresenter?.ShowWin();
                return;
            }

            _session = new GameSession(next, _session.Hud.Coin);
            _currentLevelId = next.levelId;
            RefreshViews();
            menuPresenter?.HideAll();
            _analytics.LevelStart(next.levelId);
        }

        public void TryMove(int sx, int sy, int tx, int ty)
        {
            if (_session == null)
            {
                return;
            }

            var moved = _session.TryMove((sx, sy), (tx, ty));
            if (!moved)
            {
                return;
            }

            RefreshViews();
            PersistProgress();
            if (_session.Menu.Current == MenuScreen.Win)
            {
                menuPresenter?.ShowWin();
                _completedLevelCount++;
                _analytics.LevelComplete(_currentLevelId);
                _monetization.TryShowInterstitial(_completedLevelCount);
            }
            else if (_session.Menu.Current == MenuScreen.Fail)
            {
                menuPresenter?.ShowFail();
            }
        }

        public bool TryContinueAfterFailWithRewarded()
        {
            if (_session == null || _session.Menu.Current != MenuScreen.Fail)
            {
                return false;
            }

            var ok = _monetization.TryContinueWithRewarded();
            if (!ok)
            {
                return false;
            }

            menuPresenter?.HideAll();
            _analytics.AdWatched("continue");
            return true;
        }

        public bool TryClaimRewardedCoin(int rewardCoin = 20)
        {
            if (_session == null)
            {
                return false;
            }

            var granted = _monetization.TryRewardCoin(rewardCoin);
            if (granted <= 0)
            {
                return false;
            }

            _session.Hud.AddCoin(granted);
            _progress.coin = _session.Hud.Coin;
            _progressRepository.Save(_progress);
            hudPresenter?.Render(_session.Hud);
            _analytics.AdWatched("coin_reward");
            return true;
        }

        public bool TryPurchaseNoAds()
        {
            var ok = _monetization.TryPurchase("no_ads");
            if (ok)
            {
                _analytics.IapPurchase("no_ads");
            }

            return ok;
        }

        public bool TryPurchaseCoinPack()
        {
            var ok = _monetization.TryPurchase("coin_pack");
            if (!ok || _session == null)
            {
                return false;
            }

            _session.Hud.AddCoin(500);
            _progress.coin = _session.Hud.Coin;
            _progressRepository.Save(_progress);
            hudPresenter?.Render(_session.Hud);
            _analytics.IapPurchase("coin_pack");
            return true;
        }

        public void TogglePause()
        {
            if (_session == null)
            {
                return;
            }

            _session.Hud.TogglePause();
            hudPresenter?.Render(_session.Hud);
        }

        public bool TryUseUndoBooster()
        {
            if (_session == null)
            {
                return false;
            }

            var ok = _session.TryUndo();
            if (!ok)
            {
                return false;
            }

            RefreshViews();
            return true;
        }

        public bool TryUseHintBooster(out int sx, out int sy, out int tx, out int ty)
        {
            sx = sy = tx = ty = -1;
            if (_session == null)
            {
                return false;
            }

            var hint = _session.TryHint();
            if (!hint.HasValue)
            {
                return false;
            }

            sx = hint.Value.source.x;
            sy = hint.Value.source.y;
            tx = hint.Value.target.x;
            ty = hint.Value.target.y;
            return true;
        }

        public bool TryUseShuffleBooster(int seed = 17)
        {
            if (_session == null)
            {
                return false;
            }

            _session.Shuffle(seed);
            RefreshViews();
            return true;
        }

        public void UseUndoBoosterButton()
        {
            TryUseUndoBooster();
        }

        public void UseHintBoosterButton()
        {
            if (TryUseHintBooster(out var sx, out var sy, out var tx, out var ty))
            {
                Debug.Log($"Hint: ({sx},{sy}) -> ({tx},{ty})");
            }
        }

        public void UseShuffleBoosterButton()
        {
            TryUseShuffleBooster();
        }

        private void RefreshViews()
        {
            hudPresenter?.Render(_session.Hud);
            boardDebugView?.Render(_session.Board);
        }

        private int ResolveInitialCoin()
        {
            if (_progress == null)
            {
                return initialCoin;
            }

            return Math.Max(initialCoin, _progress.coin);
        }

        private void PersistProgress()
        {
            if (_progressRepository == null || _progress == null || _session == null)
            {
                return;
            }

            _progress.coin = _session.Hud.Coin;
            _progressRepository.Save(_progress);
        }
    }
}
