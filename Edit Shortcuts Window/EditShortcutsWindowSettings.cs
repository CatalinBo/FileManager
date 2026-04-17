namespace FileManager
{
    /// <summary>
    /// Implementation of the <see cref="EditShortcutsWindowSettings"/> class.
    /// </summary>
    /// <remarks>This class contains layouts settings for the <see cref="EditShortcutsWindow"/>.</remarks>
    public class EditShortcutsWindowSettings
    {
        private const int ExtraWindowElements = 6;                      // Elements that add up to total Window Height, like TopBorder, BottomBorder, Table Header and empty rows.

        #region Key mappings

        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing the type of action and the keys available in the application.
        /// </summary>
        /// <remarks>The returned format is set in <see cref="CommandKeys.GetKeyMappingsInEditShortcutsWindowFormat"/> method.</remarks>
        public static Dictionary<string, List<ConsoleKey>> KeyMappings => CommandKeys.GetKeyMappingsInEditShortcutsWindowFormat();

        #endregion

        #region Window dimensions settings

        /// <summary>
        /// Total <see cref="EditShortcutsWindow"/> width.
        /// </summary>
        public static int WindowWidth => GetWindowSize().Width;

        /// <summary>
        /// Total <see cref="EditShortcutsWindow"/> height.
        /// </summary>
        public static int WindowHeight => GetWindowSize().Height;

        /// <summary>
        /// The width of the action description column inside <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static int ActionDescriptionWindowWidth => GetInsideWindowWidths().DescriptionWidth;

        /// <summary>
        /// The width of the primary key column inside <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static int PrimaryKeyWindowWidth => GetInsideWindowWidths().PrimaryWidth;

        /// <summary>
        /// The width of the secondary key column inside <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static int SecondaryKeyWindowWidth => GetInsideWindowWidths().SecondaryWidth;

        /// <summary>
        /// The minimum height under which the <see cref="EditShortcutsWindow"/> is not displayed.
        /// </summary>
        public static int MinimumWindowHeight => ExtraWindowElements;

        /// <summary>
        /// Calculates the necessary window dimensions (width and height):
        /// </summary>
        /// <returns>A <see cref="Tuple{T1, T2}"/> containing the width and height of the <see cref="EditShortcutsWindow"/>.</returns>
        /// <remarks>
        /// <list type="bullet">
        /// <item>Width takes into consideration the <see cref="Console.WindowWidth"/> and is dynamically generated.</item>
        /// <item>Height takes into consideration application's available keys and the required borders.</item>
        /// </list>
        /// </remarks>
        private static (int Width, int Height) GetWindowSize()
        {
            var width = Convert.ToInt32(Console.WindowWidth / 100d * 50d);
            var height = KeyMappings.Count + ExtraWindowElements;

            return (Width: width, Height: height);
        }

        /// <summary>
        /// Calculates the necessary widths of inside windows (action description, primary key and secondary key windows).
        /// </summary>
        /// <returns>A <see cref="Tuple{T1, T2, T3}"/> containing the required widths of the inside windows.</returns>
        private static (int DescriptionWidth, int PrimaryWidth, int SecondaryWidth) GetInsideWindowWidths()
        {
            var layoutHelpersWidths = LeftBorder.Length + (InsideVerticalBorder.Length * 2) + RightBorder.Length;
            var descriptionWidth = Convert.ToInt32((WindowWidth - layoutHelpersWidths) / 2);
            var primaryWidth = Convert.ToInt32((WindowWidth - layoutHelpersWidths) / 2 / 2);
            var secondaryWidth = primaryWidth;

            return (DescriptionWidth: descriptionWidth, PrimaryWidth: primaryWidth, SecondaryWidth: secondaryWidth);
        }

        #endregion

        #region Cursor position settings

        /// <summary>
        /// The default left position of the cursor for <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static int StartingColumn => Math.Max(Convert.ToInt32((Console.WindowWidth - WindowWidth) / 2), 0);

        /// <summary>
        /// The default top position of the cursor for <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static int StartingRow => Math.Max(Convert.ToInt32((Console.WindowHeight - WindowHeight) / 2), 0);

        #endregion

        #region Console colors settings

        /// <summary>
        /// The foreground console color to use in <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static ConsoleColor ForegroundColor => ConsoleColor.Green;

        /// <summary>
        /// The background console color to use in <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static ConsoleColor BackgroundColor => Console.BackgroundColor;

        /// <summary>
        /// The foreground console color to use in <see cref="EditShortcutsWindow"/> when dealing with a selected field.
        /// </summary>
        /// <remarks>Used to highlight the current selected field for editing.</remarks>
        public static ConsoleColor SelectedForegroundColor => BackgroundColor;

        /// <summary>
        /// The background console color for <see cref="EditShortcutsWindow"/> when dealing with a selected field.
        /// </summary>
        /// <remarks>Used to highlight the current selected field for editing.</remarks>
        public static ConsoleColor SelectedBackgroundColor => ForegroundColor;

        #endregion

        #region Window layout helper strings

        /// <summary>
        /// Used as the top border of the <see cref="EditShortcutsWindow"/>.
        /// </summary>
        /// <remarks>This border is dynamically generated, depending on the <see cref="EditShortcutsWindow.WindowWidth"/>.</remarks>
        public static string TopBorder 
            => $"{FileManagerSettings.TopLeftCorner}" +
            $"{string.Concat(Enumerable.Repeat(FileManagerSettings.HorizontalLine, WindowWidth - 2))}" +
            $"{FileManagerSettings.TopRightCorner}";

        /// <summary>
        /// Used as the bottom border of the <see cref="EditShortcutsWindow"/>.
        /// </summary>
        /// <remarks>This border is dynamically generated, depending on the <see cref="EditShortcutsWindow.WindowWidth"/>.</remarks>
        public static string BottomBorder 
            => $"{FileManagerSettings.BottomLeftCorner}" +
            $"{string.Concat(Enumerable.Repeat(FileManagerSettings.HorizontalLine, WindowWidth - 2))}" +
            $"{FileManagerSettings.BottomRightCorner}";

        /// <summary>
        /// Used as the left border for table lines in <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static string LeftBorder => $"{FileManagerSettings.VerticalLine} ";

        /// <summary>
        /// Used as the right border for table lines in <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static string RightBorder => $" {FileManagerSettings.VerticalLine}";

        /// <summary>
        /// Used as a separator between columns in <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static string InsideVerticalBorder => $" {FileManagerSettings.VerticalLine} ";

        /// <summary>
        /// Used as an inside table header for the column with action description in <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static string ActionNameHeader => $"ACTION" ;

        /// <summary>
        /// Used as an inside table header for the column with primary keys in <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static string PrimaryKeyHeader => $"PRIMARY";

        /// <summary>
        /// Used as an inside table header for the column with secondary keys in <see cref="EditShortcutsWindow"/>.
        /// </summary>
        public static string SecondaryKeyHeader => $"SECONDARY";

        /// <summary>
        /// Used as a header message in <see cref="EditShortcutsWindow"/> when the user is not required to press a key to assign it to a certain action.
        /// </summary>
        public static string DefaultMessage => $"Press ENTER to edit the selected key. ESCAPE to exit.";

        /// <summary>
        /// Used as a header message in <see cref="EditShortcutsWindow"/> when the user is required to press a key to assign to a certain action.
        /// </summary>
        public static string WaitingForInputMessage => $"Press a key to assign to current action.";

        #endregion
    }
}
