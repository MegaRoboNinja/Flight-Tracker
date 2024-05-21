using System.Text;

namespace Aviation_Project;

public static class Logger
{
    public static string LogPath { get; private set; }
    private static readonly object _lockObj = new object();

    static Logger()
    {
        string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
        string fileName = $"log_{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}.txt";
        LogPath = Path.Combine(directoryPath, fileName);

        if (!File.Exists(LogPath))
        {
            // Create a new file
            using (FileStream fs = File.Create(LogPath))
            {
                // Add some text to file
                Byte[] title = new UTF8Encoding(true).GetBytes("New log file created at " + DateTime.Now +"\n");
                fs.Write(title, 0, title.Length);
            }
        }
        else
        {
            // Write a message indicating a new session
            using (StreamWriter streamWriter = File.AppendText(LogPath))
            {
                streamWriter.WriteLine("New session started at "+ DateTime.Now +"\n");
            }
        }
    }

    public static void Log(string message)
    {
        lock (_lockObj)
        {
            using (StreamWriter streamWriter = File.AppendText(LogPath))
            {
                streamWriter.WriteLine(message + "\n");
            }
        }
    }
}