using System;
using TileRift.Economy;
using TileRift.Runtime;

namespace TileRift.Daily
{
    public sealed class DailyRewardService
    {
        private readonly StreakTracker _streak;
        private readonly CoinWallet _wallet;

        public DailyRewardService(StreakTracker streak, CoinWallet wallet)
        {
            _streak = streak;
            _wallet = wallet;
        }

        public int LoginAndGrant(DateTime utcNow)
        {
            var streak = _streak.Login(utcNow.Date);
            var reward = 10 + ((streak - 1) * 2);
            _wallet.Earn(reward);
            return reward;
        }

        public static void ApplyToProgress(PlayerProgress progress, DateTime utcNow)
        {
            var today = utcNow.Date;
            var streak = 1;

            if (DateTime.TryParse(progress.lastLoginUtc, out var lastDate))
            {
                var delta = (today - lastDate.Date).Days;
                if (delta == 0)
                {
                    streak = Math.Max(1, progress.streak);
                }
                else if (delta == 1)
                {
                    streak = Math.Max(1, progress.streak) + 1;
                }
            }

            var reward = 10 + ((streak - 1) * 2);
            progress.coin += reward;
            progress.streak = streak;
            progress.lastLoginUtc = utcNow.Date.ToString("yyyy-MM-dd");
        }
    }
}
