namespace Appalachia.Core.Caching
{
    public enum CacheInvalidationStrategy
    {
        NeverInvalidate = 0,
        NeverCache = 10,
        Time = 20,
        Frames = 30,
        AccessCount = 40,
        Conditional = 50,
        FramesAndConditional = 53,
        ExternalValueComparison = 60,
        FramesAndExternalValueComparison = 63
    }
}
