namespace MqttDemo.Helpers
{
    public class FileReadWriteHelper
    {
        public async static Task WriteToReportAsync(string content)
        {
            var fileName = "Reports.txt";
            var directory = GoUpDirectoryLevelHelper(Environment.CurrentDirectory, 2);
            var path = Path.Combine(directory, fileName);

            // Append to file
            using (StreamWriter outputFile = new StreamWriter(path, true))
                await outputFile.WriteAsync($"{DateTime.Now}\t|\t{content}{Environment.NewLine}");
        }

        public static string GoUpDirectoryLevelHelper(string path, int depth) => (depth > 0)
            ? GoUpDirectoryLevelHelper(path.Substring(0, path.LastIndexOf('\\')), depth - 1)
            : path.Substring(0, path.LastIndexOf('\\'));
    }
}
