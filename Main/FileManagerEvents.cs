namespace FileManager.Main
{
    /// <summary>
    /// Implementation of the <see cref="FileManagerEvents"/> class.
    /// </summary>
    public static class FileManagerEvents
    {
        public static event EventHandler<ConsoleKeyEventArgs>? EditFile;
        public static event EventHandler<ConsoleKeyEventArgs>? NavigateFiles;
        public static event EventHandler<ConsoleKeyEventArgs>? ShowInfo;
        public static event EventHandler<ConsoleKeyEventArgs>? ToggleAppFeature;

        public static void RaiseEditFile(ConsoleKeyInfo keyInfo)
            => EditFile?.Invoke(null, new ConsoleKeyEventArgs(keyInfo));

        public static void RaiseNavigateFiles(ConsoleKeyInfo keyInfo)
            => NavigateFiles?.Invoke(null, new ConsoleKeyEventArgs(keyInfo));

        public static void RaiseShowInfo(ConsoleKeyInfo keyInfo)
            => ShowInfo?.Invoke(null, new ConsoleKeyEventArgs(keyInfo));

        public static void RaiseStartAppFeature(ConsoleKeyInfo keyInfo)
            => ToggleAppFeature?.Invoke(null, new ConsoleKeyEventArgs(keyInfo));
    }
}
