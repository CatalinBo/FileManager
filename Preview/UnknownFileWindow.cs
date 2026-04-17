namespace FileManager.Preview
{
    /// <summary>
    /// Provides methods for generating window layout when the selected file is of unknown type.
    /// </summary>
    public class UnknownFileWindow : IPreviewWindow
    {
        private readonly string windowMessage;

        /// <summary>
        /// Initializes a new instance of <see cref="UnknownFileWindow"/>.
        /// </summary>
        public UnknownFileWindow()
        {
            windowMessage = PreviewWindowSettings.GetWindowMessage(this);
        }

        /// <summary>
        /// Prints the entire <see cref="UnknownFileWindow"/> layout, by calling helper methods and clearing empty lines.
        /// </summary>
        void IPreviewWindow.PrintLayout()
        {
            ConsoleManager.InitializeCursorPosition(PreviewWindowSettings.StartingColumn, PreviewWindowSettings.StartingRow);
            ConsoleManager.InitializeConsoleColors(PreviewWindowSettings.GetConsoleColors(this).ForegroundColor, PreviewWindowSettings.GetConsoleColors(this).BackgroundColor);

            PrintWindowContent();
            StringManipulator.PrintFillerRows(1, PreviewWindowSettings.WindowWidth, PreviewWindowSettings.WindowHeight, Console.CursorLeft, Console.CursorTop);

            Console.ResetColor();
        }

        /// <summary>
        /// Prints the current <see cref="UnknownFileWindow"/> contents.
        /// </summary>
        private void PrintWindowContent()
        {
            var message = StringManipulator.GetCenteredText(windowMessage, PreviewWindowSettings.WindowWidth);
            message = StringManipulator.TruncateString(message, PreviewWindowSettings.WindowWidth);

            Console.Write(message);
            ConsoleManager.JumpToNewLine(PreviewWindowSettings.StartingColumn);
        }
    }
}
