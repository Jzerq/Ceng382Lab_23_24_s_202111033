using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        public List<UserRatingViewModel> Ratings { get; set; }

        [BindProperty]
        public UserRating Rating { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadChallengeDetailsAsync(id);
            if (Challenge == null)
            {
                return NotFound("The challenge you are looking for does not exist.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostRateAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await LoadChallengeDetailsAsync(id);
                return Page();
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                _logger.LogWarning("User ID not found.");
                return RedirectToPage("/Account/Login"); // or another appropriate action
            }

            Rating.UserId = userId;
            Rating.ChallengeId = id;

            try
            {
                _context.UserRatings.Add(Rating);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Rating added by {UserId} for challenge {ChallengeId}", userId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the rating.");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the rating.");
                await LoadChallengeDetailsAsync(id);
                return Page();
            }

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostSaveChallengeAsync(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                _logger.LogWarning("User ID not found.");
                return RedirectToPage("/Account/Login"); // or another appropriate action
            }

            // Ensure Challenge property is loaded
            await LoadChallengeDetailsAsync(id);

            if (Challenge == null)
            {
                return NotFound("The challenge you are trying to save does not exist.");
            }

            var favorite = new Favorite
            {
                UserId = userId,
                ChallengeId = id,
                Title = Challenge.Title
            };

            try
            {
                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Challenge saved by {UserId} for challenge {ChallengeId}", userId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the challenge.");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the challenge.");
                await LoadChallengeDetailsAsync(id);
                return Page();
            }

            return RedirectToPage(new { id });
        }

        private async Task LoadChallengeDetailsAsync(int id)
        {
            _logger.LogInformation("Fetching challenge with ID {Id}", id);
            Challenge = await _context.Challenges
                .Include(c => c.UserChallenges)
                .Include(c => c.UserRatings)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Challenge != null)
            {
                var userChallenges = await _context.UserChallenges
                    .Where(uc => uc.ChallengeId == id)
                    .OrderByDescending(uc => uc.Points)
                    .ToListAsync();

                Leaderboard = userChallenges.Select(uc => new LeaderboardEntry
                {
                    UserId = uc.UserId,
                    Points = uc.Points
                }).ToList();

                var userRatings = await _context.UserRatings
                    .Where(r => r.ChallengeId == id)
                    .ToListAsync();

                Ratings = userRatings.Select(r => new UserRatingViewModel
                {
                    UserId = r.UserId,
                    Rating = r.Rating,
                    Comment = r.Comment
                }).ToList();

                AssignGenericUsernames();
            }
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

            foreach (var rating in Ratings)
            {
                if (!userMapping.ContainsKey(rating.UserId))
                {
                    userMapping[rating.UserId] = $"User{userCounter++}";
                }
                rating.Username = userMapping[rating.UserId];
            }
        }

        public class LeaderboardEntry
        {
            public string UserId { get; set; }
            public string Username { get; set; }
            public int Points { get; set; }
        }

        public class UserRatingViewModel
        {
            public string UserId { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
            public string Username { get; set; }
        }
    }
}
