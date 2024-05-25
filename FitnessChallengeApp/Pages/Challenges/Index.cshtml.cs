using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitnessChallengeApp.Pages.Challenges
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Challenge> Challenges { get; set; }
        public IList<UserChallenge> UserChallenges { get; set; }
        public IList<Favorite> Favorites { get; set; }
        public Dictionary<int, string> ChallengeCreators { get; set; }
        public string Keyword { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }

        public async Task OnGetAsync(string keyword, string difficulty, string category)
        {
            var challenges = from c in _context.Challenges select c;

            if (!string.IsNullOrEmpty(keyword))
            {
                challenges = challenges.Where(c => c.Title.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(difficulty))
            {
                challenges = challenges.Where(c => c.DifficultyLevel == difficulty);
            }

            if (!string.IsNullOrEmpty(category))
            {
                challenges = challenges.Where(c => c.Category == category);
            }

            Challenges = await challenges.ToListAsync();
            UserChallenges = await _context.UserChallenges.ToListAsync();
            Favorites = await _context.Favorites.ToListAsync();

            ChallengeCreators = await _context.UserChallenges
                .Where(uc => Challenges.Select(c => c.Id).Contains(uc.ChallengeId))
                .GroupBy(uc => uc.ChallengeId)
                .ToDictionaryAsync(g => g.Key, g => g.First().UserId);
        }

        public async Task<IActionResult> OnPostAddFavoriteAsync(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }

            var favorite = new Favorite { UserId = userId, ChallengeId = id, Title = challenge.Title };
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
            return RedirectToPage("/Challenges/Index");
        }

        public async Task<IActionResult> OnPostRemoveFavoriteAsync(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.ChallengeId == id);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Challenges/Index");
        }
    }
}
