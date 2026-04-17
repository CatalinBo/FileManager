namespace FileManager.Navigation
{
    /// <summary>
    /// Provides methods and properties to allow for navigating through files and folders.
    /// </summary>
    public class NavigationWindow : INavigationWindow
    {
        private readonly IFileSystem fileSystem;
        private string currentFilePath;
        private bool showHiddenFiles;
        private int selectedFileIndex;
        private bool multipleSelection;

        /// <summary>
        /// Initializes a new <see cref="NavigationWindow"/>, based on the given path.
        /// </summary>
        /// <param name="currentFilePath">Path on which the current instance of <see cref="INavigationWindow"/> will be based on.</param>
        public NavigationWindow(IFileSystem fileSystem, string currentFilePath = "")
        {
            this.fileSystem = fileSystem;
            this.currentFilePath = currentFilePath.Equals("")
                ? Environment.CurrentDirectory
                : currentFilePath;
            showHiddenFiles = false;
            multipleSelection = false;
            RefreshFileEntriesList();
            RefreshSelectedIndexes();
        }

        private IFileInfo? SelectedFile
            => FileEntries.ElementAtOrDefault(selectedFileIndex);

        /// <summary>
        /// A range of indexes that represents the starting and ending position of the file entries that must be printed to console.
        /// </summary>
        private (int RangeStart, int RangeEnd) SubsetIndexes { get; set; } = new(0, NavigationWindowSettings.WindowHeight - 1);

        /// <summary>
        /// An <see cref="IEnumerable{IFileInfo}"/> containing the file entries in the current folder.
        /// </summary>
        private List<IFileInfo> FileEntries { get; set; } = [];

        private Dictionary<IFileInfo, bool> MultipleSelectedFiles { get; set; } = [];

        private int SelectedFileRow { get; set; } = 0;

        private IFileInfo? GetCurrentFile()
            => fileSystem.FileInfo.New(currentFilePath);

        private IEnumerable<IFileInfo> GetWindowSubset()
        {
            AdjustRangeForFileCount();
            AdjustRangeForWindowSize();
            AdjustRangeForSelectedFileIndex();

            return NavigationWindowSettings.WindowHeight <= 1
                ? [FileEntries.ElementAt(selectedFileIndex)]
                : FileEntries.Skip(SubsetIndexes.RangeStart).Take(NavigationWindowSettings.WindowHeight);
        }

        private INavigationWindow? GetParentAsNavigationWindow()
        {
            return currentFilePath.Equals(fileSystem.Directory.GetDirectoryRoot(Environment.CurrentDirectory))
                ? null
                : new NavigationWindow(fileSystem, fileSystem.Directory.GetParent(currentFilePath)!.FullName);
        }

        private void MoveDown()
            => selectedFileIndex = (selectedFileIndex + 1) % FileEntries.Count;

        private void MoveUp()
            => selectedFileIndex = (selectedFileIndex - 1 + FileEntries.Count) % FileEntries.Count;

        private void Return()
        {
            if (currentFilePath.Equals(fileSystem.Directory.GetDirectoryRoot(currentFilePath)))
            {
                return;
            }

            var parentPath = Path.GetDirectoryName(currentFilePath);
            var parentFile = fileSystem.FileInfo.New(currentFilePath);

            currentFilePath = parentPath is null
                ? fileSystem.Directory.GetDirectoryRoot(currentFilePath)
                : parentPath;

            RefreshFileEntriesList();
            multipleSelection = false;
            RefreshSelectedIndexes();
            selectedFileIndex = IndexOf(parentFile);
        }

        private void Advance()
        {
            if (SelectedFile is not null)
            {
                currentFilePath = SelectedFile.FullName;
            }

            RefreshFileEntriesList();
            multipleSelection = false;
            RefreshSelectedIndexes();
        }

        private bool GetHiddenToggle()
            => showHiddenFiles;

        private bool GetMultipleSelectionToggle()
            => multipleSelection;

        private List<IFileInfo> GetMultipleSelectedFiles()
            => multipleSelection
            ? FileEntries.Where(file => MultipleSelectedFiles[file]).ToList()
            : [SelectedFile!];

        private void ToggleHidden()
        {
            showHiddenFiles = !showHiddenFiles;
            RefreshFileEntriesList();
        }

        private void ToggleMultipleSelection()
            => multipleSelection = !multipleSelection;

        private void ToggleSelection()
        {
            if (!GetMultipleSelectionToggle())
            {
                ToggleMultipleSelection();
            }

            if (SelectedFile is not null)
            {
                MultipleSelectedFiles[SelectedFile] = !MultipleSelectedFiles[SelectedFile];
            }

            MoveDown();
            MyResetEventSlims.MainWindowResetEvent.Set();
        }

        private void Refresh()
        {
            var previousSelectionIndex = selectedFileIndex;
            RefreshFileEntriesList();
            multipleSelection = false;
            RefreshSelectedIndexes();

            selectedFileIndex = previousSelectionIndex == 0 || previousSelectionIndex < FileEntries.Count
                ? previousSelectionIndex
                : previousSelectionIndex - 1;

            if (FileEntries.Count == 0)
            {
                selectedFileIndex = -1;
            }

            MyResetEventSlims.MainWindowResetEvent.Set();
        }

        private void ResetSelectedFile()
        {
            /*            if (path is null)
            {
                return;
            }

            RefreshFileEntriesList();
            selectedFileIndex = FileEntries
                .FindIndex(file => Path.GetFileName(path).Equals(file.Name));*/

            /*            RefreshFileEntriesList();
                        var newestFile = FileEntries.OrderBy(file => file.LastWriteTime).FirstOrDefault();

                        selectedFileIndex = FileEntries
                            .FindIndex(file => file.Name.Equals(newestFile.Name));*/
        }

        /// <summary>
        /// Refreshes the file entries after adding, removing or editing files in <see cref="FileManager"/>.
        /// </summary>
        private void RefreshFileEntriesList()
        {
            FileEntries.Clear();

            var newFileEntries = fileSystem.Directory.EnumerateFileSystemEntries(currentFilePath)
                .Select(fileSystem.FileInfo.New)
                .Where(file => showHiddenFiles || (file.Attributes & FileAttributes.Hidden) == 0)
                .OrderByDescending(file => file.Attributes.HasFlag(FileAttributes.Directory))
                .ThenBy(file => file.Name);

            FileEntries.AddRange(newFileEntries);
            selectedFileIndex = FileEntries.Count != 0 ? 0 : -1;
            SubsetIndexes = new(0, NavigationWindowSettings.WindowHeight - 1);
        }

        /// <summary>
        /// Changes multiple selection property of each file entry to false.
        /// </summary>
        private void RefreshSelectedIndexes()
        {
            foreach (var fileEntry in FileEntries)
            {
                MultipleSelectedFiles[fileEntry] = false;
            }
        }

        /// <summary>
        /// Adjusts the range if the total file entries count is less than the RangeEnd.
        /// </summary>
        private void AdjustRangeForFileCount()
        {
            if (FileEntries.Count > SubsetIndexes.RangeEnd)
            {
                return;
            }

            SubsetIndexes = (RangeStart: Math.Max(0, SubsetIndexes.RangeEnd - NavigationWindowSettings.WindowHeight), RangeEnd: SubsetIndexes.RangeEnd);
        }

        /// <summary>
        /// Adjusts the range based on the height of the window.
        /// </summary>
        private void AdjustRangeForWindowSize()
        {
            if (SubsetIndexes.RangeEnd < NavigationWindowSettings.WindowHeight)
            {
                return;
            }

            SubsetIndexes = (RangeStart: SubsetIndexes.RangeStart, RangeEnd: SubsetIndexes.RangeStart + NavigationWindowSettings.WindowHeight - 1);
        }

        /// <summary>
        /// Adjusts the range to ensure it includes the currently selected file index.
        /// </summary>
        private void AdjustRangeForSelectedFileIndex()
        {
            if (selectedFileIndex > SubsetIndexes.RangeEnd)
            {
                SubsetIndexes = (RangeStart: selectedFileIndex - NavigationWindowSettings.WindowHeight + 1, RangeEnd: selectedFileIndex);
            }
            else if (selectedFileIndex < SubsetIndexes.RangeStart)
            {
                SubsetIndexes = (RangeStart: selectedFileIndex, RangeEnd: selectedFileIndex + NavigationWindowSettings.WindowHeight - 1);
            }
        }

        private void PrintLayout()
        {
            var contents = FileEntries.Count > 0
                ? new ConcurrentBag<IFileInfo>(GetWindowSubset()).Reverse()
                : [];

            ConsoleManager.InitializeCursorPosition(NavigationWindowSettings.StartingColumn, NavigationWindowSettings.StartingRow);
            PrintWindowContent(contents);
            StringManipulator.PrintFillerRows(contents.Count(), NavigationWindowSettings.WindowWidth, NavigationWindowSettings.WindowHeight, Console.CursorLeft, Console.CursorTop);
        }

        /// <summary>
        /// Prints the file entries that the current <see cref="NavigationWindow"/> contains, depending on window size.
        /// </summary>
        /// <param name="files">Collection of file entries to be printed to console.</param>
        private void PrintWindowContent(IEnumerable<IFileInfo> files)
        {
            var emptyFolderMessage = NavigationWindowSettings.EmptyFolderMessage;

            if (FileEntries.Count > 0)
            {
                foreach (var currentFile in files)
                {
                    PrintEntry(currentFile, IsSelected(currentFile));
                    ConsoleManager.JumpToNewLine(NavigationWindowSettings.StartingColumn);
                }
            }
            else
            {
                var message = emptyFolderMessage.Length < NavigationWindowSettings.WindowWidth
                    ? StringManipulator.GetCenteredText(emptyFolderMessage, NavigationWindowSettings.WindowWidth)
                    : StringManipulator.TruncateString(emptyFolderMessage, NavigationWindowSettings.WindowWidth);

                Console.Write(message);
                ConsoleManager.JumpToNewLine(NavigationWindowSettings.StartingColumn);
            }
        }

        /// <summary>
        /// Prints the file entry to console.
        /// </summary>
        /// <param name="fileInfo">The <see cref="IFileInfo"/> to be printed to console.</param>
        /// <param name="selectedStatus">A boolean showing if the file is selected or not.</param>
        private void PrintEntry(IFileInfo fileInfo, bool selectedStatus)
        {
            if (selectedStatus)
            {
                SelectedFileRow = Console.CursorTop;
            }

            if (multipleSelection)
            {
                Console.BackgroundColor = MultipleSelectedFiles[fileInfo]
                    ? NavigationWindowSettings.SelectedBoxColor
                    : NavigationWindowSettings.GetConsoleColors(false, false).BackgroundColor;

                Console.Write(NavigationWindowSettings.SelectedBoxText);
                Console.ResetColor();
            }

            NavigationWindowSettings.SetConsoleColors(fileSystem.Directory.Exists(fileInfo.FullName), selectedStatus);

            var innerWidth = multipleSelection
                ? NavigationWindowSettings.WindowWidth - NavigationWindowSettings.SelectedBoxText.Length
                : NavigationWindowSettings.WindowWidth;
            var entryName = StringManipulator.TruncateString(fileInfo.Name, innerWidth);

            Console.Write(entryName);
            Console.ResetColor();
        }

        private int IndexOf(IFileInfo fileInfo)
            => FileEntries.Select((entry, index) => new { Entry = entry, Index = index })
            .FirstOrDefault(x => x.Entry.Name.Equals(fileInfo.Name))?.Index ?? -1;

        private bool IsSelected(IFileInfo fileInfo)
            => SelectedFile is not null
            && SelectedFile.Name.Equals(fileInfo.Name);

        #region Interface methods

        /// <inheritdoc/>
        IFileInfo? INavigationWindow.SelectedFile
            => SelectedFile;

        /// <inheritdoc/>
        int INavigationWindow.SelectedFileRow
            => SelectedFileRow;

        /// <inheritdoc/>
        IFileInfo? INavigationWindow.GetCurrentFile()
            => GetCurrentFile();

        /// <inheritdoc/>
        IEnumerable<IFileInfo> INavigationWindow.GetWindowSubset()
            => GetWindowSubset();

        /// <inheritdoc/>
        INavigationWindow? INavigationWindow.GetParentAsNavigationWindow()
            => GetParentAsNavigationWindow();

        /// <inheritdoc/>
        void INavigationWindow.MoveDown()
            => MoveDown();

        /// <inheritdoc/>
        void INavigationWindow.MoveUp()
            => MoveUp();

        /// <inheritdoc/>
        void INavigationWindow.Return()
            => Return();

        /// <inheritdoc/>
        void INavigationWindow.Advance()
            => Advance();

        /// <inheritdoc/>
        bool INavigationWindow.GetHiddenToggle()
            => GetHiddenToggle();

        /// <inheritdoc/>
        bool INavigationWindow.GetMultipleSelectionToggle()
            => GetMultipleSelectionToggle();

        /// <inheritdoc/>
        List<IFileInfo> INavigationWindow.GetMultipleSelectedFiles()
            => GetMultipleSelectedFiles();

        /// <inheritdoc/>
        void INavigationWindow.ToggleHidden()
            => ToggleHidden();

        /// <inheritdoc/>
        void INavigationWindow.ToggleMultipleSelection()
            => ToggleMultipleSelection();

        /// <inheritdoc/>
        void INavigationWindow.ToggleSelection()
            => ToggleSelection();

        /// <inheritdoc/>
        void INavigationWindow.Refresh()
            => Refresh();

        /// <inheritdoc/>
        void INavigationWindow.ResetSelectedFile()
            => ResetSelectedFile();

        /// <inheritdoc />
        void INavigationWindow.PrintLayout()
            => PrintLayout();

        #endregion
    }
}
