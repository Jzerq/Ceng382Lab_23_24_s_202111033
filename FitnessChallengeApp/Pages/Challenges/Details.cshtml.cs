using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FitnessChallengeApp.Pages.Challenges
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Challenge Challenge { get; set; }
        public List<LeaderboardEntry> Leaderboard { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ViewData["BodyClass"] = "challenges";
            Challenge = await _context.Challenges.FindAsync(id);

            if (Challenge == null)
            {
                return NotFound();
            }

            Leaderboard = await _context.UserChallenges
                .Where(uc => uc.ChallengeId == id)
                .OrderByDescending(uc => uc.Points)
                .Select(uc => new LeaderboardEntry
                {
                    Username = _context.Users.Find(uc.UserId).UserName,
                    Points = uc.Points
                })
                .ToListAsync();

            return Page();
        }

        public class LeaderboardEntry
        {
            public string Username { get; set; }
            public int Points { get; set; }
        }
    }
}
