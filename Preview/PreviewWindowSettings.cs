namespace FileManager.Preview
{
    /// <summary>
    /// Hold data and settings for all instances of <see cref="IPreviewWindow"/>.
    /// </summary>
    public static class PreviewWindowSettings
    {
        #region Window dimension and cursor position settings

        /// <summary>
        /// Width of <see cref="IPreviewWindow"/>.
        /// </summary>
        public static int WindowWidth
            => Convert.ToInt32(Console.WindowWidth - ParentWindowSettings.WindowWidth - NavigationWindowSettings.WindowWidth - 1);

        /// <summary>
        /// Height of <see cref="IPreviewWindow"/>.
        /// </summary>
        public static int WindowHeight
            => Math.Max(Console.WindowHeight - StatusBarSettings.WindowHeight, 0);

        /// <summary>
        /// The column (left) position at which the printing starts in <see cref="IPreviewWindow"/>.
        /// </summary>
        public static int StartingColumn
            => ParentWindowSettings.WindowWidth + NavigationWindowSettings.WindowWidth;

        /// <summary>
        /// The row (top) position at which the printing starts in <see cref="IPreviewWindow"/>.
        /// </summary>
        public static int StartingRow
            => 0;

        #endregion

        #region Console colors settings

        private static ConsoleColor DirectoryForegroundColor
            => Console.ForegroundColor;

        private static ConsoleColor DirectoryBackgroundColor
            => Console.BackgroundColor;

        private static ConsoleColor EmptyFileForegroundColor
            => ConsoleColor.DarkGray;

        private static ConsoleColor EmptyFileBackgroundColor
            => Console.BackgroundColor;

        private static ConsoleColor ProtectedFileForegroundColor
            => ConsoleColor.Red;

        private static ConsoleColor ProtectedFileBackgroundColor
            => Console.BackgroundColor;

        private static ConsoleColor TextFileForegroundColor
            => Console.ForegroundColor;

        private static ConsoleColor TextFileBackgroundColor
            => Console.BackgroundColor;

        private static ConsoleColor UnknownFileForegroundColor
            => ConsoleColor.Red;

        private static ConsoleColor UnknownFileBackgroundColor
            => Console.BackgroundColor;

        #endregion

        #region Window layour helper strings

        /// <summary>
        /// The message displayed when dealing with an empty folder.
        /// </summary>
        public static string EmptyFolderMessage
            => $"Folder is empty!";

        /// <summary>
        /// The message displayed when dealing with an unknown file type.
        /// </summary>
        public static string UnknownTypeMessage
            => $"Cannot preview this file!";

        #endregion

        #region Methods to retrieve instance linked settings

        /// <summary>
        /// Gets the foreground and background console colors for the given <see cref="IPreviewWindow"/>.
        /// </summary>
        /// <param name="previewWindow">The instance of <see cref="IPreviewWindow"/> for which to retrieve the console colors.</param>
        /// <returns>A <see cref="Tuple{T1, T2}"/> containing foreground and background console colors.</returns>
        public static (ConsoleColor ForegroundColor, ConsoleColor BackgroundColor) GetConsoleColors(IPreviewWindow previewWindow)
        {
            return previewWindow switch
            {
                DirectoryFileWindow => (DirectoryForegroundColor, DirectoryBackgroundColor),
                EmptyFileWindow => (EmptyFileForegroundColor, EmptyFileBackgroundColor),
                ProtectedFileWindow => (ProtectedFileForegroundColor, ProtectedFileBackgroundColor),
                TextFileWindow => (TextFileForegroundColor, TextFileBackgroundColor),
                UnknownFileWindow => (UnknownFileForegroundColor, UnknownFileBackgroundColor),
                _ => (Console.ForegroundColor, Console.BackgroundColor)
            };
        }

        /// <summary>
        /// Gets the message to be displayed to the given <see cref="IPreviewWindow"/>.
        /// </summary>
        /// <param name="previewWindow">The <see cref="IPreviewWindow"/> for which to retrieve the message.</param>
        /// <returns>A <see cref="string"/> representing the message to be displayed inside the <see cref="IPreviewWindow"/>.</returns>
        public static string GetWindowMessage(IPreviewWindow previewWindow)
        {
            return previewWindow switch
            {
                EmptyFileWindow => EmptyFolderMessage,
                UnknownFileWindow => UnknownTypeMessage,
                _ => string.Empty
            };
        }

        #endregion
    }
}
