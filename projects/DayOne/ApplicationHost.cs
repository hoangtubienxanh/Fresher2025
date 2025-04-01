internal static class ApplicationHost
{
    public static bool Cancelled { get; private set; }

    public static void Cancel()
    {
        Cancelled = true;
    }
}