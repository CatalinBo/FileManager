namespace FileManager.Rename
{
    /// <summary>
    /// Provides methods and properties to proper handle directory or file renaming.
    /// </summary>
    public class RenameWindow
    {
        private readonly INavigationWindow navigationWindow;
        private readonly string? oldFileName;
        private readonly string parentPath;
        private readonly string fileExtension;
        private readonly StringBuilder? newFileName;
        

        /// <summary>
        /// Initializes a new instance of <see cref="RenameWindow"/>.
        /// </summary>
        /// <param name="navWindow">Current <see cref="NavigationWindow"/> of the <see cref="FileManager"/>.</param>
        public RenameWindow(INavigationWindow navWindow)
        {
            navigationWindow = navWindow;
            oldFileName = Path.GetFileNameWithoutExtension(navigationWindow.SelectedFile?.FullName);
            parentPath = navigationWindow.GetCurrentFile()!.FullName;
            fileExtension = Path.GetExtension(navigationWindow.SelectedFile!.FullName);
            newFileName = new StringBuilder(oldFileName);
            CursorPositionHandler.InitializeCursorPosition(newFileName!.Length, RenameWindowSettings.TextBoxWidth);
            CursorPositionHandler.InitializeDisplayRange(newFileName!.Length, RenameWindowSettings.TextBoxWidth);
            
        }

        /// <summary>
        /// Column at which the text box starts, relative to the console window.
        /// </summary>
        private static int TextBoxStartingColumn { get; set; } = RenameWindowSettings.TextBoxStartingColumn;

        /// <summary>
        /// Allows editing of a string, representing the new file name of the selected file.
        /// </summary>
        /// <returns>A <see cref="string"/> representing the new name of the selected file.</returns>
        public string GetNewFileName()
        {
            MyResetEventSlims.RenameResetEvent.Set();

            while (true)
            {
                if (KeyStack.Keys.TryPeek(out ConsoleKeyInfo key))
                {
                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:
                            Console.CursorVisible = false;
                            var newName = newFileName!.Equals("") ? oldFileName! : newFileName!.ToString();
                            var newPath = Path.Combine(parentPath, newName) + fileExtension;
                            return newPath;
                        case ConsoleKey.LeftArrow:
                            MoveLeft(key);
                            break;
                        case ConsoleKey.RightArrow:
                            MoveRight(key);
                            break;
                        case ConsoleKey.Backspace:
                            Delete(key);
                            break;
                        case ConsoleKey.Escape:
                            Console.CursorVisible = false;
                            return Path.Combine(parentPath, oldFileName!) + fileExtension;
                        default:
                            InsertChar(key);
                            break;
                    }

                    KeyStack.Keys.Clear();
                }

                if (WindowSizeMonitor.WindowSizeHasChanged())
                {
                    HandleWindowSizeChange();
                }

                MyResetEventSlims.RenameResetEvent.Set();
                Thread.Sleep(FileManagerSettings.ThrottleDelay);
            }
        }

        /// <summary>
        /// Prints the entire layout of <see cref="RenameWindow"/>.
        /// </summary>
        public void PrintLayout()
        {
            Console.CursorVisible = false;
            ConsoleManager.InitializeCursorPosition(RenameWindowSettings.LabelStartingColumn, navigationWindow.SelectedFileRow);
            ConsoleManager.InitializeConsoleColors(RenameWindowSettings.ForegroundColor, RenameWindowSettings.BackgroundColor);

            PrintLabelMessage();
            PrintTextBox();

            Console.SetCursorPosition(CursorPositionHandler.PositionWithinWindow, navigationWindow.SelectedFileRow);
            Console.CursorVisible = true;
            Console.ResetColor();
            MyResetEventSlims.RenameResetEvent.Reset();

            void PrintLabelMessage() 
                => Console.Write(StringManipulator.TruncateString(RenameWindowSettings.LabelMessage, RenameWindowSettings.LabelBoxWidth).PadRight(RenameWindowSettings.LabelBoxWidth, ' '));

            void PrintTextBox()
            {
                Console.SetCursorPosition(RenameWindowSettings.TextBoxStartingColumn, navigationWindow.SelectedFileRow);
                Console.Write(newFileName!.ToString()[CursorPositionHandler.StartingDisplayRange..CursorPositionHandler.EndingDisplayRange].PadRight(RenameWindowSettings.TextBoxWidth, ' '));
            }
        }

        #region Word editor navigation methods (Move left/right, Jump to next/previous, etc.)

        /// <summary>
        /// If <see cref="ConsoleModifiers.Control"/> is pressed, jumps to previous word, otherwise moves to previous character.
        /// </summary>
        /// <param name="key">The key that contains (or not) the <see cref="ConsoleModifiers.Control"/>.</param>
        private void MoveLeft(ConsoleKeyInfo key)
        {
            if (newFileName!.Length == 0)
            {
                return;
            }

            if (key.Modifiers == ConsoleModifiers.Control)
            {
                JumpToPrevious();
            }
            else
            {
                DecrementCursorPositionWithinName();
                DecrementCursorPositionWithinWindow();
                DecrementDisplayRange();
            }
        }

        /// <summary>
        /// Moves the cursor to the previous word separator.
        /// </summary>
        private void JumpToPrevious()
        {
            var index = GetIndexOfWordSeparator(false);

            if (index > -1)
            {
                CursorPositionHandler.UpdateCursorPosition(posWithinName: index + 1);
                DecrementDisplayRange();
                CursorPositionHandler.UpdateCursorPosition(posWithinWindow: TextBoxStartingColumn + (CursorPositionHandler.PositionWithinName - CursorPositionHandler.StartingDisplayRange));
            }
            else
            {
                CursorPositionHandler.UpdateCursorPosition(posWithinName: 0);
                DecrementDisplayRange();
                CursorPositionHandler.UpdateCursorPosition(posWithinWindow: TextBoxStartingColumn);
            }
        }

        /// <summary>
        /// If <see cref="ConsoleModifiers.Control"/> is pressed, jumps to next word, otherwise moves to next character.
        /// </summary>
        /// <param name="key">The key that contains (or not) the <see cref="ConsoleModifiers.Control"/>.</param>
        private void MoveRight(ConsoleKeyInfo key)
        {
            if (newFileName!.Length == 0)
            {
                return;
            }

            if (key.Modifiers == ConsoleModifiers.Control)
            {
                JumpToNext();
            }
            else
            {
                IncrementCursorPositionWithinName();
                IncrementCursorPositionWithinWindow();
                IncrementDisplayRange();
            }
        }

        /// <summary>
        /// Moves the cursor to the next word separator.
        /// </summary>
        private void JumpToNext()
        {
            var index = GetIndexOfWordSeparator(ascendingOrder: true);

            if (index > -1)
            {
                CursorPositionHandler.UpdateCursorPosition(posWithinName: index + 1);
                IncrementDisplayRange();
                CursorPositionHandler.UpdateCursorPosition(posWithinWindow: TextBoxStartingColumn + (CursorPositionHandler.PositionWithinName - CursorPositionHandler.StartingDisplayRange));
            }
            else
            {
                CursorPositionHandler.UpdateCursorPosition(posWithinName: newFileName!.Length);
                IncrementDisplayRange();
                CursorPositionHandler.UpdateCursorPosition(posWithinWindow: TextBoxStartingColumn + (CursorPositionHandler.PositionWithinName - CursorPositionHandler.StartingDisplayRange));
            }
        }

        /// <summary>
        /// Gets the index of the next word separator.
        /// </summary>
        /// <param name="ascendingOrder">A <see cref="bool"/> that indicates the navigation orientation.
        /// <see cref="true"/> will jump the cursor to the next word separator, while <see cref="false"/> will jump the cursor to the previous word separator.</param>
        /// <returns>Index of the found word separator, and -1 if no word separator is found.</returns>
        private int GetIndexOfWordSeparator(bool ascendingOrder)
        {
            int position;
            bool isSeparator;

            if (ascendingOrder)
            {
                position = Math.Min(CursorPositionHandler.PositionWithinName, newFileName!.Length - 1);
                isSeparator = FileManagerSettings.WordSeparators.Contains(newFileName[position]);

                for (var i = position + 1; i < newFileName!.Length; i++)
                {
                    if ((isSeparator && !FileManagerSettings.WordSeparators.Contains(newFileName![i]))
                        || (!isSeparator && FileManagerSettings.WordSeparators.Contains(newFileName![i])))
                    {
                        return i - 1;
                    }
                }
            }
            else
            {
                position = Math.Max(0, CursorPositionHandler.PositionWithinName - 1);
                isSeparator = FileManagerSettings.WordSeparators.Contains(newFileName![position]);

                for (var i = position - 1; i >= 0; i--)
                {
                    if ((isSeparator && !FileManagerSettings.WordSeparators.Contains(newFileName![i]))
                        || (!isSeparator && FileManagerSettings.WordSeparators.Contains(newFileName![i])))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        #endregion

        #region Word Editor methods (Insert, Delete, etc.)

        /// <summary>
        /// Inserts a char at the cursor position whithin name.
        /// </summary>
        /// <param name="keyInfo">The char to be inserted.</param>
        private void InsertChar(ConsoleKeyInfo keyInfo)
        {
            var ch = keyInfo.KeyChar;

            if (!char.IsControl(ch))
            {
                newFileName!.Insert(CursorPositionHandler.PositionWithinName, ch);
                IncrementCursorPositionWithinName();
                IncrementCursorPositionWithinWindow();
                IncrementDisplayRange();
            }
        }

        /// <summary>
        /// Deletes a character (or a word if <see cref="ConsoleModifiers.Control"/> is pressed).
        /// </summary>
        /// <param name="key"><see cref="ConsoleKeyInfo"/> that contains <see cref="ConsoleModifiers.Control"/>. 
        /// Deletes the word if <see cref="ConsoleModifiers.Control"/> is pressed.</param>
        private void Delete(ConsoleKeyInfo key)
        {
            if (key.Modifiers == ConsoleModifiers.Control)
            {
                RemoveWord(key);
            }
            else if (newFileName!.Length > 0
                && CursorPositionHandler.PositionWithinName != 0)
            {
                RemoveChar();
            }
        }

        /// <summary>
        /// Removes the character at which the cursor is currently positioned.
        /// </summary>
        private void RemoveChar()
        {
            newFileName!.Remove(CursorPositionHandler.PositionWithinName - 1, 1);
            DecrementCursorPositionWithinName();
            DecrementCursorPositionWithinWindow();

            if (CursorPositionHandler.PositionWithinWindow == RenameWindowSettings.TextBoxStartingColumn)
            {
                if (CursorPositionHandler.PositionWithinName > RenameWindowSettings.TextBoxWidth)
                {
                    CursorPositionHandler.UpdateDisplayRange(endingPos: CursorPositionHandler.PositionWithinName);
                    CursorPositionHandler.UpdateDisplayRange(Math.Max(0, CursorPositionHandler.EndingDisplayRange - Math.Min(newFileName.Length, RenameWindowSettings.TextBoxWidth)));
                    CursorPositionHandler.UpdateCursorPosition(posWithinWindow: RenameWindowSettings.TextBoxStartingColumn + RenameWindowSettings.TextBoxWidth);
                }
                else
                {
                    CursorPositionHandler.UpdateDisplayRange(startingPos: 0);
                    CursorPositionHandler.UpdateDisplayRange(endingPos: CursorPositionHandler.StartingDisplayRange + Math.Min(newFileName.Length, RenameWindowSettings.TextBoxWidth));
                    CursorPositionHandler.UpdateCursorPosition(posWithinWindow: RenameWindowSettings.TextBoxStartingColumn + CursorPositionHandler.PositionWithinName);
                }
            }
            else
            {
                DecrementDisplayRange();
            }
        }

        /// <summary>
        /// Removes the word at which the cursor is currently positioned.
        /// </summary>
        /// <param name="key">The <see cref="ConsoleKeyInfo"/> that contains the instruction whether to delete until the previous character or word separator</param>
        private void RemoveWord(ConsoleKeyInfo key)
        {
            var initialPosWithinName = CursorPositionHandler.PositionWithinName;
            MoveLeft(key);
            var wordLength = initialPosWithinName - CursorPositionHandler.PositionWithinName;
            newFileName!.Remove(CursorPositionHandler.PositionWithinName, wordLength);
            DecrementDisplayRange();
        }

        #endregion

        #region CursorPositionWithinName alteration methods

        /// <summary>
        /// Increments cursor position within name by an order of 1, if needed.
        /// </summary>
        private void IncrementCursorPositionWithinName()
        {
            var newPos = Math.Clamp(
                CursorPositionHandler.PositionWithinName + 1,
                0,
                newFileName!.Length);

            CursorPositionHandler.UpdateCursorPosition(posWithinName: newPos);
        }

        /// <summary>
        /// Decrements the cursor position within name by the given value. Default value is 1.
        /// </summary>
        /// <param name="value">The value to be subtracted from the cursor position within name.</param>
        private void DecrementCursorPositionWithinName(int value = 1)
        {
            var newPos = Math.Clamp(
                CursorPositionHandler.PositionWithinName - value,
                0,
                newFileName!.Length);

            CursorPositionHandler.UpdateCursorPosition(posWithinName: newPos);
        }

        #endregion

        #region CursorPositionWithinWindow alteration methods

        /// <summary>
        /// Increments cursor position within window by an order of 1, if needed.
        /// </summary>
        private void IncrementCursorPositionWithinWindow()
        {
            var newPos = Math.Clamp(
                CursorPositionHandler.PositionWithinWindow + 1,
                RenameWindowSettings.TextBoxStartingColumn,
                Math.Min(RenameWindowSettings.TextBoxStartingColumn + newFileName!.Length, RenameWindowSettings.TextBoxStartingColumn + RenameWindowSettings.TextBoxWidth));

            CursorPositionHandler.UpdateCursorPosition(posWithinWindow: newPos);
        }

        /// <summary>
        /// Decrements the cursor position within window by the given value. Default value is 1.
        /// </summary>
        /// <param name="value">The value to be subtracted from the cursor position within window.</param>
        private void DecrementCursorPositionWithinWindow(int value = 1)
        {
            var newPos = Math.Clamp(
                CursorPositionHandler.PositionWithinWindow - value,
                RenameWindowSettings.TextBoxStartingColumn,
                Math.Min(RenameWindowSettings.TextBoxStartingColumn + newFileName!.Length, RenameWindowSettings.TextBoxStartingColumn + RenameWindowSettings.TextBoxWidth));

            CursorPositionHandler.UpdateCursorPosition(posWithinWindow: newPos);
        }

        #endregion

        #region DisplayRange alteration methods

        /// <summary>
        /// Increments display range by calculating a new ending position and updating starting position accordingly.
        /// </summary>
        private void IncrementDisplayRange()
        {
            var newEndingPos = newFileName!.Length < RenameWindowSettings.TextBoxWidth 
                ? newFileName!.Length
                : Math.Max(CursorPositionHandler.PositionWithinName, CursorPositionHandler.EndingDisplayRange);

            CursorPositionHandler.UpdateDisplayRange(endingPos: newEndingPos);
            CursorPositionHandler.UpdateDisplayRange(startingPos: CursorPositionHandler.EndingDisplayRange - Math.Min(newFileName!.Length, RenameWindowSettings.TextBoxWidth));
        }

        /// <summary>
        /// Decrements display range by updating starting and ending positions. New range depends on the current cursor positions.
        /// </summary>
        private void DecrementDisplayRange()
        {
            CursorPositionHandler.UpdateDisplayRange(startingPos: Math.Min(CursorPositionHandler.PositionWithinName, CursorPositionHandler.StartingDisplayRange));
            CursorPositionHandler.UpdateDisplayRange(endingPos: Math.Min(newFileName!.Length, CursorPositionHandler.StartingDisplayRange + Math.Min(newFileName!.Length, RenameWindowSettings.TextBoxWidth)));
        }

        #endregion

        #region Window size change handling methods

        /// <summary>
        /// Handles the case in which the window size has changed since the last printing. Moves the cursor position accordingly.
        /// </summary>
        private void HandleWindowSizeChange()
        {
            var newStart = Math.Min(CursorPositionHandler.PositionWithinName - 1, Math.Max(0, newFileName!.Length - RenameWindowSettings.TextBoxWidth));
            CursorPositionHandler.UpdateDisplayRange(startingPos: newStart);

            var newEnd = CursorPositionHandler.StartingDisplayRange + Math.Min(newFileName!.Length, RenameWindowSettings.TextBoxWidth);
            CursorPositionHandler.UpdateDisplayRange(endingPos: newEnd);

            var newPos = CursorPositionHandler.PositionWithinName - CursorPositionHandler.StartingDisplayRange + RenameWindowSettings.TextBoxWidth;
            CursorPositionHandler.UpdateCursorPosition(posWithinWindow: newPos);
        }

        #endregion
    }
}
