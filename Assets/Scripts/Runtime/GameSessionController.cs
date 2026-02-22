using System.Collections.Generic;
using TileRift.Level;
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

            _levelFlow = new LevelFlowController(_levels);
            StartGame();
        }

        public void StartGame()
        {
            var level = _levelFlow.Start();
            _session = new GameSession(level, initialCoin);
            RefreshViews();
            menuPresenter?.ShowHome();
        }

        public void RestartLevel()
        {
            var level = _levelFlow.Restart();
            _session = new GameSession(level, initialCoin);
            RefreshViews();
            menuPresenter?.HideAll();
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
            RefreshViews();
            menuPresenter?.HideAll();
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
            if (_session.Menu.Current == MenuScreen.Win)
            {
                menuPresenter?.ShowWin();
            }
            else if (_session.Menu.Current == MenuScreen.Fail)
            {
                menuPresenter?.ShowFail();
            }
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

        private void RefreshViews()
        {
            hudPresenter?.Render(_session.Hud);
            boardDebugView?.Render(_session.Board);
        }
    }
}
