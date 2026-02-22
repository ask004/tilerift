using TileRift.Boosters;
using TileRift.Core;
using TileRift.Economy;
using TileRift.Level;
using TileRift.UI;

namespace TileRift.Runtime
{
    public sealed class GameSession
    {
        private readonly LevelData _level;

        public BoardModel Board { get; }
        public DragDropSystem DragDrop { get; }
        public RuleEngine Rules { get; }
        public GameStateManager State { get; }
        public CoinWallet Wallet { get; }
        public HudState Hud { get; }
        public MenuStateMachine Menu { get; }
        public BoosterService Boosters { get; }

        public GameSession(LevelData level, int initialCoin)
        {
            _level = level;
            Board = new BoardModel(level.width, level.height);
            DragDrop = new DragDropSystem(Board);
            Rules = new RuleEngine(Board);
            State = new GameStateManager(targetMatches: 1, maxMoves: level.maxMoves);
            Wallet = new CoinWallet();
            Wallet.Earn(initialCoin);
            Hud = new HudState(level.maxMoves, Wallet.Balance);
            Menu = new MenuStateMachine();
            LoadInitialBoard();
            Boosters = new BoosterService(Board);
        }

        public bool TryMove((int x, int y) source, (int x, int y) target)
        {
            if (!Rules.IsValidMove(source, target))
            {
                return false;
            }

            Boosters.Snapshot();
            var moved = Board.Move(source, target);
            if (!moved)
            {
                return false;
            }

            State.RecordMove();
            Hud.ConsumeMove();

            if (Rules.HasMatch())
            {
                State.RecordMatch();
            }

            if (State.IsWin())
            {
                Menu.GoWin();
                Wallet.Earn(10);
                Hud.AddCoin(10);
            }
            else if (State.IsLose())
            {
                Menu.GoFail();
            }

            return true;
        }

        public bool TryUndo()
        {
            return Boosters.Undo();
        }

        public ((int x, int y) source, (int x, int y) target)? TryHint()
        {
            return Boosters.Hint();
        }

        public void Shuffle(int seed = 17)
        {
            Boosters.Snapshot();
            Boosters.Shuffle(seed);
        }

        private void LoadInitialBoard()
        {
            for (var y = 0; y < _level.height; y++)
            {
                for (var x = 0; x < _level.width; x++)
                {
                    var tile = ParseTile(_level.initialRows[y][x]);
                    Board.Set(x, y, tile);
                }
            }
        }

        private static TileType ParseTile(char symbol)
        {
            return symbol switch
            {
                'R' => TileType.Red,
                'G' => TileType.Green,
                'B' => TileType.Blue,
                'Y' => TileType.Yellow,
                _ => TileType.None,
            };
        }
    }
}
