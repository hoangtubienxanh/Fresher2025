static class ApplicationHost
{
    public static bool Cancelled { get; private set; } = false;
    public static void Stop() => Cancelled = true;
}