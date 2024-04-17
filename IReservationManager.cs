using Classes;

public interface IReservationManager
{
    void AddReservation(Reservation reservation);
    void DeleteReservation(string roomId);
    void CheckReservation(string roomId);
    void DisplayReservations();
}
