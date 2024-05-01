using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;


/*The Single Responsibility Principle (SRP) advocates for classes to have only one responsibility, promoting maintainability and reusability by separating concerns.

Dependency Injection (DI) fosters loose coupling by allowing external provision of a class's dependencies, enhancing testability and modularity.

The Program class violates SRP by handling user input, reservations, logging, and file I/O, leading to multiple reasons for change. Additionally, it violates DI by directly instantiating concrete implementations like ReservationHandler and FileLogger, hindering flexibility and testability.

This tight coupling with concrete implementations rather than abstractions makes it challenging to extend functionality or swap implementations without modifying the class.

Adhering to SRP and DI principles is crucial for scalability, testability, collaboration, flexibility, reusability, maintainability, and modularity.
*/


class Program
{
    static void Main(string[] args)
    {
        var jsonString = File.ReadAllText("RoomData.json");
        var options = new JsonSerializerOptions()
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
        };

        var roomData = JsonSerializer.Deserialize<RoomData>(jsonString, options);
        var fileLogger = new FileLogger("ReservationData.json");
        var reservationRepository = new ReservationRepository();
        var reservationHandler = new ReservationHandler(roomData, fileLogger);

        var program = new Program(reservationRepository, fileLogger, reservationHandler);
        program.Run();
    }

    private readonly IReservationRepository _reservationRepository;
    private readonly ILogger _logger;
    private readonly ReservationHandler _reservationHandler;

    public Program(IReservationRepository reservationRepository, ILogger logger, ReservationHandler reservationHandler)
    {
        _reservationRepository = reservationRepository;
        _logger = logger;
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
        Console.WriteLine("4. Display Reservation Schedule");
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
}
