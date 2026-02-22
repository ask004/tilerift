#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace TileRift.EditorTools
{
    public static class CommandLineEditModeRunner
    {
        private sealed class Callback : ICallbacks
        {
            public void RunStarted(ITestAdaptor testsToRun)
            {
                // no-op
            }

            public void RunFinished(ITestResultAdaptor result)
            {
                var summaryPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "EditModeSummary.txt");
                Directory.CreateDirectory(Path.GetDirectoryName(summaryPath) ?? "Logs");

                var summary = string.Join(
                    Environment.NewLine,
                    $"Total:{result.PassCount + result.FailCount + result.SkipCount}",
                    $"Passed:{result.PassCount}",
                    $"Failed:{result.FailCount}",
                    $"Skipped:{result.SkipCount}",
                    $"Duration:{result.Duration:0.000}");

                File.WriteAllText(summaryPath, summary);
                EditorApplication.Exit(result.FailCount > 0 ? 2 : 0);
            }

            public void TestStarted(ITestAdaptor test)
            {
                // no-op
            }

            public void TestFinished(ITestResultAdaptor result)
            {
                // no-op
            }
        }

        public static void Run()
        {
            try
            {
                var api = ScriptableObject.CreateInstance<TestRunnerApi>();
                api.RegisterCallbacks(new Callback());
                var filter = new Filter
                {
                    testMode = TestMode.EditMode,
                };
                api.Execute(new ExecutionSettings(filter));
            }
            catch (Exception ex)
            {
                var summaryPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "EditModeSummary.txt");
                Directory.CreateDirectory(Path.GetDirectoryName(summaryPath) ?? "Logs");
                File.WriteAllText(summaryPath, "RunnerError:" + ex);
                EditorApplication.Exit(3);
            }
        }
    }
}
#endif
