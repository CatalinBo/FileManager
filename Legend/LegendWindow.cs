namespace FileManager.Legend
{
    /// <summary>
    /// Static class that provides methods for generating and printing window layout.
    /// </summary>
    public static class LegendWindow
    {
        public static bool IsCalled { get; private set; } = false;

        /// <summary>
        /// <see cref="Dictionary{TKey, TValue}"/> containing all command keys, with a short description of the key, processed to fit the window.
        /// </summary>
        private static Dictionary<string, string> KeyMappings
            => GetKeyMappings();

        /// <summary>
        /// Renders the complete layout of the current <see cref="LegendWindow"/> by calling helper methods to print contents.
        /// </summary>
        public static void PrintLayout()
        {
            if (LegendWindowSettings.WindowHeight < LegendWindowSettings.MinimumWindowHeight)
            {
                return;
            }

            ConsoleManager.InitializeCursorPosition(LegendWindowSettings.StartingColumn, LegendWindowSettings.StartingRow);
            ConsoleManager.InitializeConsoleColors(LegendWindowSettings.ForegroundColor, LegendWindowSettings.BackgroundColor);

            PrintTopBorder();
            PrintContents();
            PrintBottomBorder();

            Console.ResetColor();

            static void PrintTopBorder()
            {
                Console.Write(LegendWindowSettings.TopBorder);
                ConsoleManager.JumpToNewLine(LegendWindowSettings.StartingColumn);
            }

            static void PrintContents()
            {
                foreach (var key in KeyMappings)
                {
                    var keyName = StringManipulator.GetCenteredText(key.Key, LegendWindowSettings.GetMaxKeyNameLength());
                    var keyInfo = StringManipulator.GetCenteredText(key.Value, LegendWindowSettings.GetMaxKeyInfoLength());

                    Console.Write($"{LegendWindowSettings.LeftBorder}{keyName}{LegendWindowSettings.ColumnSeparator}{keyInfo}{LegendWindowSettings.RightBorder}");
                    ConsoleManager.JumpToNewLine(LegendWindowSettings.StartingColumn);
                }
            }

            static void PrintBottomBorder()
                => Console.Write(LegendWindowSettings.BottomBorder);
        }

        public static void ToggleWindow()
            => IsCalled = !IsCalled;

        public static void CloseWindow()
            => IsCalled = false;

        /// <summary>
        /// Processes all command keys and their description, making them fit inside the window.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> containing all command keys, with a short description of the key.</returns>
        private static Dictionary<string, string> GetKeyMappings()
        {
            return (Console.WindowHeight <= LegendWindowSettings.WindowHeight)
                ? LegendWindowSettings.KeyMappings.Take(LegendWindowSettings.WindowHeight - 2).ToDictionary(key => key.Key, value => value.Value)
                : LegendWindowSettings.KeyMappings;
        }
    }
}
