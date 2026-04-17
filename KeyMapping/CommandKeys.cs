namespace FileManager.KeyMapping
{
    /// <summary>
    /// Provides methods to retrieve available keys to use in the application.
    /// </summary>
    public static class CommandKeys
    {
        private const int PrimaryKeyIndex = 0;
        private const int SecondaryKeyIndex = 1;

        #region Action descriptions

        private static readonly string QuitDescription                          =   "Quit/Exit";
        private static readonly string RenameDescription                        =   "Rename";
        private static readonly string DeleteDescription                        =   "Delete selected";
        private static readonly string CutDescription                           =   "Cut";
        private static readonly string CopyDescription                          =   "Copy";
        private static readonly string PasteDescription                         =   "Paste";
        private static readonly string MoveUpDescription                        =   "Move Up";
        private static readonly string MoveDownDescription                      =   "Move Down";
        private static readonly string MoveLeftDescription                      =   "Back";
        private static readonly string MoveRightDescription                     =   "Advance";
        private static readonly string AccessDescription                        =   "Access/Open";
        private static readonly string ToggleHiddenDescription                  =   "Toggle hidden";
        private static readonly string SelectDescription                        =   "Select/Deselect item";
        private static readonly string ShowHelpDescription                      =   "Help";
        private static readonly string ShowErrorLogDescription                  =   "Error Log";
        private static readonly string ToggleBackgroundTasksOverviewDescription =   "Expand background tasks";
        private static readonly string EditShortcutsDescription                 =   "Edit app shortcuts";

        #endregion

        #region Keys

        public static List<ConsoleKey> QuitKeys
        {
            get => [ConsoleKey.Escape, ConsoleKey.Q];
            set => QuitKeys = value;
        }

        public static List<ConsoleKey> RenameKeys
        {
            get => [ConsoleKey.F2, FileManagerSettings.DefaultKey.Key];
            set => RenameKeys = value;
        }

        public static List<ConsoleKey> DeleteKeys
        {
            get => [ConsoleKey.Delete, FileManagerSettings.DefaultKey.Key];
            set => DeleteKeys = value;
        }

        public static List<ConsoleKey> CutKeys
        {
            get => [ConsoleKey.X, FileManagerSettings.DefaultKey.Key];
            set => CutKeys = value;
        }

        public static List<ConsoleKey> CopyKeys
        {
            get => [ConsoleKey.C, ConsoleKey.Y];
            set => CopyKeys = value;
        }

        public static List<ConsoleKey> PasteKeys
        {
            get => [ConsoleKey.V, ConsoleKey.P];
            set => PasteKeys = value;
        }

        public static List<ConsoleKey> MoveUpKeys
        {
            get => [ConsoleKey.UpArrow, ConsoleKey.K];
            set => MoveUpKeys = value;
        }

        public static List<ConsoleKey> MoveDownKeys
        {
            get => [ConsoleKey.DownArrow, ConsoleKey.J];
            set => MoveDownKeys = value;
        }

        public static List<ConsoleKey> MoveLeftKeys
        {
            get => [ConsoleKey.LeftArrow, ConsoleKey.H];
            set => MoveLeftKeys = value;
        }

        public static List<ConsoleKey> MoveRightKeys
        {
            get => [ConsoleKey.RightArrow, ConsoleKey.L];
            set => MoveRightKeys = value;
        }

        public static List<ConsoleKey> AccessKeys
        {
            get => [ConsoleKey.Enter, FileManagerSettings.DefaultKey.Key];
            set => AccessKeys = value;
        }

        public static List<ConsoleKey> ToggleHiddenKeys
        {
            get => [ConsoleKey.F3, FileManagerSettings.DefaultKey.Key];
            set => ToggleHiddenKeys = value;
        }

        public static List<ConsoleKey> SelectKeys
        {
            get => [ConsoleKey.Spacebar, FileManagerSettings.DefaultKey.Key];
            set => SelectKeys = value;
        }

        public static List<ConsoleKey> ShowLegendKeys
        {
            get => [ConsoleKey.F1, FileManagerSettings.DefaultKey.Key];
            set => ShowLegendKeys = value;
        }

        public static List<ConsoleKey> ShowErrorLogKeys
        {
            get => [ConsoleKey.F4, FileManagerSettings.DefaultKey.Key];
            set => ShowErrorLogKeys = value;
        }

        public static List<ConsoleKey> ToggleBackgroundTaskWindowKeys
        {
            get => [ConsoleKey.Z, FileManagerSettings.DefaultKey.Key];
            set => ToggleBackgroundTaskWindowKeys = value;
        }

        public static List<ConsoleKey> EditShortcutsKeys
        {
            get => [ConsoleKey.F6, FileManagerSettings.DefaultKey.Key];
            set => EditShortcutsKeys = value;
        }

        #endregion

        #region Key mappings edit methods

        /// <summary>
        /// A <see cref="Dictionary{TKey, TValue}"/> containing all the available keys, grouped by <see cref="CommandType"/>.
        /// </summary>
        private static readonly Dictionary<CommandType, Dictionary<List<ConsoleKey>, string>> KeyMappings = new()
        {
            [CommandType.EndExecution] = new Dictionary<List<ConsoleKey>, string>
            {
                { QuitKeys, QuitDescription }
            },
            [CommandType.FileManipulation] = new Dictionary<List<ConsoleKey>, string>
            {
                { RenameKeys, RenameDescription },
                { DeleteKeys, DeleteDescription },
                { CutKeys, CutDescription },
                { CopyKeys, CopyDescription },
                { PasteKeys, PasteDescription }
            },
            [CommandType.Navigation] = new Dictionary<List<ConsoleKey>, string>
            {
                { MoveUpKeys, MoveUpDescription },
                { MoveDownKeys, MoveDownDescription },
                { MoveLeftKeys, MoveLeftDescription },
                { MoveRightKeys, MoveRightDescription },
                { AccessKeys, AccessDescription },
                { ToggleHiddenKeys, ToggleHiddenDescription }
            },
            [CommandType.ShowInfo] = new Dictionary<List<ConsoleKey>, string>
            {
                { ShowLegendKeys, ShowHelpDescription },
                { ShowErrorLogKeys, ShowErrorLogDescription },
                { ToggleBackgroundTaskWindowKeys, ToggleBackgroundTasksOverviewDescription }
            },
            [CommandType.AppFeatures] = new Dictionary<List<ConsoleKey>, string>
            {
                { SelectKeys, SelectDescription },
                { EditShortcutsKeys, EditShortcutsDescription }
            }
        };

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey, TValue}"/> containing the keys name and a short explanation for what they do.
        /// </summary>
        /// <remarks>This <see cref="Dictionary{TKey, TValue}"/> is used in <see cref="LegendWindow"/>.</remarks>
        public static Dictionary<string, string> GetKeyMappingsInHelpWindowFormat()
        {
            return KeyMappings.Values
                .SelectMany(innerDict => innerDict)
                .ToDictionary(
                    kvp => ListToString(kvp.Key),
                    kvp => kvp.Value
                    );
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey, TValue}"/> containing the type of action and the keys available to do that action.
        /// </summary>
        /// <remarks>This <see cref="Dictionary{TKey, TValue}"/> is used in <see cref="EditShortcutsWindow"/>.</remarks>
        public static Dictionary<string, List<ConsoleKey>> GetKeyMappingsInEditShortcutsWindowFormat()
        {
            return KeyMappings.Values
                .SelectMany(innerDict => innerDict)
                .ToDictionary(
                    kvp => kvp.Value,
                    kvp => kvp.Key
                    );
        }

        /// <summary>
        /// Transforms the given <see cref="List{T}"/> to a string of key names.
        /// </summary>
        /// <param name="keyList">A <see cref="List{T}"/> of <see cref="ConsoleKey"/>.</param>
        /// <returns>A <see cref="string"/> containing key names.</returns>
        private static string ListToString(List<ConsoleKey> keyList)
        {
            return keyList[SecondaryKeyIndex].Equals(FileManagerSettings.DefaultKey.Key)
                ? keyList[PrimaryKeyIndex].ToString()
                : string.Join(", ", keyList.Select(k => k.ToString()));
        }

        /// <summary>
        /// Gets a <see cref="Dictionary{TKey, TValue}"/> of keys for the given <see cref="CommandType"/>.
        /// </summary>
        /// <param name="commandType"><see cref="CommandType"/> for which to retrieve the keys.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> containing keys and a short description for the given <see cref="CommandType"/>.</returns>
        public static IDictionary<List<ConsoleKey>, string> GetKeys(CommandType commandType)
        {
            return KeyMappings.TryGetValue(commandType, out var keyMappings)
                ? keyMappings
                : [];
        }

        #endregion
    }
}
