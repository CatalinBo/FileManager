namespace FileManager.Main
{
    public class FileManager
    {
        private readonly IFileSystem fileSystem;
        private readonly ConcurrentQueue<EventActionPair> eventActionPairs = new();

        public FileManager()
        {
            fileSystem = new FileSystem();
            InitializeEventActionPairs();
            MyClipboard.InitializeClipboard();
            NavigationWindow = new NavigationWindow(fileSystem);
        }

        public FileManager(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            InitializeEventActionPairs();
            MyClipboard.InitializeClipboard();
            NavigationWindow = new NavigationWindow(fileSystem);
        }

        private ParentWindow ParentWindow
            => GetParentWindow();
        private INavigationWindow NavigationWindow { get; set; }
        private IPreviewWindow PreviewWindow
            => GetPreviewWindow();
        private StatusBar StatusBar
            => GetStatusBar();
        public IFileInfo? SelectedFile
            => NavigationWindow.SelectedFile;
        private PopUpDialog? PopUpDialog { get; set; }
        private RenameWindow? RenameWindow { get; set; }
        private IBackgroundTasksWindow? TasksWindow { get; set; } = null;

        /// <summary>
        /// Handles the specified console key input to manage different features within the application.
        /// </summary>
        /// <param name="key">The console key input representing a user's application feature demand.</param>
        private void StartAppFeature(ConsoleKeyInfo key)
        {
            switch (key)
            {
                case var _ when KeyTypeCheck.IsMultipleSelectionKey(key):
                    NavigationWindow.ToggleSelection();
                    return;
                case var _ when KeyTypeCheck.IsEditShortcutsKey(key):
                    //EditShortcutsTriggered = true;
                    var editShortcutsTask = Task.Factory.StartNew(EditShortcutsWindow.SetNewShortcuts);
                    //editShortcutsTask.Wait();
                    return;
            }
        }

        #region REVIEWED METHODS

        /// <summary>
        /// Initiates and runs the main program logic asynchronously, managing key captures, file processing, window size checks, and layout printing.
        /// It starts the necessary tasks for handling key input, processing commands, monitoring window size changes, and printing layouts.
        /// </summary>
        public void Run()
        {
            MyResetEventSlims.MainWindowResetEvent.Set();

            var captureKeysTask = Task.Run(KeyStack.CaptureKeysAsync, AppControl.MainKillSwitch.Token);
            var mainTask = Task.Run(ProcessKeysAsync, AppControl.MainKillSwitch.Token);
            var windowSize = Task.Run(WindowSizeMonitor.CheckForWindowSizeChangesAsync, AppControl.MainKillSwitch.Token);
            var printingTask = Task.Run(() => LayoutHandler.HandleLayoutEvents(eventActionPairs), AppControl.MainKillSwitch.Token);

            Task.WaitAny(captureKeysTask, mainTask, windowSize, printingTask);

            AppControl.ResetConsoleState();
        }

        private ParentWindow GetParentWindow()
            => new(NavigationWindow, TasksWindow);

        private IPreviewWindow GetPreviewWindow()
            => new ConcretePreviewWindowFactory().GetPreviewWindow(NavigationWindow);

        private StatusBar GetStatusBar()
            => new(SelectedFile);

        public void PrintLayout()
        {
            if (ErrorLogWindow.IsCalled)
            {
                ErrorLogWindow.PrintLayout();
                return;
            }

            ParentWindow.PrintLayout();
            NavigationWindow.PrintLayout();
            PreviewWindow.PrintLayout();
            StatusBar.PrintDetails();

            if (LegendWindow.IsCalled)
            {
                LegendWindow.PrintLayout();
            }

            Console.SetCursorPosition(0, 0);
            MyResetEventSlims.MainWindowResetEvent.Reset();
        }

        /// <summary>
        /// Initializes a series of event-action pairs and enqueues them for later processing.
        /// Each pair links a reset event with an associated action (such as printing the layout).
        /// </summary>
        private void InitializeEventActionPairs()
        {
            EventActionPair mainWindowEvAcPa = new(MyResetEventSlims.MainWindowResetEvent, PrintLayout);
            EventActionPair backgroundEvAcPa = new(MyResetEventSlims.BackgroundResetEvent, () => TasksWindow?.PrintFullLayout());
            EventActionPair renameWindowEvAcPa = new(MyResetEventSlims.RenameResetEvent, () => RenameWindow?.PrintLayout());
            EventActionPair popUpEvAcPa = new(MyResetEventSlims.PopUpResetEvent, () => PopUpDialog?.PrintLayout());

            eventActionPairs.Enqueue(mainWindowEvAcPa);
            eventActionPairs.Enqueue(backgroundEvAcPa);
            eventActionPairs.Enqueue(renameWindowEvAcPa);
            eventActionPairs.Enqueue(popUpEvAcPa);
        }

        /// <summary>
        /// Handles file and directory navigation commands based on a specified key input.
        /// </summary>
        /// <param name="key">The console key input that triggers a navigation or action command.</param>
        /// <remarks>
        /// This method supports the following navigation and action commands:
        /// <list type="bullet">
        ///   <item>
        ///     <description><c>MoveLeft</c>: Navigates to the previous directory or location.</description>
        ///   </item>
        ///   <item>
        ///     <description><c>ToggleHidden</c>: Toggles the visibility of hidden files.</description>
        ///   </item>
        ///   <item>
        ///     <description><c>MoveDown</c> / <c>MoveUp</c>: Moves selection up or down in the file list.</description>
        ///   </item>
        ///   <item>
        ///     <description><c>MoveRight</c> / <c>Access</c>: Opens a directory if selected; opens a file if the Access key is used and the selection is not a directory.</description>
        ///   </item>
        /// </list>
        /// </remarks>
        private void NavigateFiles(ConsoleKeyInfo key)
        {
            try
            {
                switch (key)
                {
                    case var _ when KeyTypeCheck.IsMoveLeftKey(key):
                        NavigationWindow.Return();
                        return;
                    case var _ when KeyTypeCheck.IsToggleHiddenKey(key):
                        NavigationWindow.ToggleHidden();
                        return;
                }

                if (SelectedFile is null)
                {
                    return;
                }

                switch (key)
                {
                    case var _ when KeyTypeCheck.IsMoveDownKey(key):
                        NavigationWindow.MoveDown();
                        return;
                    case var _ when KeyTypeCheck.IsMoveUpKey(key):
                        NavigationWindow.MoveUp();
                        return;
                    case var _ when KeyTypeCheck.IsMoveRightKey(key):
                    case var _ when KeyTypeCheck.IsAccessKey(key):
                        if (fileSystem.Directory.Exists(SelectedFile.FullName))
                        {
                            NavigationWindow.Advance();
                        }
                        else if (KeyTypeCheck.IsAccessKey(key))
                        {
                            OpenFile();
                        }
                        return;
                }
            }
            catch (Exception exception)
            {
                ErrorLogWindow.AddFileError(FileManagerSettings.NavigationErrorMessage, exception);
            }
        }

        /// <summary>
        /// Asynchronously processes keys from a <see cref="ConcurrentStack{T}"/> and performs associated actions based on predefined commands.
        /// The method listens for specific key combinations and triggers events or cancels execution when certain keys are pressed.
        /// </summary>
        private async Task ProcessKeysAsync()
        {
            while (true)
            {
                AppControl.MainKillSwitch.Token.ThrowIfCancellationRequested();

                if (KeyStack.Keys.TryPeek(out ConsoleKeyInfo keyInfo))
                {
                    if (CommandKeys.GetKeys(CommandType.EndExecution).Keys.Any(keyList => keyList.Contains(keyInfo.Key)))
                    {
                        if (LegendWindow.IsCalled || ErrorLogWindow.IsCalled)
                        {
                            LegendWindow.CloseWindow();
                            ErrorLogWindow.CloseWindow();
                        }
                        else
                        {
                            AppControl.EndMainProgram();
                        }
                    }

                    if (CommandKeys.GetKeys(CommandType.ShowInfo).Keys.Any(keyList => keyList.Contains(keyInfo.Key)))
                    {
                        ShowInfo(keyInfo);
                    }
                    else if (!ErrorLogWindow.IsCalled && !LegendWindow.IsCalled)
                    {
                        if (CommandKeys.GetKeys(CommandType.AppFeatures).Keys.Any(keyList => keyList.Contains(keyInfo.Key)))
                        {
                            StartAppFeature(keyInfo);
                        }

                        if (CommandKeys.GetKeys(CommandType.FileManipulation).Keys.Any(keyList => keyList.Contains(keyInfo.Key)))
                        {
                            await EditSelectedFileEntryAsync(keyInfo);
                        }

                        if (CommandKeys.GetKeys(CommandType.Navigation).Keys.Any(keyList => keyList.Contains(keyInfo.Key)))
                        {
                            NavigateFiles(keyInfo);
                        }
                    }

                    KeyStack.Keys.Clear();
                    MyResetEventSlims.MainWindowResetEvent.Set();
                }

                Thread.Sleep(FileManagerSettings.ThrottleDelay);
            }
        }

        /// <summary>
        /// Displays a confirmation dialog to the user and, if confirmed, updates the clipboard
        /// with the specified command key and starts a background file manipulation task.
        /// </summary>
        /// <param name="keyInfo">The <see cref="ConsoleKeyInfo"/> associated with the user's requested command.</param>
        /// <remarks>
        /// This method creates a confirmation dialog using a <see cref="ConfirmationWindow"/> instance and displays it to the user.
        /// If the request is granted, the clipboard is updated with the provided <paramref name="keyInfo"/>,
        /// and the background file manipulation task is started.
        /// </remarks>
        private async Task ProceedWithConfirmationRequestAsync(ConsoleKeyInfo keyInfo)
        {
            PopUpDialog = new ConfirmationWindow();

            var isConfirmed = Task.Run(() => PopUpDialog.GetConfirmation());

            if (isConfirmed.Result)
            {
                MyClipboard.UpdateClipboard(keyInfo, NavigationWindow);
                PopUpDialog = null;
                RefreshMainWindow();
                await ExecuteBackgroundFileManipulationAsync();
            }
        }

        /// <summary>
        /// Handles file operations (delete, copy, cut, rename, and paste) for the currently selected file based on the user's key input.
        /// Performs the appropriate operation asynchronously and updates the UI afterward.
        /// </summary>
        /// <param name="key">The <see cref="ConsoleKeyInfo"/> representing the key pressed by the user.</param>
        /// <remarks>
        /// This method checks if a file is selected and then determines the operation to perform based on the key pressed.
        /// The method supports delete, copy, cut, rename, and paste operations, with each operation triggering a corresponding function.
        /// It also checks if the clipboard has source files before attempting to paste and starts a background task for file manipulation.
        /// Finally, the method refreshes the UI after the operation is complete.
        /// </remarks>
        private async Task EditSelectedFileEntryAsync(ConsoleKeyInfo key)
        {
            if (SelectedFile is not null)
            {
                await HandleFileOperationAsync(key);
            }

            if (MyClipboard.SourceFiles is not null
                && KeyTypeCheck.IsPasteKey(key))
            {
                await HandlePasteOperationAsync();
            }
        }

        private async Task HandleFileOperationAsync(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo)
            {
                case var _ when KeyTypeCheck.IsDeleteKey(keyInfo):
                    await ProceedWithConfirmationRequestAsync(keyInfo);
                    break;
                case var _ when KeyTypeCheck.IsCopyKey(keyInfo) || KeyTypeCheck.IsCutKey(keyInfo):
                    MyClipboard.UpdateClipboard(keyInfo, NavigationWindow);
                    break;
                case var _ when KeyTypeCheck.IsRenameKey(keyInfo):
                    await Task.Run(Rename);
                    break;
            }
        }

        private async Task HandlePasteOperationAsync()
        {
            MyClipboard.SetDestinationPath(NavigationWindow.GetCurrentFile()!.FullName);
            await ExecuteBackgroundFileManipulationAsync();
        }

        /// <summary>
        /// Starts a new file manipulation operation, by creating a new instance of <see cref="IBackgroundTasksWindow"/>
        /// in detailed mode, allowing for operation cancellation or sending it to the background.
        /// Updates the UI after the required command has been given.
        /// </summary>
        private async Task ExecuteBackgroundFileManipulationAsync()
        {
            var taskProgress = new ProgressBar();
            var op = new ConcreteIOOperationFactory().CreateIOOperation(fileSystem, taskProgress);
            var fileManipulationTask = Task.Run(op!.Execute, taskProgress.CancellationTokenSource.Token);

            taskProgress.LinkToTask(fileManipulationTask);
            MyTaskList.InitializeTasksWindow(taskProgress);
            TasksWindow = MyTaskList.GetTaskWindow();

            await Task.Run(() => TasksWindow!.DisplayWindow());

            TasksWindow = MyTaskList.GetTaskWindow();
            RefreshMainWindow();
        }

        /// <summary>
        /// Toggles visibility for different information windows based on key input.
        /// </summary>
        /// <param name="key">The console key input that triggers specific information display actions.</param>
        private void ShowInfo(ConsoleKeyInfo key)
        {
            switch (key)
            {
                case var _ when KeyTypeCheck.IsShowHelpKey(key):
                    LegendWindow.ToggleWindow();
                    return;
                case var _ when KeyTypeCheck.IsShowErrorLogKey(key):
                    ErrorLogWindow.ToggleWindow();
                    return;
                case var _ when KeyTypeCheck.IsToggleBackgroundTaskWindowKey(key):
                    SwitchBackgroundTasksWindow();
                    break;
            }
        }

        private void SwitchBackgroundTasksWindow()
        {
            MyTaskList.SwitchTaskWindowState();
            TasksWindow = MyTaskList.GetTaskWindow();
            MyResetEventSlims.BackgroundResetEvent.Set();

            if (TasksWindow is OverviewTaskWindow)
            {
                TasksWindow.DisplayWindow();
                TasksWindow = MyTaskList.GetTaskWindow();
            }
        }

        /// <summary>
        /// Opens the currently selected file using the system's default application for the file type.
        /// </summary>
        /// <remarks>
        /// It uses the default application associated with the file extension on the host system.
        /// If an error occurs while attempting to open the file, the error is logged with the file path.
        /// </remarks>
        private void OpenFile()
        {
            if (SelectedFile is null)
            {
                return;
            }

            try
            {
                using var process = new Process();
                process.StartInfo.FileName = SelectedFile.FullName;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
            catch (Exception exception)
            {
                ErrorLogWindow.AddFileError($"Failed to open file: {SelectedFile.FullName}", exception);
            }
        }

        private void Rename()
        {
            var selectedFile = SelectedFile;
            RenameWindow = new RenameWindow(NavigationWindow);
            var newPath = Task.Run(() => RenameWindow.GetNewFileName()).Result;
            RenameWindow = null;

            try
            {
                if (!newPath.Equals(selectedFile!.FullName))
                {
                    if (fileSystem.Directory.Exists(selectedFile.FullName))
                    {
                        fileSystem.Directory.Move(selectedFile.FullName, newPath);
                    }
                    else
                    {
                        fileSystem.File.Move(selectedFile.FullName, newPath);
                    }
                }
            }
            catch (Exception exception) when (
            exception is PathTooLongException
            || exception is DirectoryNotFoundException
            || exception is FileNotFoundException
            || exception is ArgumentNullException
            || exception is ArgumentException
            || exception is UnauthorizedAccessException
            || exception is NotSupportedException
            || exception is IOException)
            {
                ErrorLogWindow.AddFileError(selectedFile!.FullName, exception);
            }
        }

        /// <summary>
        /// Refreshes the main window by updating its size and refreshing the navigation window content.
        /// </summary>
        private void RefreshMainWindow()
        {
            WindowSizeMonitor.UpdateWindowSize();
            NavigationWindow.Refresh();
        }

        #endregion
    }
}
