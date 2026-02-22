using System.Collections.Generic;
using TileRift.Analytics;

namespace TileRift.Runtime
{
    public sealed class RuntimeAnalyticsBridge
    {
        private readonly AnalyticsService _analytics;

        public RuntimeAnalyticsBridge(AnalyticsService analytics)
        {
            _analytics = analytics;
        }

        public void LevelStart(int levelId)
        {
            _analytics.Log("level_start", new Dictionary<string, string>
            {
                { "level_id", levelId.ToString() }
            });
        }

        public void LevelComplete(int levelId)
        {
            _analytics.Log("level_complete", new Dictionary<string, string>
            {
                { "level_id", levelId.ToString() }
            });
        }

        public void AdWatched(string placement)
        {
            _analytics.Log("ad_watched", new Dictionary<string, string>
            {
                { "placement", placement }
            });
        }

        public void IapPurchase(string productId)
        {
            _analytics.Log("iap_purchase", new Dictionary<string, string>
            {
                { "product_id", productId }
            });
        }
    }
}
