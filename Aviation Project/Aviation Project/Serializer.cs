using System.Text.Json;

namespace Aviation_Project;

public static class Serializer
{
    public static void Snapshot(List<Object> objects, string filePath = null)
    {
        // TODO: add a mutex here or something
        string snapName = $"snapshot_{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.json";
        foreach (var obj in objects)
        {
            Serializer.Serialize(obj, snapName, filePath);
        }
    }
    public static void Serialize(object obj, string fileName = "serialized.json", string filePath = null)
    {
        if (filePath == null)
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        
        string serialized = JsonSerializer.Serialize(obj);
        WriteToFile(serialized, fileName, filePath);
    }

    private static void WriteToFile(string serialized, string fileName, string filePath)
    {
        if (!File.Exists(filePath))
        {
            using(File.Create(filePath)) {}
        }
        
        using (StreamWriter streamWriter = File.AppendText(filePath))
        {
            streamWriter.WriteLine(serialized);
        }
    }
}