namespace TileRift.Monetization
{
    public sealed class IapProviderMock : IIapProvider
    {
        private readonly IapServiceMock _service;

        public bool IsInitialized => true;

        public IapProviderMock(IapServiceMock service)
        {
            _service = service;
        }

        public bool Purchase(string productId)
        {
            return _service.Purchase(productId);
        }
    }
}
