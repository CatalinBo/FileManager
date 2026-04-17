namespace FileManager.ErrorLog
{
    /// <summary>
    /// Non static class that copies <see cref="ErrorLogWindow"/> behaviour, enabling mocking and testing.
    /// </summary>
    /// <param name="errorLogger">Mocked instance of <see cref="IErrorLog"/> used during testing.</param>
    public class ErrorLogProxy(IErrorLog errorLogger)
    {
        private readonly IErrorLog errorLogger = errorLogger;

        public void AddFileError(string path, Exception exception)
            => errorLogger.AddFileError(path, exception);

        public void ClearErrorLogs()
            => errorLogger.ClearErrorLogs();

        public ConcurrentDictionary<string, Exception> GetErrorLogs()
            => errorLogger.GetFileErrors();
    }
}
