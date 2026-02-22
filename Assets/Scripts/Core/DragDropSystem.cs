namespace TileRift.Core
{
    public sealed class DragDropSystem
    {
        private readonly BoardModel _board;

        public (int x, int y)? SelectedCell { get; private set; }

        public DragDropSystem(BoardModel board)
        {
            _board = board;
        }

        public bool Select(int x, int y)
        {
            if (!_board.InBounds(x, y) || _board.Get(x, y) == TileType.None)
            {
                return false;
            }

            SelectedCell = (x, y);
            return true;
        }

        public bool Drop(int x, int y)
        {
            if (!SelectedCell.HasValue)
            {
                return false;
            }

            var moved = _board.Move(SelectedCell.Value, (x, y));
            SelectedCell = null;
            return moved;
        }
    }
}
