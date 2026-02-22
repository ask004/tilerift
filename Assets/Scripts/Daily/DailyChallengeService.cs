using System;
using System.Security.Cryptography;
using System.Text;

namespace TileRift.Daily
{
    public static class DailyChallengeService
    {
        public static int SeedFor(DateTime day)
        {
            var key = day.ToString("yyyy-MM-dd");
            byte[] data;
            using (var sha = SHA256.Create())
            {
                data = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
            }
            return BitConverter.ToInt32(data, 0) & int.MaxValue;
        }
    }
}
