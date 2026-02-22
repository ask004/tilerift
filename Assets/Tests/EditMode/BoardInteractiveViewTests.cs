using System.Reflection;
using NUnit.Framework;
using TileRift.Core;
using TileRift.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace TileRift.Tests.EditMode
{
    public sealed class BoardInteractiveViewTests
    {
        private static readonly FieldInfo GridField =
            typeof(BoardInteractiveView).GetField("grid", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo SelectionField =
            typeof(TapMoveInput).GetField("_selection", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly FieldInfo SourceField =
            typeof(MoveSelectionState).GetField("_source", BindingFlags.Instance | BindingFlags.NonPublic);

        [Test]
        public void Render_CreatesExpectedCellsAndLabels()
        {
            var root = new GameObject("BoardRoot", typeof(RectTransform), typeof(GridLayoutGroup));
            try
            {
                var view = root.AddComponent<BoardInteractiveView>();
                GridField.SetValue(view, root.GetComponent<GridLayoutGroup>());

                var board = new BoardModel(2, 2);
                board.Place(0, 0, TileType.Red);
                board.Place(1, 0, TileType.Green);
                board.Place(0, 1, TileType.Blue);
                board.Place(1, 1, TileType.Yellow);

                view.Render(board);

                Assert.That(root.transform.childCount, Is.EqualTo(4));
                Assert.That(LabelOf(root.transform, 0), Is.EqualTo("R"));
                Assert.That(LabelOf(root.transform, 1), Is.EqualTo("G"));
                Assert.That(LabelOf(root.transform, 2), Is.EqualTo("B"));
                Assert.That(LabelOf(root.transform, 3), Is.EqualTo("Y"));
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void CellClick_ForwardsIntoTapSelectionState()
        {
            var root = new GameObject("BoardRoot", typeof(RectTransform), typeof(GridLayoutGroup));
            var inputGo = new GameObject("Input");
            try
            {
                var input = inputGo.AddComponent<TapMoveInput>();
                var view = root.AddComponent<BoardInteractiveView>();
                GridField.SetValue(view, root.GetComponent<GridLayoutGroup>());
                view.BindInput(input);

                var board = new BoardModel(2, 1);
                board.Place(0, 0, TileType.Red);
                board.Place(1, 0, TileType.Green);
                view.Render(board);

                root.transform.GetChild(0).GetComponent<Button>().onClick.Invoke();
                Assert.That(SelectedSource(input).HasValue, Is.True);

                root.transform.GetChild(1).GetComponent<Button>().onClick.Invoke();
                Assert.That(SelectedSource(input).HasValue, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(root);
                Object.DestroyImmediate(inputGo);
            }
        }

        private static string LabelOf(Transform parent, int index)
        {
            return parent.GetChild(index).GetComponentInChildren<Text>().text;
        }

        private static (int x, int y)? SelectedSource(TapMoveInput input)
        {
            var selection = SelectionField.GetValue(input);
            return ((int x, int y)?)SourceField.GetValue(selection);
        }
    }
}
