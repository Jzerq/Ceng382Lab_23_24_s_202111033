public class ReservationService : IReservationService
{
    private readonly ReservationHandler reservationHandler;

    public ReservationService(ReservationHandler reservationHandler)
    {
        this.reservationHandler = reservationHandler;
    }

    public void AddReservation(Reservation reservation, string roomId)
    {
        reservationHandler.addReservation(reservation, roomId);
    }

    public void DeleteReservation(string roomId, DateTime date, DateTime time, string reserverName)
    {
        reservationHandler.deleteReservation(roomId, date, time, reserverName);
    }

    public void DisplayWeeklySchedule()
    {
        reservationHandler.displayWeeklySchedule();
    }
}
