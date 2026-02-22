namespace TileRift.Core
{
    public sealed class RuleEngine
    {
        private readonly BoardModel _board;

        public RuleEngine(BoardModel board)
        {
            _board = board;
        }

        public bool IsValidMove((int x, int y) source, (int x, int y) target)
        {
            if (!_board.InBounds(source.x, source.y) || !_board.InBounds(target.x, target.y))
            {
                return false;
            }

            if (_board.Get(source.x, source.y) == TileType.None)
            {
                return false;
            }

            return _board.Get(target.x, target.y) == TileType.None;
        }

        public bool HasMatch(int minLength = 3)
        {
            for (var y = 0; y < _board.Height; y++)
            {
                var run = 0;
                var prev = TileType.None;
                for (var x = 0; x < _board.Width; x++)
                {
                    var current = _board.Get(x, y);
                    if (current != TileType.None && current == prev)
                    {
                        run++;
                    }
                    else
                    {
                        prev = current;
                        run = current == TileType.None ? 0 : 1;
                    }

                    if (run >= minLength)
                    {
                        return true;
                    }
                }
            }

            for (var x = 0; x < _board.Width; x++)
            {
                var run = 0;
                var prev = TileType.None;
                for (var y = 0; y < _board.Height; y++)
                {
                    var current = _board.Get(x, y);
                    if (current != TileType.None && current == prev)
                    {
                        run++;
                    }
                    else
                    {
                        prev = current;
                        run = current == TileType.None ? 0 : 1;
                    }

                    if (run >= minLength)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
