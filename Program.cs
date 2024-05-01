using System;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        var jsonString = File.ReadAllText("RoomData.json");
        var options = new JsonSerializerOptions();


        var roomData = JsonSerializer.Deserialize<RoomData>(jsonString, options);
        var fileLogger = new FileLogger("ReservationLog.json");
        var reservationRepository = new ReservationRepository();
        var reservationHandler = new ReservationHandler(roomData, fileLogger, reservationRepository);
        var reservationService = new ReservationService(reservationRepository);
        var logService = new LogService(fileLogger.GetLogs());

        var program = new Program(reservationService, logService, reservationHandler);
        program.Run();
    }

    private readonly ReservationService _reservationService;
    private readonly LogService _logService;
    private readonly ReservationHandler _reservationHandler;

    public Program(ReservationService reservationService, LogService logService, ReservationHandler reservationHandler)
    {
        _reservationService = reservationService;
        _logService = logService;
        _reservationHandler = reservationHandler;
    }

    public void Run()
    {
        int choice;
        do
        {
            DisplayMenu();
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input! Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ViewRoomInformation();
                    break;
                case 2:
                    AddReservation();
                    break;
                case 3:
                    DeleteReservation();
                    break;
                case 4:
                    DisplayWeeklySchedule();
                    break;
                case 5:
                    DisplayReservationsByReserver();
                    break;
                case 6:
                    DisplayReservationsByRoomId();
                    break;
                case 7:
                    DisplayLogsByName();
                    break;
                case 8:
                    DisplayLogsByTime();
                    break;
                case 0:
                    Console.WriteLine("Exiting...");
                    break;
                default:
                    Console.WriteLine("Invalid choice! Please select a valid option.");
                    break;
            }
        } while (choice != 0);
    }

    private void DisplayMenu()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. View Room Information");
        Console.WriteLine("2. Add Reservation");
        Console.WriteLine("3. Delete Reservation");
        Console.WriteLine("4. Display Weekly Schedule");
        Console.WriteLine("5. Display Reservations By Reserver");
        Console.WriteLine("6. Display Reservations By Room ID");
        Console.WriteLine("7. Display Logs By Name");
        Console.WriteLine("8. Display Logs By Time");
        Console.WriteLine("0. Exit");
        Console.Write("Enter your choice: ");
    }

    private void ViewRoomInformation()
    {
        if (_reservationHandler.RoomData?.Rooms != null)
        {
            Console.WriteLine("Room Information:");
            foreach (var room in _reservationHandler.RoomData.Rooms)
            {
                Console.WriteLine($"Room ID: {room.roomId}, Name: {room.roomName}, Capacity: {room.capacity}");
            }
        }
        else
        {
            Console.WriteLine("No room information available.");
        }
    }

    private void AddReservation()
    {
        Console.WriteLine("Enter room ID:");
        string roomId = Console.ReadLine();
        if (!_reservationHandler.IsRoomValid(roomId))
        {
            Console.WriteLine("Invalid room ID!");
            return;
        }

        Console.WriteLine("Enter date (yyyy-MM-dd):");
        DateTime date;
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
        {
            Console.WriteLine("Invalid date format! Please enter date in the format yyyy-MM-dd.");
            return;
        }

        Console.WriteLine("Enter time (HH:mm):");
        DateTime time;
        if (!DateTime.TryParseExact(Console.ReadLine(), "HH:mm", null, System.Globalization.DateTimeStyles.None, out time))
        {
            Console.WriteLine("Invalid time format!");
            return;
        }

        Console.WriteLine("Enter reserver name:");
        string reserverName = Console.ReadLine();

        _reservationHandler.addReservation(new Reservation
        {
            room = new Room { roomId = roomId },
            date = date,
            time = time,
            reserverName = reserverName
        }, roomId);
    }

    private void DeleteReservation()
    {
        Console.WriteLine("Enter room ID:");
        string roomId = Console.ReadLine();
        Console.WriteLine("Enter date (yyyy-MM-dd):");
        DateTime date;
        if (!DateTime.TryParse(Console.ReadLine(), out date))
        {
            Console.WriteLine("Invalid date format!");
            return;
        }
        Console.WriteLine("Enter time (HH:mm):");
        DateTime time;
        if (!DateTime.TryParseExact(Console.ReadLine(), "HH:mm", null, System.Globalization.DateTimeStyles.None, out time))
        {
            Console.WriteLine("Invalid time format!");
            return;
        }
        Console.WriteLine("Enter reserver name:");
        string reserverName = Console.ReadLine();

        _reservationHandler.deleteReservation(roomId, date, time, reserverName);
    }

    private void DisplayWeeklySchedule()
    {
        _reservationHandler.displayWeeklySchedule();
    }

    private void DisplayReservationsByReserver()
    {
        Console.WriteLine("Enter reserver name:");
        string reserverName = Console.ReadLine();
        var reservations = _reservationService.DisplayReservationByReserver(reserverName);
        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found for the specified reserver name.");
        }
        else
        {
            Console.WriteLine("Reservations for " + reserverName + ":");
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Room ID: {reservation.room.roomId}, Date: {reservation.date}, Time: {reservation.time}");
            }
        }
    }

    private void DisplayReservationsByRoomId()
    {
        Console.WriteLine("Enter room ID:");
        string roomId = Console.ReadLine();
        var reservations = _reservationService.DisplayReservationByRoomId(roomId);
        if (reservations.Count == 0)
        {
            Console.WriteLine("No reservations found for the specified room ID.");
        }
        else
        {
            Console.WriteLine("Reservations for Room ID " + roomId + ":");
            foreach (var reservation in reservations)
            {
                Console.WriteLine($"Reserver: {reservation.reserverName}, Date: {reservation.date}, Time: {reservation.time}");
            }
        }
    }

    private void DisplayLogsByName()
    {
        Console.WriteLine("Enter reserver name:");
        string reserverName = Console.ReadLine();
        var logs = _logService.DisplayLogsByName(reserverName);
        if (logs.Count == 0)
        {
            Console.WriteLine("No logs found for the specified reserver name.");
        }
        else
        {
            Console.WriteLine("Logs for " + reserverName + ":");
            foreach (var log in logs)
            {
                Console.WriteLine($"Timestamp: {log.Timestamp}, Status: {log.Status}, Room ID: {log.RoomID}");
            }
        }
    }

    private void DisplayLogsByTime()
    {
        Console.WriteLine("Enter start date and time (yyyy-MM-dd HH:mm):");
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime start))
        {
            Console.WriteLine("Invalid date and time format! Please enter in the format yyyy-MM-dd HH:mm.");
            return;
        }

        Console.WriteLine("Enter end date and time (yyyy-MM-dd HH:mm):");
        if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime end))
        {
            Console.WriteLine("Invalid date and time format! Please enter in the format yyyy-MM-dd HH:mm.");
            return;
        }

        var logs = _logService.DisplayLogs(start, end);
        if (logs.Count == 0)
        {
            Console.WriteLine("No logs found within the specified time interval.");
        }
        else
        {
            Console.WriteLine("Logs within the specified time interval:");
            foreach (var log in logs)
            {
                Console.WriteLine($"Timestamp: {log.Timestamp}, Status: {log.Status}, Room ID: {log.RoomID}");
            }
        }
    }
}