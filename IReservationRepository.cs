public interface IReservationRepository
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(string roomId, DateTime date, DateTime time, string reserverName);
    List<Reservation> GetAllReservations();
}
