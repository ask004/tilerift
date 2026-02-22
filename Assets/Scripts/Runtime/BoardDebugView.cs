using System.Text;
using TileRift.Core;
using UnityEngine;
using UnityEngine.UI;

namespace TileRift.Runtime
{
    public sealed class BoardDebugView : MonoBehaviour
    {
        [SerializeField] private Text boardText;

        public void Render(BoardModel board)
        {
            if (boardText == null)
            {
                return;
            }

            var sb = new StringBuilder();
            for (var y = 0; y < board.Height; y++)
            {
                for (var x = 0; x < board.Width; x++)
                {
                    sb.Append(TileToChar(board.Get(x, y)));
                }

                if (y < board.Height - 1)
                {
                    sb.AppendLine();
                }
            }

            boardText.text = sb.ToString();
        }

        private static char TileToChar(TileType tile)
        {
            return tile switch
            {
                TileType.Red => 'R',
                TileType.Green => 'G',
                TileType.Blue => 'B',
                TileType.Yellow => 'Y',
                _ => '.',
            };
        }
    }
}
