namespace TileRift.Economy
{
    public sealed class CoinWallet
    {
        public int Balance { get; private set; }

        public void Earn(int amount)
        {
            if (amount > 0)
            {
                Balance += amount;
            }
        }

        public bool Spend(int amount)
        {
            if (amount < 0 || amount > Balance)
            {
                return false;
            }

            Balance -= amount;
            return true;
        }
    }
}
