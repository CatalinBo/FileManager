namespace FileManager.Utility
{
    /// <summary>
    /// Static class that acts as a clipboard, holding data about files that need to be copied, moved, deleted or renamed.
    /// </summary>
    public static class MyClipboard
    {
        /// <summary>
        /// An <see cref="IEnumerable{T}"/> holding all <see cref="IFileInfo"/>s that needs to be processed.
        /// </summary>
        public static List<IFileInfo>? SourceFiles { get; private set; }

        /// <summary>
        /// The path at which to move or copy current <see cref="IFileInfo"/>.
        /// </summary>
        public static string? DestinationPath { get; private set; }

        /// <summary>
        /// <see cref="ConsoleKeyInfo"/> that represents the command given to <see cref="FileManager"/>.
        /// </summary>
        public static ConsoleKeyInfo CommandKey { get; private set; }

        /// <summary>
        /// A counter used to keep track of how many paste instances where used. The counter is used to rename the files accordingly.
        /// </summary>
        public static int PasteInstance { get; private set; }

        /// <summary>
        /// Initializes the clipboard by setting its properties to their default values.
        /// </summary>
        /// <remarks>This method clears any selected source files, sets the destination path and command key to their default values,
        /// and resets the PasteInstance counter.</remarks>
        public static void InitializeClipboard()
        {
            SourceFiles = null;
            DestinationPath = null;
            CommandKey = FileManagerSettings.DefaultKey;
            PasteInstance = 0;
        }

        /// <summary>
        /// Updates the clipboard with selected files and command key information.
        /// </summary>
        /// <param name="keyInfo">The key information representing the command key used to trigger file manipulation.</param>
        /// <param name="navigationWindow">The <see cref="INavigationWindow"/> instance that contains the selected files.</param>
        public static void UpdateClipboard(ConsoleKeyInfo keyInfo, INavigationWindow navigationWindow)
        {
            SourceFiles = navigationWindow.GetMultipleSelectedFiles();
            CommandKey = keyInfo;
            PasteInstance = 0;
        }

        #region Property Setters

        public static void SetCommandKey(ConsoleKeyInfo keyInfo)
            => CommandKey = keyInfo;

        public static void SetDestinationPath(string? newPath)
            => DestinationPath = newPath;

        public static void IncrementPasteCounter()
            => PasteInstance++;

        #endregion
    }
}
