using System;

namespace TileRift.Runtime
{
    [Serializable]
    public sealed class PlayerProgress
    {
        public int coin;
        public int streak;
        public string lastLoginUtc;

        public static PlayerProgress Default()
        {
            return new PlayerProgress
            {
                coin = 0,
                streak = 0,
                lastLoginUtc = string.Empty,
            };
        }
    }
}
