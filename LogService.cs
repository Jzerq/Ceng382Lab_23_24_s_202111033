using System;
using System.Collections.Generic;
using System.Linq;

public class LogService
{

    private readonly List<LogRecord> _logRecords;

    public LogService(List<LogRecord> logRecords)
    {
        _logRecords = logRecords;
    }

    public List<LogRecord> DisplayLogsByName(string name)
    {
        return _logRecords.Where(record => record.ReserverName == name).ToList();
    }

    public List<LogRecord> DisplayLogs(DateTime start, DateTime end)
    {
        
        return _logRecords.Where(record => record.Timestamp >= start && record.Timestamp <= end).ToList();
    }
}
