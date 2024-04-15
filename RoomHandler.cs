// RoomHandler.cs
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class RoomHandler
{
    private readonly string _roomFilePath;

    public RoomHandler(string roomFilePath)
    {
        _roomFilePath = roomFilePath;
    }

    public List<Room> GetRooms()
    {
        try
        {
            string jsonString = File.ReadAllText(_roomFilePath);
            return JsonSerializer.Deserialize<List<Room>>(jsonString) ?? new List<Room>();
        }
        catch (FileNotFoundException)
        {
            // Room file doesn't exist, return an empty list
            return new List<Room>();
        }
        catch (JsonException)
        {
            // Error parsing room file, return an empty list
            return new List<Room>();
        }
    }

    public void SaveRooms(List<Room> rooms)
    {
        File.WriteAllText(_roomFilePath, JsonSerializer.Serialize(rooms));
    }
}
