namespace TileRift.UI
{
    public enum MenuScreen
    {
        Home,
        Fail,
        Win
    }

    public sealed class MenuStateMachine
    {
        public MenuScreen Current { get; private set; } = MenuScreen.Home;

        public void GoHome() => Current = MenuScreen.Home;
        public void GoFail() => Current = MenuScreen.Fail;
        public void GoWin() => Current = MenuScreen.Win;
    }
}
