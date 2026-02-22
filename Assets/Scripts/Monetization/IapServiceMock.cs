namespace TileRift.Monetization
{
    public sealed class IapServiceMock
    {
        public bool NoAdsOwned { get; private set; }
        public int CoinBalance { get; private set; }

        public bool Purchase(string productId)
        {
            if (productId == "no_ads")
            {
                NoAdsOwned = true;
                return true;
            }

            if (productId == "coin_pack")
            {
                CoinBalance += 500;
                return true;
            }

            return false;
        }
    }
}
