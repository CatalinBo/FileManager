namespace FileManager.Tasks
{
    /// <summary>
    /// Contains settings for <see cref="IBackgroundTasksWindow"/>.
    /// </summary>
    public static class BackgroundTasksWindowSettings
    {
        #region Tasks window helper strings

        public const string DeleteMessage = "Deleting: ";
        public const string MoveMessage = "Moving: ";
        public const string CopyMessage = "Copying: ";
        public const string SourcePathLabel = "Source: ";
        public const string TargetPathLabel = "Target: ";
        public const string TotalProcessedLabel = "Total: ";
        public const string CancelCommandLabel = "Cancel";

        #endregion

        #region Tasks window inside elements

        public const int DetailedWindowElements = 10;
        public const int DetailedWindowVerticalBorders = 2;
        public const int OverviewHorizontalWindowBorders = 2;

        #endregion

        #region Task window dimensions

        /// <summary>
        /// Gets the width of <see cref="IBackgroundTasksWindow"/>.
        /// </summary>
        /// <returns>Width of <see cref="IBackgroundTasksWindow"/>.</returns>
        public static int GetWindowWidth()
            => ParentWindowSettings.WindowWidth;

        /// <summary>
        /// Gets the height of <see cref="IBackgroundTasksWindow"/>.
        /// </summary>
        /// <param name="backgroundTaskWindow">The instance of <see cref="IBackgroundTasksWindow"/> for which to retrieve the height.</param>
        /// <returns>Height of current <see cref="IBackgroundTasksWindow"/> instance.</returns>
        public static int GetWindowHeight(IBackgroundTasksWindow backgroundTaskWindow)
        {
            return backgroundTaskWindow switch
            {
                DetailedTaskWindow => DetailedWindowElements,
                OverviewTaskWindow => backgroundTaskWindow.WindowHeight,
                MinimizedTaskWindow => 1,
                _ => 0
            };
        }

        #endregion

        #region Cursor positioning

        /// <summary>
        /// Gets the starting column (left) position of <see cref="Console"/> cursor.
        /// </summary>
        /// <returns>Starting column (left) position of <see cref="Console"/> cursor.</returns>
        public static int GetStartingColumn()
            => ParentWindowSettings.StartingColumn;

        /// <summary>
        /// Gets the starting row (top) position of <see cref="Console"/> cursor, based on the given instance of <see cref="IBackgroundTasksWindow"/>.
        /// </summary>
        /// <param name="backgroundTaskWindow">Instance of <see cref="IBackgroundTasksWindow"/> for which to retrieve the cursor position.</param>
        /// <returns>Starting row (top) position of <see cref="Console"/> cursor.</returns>
        public static int GetStartingRow(IBackgroundTasksWindow backgroundTaskWindow)
            => ParentWindowSettings.WindowHeight - GetWindowHeight(backgroundTaskWindow);

        #endregion

        #region Console colors

        public static ConsoleColor DeleteTaskForegroundColor
            => ConsoleColor.Red;

        public static ConsoleColor MoveTaskForegroundColor
            => ConsoleColor.Yellow;

        public static ConsoleColor CopyTaskForegroundColor
            => ConsoleColor.Green;

        public static ConsoleColor DetailedBackgroundColor
            => Console.BackgroundColor;

        public static ConsoleColor MinimizedForegroundColor
            => Console.ForegroundColor;

        public static ConsoleColor MinimizedBackgroundColor
            => Console.BackgroundColor;

        public static ConsoleColor OverviewForegroundColor
            => Console.ForegroundColor;

        public static ConsoleColor OverviewBackgroundColor
            => Console.BackgroundColor;

        public static (ConsoleColor ForegroundColor, ConsoleColor BackgroundColor) GetConsoleColors(IProgressBar progressBar)
        {
            var command = progressBar.CommandKey;

            return command switch
            {
                var _ when KeyTypeCheck.IsDeleteKey(command) => (DeleteTaskForegroundColor, DetailedBackgroundColor),
                var _ when KeyTypeCheck.IsCutKey(command) => (MoveTaskForegroundColor, DetailedBackgroundColor),
                var _ when KeyTypeCheck.IsCopyKey(command) => (CopyTaskForegroundColor, DetailedBackgroundColor),
                _ => (Console.ForegroundColor, Console.BackgroundColor),
            };
        }

        #endregion

        #region Task window layout helper strings

        public static string TopBorder
            => StringManipulator.GetDynamicallyGeneratedBorder(
                FileManagerSettings.TopLeftCorner,
                FileManagerSettings.TopRightCorner,
                FileManagerSettings.HorizontalLine,
                GetWindowWidth() - 2);

        public static string BottomBorder
            => StringManipulator.GetDynamicallyGeneratedBorder(
                FileManagerSettings.BottomLeftCorner,
                FileManagerSettings.BottomRightCorner,
                FileManagerSettings.HorizontalLine,
                GetWindowWidth() - 2);

        public static string LeftBorder
            => $"{FileManagerSettings.VerticalLine} ";

        public static string RightBorder
            => $" {FileManagerSettings.VerticalLine}";

        public static string InsideBorder
            => StringManipulator.GetDynamicallyGeneratedBorder(
                FileManagerSettings.TeeRight,
                FileManagerSettings.TeeLeft,
                FileManagerSettings.HorizontalLine,
                GetWindowWidth() - 2);

        #endregion
    }
}
