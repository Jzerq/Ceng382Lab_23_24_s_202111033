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
    public class JoinModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JoinModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);

            if (challenge == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var userChallenge = new UserChallenge
            {
                UserId = user.Id,
                ChallengeId = id,
                JoinDate = DateTime.Now,
                Points = 0
            };

            _context.UserChallenges.Add(userChallenge);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
