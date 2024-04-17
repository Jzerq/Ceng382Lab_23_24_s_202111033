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
        try
        {
            // Read existing logs from the file
            List<LogRecord> logs = ReadLogsFromFile();

            // Add the new log
            logs.Add(log);

            // Serialize logs to JSON format
            string jsonLogs = JsonSerializer.Serialize(logs);

            // Write the JSON data to the log file
            File.WriteAllText(_logFilePath, jsonLogs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging to file: {ex.Message}");
        }
    }

    private List<LogRecord> ReadLogsFromFile()
    {
        try
        {
            // Check if the log file exists
            if (File.Exists(_logFilePath))
            {
                // Read existing JSON data from the log file
                string jsonLogs = File.ReadAllText(_logFilePath);

                // Deserialize JSON to List<LogRecord>
                return JsonSerializer.Deserialize<List<LogRecord>>(jsonLogs) ?? new List<LogRecord>();
            }
            else
            {
                // Log file doesn't exist, return an empty list
                return new List<LogRecord>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading log file: {ex.Message}");
            return new List<LogRecord>();
        }
    }
}
