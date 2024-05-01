using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class ReservationHandler
{
    private readonly IReservationRepository _reservationRepository;

    private readonly List<Reservation>[][] reservations;
    private readonly RoomData roomData;
    private readonly ILogger logger;
    
    public ReservationHandler(RoomData roomData, ILogger logger,IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
        this.roomData = roomData;
        this.logger = logger;

        reservations = new List<Reservation>[7][];
        for (int i = 0; i < 7; i++)
        {
            reservations[i] = new List<Reservation>[24];
            for (int j = 0; j < 24; j++)
            {
                reservations[i][j] = new List<Reservation>();
            }
        }
    }

    public bool IsRoomValid(string roomId)
    {
        foreach (var room in roomData.Rooms)
        {
            if (room.roomId == roomId)
                return true;
        }
        return false;
    }

    public bool IsDateWithinCurrentWeek(DateTime date)
    {
        DateTime currentDate = DateTime.Now.Date;
        DateTime startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(6);
        return date >= startOfWeek && date <= endOfWeek;
    }

    public RoomData RoomData { get => roomData; }

    public void addReservation(Reservation reservation, string roomId)
    {
    
        if (reservation.date < DateTime.Today)
        {
            Console.WriteLine("Cannot add reservation for a past date.");
            return;
        }

        int dayOfWeek = (int)reservation.date.DayOfWeek;
        int hourOfDay = reservation.time.Hour;

       
        bool isReservationExists = reservations[dayOfWeek][hourOfDay]
            .Any(r => r.time == reservation.time && r.room.roomId == reservation.room.roomId);

        if (isReservationExists)
        {
            Console.WriteLine("There is already a reservation for this time and room ID. Cannot add it.");
            return;
        }

      
        reservations[dayOfWeek][hourOfDay].Add(reservation);
        Console.WriteLine("Reservation added successfully!\n");

        LogRecord logRecord = new LogRecord(DateTime.Now, reservation.reserverName, "Added", reservation.room.roomId);
        logger.Log(logRecord);

        

        
    }

    public void deleteReservation(string roomId, DateTime date, DateTime time, string reserverName)
    {
        if (!IsRoomValid(roomId))
        {
            Console.WriteLine("Invalid room ID!");
            return;
        }


        if (date < DateTime.Today)
        {
            Console.WriteLine("Cannot delete reservation for a past date.");
            return;
        }

        int dayOfWeek = (int)date.DayOfWeek;
        int hourOfDay = time.Hour;

        var matchingReservations = reservations[dayOfWeek][hourOfDay]
            .Where(r => r.room.roomId == roomId && r.date == date && r.time == time && r.reserverName == reserverName)
            .ToList();

        var reservation = reservations[dayOfWeek][hourOfDay]
            .FirstOrDefault(r => r.room.roomId == roomId
                                && r.date.Date == date.Date
                                && r.time.TimeOfDay == time.TimeOfDay
                                && r.reserverName == reserverName);

        if (reservation != null)
        {
            reservations[dayOfWeek][hourOfDay].Remove(reservation);
            Console.WriteLine("Reservation deleted successfully!\n");

      
            LogRecord logRecord = new LogRecord(DateTime.Now, reserverName, "Deleted", reservation.room.roomId);
            logger.Log(logRecord);
            
      
        }
        else
        {
            Console.WriteLine("The reservation does not exist.");
        }
    }

    public void displayWeeklySchedule()
    {
        DateTime currentDate = DateTime.Now.Date;
        DateTime startOfWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
        DateTime endOfWeek = startOfWeek.AddDays(6);

        for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
        {
            for (int hourOfDay = 0; hourOfDay < 24; hourOfDay++)
            {
                foreach (var reservation in reservations[dayOfWeek][hourOfDay])
                {
                    Room room = GetRoomById(reservation.room.roomId);
                    if (room != null)
                    {
                        Console.WriteLine($"  {reservation.date:yyyy-MM-dd}   {hourOfDay:00}:{reservation.time.Minute:00} - {room.roomName} - {reservation.reserverName}");
                    }
                }
            }
        }
    }

    private Room GetRoomById(string roomId)
    {
        foreach (var room in roomData.Rooms)
        {
            if (room.roomId == roomId)
                return room;
        }
        return null;
    }

}
