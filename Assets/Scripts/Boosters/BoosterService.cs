using System;
using System.Collections.Generic;
using TileRift.Core;

namespace TileRift.Boosters
{
    public sealed class BoosterService
    {
        private readonly BoardModel _board;
        private readonly Stack<TileType[,]> _history = new();

        public BoosterService(BoardModel board)
        {
            _board = board;
        }

        public void Snapshot()
        {
            var snapshot = new TileType[_board.Height, _board.Width];
            for (var y = 0; y < _board.Height; y++)
            {
                for (var x = 0; x < _board.Width; x++)
                {
                    snapshot[y, x] = _board.Get(x, y);
                }
            }

            _history.Push(snapshot);
        }

        public bool Undo()
        {
            if (_history.Count == 0)
            {
                return false;
            }

            var snapshot = _history.Pop();
            for (var y = 0; y < _board.Height; y++)
            {
                for (var x = 0; x < _board.Width; x++)
                {
                    _board.Set(x, y, snapshot[y, x]);
                }
            }

            return true;
        }

        public ((int x, int y) source, (int x, int y) target)? Hint()
        {
            for (var y = 0; y < _board.Height; y++)
            {
                for (var x = 0; x < _board.Width; x++)
                {
                    if (_board.Get(x, y) == TileType.None)
                    {
                        continue;
                    }

                    for (var ty = 0; ty < _board.Height; ty++)
                    {
                        for (var tx = 0; tx < _board.Width; tx++)
                        {
                            if (_board.Get(tx, ty) == TileType.None)
                            {
                                return ((x, y), (tx, ty));
                            }
                        }
                    }
                }
            }

            return null;
        }

        public void Shuffle(int seed = 17)
        {
            var pieces = new List<TileType>();
            for (var y = 0; y < _board.Height; y++)
            {
                for (var x = 0; x < _board.Width; x++)
                {
                    var tile = _board.Get(x, y);
                    if (tile != TileType.None)
                    {
                        pieces.Add(tile);
                    }
                }
            }

            var rng = new Random(seed);
            for (var i = pieces.Count - 1; i > 0; i--)
            {
                var j = rng.Next(0, i + 1);
                (pieces[i], pieces[j]) = (pieces[j], pieces[i]);
            }

            var index = 0;
            for (var y = 0; y < _board.Height; y++)
            {
                for (var x = 0; x < _board.Width; x++)
                {
                    if (_board.Get(x, y) != TileType.None)
                    {
                        _board.Set(x, y, pieces[index++]);
                    }
                }
            }
        }
    }
}
