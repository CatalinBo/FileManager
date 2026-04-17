namespace FileManager.Main
{
    public class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            var fileManager = new FileManager();

            fileManager.Run();
        }
    }
}
