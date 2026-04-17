namespace FileManager.PopUp
{
    /// <summary>
    /// Provides methods to generate and print window layout.
    /// </summary>
    /// <param name="message">A <see cref="string"/> that will appear inside the <see cref="PopUpDialog"/> window.</param>
    public class PopUpDialog(string message = "")
    {
        protected CancellationTokenSource cts = new();
        protected string message = message;
        protected int selectedOptionIndex = 0;
        protected int numberOfOptions = Enum.GetNames(typeof(PopUpOptions)).Length;

        /// <summary>
        /// Width of <see cref="PopUpDialog"/> window.
        /// </summary>
        protected virtual int WindowWidth
            => PopUpDialogSettings.GetWindowWidth(this);

        /// <summary>
        /// Gets the message that must be printed inside the <see cref="PopUpDialog"/> window.
        /// </summary>
        /// <returns>The message that must be printed inside the <see cref="PopUpDialog"/> window.</returns>
        public virtual string GetWindowMessage()
            => message;

        public virtual Task<bool> GetConfirmation()
        {
            MyResetEventSlims.PopUpResetEvent.Set();

            while (true)
            {
                cts.Token.ThrowIfCancellationRequested();

                if (KeyStack.Keys.TryPeek(out var key))
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.RightArrow:
                            MoveNext();
                            break;
                        case ConsoleKey.LeftArrow:
                            MovePrevious();
                            break;
                        case ConsoleKey.Enter:
                            KeyStack.Keys.Clear();
                            MyResetEventSlims.PopUpResetEvent.Reset();
                            return Task.FromResult(GetResponseFromUser());
                        case ConsoleKey.Escape:
                            cts.Cancel();
                            MyResetEventSlims.PopUpResetEvent.Reset();
                            return Task.FromResult(false);
                        default:
                            break;
                    }
                }

                KeyStack.Keys.Clear();
                MyResetEventSlims.PopUpResetEvent.Set();
                Thread.Sleep(FileManagerSettings.ThrottleDelay);
            }
        }

        #region Navigate through options methods

        public virtual void MoveNext()
            => selectedOptionIndex = (selectedOptionIndex + 1) % numberOfOptions;

        public virtual void MovePrevious()
            => selectedOptionIndex = (selectedOptionIndex - 1 + numberOfOptions) % numberOfOptions;

        public virtual bool GetResponseFromUser()
            => selectedOptionIndex == (int)PopUpOptions.Ok;

        #endregion

        #region Window layout generating and printing methods

        /// <summary>
        /// Prints the entire <see cref="PopUpDialog"/> window layout, by using helper methods.
        /// </summary>
        public virtual void PrintLayout()
        {
            ConsoleManager.InitializeCursorPosition(PopUpDialogSettings.GetStartingColumn(this), PopUpDialogSettings.StartingRow);
            PopUpDialogSettings.SetConsoleColors();

            PrintTopBorder();
            PrintWindowMessage();
            PrintBottomBorder();
            MyResetEventSlims.PopUpResetEvent.Reset();
        }

        public virtual void PrintTopBorder()
        {
            Console.Write(PopUpDialogSettings.GetTopBorder(WindowWidth));
            ConsoleManager.JumpToNewLine(PopUpDialogSettings.GetStartingColumn(this));
        }

        public virtual void PrintWindowMessage()
        {
            Console.Write($"{PopUpDialogSettings.LeftBorder}{GetWindowMessage()}{PopUpDialogSettings.RightBorder}");
            ConsoleManager.JumpToNewLine(PopUpDialogSettings.GetStartingColumn(this));
        }

        public virtual void PrintBottomBorder()
            => Console.Write(PopUpDialogSettings.GetBottomBorder(WindowWidth));

        #endregion
    }
}
