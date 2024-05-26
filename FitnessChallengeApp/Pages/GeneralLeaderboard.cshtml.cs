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

            var userChallenges = await _context.UserChallenges
                .GroupBy(uc => uc.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalPoints = g.Sum(uc => uc.Points)
                })
                .OrderByDescending(g => g.TotalPoints)
                .ToListAsync();

            Leaderboard = userChallenges.Select(uc => new LeaderboardEntry
            {
                UserId = uc.UserId,
                TotalPoints = uc.TotalPoints
            }).ToList();

            AssignGenericUsernames();
        }

        private void AssignGenericUsernames()
        {
            var userMapping = new Dictionary<string, string>();
            int userCounter = 1;

            foreach (var entry in Leaderboard)
            {
                if (!userMapping.ContainsKey(entry.UserId))
                {
                    userMapping[entry.UserId] = $"User{userCounter++}";
                }
                entry.Username = userMapping[entry.UserId];
            }
        }

        public class LeaderboardEntry
        {
            public string UserId { get; set; }
            public string Username { get; set; }
            public int TotalPoints { get; set; }
        }
    }
}
