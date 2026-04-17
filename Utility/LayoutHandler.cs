namespace FileManager.Utility
{
    public class LayoutHandler
    {
        /// <summary>
        /// Continuously monitors and executes actions associated with given set events.
        /// </summary>
        public static Task HandleLayoutEvents(ConcurrentQueue<EventActionPair> eventActionPairs)
        {
            Console.CursorVisible = false;

            while (true)
            {
                AppControl.MainKillSwitch.Token.ThrowIfCancellationRequested();

                try
                {
                    foreach (var pair in eventActionPairs)
                    {
                        if (pair.ResetEventSlim.IsSet)
                        {
                            pair.Action();
                        }
                    }
                }
                catch
                {
                    // getting an exception here is ok. handles cases when there's some slowing in generating components.
                }

                Thread.Sleep(FileManagerSettings.ThrottleDelay);
            }
        }
    }
}
