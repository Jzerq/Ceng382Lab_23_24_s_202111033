using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using FitnessChallengeApp.Data;
using FitnessChallengeApp.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FitnessChallengeApp.Pages.Challenges
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Challenge Challenge { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync method called.");

            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("User is not authenticated.");
                return Challenge(); // Or RedirectToPage("/Account/Login") if you want to redirect to login
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID claim is missing.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the challenge. Please try again.");
                return Page();
            }

            Challenge.CreatedDate = DateTime.Now;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is not valid.");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning("Error: {ErrorMessage}", error.ErrorMessage);
                    }
                }
                return Page();
            }

            try
            {
                _context.Challenges.Add(Challenge);
                await _context.SaveChangesAsync();

                var userChallenge = new UserChallenge
                {
                    UserId = userId,
                    ChallengeId = Challenge.Id,
                    JoinDate = DateTime.Now
                };

                _context.UserChallenges.Add(userChallenge);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Challenge created successfully!";
                _logger.LogInformation("Challenge created successfully.");
                return RedirectToPage("/Challenges/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while creating the challenge: {Message}", ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while creating the challenge. Please try again.");
                return Page();
            }
        }
    }
}
