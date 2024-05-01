public class ReservationRepository : IReservationRepository
{
    private List<Reservation> _reservations;

    public ReservationRepository()
    {
        _reservations = new List<Reservation>();
    }

    public void AddReservation(Reservation reservation)
    {
        _reservations.Add(reservation);
    }

    public void DeleteReservation(string roomId, DateTime date, DateTime time, string reserverName)
    {
        _reservations.RemoveAll(r => r.room.roomId == roomId && r.date == date && r.time == time && r.reserverName == reserverName);
    }

    public List<Reservation> GetAllReservations()
    {
        return _reservations;
    }
}
