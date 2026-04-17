namespace FileManager.PopUp
{
    /// <summary>
    /// Static class that holds settings for <see cref="PopUpDialog"/>.
    /// </summary>
    public static class PopUpDialogSettings
    {
        private static readonly string ConfirmationRequestShort = $"Are you sure?";
        private static readonly string ConfirmationRequestLong = $"Are you sure you want to delete the selected file(s)?";

        public static readonly string RightBorder = $" {FileManagerSettings.VerticalLine}";
        public static readonly string LeftBorder = $"{FileManagerSettings.VerticalLine} ";

        public static readonly int MinimumWindowHeight = 3;

        #region Window dimensions and cursor position settings

        /// <summary>
        /// Gets the width of <see cref="PopUpDialog"/> window.
        /// </summary>
        /// <param name="dialog">The instance of <see cref="PopUpDialog"/> for which to calculate the width.</param>
        /// <returns>The width of the <see cref="PopUpDialog"/>.</returns>
        public static int GetWindowWidth(PopUpDialog dialog)
            => Math.Min(Console.WindowWidth, dialog.GetWindowMessage().Length + LeftBorder.Length + RightBorder.Length);

        /// <summary>
        /// Height of <see cref="PopUpDialog"/> window.
        /// </summary>
        public static int WindowHeight
            => Console.WindowHeight > MinimumWindowHeight
            ? MinimumWindowHeight
            : 0;

        /// <summary>
        /// Gets the column position at which the rendering of <see cref="PopUpDialog"/> starts.
        /// </summary>
        /// <param name="dialog">The instance of <see cref="PopUpDialog"/> for which to retrieve the column position.</param>
        /// <returns>Column position at which printing starts.</returns>
        public static int GetStartingColumn(PopUpDialog dialog)
            => Math.Max((Console.WindowWidth - GetWindowWidth(dialog)) / 2, 0);

        /// <summary>
        /// The row (top) position at which the rendering of <see cref="PopUpDialog"/> starts.
        /// </summary>
        public static int StartingRow
            => (Console.WindowHeight - WindowHeight) / 2;

        #endregion

        #region Console color settings

        public static ConsoleColor ForegroundColor 
            => ConsoleColor.Magenta;

        public static ConsoleColor BackgroundColor
            => Console.BackgroundColor;

        public static ConsoleColor SelectedForegroundColor
            => BackgroundColor;

        public static ConsoleColor SelectedBackgroundColor
            => ForegroundColor;

        public static void SetConsoleColors(bool isSelected = false)
        {
            (Console.ForegroundColor, Console.BackgroundColor) = isSelected
                ? (SelectedForegroundColor, SelectedBackgroundColor)
                : (ForegroundColor, BackgroundColor);
        }

        #endregion

        #region Window layout helper strings

        /// <summary>
        /// Gets the top border for the <see cref="PopUpDialog"/>.
        /// </summary>
        /// <param name="width">The width of the <see cref="PopUpDialog"/> window.</param>
        /// <returns>The top border for the <see cref="PopUpDialog"/> window.</returns>
        public static string GetTopBorder(int width)
            => StringManipulator.GetDynamicallyGeneratedBorder(
                FileManagerSettings.TopLeftCorner,
                FileManagerSettings.TopRightCorner,
                FileManagerSettings.HorizontalLine,
                width - 2);

        /// <summary>
        /// Gets the bottom border for the <see cref="PopUpDialog"/>.
        /// </summary>
        /// <param name="width">The width of the <see cref="PopUpDialog"/> window.</param>
        /// <returns>The bottom border for the <see cref="PopUpDialog"/> window.</returns>
        public static string GetBottomBorder(int width)
            => StringManipulator.GetDynamicallyGeneratedBorder(
                FileManagerSettings.BottomLeftCorner,
                FileManagerSettings.BottomRightCorner,
                FileManagerSettings.HorizontalLine,
                width - 2);

        /// <summary>
        /// Gets the confirmation request message, depending on window width.
        /// </summary>
        /// <returns>The confirmation request message, depending on window width.</returns>
        public static string GetConfirmationRequestMessage()
        {
            return Console.WindowWidth - (LeftBorder.Length + RightBorder.Length) >= ConfirmationRequestLong.Length
                ? ConfirmationRequestLong
                : ConfirmationRequestShort;
        }

        #endregion
    }

    public enum ConfirmationOptions
    {
        Yes = 0,
        No = 1
    }

    public enum PopUpOptions
    {
        Ok = 0
    }
}
