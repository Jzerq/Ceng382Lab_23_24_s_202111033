using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;

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
        public string Keyword { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }

        public async Task OnGetAsync(string keyword, string difficulty, string category)
        {
            Keyword = keyword;
            Difficulty = difficulty;
            Category = category;

            IQueryable<Challenge> query = _context.Challenges;

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(c => c.Title.Contains(keyword));
            }

            if (!string.IsNullOrEmpty(difficulty))
            {
                query = query.Where(c => c.DifficultyLevel == difficulty);
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(c => c.Category == category);
            }

            Challenges = await query.ToListAsync();
        }
    }
}
