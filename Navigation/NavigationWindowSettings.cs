namespace FileManager.Navigation
{
    /// <summary>
    /// Static class holding methods and settings for <see cref="INavigationWindow"/>.
    /// </summary>
    public static class NavigationWindowSettings
    {
        #region Window dimensions and cursor position settings

        public static int WindowWidth
            => Convert.ToInt32(Console.WindowWidth / 100d * 30d);

        public static int WindowHeight
            => Math.Max(Console.WindowHeight - StatusBarSettings.WindowHeight, 0);

        public static int SelectedBoxWidth
            => SelectedBoxText.Length;

        public static int StartingColumn
            => ParentWindowSettings.WindowWidth;

        public static int StartingRow
            => 0;

        #endregion

        #region Console colors settings

        public static ConsoleColor SelectedBoxColor
            => ConsoleColor.Magenta;

        private static ConsoleColor DirectoryForegroundColor
            => ConsoleColor.DarkBlue;

        private static ConsoleColor DirectoryBackgroundColor
            => Console.BackgroundColor;

        private static ConsoleColor SelectedDirectoryForegroundColor
            => ConsoleColor.White;

        private static ConsoleColor SelectedDirectoryBackgroundColor
            => ConsoleColor.DarkBlue;

        private static ConsoleColor FileForegroundColor
            => Console.ForegroundColor;

        private static ConsoleColor FileBackgroundColor
            => Console.BackgroundColor;

        private static ConsoleColor SelectedFileForegroundColor
            => ConsoleColor.White;

        private static ConsoleColor SelectedFileBackgroundColor
            => ConsoleColor.DarkBlue;

        public static void SetConsoleColors(bool isDirectory, bool isSelected)
            => (Console.ForegroundColor, Console.BackgroundColor) = GetConsoleColors(isDirectory, isSelected);

        #endregion

        #region Console colors collections

        /// <summary>
        /// A dictionary containing all colors settings for different scenarios (selected/not selected - directory/file).
        /// </summary>
        private static Dictionary<(bool isSelected, bool isDirectory), (ConsoleColor ForegroundColor, ConsoleColor BackgroundColor)> ConsoleColorsDictionary
        {
            get
            {
                return new Dictionary<(bool isSelected, bool isDirectory), (ConsoleColor ForegroundColor, ConsoleColor BackgroundColor)> 
                { 
                    { (true, true), (SelectedDirectoryForegroundColor, SelectedDirectoryBackgroundColor) },     // FileInfo is SELECTED     and a DIRECTORY
                    { (true, false), (SelectedFileForegroundColor, SelectedFileBackgroundColor) },              // FileInfo is SELECTED     and a FILE
                    { (false, true), (DirectoryForegroundColor, DirectoryBackgroundColor) },                    // FileInfo is NOT SELECTED and a DIRECTORY
                    { (false, false), (FileForegroundColor, FileBackgroundColor)}                               // FileInfo is NOT SELECTED and a FILE
                };
            }
        }

        /// <summary>
        /// Gets the foreground and backgroudn console colors for <see cref="INavigationWindow"/>.
        /// </summary>
        /// <param name="isDirectory"><see cref="true"/> if the file entry is a directory, otherwise <see cref="false"/>.</param>
        /// <param name="isSelected"><see cref="true"/> if the file entry is selected, otherwise <see cref="false"/>.</param>
        /// <returns>A <see cref="Tuple{T1, T2}"/> containing foreground and background console colors.</returns>
        public static (ConsoleColor ForegroundColor, ConsoleColor BackgroundColor) GetConsoleColors(bool isDirectory, bool isSelected)
            => ConsoleColorsDictionary[(isSelected, isDirectory)];

        #endregion

        #region Layout helper strings

        /// <summary>
        /// The message printed to console when the folder is empty.
        /// </summary>
        public static readonly string EmptyFolderMessage = $"Folder is empty!";

        /// <summary>
        /// The text to be printed inside the toggle selected box.
        /// </summary>
        public static readonly string SelectedBoxText = $" ";

        #endregion
    }
}
