namespace FileManager.Rename
{
    public static class CursorPositionHandler
    {
        private static (int PositionWithinName, int PositionWithinWindow) cursorPosition = (0, 0);
        private static (int Left, int Right) displayRange = (0, 0);

        #region Cursor Position and Display Range properties

        public static int PositionWithinName
        {
            get => cursorPosition.PositionWithinName;
            private set => cursorPosition.PositionWithinName = value;
        }

        public static int PositionWithinWindow
        {
            get => cursorPosition.PositionWithinWindow;
            private set => cursorPosition.PositionWithinWindow = value;
        }

        public static int StartingDisplayRange
        {
            get => displayRange.Left;
            private set => displayRange.Left = value;
        }

        public static int EndingDisplayRange
        {
            get => displayRange.Right;
            private set => displayRange.Right = value;
        }

        #endregion

        #region Update methods

        public static void UpdateCursorPosition(int posWithinName = -1, int posWithinWindow = -1)
        {
            PositionWithinName = posWithinName >= 0 ? posWithinName : PositionWithinName;
            PositionWithinWindow = posWithinWindow >= 0 ? posWithinWindow : PositionWithinWindow;
        }

        public static void UpdateDisplayRange(int startingPos = -1, int endingPos = -1)
        {
            StartingDisplayRange = startingPos >= 0 ? startingPos : StartingDisplayRange;
            EndingDisplayRange = endingPos >= 0 ? endingPos : EndingDisplayRange;
        }

        #endregion

        #region Cursor and Display range initialization

        public static void InitializeCursorPosition(int wordLength, int textBoxWidth)
        {
            cursorPosition.PositionWithinName = wordLength;
            cursorPosition.PositionWithinWindow = wordLength > textBoxWidth
                ? RenameWindowSettings.TextBoxStartingColumn + textBoxWidth
                : RenameWindowSettings.TextBoxStartingColumn + wordLength;
        }

        public static void InitializeDisplayRange(int wordLength, int textBoxWidth)
        {
            displayRange.Left = Math.Max(wordLength - textBoxWidth, 0);
            displayRange.Right = wordLength;
        }

        #endregion
    }
}
