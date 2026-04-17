namespace FileManager.Utility
{
    /// <summary>
    /// Provides methods to scan for console window size changes and update accordingly.
    /// </summary>
    public static class WindowSizeMonitor
    {
        private static int WindowWidth { get; set; } = Console.WindowWidth;
        private static int WindowHeight { get; set; } = Console.WindowHeight;

        /// <summary>
        /// Updates window width and height to their current state.
        /// </summary>
        public static void UpdateWindowSize()
        {
            WindowWidth = Console.WindowWidth;
            WindowHeight = Console.WindowHeight;
        }

        /// <summary>
        /// Check if window width or height has changed since last updated state.
        /// </summary>
        /// <returns><see cref="true"/> if any change occurred, otherwise <see cref="false"/>.</returns>
        public static bool WindowSizeHasChanged()
        {
            return WindowWidth != Console.WindowWidth 
                || WindowHeight != Console.WindowHeight;
        }

        /// <summary>
        /// Continuously checks for changes in the window size and updates accordingly.
        /// </summary>
        /// <remarks>When a change is detected, it triggers several reset events for different components.
        /// The method periodically checks for changes using a throttle delay to avoid excessive CPU usage.</remarks>
        public static Task CheckForWindowSizeChangesAsync()
        {
            while (true)
            {
                AppControl.MainKillSwitch.Token.ThrowIfCancellationRequested();

                if (WindowSizeHasChanged())
                {
                    UpdateWindowSize();
                    MyResetEventSlims.MainWindowResetEvent.Set();
                    MyResetEventSlims.BackgroundResetEvent.Set();
                    MyResetEventSlims.RenameResetEvent.Set();
                    MyResetEventSlims.PopUpResetEvent.Set();
                    MyResetEventSlims.AppFeatureResetEvent.Set();
                }

                Thread.Sleep(FileManagerSettings.ThrottleDelay);
            }
        }
    }
}
