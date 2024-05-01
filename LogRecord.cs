using System;

public record LogRecord
{
    public DateTime Timestamp { get; init; }
    public string ReserverName { get; init; }
    public string Status { get; init; }
    public string RoomID { get; init; }

    public LogRecord(DateTime timestamp, string reserverName, string status, string roomID)
    {
        Timestamp = timestamp;
        ReserverName = reserverName;
        Status = status;
        RoomID = roomID;
    }
}