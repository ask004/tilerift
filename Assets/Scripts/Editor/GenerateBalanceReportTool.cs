#if UNITY_EDITOR
using System.IO;
using TileRift.Content;
using TileRift.Level;
using UnityEditor;
using UnityEngine;

namespace TileRift.EditorTools
{
    public static class GenerateBalanceReportTool
    {
        [MenuItem("TileRift/Content/Generate Level Balance Report")]
        public static void Generate()
        {
            var levelAsset = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Data/levels_mvp.json");
            if (levelAsset == null)
            {
                Debug.LogError("levels_mvp.json not found at Assets/Data/levels_mvp.json");
                return;
            }

            var levels = LevelLoader.LoadMany(levelAsset.text);
            var report = LevelBalanceAnalyzer.Analyze(levels);
            var json = JsonUtility.ToJson(report, true);

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, "level-balance-report.json");
            File.WriteAllText(path, json);

            Debug.Log($"Level balance report generated: {path}");
        }
    }
}
#endif
