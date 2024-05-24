using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FitnessChallengeApp.Pages
{
    [Authorize]
    public class FavoritesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FavoritesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Favorite> Favorites { get; set; }

        public async Task OnGetAsync()
        {
            ViewData["BodyClass"] = "favorites";
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Favorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => new Favorite { Id = f.ChallengeId, Title = _context.Challenges.Find(f.ChallengeId).Title })
                .ToListAsync();
        }
    }
}
