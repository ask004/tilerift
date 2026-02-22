#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace TileRift.EditorTools
{
    public static class ConfigureAndroidReleaseTool
    {
        [MenuItem("TileRift/QA/Configure Android Release Defaults")]
        public static void ConfigureFromMenu()
        {
            var ok = Configure();
            Debug.Log(ok ? "Android release defaults applied." : "Android release defaults partially applied.");
        }

        public static void ConfigureFromCommandLine()
        {
            var ok = Configure();
            EditorApplication.Exit(ok ? 0 : 2);
        }

        private static bool Configure()
        {
            var mainScenePath = "Assets/Scenes/Main.unity";
            EnsureMainSceneInBuildSettings(mainScenePath);

            var switched = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel23;

            if (string.IsNullOrWhiteSpace(PlayerSettings.applicationIdentifier))
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.ask004.tilerift");
            }

            AssetDatabase.SaveAssets();
            return switched;
        }

        private static void EnsureMainSceneInBuildSettings(string scenePath)
        {
            var scenes = EditorBuildSettings.scenes;
            for (var i = 0; i < scenes.Length; i++)
            {
                if (string.Equals(scenes[i].path, scenePath, StringComparison.OrdinalIgnoreCase))
                {
                    scenes[i].enabled = true;
                    EditorBuildSettings.scenes = scenes;
                    return;
                }
            }

            var newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
            Array.Copy(scenes, newScenes, scenes.Length);
            newScenes[scenes.Length] = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettings.scenes = newScenes;
        }
    }
}
#endif
