using System;
using System.Collections.Generic;

namespace labsapp.Models;

public partial class TblTodo
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? EndDate { get; set; }
}
