namespace FileManager.KeyMapping
{
    /// <summary>
    /// Holds a collection (<see cref="ConcurrentStack{T}"/>) of keys pressed during execution.
    /// </summary>
    public static class KeyStack
    {
        private static readonly ConcurrentStack<ConsoleKeyInfo> keys = new();

        public static ConcurrentStack<ConsoleKeyInfo> Keys
            => keys;

        /// <summary>
        /// Asynchronously captures keyboard input and stores it in a <see cref="ConcurrentStack{T}"/> while the <see cref="CancellationToken"/> is not triggered.
        /// </summary>
        /// <remarks>
        /// This method uses a debounce timer to limit the frequency of key handling, preventing high CPU usage.
        /// It runs in a separate task and periodically checks for cancellation requests.
        /// </remarks>
        public static Task CaptureKeysAsync()
        {
            var debounceTimer = new Stopwatch();
            debounceTimer.Start();

            while (true)
            {
                AppControl.MainKillSwitch.Token.ThrowIfCancellationRequested();

                var keyInfo = Console.ReadKey(true);
                keys.Push(keyInfo);

                if (debounceTimer.ElapsedMilliseconds > FileManagerSettings.DebounceDelay)
                {
                    debounceTimer.Restart();
                }

                Thread.Sleep(FileManagerSettings.DebounceDelay);
            }
        }
    }
}
