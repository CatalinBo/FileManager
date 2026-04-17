namespace FileManager.Tasks
{
    /// <summary>
    /// Common interface for all instances of background tasks window.
    /// </summary>
    public interface IBackgroundTasksWindow
    {
        int WindowWidth { get; }
        int WindowHeight { get; }
        bool DisplayWindow();
        void PrintFullLayout();
    }
}
