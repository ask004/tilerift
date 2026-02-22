namespace TileRift.QA
{
    public sealed class BugfixBuffer
    {
        public int P0Open { get; private set; }
        public int P1Open { get; private set; }

        public BugfixBuffer(int p0Open, int p1Open)
        {
            P0Open = p0Open;
            P1Open = p1Open;
        }

        public void CloseP0(int count = 1)
        {
            P0Open = P0Open - count;
            if (P0Open < 0)
            {
                P0Open = 0;
            }
        }

        public void CloseP1(int count = 1)
        {
            P1Open = P1Open - count;
            if (P1Open < 0)
            {
                P1Open = 0;
            }
        }

        public bool IsClear() => P0Open == 0 && P1Open == 0;
    }
}
