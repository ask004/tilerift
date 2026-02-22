using System;
using System.Collections.Generic;

namespace TileRift.QA
{
    [Serializable]
    public sealed class AndroidBuildRequest
    {
        public string outputPath;
        public bool developmentBuild;
        public bool canRelease;
        public int sceneCount;
    }

    [Serializable]
    public sealed class AndroidBuildValidation
    {
        public bool isValid;
        public List<string> errors = new();
    }

    public static class AndroidBuildRequestValidator
    {
        public static AndroidBuildValidation Validate(AndroidBuildRequest request)
        {
            var result = new AndroidBuildValidation();

            if (request == null)
            {
                result.errors.Add("Request is null.");
                return Finalize(result);
            }

            if (string.IsNullOrWhiteSpace(request.outputPath))
            {
                result.errors.Add("Output path is required.");
            }

            if (!request.outputPath.EndsWith(".aab", StringComparison.OrdinalIgnoreCase))
            {
                result.errors.Add("Output must be an .aab file.");
            }

            if (!request.canRelease)
            {
                result.errors.Add("Readiness is not green; release build is blocked.");
            }

            if (request.sceneCount <= 0)
            {
                result.errors.Add("No enabled scenes found for build.");
            }

            return Finalize(result);
        }

        private static AndroidBuildValidation Finalize(AndroidBuildValidation result)
        {
            result.isValid = result.errors.Count == 0;
            return result;
        }
    }
}
