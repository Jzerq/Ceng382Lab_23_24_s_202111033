using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

public class RoomData
{
    [JsonPropertyName("Room")]
    public Room[] Rooms { get; set; }
}

public class Room
{
    [JsonPropertyName("roomId")]
    public string RoomId { get; set; }

    [JsonPropertyName("roomName")]
    public string RoomName { get; set; }

    [JsonPropertyName("capacity")]
    [JsonConverter(typeof(StringToIntConverter))] // Custom converter to convert string to int
    public int Capacity { get; set; }
}

public class Reservation
{
    public string DayOfWeek { get; set; }
    public string TimeSlot { get; set; }
    public string ReserverName { get; set; }
    public string Dateday { get; set; }
    public string RoomId { get; set; }
}

public interface IReservationManager
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(string roomId);
    void CheckReservation(string roomId);
    void DisplayReservations();
}

public class ReservationHandler : IReservationManager
{
    private Room[] rooms;
    private List<Reservation> reservations;

    public ReservationHandler(Room[] rooms)
    {
        this.rooms = rooms;
        reservations = new List<Reservation>();
    }

    public void AddReservation(Reservation reservation)
    {
        if (IsRoomAvailable(reservation.RoomId))
        {
            reservations.Add(reservation);
            Console.WriteLine("Reservation added successfully.");
        }
        else
        {
            Console.WriteLine("Room capacity is full. Reservation not added.");
        }
    }

    public void DeleteReservation(string roomId)
    {
        var reservationToRemove = reservations.FirstOrDefault(r => r.RoomId == roomId);
        if (reservationToRemove != null)
        {
            reservations.Remove(reservationToRemove);
            Console.WriteLine("Reservation deleted successfully.");
        }
        else
        {
            Console.WriteLine("No reservation found for the specified room.");
        }
    }

    public void CheckReservation(string roomId)
    {
        var roomReservations = reservations.Where(r => r.RoomId == roomId).ToList();
        if (roomReservations.Any())
        {
            Console.WriteLine($"Reservations for Room {roomId}:");
            foreach (var reservation in roomReservations)
            {
                Console.WriteLine($"Day: {reservation.DayOfWeek}, Time: {reservation.TimeSlot}, Reserved by: {reservation.ReserverName}, Reserved Date: {reservation.Dateday}");
            }
        }
        else
        {
            Console.WriteLine($"No reservations found for Room {roomId}.");
        }
    }

    public void DisplayReservations()
    {
        Console.WriteLine("All Reservations:");
        foreach (var reservation in reservations)
        {
            var room = GetRoomById(reservation.RoomId);
            Console.WriteLine($"Room ID: {room.RoomId}, Room Name: {room.RoomName}, Day: {reservation.DayOfWeek},Reservation Date {reservation.Dateday} ,Time: {reservation.TimeSlot}, Reserved by: {reservation.ReserverName}");
        }
    }

    private bool IsRoomAvailable(string roomId)
    {
        var room = GetRoomById(roomId);
        if (room != null)
        {
            int reservedCount = reservations.Count(r => r.RoomId == roomId);
            return reservedCount < room.Capacity;
        }
        return false;
    }

    private Room GetRoomById(string roomId)
    {
        return rooms.FirstOrDefault(r => r.RoomId == roomId);
    }
}

// Custom converter to convert string to int
public class StringToIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (int.TryParse(reader.GetString(), out int value))
            {
                return value;
            }
        }
        throw new JsonException("Unable to convert value to int.");
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

class Program
{
    static void Main(string[] args)
    {
        string jsonFilePath = "Data.json";

        try
        {
            string jsonString = File.ReadAllText(jsonFilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var roomData = JsonSerializer.Deserialize<RoomData>(jsonString, options);

            if (roomData?.Rooms != null)
            {
                IReservationManager reservationManager = new ReservationHandler(roomData.Rooms);

                // Display options
                while (true)
                {
                    Console.WriteLine("Select an option:");
                    Console.WriteLine("1. Add Reservation");
                    Console.WriteLine("2. Delete Reservation");
                    Console.WriteLine("3. Check Reservations for a Room");
                    Console.WriteLine("4. Display All Reservations");
                    Console.WriteLine("5. Exit");

                    int choice;
                    if (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    switch (choice)
                    {
                        case 1:
                            AddReservation(reservationManager);
                            break;
                        case 2:
                            DeleteReservation(reservationManager);
                            break;
                        case 3:
                            CheckReservation(reservationManager);
                            break;
                        case 4:
                            reservationManager.DisplayReservations();
                            break;
                        case 5:
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please select a valid option.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("No room data found in JSON.");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: The data file '{jsonFilePath}' was not found.");
        }
        catch (JsonException)
        {
            Console.WriteLine($"Error: Unable to parse JSON data from '{jsonFilePath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void AddReservation(IReservationManager reservationManager)
    {
        Console.WriteLine("Enter details for the reservation:");
        Console.Write("Day of week: ");
        string dayOfWeek = Console.ReadLine();
        Console.Write("Reservation date(DD/MM/YYY): ");
        string dateday = Console.ReadLine();
        Console.Write("Time(Time format HH:MM): ");
        string timeSlot = Console.ReadLine();
        Console.Write("Reserver name: ");
        string reserverName = Console.ReadLine();
        Console.Write("Room ID: ");
        string roomId = Console.ReadLine();

        Reservation reservation = new Reservation
        {
            DayOfWeek = dayOfWeek,
            TimeSlot = timeSlot,
            ReserverName = reserverName,
            RoomId = roomId,
            Dateday=dateday
        };

        reservationManager.AddReservation(reservation);
    }

    static void DeleteReservation(IReservationManager reservationManager)
    {
        Console.Write("Enter Room ID to delete reservation: ");
        string roomId = Console.ReadLine();

        reservationManager.DeleteReservation(roomId);
    }

    static void CheckReservation(IReservationManager reservationManager)
    {
        Console.Write("Enter Room ID to check reservations: ");
        string roomId = Console.ReadLine();

        reservationManager.CheckReservation(roomId);
    }
}
