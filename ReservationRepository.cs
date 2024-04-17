// ReservationRepository.cs
public class ReservationRepository : IReservationRepository
{
    private List<Reservation> reservations;

    public ReservationRepository()
    {
        reservations = new List<Reservation>();
    }

    public void AddReservation(Reservation reservation)
    {
        reservations.Add(reservation);
    }

    public void DeleteReservation(string roomId)
    {
        var reservationToRemove = reservations.FirstOrDefault(r => r.RoomId == roomId);
        if (reservationToRemove != null)
        {
            reservations.Remove(reservationToRemove);
        }
    }

    public List<Reservation> GetAllReservations()
    {
        return reservations;
    }
}
