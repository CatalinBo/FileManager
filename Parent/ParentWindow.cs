namespace FileManager.Parent
{
    /// <summary>
    /// Holds data and methods used for generating and printing the <see cref="INavigationWindow"/>'s parent window.
    /// </summary>
    public class ParentWindow
    {
        private readonly INavigationWindow currentNavigationWindow;
        private readonly INavigationWindow? parentNavigationWindow;
        private readonly FileSystem fileSystem;
        private readonly IFileInfo? currentFile;
        private readonly IBackgroundTasksWindow? backgroundTasksWindow;
        private readonly IEnumerable<IFileInfo> windowContents;

        /// <summary>
        /// Provides methods for <see cref="ParentWindow"/> layout generator.
        /// </summary>
        /// <param name="navigationWindow">The instance of <see cref="INavigationWindow"/> on which the current <see cref="ParentWindow"/> will be constructed.</param>
        /// <param name="backgroundTasksWindow">The instance of <see cref="IBackgroundTasksWindow"/> that will affect the current <see cref="ParentWindow"/> height.</param>
        public ParentWindow(INavigationWindow navigationWindow, IBackgroundTasksWindow? backgroundTasksWindow)
        {
            fileSystem = new FileSystem();
            currentNavigationWindow = navigationWindow;
            currentFile = currentNavigationWindow.GetCurrentFile();
            parentNavigationWindow = currentNavigationWindow.GetParentAsNavigationWindow();
            this.backgroundTasksWindow = backgroundTasksWindow;
            windowContents = GetWindowContents();
        }

        /// <summary>
        /// Height of <see cref="ParentWindow"/>.
        /// </summary>
        private int WindowHeight
        {
            get
            {
                return backgroundTasksWindow is null
                    ? ParentWindowSettings.WindowHeight
                    : ParentWindowSettings.WindowHeight - backgroundTasksWindow.WindowHeight;
            }
        }

        #region Window layout printer methods

        /// <summary>
        /// Prints the entire <see cref="ParentWindow"/> layout, by calling helper methods and clearing empty lines.
        /// </summary>
        public void PrintLayout()
        {
            ConsoleManager.InitializeCursorPosition(ParentWindowSettings.StartingColumn, ParentWindowSettings.StartingRow);
            PrintWindowContent();
            StringManipulator.PrintFillerRows(windowContents.Count(), ParentWindowSettings.WindowWidth, WindowHeight, Console.CursorLeft, Console.CursorTop);
        }

        /// <summary>
        /// Prints current <see cref="ParentWindow"/> contents.
        /// </summary>
        private void PrintWindowContent()
        {
            if (parentNavigationWindow is null)
            {
                PrintRootPath();
                return;
            }

            foreach (var entry in windowContents)
            {
                ParentWindowSettings.SetConsoleColors(fileSystem.Directory.Exists(entry.FullName), currentFile!.Name.Equals(entry.Name));
                Console.Write(StringManipulator.TruncateString(entry.Name, ParentWindowSettings.WindowWidth));
                ConsoleManager.JumpToNewLine(ParentWindowSettings.StartingColumn);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Prints the root path of the current instance of <see cref="INavigationWindow"/>.
        /// </summary>
        private void PrintRootPath()
        {
            var rootPath = fileSystem.Directory.GetDirectoryRoot(Environment.CurrentDirectory);

            ParentWindowSettings.SetConsoleColors(true, true);
            Console.Write(StringManipulator.TruncateString(rootPath, ParentWindowSettings.WindowWidth));
            ConsoleManager.JumpToNewLine(ParentWindowSettings.StartingColumn);
            Console.ResetColor();
        }

        #endregion

        #region Methods used to generate window contents.

        /// <summary>
        /// Gets the contents of the current parent folder.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{IFileInfo}"/> containing the files and directories of the current parent folder.</returns>
        private IEnumerable<IFileInfo> GetWindowContents()
        {
            return parentNavigationWindow is null
                ? []
                : parentNavigationWindow.GetWindowSubset().Take(WindowHeight);
        }

        #endregion
    }
}
