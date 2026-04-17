namespace FileManager.Tasks
{
    /// <summary>
    /// Provides properties and methods to generate window layout when the task window is minimized.
    /// </summary>
    public class MinimizedTaskWindow : IBackgroundTasksWindow
    {
        public MinimizedTaskWindow() { }

        public int WindowWidth
            => BackgroundTasksWindowSettings.GetWindowWidth();

        public int WindowHeight
            => BackgroundTasksWindowSettings.GetWindowHeight(this);

        public bool DisplayWindow()
            => throw new NotImplementedException();

        public void PrintFullLayout()
        {
            ConsoleManager.InitializeCursorPosition(BackgroundTasksWindowSettings.GetStartingColumn(), BackgroundTasksWindowSettings.GetStartingRow(this));
            ConsoleManager.InitializeConsoleColors(BackgroundTasksWindowSettings.MinimizedForegroundColor, BackgroundTasksWindowSettings.MinimizedBackgroundColor);

            var message = MyTaskList.RunningTasks.Count > 0
                ? $"{MyTaskList.RunningTasks.Count} file operations running..."
                : $"{string.Concat(Enumerable.Repeat(' ', WindowWidth))}";

            Console.Write(StringManipulator.TruncateString(message, WindowWidth));
            Console.ResetColor();
            MyResetEventSlims.BackgroundResetEvent.Reset();
        }
    }
}
