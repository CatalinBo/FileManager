namespace FileManager.Tasks
{
    public interface IProgressBar
    {
        CancellationTokenSource CancellationTokenSource { get; }
        List<IFileInfo>? SourceFiles { get; }
        string SourcePath { get; }
        string? TargetPath { get; }
        ConsoleKeyInfo CommandKey { get; }
        int FilesProcessed { get; }
        int TotalFiles { get; }
        TaskStatus? TaskStatus { get; }
        string GetOperationMessage();
        void UpdateProgress(string filePath);
    }
}
