namespace FileManager
{
    /// <summary>
    /// Represents a wrapper around <see cref="Console"/> interactions.
    /// </summary>
    /// <remarks>
    /// This interface abstracts console operations such as reading user input, writing output, and manipulating the console cursor.
    /// Implementing classes should provide concrete behaviors for these console operations, ensuring thread safety.
    /// </remarks>
    public interface IConsoleWrapper
    {
        /// <summary>
        /// Gets the width of the console window.
        /// </summary>
        int WindowWidth { get; }

        /// <summary>
        /// Gets the height of the console window area.
        /// </summary>
        int WindowHeight { get; }

        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color of the console.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        bool CursorVisible { get; set; }

        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        bool KeyAvailable { get; }

        /// <summary>
        /// Sets the foreground and background console colors to their default values.
        /// </summary>
        void ResetColor();

        /// <summary>
        /// Sets the position of the cursor.
        /// </summary>
        /// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
        void SetCursorPosition(int left, int top);

        /// <summary>
        /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. If <see langword="null"/>, an empty string will be written to the output stream.</param>
        void WriteLine(object? value);

        /// <summary>
        /// Writes the text representation of the specified object to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. If <see langword="null"/>, an empty string will be written to the output stream.</param>
        void Write(object? value);

        /// <summary>
        /// Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">Specifies whether to display the pressed key in the <see cref="Console"/> window. 
        /// <see langword="true"/> to not display the pressed key; otherwise, <see langword="false"/>.</param>
        /// <returns>
        /// An object that describes the <see cref="ConsoleKey"/> constant and Unicode character, if any, that correspond to the pressed console key. 
        /// The <see cref="ConsoleKeyInfo"/> object also describes, in a bitwise combination of <see cref="ConsoleModifiers"/> values, 
        /// whether one or more Shift, Alt, or Ctrl modifier keys were pressed simultaneously with the console key.
        /// </returns>
        ConsoleKeyInfo ReadKey(bool intercept = false);
    }
}
