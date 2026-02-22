using System;
using System.Collections.Generic;

namespace TileRift.QA
{
    [Serializable]
    public sealed class BuildReadinessInput
    {
        public bool hasMainScene;
        public bool targetAndroid;
        public bool il2CppEnabled;
        public bool minSdkConfigured;
        public bool bundleIdentifierSet;
        public int criticalCrashCount;
    }

    [Serializable]
    public sealed class BuildReadinessReport
    {
        public bool canRelease;
        public List<string> passed = new();
        public List<string> failed = new();
    }

    public static class BuildReadinessEvaluator
    {
        public static BuildReadinessReport Evaluate(BuildReadinessInput input)
        {
            var report = new BuildReadinessReport();

            Check(report, input.hasMainScene, "Main scene is present in build settings");
            Check(report, input.targetAndroid, "Active build target is Android");
            Check(report, input.il2CppEnabled, "Scripting backend is IL2CPP");
            Check(report, input.minSdkConfigured, "Minimum SDK is configured");
            Check(report, input.bundleIdentifierSet, "Bundle identifier is set");
            Check(report, input.criticalCrashCount == 0, "No critical crashes are open");

            report.canRelease = report.failed.Count == 0;
            return report;
        }

        private static void Check(BuildReadinessReport report, bool condition, string message)
        {
            if (condition)
            {
                report.passed.Add(message);
            }
            else
            {
                report.failed.Add(message);
            }
        }
    }
}
