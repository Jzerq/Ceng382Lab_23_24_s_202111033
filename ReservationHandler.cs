using System;
using System.Collections.Generic;
using System.Linq;
using Classes;

public class ReservationHandler : IReservationManager
{
    private readonly Room[] rooms;
    private readonly IReservationRepository reservationRepository;

    public ReservationHandler(Room[] rooms, IReservationRepository reservationRepository)
    {
        this.rooms = rooms;
        this.reservationRepository = reservationRepository;
    }

    public void AddReservation(Reservation reservation)
    {
        if (!IsRoomAvailable(reservation.RoomId))
        {
            Console.WriteLine("Room capacity is full. Reservation not added.");
            return;
        }

        if (!IsValidReservationDate(reservation.Dateday))
        {
            Console.WriteLine("Invalid reservation date. Reservation not added.");
            return;
        }

        if (!IsValidReservationTime(reservation.TimeSlot))
        {
            Console.WriteLine("Invalid reservation time. Reservation not added.");
            return;
        }

        if (!IsReservationDateInFuture(reservation.Dateday, reservation.TimeSlot))
        {
            Console.WriteLine("Cannot add reservation for past dates. Reservation not added.");
            return;
        }

        if (IsDuplicateReservation(reservation))
        {
            Console.WriteLine("Duplicate reservation found. Reservation not added.");
            return;
        }

        reservationRepository.AddReservation(reservation);
        Console.WriteLine("Reservation added successfully.");
    }

    public void DeleteReservation(string roomId)
    {
        reservationRepository.DeleteReservation(roomId);
    }

    public void CheckReservation(string roomId)
    {
        var roomReservations = reservationRepository.GetAllReservations().Where(r => r.RoomId == roomId).ToList();
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
        var allReservations = reservationRepository.GetAllReservations();
        if (allReservations.Any())
        {
            Console.WriteLine("All Reservations:");
            foreach (var reservation in allReservations)
            {
                var room = GetRoomById(reservation.RoomId);
                Console.WriteLine($"Room ID: {room.RoomId}, Room Name: {room.RoomName}, Day: {reservation.DayOfWeek}, Reservation Date: {reservation.Dateday}, Time: {reservation.TimeSlot}, Reserved by: {reservation.ReserverName}");
            }
        }
        else
        {
            Console.WriteLine("No reservations found.");
        }
    }

    private bool IsRoomAvailable(string roomId)
    {
        var room = GetRoomById(roomId);
        if (room != null)
        {
            int reservedCount = reservationRepository.GetAllReservations().Count(r => r.RoomId == roomId);
            return reservedCount < room.Capacity;
        }
        return false;
    }

    private Room GetRoomById(string roomId)
    {
        return rooms.FirstOrDefault(r => r.RoomId == roomId);
    }

    private bool IsValidReservationDate(string date)
    {
        return DateTime.TryParseExact(date, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _);
    }

    private bool IsValidReservationTime(string time)
    {
        return TimeSpan.TryParseExact(time, @"hh\:mm", null, out _);
    }

    private bool IsReservationDateInFuture(string date, string time)
    {
        DateTime reservationDateTime = DateTime.Parse(date).Date + TimeSpan.Parse(time);
        return reservationDateTime > DateTime.Now;
    }

    private bool IsDuplicateReservation(Reservation newReservation)
    {
        return reservationRepository.GetAllReservations().Any(r =>
            r.RoomId == newReservation.RoomId &&
            r.Dateday == newReservation.Dateday &&
            r.TimeSlot == newReservation.TimeSlot);
    }
}
