using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp1.Models;

public class RoomListModel : PageModel
{
    private readonly WebAppDatabaseContext _context;

    public RoomListModel(WebAppDatabaseContext context)
    {
        _context = context;
    }

    public IList<Room> Rooms { get; set; }

    public async Task OnGetAsync()
    {
        Rooms = await _context.Rooms.ToListAsync();
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostDeleteAsync(int deleteId)
    {
        var roomToDelete = await _context.Rooms.FindAsync(deleteId);

        if (roomToDelete != null)
        {
            _context.Rooms.Remove(roomToDelete);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}
