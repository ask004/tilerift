namespace TileRift.UI
{
    public sealed class HudState
    {
        public int MovesLeft { get; private set; }
        public int Coin { get; private set; }
        public bool IsPaused { get; private set; }

        public HudState(int movesLeft, int coin)
        {
            MovesLeft = movesLeft;
            Coin = coin;
        }

        public void ConsumeMove()
        {
            if (MovesLeft > 0)
            {
                MovesLeft--;
            }
        }

        public void AddCoin(int amount)
        {
            if (amount > 0)
            {
                Coin += amount;
            }
        }

        public void TogglePause() => IsPaused = !IsPaused;
    }
}
