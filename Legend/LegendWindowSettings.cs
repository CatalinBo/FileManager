namespace FileManager.Legend
{
    /// <summary>
    /// Class containing settings for <see cref="LegendWindowSettings"/>.
    /// </summary>
    public static class LegendWindowSettings
    {
        /// <summary>
        /// Height below which the <see cref="LegendWindow"/> is not displayed anymore.
        /// </summary>
        public static readonly int MinimumWindowHeight = 3;

        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing all command keys, in a specific <see cref="LegendWindow"/> format.
        /// </summary>
        public static Dictionary<string, string> KeyMappings
            => CommandKeys.GetKeyMappingsInHelpWindowFormat();

        #region Window dimensions and cursor position settings

        public static int WindowWidth
            => GetWindowSize().Width;

        public static int WindowHeight
            => GetWindowSize().Height;

        public static int StartingColumn
            => Math.Max(Convert.ToInt32((Console.WindowWidth - GetWindowSize().Width) / 2), 0);

        public static int StartingRow
            => Math.Max(Convert.ToInt32((Console.WindowHeight - GetWindowSize().Height) / 2), 0);

        #endregion

        #region Console color settings

        public static ConsoleColor ForegroundColor
            => ConsoleColor.Green;

        public static ConsoleColor BackgroundColor
            => Console.BackgroundColor;

        #endregion

        #region Layout border settings

        public static string TopBorder
            => StringManipulator.GetDynamicallyGeneratedBorder(
                FileManagerSettings.TopLeftCorner,
                FileManagerSettings.TopRightCorner,
                FileManagerSettings.HorizontalLine,
                WindowWidth - 2);
        
        public static string BottomBorder
            => StringManipulator.GetDynamicallyGeneratedBorder(
                FileManagerSettings.BottomLeftCorner,
                FileManagerSettings.BottomRightCorner,
                FileManagerSettings.HorizontalLine,
                WindowWidth - 2);

        public static string LeftBorder
            => $"{FileManagerSettings.VerticalLine} ";

        public static string RightBorder
            => $" {FileManagerSettings.VerticalLine}";

        public static string ColumnSeparator
            => $" {FileManagerSettings.HorizontalLine} ";

        #endregion

        #region Layout generator helper methods

        /// <summary>
        /// Gets the required window size for the <see cref="LegendWindow"/>.
        /// </summary>
        /// <returns>A <see cref="Tuple{T1, T2}"/> containing width and height of <see cref="LegendWindow"/>.</returns>
        private static (int Width, int Height) GetWindowSize()
        {
            var layoutHelpers = GetLayoutHelpersLength();
            var maxKeyLength = GetMaxKeyNameLength();
            var maxValueLength = GetMaxKeyInfoLength();
            var borders = 2;
            var contents = KeyMappings.Keys.Count;

            var width = layoutHelpers + maxKeyLength + maxValueLength;
            var height = borders + contents;

            width = Math.Min(width, Console.WindowWidth);
            height = Math.Min(height, Console.WindowHeight);

            return (width, height);
        }

        /// <summary>
        /// Gets the length of all layout helper strings.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the sum of layout helper strings lengths.</returns>
        private static int GetLayoutHelpersLength()
            => LeftBorder.Length + RightBorder.Length + ColumnSeparator.Length;

        /// <summary>
        /// Gets the length of the longest key name.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the length of the longest key name.</returns>
        public static int GetMaxKeyNameLength()
            => KeyMappings.Keys.Max(key => key.ToString().Length);

        /// <summary>
        /// Gets the length of the longest key description.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the length of the longest key description.</returns>
        public static int GetMaxKeyInfoLength()
            => KeyMappings.Values.Max(value => value.Length);

        #endregion
    }
}
