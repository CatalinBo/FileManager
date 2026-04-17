namespace FileManager.Utility
{
    /// <summary>
    /// Provides methods for <see cref="Console"/> state initialization.
    /// </summary>
    public static class ConsoleManager
    {
        /// <summary>
        /// Sets the cursor to the given position.
        /// </summary>
        /// <param name="column">'Left' position for the cursor.</param>
        /// <param name="row">'Top' position for the cursor.</param>
        public static void InitializeCursorPosition(int column, int row)
            => Console.SetCursorPosition(column, row);

        /// <summary>
        /// Sets the <see cref="Console"/> colors to the given values.
        /// </summary>
        /// <param name="foregroundColor"><see cref="Console"/> foreground color.</param>
        /// <param name="backgroundColor"><see cref="Console"/> background color.</param>
        public static void InitializeConsoleColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }

        /// <summary>
        /// Moves the cursor at the start of a new line.
        /// </summary>
        /// <param name="startingColumn">'Left' position at which to move the cursor on the new line.</param>
        public static void JumpToNewLine(int startingColumn)
        {
            Console.CursorLeft = startingColumn;
            Console.CursorTop++;
        }
    }
}
