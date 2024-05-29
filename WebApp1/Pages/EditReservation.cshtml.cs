using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp1.Models;

public class EditReservationModel : PageModel
{
    private readonly WebAppDatabaseContext _context;
    private readonly ILogger<EditReservationModel> _logger;

    public EditReservationModel(WebAppDatabaseContext context, ILogger<EditReservationModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public Reservation Reservation { get; set; }
    public SelectList Rooms { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Reservation = await _context.Reservations.FindAsync(id);

        if (Reservation == null)
        {
            return NotFound();
        }

        Rooms = new SelectList(_context.Rooms.ToList(), "RoomId", "RoomName");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Rooms = new SelectList(_context.Rooms.ToList(), "RoomId", "RoomName");
            return Page();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null || Reservation.UserId != userId)
        {
            return Unauthorized();
        }

        var existingReservation = await _context.Reservations.FindAsync(Reservation.Id);
        if (existingReservation == null)
        {
            return NotFound();
        }

        existingReservation.Name = Reservation.Name;
        existingReservation.RoomId = Reservation.RoomId;
        existingReservation.StartTime = Reservation.StartTime;
        existingReservation.EndTime = Reservation.EndTime;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Reservation updated: {@Reservation}", Reservation);
            TempData["SuccessMessage"] = "Reservation updated successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating reservation: {@Reservation}", Reservation);
            TempData["ErrorMessage"] = "An error occurred while updating the reservation. Please try again.";
            return Page();
        }

        return RedirectToPage("/ReservationTable");
    }
}
