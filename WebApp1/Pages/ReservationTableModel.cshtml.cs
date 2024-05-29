using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp1.Data;
using WebApp1.Models;

public class ReservationTableModel : PageModel
{
    private readonly WebAppDatabaseContext _context;

    public ReservationTableModel(WebAppDatabaseContext context)
    {
        _context = context;
    }

    public IList<Reservation> Reservations { get; set; }
    public DateTime StartOfWeek { get; private set; }

    [BindProperty(SupportsGet = true)]
    public ReservationFilter Filter { get; set; }

    public async Task OnGetAsync()
    {
        StartOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);

        var query = _context.Reservations.Include(r => r.Room).AsQueryable();

        if (!string.IsNullOrEmpty(Filter.RoomName))
        {
            query = query.Where(r => r.Room.RoomName.Contains(Filter.RoomName));
        }

        if (Filter.StartDate.HasValue && Filter.EndDate.HasValue)
        {
            query = query.Where(r => r.StartTime >= Filter.StartDate.Value && r.EndTime <= Filter.EndDate.Value);
        }

        if (Filter.Capacity.HasValue)
        {
            query = query.Where(r => r.Room.Capacity >= Filter.Capacity.Value);
        }

        Reservations = await query.ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public List<Reservation> GetReservationsForTime(DateTime date, int hour)
    {
        return Reservations.Where(r => r.StartTime.Date == date.Date && r.StartTime.Hour == hour).ToList();
    }
}

public class ReservationFilter
{
    public string RoomName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Capacity { get; set; }
}
