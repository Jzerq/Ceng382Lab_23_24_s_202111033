using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp1.Data;
using WebApp1.Models;

public class CreateReservationModel : PageModel
{
    private readonly WebAppDataBaseContext _context;
    private readonly ILogger<CreateReservationModel> _logger;

    public CreateReservationModel(WebAppDataBaseContext context, ILogger<CreateReservationModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public Reservation Reservation { get; set; }
    public SelectList Rooms { get; set; }

    public void OnGet()
    {
        Rooms = new SelectList(_context.Rooms.ToList(), "Id", "RoomName");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Rooms = new SelectList(_context.Rooms.ToList(), "Id", "RoomName");
            return Page();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        Reservation.UserId = userId;
        _context.Reservations.Add(Reservation);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Reservation created: {@Reservation}", Reservation);

        return RedirectToPage("./Index");
    }
}
