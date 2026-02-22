#if UNITY_EDITOR
using TileRift.Runtime;
using TileRift.UI;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace TileRift.EditorTools
{
    public static class CreateMvpSceneTool
    {
        [MenuItem("TileRift/Create MVP Scene")]
        public static void CreateScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var runtimeGo = new GameObject("Runtime");
            var controller = runtimeGo.AddComponent<GameSessionController>();
            runtimeGo.AddComponent<SceneBootstrap>();
            runtimeGo.AddComponent<TapMoveInput>();

            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var hudGo = new GameObject("HUD", typeof(RectTransform));
            hudGo.transform.SetParent(canvasGo.transform, false);
            var hud = hudGo.AddComponent<HudPresenter>();

            var menuGo = new GameObject("Menu", typeof(RectTransform));
            menuGo.transform.SetParent(canvasGo.transform, false);
            var menu = menuGo.AddComponent<MenuPresenter>();

            var boardGo = new GameObject("BoardDebug", typeof(RectTransform));
            boardGo.transform.SetParent(canvasGo.transform, false);
            var board = boardGo.AddComponent<BoardDebugView>();
            var actionGo = new GameObject("Actions", typeof(RectTransform));
            actionGo.transform.SetParent(canvasGo.transform, false);
            var actionPanel = actionGo.AddComponent<DebugActionPanel>();
            var actionRect = actionGo.GetComponent<RectTransform>();
            actionRect.anchorMin = new Vector2(0f, 0f);
            actionRect.anchorMax = new Vector2(0f, 0f);
            actionRect.pivot = new Vector2(0f, 0f);
            actionRect.anchoredPosition = new Vector2(20f, 20f);
            actionRect.sizeDelta = new Vector2(260f, 370f);

            var boosterGo = new GameObject("Boosters", typeof(RectTransform));
            boosterGo.transform.SetParent(canvasGo.transform, false);
            var boosterRect = boosterGo.GetComponent<RectTransform>();
            boosterRect.anchorMin = new Vector2(1f, 1f);
            boosterRect.anchorMax = new Vector2(1f, 1f);
            boosterRect.pivot = new Vector2(1f, 1f);
            boosterRect.anchoredPosition = new Vector2(-20f, -20f);
            boosterRect.sizeDelta = new Vector2(220f, 140f);

            var moveText = CreateText("MovesText", hudGo.transform, new Vector2(20, -20));
            var coinText = CreateText("CoinText", hudGo.transform, new Vector2(20, -60));
            var pauseText = CreateText("PauseText", hudGo.transform, new Vector2(20, -100));
            pauseText.text = "PAUSED";
            pauseText.gameObject.SetActive(false);

            var homePanel = CreatePanel("HomePanel", menuGo.transform, new Color(0f, 0f, 0f, 0.4f));
            var failPanel = CreatePanel("FailPanel", menuGo.transform, new Color(0.4f, 0f, 0f, 0.4f));
            var winPanel = CreatePanel("WinPanel", menuGo.transform, new Color(0f, 0.4f, 0f, 0.4f));
            failPanel.SetActive(false);
            winPanel.SetActive(false);

            var boardText = CreateText("BoardText", boardGo.transform, new Vector2(300, -20));
            boardText.alignment = TextAnchor.UpperLeft;
            var actionOut = CreateText("ActionOutput", actionGo.transform, new Vector2(8f, -332f));
            actionOut.rectTransform.anchorMin = new Vector2(0f, 0f);
            actionOut.rectTransform.anchorMax = new Vector2(1f, 0f);
            actionOut.rectTransform.pivot = new Vector2(0f, 0f);
            actionOut.rectTransform.sizeDelta = new Vector2(-16f, 28f);
            actionOut.fontSize = 16;
            actionOut.text = "Action output";

            var startBtn = CreateButton("StartButton", actionGo.transform, "Start", new Vector2(0, -20), false);
            var dailyBtn = CreateButton("DailyButton", actionGo.transform, "Daily", new Vector2(0, -56), false);
            var restartBtn = CreateButton("RestartButton", actionGo.transform, "Restart", new Vector2(0, -92), false);
            var nextBtn = CreateButton("NextButton", actionGo.transform, "Next", new Vector2(0, -128), false);
            var contBtn = CreateButton("ContinueAdButton", actionGo.transform, "Continue Ad", new Vector2(0, -164), false);
            var coinAdBtn = CreateButton("CoinAdButton", actionGo.transform, "Reward Coin", new Vector2(0, -200), false);
            var buyNoAdsBtn = CreateButton("BuyNoAdsButton", actionGo.transform, "Buy No Ads", new Vector2(0, -236), false);
            var buyCoinsBtn = CreateButton("BuyCoinsButton", actionGo.transform, "Buy Coin Pack", new Vector2(0, -272), false);

            var undoBtn = CreateButton("UndoButton", boosterGo.transform, "Undo", new Vector2(-110, -20));
            var hintBtn = CreateButton("HintButton", boosterGo.transform, "Hint", new Vector2(-110, -60));
            var shuffleBtn = CreateButton("ShuffleButton", boosterGo.transform, "Shuffle", new Vector2(-110, -100));
            UnityEventTools.AddPersistentListener(undoBtn.onClick, controller.UseUndoBoosterButton);
            UnityEventTools.AddPersistentListener(hintBtn.onClick, controller.UseHintBoosterButton);
            UnityEventTools.AddPersistentListener(shuffleBtn.onClick, controller.UseShuffleBoosterButton);
            UnityEventTools.AddPersistentListener(startBtn.onClick, actionPanel.StartGame);
            UnityEventTools.AddPersistentListener(dailyBtn.onClick, actionPanel.StartDaily);
            UnityEventTools.AddPersistentListener(restartBtn.onClick, actionPanel.Restart);
            UnityEventTools.AddPersistentListener(nextBtn.onClick, actionPanel.Next);
            UnityEventTools.AddPersistentListener(contBtn.onClick, actionPanel.ContinueRewarded);
            UnityEventTools.AddPersistentListener(coinAdBtn.onClick, actionPanel.RewardCoin);
            UnityEventTools.AddPersistentListener(buyNoAdsBtn.onClick, actionPanel.BuyNoAds);
            UnityEventTools.AddPersistentListener(buyCoinsBtn.onClick, actionPanel.BuyCoinPack);

            var levels = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/levels_mvp.json");
            if (levels == null)
            {
                Debug.LogWarning("Assets/Data/levels_mvp.json not found. Assign manually on GameSessionController.");
            }

            SetSerializedReference(controller, "levelCatalogJson", levels);
            SetSerializedReference(controller, "hudPresenter", hud);
            SetSerializedReference(controller, "menuPresenter", menu);
            SetSerializedReference(controller, "boardDebugView", board);
            SetSerializedReference(actionPanel, "controller", controller);
            SetSerializedReference(actionPanel, "outputText", actionOut);

            SetSerializedReference(hud, "movesText", moveText);
            SetSerializedReference(hud, "coinText", coinText);
            SetSerializedReference(hud, "pauseIndicator", pauseText.gameObject);

            SetSerializedReference(menu, "homePanel", homePanel);
            SetSerializedReference(menu, "failPanel", failPanel);
            SetSerializedReference(menu, "winPanel", winPanel);

            SetSerializedReference(board, "boardText", boardText);

            EditorSceneManager.SaveScene(scene, "Assets/Scenes/Main.unity");
            AssetDatabase.SaveAssets();
            Debug.Log("MVP scene created at Assets/Scenes/Main.unity");
        }

        private static Text CreateText(string name, Transform parent, Vector2 anchoredPos)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = new Vector2(360, 32);

            var text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 24;
            text.color = Color.white;
            text.text = name;
            return text;
        }

        private static GameObject CreatePanel(string name, Transform parent, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var image = go.GetComponent<Image>();
            image.color = color;
            return go;
        }

        private static Button CreateButton(string name, Transform parent, string label, Vector2 anchoredPos, bool rightAligned = true)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);

            var rect = go.GetComponent<RectTransform>();
            rect.anchorMin = rightAligned ? new Vector2(1f, 1f) : new Vector2(0f, 1f);
            rect.anchorMax = rightAligned ? new Vector2(1f, 1f) : new Vector2(0f, 1f);
            rect.pivot = rightAligned ? new Vector2(1f, 1f) : new Vector2(0f, 1f);
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = new Vector2(220f, 30f);

            var image = go.GetComponent<Image>();
            image.color = new Color(0.1f, 0.1f, 0.1f, 0.75f);

            var text = CreateText(name + "Text", go.transform, new Vector2(8f, -4f));
            text.text = label;
            text.alignment = TextAnchor.MiddleCenter;
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            text.rectTransform.anchoredPosition = Vector2.zero;
            text.rectTransform.sizeDelta = Vector2.zero;

            return go.GetComponent<Button>();
        }

        private static void SetSerializedReference(Object target, string field, Object value)
        {
            var so = new SerializedObject(target);
            var prop = so.FindProperty(field);
            if (prop != null)
            {
                prop.objectReferenceValue = value;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}
#endif
