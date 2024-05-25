using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FitnessChallengeApp.Pages.Challenges
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Challenge Challenge { get; set; }
        public List<LeaderboardEntry> Leaderboard { get; set; }
        public List<UserRating> Ratings { get; set; }
        [BindProperty]
        public UserRating Rating { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Challenge = await _context.Challenges
                .Include(c => c.UserChallenges)
                .Include(c => c.UserRatings)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Challenge == null)
            {
                _logger.LogWarning("Challenge with ID {Id} not found.", id);
                return NotFound("The challenge you are looking for does not exist.");
            }

            Leaderboard = await _context.UserChallenges
                .Where(uc => uc.ChallengeId == id)
                .OrderByDescending(uc => uc.Points)
                .Select(uc => new LeaderboardEntry
                {
                    Username = _context.Users.FirstOrDefault(u => u.Id == uc.UserId).UserName,
                    Points = uc.Points
                })
                .ToListAsync();

            Ratings = await _context.UserRatings
                .Where(r => r.ChallengeId == id)
                .Select(r => new UserRating
                {
                    UserId = r.UserId,
                    ChallengeId = r.ChallengeId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    Username = _context.Users.FirstOrDefault(u => u.Id == r.UserId).UserName
                })
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostRateAsync(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                _logger.LogWarning("Challenge with ID {Id} not found.", id);
                return NotFound("The challenge you are looking for does not exist.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Rating.UserId = userId;
            Rating.ChallengeId = id;

            _context.UserRatings.Add(Rating);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Challenges/Details", new { id });
        }

        public class LeaderboardEntry
        {
            public string Username { get; set; }
            public int Points { get; set; }
        }
    }
}
