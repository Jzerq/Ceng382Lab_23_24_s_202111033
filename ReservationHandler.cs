using System;
using System.Collections.Generic;
using System.Linq;

public class ReservationHandler : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly RoomHandler _roomHandler;
    private readonly LogHandler _logHandler;

    public ReservationHandler(IReservationRepository reservationRepository, RoomHandler roomHandler, LogHandler logHandler)
    {
        _reservationRepository = reservationRepository;
        _roomHandler = roomHandler;
        _logHandler = logHandler;
    }

    public void AddReservation(Reservation reservation)
    {
        _reservationRepository.AddReservation(reservation);
        _logHandler.AddLog(new LogRecord(DateTime.Now, reservation.ReserverName, GetRoomNameById(reservation.Room)));
    }

    public void DeleteReservation(Reservation reservation)
    {
        _reservationRepository.DeleteReservation(reservation);
        _logHandler.AddLog(new LogRecord(DateTime.Now, reservation.ReserverName, GetRoomNameById(reservation.Room)));
    }

    public void DisplayWeeklySchedule()
    {
        // Display weekly schedule logic
    }

    private string GetRoomNameById(string roomId)
    {
        var rooms = _roomHandler.GetRooms();
        var room = rooms.FirstOrDefault(r => r.ID == roomId);
        return room != null ? room.Name : "Unknown";
    }
}
