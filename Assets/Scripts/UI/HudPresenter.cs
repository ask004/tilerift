using UnityEngine;
using UnityEngine.UI;

namespace TileRift.UI
{
    public sealed class HudPresenter : MonoBehaviour
    {
        [SerializeField] private Text movesText;
        [SerializeField] private Text coinText;
        [SerializeField] private GameObject pauseIndicator;

        public void Render(HudState state)
        {
            if (movesText != null)
            {
                movesText.text = $"Moves: {state.MovesLeft}";
            }

            if (coinText != null)
            {
                coinText.text = $"Coin: {state.Coin}";
            }

            if (pauseIndicator != null)
            {
                pauseIndicator.SetActive(state.IsPaused);
            }
        }
    }
}
