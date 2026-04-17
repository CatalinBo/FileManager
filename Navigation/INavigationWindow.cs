namespace FileManager.Navigation
{
    public interface INavigationWindow
    {
        #region Properties

        /// <summary>
        /// Currently selected file.
        /// </summary>
        IFileInfo? SelectedFile { get; }

        /// <summary>
        /// The row of the currently selected <see cref="IFileInfo"/>.
        /// </summary>
        int SelectedFileRow { get; }

        #endregion

        #region Property getter methods

        /// <summary>
        /// Gets the current instance of <see cref="IFileInfo"/> on which <see cref="INavigationWindow"/> is based on.
        /// </summary>
        IFileInfo? GetCurrentFile();

        /// <summary>
        /// Gets file entries that can be printed to console.
        /// </summary>
        IEnumerable<IFileInfo> GetWindowSubset();

        /// <summary>
        /// Gets the current status of the hidden files toggle.
        /// </summary>
        /// <returns><see cref="true"/> if hidden files are shown, otherwise <see cref="false"/>.</returns>
        bool GetHiddenToggle();

        /// <summary>
        /// Gets the multiple selection toggle.
        /// </summary>
        /// <returns><see cref="true"/> if the multiple selection toggle is on, otherwise <see cref="false"/>.</returns>
        bool GetMultipleSelectionToggle();

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> containing all selected files for processing.
        /// </summary>
        /// <returns></returns>
        List<IFileInfo> GetMultipleSelectedFiles();

        /// <summary>
        /// Gets the parent window contents as a separate instance of <see cref="INavigationWindow"/>.
        /// </summary>
        /// <returns></returns>
        INavigationWindow? GetParentAsNavigationWindow();

        #endregion

        #region Navigation methods

        void MoveDown();

        void MoveUp();

        /// <summary>
        /// Moves the navigation to the parent folder.
        /// </summary>
        void Return();

        /// <summary>
        /// Moves the navigation inside the currently selected directory.
        /// </summary>
        void Advance();

        #endregion

        #region Toggles

        /// <summary>
        /// Shows/hides hidden files.
        /// </summary>
        void ToggleHidden();

        /// <summary>
        /// Toggles the multiple selection of files.
        /// </summary>
        void ToggleMultipleSelection();

        /// <summary>
        /// Selects/deselects highlighted file system entry.
        /// </summary>
        void ToggleSelection();

        #endregion

        #region Window refresh/reset methods

        /// <summary>
        /// Refreshes the current instance of <see cref="INavigationWindow"/>.
        /// </summary>
        void Refresh();

        void ResetSelectedFile();

        #endregion

        #region Layout printing methods

        /// <summary>
        /// Prints the entire <see cref="INavigationWindow"/> layout, by using helper methods and filling remaining space.
        /// </summary>
        void PrintLayout();

        #endregion
    }
}
