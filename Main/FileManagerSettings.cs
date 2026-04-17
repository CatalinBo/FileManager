using System.Runtime.InteropServices;

namespace FileManager
{
    /// <summary>
    /// Implementation of <see cref="FileManagerSettings"/> class.
    /// </summary>
    public static class FileManagerSettings
    {
        private const int WindowsMaxPathLength = 260;
        private const int WindowsExtendedMaxPathLength = 32767;
        private const int UnixMaxPathLength = 4096;

        public static int MaximumPathLength => GetMaximumPathLength();

        public const int ThrottleDelay = 30;
        public const int DebounceDelay = ThrottleDelay;
        public const int MaxTasksAllowed = 3;
        
        public const string EscapeSeq = "\u001b[2J\u001b[3J";
        public const string NavigationErrorMessage = "Error during file navigation";

        public const char TopLeftCorner =       '\u256D';
        public const char TopRightCorner =      '\u256E';
        public const char BottomLeftCorner =    '\u2570';
        public const char BottomRightCorner =   '\u256F';
        public const char HorizontalLine =      '\u2500';
        public const char VerticalLine =        '\u2502';
        public const char TeeRight =            '\u251C';
        public const char TeeLeft =             '\u2524';
        public const char TeeUp =               '\u252C';
        public const char TeeDown =             '\u2534';

        public const long Kilobyte = 1024;
        public const long Megabyte = Kilobyte * 1024;
        public const long Gigabyte = Megabyte * 1024;

        public const string ByteShorthand = "B";
        public const string KilobyteShorthand = "KB";
        public const string MegabyteShorthand = "MB";
        public const string GigabyteShorthand = "GB";

        public static readonly ConsoleKeyInfo DefaultKey = new ConsoleKeyInfo((char)ConsoleKey.None, ConsoleKey.None, false, false, false);
        public static readonly ConsoleKeyInfo RenameKey = new ConsoleKeyInfo((char)ConsoleKey.F2, ConsoleKey.F2, false, false, false);

        public static readonly HashSet<char> WordSeparators =
            [
                ' ',    // Space
                '\t',   // Tab
                '\n',   // Newline
                '\r',   // Carriage return
                '.',    // Period
                ',',    // Comma
                '!',    // Exclamation mark
                '?',    // Question mark
                ':',    // Colon
                ';',    // Semicolon
                '-',    // Hyphen
                '–',    // En dash
                '—',    // Em dash
                '\'',   // Apostrophe
                '"',    // Double quotation mark
                '(',    // Left parenthesis
                ')',    // Right parenthesis
                '[',    // Left square bracket
                ']',    // Right square bracket
                '{',    // Left curly brace
                '}',    // Right curly brace
                '/',    // Forward slash
                '\\',   // Backslash
                '|',    // Pipe
                '+',    // Plus
                '*',    // Asterisk
                '=',    // Equals
                '<',    // Less than
                '>',    // Greater than
                '&',    // Ampersand
                '%',    // Percent
                '@',    // At symbol
                '#',    // Hash/Pound
                '$'     // Dollar sign
            ];

        /// <summary>
        /// Gets the maximum length of paths, depending on the Operating System installed on the machine.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the maximum characters that a path can contain.</returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        private static int GetMaximumPathLength()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetWindowsMaxPathLength();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return UnixMaxPathLength;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }

            static int GetWindowsMaxPathLength()
            {
                return AreLongPathsEnabled()
                    ? WindowsExtendedMaxPathLength
                    : WindowsMaxPathLength;

                static bool AreLongPathsEnabled()
                {
                    string longPathsKey = @"SYSTEM\CurrentControlSet\Control\FileSystem";
                    string longPathsValue = "LongPathsEnabled";

                    using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(longPathsKey);

                    if (key != null)
                    {
                        var val = key.GetValue(longPathsValue);

                        if (val != null)
                        {
                            return (int)val == 1;
                        }
                    }

                    return false;
                }
            }
        }
    }
}
