using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp1.Models;

public class MyReservationsModel : PageModel
{
    private readonly WebAppDatabaseContext _context;
    private readonly ILogger<MyReservationsModel> _logger;

    public MyReservationsModel(WebAppDatabaseContext context, ILogger<MyReservationsModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IList<Reservation> CurrentReservations { get; set; }
    public IList<Reservation> PastReservations { get; set; }

    public async Task OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            RedirectToPage("/Account/Login");
        }

        CurrentReservations = await _context.Reservations
            .Include(r => r.Room)
            .Where(r => r.UserId == userId && r.EndTime >= DateTime.Now)
            .ToListAsync();

        PastReservations = await _context.Reservations
            .Include(r => r.Room)
            .Where(r => r.UserId == userId && r.EndTime < DateTime.Now)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            TempData["ErrorMessage"] = "Reservation not found.";
            return RedirectToPage();
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Reservation canceled successfully.";
        return RedirectToPage();
    }
}
