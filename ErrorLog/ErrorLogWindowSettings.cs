namespace FileManager.ErrorLog
{
    /// <summary>
    /// Class containing layout settings for the <see cref="ErrorLogWindow"/>.
    /// </summary>
    public static class ErrorLogWindowSettings
    {
        #region Console window and cursor position settings

        public static int WindowWidth
            => Console.WindowWidth;

        public static int WindowHeight
            => Console.WindowHeight;

        public static int StartingColumn
            => 0;

        public static int StartingRow
            => 0;

        public static ConsoleColor ForegroundColor
            => Console.ForegroundColor;

        public static ConsoleColor BackgroundColor
            => Console.BackgroundColor;

        #endregion
    }
}
