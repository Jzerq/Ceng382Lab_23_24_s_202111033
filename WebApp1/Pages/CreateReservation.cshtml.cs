using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp1.Models;

public class CreateReservationModel : PageModel
{
    private readonly WebAppDatabaseContext _context;
    private readonly ILogger<CreateReservationModel> _logger;

    public CreateReservationModel(WebAppDatabaseContext context, ILogger<CreateReservationModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public Reservation Reservation { get; set; }
    public SelectList Rooms { get; set; }
    public IList<Room> AvailableRooms { get; set; }

    public void OnGet()
    {
        Rooms = new SelectList(_context.Rooms.ToList(), "RoomId", "RoomName");
        AvailableRooms = _context.Rooms.ToList();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            {
                // Debug output to console
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Any())
                    {
                        Console.WriteLine($"Key: {state.Key}");
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"Error: {error.ErrorMessage}");
                        }
                    }
                }

                // Original logging code
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Any())
                    .Select(ms => new
                    {
                        Key = ms.Key,
                        Errors = ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    })
                    .ToList();

                _logger.LogError("ModelState is invalid. Errors: {@ModelStateErrors}", errors);

                Rooms = new SelectList(_context.Rooms.ToList(), "RoomId", "RoomName");
                AvailableRooms = _context.Rooms.ToList();
                
                TempData["ErrorMessage"] = "Invalid input. Please correct the errors and try again.";
                return Page();
            }


        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        bool isRoomAvailable = !_context.Reservations.Any(r =>
            r.RoomId == Reservation.RoomId &&
            r.StartTime < Reservation.EndTime &&
            r.EndTime > Reservation.StartTime);

       
        Reservation.UserId = userId;

        try
        {
            _context.Reservations.Add(Reservation);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Reservation created: {@Reservation}", Reservation);
            TempData["SuccessMessage"] = "Reservation created successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reservation: {@Reservation}", Reservation);
            TempData["ErrorMessage"] = "An error occurred while creating the reservation. Please try again.";
            return Page();
        }

        return RedirectToPage("./CreateReservation");
    }
}
