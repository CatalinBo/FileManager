namespace FileManager.ErrorLog
{
    /// <summary>
    /// Represents a wrapper around <see cref="ErrorLogWindow"/> interactions, allowing mocking and testing.
    /// </summary>
    public class ErrorLogWrapper : IErrorLog
    {
        public void AddFileError(string path, Exception exception)
            => ErrorLogWindow.AddFileError(path, exception);

        public void ClearErrorLogs()
            => ErrorLogWindow.ClearErrorLogs();

        public ConcurrentDictionary<string, Exception> GetFileErrors()
            => ErrorLogWindow.GetFileErrors();
    }
}
