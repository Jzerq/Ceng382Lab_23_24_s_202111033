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

    private readonly IReservationRepository _reservationRepository;

    public ReservationService(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public List<Reservation> DisplayReservationByReserver(string name)
    {
        return _reservationRepository.GetReservations().Where(reservation => reservation.reserverName == name).ToList();
    }

    public List<Reservation> DisplayReservationByRoomId(string id)
    {
        return _reservationRepository.GetReservations().Where(reservation => reservation.room.roomId == id).ToList();
    }
    


}
