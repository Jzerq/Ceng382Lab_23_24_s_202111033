using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApp1.Data; 
using  WebApp1.Models; 
public class EditRoomModel : PageModel
{
    private readonly WebAppDataBaseContext _context;

    public EditRoomModel(WebAppDataBaseContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Room Room { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Room = await _context.Rooms.FindAsync(id);

        if (Room == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Room).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!RoomExists(Room.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./RoomList");
    }

    private bool RoomExists(int id)
    {
        return _context.Rooms.Any(e => e.Id == id);
    }
}
