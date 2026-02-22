#if UNITY_EDITOR
using System;
using System.IO;
using TileRift.Content;
using TileRift.Level;
using TileRift.QA;
using UnityEditor;
using UnityEngine;

namespace TileRift.EditorTools
{
    public static class RunReleaseRehearsalTool
    {
        [MenuItem("TileRift/QA/Run Release Rehearsal")]
        public static void RunFromMenu()
        {
            RunCore(quitOnFinish: false);
        }

        public static void RunFromCommandLine()
        {
            RunCore(quitOnFinish: true);
        }

        private static void RunCore(bool quitOnFinish)
        {
            ConfigureAndroidReleaseTool.ConfigureFromMenu();
            GenerateQaReportTool.Generate();
            GenerateBalanceReportTool.Generate();
            AndroidBuildTool.DryRunNoExit();

            var logs = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            var readinessPath = Path.Combine(logs, "android-readiness-report.json");
            var balancePath = Path.Combine(logs, "level-balance-report.json");
            var buildPath = Path.Combine(logs, "android-build-report.json");

            var readinessGreen = false;
            var balanceHealthy = false;
            var buildDryRunPassed = false;

            if (File.Exists(readinessPath))
            {
                var readiness = JsonUtility.FromJson<BuildReadinessReport>(File.ReadAllText(readinessPath));
                readinessGreen = readiness != null && readiness.canRelease;
            }

            if (File.Exists(balancePath))
            {
                var balance = JsonUtility.FromJson<BalanceReport>(File.ReadAllText(balancePath));
                balanceHealthy = balance != null && !balance.hasEarlyHardBlock;
            }

            if (File.Exists(buildPath))
            {
                var build = JsonUtility.FromJson<AndroidBuildRunReport>(File.ReadAllText(buildPath));
                buildDryRunPassed = build != null && build.success;
            }

            var summary = ReleaseRehearsal.Compose(
                androidConfigured: true,
                readinessGreen: readinessGreen,
                balanceHealthy: balanceHealthy,
                buildDryRunPassed: buildDryRunPassed);

            var output = Path.Combine(logs, "release-rehearsal.json");
            File.WriteAllText(output, JsonUtility.ToJson(summary, true));
            Debug.Log($"Release rehearsal generated: {output}");

            if (quitOnFinish)
            {
                EditorApplication.Exit(summary.readyToShip ? 0 : 3);
            }
        }
    }
}
#endif
