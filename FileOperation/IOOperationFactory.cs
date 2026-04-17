namespace FileManager.FileOperation
{
    public abstract class IOOperationFactory
    {
        public abstract IIOOperation? CreateIOOperation(IFileSystem fileSystem, IProgressBar progressBar);
    }
}
