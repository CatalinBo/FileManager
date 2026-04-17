namespace FileManager.ErrorLog
{
    /// <summary>
    /// Interface used for <see cref="ErrorLogWindow"/> methods abstractization.
    /// </summary>
    public interface IErrorLog
    {
        void AddFileError(string path, Exception exception);

        void ClearErrorLogs();

        ConcurrentDictionary<string, Exception> GetFileErrors();
    }
}
