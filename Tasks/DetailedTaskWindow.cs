namespace FileManager.Tasks
{
    /// <summary>
    /// Provides methods and properties for handling a new file operation.
    /// </summary>
    public class DetailedTaskWindow(IProgressBar progressBar) : IBackgroundTasksWindow
    {
        private readonly IProgressBar progressBar = progressBar;
        private readonly DetailedTaskOptions[] options = (DetailedTaskOptions[])Enum.GetValues(typeof(DetailedTaskOptions));
        private readonly int optionsCount = Enum.GetNames(typeof(DetailedTaskOptions)).Length;
        private int selectedOptionIndex = 0;

        #region Window dimensions and cursor positioning

        public int WindowWidth
            => BackgroundTasksWindowSettings.GetWindowWidth();

        public int WindowHeight
            => BackgroundTasksWindowSettings.GetWindowHeight(this);

        private int StartingColumn
            => BackgroundTasksWindowSettings.GetStartingColumn();

        #endregion

        #region Console colors

        private ConsoleColor ForegroundColor { get; } = BackgroundTasksWindowSettings.GetConsoleColors(progressBar).ForegroundColor;

        private ConsoleColor BackgroundColor { get; } = BackgroundTasksWindowSettings.GetConsoleColors(progressBar).BackgroundColor;

        public ConsoleColor SelectedForegroundColor
            => BackgroundColor;

        public ConsoleColor SelectedBackgroundColor
            => ForegroundColor;

        #endregion

        #region Window layout helper strings

        private string SourceInfo
        {
            get
            {
                var innerWidth = GetInnerWidth(BackgroundTasksWindowSettings.SourcePathLabel);

                return $"{StringManipulator.TruncateString(progressBar.SourcePath, innerWidth)}";
            }
        }

        private string? TargetInfo
        {
            get
            {
                var innerWidth = GetInnerWidth(BackgroundTasksWindowSettings.TargetPathLabel);

                return progressBar.TargetPath is null
                    ? $"{string.Concat(Enumerable.Repeat(' ', innerWidth))}"
                    : $"{StringManipulator.TruncateString(progressBar.TargetPath, innerWidth)}";
            }
        }

        private string ProgressInfo
        {
            get
            {
                var innerWidth = GetInnerWidth(BackgroundTasksWindowSettings.TotalProcessedLabel);

                return $"{StringManipulator.TruncateString($"{progressBar.FilesProcessed} / {progressBar.TotalFiles}", innerWidth)}";
            }
        }

        #endregion

        public bool DisplayWindow()
        {
            MyResetEventSlims.BackgroundResetEvent.Set();

            while (progressBar.TaskStatus is TaskStatus.Running)
            {
                if (KeyStack.Keys.TryPeek(out ConsoleKeyInfo keyInfo))
                {
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            MovePrevious();
                            break;
                        case ConsoleKey.RightArrow:
                            MoveNext();
                            break;
                        case ConsoleKey.Enter:
                            ExecuteCommand();
                            return false;
                    }
                }

                KeyStack.Keys.Clear();
                MyResetEventSlims.BackgroundResetEvent.Set();
                Task.Delay(FileManagerSettings.DebounceDelay);
            }

            MyTaskList.CloseCurrentTasksWindow();
            return false;
        }

        #region Window layout generating and printing methods

        public void PrintFullLayout()
        {
            ConsoleManager.InitializeCursorPosition(StartingColumn, BackgroundTasksWindowSettings.GetStartingRow(this));
            SetConsoleColors();

            PrintTopBorder();
            PrintTaskOperationName();
            PrintInsideBorder();
            PrintSourceInfo();
            PrintTargetInfo();
            PrintInsideBorder();
            PrintProgressInfo();
            PrintInsideBorder();
            PrintOptions();
            PrintBottomBorder();
            MyResetEventSlims.BackgroundResetEvent.Reset();

            void PrintTopBorder()
            {
                Console.Write(BackgroundTasksWindowSettings.TopBorder);
                ConsoleManager.JumpToNewLine(StartingColumn);
            }

            void PrintTaskOperationName()
            {
                var insideWindowWidth = WindowWidth - BackgroundTasksWindowSettings.LeftBorder.Length - BackgroundTasksWindowSettings.RightBorder.Length;
                var opName = StringManipulator.TruncateString(progressBar.GetOperationMessage(), insideWindowWidth);

                Console.Write($"{BackgroundTasksWindowSettings.LeftBorder}{opName}{BackgroundTasksWindowSettings.RightBorder}");
                ConsoleManager.JumpToNewLine(StartingColumn);
            }

            void PrintSourceInfo()
            {
                Console.Write($"{BackgroundTasksWindowSettings.LeftBorder}{BackgroundTasksWindowSettings.SourcePathLabel}{SourceInfo}{BackgroundTasksWindowSettings.RightBorder}");
                ConsoleManager.JumpToNewLine(StartingColumn);
            }

            void PrintTargetInfo()
            {
                Console.Write($"{BackgroundTasksWindowSettings.LeftBorder}{BackgroundTasksWindowSettings.TargetPathLabel}{TargetInfo}{BackgroundTasksWindowSettings.RightBorder}");
                ConsoleManager.JumpToNewLine(StartingColumn);
            }

            void PrintInsideBorder()
            {
                Console.Write(BackgroundTasksWindowSettings.InsideBorder);
                ConsoleManager.JumpToNewLine(StartingColumn);
            }

            void PrintProgressInfo()
            {
                Console.Write($"{BackgroundTasksWindowSettings.LeftBorder}{BackgroundTasksWindowSettings.TotalProcessedLabel}{ProgressInfo}{BackgroundTasksWindowSettings.RightBorder}");
                ConsoleManager.JumpToNewLine(StartingColumn);
            }

            void PrintOptions()
            {
                var optionWidth = (WindowWidth - BackgroundTasksWindowSettings.LeftBorder.Length - BackgroundTasksWindowSettings.RightBorder.Length) / optionsCount;

                Console.Write(BackgroundTasksWindowSettings.LeftBorder);

                foreach (var option in options)
                {
                    PrintOption(option);
                }

                SetConsoleColors();
                Console.Write(BackgroundTasksWindowSettings.RightBorder.PadLeft(WindowWidth - Console.CursorLeft));
                ConsoleManager.JumpToNewLine(StartingColumn);

                void PrintOption(DetailedTaskOptions option)
                {
                    SetConsoleColors(option.Equals(options[selectedOptionIndex]));
                    Console.Write($"{StringManipulator.GetCenteredText(option.ToString(), optionWidth)}");
                    Console.ResetColor();
                }
            }

            void PrintBottomBorder()
                => Console.Write(BackgroundTasksWindowSettings.BottomBorder);
        }

        /// <summary>
        /// Uses a pattern to calculate the width of a inner window.
        /// </summary>
        /// <param name="label">Text that will be displayed inside the window.</param>
        /// <remarks>Method is used to calculate the inner width of a window, to use it when truncating text when its length is oveflowing available space.</remarks>
        private int GetInnerWidth(string label)
            => WindowWidth - BackgroundTasksWindowSettings.LeftBorder.Length - label.Length - BackgroundTasksWindowSettings.RightBorder.Length;

        #endregion

        #region Navigating methods

        private void MovePrevious()
            => selectedOptionIndex = (selectedOptionIndex - 1 + optionsCount) % optionsCount;

        private void MoveNext()
            => selectedOptionIndex = (selectedOptionIndex + 1) % optionsCount;

        private void ExecuteCommand()
        {
            KeyStack.Keys.Clear();
            if (selectedOptionIndex == 1)
            {
                progressBar.CancellationTokenSource.Cancel();
            }
            MyTaskList.CloseCurrentTasksWindow();
        }

        #endregion

        /// <summary>
        /// Sets foreground and background console colors, when printing window layout.
        /// </summary>
        /// <param name="isSelected"><see cref="bool"/> signaling the currently selected option.</param>
        private void SetConsoleColors(bool isSelected = false)
        {
            (Console.ForegroundColor, Console.BackgroundColor) = isSelected
                ? (SelectedForegroundColor, SelectedBackgroundColor)
                : (ForegroundColor, BackgroundColor);
        }
    }

    /// <summary>
    /// Holds available selectable options when dealing with a <see cref="DetailedTaskWindow"/>.
    /// </summary>
    public enum DetailedTaskOptions
    {
        Background = 0,
        Cancel = 1
    }
}
