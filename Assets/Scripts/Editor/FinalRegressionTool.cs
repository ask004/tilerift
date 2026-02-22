#if UNITY_EDITOR
using System;
using System.IO;
using TileRift.QA;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace TileRift.EditorTools
{
    public static class FinalRegressionTool
    {
        [MenuItem("TileRift/QA/Run Final Regression")]
        public static void Run()
        {
            GenerateQaReportTool.Generate();

            var readinessPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "android-readiness-report.json");
            var readiness = File.Exists(readinessPath) ? File.ReadAllText(readinessPath) : "{}";

            var summaryPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "final-regression-summary.txt");
            var summary = string.Join(
                Environment.NewLine,
                "TileRift Final Regression",
                $"TimestampUtc: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
                $"ActiveTarget: {EditorUserBuildSettings.activeBuildTarget}",
                $"SceneCount: {EditorBuildSettings.scenes.Length}",
                "ReadinessReport:",
                readiness);

            File.WriteAllText(summaryPath, summary);
            Debug.Log($"Final regression summary generated: {summaryPath}");
        }
    }
}
#endif
