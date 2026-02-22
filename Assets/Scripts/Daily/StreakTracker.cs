using System;

namespace TileRift.Daily
{
    public sealed class StreakTracker
    {
        public DateTime? LastLoginUtcDate { get; private set; }
        public int Streak { get; private set; }

        public int Login(DateTime todayUtcDate)
        {
            todayUtcDate = todayUtcDate.Date;

            if (LastLoginUtcDate == null)
            {
                Streak = 1;
            }
            else
            {
                var delta = (todayUtcDate - LastLoginUtcDate.Value).Days;
                if (delta == 1)
                {
                    Streak++;
                }
                else if (delta > 1)
                {
                    Streak = 1;
                }
            }

            LastLoginUtcDate = todayUtcDate;
            return Streak;
        }
    }
}
