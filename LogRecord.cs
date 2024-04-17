// LogRecord.cs
public record LogRecord
{
    public DateTime Timestamp { get; init; }
    public string ReserverName { get; init; }
    public string RoomName { get; init; }
}