namespace FileManager.Utility
{
    public class ConsoleKeyEventArgs(ConsoleKeyInfo keyInfo) : EventArgs
    {
        public ConsoleKeyInfo KeyInfo { get; } = keyInfo;
    }
}
