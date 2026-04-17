namespace FileManager.Utility
{
    /// <summary>
    /// Provides methods for generating manipulating strings to fulfill certain requirments.
    /// </summary>
    public static class StringManipulator
    {
        private const int TabSize = 8;
        private const string OverflowPlaceholder = "...";

        /// <summary>
        /// Truncates a <see cref="string"/> to fill a given window width. The text gets cut at the end.
        /// </summary>
        /// <param name="str"><see cref="string"/> that needs to be truncated.</param>
        /// <param name="width">The width to which to truncate the <see cref="string"/>.</param>
        /// <returns>Truncated <see cref="string"/>.</returns>
        public static string TruncateString(string str, int width)
        {
            var numberOfTabs = str.Count(ch => ch == '\t');

            width = Math.Max(OverflowPlaceholder.Length, width -= numberOfTabs * TabSize);

            return str.Length > width
                ? $"{str[..(width - OverflowPlaceholder.Length)]}{OverflowPlaceholder}"
                : str.PadRight(width);
        }

        /// <summary>
        /// Centers a <see cref="string"/> inside a given window width.
        /// </summary>
        /// <param name="str"><see cref="string"/> to be centered.</param>
        /// <param name="width">Width of the window in which the <see cref="string"/> will be displayed.</param>
        /// <returns>Centered text.</returns>
        public static string GetCenteredText(string str, int width)
        {
            var spaces = width - str.Length;
            var padLeft = spaces / 2 + str.Length;

            return str.PadLeft(padLeft).PadRight(width);
        }

        /// <summary>
        /// Generates a window border of a certain length, by using variables passed as arguments.
        /// </summary>
        /// <param name="leftCorner"><see cref="char"/> representing the starting point of the border.</param>
        /// <param name="rightCorner"><see cref="char"/> representing the ending point of the border.</param>
        /// <param name="separator"><see cref="char"/> representing the border body.</param>
        /// <param name="width">Total width of the required border.</param>
        /// <returns>A <see cref="string"/> to be used as a window border.</returns>
        public static string GetDynamicallyGeneratedBorder(char leftCorner, char rightCorner, char separator, int width)
            => $"{leftCorner}{string.Concat(Enumerable.Repeat(separator, width))}{rightCorner}";

        /// <summary>
        /// Prints required number of empty rows, starting and ending at a given line.
        /// </summary>
        /// <param name="numberOfElements">Number of current window elements.</param>
        /// <param name="windowWidth">Width of the window.</param>
        /// <param name="windowHeight">Height of the window.</param>
        /// <param name="column">The 'left' value in <see cref="Console.SetCursorPosition(int, int)"/>.</param>
        /// <param name="printingRow">The 'top' value in <see cref="Console.SetCursorPosition(int, int)"/>.</param>
        public static void PrintFillerRows(int numberOfElements, int windowWidth, int windowHeight, int column, int printingRow)
        {
            var size = Math.Max(windowHeight - numberOfElements, 0);
            var fillerRows = Enumerable.Repeat(string.Empty.PadRight(windowWidth), size);

            foreach (var line in fillerRows)
            {
                Console.SetCursorPosition(column, printingRow);
                Console.Write(line);
                printingRow++;
            }
        }

        /// <summary>
        /// When another file with the same name is found when copying, edits the current file name with a counter of copied instances.
        /// </summary>
        /// <param name="name">The name of the copied file.</param>
        /// <param name="extension">Extension of the copied file.</param>
        /// <param name="count">Counter to keep track of how many copy instances have been processed.</param>
        /// <returns>The new file name.</returns>
        public static string GetDuplicateFileName(string name, string extension, int count)
        {
            return count == 0
                ? $"{name} - Copy{extension}"
                : $"{name} - Copy ({count + 1}){extension}";
        }

        /// <summary>
        /// Gets the placeholder for the current <see cref="TaskStatus"/>.
        /// </summary>
        /// <param name="status">Current status of the current task displayed in <see cref="OverviewTaskWindow"/>.</param>
        public static TasksOverviewState GetTaskStatusPlaceholder(TaskStatus? status)
        {
            return status switch
            {
                var _ when status is TaskStatus.Running => TasksOverviewState.Cancel,
                var _ when status is TaskStatus.RanToCompletion => TasksOverviewState.Completed,
                var _ when status is TaskStatus.Canceled => TasksOverviewState.Canceled,
                _ => TasksOverviewState.None
            };
        }
    }
}
