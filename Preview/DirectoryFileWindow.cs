namespace FileManager.Preview
{
    /// <summary>
    /// Provides methods for generating and printing window layout.
    /// </summary>
    /// <param name="navigationWindow">Instance of <see cref="INavigationWindow"/> on which the current <see cref="IPreviewWindow"/> is based.</param>
    public class DirectoryFileWindow(INavigationWindow navigationWindow) : IPreviewWindow
    {
        private readonly FileSystem fileSystem = new();
        private readonly IFileInfo? fileInfo = navigationWindow.SelectedFile;
        private readonly bool showHiddenFiles = navigationWindow.GetHiddenToggle();

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> representing the current file entries collection.
        /// </summary>
        private IEnumerable<IFileInfo> FileEntries
            => showHiddenFiles
            ? GetAllFileSystemEntries()
            : GetAllFileSystemEntries().Where(file => !file.Attributes.HasFlag(FileAttributes.Hidden));

        #region Window layout printing methods

        /// <summary>
        /// Prints the entire <see cref="DirectoryFileWindow"/> layout, by calling helper methods and clearing empty lines.
        /// </summary>
        void IPreviewWindow.PrintLayout()
        {
            ConsoleManager.InitializeCursorPosition(PreviewWindowSettings.StartingColumn, PreviewWindowSettings.StartingRow);
            ConsoleManager.InitializeConsoleColors(PreviewWindowSettings.GetConsoleColors(this).ForegroundColor, PreviewWindowSettings.GetConsoleColors(this).BackgroundColor);

            var contents = GetWindowContents();

            PrintContent(contents);
            StringManipulator.PrintFillerRows(contents.Count(), PreviewWindowSettings.WindowWidth, PreviewWindowSettings.WindowHeight, Console.CursorLeft, Console.CursorTop);

            Console.ResetColor();
        }

        /// <summary>
        /// Prints the contents of the current <see cref="DirectoryFileWindow"/>.
        /// </summary>
        /// <param name="fileEntries">The <see cref="IEnumerable{T}"/> that needs to be printed to console.</param>
        private void PrintContent(IEnumerable<IFileInfo> fileEntries)
        {
            foreach (var entry in fileEntries)
            {
                var name = StringManipulator.TruncateString(entry.Name, PreviewWindowSettings.WindowWidth);

                Console.Write(name);
                ConsoleManager.JumpToNewLine(PreviewWindowSettings.StartingColumn);
            }

            if (fileEntries.Count() < FileEntries.Count())
            {
                var message = StringManipulator.TruncateString($"...and {FileEntries.Count() - fileEntries.Count()} more files.", PreviewWindowSettings.WindowWidth);

                Console.Write(message);
                ConsoleManager.JumpToNewLine(PreviewWindowSettings.StartingColumn);
            }
        }

        #endregion

        #region Window contents generator methods

        /// <summary>
        /// Gets all file system entries located in the current directory, ordering them by type and name.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of file system entries if the <see cref="fileInfo"/> is not <see cref="null"/>,
        /// otherwise an empty <see cref="IEnumerable{T}"/>.</returns>
        private IEnumerable<IFileInfo> GetAllFileSystemEntries()
        {
            if (fileInfo is null)
            {
                return [];
            }

            return fileSystem.Directory.GetFileSystemEntries(fileInfo.FullName)
                .Select(fileSystem.FileInfo.New)
                .OrderByDescending(fileInfo => fileSystem.Directory.Exists(fileInfo.FullName))
                .ThenBy(fileInfo => fileInfo.FullName);
        }

        /// <summary>
        /// Gets a collection of <see cref="IFileInfo"/>, depending on the window size.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/>, of different sizes, depending on the current window height.</returns>
        private IEnumerable<IFileInfo> GetWindowContents()
        {
            var reqSize = PreviewWindowSettings.WindowHeight < FileEntries.Count()
                ? PreviewWindowSettings.WindowHeight - 1
                : Math.Min(PreviewWindowSettings.WindowHeight, FileEntries.Count());

            return FileEntries.Take(reqSize);
        }

        #endregion
    }
}
