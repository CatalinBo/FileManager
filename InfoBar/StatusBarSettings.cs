namespace FileManager.InfoBar
{
    /// <summary>
    /// Static class containing settings for <see cref="StatusBar"/> window.
    /// </summary>
    public static class StatusBarSettings
    {
        #region Window dimensions settings

        /// <summary>
        /// The height of the <see cref="StatusBar"/> when dealing with a small <see cref="Console"/> window height.
        /// </summary>
        private const int SmallSizeHeight = 0;
        /// <summary>
        /// The height of the <see cref="StatusBar"/> when dealing with a large <see cref="Console"/> window height.
        /// </summary>
        private const int NormalSizeHeight = 2;
        /// <summary>
        /// The height of the <see cref="StatusBar"/> when dealing with a small <see cref="Console"/> window width.
        /// </summary>
        private const int NarrowSizeHeight = 3;
        /// <summary>
        /// The minimum threshold width for the <see cref="StatusBar"/> to be displayed.
        /// </summary>
        private const int MinimumWidthThreshold = 50;

        #endregion

        #region Console dimensions and cursor position settings

        /// <summary>
        /// The width of <see cref="StatusBar"/> window.
        /// </summary>
        public static int WindowWidth
            => Console.WindowWidth;

        /// <summary>
        /// The height of <see cref="StatusBar"/> window, depending on <see cref="Console"/> window size.
        /// </summary>
        public static int WindowHeight
            => GetHeight();

        /// <summary>
        /// Gets the required <see cref="StatusBar"/> window height, depending on <see cref="Console"/> window height.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the necessary window height.</returns>
        private static int GetHeight()
        {
            if (Console.WindowWidth < MinimumWidthThreshold && Console.WindowHeight > NarrowSizeHeight)
            {
                return NarrowSizeHeight;
            }

            if (Console.WindowHeight > NormalSizeHeight)
            {
                return NormalSizeHeight;
            }

            return SmallSizeHeight;
        }

        /// <summary>
        /// The width of size info area inside the <see cref="StatusBar"/> window.
        /// </summary>
        public static int SizeInfoWindowWidth
            => WindowWidth / 2;

        /// <summary>
        /// The width of last modified info area inside the <see cref="StatusBar"/> window.
        /// </summary>
        public static int LastModifiedInfoWindowWidth
            => WindowWidth - SizeInfoWindowWidth;

        /// <summary>
        /// The column (left) position at which the <see cref="StatusBar"/> window printing starts.
        /// </summary>
        public static int StartingColumn
            => 0;

        /// <summary>
        /// The row (top) position at which the <see cref="StatusBar"/> window printing starts.
        /// </summary>
        public static int StartingRow
            => Console.WindowHeight - WindowHeight;

        #endregion

        #region Console colors settings

        /// <summary>
        /// Foreground console color for <see cref="StatusBar"/>.
        /// </summary>
        public static ConsoleColor ForegroundColor
            => ConsoleColor.Green;

        /// <summary>
        /// Background console color for <see cref="StatusBar"/>.
        /// </summary>
        public static ConsoleColor BackgroundColor
            => Console.BackgroundColor;

        #endregion

        #region Layout helper strings

        public static string TopBorder
            => $"{string.Concat(Enumerable.Repeat(FileManagerSettings.HorizontalLine, WindowWidth))}";

        public static string SizeLabel
            => $"Size:";

        public static string LastModifiedLabel
            => $"Last modified on:";

        #endregion
    }
}
