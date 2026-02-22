#if UNITY_EDITOR
using TileRift.Runtime;
using TileRift.UI;
using UnityEditor;
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

            var levels = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/levels_mvp.json");
            if (levels == null)
            {
                Debug.LogWarning("Assets/Data/levels_mvp.json not found. Assign manually on GameSessionController.");
            }

            SetSerializedReference(controller, "levelCatalogJson", levels);
            SetSerializedReference(controller, "hudPresenter", hud);
            SetSerializedReference(controller, "menuPresenter", menu);
            SetSerializedReference(controller, "boardDebugView", board);

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
