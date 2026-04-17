namespace FileManager.InfoBar
{
    /// <summary>
    /// Provides properties and methods to display information (file size, last modified info) about the selected <see cref="IFileInfo"/>.
    /// </summary>
    public class StatusBar
    {
        private readonly IFileInfo? fileInfo;

        public StatusBar(IFileInfo? fileInfo)
        {
            this.fileInfo = fileInfo;
            FileSize = GetFileSizeInfo();
            LastModified = fileInfo?.LastWriteTime;
        }

        #region File information properties

        /// <summary>
        /// A <see cref="Tuple{T1, T2}"/> containing the size and unit of measurement for the selected <see cref="FileInfo"/>.
        /// </summary>
        private (double SizeValue, string Unit) FileSize { get; set; }

        /// <summary>
        /// <see cref="DateTime"/> containing information about the date and time at which the last file edit occurred.
        /// </summary>
        private DateTime? LastModified { get; set; }

        #endregion

        /// <summary>
        /// Print the entire <see cref="StatusBar"/> layout, by calling helper methods.
        /// </summary>
        public void PrintDetails()
        {
            if (Console.WindowHeight < NavigationWindowSettings.WindowHeight)
            {
                return;
            }

            ConsoleManager.InitializeCursorPosition(StatusBarSettings.StartingColumn, StatusBarSettings.StartingRow);
            ConsoleManager.InitializeConsoleColors(StatusBarSettings.ForegroundColor, StatusBarSettings.BackgroundColor);

            var sizeInfo = FormatSizeInfo();
            var modifiedInfo = FormatModifiedInfo();
            var onSeparateRows = (sizeInfo.Length + modifiedInfo.Length) >= StatusBarSettings.WindowWidth;

            PrintTopBorder();
            PrintSizeInfo();
            PrintLastModifiedInfo();

            Console.ResetColor();

            void PrintTopBorder()
            {
                Console.Write(StatusBarSettings.TopBorder);
                ConsoleManager.JumpToNewLine(StatusBarSettings.StartingColumn);
            }

            void PrintSizeInfo()
            {
                var sizeInfoMessage = onSeparateRows
                    ? sizeInfo.PadRight(StatusBarSettings.WindowWidth, ' ')
                    : sizeInfo.PadRight(StatusBarSettings.WindowWidth - modifiedInfo.Length, ' ');

                Console.Write(sizeInfoMessage);
            }

            void PrintLastModifiedInfo()
            {
                if (onSeparateRows)
                {
                    ConsoleManager.JumpToNewLine(StatusBarSettings.StartingColumn);
                }

                var modifiedInfoMessage = onSeparateRows
                    ? modifiedInfo.PadRight(StatusBarSettings.WindowWidth, ' ')
                    : modifiedInfo;

                Console.Write(modifiedInfoMessage);
            }
        }

        #region Field strings formatting

        /// <summary>
        /// Gets the size info of the current <see cref="FileInfo"/>.
        /// </summary>
        /// <returns>A <see cref="Tuple{T1, T2}"/> containing file size and measurement unit.</returns>
        private (double SizeValue, string Unit) GetFileSizeInfo()
        {
            long sizeInBytes = 0;

            if (fileInfo is not null)
            {
                sizeInBytes = Directory.Exists(fileInfo.FullName)
                    ? 0
                    : fileInfo.Length;
            }

            return sizeInBytes switch
            {
                < FileManagerSettings.Kilobyte => (sizeInBytes, FileManagerSettings.ByteShorthand),
                < FileManagerSettings.Megabyte => ((double)sizeInBytes / FileManagerSettings.Kilobyte, FileManagerSettings.KilobyteShorthand),
                < FileManagerSettings.Gigabyte => ((double)sizeInBytes / FileManagerSettings.Megabyte, FileManagerSettings.MegabyteShorthand),
                _ => ((double)sizeInBytes / FileManagerSettings.Gigabyte, FileManagerSettings.GigabyteShorthand),
            };
        }

        private string FormatSizeInfo()
        {
            return FileSize.SizeValue == 0
                ? string.Format("{0}{1, 10}", StatusBarSettings.SizeLabel, "N/A")
                : string.Format("{0, -6}{1, 7}{2, 3}", StatusBarSettings.SizeLabel, $"{FileSize.SizeValue:F2}", FileSize.Unit);
        }

        private string FormatModifiedInfo()
            => $"{StatusBarSettings.LastModifiedLabel} {LastModified}";

        #endregion

        #region Public methods used only for unit testing

        /// <summary>
        /// Gets the size info of the current <see cref="FileInfo"/>.
        /// </summary>
        /// <returns>A <see cref="Tuple{T1, T2}"/> containing file size and measurement unit.</returns>
        public (double SizeValue, string Unit) GetFileSizeInfoTest()
            => GetFileSizeInfo();

        public DateTime? GetLastModifiedInfo()
            => LastModified;

        #endregion
    }
}
