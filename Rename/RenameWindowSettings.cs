namespace FileManager.Rename
{
    /// <summary>
    /// Static class containing settings for <see cref="RenameWindow"/>.
    /// </summary>
    public static class RenameWindowSettings
    {
        /// <summary>
        /// Maximum length of file paths, as supported by the operating system.
        /// </summary>
        public static int MaximumFilePathChars
            => FileManagerSettings.MaximumPathLength;

        #region Console window and cursor position settings

        /// <summary>
        /// Width of <see cref="RenameWindow"/>.
        /// </summary>
        public static int WindowWidth
            => ParentWindowSettings.WindowWidth + NavigationWindowSettings.WindowWidth;

        /// <summary>
        /// The column (left) position at which the printing of <see cref="RenameWindow"/> label starts.
        /// </summary>
        public static int LabelStartingColumn
            => 0;

        /// <summary>
        /// Foreground console color for <see cref="RenameWindow"/>.
        /// </summary>
        public static ConsoleColor ForegroundColor
            => ConsoleColor.Magenta;

        /// <summary>
        /// Background console color for <see cref="RenameWindow"/>.
        /// </summary>
        public static ConsoleColor BackgroundColor
            => Console.BackgroundColor;

        #endregion

        #region Inside windows settings

        /// <summary>
        /// The column (left) position at which the text box area starts.
        /// </summary>
        public static int TextBoxStartingColumn
            => NavigationWindowSettings.StartingColumn;

        /// <summary>
        /// Width of the label box area.
        /// </summary>
        public static int LabelBoxWidth
            => ParentWindowSettings.WindowWidth;

        /// <summary>
        /// Total available text input box width.
        /// </summary>
        public static int TextBoxWidth
            => NavigationWindowSettings.WindowWidth;

        /// <summary>
        /// Text for the label box area when trying to rename a directory/file.
        /// </summary>
        public static string LabelMessage
            => $"Enter a new name: ";

        #endregion
    }
}
