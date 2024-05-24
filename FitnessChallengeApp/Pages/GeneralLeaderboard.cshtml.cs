using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FitnessChallengeApp.Pages
{
    [Authorize]
    public class GeneralLeaderboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GeneralLeaderboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<LeaderboardEntry> Leaderboard { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["BodyClass"] = "leaderboard";

            Leaderboard = await _context.UserChallenges
                .GroupBy(uc => uc.UserId)
                .Select(g => new LeaderboardEntry
                {
                    Username = _context.Users.Find(g.Key).UserName,
                    TotalPoints = g.Sum(uc => uc.Points)
                })
                .OrderByDescending(le => le.TotalPoints)
                .ToListAsync();
        }

        public class LeaderboardEntry
        {
            public string Username { get; set; }
            public int TotalPoints { get; set; }
        }
    }
}
