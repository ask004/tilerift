namespace TileRift.Store
{
    public sealed class StoreAssetsChecklist
    {
        public bool IconReady { get; set; }
        public int ScreenshotCount { get; set; }
        public bool ShortVideoReady { get; set; }

        public bool IsReady()
        {
            return IconReady && ScreenshotCount >= 5 && ShortVideoReady;
        }
    }
}
