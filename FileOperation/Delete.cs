namespace FileManager.FileOperation
{
    public class Delete(IFileSystem fileSystem, IProgressBar progressBar) : IIOOperation
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

            foreach (var entry in sourceFiles)
            {
                if (fileSystem.Directory.Exists(entry.FullName))
                {
                    var dir = fileSystem.DirectoryInfo.New(entry.FullName);
                    DeleteEntry(dir);
                }
                else
                {
                    DeleteEntry(entry);
                }
            }
        }

        private void DeleteEntry(IFileInfo fileInfo)
        {
            try
            {
                fileSystem.File.Delete(fileInfo.FullName);
                progressBar.UpdateProgress(fileInfo.FullName);
            }
            catch (Exception exception) when (
            exception is ArgumentException
            || exception is DirectoryNotFoundException
            || exception is NotSupportedException
            || exception is UnauthorizedAccessException
            || exception is IOException)
            {
                ErrorLogWindow.AddFileError(fileInfo.FullName, exception);
            }
        }

        private void DeleteEntry(IDirectoryInfo directoryInfo)
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

            foreach (var file in files)
            {
                progressBar.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                try
                {
                    fileSystem.File.Delete(file.FullName);
                    progressBar.UpdateProgress(file.FullName);
                }
                catch (Exception exception) when (
                exception is ArgumentNullException
                || exception is ArgumentException
                || exception is DirectoryNotFoundException
                || exception is PathTooLongException
                || exception is NotSupportedException
                || exception is UnauthorizedAccessException
                || exception is IOException)
                {
                    ErrorLogWindow.AddFileError(file.FullName, exception);
                }
            }

            foreach (var subDir in directories)
            {
                progressBar.CancellationTokenSource.Token.ThrowIfCancellationRequested();
                DeleteEntry(subDir);
            }

            progressBar.CancellationTokenSource.Token.ThrowIfCancellationRequested();

            try
            {
                directoryInfo.Attributes &= ~(FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly);
                fileSystem.Directory.Delete(directoryInfo.FullName);
            }
            catch (IOException exception)
            {
                ErrorLogWindow.AddFileError(directoryInfo.FullName, exception);
            }
        }
    }
}
