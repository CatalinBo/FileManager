namespace FileManager
{
    /// <summary>
    /// Implementation of the <see cref="FileConflictResolverSettings"/> class.
    /// </summary>
    public static class FileConflictResolverSettings
    {
        public const int MinimumWindowHeight = 6;
        private const string FileDeletionMessage = $"One or more files could not be deleted";
        private const string FileConflictMessage = $"The destination has one or more files with the same names";
        private const string RenameConflictMessage = $"File with the same name already found in this directory";

        /// <summary>
        /// The width of the <see cref="FileConflictResolver"/> window.
        /// </summary>
        public static int WindowWidth
            => Math.Min(MyConsole.WindowWidth, Math.Max(FullFileConflictMessage.Length, FullFileDeletionMessage.Length));

        /// <summary>
        /// The height of the <see cref="FileConflictResolver"/> window.
        /// </summary>
        public static int WindowHeight
            => MyConsole.WindowHeight >= MinimumWindowHeight ? MinimumWindowHeight : 0;

        /// <summary>
        /// The column at which the printing of <see cref="FileConflictResolver"/> starts
        /// </summary>
        public static int StartingColumn
            => Math.Max((MyConsole.WindowWidth - WindowWidth) / 2, 0);

        /// <summary>
        /// The row at which the printing of <see cref="FileConflictResolver"/> starts.
        /// </summary>
        public static int StartingRow
            => (MyConsole.WindowHeight - WindowHeight) / 2;

        /// <summary>
        /// Top border for the <see cref="FileConflictResolver"/> window.
        /// </summary>
        public static string TopBorder
            => $"{string.Concat(Enumerable.Repeat('-', WindowWidth))}";

        /// <summary>
        /// Bottom border for the <see cref="FileConflictResolver"/> window.
        /// </summary>
        public static string BottomBorder
            => TopBorder;

        /// <summary>
        /// Left border for the <see cref="FileConflictResolver"/> window.
        /// </summary>
        private static string LeftBorder
            => $"| ";

        /// <summary>
        /// Right border for the <see cref="FileConflictResolver"/> window.
        /// </summary>
        private static string RightBorder
            => $" |";

        /// <summary>
        /// Left border for available options.
        /// </summary>
        private static string OptionLeftBorder
            => $"[";

        /// <summary>
        /// Right border for available options.
        /// </summary>
        private static string OptionRightBorder
            => $"]";

        /// <summary>
        /// Gets the full message to be displayed, including borders.
        /// </summary>
        private static string FullFileDeletionMessage
            => $"{LeftBorder}{FileDeletionMessage}{RightBorder}";

        /// <summary>
        /// Gets the full message to be displayed, including borders.
        /// </summary>
        private static string FullFileConflictMessage
            => $"{LeftBorder}{FileConflictMessage}{RightBorder}";

        /// <summary>
        /// Gets the available text box width, depending on window width.
        /// </summary>
        private static int TextBoxWidth
            => WindowWidth - LeftBorder.Length - RightBorder.Length;

        /// <summary>
        /// Gets the list of options when dealing with conflicts on deletion.
        /// </summary>
        private static List<string> FileDeletionOptions
            => new List<string>
            {
                $"{OptionLeftBorder}Try again{OptionRightBorder}",
                $"{OptionLeftBorder}Cancel{OptionRightBorder}"
            };

        /// <summary>
        /// Gets the list of options when dealing with conflicts on moving.
        /// </summary>
        private static List<string> MoveConflictOptions
            => new List<string>
            {
                $"{OptionLeftBorder}Overwrite{OptionRightBorder}",
                $"{OptionLeftBorder}Keep both{OptionRightBorder}",
                $"{OptionLeftBorder}Skip{OptionRightBorder}"
            };

        /// <summary>
        /// Gets the list of options when dealing with conflicts on copying.
        /// </summary>
        private static List<string> CopyConflictOptions
            => new List<string>
            {
                $"{OptionLeftBorder}Overwrite{OptionRightBorder}",
                $"{OptionLeftBorder}Keep both{OptionRightBorder}",
                $"{OptionLeftBorder}Skip{OptionRightBorder}"
            };

        /// <summary>
        /// Gets the list of options when dealing with conflicts on renaming.
        /// </summary>
        private static List<string> RenameConflictOptions
            => new List<string>
            {
                $"{OptionLeftBorder}Overwrite{OptionRightBorder}",
                $"{OptionLeftBorder}Cancel{OptionRightBorder}"
            };

        /// <summary>
        /// Gets the list of options, depending on the current process.
        /// </summary>
        /// <param name="enumType">Type of file conflict encountered.</param>
        /// <returns>A list of available options.</returns>
        public static List<string> GetOptions(Type enumType)
        {
            return enumType switch
            {
                Type type when type == typeof(DeleteConflictOptions) => FileDeletionOptions,
                Type type when type == typeof(MoveConflictOptions) => MoveConflictOptions,
                Type type when type == typeof(CopyConflictOptions) => CopyConflictOptions,
                Type type when type == typeof(RenameConflictOptions) => RenameConflictOptions,
                _ => new List<string>()
            };
        }

        /// <summary>
        /// Gets the printed message, depending on the current process.
        /// </summary>
        /// <param name="enumType">Type of file conflict encountered.</param>
        /// <returns>A string to be displayed inside the window.</returns>
        public static string GetFileConflictMessage(Type enumType)
        {
            return enumType switch
            {
                Type type when type == typeof(DeleteConflictOptions) => GetFileDeletionMessage(),
                Type type when type == typeof(MoveConflictOptions) => GetFileConflictMessage(),
                Type type when type == typeof(CopyConflictOptions) => GetFileConflictMessage(),
                Type type when type == typeof(RenameConflictOptions) => GetRenameConflictMessage(),
                _ => string.Empty
            };
        }

        /// <summary>
        /// Gets the message to be printed when dealing with conflicts on deletion.
        /// </summary>
        /// <returns>The message to be printed when dealing with conflicts on deletion.</returns>
        public static string GetFileDeletionMessage()
        {
            return FileDeletionMessage.Length < TextBoxWidth
                ? $"{LeftBorder}{StringManipulator.GetCenteredText(FileDeletionMessage, TextBoxWidth)}{RightBorder}"
                : $"{LeftBorder}{StringManipulator.TruncateString(FileDeletionMessage, TextBoxWidth)}{RightBorder}";
        }

        /// <summary>
        /// Gets the message to be printed when dealing with conflicts on moving or copying.
        /// </summary>
        /// <returns>The message to be printed when dealing with conflicts on moving or copying.</returns>
        public static string GetFileConflictMessage()
        {
            return FileConflictMessage.Length < TextBoxWidth
                ? $"{LeftBorder}{StringManipulator.GetCenteredText(FileConflictMessage, TextBoxWidth)}{RightBorder}"
                : $"{LeftBorder}{StringManipulator.TruncateString(FileConflictMessage, TextBoxWidth)}{RightBorder}";
        }

        /// <summary>
        /// Gets the message to be printed when dealing with conflicts on renaming.
        /// </summary>
        /// <returns>The message to be printed when dealing with conflicts on renaming.</returns>
        public static string GetRenameConflictMessage()
        {
            return RenameConflictMessage.Length < TextBoxWidth
                ? $"{LeftBorder}{StringManipulator.GetCenteredText(RenameConflictMessage, TextBoxWidth)}{RightBorder}"
                : $"{LeftBorder}{StringManipulator.TruncateString(RenameConflictMessage, TextBoxWidth)}{RightBorder}";
        }

        /// <summary>
        /// Gets the centered option message, including borders.
        /// </summary>
        /// <param name="option">Available option.</param>
        /// <returns>Processed <see cref="string"/>, centered and bordered.</returns>
        public static string GetOptionMessage(string option)
            => $"{LeftBorder}{StringManipulator.GetCenteredText(option, TextBoxWidth)}{RightBorder}";

        /// <summary>
        /// Foreground color for the <see cref="FileConflictResolver"/>.
        /// </summary>
        private static ConsoleColor ForegroundColor
            => ConsoleColor.Green;

        /// <summary>
        /// Background color for the <see cref="FileConflictResolver"/>.
        /// </summary>
        private static ConsoleColor BackgroundColor
            => MyConsole.BackgroundColor;

        /// <summary>
        /// Foreground color for the selected option.
        /// </summary>
        private static ConsoleColor SelectedForegroundColor
            => MyConsole.BackgroundColor;

        /// <summary>
        /// Background color for the selected option.
        /// </summary>
        private static ConsoleColor SelectedBackgroundColor
            => ConsoleColor.Green;

        /// <summary>
        /// Gets the console colors for the <see cref="FileConflictResolver"/>.
        /// </summary>
        /// <param name="isSelected">A <see cref="bool"/> representing the select status for the current option. <see cref="true"/> if the options is selected, otherwise <see cref="false"/>.</param>
        /// <returns>A <see cref="Tuple{ConsoleColor, ConsoleColor}"/> containing foreground and background colors for the <see cref="FileConflictResolver"/>.</returns>
        public static (ConsoleColor ForegroundColor, ConsoleColor BackgroundColor) GetConsoleColors(bool isSelected = false)
        {
            return isSelected
                ? (SelectedForegroundColor, SelectedBackgroundColor)
                : (ForegroundColor, BackgroundColor);
        }
    }
}
