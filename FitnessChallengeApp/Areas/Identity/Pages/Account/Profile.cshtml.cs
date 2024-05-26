using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data; // Ensure this line is present
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FitnessChallengeApp.Areas.Identity.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context; // Ensure this line is present

        public ProfileModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context; // Ensure this line is present
        }

        [BindProperty]
        public string Bio { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        public string PhotoUrl { get; set; }
        public List<Favorite> SavedChallenges { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            Bio = user.Bio;

            if (user.Photo != null)
            {
                var base64 = Convert.ToBase64String(user.Photo);
                PhotoUrl = $"data:image/png;base64,{base64}";
            }

            SavedChallenges = await _context.Favorites
                .Where(f => f.UserId == user.Id)
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.Bio = Bio;

            if (Photo != null && Photo.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Photo.CopyToAsync(memoryStream);
                    user.Photo = memoryStream.ToArray();
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadAsync(user);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteSavedChallengeAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == user.Id);

            if (favorite == null)
            {
                return NotFound("The saved challenge was not found.");
            }

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            await LoadAsync(user);
            return RedirectToPage();
        }
    }
}
