using TileRift.UI;
using UnityEngine;

namespace TileRift.Runtime
{
    public sealed class SceneBootstrap : MonoBehaviour
    {
        [SerializeField] private GameSessionController controller;
        [SerializeField] private TapMoveInput input;
        [SerializeField] private HudPresenter hud;
        [SerializeField] private MenuPresenter menu;
        [SerializeField] private BoardDebugView board;

        private void Awake()
        {
            if (controller == null)
            {
                controller = FindFirstObjectByType<GameSessionController>();
            }

            if (input == null)
            {
                input = FindFirstObjectByType<TapMoveInput>();
            }

            if (hud == null)
            {
                hud = FindFirstObjectByType<HudPresenter>();
            }

            if (menu == null)
            {
                menu = FindFirstObjectByType<MenuPresenter>();
            }

            if (board == null)
            {
                board = FindFirstObjectByType<BoardDebugView>();
            }

            if (controller == null)
            {
                Debug.LogError("GameSessionController is missing in scene.");
                return;
            }

            if (input != null)
            {
                input.Bind(controller);
            }

            // Ensure presenter references can be assigned in one place if needed.
            _ = hud;
            _ = menu;
            _ = board;
        }
    }
}
