using NUnit.Framework;
using TileRift.Analytics;
using TileRift.Runtime;

namespace TileRift.Tests.EditMode
{
    public sealed class RuntimeAnalyticsBridgeTests
    {
        [Test]
        public void Bridge_LogsExpectedEvents()
        {
            var analytics = new AnalyticsService();
            var bridge = new RuntimeAnalyticsBridge(analytics);

            bridge.LevelStart(1);
            bridge.LevelComplete(1);
            bridge.AdWatched("continue");
            bridge.IapPurchase("coin_pack");

            Assert.That(analytics.Events.Count, Is.EqualTo(4));
            Assert.That(analytics.Events[0].eventName, Is.EqualTo("level_start"));
            Assert.That(analytics.Events[1].eventName, Is.EqualTo("level_complete"));
            Assert.That(analytics.Events[2].eventName, Is.EqualTo("ad_watched"));
            Assert.That(analytics.Events[3].eventName, Is.EqualTo("iap_purchase"));
        }
    }
}
