namespace TileRift.Runtime
{
    public sealed class MoveSelectionState
    {
        private (int x, int y)? _source;

        public void Reset()
        {
            _source = null;
        }

        public (bool ready, (int x, int y) source, (int x, int y) target) Click(int x, int y)
        {
            if (!_source.HasValue)
            {
                _source = (x, y);
                return (false, (0, 0), (0, 0));
            }

            var source = _source.Value;
            _source = null;
            return (true, source, (x, y));
        }
    }
}
