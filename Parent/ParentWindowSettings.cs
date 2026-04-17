namespace FileManager.Parent
{
    /// <summary>
    /// Static class containing settings for <see cref="ParentWindow"/>.
    /// </summary>
    public static class ParentWindowSettings
    {
        #region Window dimesions and cursor position settings

        /// <summary>
        /// Width of <see cref="ParentWindow"/>.
        /// </summary>
        public static int WindowWidth
            => Convert.ToInt32(Console.WindowWidth / 100d * 30d);

        /// <summary>
        /// Default height of <see cref="ParentWindow"/>.
        /// </summary>
        public static int WindowHeight
            => Math.Max(Console.WindowHeight - StatusBarSettings.WindowHeight, 0);

        /// <summary>
        /// Default column (left) position for the cursor.
        /// </summary>
        public static int StartingColumn
            => 0;

        /// <summary>
        /// Default row (top) position for the cursor.
        /// </summary>
        public static int StartingRow
            => 0;

        #endregion

        #region Console color settings

        /// <summary>
        /// Foreground console color for files.
        /// </summary>
        public static ConsoleColor FileForegroundColor
            => NavigationWindowSettings.GetConsoleColors(false, false).ForegroundColor;

        /// <summary>
        /// Background console color for files.
        /// </summary>
        public static ConsoleColor FileBackgroundColor
            => NavigationWindowSettings.GetConsoleColors(false, false).BackgroundColor;

        /// <summary>
        /// Foreground console color for directories.
        /// </summary>
        public static ConsoleColor DirectoryForegroundColor
            => NavigationWindowSettings.GetConsoleColors(true, false).ForegroundColor;

        /// <summary>
        /// Background console color for directories.
        /// </summary>
        public static ConsoleColor DirectoryBackgroundColor
            => NavigationWindowSettings.GetConsoleColors(true, false).BackgroundColor;

        /// <summary>
        /// Foreground console color for parent directory of the <see cref="NavigationWindow"/>.
        /// </summary>
        public static ConsoleColor SelectedForegroundColor
            => NavigationWindowSettings.GetConsoleColors(true, true).ForegroundColor;

        /// <summary>
        /// Background console color for parent directory of the <see cref="NavigationWindow"/>.
        /// </summary>
        public static ConsoleColor SelectedBackgroundColor
            => NavigationWindowSettings.GetConsoleColors(true, true).BackgroundColor;

        /// <summary>
        /// Sets the foreground and background console colors, depending on what kind of entry is supposed to be printed to console.
        /// </summary>
        /// <param name="isDirectory">A <see cref="bool"/> signaling if the entry is a directory.</param>
        /// <param name="isSelected">A <see cref="bool"/> signaling if the entry is selected (is the parent directory of the current <see cref="NavigationWindow"/>).</param>
        public static void SetConsoleColors(bool isDirectory, bool isSelected)
        {
            if (isDirectory)
            {
                (Console.ForegroundColor, Console.BackgroundColor) = isSelected
                    ? (SelectedForegroundColor, SelectedBackgroundColor)
                    : (DirectoryForegroundColor, DirectoryBackgroundColor);
            }
            else
            {
                Console.ForegroundColor = FileForegroundColor;
                Console.BackgroundColor = FileBackgroundColor;
            }
        }

        #endregion
    }
}
