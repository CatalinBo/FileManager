namespace FileManager.Tasks
{
    public class ProgressBar : IProgressBar
    {
        private readonly CancellationTokenSource cts = new();
        private readonly FileSystem fileSystem = new();
        private Task? correspondingTask;

        private readonly List<IFileInfo>? sourceFiles;
        private readonly string? destinationPath;
        private readonly ConsoleKeyInfo commandKey;

        private int totalFiles;
        private int filesProcessed = 0;

        public ProgressBar()
        {
            sourceFiles = MyClipboard.SourceFiles is not null ? new(MyClipboard.SourceFiles) : null;
            destinationPath = MyClipboard.DestinationPath;
            commandKey = MyClipboard.CommandKey;
            SourcePath = string.Empty;
            correspondingTask = null;

            var countingTask = Task.Run(() => CountFiles(ref totalFiles), cts.Token);
        }

        public List<IFileInfo>? SourceFiles
            => sourceFiles;

        public ConsoleKeyInfo CommandKey
            => commandKey;

        public string SourcePath { get; private set; }

        public string? TargetPath
            => destinationPath;

        public CancellationTokenSource CancellationTokenSource
            => cts;

        public int FilesProcessed
            => filesProcessed;

        public int TotalFiles
            => totalFiles;

        public TaskStatus? TaskStatus
            => correspondingTask?.Status;

        public void LinkToTask(Task task)
            => correspondingTask = task;

        public string GetOperationMessage()
        {
            var keyMessageMap = new Dictionary<List<ConsoleKey>, string>
            {
                { CommandKeys.DeleteKeys, BackgroundTasksWindowSettings.DeleteMessage },
                { CommandKeys.CutKeys, BackgroundTasksWindowSettings.MoveMessage },
                { CommandKeys.CopyKeys, BackgroundTasksWindowSettings.CopyMessage }
            };

            return keyMessageMap
                .FirstOrDefault(kmm => kmm.Key.Contains(CommandKey.Key))
                .Value ?? string.Empty;
        }

        public void UpdateProgress(string newSourcePath)
        {
            cts.Token.ThrowIfCancellationRequested();
            SourcePath = newSourcePath;
            Interlocked.Increment(ref filesProcessed);
            MyResetEventSlims.BackgroundResetEvent.Set();
        }

        /// <summary>
        /// Counts all files that are found in the <see cref="MyClipboard.SourceFiles"/> collection.
        /// </summary>
        /// <param name="totalFiles">Value that holds total files counter.</param>
        private void CountFiles(ref int totalFiles)
        {
            if (sourceFiles is null)
            {
                return;
            }

            foreach (var file in sourceFiles)
            {
                try
                {
                    if (fileSystem.Directory.Exists(file.FullName))
                    {
                        var directory = fileSystem.DirectoryInfo.New(file.FullName);
                        var fileEnumeration = directory.EnumerateFiles("*", SearchOption.AllDirectories);

                        foreach (var singleFile in fileEnumeration)
                        {
                            cts.Token.ThrowIfCancellationRequested();
                            Interlocked.Increment(ref totalFiles);
                        }
                    }
                    else
                    {
                        Interlocked.Increment(ref totalFiles);
                    }
                }
                catch (Exception exception)
                {
                    ErrorLogWindow.AddFileError(file.FullName, exception);
                }
            }
        }
    }
}
