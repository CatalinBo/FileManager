namespace FileManager
{
    public static class EditShortcutsWindow
    {
        #region Key mappings

        /// <summary>
        /// A <see cref="List{T}"/> containing all the action names for app's available command keys.
        /// </summary>
        private static List<string> ActionDescription => [.. EditShortcutsWindowSettings.KeyMappings.Keys];

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of all primary command keys available in the application.
        /// </summary>
        private static IEnumerable<ConsoleKey> PrimaryKeys => EditShortcutsWindowSettings.KeyMappings.Values.Select(list => list.First());

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of all secondary command keys available in the application.
        /// </summary>
        private static IEnumerable<ConsoleKey> SecondaryKeys => EditShortcutsWindowSettings.KeyMappings.Values.Select(list => list.Last());

        #endregion

        public static Task SetNewShortcuts()
        {
            MyResetEventSlims.EditShortcutsResetEvent.Set();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            break;
                        default:
                            break;
                    }

                    MyResetEventSlims.EditShortcutsResetEvent.Set();
                }
            }
        }

        public static void PrintLayout()
        {
            if (EditShortcutsWindowSettings.WindowHeight >= EditShortcutsWindowSettings.MinimumWindowHeight)
            {
                SetCursorPosition();
                SetConsoleColors();

                MyConsole.Write(EditShortcutsWindowSettings.TopBorder);
                Console.CursorTop++;
                Console.CursorLeft = EditShortcutsWindowSettings.StartingColumn;
                MyConsole.Write(EditShortcutsWindowSettings.BottomBorder);
            }
        }

        #region Cursor parameters initialization

        /// <summary>
        /// Sets the cursor coordinates to their initial values, as set in <see cref="EditShortcutsWindowSettings"/>.
        /// </summary>
        private static void SetCursorPosition() => MyConsole.SetCursorPosition(EditShortcutsWindowSettings.StartingColumn, EditShortcutsWindowSettings.StartingRow);

        /// <summary>
        /// Sets the foreground and background console colors.
        /// </summary>
        private static void SetConsoleColors()
        {
            Console.ForegroundColor = EditShortcutsWindowSettings.ForegroundColor;
            Console.BackgroundColor = EditShortcutsWindowSettings.BackgroundColor;
        }

        #endregion
    }
}
