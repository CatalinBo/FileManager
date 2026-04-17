namespace FileManager.Preview
{
    /// <summary>
    /// Factory abstract class used to create a new instance of <see cref="IPreviewWindow"/>.
    /// </summary>
    public abstract class PreviewWindowFactory
    {
        /// <summary>
        /// Gets the <see cref="IPreviewWindow"/>, depending on the given <see cref="INavigationWindow"/>.
        /// </summary>
        /// <param name="navigationWindow">The current instance of <see cref="INavigationWindow"/>.</param>
        /// <returns>A new instance of <see cref="IPreviewWindow"/>.</returns>
        public abstract IPreviewWindow GetPreviewWindow(INavigationWindow navigationWindow);
    }
}
