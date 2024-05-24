using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System;
using System.Threading.Tasks;

namespace FitnessChallengeApp.Pages.Challenges
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Challenge Challenge { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ViewData["BodyClass"] = "challenges";
            Challenge = await _context.Challenges.FindAsync(id);

            if (Challenge == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (Challenge.CreatedBy != user.Id)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var challenge = await _context.Challenges.FindAsync(Challenge.Id);

            if (challenge == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (challenge.CreatedBy != user.Id)
            {
                return Forbid();
            }

            challenge.Title = Challenge.Title;
            challenge.Description = Challenge.Description;
            challenge.Category = Challenge.Category;
            challenge.DifficultyLevel = Challenge.DifficultyLevel;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
