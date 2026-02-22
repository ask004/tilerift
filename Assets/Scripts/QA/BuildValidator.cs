namespace TileRift.QA
{
    public sealed class BuildValidator
    {
        public int CriticalCrashCount { get; set; }

        public bool CanRelease() => CriticalCrashCount == 0;
    }
}
