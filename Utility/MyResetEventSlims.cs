namespace FileManager.Utility
{
    /// <summary>
    /// Static class that holds <see cref="ManualResetEventSlim"/>s used throughout the application.
    /// </summary>
    public static class MyResetEventSlims
    {
        public static ManualResetEventSlim MainWindowResetEvent { get; } = new(false);
        public static ManualResetEventSlim BackgroundResetEvent { get; } = new(false);
        public static ManualResetEventSlim RenameResetEvent { get; } = new(false);
        public static ManualResetEventSlim PopUpResetEvent { get; } = new(false);
        public static ManualResetEventSlim EditShortcutsResetEvent { get; } = new(false);
        public static ManualResetEventSlim AppFeatureResetEvent { get; } = new(false);
    }
}
