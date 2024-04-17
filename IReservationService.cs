public interface IReservationService
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(string roomId);
    void DisplayWeeklySchedule();
}