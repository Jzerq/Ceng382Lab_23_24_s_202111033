public class Reservation 
{
    [Key]
    public int id { get; set; }
    public string ?Name { get; set; }

    public Room ?Room{ get; set; }
    public DateTime DateTime { get; set; }

}