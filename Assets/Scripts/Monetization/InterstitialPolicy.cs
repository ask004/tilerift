namespace TileRift.Monetization
{
    public sealed class InterstitialPolicy
    {
        public int EveryNLevels { get; }

        public InterstitialPolicy(int everyNLevels = 3)
        {
            EveryNLevels = everyNLevels;
        }

        public bool ShouldShow(int completedLevelCount)
        {
            return completedLevelCount > 0 && completedLevelCount % EveryNLevels == 0;
        }
    }
}
