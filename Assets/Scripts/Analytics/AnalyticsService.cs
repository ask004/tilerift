using System.Collections.Generic;

namespace TileRift.Analytics
{
    public sealed class AnalyticsService
    {
        public readonly List<(string eventName, Dictionary<string, string> payload)> Events = new();

        public void Log(string eventName, Dictionary<string, string> payload = null)
        {
            Events.Add((eventName, payload ?? new Dictionary<string, string>()));
        }
    }
}
