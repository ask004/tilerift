using System.Collections.Generic;

namespace TileRift.Level
{
    public sealed class LevelFlowController
    {
        private readonly IReadOnlyList<LevelData> _levels;
        private int _currentIndex;

        public LevelFlowController(IReadOnlyList<LevelData> levels)
        {
            _levels = levels;
            _currentIndex = 0;
        }

        public LevelData Start()
        {
            _currentIndex = 0;
            return _levels[_currentIndex];
        }

        public LevelData Restart() => _levels[_currentIndex];

        public LevelData Next()
        {
            if (_currentIndex + 1 >= _levels.Count)
            {
                return null;
            }

            _currentIndex++;
            return _levels[_currentIndex];
        }
    }
}
