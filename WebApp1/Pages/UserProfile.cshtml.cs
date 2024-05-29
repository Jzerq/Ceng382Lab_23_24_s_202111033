using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp1.Models;

public class UserProfileModel : PageModel
{
    private readonly WebAppDatabaseContext _context;
    private readonly ILogger<UserProfileModel> _logger;

    public UserProfileModel(WebAppDatabaseContext context, ILogger<UserProfileModel> logger)
    {
        _context = context;
        _logger = logger;
    }

    [BindProperty]
    public UserDetail UserDetails { get; set; }

    [BindProperty]
    public IList<Reservation> BookingHistory { get; set; }

    [BindProperty]
    public IFormFile PhotoUpload { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return RedirectToPage("/Account/Login");
        }

        UserDetails = await _context.UserDetails
            .Where(u => u.UserId == userId)
            .FirstOrDefaultAsync();

        if (UserDetails == null)
        {
            UserDetails = new UserDetail
            {
                UserId = userId
            };
            _context.UserDetails.Add(UserDetails);
            await _context.SaveChangesAsync();
        }

        BookingHistory = await _context.Reservations
            .Include(r => r.Room)
            .Where(r => r.UserId == userId)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userDetails = await _context.UserDetails
            .Where(u => u.UserId == userId)
            .FirstOrDefaultAsync();

        if (userDetails == null)
        {
            return NotFound();
        }

        userDetails.City = UserDetails.City;
        userDetails.Description = UserDetails.Description;

        if (PhotoUpload != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await PhotoUpload.CopyToAsync(memoryStream);
                userDetails.Photo = memoryStream.ToArray();
            }
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Profile updated successfully.";

        return RedirectToPage();
    }
}
