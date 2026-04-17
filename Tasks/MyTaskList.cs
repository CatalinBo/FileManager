namespace FileManager.Tasks
{
    /// <summary>
    /// Static class that provides properties and methods for tasks window handling.
    /// </summary>
    public static class MyTaskList
    {
        private static TasksWindowState CurrentWindowState { get; set; } = TasksWindowState.NonExistent;

        private static IProgressBar? NewTask { get; set; } = null;

        public static List<IProgressBar> TaskList { get; private set; } = [];

        public static List<IProgressBar> RunningTasks
            => TaskList.Select(task => task)
            .Where(task => task.TaskStatus is TaskStatus.Running)
            .ToList();

        public static void AddToTaskList(IProgressBar progressBar)
            => TaskList.Add(progressBar);

        public static void InitializeTasksWindow(IProgressBar taskProgress)
        {
            CurrentWindowState = TasksWindowState.Detailed;
            NewTask = taskProgress;
            AddToTaskList(taskProgress);
        }

        public static IBackgroundTasksWindow? GetTaskWindow()
        {
            return CurrentWindowState switch
            {
                TasksWindowState.Detailed => new DetailedTaskWindow(NewTask!),
                TasksWindowState.Minimized => new MinimizedTaskWindow(),
                TasksWindowState.Overview => new OverviewTaskWindow(),
                _ => null
            };
        }

        public static void CloseCurrentTasksWindow()
        {
            CurrentWindowState = RunningTasks.Count == 0
                ? TasksWindowState.NonExistent
                : TasksWindowState.Minimized;
        }

        public static void SwitchTaskWindowState()
        {
            if (RunningTasks.Count == 0)
            {
                CurrentWindowState = TasksWindowState.NonExistent;
                return;
            }

            switch (CurrentWindowState)
            {
                case var _ when CurrentWindowState is TasksWindowState.Minimized:
                    CurrentWindowState = TasksWindowState.Overview;
                    break;
                case var _ when CurrentWindowState is TasksWindowState.Overview:
                    CurrentWindowState = TasksWindowState.Minimized;
                    break;
            }
        }
    }
}
