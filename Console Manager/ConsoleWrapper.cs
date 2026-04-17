namespace FileManager
{
    /// <summary>
    /// Implementation of the <see cref="IConsoleWrapper"/> interface that provides access to <see cref="Console"/> Window properties.
    /// </summary>
    public class ConsoleWrapper : IConsoleWrapper
    {
        /// <summary>
        /// Gets the width of the console window.
        /// </summary>
        public int WindowWidth 
            => Console.WindowWidth;

        /// <summary>
        /// Gets the height of the console window area.
        /// </summary>
        public int WindowHeight 
            => Console.WindowHeight;

        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        /// <summary>
        /// Gets or sets the background color of the console.
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        public bool CursorVisible
        {
            get => Console.CursorVisible;
            set => Console.CursorVisible = value;
        }

        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        public bool KeyAvailable 
            => Console.KeyAvailable;

        /// <summary>
        /// Sets the foreground and background console colors to their default values.
        /// </summary>
        public void ResetColor() 
            => Console.ResetColor();

        /// <summary>
        /// Sets the position of the cursor.
        /// </summary>
        /// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
        public void SetCursorPosition(int left, int top) 
            => Console.SetCursorPosition(left, top);

        /// <summary>
        /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteLine(object? value) 
            => Console.WriteLine(value);

        /// <summary>
        /// Writes the specified string value to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write, or null.</param>
        public void Write(object? value) 
            => Console.Write(value);

        /// <summary>
        /// Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the <see cref="Console"/> window. <see cref="true"/> to not display the pressed key; 
        /// otherwise, <see cref="false"/>.</param>
        /// <returns>An object that describes the <see cref="ConsoleKey"/> constant and Unicode character, if any, that correspond to the pressed console key. 
        /// The <see cref="ConsoleKeyInfo"/> object also describes, in a bitwise combination of <see cref="ConsoleModifiers"/> values, 
        /// whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously with the console key.</returns>
        public ConsoleKeyInfo ReadKey(bool intercept = false) 
            => Console.ReadKey(intercept);
    }
}
