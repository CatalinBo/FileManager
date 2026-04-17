namespace FileManager.KeyMapping
{
    /// <summary>
    /// Provides methods to check if the given key falls into one of the provided categories.
    /// </summary>
    public static class KeyTypeCheck
    {
        public static bool IsDeleteKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.DeleteKeys.Contains(keyInfo.Key);

        public static bool IsCopyKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.CopyKeys.Contains(keyInfo.Key);

        public static bool IsCutKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.CutKeys.Contains(keyInfo.Key);

        public static bool IsRenameKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.RenameKeys.Contains(keyInfo.Key);

        public static bool IsPasteKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.PasteKeys.Contains(keyInfo.Key);

        public static bool IsMultipleSelectionKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.SelectKeys.Contains(keyInfo.Key);

        public static bool IsMoveDownKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.MoveDownKeys.Contains(keyInfo.Key);

        public static bool IsMoveUpKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.MoveUpKeys.Contains(keyInfo.Key);

        public static bool IsMoveRightKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.MoveRightKeys.Contains(keyInfo.Key);

        public static bool IsAccessKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.AccessKeys.Contains(keyInfo.Key);

        public static bool IsMoveLeftKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.MoveLeftKeys.Contains(keyInfo.Key);

        public static bool IsToggleHiddenKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.ToggleHiddenKeys.Contains(keyInfo.Key);

        public static bool IsShowHelpKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.ShowLegendKeys.Contains(keyInfo.Key);

        public static bool IsShowErrorLogKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.ShowErrorLogKeys.Contains(keyInfo.Key);

        public static bool IsToggleBackgroundTaskWindowKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.ToggleBackgroundTaskWindowKeys.Contains(keyInfo.Key);

        public static bool IsEditShortcutsKey(ConsoleKeyInfo keyInfo)
            => CommandKeys.EditShortcutsKeys.Contains(keyInfo.Key);
    }
}
