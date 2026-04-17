namespace FileManager.Utility
{
    /// <summary>
    /// Provides methods to control the execution of certain application components.
    /// </summary>
    public static class AppControl
    {
        public static CancellationTokenSource MainKillSwitch { get; private set; } = new();

        /// <summary>
        /// Ends the execution of the main program, by cancelling the main <see cref="CancellationTokenSource.Token"/>.
        /// </summary>
        public static void EndMainProgram()
            => MainKillSwitch.Cancel();

        /// <summary>
        /// Resets the <see cref="Console"/> to its default state.
        /// </summary>
        public static void ResetConsoleState()
        {
            Console.WriteLine(FileManagerSettings.EscapeSeq);
            Console.CursorVisible = true;
        }
    }
}
