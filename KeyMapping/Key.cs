namespace FileManager.KeyMapping
{
    public class Key(ConsoleKey primary, ConsoleKey secondary, CommandType type, string? description)
    {
        public ConsoleKey Primary { get; private set; } = primary;
        public ConsoleKey Secondary { get; private set; } = secondary;
        public CommandType Type { get; private set; } = type;
        public string? Description { get; private set; } = description;

        public void SetPrimary(ConsoleKey primary)
            => Primary = primary;

        public void SetSecondary(ConsoleKey secondary)
            => Secondary = secondary;
    }
}
