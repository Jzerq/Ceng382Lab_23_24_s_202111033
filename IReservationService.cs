public interface IReservationService
{
    void AddReservation(Reservation reservation, string roomId);
    void DeleteReservation(string roomId, DateTime date, DateTime time, string reserverName);
    void DisplayWeeklySchedule();
}
