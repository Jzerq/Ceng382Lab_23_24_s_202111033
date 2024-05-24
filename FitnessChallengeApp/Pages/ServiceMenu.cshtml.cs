using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessChallengeApp.Pages
{
    [Authorize]
    public class ServiceMenuModel : PageModel
    {
        public void OnGet()
        {
            ViewData["BodyClass"] = "service-menu";
        }
    }
}
