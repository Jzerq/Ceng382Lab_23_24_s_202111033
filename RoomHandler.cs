using System;
using System.IO;
using System.Text.Json;
using Classes;

public class RoomHandler 
{
    private readonly string _roomFilePath;

    public RoomHandler(string roomFilePath)
    {
        _roomFilePath = roomFilePath;
    }

    public Room[] GetRooms()
    {
        try
        {
            // Read room data from JSON file
            string jsonString = File.ReadAllText(_roomFilePath);

            // Deserialize JSON to RoomData object
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var roomData = JsonSerializer.Deserialize<RoomData>(jsonString, options);

            return roomData?.Rooms ?? Array.Empty<Room>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading room data: {ex.Message}");
            return Array.Empty<Room>();
        }
    }

    public void SaveRooms(Room[] rooms)
    {
        try
        {
            // Create RoomData object
            var roomData = new RoomData { Rooms = rooms };

            // Serialize RoomData to JSON format
            string jsonString = JsonSerializer.Serialize(roomData);

            // Write JSON data to the room data file
            File.WriteAllText(_roomFilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving room data: {ex.Message}");
        }
    }
}
