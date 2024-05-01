using System;

public record Reservation{
    public Room? room { get; init; }
    public DateTime time { get; init;}
    public DateTime date { get; init;}
    public string? reserverName { get; init; }

}
