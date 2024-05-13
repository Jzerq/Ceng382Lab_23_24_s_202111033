using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApp1.Data; 

using  WebApp1.Models; 

public class AddRoomModel : PageModel
{
    private readonly ILogger<AddRoomModel> _logger;
    private readonly WebAppDataBaseContext _context;

    public AddRoomModel(ILogger<AddRoomModel> logger, WebAppDataBaseContext context) 
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

        return RedirectToPage("./AddRoom");
    }
}
