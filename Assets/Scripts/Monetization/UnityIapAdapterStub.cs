using UnityEngine;

namespace TileRift.Monetization
{
    // Placeholder adapter: replace body with real Unity IAP callbacks.
    public sealed class UnityIapAdapterStub : IIapProvider
    {
        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public bool Purchase(string productId)
        {
            if (!IsInitialized)
            {
                return false;
            }

            Debug.Log($"Purchase requested (stub): {productId}");
            return productId == "no_ads" || productId == "coin_pack";
        }
    }
}
