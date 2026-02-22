using NUnit.Framework;
using TileRift.QA;

namespace TileRift.Tests.EditMode
{
    public sealed class AndroidBuildRequestValidatorTests
    {
        [Test]
        public void Validator_Accepts_ValidRequest()
        {
            var request = new AndroidBuildRequest
            {
                outputPath = "Builds/Android/TileRift.aab",
                canRelease = true,
                developmentBuild = false,
                sceneCount = 1,
            };

            var result = AndroidBuildRequestValidator.Validate(request);
            Assert.That(result.isValid, Is.True);
            Assert.That(result.errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void Validator_Rejects_WhenReadinessIsFalse()
        {
            var request = new AndroidBuildRequest
            {
                outputPath = "Builds/Android/TileRift.aab",
                canRelease = false,
                developmentBuild = false,
                sceneCount = 1,
            };

            var result = AndroidBuildRequestValidator.Validate(request);
            Assert.That(result.isValid, Is.False);
            Assert.That(result.errors.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Validator_Rejects_WhenExtensionIsNotAab()
        {
            var request = new AndroidBuildRequest
            {
                outputPath = "Builds/Android/TileRift.apk",
                canRelease = true,
                developmentBuild = false,
                sceneCount = 1,
            };

            var result = AndroidBuildRequestValidator.Validate(request);
            Assert.That(result.isValid, Is.False);
        }
    }
}
