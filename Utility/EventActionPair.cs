namespace FileManager.Utility
{
    public class EventActionPair(ManualResetEventSlim resetEventSlim, Action printAction)
    {
        public ManualResetEventSlim ResetEventSlim { get; } = resetEventSlim;
        public Action Action { get; } = printAction;
    }
}
