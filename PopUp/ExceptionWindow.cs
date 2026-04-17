namespace FileManager.PopUp
{
    /// <summary>
    /// Inherits properties and methods from <see cref="PopUpDialog"/> and deals only with exceptions.
    /// </summary>
    /// <param name="exceptionMessage">A <see cref="string"/> representing the encountered exception's message.</param>
    public class ExceptionWindow(string exceptionMessage) : PopUpDialog(exceptionMessage) { }
}
