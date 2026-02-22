using UnityEngine;
using UnityEngine.UI;

namespace TileRift.Runtime
{
    public sealed class DebugActionPanel : MonoBehaviour
    {
        [SerializeField] private GameSessionController controller;
        [SerializeField] private Text outputText;

        public void Bind(GameSessionController target)
        {
            controller = target;
        }

        public void StartGame()
        {
            if (controller == null) return;
            controller.StartGame();
            Write("StartGame");
        }

        public void StartDaily()
        {
            if (controller == null) return;
            controller.StartDailyChallenge();
            Write("StartDaily");
        }

        public void Restart()
        {
            if (controller == null) return;
            controller.RestartLevel();
            Write("RestartLevel");
        }

        public void Next()
        {
            if (controller == null) return;
            controller.NextLevel();
            Write("NextLevel");
        }

        public void ContinueRewarded()
        {
            if (controller == null) return;
            var ok = controller.TryContinueAfterFailWithRewarded();
            Write(ok ? "ContinueRewarded:OK" : "ContinueRewarded:FAIL");
        }

        public void RewardCoin()
        {
            if (controller == null) return;
            var ok = controller.TryClaimRewardedCoin();
            Write(ok ? "RewardCoin:OK" : "RewardCoin:FAIL");
        }

        public void BuyNoAds()
        {
            if (controller == null) return;
            var ok = controller.TryPurchaseNoAds();
            Write(ok ? "BuyNoAds:OK" : "BuyNoAds:FAIL");
        }

        public void BuyCoinPack()
        {
            if (controller == null) return;
            var ok = controller.TryPurchaseCoinPack();
            Write(ok ? "BuyCoinPack:OK" : "BuyCoinPack:FAIL");
        }

        public void Hint()
        {
            if (controller == null) return;
            if (controller.TryUseHintBooster(out var sx, out var sy, out var tx, out var ty))
            {
                Write($"Hint: ({sx},{sy})->({tx},{ty})");
            }
            else
            {
                Write("Hint:FAIL");
            }
        }

        public void Undo()
        {
            if (controller == null) return;
            var ok = controller.TryUseUndoBooster();
            Write(ok ? "Undo:OK" : "Undo:FAIL");
        }

        public void Shuffle()
        {
            if (controller == null) return;
            var ok = controller.TryUseShuffleBooster();
            Write(ok ? "Shuffle:OK" : "Shuffle:FAIL");
        }

        private void Write(string message)
        {
            if (outputText != null)
            {
                outputText.text = message;
            }

            Debug.Log(message);
        }
    }
}
