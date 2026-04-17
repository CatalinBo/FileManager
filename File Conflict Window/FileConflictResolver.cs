namespace FileManager
{
    /// <summary>
    /// Enum containing options available when encountering conflicts on deletion.
    /// </summary>
    internal enum DeleteConflictOptions
    {
        TryAgain,
        Cancel
    }

    /// <summary>
    /// Enum containing options available when encountering conflicts on moving.
    /// </summary>
    internal enum MoveConflictOptions
    {
        Overwrite,
        KeepBoth,
        Skip
    }

    /// <summary>
    /// Enum containing options available when encountering conflicts on copying.
    /// </summary>
    internal enum CopyConflictOptions
    {
        Overwrite,
        KeepBoth,
        Skip
    }

    /// <summary>
    /// Enum containing options available when encountering conflicts on renaming.
    /// </summary>
    internal enum RenameConflictOptions
    {
        Overwrite,
        Cancel
    }

    /// <summary>
    /// Implementation of the <see cref="FileConflictResolver"/> class.
    /// </summary>
    internal class FileConflictResolver
    {
        private List<(string SourcePath, string DestinationPath)> conflictingFiles;
        private readonly ConsoleKeyInfo commandKey;
        private readonly Type enumType;

        /// <summary>
        /// Initializes a new instance of <see cref="FileConflictResolver"/>.
        /// </summary>
        /// <param name="key">The key that represents the command given to the <see cref="FileManager"/>.</param>
        internal FileConflictResolver(ConsoleKeyInfo key)
        {
            conflictingFiles = new List<(string SourcePath, string DestinationPath)>();
            commandKey = key;
            enumType = InitializeEnum();
        }

        /// <summary>
        /// The width of the <see cref="FileConflictResolver"/> window.
        /// </summary>
        private int WindowWidth { get; set; } = FileConflictResolverSettings.WindowWidth;

        /// <summary>
        /// The height of the <see cref="FileConflictResolver"/> window.
        /// </summary>
        private int WindowHeight { get; set; } = FileConflictResolverSettings.WindowHeight;

        /// <summary>
        /// The column position at which the printing of the <see cref="FileConflictResolver"/> starts.
        /// </summary>
        private static int PrintingColumn
            => MyConsole.GetCursorPosition().Column;

        /// <summary>
        /// The row position at which the printing of the <see cref="FileConflictResolver"/> window starts.
        /// </summary>
        private static int PrintingRow
            => MyConsole.GetCursorPosition().Row;

        /// <summary>
        /// The index of the currently selected option.
        /// </summary>
        private int SelectedOption { get; set; } = 0;

        /// <summary>
        /// Gets the list of available options.
        /// </summary>
        private List<string> Options
            => FileConflictResolverSettings.GetOptions(enumType);

        /// <summary>
        /// Gets the label message for the <see cref="FileConflictResolver"/>.
        /// </summary>
        private string Message
            => FileConflictResolverSettings.GetFileConflictMessage(enumType);

        /// <summary>
        /// Gets the number of options available.
        /// </summary>
        private int MaxOptions
            => Options.Count;

        /// <summary>
        /// Adds a conflicting file to a list of conflicting files.
        /// </summary>
        /// <param name="sourcePath">The source path of the conflicting file.</param>
        /// <param name="destinationPath">The destination path of the conflicting file.</param>
        public void AddConflictingFile(string sourcePath, string destinationPath = "")
            => conflictingFiles.Add((sourcePath, destinationPath));

        /// <summary>
        /// Removes the conflicting file from the list of conflicting files.
        /// </summary>
        /// <param name="sourcePath">The source path of the conflicting file.</param>
        /// <param name="destinationPath">The destination path of the conflicting file.</param>
        public void RemoveConflictingFile(string sourcePath, string destinationPath = "")
            => conflictingFiles.Remove((sourcePath, destinationPath));

        /// <summary>
        /// Gets the list of conflicting files.
        /// </summary>
        /// <returns>A list of conflicting files.</returns>
        public List<(string SourcePath, string DestinationPath)> GetConflictingFilesList()
            => conflictingFiles;

        /// <summary>
        /// Gets the selected option when encountering conflicting files.
        /// </summary>
        /// <typeparam name="T">Type of options enum.</typeparam>
        /// <returns>The selected option.</returns>
        public Task<T> GetFileResolveOption<T>() where T : Enum
        {
            //MyResetEventSlims.BackgroundResetEvent.Set();
            //MyResetEventSlims.SetBackground();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            SelectedOption = (SelectedOption - 1 + MaxOptions) % MaxOptions;
                            break;
                        case ConsoleKey.DownArrow:
                            SelectedOption = (SelectedOption + 1) % MaxOptions;
                            break;
                        case ConsoleKey.Enter:
                            return Task.FromResult((T)(object)SelectedOption);
                        default:
                            continue;
                    }

                    //MyResetEventSlims.BackgroundResetEvent.Set();
                    //MyResetEventSlims.SetBackground();
                }

                if (WindowSizeHasChanged())
                {
                    HandleWindowSizeChange();
                }

                //Thread.Sleep(FileManagerSettings.ThreadSleepTime);
            }
        }

        /// <summary>
        /// Prints the entire <see cref="FileConflictResolver"/> layout.
        /// </summary>
        public void PrintLayout()
        {
            if (WindowHeight >= FileConflictResolverSettings.MinimumWindowHeight)
            {
                SetCursorPosition();

                PrintTopBorder();
                PrintMessage();

                for (int i = 0; i < Options.Count; i++)
                {
                    bool isSelected = i == SelectedOption;
                    MyConsole.SetCursorPosition(PrintingColumn, PrintingRow);
                    PrintOption(isSelected, i);
                }

                PrintBottomBorder();

                MyConsole.ResetCursorPosition();
            }

            void PrintTopBorder()
                => PrintLine(FileConflictResolverSettings.TopBorder);

            void PrintMessage()
                => PrintLine(Message);

            void PrintOption(bool isSelected, int index)
                => PrintLine(FileConflictResolverSettings.GetOptionMessage(Options.ElementAt(index)), isSelected);

            void PrintBottomBorder()
                => PrintTopBorder();
        }

        /// <summary>
        /// Prints the given <see cref="string"/> params on a separate line, inside the <see cref="FileConflictResolver"/>.
        /// </summary>
        /// <param name="message">Message to be printed on line.</param>
        /// <param name="isSelected"><see cref="true"/> if the line is selected, otherwise <see cref="false"/>.</param>
        private static void PrintLine(string message, bool isSelected = false)
        {
            SetConsoleColors(isSelected);
            MyConsole.SetCursorPosition(PrintingColumn, PrintingRow);
            MyConsole.Write(message);
            MyConsole.IncrementRow();
            MyConsole.ResetColor();
        }

        /// <summary>
        /// Gets the Conflict Options enum, depending on the <see cref="commandKey"/>.
        /// </summary>
        /// <returns><see cref="Enum"/> containing all file conflict options.</returns>
        /// <exception cref="ArgumentException"></exception>
        private Type InitializeEnum()
        {
            return commandKey.Key switch
            {
                ConsoleKey.Delete => typeof(DeleteConflictOptions),
                ConsoleKey.X => typeof(MoveConflictOptions),
                ConsoleKey.C => typeof(CopyConflictOptions),
                ConsoleKey.Y => typeof(CopyConflictOptions),
                ConsoleKey.F2 => typeof(RenameConflictOptions),
                _ => throw new ArgumentException("Key not accepted!")
            };
        }

        /// <summary>
        /// Updates <see cref="WindowWidth"/> and <see cref="WindowHeight"/> with their current state.
        /// </summary>
        private void UpdateWindowSize()
        {
            WindowWidth = FileConflictResolverSettings.WindowWidth;
            WindowHeight = FileConflictResolverSettings.WindowHeight;
        }

        /// <summary>
        /// Checks if the window size has changed.
        /// </summary>
        /// <returns><see cref="true"/> if the window size has changed, otherwise <see cref="false"/>.</returns>
        private bool WindowSizeHasChanged()
        {
            return FileConflictResolverSettings.WindowWidth != WindowWidth
                || FileConflictResolverSettings.WindowHeight != WindowHeight;
        }

        /// <summary>
        /// Handles the window size change event to update the layout and cursor position accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains no event data.</param>
        private void HandleWindowSizeChange()
        {
            UpdateWindowSize();
            //MyResetEventSlims.BackgroundResetEvent.Set();
            //MyResetEventSlims.SetBackground();
        }

        /// <summary>
        /// Sets the cursor position to its default values, as set in <see cref="FileConflictResolverSettings"/>.
        /// </summary>
        private static void SetCursorPosition()
            => MyConsole.SetCursorPosition(FileConflictResolverSettings.StartingColumn, FileConflictResolverSettings.StartingRow);

        /// <summary>
        /// Sets the console colors for <see cref="FileConflictResolver"/>.
        /// </summary>
        /// <param name="isSelected"><see cref="true"/> if the option is selected, otherwise <see cref="false"/>.</param>
        private static void SetConsoleColors(bool isSelected = false)
        {
            MyConsole.ForegroundColor = FileConflictResolverSettings.GetConsoleColors(isSelected).ForegroundColor;
            MyConsole.BackgroundColor = FileConflictResolverSettings.GetConsoleColors(isSelected).BackgroundColor;
        }
    }
}
