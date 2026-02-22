#if UNITY_EDITOR
using System;
using System.IO;
using TileRift.QA;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace TileRift.EditorTools
{
    [Serializable]
    public sealed class AndroidBuildRunReport
    {
        public bool success;
        public string outputPath;
        public string result;
        public string message;
        public float totalTimeSeconds;
        public long totalSizeBytes;
    }

    public static class AndroidBuildTool
    {
        [MenuItem("TileRift/QA/Build Android AAB")]
        public static void BuildFromMenu()
        {
            ExecuteBuild(dryRun: false, quitOnFinish: false);
        }

        public static void BuildFromCommandLine()
        {
            ExecuteBuild(dryRun: false, quitOnFinish: true);
        }

        public static void DryRunFromCommandLine()
        {
            ExecuteBuild(dryRun: true, quitOnFinish: true);
        }

        public static void DryRunNoExit()
        {
            ExecuteBuild(dryRun: true, quitOnFinish: false);
        }

        private static void ExecuteBuild(bool dryRun, bool quitOnFinish)
        {
            var reportFolder = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            Directory.CreateDirectory(reportFolder);
            var reportPath = Path.Combine(reportFolder, "android-build-report.json");

            var scenes = EditorBuildSettings.scenes;
            var enabledScenes = 0;
            for (var i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].enabled)
                {
                    enabledScenes++;
                }
            }

            var readinessJsonPath = Path.Combine(reportFolder, "android-readiness-report.json");
            var canRelease = false;
            if (File.Exists(readinessJsonPath))
            {
                var readiness = JsonUtility.FromJson<BuildReadinessReport>(File.ReadAllText(readinessJsonPath));
                canRelease = readiness != null && readiness.canRelease;
            }

            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "Android", "TileRift.aab");
            var request = new AndroidBuildRequest
            {
                outputPath = outputPath,
                canRelease = canRelease,
                developmentBuild = false,
                sceneCount = enabledScenes,
            };

            var validation = AndroidBuildRequestValidator.Validate(request);
            if (!validation.isValid)
            {
                var failedReport = new AndroidBuildRunReport
                {
                    success = false,
                    outputPath = outputPath,
                    result = "ValidationFailed",
                    message = string.Join(" | ", validation.errors),
                    totalTimeSeconds = 0f,
                    totalSizeBytes = 0,
                };

                File.WriteAllText(reportPath, JsonUtility.ToJson(failedReport, true));
                Debug.LogError($"Android build validation failed: {failedReport.message}");
                if (quitOnFinish)
                {
                    EditorApplication.Exit(2);
                }

                return;
            }

            if (dryRun)
            {
                var dryReport = new AndroidBuildRunReport
                {
                    success = true,
                    outputPath = outputPath,
                    result = "DryRun",
                    message = "Validation passed. Build not executed.",
                    totalTimeSeconds = 0f,
                    totalSizeBytes = 0,
                };

                File.WriteAllText(reportPath, JsonUtility.ToJson(dryReport, true));
                Debug.Log("Android build dry run passed.");
                if (quitOnFinish)
                {
                    EditorApplication.Exit(0);
                }

                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? Path.Combine(Directory.GetCurrentDirectory(), "Builds"));
            EditorUserBuildSettings.buildAppBundle = true;
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Public;

            var scenePaths = new string[enabledScenes];
            var idx = 0;
            for (var i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].enabled)
                {
                    scenePaths[idx++] = scenes[i].path;
                }
            }

            var options = new BuildPlayerOptions
            {
                scenes = scenePaths,
                locationPathName = outputPath,
                target = BuildTarget.Android,
                options = BuildOptions.None,
            };

            var unityReport = BuildPipeline.BuildPlayer(options);
            var summary = unityReport.summary;
            var runReport = new AndroidBuildRunReport
            {
                success = summary.result == BuildResult.Succeeded,
                outputPath = outputPath,
                result = summary.result.ToString(),
                message = summary.result == BuildResult.Succeeded ? "Build succeeded." : "Build failed.",
                totalTimeSeconds = (float)summary.totalTime.TotalSeconds,
                totalSizeBytes = (long)summary.totalSize,
            };

            File.WriteAllText(reportPath, JsonUtility.ToJson(runReport, true));
            Debug.Log($"Android build finished: {runReport.result}");

            if (quitOnFinish)
            {
                EditorApplication.Exit(runReport.success ? 0 : 3);
            }
        }
    }
}
#endif
