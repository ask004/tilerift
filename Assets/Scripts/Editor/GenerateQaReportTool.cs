#if UNITY_EDITOR
using System.IO;
using TileRift.QA;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace TileRift.EditorTools
{
    public static class GenerateQaReportTool
    {
        [MenuItem("TileRift/QA/Generate Android Readiness Report")]
        public static void Generate()
        {
            var scenes = EditorBuildSettings.scenes;
            var hasMainScene = false;
            for (var i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].enabled && scenes[i].path.EndsWith("Assets/Scenes/Main.unity"))
                {
                    hasMainScene = true;
                    break;
                }
            }

            var input = new BuildReadinessInput
            {
                hasMainScene = hasMainScene,
                targetAndroid = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android,
                il2CppEnabled = PlayerSettings.GetScriptingBackend(BuildTargetGroup.Android) == ScriptingImplementation.IL2CPP,
                minSdkConfigured = (int)PlayerSettings.Android.minSdkVersion >= 23,
                bundleIdentifierSet = !string.IsNullOrWhiteSpace(PlayerSettings.applicationIdentifier),
                criticalCrashCount = 0,
            };

            var report = BuildReadinessEvaluator.Evaluate(input);
            var json = JsonUtility.ToJson(report, true);

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            Directory.CreateDirectory(folder);
            var path = Path.Combine(folder, "android-readiness-report.json");
            File.WriteAllText(path, json);

            Debug.Log($"QA report generated: {path}");
        }
    }
}
#endif
