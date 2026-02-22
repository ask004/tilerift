using TileRift.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TileRift.Runtime
{
    public sealed class BoardInteractiveView : MonoBehaviour
    {
        [SerializeField] private TapMoveInput input;
        [SerializeField] private GridLayoutGroup grid;
        [SerializeField] private Font font;

        private Button[] _buttons = new Button[0];
        private int _width;
        private int _height;

        public void BindInput(TapMoveInput target)
        {
            input = target;
        }

        public void Render(BoardModel board)
        {
            if (grid == null)
            {
                return;
            }

            EnsureGrid(board.Width, board.Height);

            for (var y = 0; y < board.Height; y++)
            {
                for (var x = 0; x < board.Width; x++)
                {
                    var index = y * board.Width + x;
                    var button = _buttons[index];
                    var label = button.GetComponentInChildren<Text>();
                    label.text = BoardTileGlyph.ToLabel(board.Get(x, y));
                }
            }
        }

        private void EnsureGrid(int width, int height)
        {
            if (_buttons.Length == width * height && _width == width && _height == height)
            {
                return;
            }

            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            _width = width;
            _height = height;
            _buttons = new Button[width * height];
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = width;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = y * width + x;
                    var btn = CreateCellButton($"Cell_{x}_{y}", x, y);
                    _buttons[index] = btn;
                }
            }
        }

        private Button CreateCellButton(string name, int x, int y)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(transform, false);
            var image = go.GetComponent<Image>();
            image.color = new Color(0.12f, 0.12f, 0.12f, 0.85f);

            var textGo = new GameObject("Label", typeof(RectTransform), typeof(Text));
            textGo.transform.SetParent(go.transform, false);
            var txt = textGo.GetComponent<Text>();
            txt.font = font != null ? font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = Color.white;
            txt.text = ".";

            var rect = textGo.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => OnCellClick(x, y));
            return btn;
        }

        private void OnCellClick(int x, int y)
        {
            if (input == null)
            {
                return;
            }

            input.OnCellClicked(x, y);
        }
    }
}
