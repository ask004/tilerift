namespace TileRift.Core
{
    public sealed class GameStateManager
    {
        public int TargetMatches { get; }
        public int MaxMoves { get; }

        public int MatchesDone { get; private set; }
        public int MovesUsed { get; private set; }

        public GameStateManager(int targetMatches, int maxMoves)
        {
            TargetMatches = targetMatches;
            MaxMoves = maxMoves;
        }

        public void RecordMove() => MovesUsed++;

        public void RecordMatch() => MatchesDone++;

        public bool IsWin() => MatchesDone >= TargetMatches;

        public bool IsLose() => MovesUsed >= MaxMoves && !IsWin();
    }
}
