namespace TileRift.Monetization
{
    public interface IIapProvider
    {
        bool IsInitialized { get; }
        bool Purchase(string productId);
    }
}
