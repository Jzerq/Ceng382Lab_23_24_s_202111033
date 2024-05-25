using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitnessChallengeApp.Pages.Challenges
{
    public class JoinModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public JoinModel(ApplicationDbContext context)
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

            var random = new Random();
            var points = random.Next(1, 101);

            var userChallenge = new UserChallenge
            {
                ChallengeId = id,
                UserId = userId,
                Points = points,   
                JoinDate = DateTime.Now
            };

            _context.UserChallenges.Add(userChallenge);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Challenges/Index");
        }
    }
}
