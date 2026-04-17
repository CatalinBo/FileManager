namespace FileManager.ErrorLog
{
    /// <summary>
    /// Contains methods for logging errors encountered during the execution of the app, and printing them to console.
    /// </summary>
    public static class ErrorLogWindow
    {
        public static bool IsCalled { get; private set; } = false;

        /// <summary>
        /// A <see cref="ConcurrentDictionary{TKey, TValue}"/> that stores all exception caught when manipulating files.
        /// </summary>
        private static ConcurrentDictionary<string, Exception> FileErrors { get; set; } = [];

        /// <summary>
        /// Renders the complete layout of the current <see cref="ErrorLogWindow"/> by calling helper methods to print content and fill remaining space.
        /// </summary>
        public static void PrintLayout()
        {
            ConsoleManager.InitializeCursorPosition(ErrorLogWindowSettings.StartingColumn, ErrorLogWindowSettings.StartingRow);
            ConsoleManager.InitializeConsoleColors(ErrorLogWindowSettings.ForegroundColor, ErrorLogWindowSettings.BackgroundColor);

            var errors = FileErrors.Take(ErrorLogWindowSettings.WindowHeight);

            foreach (var error in errors)
            {
                Console.Write(StringManipulator.TruncateString($"{error.Value.Message}", ErrorLogWindowSettings.WindowWidth));
                ConsoleManager.JumpToNewLine(ErrorLogWindowSettings.StartingColumn);
            }

            StringManipulator.PrintFillerRows(errors.Count(), ErrorLogWindowSettings.WindowWidth, ErrorLogWindowSettings.WindowHeight, Console.CursorLeft, Console.CursorTop);
            Console.ResetColor();
        }

        public static void ToggleWindow()
            => IsCalled = !IsCalled;

        public static void CloseWindow()
            => IsCalled = false;

        /// <summary>
        /// Adds the encountered exception to a <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="path">The path at which the exception was encountered.</param>
        /// <param name="exception">The <see cref="Exception"/> that was encountered.</param>
        public static void AddFileError(string path, Exception exception)
            => FileErrors.TryAdd(path, exception);

        /// <summary>
        /// Clears the entire dictionary of error logs.
        /// </summary>
        public static void ClearErrorLogs()
            => FileErrors.Clear();

        /// <summary>
        /// Retrieves a <see cref="ConcurrentDictionary{TKey, TValue}"/> containing encountered exceptions.
        /// </summary>
        /// <returns>A <see cref="ConcurrentDictionary{TKey, TValue}"/> with all encountered exceptions.</returns>
        public static ConcurrentDictionary<string, Exception> GetFileErrors()
            => FileErrors;
    }
}
