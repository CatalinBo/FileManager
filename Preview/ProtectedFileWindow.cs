namespace FileManager.Preview
{
    /// <summary>
    /// Provides methods for generating and printing window layout.
    /// </summary>
    /// <param name="message">The message that must be printed to console. Default value is an empty <see cref="string"/>.</param>
    public class ProtectedFileWindow(string message = "") : IPreviewWindow
    {
        private readonly string exceptionMessage = message;

        /// <summary>
        /// Prints the entire <see cref="ProtectedFileWindow"/> layout, by calling helper methods and clearing empty lines.
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
        /// Prints the contents of the current <see cref="ProtectedFileWindow"/>.
        /// </summary>
        private void PrintWindowContent()
        {
            string message = StringManipulator.GetCenteredText(exceptionMessage, PreviewWindowSettings.WindowWidth);
            message = StringManipulator.TruncateString(message, PreviewWindowSettings.WindowWidth);

            Console.Write(message);
            ConsoleManager.JumpToNewLine(PreviewWindowSettings.StartingColumn);
        }
    }
}
