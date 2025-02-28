using System.Text.Json;
namespace JsonFileReader
{
    class Program
    {
        
        private static readonly string inputFolder = Path.Combine(Directory.GetCurrentDirectory(), "input");
        private static readonly string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "output");

        static void Main(string[] args)
        {
            
            if (!Directory.Exists(inputFolder))
            {
                Directory.CreateDirectory(inputFolder);
            }
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            FileSystemWatcher fileWatcher = new FileSystemWatcher
            {
                Path = inputFolder,
                Filter = "*.json",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            fileWatcher.Created += OnNewFileCreated;
            fileWatcher.EnableRaisingEvents = true;

            Console.WriteLine($"Searching for .json files in: {inputFolder}");
            Console.WriteLine("Press enter to exit.");
            
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Q)
                    break;
            }
        }

        private static void OnNewFileCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"\nNew file detected: {e.FullPath}");

            try
            {
                string jsonString = File.ReadAllText(e.FullPath);
                var jsonObject = JsonSerializer.Deserialize<JsonData>(jsonString);
                Console.WriteLine("\nFile Content:");
                Console.WriteLine($"FirstName: {jsonObject.FirstName}");
                Console.WriteLine($"LastName: {jsonObject.LastName}");
                Console.WriteLine($"Age: {jsonObject.Age}");
                string outputFilePath = Path.Combine(outputFolder, Path.GetFileName(e.FullPath));
                File.Move(e.FullPath, outputFilePath);
                Console.WriteLine($"File moved to: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading or parsing the file: {ex.Message}");
            }
        }
    }
    public class JsonData
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}