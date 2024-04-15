using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class FileLogger : ILogger
{
    private readonly string _logFilePath;

    public FileLogger(string logFilePath)
    {
        _logFilePath = logFilePath;
    }

    public void Log(LogRecord log)
    {
        var logs = new List<LogRecord>();

        try
        {
            // Read existing logs from file, if any
            string jsonString = File.ReadAllText(_logFilePath);
            logs = JsonSerializer.Deserialize<List<LogRecord>>(jsonString) ?? new List<LogRecord>();
        }
        catch (FileNotFoundException)
        {
            // Log file doesn't exist, create an empty list
        }
        catch (JsonException)
        {
            // Error parsing log file, create an empty list
        }

        // Add new log entry
        logs.Add(log);

        try
        {
            // Write updated logs back to file
            File.WriteAllText(_logFilePath, JsonSerializer.Serialize(logs));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing log to file: {ex.Message}");
        }
    }
}
