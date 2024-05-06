using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class AddRoomModel : PageModel
{
    private readonly ILogger<AddRoomModel> _logger;
    private readonly AppDbContext _context;

    public AddRoomModel(ILogger<AddRoomModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [BindProperty]
    public Room Room { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Rooms.Add(Room);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
