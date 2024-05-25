using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FitnessChallengeApp.Pages.Challenges
{
    public class LeaveModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LeaveModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Challenge Challenge { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Challenge = await _context.Challenges.FindAsync(id);

            if (Challenge == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var userChallenge = await _context.UserChallenges
                .FirstOrDefaultAsync(uc => uc.ChallengeId == id && uc.UserId == userId);

            if (userChallenge != null)
            {
                _context.UserChallenges.Remove(userChallenge);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Challenges/Index");
        }
    }
}
