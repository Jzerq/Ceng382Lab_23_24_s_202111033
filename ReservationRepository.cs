// ReservationRepository.cs
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ReservationRepository : IReservationRepository
{
    private readonly string _reservationFilePath;

    public ReservationRepository(string reservationFilePath)
    {
        _reservationFilePath = reservationFilePath;
    }

    public void AddReservation(Reservation reservation)
    {
        var reservations = GetAllReservations();
        reservations.Add(reservation);
        File.WriteAllText(_reservationFilePath, JsonSerializer.Serialize(reservations));
    }

    public void DeleteReservation(Reservation reservation)
    {
        var reservations = GetAllReservations();
        reservations.Remove(reservation);
        File.WriteAllText(_reservationFilePath, JsonSerializer.Serialize(reservations));
    }

    public List<Reservation> GetAllReservations()
    {
        try
        {
            string jsonString = File.ReadAllText(_reservationFilePath);
            return JsonSerializer.Deserialize<List<Reservation>>(jsonString) ?? new List<Reservation>();
        }
        catch (FileNotFoundException)
        {
            // Reservation file doesn't exist, return an empty list
            return new List<Reservation>();
        }
        catch (JsonException)
        {
            // Error parsing reservation file, return an empty list
            return new List<Reservation>();
        }
    }
}
