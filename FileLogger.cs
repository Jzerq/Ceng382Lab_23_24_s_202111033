using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class FileLogger : ILogger
{
    private string _logFilePath;

    public FileLogger(string logFilePath)
    {
        _logFilePath = logFilePath;
    }
     public List<LogRecord> GetLogs()
    {
        try
        {
            var jsonString = File.ReadAllText(_logFilePath);
            var logs = JsonSerializer.Deserialize<List<LogRecord>>(jsonString);
            return logs;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading log file: {ex.Message}");
            return new List<LogRecord>();
        }
    }


    public void Log(LogRecord log)
    {
        List<LogRecord> logs;

        // Read existing log records from file, if any
        if (File.Exists(_logFilePath))
        {
            string existingJson = File.ReadAllText(_logFilePath);
            logs = JsonSerializer.Deserialize<List<LogRecord>>(existingJson);
        }
        else
        {
            logs = new List<LogRecord>();
        }

        // Add new log record
        logs.Add(log);

        // Write updated log records to file
        string updatedJson = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_logFilePath, updatedJson);
    }
}
