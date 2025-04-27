namespace ByReplace.Providers;

[ExcludeFromCodeCoverage]
internal static class ParallelOptionsProvider
{
    public static ParallelOptions GetDefault()
    {
        return new ParallelOptions()
        {
#if DEBUG
            MaxDegreeOfParallelism = 1,
#else
            MaxDegreeOfParallelism = Environment.ProcessorCount / 2,
#endif
        };
    }
}
