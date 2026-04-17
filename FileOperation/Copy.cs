namespace FileManager.FileOperation
{
    public class Copy(IFileSystem fileSystem, IProgressBar progressBar) : IIOOperation
    {
        private readonly IFileSystem fileSystem = fileSystem;
        private readonly IProgressBar progressBar = progressBar;
        private readonly List<IFileInfo>? sourceFiles = progressBar.SourceFiles;

        public void Execute()
        {
            if (sourceFiles is null)
            {
                return;
            }

            foreach (var file in sourceFiles)
            {
                if (fileSystem.Directory.Exists(file.FullName))
                {
                    var dir = fileSystem.DirectoryInfo.New(file.FullName);
                    var destinationPath = Path.Combine(progressBar.TargetPath!, dir.Name);
                    CopyEntry(dir, destinationPath);
                }
                else
                {
                    CopyEntry(file);
                }
            }
        }

        private void CopyEntry(IFileInfo fileInfo)
        {
            try
            {
                var destinationPath = Path.Combine(progressBar.TargetPath!, fileInfo.Name);
                fileSystem.File.Copy(fileInfo.FullName, destinationPath, false);
                progressBar.UpdateProgress(fileInfo.FullName);
            }
            catch (Exception exception) when (
            exception is UnauthorizedAccessException
            || exception is ArgumentNullException
            || exception is ArgumentException
            || exception is PathTooLongException
            || exception is DirectoryNotFoundException
            || exception is FileNotFoundException
            || exception is NotSupportedException
            || exception is IOException)
            {
                ErrorLogWindow.AddFileError(fileInfo.FullName, exception);
            }
        }

        private void CopyEntry(IDirectoryInfo directoryInfo, string destinationPath)
        {
            IEnumerable<IFileInfo> files;
            IEnumerable<IDirectoryInfo> directories;

            try
            {
                files = directoryInfo.EnumerateFiles();
                directories = directoryInfo.EnumerateDirectories();
            }
            catch (Exception exception) when (
            exception is UnauthorizedAccessException
            || exception is DirectoryNotFoundException
            || exception is SecurityException
            || exception is ArgumentNullException
            || exception is ArgumentOutOfRangeException)
            {
                ErrorLogWindow.AddFileError(directoryInfo.FullName, exception);
                return;
            }

            if (!fileSystem.Directory.Exists(destinationPath))
            {
                fileSystem.Directory.CreateDirectory(destinationPath);
            }

            foreach (var file in files)
            {
                progressBar.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                try
                {
                    var tempPath = Path.Combine(destinationPath, file.Name);
                    file.CopyTo(tempPath);
                    progressBar.UpdateProgress(file.FullName);
                }
                catch (Exception exception) when (
                exception is ArgumentNullException
                || exception is ArgumentException
                || exception is SecurityException
                || exception is UnauthorizedAccessException
                || exception is DirectoryNotFoundException
                || exception is PathTooLongException
                || exception is NotSupportedException
                || exception is IOException)
                {
                    ErrorLogWindow.AddFileError(file.FullName, exception);
                }
            }

            foreach (var subDir in directories)
            {
                progressBar.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                var tempPath = Path.Combine(destinationPath, subDir.Name);
                CopyEntry(subDir, tempPath);
            }
        }
    }
}
