using System;
using System.Collections.Generic;

namespace WebApp1.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;    
    public string Name { get; set; } = null!;

    public int RoomId { get; set; }

    public DateTime DateTime { get; set; }

    public virtual Room Room { get; set; } = null!;
}
