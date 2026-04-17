namespace FileManager.Preview
{
    /// <summary>
    /// Provides methods for generating and printing window layout.
    /// </summary>
    public class TextFileWindow : IPreviewWindow
    {
        private readonly IFileInfo? fileInfo;
        private readonly List<string> rows = [];

        /// <summary>
        /// Initializes a new instance of <see cref="TextFileWindow"/>.
        /// </summary>
        /// <param name="fileInfo">Instance of <see cref="IFileInfo"/> on which the current <see cref="TextFileWindow"/> is built on.</param>
        public TextFileWindow(IFileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
            ReadFileContent();
        }

        #region Window layout methods

        /// <summary>
        /// Prints the entire <see cref="TextFileWindow"/> layout, by calling helper methods and clearing empty lines.
        /// </summary>
        void IPreviewWindow.PrintLayout()
        {
            ConsoleManager.InitializeCursorPosition(PreviewWindowSettings.StartingColumn, PreviewWindowSettings.StartingRow);
            ConsoleManager.InitializeConsoleColors(PreviewWindowSettings.GetConsoleColors(this).ForegroundColor, PreviewWindowSettings.GetConsoleColors(this).BackgroundColor);

            var text = GetFileContent();

            PrintFileContent(text);
            StringManipulator.PrintFillerRows(text.Count(), PreviewWindowSettings.WindowWidth, PreviewWindowSettings.WindowHeight, Console.CursorLeft, Console.CursorTop);

            Console.ResetColor();
        }

        /// <summary>
        /// Prints <see cref="TextFileWindow"/> contents, depending on window size.
        /// </summary>
        /// <param name="text">An <see cref="IEnumerable{T}"/> containing the text from the current instance of <see cref="IFileInfo"/>.</param>
        private static void PrintFileContent(IEnumerable<string> text)
        {
            foreach (var line in text)
            {
                var textLine = StringManipulator.TruncateString(line, PreviewWindowSettings.WindowWidth);

                ClearLine();
                Console.Write(textLine);
                ConsoleManager.JumpToNewLine(PreviewWindowSettings.StartingColumn);
            }
        }

        /// <summary>
        /// Clears the current line, before printing to it.
        /// </summary>
        private static void ClearLine()
        {
            var emptyLine = string.Concat(Enumerable.Repeat(' ', PreviewWindowSettings.WindowWidth));

            Console.Write(emptyLine);
            Console.CursorLeft = PreviewWindowSettings.StartingColumn;
        }

        #endregion

        #region File reading methods

        /// <summary>
        /// Reads the file contents and add them to <see cref="rows"/>.
        /// </summary>
        private void ReadFileContent()
        {
            using var sr = fileInfo?.OpenText();
            string? line;

            while ((line = sr?.ReadLine()) != null)
            {
                rows.Add(line);
            }
        }

        /// <summary>
        /// Gets a limited number of rows to print, depending on window size.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> containing text to be printed to console.</returns>
        private IEnumerable<string> GetFileContent()
        {
            var message = $"...and {rows.Count - PreviewWindowSettings.WindowHeight + 1} more lines.";
            var reqSize = Math.Min(PreviewWindowSettings.WindowHeight, rows.Count);

            return rows.Count >= PreviewWindowSettings.WindowHeight
                ? rows.Take(reqSize - 1).Concat([message])
                : rows.Take(reqSize);
        }

        #endregion
    }
}
