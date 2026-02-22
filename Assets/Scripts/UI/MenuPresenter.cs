using UnityEngine;

namespace TileRift.UI
{
    public sealed class MenuPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject homePanel;
        [SerializeField] private GameObject failPanel;
        [SerializeField] private GameObject winPanel;

        public void HideAll()
        {
            SetPanel(homePanel, false);
            SetPanel(failPanel, false);
            SetPanel(winPanel, false);
        }

        public void ShowHome()
        {
            HideAll();
            SetPanel(homePanel, true);
        }

        public void ShowFail()
        {
            HideAll();
            SetPanel(failPanel, true);
        }

        public void ShowWin()
        {
            HideAll();
            SetPanel(winPanel, true);
        }

        private static void SetPanel(GameObject panel, bool state)
        {
            if (panel != null)
            {
                panel.SetActive(state);
            }
        }
    }
}
