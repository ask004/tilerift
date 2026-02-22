using System.Collections.Generic;

namespace TileRift.Store
{
    public sealed class StoreListingDraft
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public List<string> Keywords { get; set; } = new();

        public bool IsReady()
        {
            return !string.IsNullOrWhiteSpace(Title)
                && !string.IsNullOrWhiteSpace(ShortDescription)
                && !string.IsNullOrWhiteSpace(LongDescription)
                && Keywords.Count >= 5;
        }
    }
}
