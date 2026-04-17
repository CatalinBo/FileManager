namespace FileManager.Preview
{
    /// <summary>
    /// Factory class that provides methods for creating new instances of <see cref="IPreviewWindow"/>.
    /// </summary>
    public class ConcretePreviewWindowFactory : PreviewWindowFactory
    {
        private readonly IFileSystem fileSystem = new FileSystem();

        public ConcretePreviewWindowFactory() { }

        public ConcretePreviewWindowFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IPreviewWindow"/>, depending on the given <see cref="INavigationWindow"/>.
        /// </summary>
        /// <param name="navigationWindow"><see cref="INavigationWindow"/> instance for which to generate a new <see cref="IPreviewWindow"/>.</param>
        /// <returns>A new instance of <see cref="IPreviewWindow"/>.</returns>
        public override IPreviewWindow GetPreviewWindow(INavigationWindow navigationWindow)
        {
            var selectedFile = navigationWindow.SelectedFile;

            if (selectedFile is null)
            {
                return new EmptyFileWindow(string.Empty);
            }

            try
            {
                if (IsDirectory(selectedFile))
                {
                    if (IsEmptyDirectory(selectedFile))
                    {
                        return new EmptyFileWindow($"{PreviewWindowSettings.EmptyFolderMessage}");
                    }

                    return new DirectoryFileWindow(navigationWindow);
                }

                if (IsTextFile(selectedFile))
                {
                    return new TextFileWindow(selectedFile);
                }
            }
            catch (UnauthorizedAccessException)
            {
                return new ProtectedFileWindow($"{PreviewWindowSettings.UnknownTypeMessage}");
            }
            catch (DecoderFallbackException)
            {
                return new UnknownFileWindow();
            }

            return new UnknownFileWindow();
        }

        #region Helper checking methods

        /// <summary>
        /// Checks if the given <see cref="IFileInfo"/> is a directory.
        /// </summary>
        /// <param name="fileInfo"><see cref="IFileInfo"/> for which to perform the check.</param>
        /// <returns><see cref="true"/> if the <see cref="IFileInfo"/> is a directory, otherwise <see cref="false"/>.</returns>
        public bool IsDirectory(IFileInfo fileInfo)
            => fileSystem.Directory.Exists(fileInfo.FullName);

        /// <summary>
        /// Checks if the given <see cref="IFileInfo"/> is an empty directory.
        /// </summary>
        /// <param name="fileInfo"><see cref="IFileInfo"/> for which to perform the check.</param>
        /// <returns><see cref="true"/> if the <see cref="IFileInfo"/> is an empty directory, otherwise <see cref="false"/>.</returns>
        public bool IsEmptyDirectory(IFileInfo fileInfo)
            => !fileSystem.Directory.EnumerateFileSystemEntries(fileInfo.FullName).Any();

        /// <summary>
        /// Checks if the given <see cref="IFileInfo"/> is a text file.
        /// </summary>
        /// <param name="fileInfo"><see cref="IFileInfo"/> for which to perform the check.</param>
        /// <returns><see cref="true"/> if the <see cref="IFileInfo"/> is a text file, otherwise <see cref="false"/>.</returns>
        public bool IsTextFile(IFileInfo fileInfo)
            => fileSystem.File.ReadLines(fileInfo.FullName).All(HasOnlyValidCharacters);

        /// <summary>
        /// Checks if the given <see cref="string"/> contains only valid (printable) characters.
        /// </summary>
        /// <param name="text"><see cref="string"/> to check for valid characters.</param>
        /// <returns><see cref="true"/> if the text has only valid characters, otherwise <see cref="false""/>.</returns>
        private bool HasOnlyValidCharacters(string text)
            => text.All(ch => !char.IsControl(ch));

        #endregion
    }
}
