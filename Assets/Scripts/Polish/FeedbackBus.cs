using System.Collections.Generic;

namespace TileRift.Polish
{
    public sealed class FeedbackBus
    {
        public readonly List<string> Events = new();

        public void OnMove() => Events.Add("move");
        public void OnSuccess() => Events.Add("success");
        public void OnFail() => Events.Add("fail");
    }
}
