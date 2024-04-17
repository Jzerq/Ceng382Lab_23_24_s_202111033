// IReservationRepository.cs
public interface IReservationRepository
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(string roomId);
    List<Reservation> GetAllReservations();
}
