using System;

namespace TileRift.Core
{
    public sealed class BoardModel
    {
        private readonly TileType[,] _grid;

        public int Width { get; }
        public int Height { get; }

        public BoardModel(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Board size must be positive.");
            }

            Width = width;
            Height = height;
            _grid = new TileType[height, width];
        }

        public bool InBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        public TileType Get(int x, int y)
        {
            EnsureInBounds(x, y);
            return _grid[y, x];
        }

        public void Set(int x, int y, TileType tile)
        {
            EnsureInBounds(x, y);
            _grid[y, x] = tile;
        }

        public bool IsEmpty(int x, int y) => Get(x, y) == TileType.None;

        public bool CanPlace(int x, int y, TileType tile)
        {
            return tile != TileType.None && InBounds(x, y) && IsEmpty(x, y);
        }

        public bool Place(int x, int y, TileType tile)
        {
            if (!CanPlace(x, y, tile))
            {
                return false;
            }

            Set(x, y, tile);
            return true;
        }

        public bool Move((int x, int y) source, (int x, int y) target)
        {
            if (!InBounds(source.x, source.y) || !InBounds(target.x, target.y))
            {
                return false;
            }

            var tile = Get(source.x, source.y);
            if (tile == TileType.None || !IsEmpty(target.x, target.y))
            {
                return false;
            }

            Set(source.x, source.y, TileType.None);
            Set(target.x, target.y, tile);
            return true;
        }

        public int OccupiedCount()
        {
            var count = 0;
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (_grid[y, x] != TileType.None)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private void EnsureInBounds(int x, int y)
        {
            if (!InBounds(x, y))
            {
                throw new IndexOutOfRangeException($"Out of bounds: ({x},{y})");
            }
        }
    }
}
