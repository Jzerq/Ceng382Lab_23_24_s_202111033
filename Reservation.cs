namespace Classes
{
    public record Reservation
    {
        public string DayOfWeek { get; init; }
        public string TimeSlot { get; init; }
        public string ReserverName { get; init; }
        public string Dateday { get; init; }
        public string RoomId { get; init; }
    }
}
