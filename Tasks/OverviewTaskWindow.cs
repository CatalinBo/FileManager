namespace FileManager.Tasks
{
    /// <summary>
    /// Holds a list of all running background tasks, and provides methods to navigate and cancel selected tasks.
    /// </summary>
    public class OverviewTaskWindow : IBackgroundTasksWindow
    {
        private int selectedOperation;
        private readonly List<IProgressBar> runningTasksList;

        public OverviewTaskWindow()
        {
            selectedOperation = 0;
            runningTasksList = new(MyTaskList.RunningTasks);
        }

        #region Window dimensions and cursor positioning

        public int WindowWidth
            => BackgroundTasksWindowSettings.GetWindowWidth();

        public int WindowHeight
            => runningTasksList.Count + BackgroundTasksWindowSettings.OverviewHorizontalWindowBorders;

        #endregion

        #region Console color settings

        private static ConsoleColor ForegroundColor
            => BackgroundTasksWindowSettings.OverviewForegroundColor;

        private static ConsoleColor BackgroundColor
            => BackgroundTasksWindowSettings.OverviewBackgroundColor;

        #endregion

        public bool DisplayWindow()
        {
            MyResetEventSlims.BackgroundResetEvent.Set();
            KeyStack.Keys.Clear();

            while (true)
            {
                if (KeyStack.Keys.TryPeek(out var key))
                {
                    if (CommandKeys.ToggleBackgroundTaskWindowKeys.Contains(key.Key))
                    {
                        MyTaskList.SwitchTaskWindowState();
                        KeyStack.Keys.Clear();
                        return false;
                    }

                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            MoveUp();
                            break;
                        case ConsoleKey.DownArrow:
                            MoveDown();
                            break;
                        case ConsoleKey.Enter:
                            CancelTask();
                            break;
                    }

                    KeyStack.Keys.Clear();
                }

                Thread.Sleep(FileManagerSettings.ThrottleDelay);
                MyResetEventSlims.BackgroundResetEvent.Set();
            }
        }

        public void PrintFullLayout()
        {
            ConsoleManager.InitializeCursorPosition(BackgroundTasksWindowSettings.GetStartingColumn(), BackgroundTasksWindowSettings.GetStartingRow(this));
            SetConsoleColors();

            PrintTopBorder();
            PrintOperations();
            PrintBottomBorder();

            Console.ResetColor();
            MyResetEventSlims.BackgroundResetEvent.Reset();

            void PrintTopBorder()
            {
                Console.Write($"{BackgroundTasksWindowSettings.TopBorder}");
                ConsoleManager.JumpToNewLine(BackgroundTasksWindowSettings.GetStartingColumn());
            }

            void PrintOperations()
            {
                foreach (var pb in runningTasksList)
                {
                    PrintOperationLine(pb);
                    ConsoleManager.JumpToNewLine(BackgroundTasksWindowSettings.GetStartingColumn());
                }

                void PrintOperationLine(IProgressBar progressBar)
                {
                    Console.Write($"{BackgroundTasksWindowSettings.LeftBorder}");
                    PrintOperationInfo();
                    PrintOperationStatus();
                    Console.Write($"{BackgroundTasksWindowSettings.RightBorder}");

                    // Prints info about the current progress bar (operation name, total files and percentage of completion).
                    void PrintOperationInfo()
                    {
                        var innderWidth = WindowWidth                                                               // calculates available inside window width by substracting from total window width
                            - BackgroundTasksWindowSettings.LeftBorder.Length                                       // left border width
                            - BackgroundTasksWindowSettings.RightBorder.Length                                      // right border width
                            - progressBar.GetOperationMessage().Length                                              // operation name label length
                            - StringManipulator.GetTaskStatusPlaceholder(progressBar.TaskStatus).ToString().Length; // current status length
                        var message = $"{progressBar.GetOperationMessage()}{progressBar.TotalFiles} files ({progressBar.FilesProcessed} / {progressBar.TotalFiles})";

                        Console.Write($"{StringManipulator.TruncateString(message, innderWidth)}");
                    }

                    // Prints the status of current progress bar (Completed or Cancelled), with an available "Cancel" option.
                    void PrintOperationStatus()
                    {
                        var isSelected = MyTaskList.RunningTasks.Count > 0
                            && progressBar.Equals(runningTasksList.ElementAt(selectedOperation));
                        var option = StringManipulator.GetTaskStatusPlaceholder(progressBar.TaskStatus).ToString();
                        var emptySpaces = "".PadLeft(WindowWidth - Console.CursorLeft - option.Length - BackgroundTasksWindowSettings.RightBorder.Length, ' '); // pad status to the right

                        Console.Write(emptySpaces);
                        SetConsoleColors(isSelected);
                        Console.Write(option);
                        Console.ResetColor();
                    }
                }
            }

            void PrintBottomBorder()
                => Console.Write($"{BackgroundTasksWindowSettings.BottomBorder}");
        }

        #region Navigation methods

        private void MoveUp()
        {
            var originalPosition = selectedOperation;

            do
            {
                selectedOperation = (selectedOperation - 1 + runningTasksList.Count) % runningTasksList.Count;
            }
            while (runningTasksList[selectedOperation].TaskStatus != TaskStatus.Running 
            && selectedOperation != originalPosition);
        }

        private void MoveDown()
        {
            var originalPosition = selectedOperation;

            do
            {
                selectedOperation = (selectedOperation + 1) % runningTasksList.Count;
            }
            while (runningTasksList[selectedOperation].TaskStatus != TaskStatus.Running
            && selectedOperation != originalPosition);
        }

        private void CancelTask()
        {
            runningTasksList.ElementAt(selectedOperation).CancellationTokenSource.Cancel();
            Thread.Sleep(FileManagerSettings.DebounceDelay);
            MoveDown();
        }

        #endregion

        /// <summary>
        /// Sets foreground and background console colors.
        /// </summary>
        /// <param name="isSelected"><see cref="bool"/> to highlight if the next to be printed option is selected.</param>
        private static void SetConsoleColors(bool isSelected = false)
        {
            (Console.ForegroundColor, Console.BackgroundColor) = isSelected
                ? (BackgroundColor, ForegroundColor)
                : (ForegroundColor, BackgroundColor);
        }
    }
}
