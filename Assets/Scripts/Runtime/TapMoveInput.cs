using UnityEngine;

namespace TileRift.Runtime
{
    public sealed class TapMoveInput : MonoBehaviour
    {
        [SerializeField] private GameSessionController controller;
        private readonly MoveSelectionState _selection = new();

        public void Bind(GameSessionController sessionController)
        {
            controller = sessionController;
            _selection.Reset();
        }

        // Hook this from tile buttons or cell proxies in the scene.
        public void OnCellClicked(int x, int y)
        {
            if (controller == null)
            {
                return;
            }

            var result = _selection.Click(x, y);
            if (!result.ready)
            {
                return;
            }

            controller.TryMove(result.source.x, result.source.y, result.target.x, result.target.y);
        }

        public void ClearSelection()
        {
            _selection.Reset();
        }
    }
}
