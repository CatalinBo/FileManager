namespace FileManager.FileOperation
{
    public class ConcreteIOOperationFactory : IOOperationFactory
    {
        public override IIOOperation? CreateIOOperation(IFileSystem fileSystem, IProgressBar progressBar)
        {
            var key = progressBar.CommandKey;

            return key switch
            {
                var _ when KeyTypeCheck.IsDeleteKey(key) => new Delete(fileSystem, progressBar),
                var _ when KeyTypeCheck.IsCopyKey(key) => new Copy(fileSystem, progressBar),
                var _ when KeyTypeCheck.IsCutKey(key) => new Move(fileSystem, progressBar),
                _ => null,
            };
        }
    }
}
