using System;
using Classes;

public class ReservationService : IReservationService
{
    private IReservationManager reservationManager;

    public ReservationService(IReservationManager reservationManager)
    {
        this.reservationManager = reservationManager;
    }

    public void AddReservation(Reservation reservation)
    {
        reservationManager.AddReservation(reservation);
    }

    public void DeleteReservation(string roomId)
    {
        reservationManager.DeleteReservation(roomId);
    }

    public void DisplayWeeklySchedule()
    {
        
        Console.WriteLine("Displaying Weekly Schedule...");
    }
}
