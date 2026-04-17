namespace FileManager.Preview
{
    /// <summary>
    /// Provides methods for generating and printing window layout.
    /// </summary>
    /// <param name="message">Message to be printed inside the window when selecting an empty <see cref="IFileInfo"/>.</param>
    public class EmptyFileWindow(string message = "") : IPreviewWindow
    {
        private readonly string windowMessage = message;

        /// <summary>
        /// Prints the entire layout of <see cref="EmptyFileWindow"/>, by calling helper methods and clearing empty lines.
        /// </summary>
        void IPreviewWindow.PrintLayout()
        {
            ConsoleManager.InitializeCursorPosition(PreviewWindowSettings.StartingColumn, PreviewWindowSettings.StartingRow);
            ConsoleManager.InitializeConsoleColors(PreviewWindowSettings.GetConsoleColors(this).ForegroundColor, PreviewWindowSettings.GetConsoleColors(this).BackgroundColor);

            PrintContent();
            StringManipulator.PrintFillerRows(1, PreviewWindowSettings.WindowWidth, PreviewWindowSettings.WindowHeight, Console.CursorLeft, Console.CursorTop);

            Console.ResetColor();
        }

        /// <summary>
        /// Prints the contents of the <see cref="EmptyFileWindow"/>.
        /// </summary>
        private void PrintContent()
        {
            string message = GetProcessedMessage(windowMessage);

            Console.Write(message);
            ConsoleManager.JumpToNewLine(PreviewWindowSettings.StartingColumn);
        }

        /// <summary>
        /// Truncates and centeres the window message, if necessary.
        /// </summary>
        /// <param name="message">The <see cref="string"/> to be centered and truncated.</param>
        /// <returns>Centered and truncated <see cref="string"/>.</returns>
        private static string GetProcessedMessage(string message)
            => StringManipulator.TruncateString(StringManipulator.GetCenteredText(message, PreviewWindowSettings.WindowWidth), PreviewWindowSettings.WindowWidth);
    }
}
