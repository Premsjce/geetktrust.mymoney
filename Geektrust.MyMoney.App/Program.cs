using System.IO;

namespace Geektrust.MyMoney.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var filePath = "inputs\\First.txt";
            if (args.Length != 0)
                filePath = args[0];

            var inputCommands = File.ReadAllLines(filePath);

            var appCatalog = new AppCatalog();
            var task = appCatalog.Driver(inputCommands);
            while (!task.IsCompleted)
            {

            }
        }
    }
}
