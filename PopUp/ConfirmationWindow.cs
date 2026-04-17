namespace FileManager.PopUp
{
    /// <summary>
    /// Inherits properties and methods from <see cref="PopUpDialog"/> and deals with confirmation dialogues.
    /// </summary>
    public class ConfirmationWindow : PopUpDialog
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConfirmationWindow"/>.
        /// </summary>
        public ConfirmationWindow() : base()
        {
            selectedOptionIndex = 0;
            numberOfOptions = Enum.GetNames(typeof(ConfirmationOptions)).Length;
        }

        protected override int WindowWidth
            => PopUpDialogSettings.GetWindowWidth(this);

        public override string GetWindowMessage()
            => PopUpDialogSettings.GetConfirmationRequestMessage();

        public override bool GetResponseFromUser()
            => selectedOptionIndex == (int)ConfirmationOptions.Yes;

        public override void PrintLayout()
        {
            if (NavigationWindowSettings.WindowHeight <= PopUpDialogSettings.MinimumWindowHeight)
            {
                return;
            }

            ConsoleManager.InitializeCursorPosition(PopUpDialogSettings.GetStartingColumn(this), PopUpDialogSettings.StartingRow);
            PopUpDialogSettings.SetConsoleColors();

            PrintTopBorder();
            PrintWindowMessage();
            PrintOptions();
            PrintBottomBorder();
            MyResetEventSlims.PopUpResetEvent.Reset();

            void PrintOptions()
            {
                var optionWidth = (WindowWidth - PopUpDialogSettings.LeftBorder.Length - PopUpDialogSettings.RightBorder.Length) / numberOfOptions;

                Console.Write(PopUpDialogSettings.LeftBorder);

                foreach (var option in Enum.GetNames(typeof(ConfirmationOptions)))
                {
                    PrintOption(option);
                }

                PopUpDialogSettings.SetConsoleColors();
                Console.Write(PopUpDialogSettings.RightBorder.PadLeft(PopUpDialogSettings.RightBorder.Length + 1));
                ConsoleManager.JumpToNewLine(PopUpDialogSettings.GetStartingColumn(this));

                void PrintOption(string option)
                {
                    var index = Array.IndexOf(Enum.GetNames(typeof(ConfirmationOptions)), option);
                    var isSelected = index == selectedOptionIndex;

                    PopUpDialogSettings.SetConsoleColors(isSelected);
                    Console.Write(StringManipulator.GetCenteredText(option, optionWidth));
                    Console.ResetColor();
                }
            }
        }
    }
}
