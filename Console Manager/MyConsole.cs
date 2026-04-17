namespace FileManager
{
    /// <summary>
    /// Provides a centralized manager for <see cref="Console"/> interaction, allowing for easy mocking and testing.
    /// </summary>
    public static class MyConsole
    {
        private static IConsoleWrapper consoleWrapper;
        private static int columnIndex;
        private static int rowIndex;
        private static int foregroundColor;
        private static int backgroundColor;

        /// <summary>
        /// Initializes the <see cref="MyConsole"/> with the default <see cref="ConsoleWrapper"/>.
        /// </summary>
        static MyConsole()
        {
            consoleWrapper = new ConsoleWrapper();
        }

        /// <summary>
        /// Gets the width of the console window.
        /// </summary>
        public static int WindowWidth 
            => consoleWrapper.WindowWidth;

        /// <summary>
        /// Gets the height of the console window.
        /// </summary>
        public static int WindowHeight 
            => consoleWrapper.WindowHeight;

        /// <summary>
        /// Gets or sets the foreground color of the console.
        /// </summary>
        public static ConsoleColor ForegroundColor
        {
            get => consoleWrapper.ForegroundColor;
            set
            {
                Interlocked.Exchange(ref foregroundColor, (int)value);
                consoleWrapper.ForegroundColor = (ConsoleColor)foregroundColor;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the console.
        /// </summary>
        public static ConsoleColor BackgroundColor
        {
            get => consoleWrapper.BackgroundColor;
            set
            {
                Interlocked.Exchange(ref backgroundColor, (int)value);
                consoleWrapper.BackgroundColor = (ConsoleColor)backgroundColor;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cursor is visible.
        /// </summary>
        public static bool CursorVisible
        {
            get => consoleWrapper.CursorVisible;
            set => consoleWrapper.CursorVisible = value;
        }

        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        public static bool KeyAvailable 
            => consoleWrapper.KeyAvailable;

        /// <summary>
        /// Sets the foreground and background console colors to their default values.
        /// </summary>
        public static void ResetColor()
        {
            consoleWrapper.ResetColor();
            Interlocked.Exchange(ref foregroundColor, (int)consoleWrapper.ForegroundColor);
            Interlocked.Exchange(ref backgroundColor, (int)consoleWrapper.BackgroundColor);
        }

        /// <summary>
        /// Sets the position of the cursor.
        /// </summary>
        /// <param name="left">The column position of the cursor. Columns are numbered from left to right starting at 0.</param>
        /// <param name="top">The row position of the cursor. Rows are numbered from top to bottom starting at 0.</param>
        public static void SetCursorPosition(int left, int top)
        {
            SetColumnValue(left);
            SetRowValue(top);
            consoleWrapper.SetCursorPosition(columnIndex, rowIndex);
        }

        /// <summary>
        /// Resets the cursor position to its default values, which is 0 for column and 0 for row.
        /// </summary>
        public static void ResetCursorPosition() 
            => SetCursorPosition(0, 0);

        /// <summary>
        /// Provides atomic incrementation of the column position of the cursor.
        /// </summary>
        /// <remarks>This method increments the current column position of the cursor by one. It is thread-safe due to the use of atomic operations via <see cref="Interlocked"/> 
        /// to ensure safe updates. Ensure that the column position does not exceed the console's maximum value.</remarks>
        public static void IncrementColumn() 
            => Interlocked.Increment(ref columnIndex);

        /// <summary>
        /// Provides atomic incrementation of the row position of the cursor.
        /// </summary>
        /// <remarks>This method increments the current row position of the cursor by one. It is thread-safe due to the use of atomic operations via <see cref="Interlocked"/> 
        /// to ensure safe updates. Ensure that the row position does not exceed the console's maximum value.</remarks>
        public static void IncrementRow() 
            => Interlocked.Increment(ref rowIndex);

        /// <summary>
        /// Provides atomic decrementation of the column position of the cursor.
        /// </summary>
        /// <remarks>This method decrements the current column position of the cursor by one. It is thread-safe due to the use of atomic operations via <see cref="Interlocked"/> 
        /// to ensure safe updates. Ensure that the column position does not go below zero.</remarks>
        public static void DecrementColumn() 
            => Interlocked.Decrement(ref columnIndex);

        /// <summary>
        /// Provides atomic decrementation of the row position of the cursor.
        /// </summary>
        /// <remarks>This method decrements the current row position of the cursor by one. It is thread-safe due to the use of atomic operations 
        /// via <see cref="Interlocked"/> to ensure safe updates. 
        /// Ensure that the row position does not go below zero.</remarks>
        public static void DecrementRow() 
            => Interlocked.Decrement(ref rowIndex);

        /// <summary>
        /// Provides an atomic setter for the column position of the cursor.
        /// </summary>
        /// <param name="value">New value to replace the column position of the cursor. Ensure this value is within the valid range for the console.</param>
        /// <remarks>This method is thread-safe due to the use of atomic operations via <see cref="Interlocked"/> to ensure safe updates.</remarks>
        public static void SetColumnValue(int value) 
            => Interlocked.Exchange(ref columnIndex, value);

        /// <summary>
        /// Provides an atomic setter for the row position of the cursor.
        /// </summary>
        /// <param name="value">New value to replace the row position of the cursor. Ensure this value is within the valid range for the console.</param>
        /// <remarks>This method is thread-safe due to the use of atomic operations via <see cref="Interlocked"/> to ensure safe updates.</remarks>
        public static void SetRowValue(int value) 
            => Interlocked.Exchange(ref rowIndex, value);

        /// <summary>
        /// Gets the current position of the console cursor.
        /// </summary>
        /// <returns>A value tuple containing the coordinates of the cursor's current position: <c>Column</c> and <c>Row</c>.</returns>
        public static (int Column, int Row) GetCursorPosition() 
            => (Column: columnIndex, Row: rowIndex);

        /// <summary>
        /// Writes the text representation of the specified object, followed by the current line terminator, to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write. If <see langword="null"/>, an empty line is written.</param>
        public static void WriteLine(object? value) 
            => consoleWrapper.WriteLine(value);

        /// <summary>
        /// Writes the text representation of the specified object to the standard output stream.
        /// </summary>
        /// <param name="value">The value to write, or <see langword="null"/>. If <see langword="null"/>, an empty string is written.</param>
        public static void Write(object? value) 
            => consoleWrapper.Write(value);

        /// <summary>
        /// Obtains the next character or function key pressed by the user.
        /// </summary>
        /// <param name="intercept">If set to <see langword="true"/>, the pressed key will not be displayed in the <see cref="Console"/> window; 
        /// otherwise, it will be displayed.</param>
        /// <returns>An object that describes the <see cref="ConsoleKey"/> constant and Unicode character, if any, that correspond to the pressed console key. 
        /// The <see cref="ConsoleKeyInfo"/> object also describes, in a bitwise combination of <see cref="ConsoleModifiers"/> values, 
        /// whether one or more Shift, Alt, or Ctrl modifier keys were pressed simultaneously with the console key.</returns>
        public static ConsoleKeyInfo ReadKey(bool intercept = false) 
            => consoleWrapper.ReadKey(intercept);

        /// <summary>
        /// Overrides the current <see cref="ConsoleWrapper"/> with a new one.
        /// </summary>
        /// <param name="newConsoleWrapper">The new <see cref="ConsoleWrapper"/> to replace the old <see cref="ConsoleWrapper"/> with.</param>
        public static void OverrideConsoleWrapper(IConsoleWrapper newConsoleWrapper)
            => consoleWrapper = newConsoleWrapper;
    }
}
