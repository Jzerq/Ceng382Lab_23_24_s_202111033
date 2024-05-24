using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessChallengeApp.Models;
using System.Threading.Tasks;
using System;
using System.IO;

namespace FitnessChallengeApp.Areas.Identity.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string Bio { get; set; }

        [BindProperty]
        public DateTime Birthdate { get; set; }

        [BindProperty]
        public DateTime JoinDate { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        public byte[] CurrentPhoto { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["BodyClass"] = "profile";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            Bio = user.Bio;
            Birthdate = user.Birthdate;
            JoinDate = user.JoinDate;
            CurrentPhoto = user.Photo;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["BodyClass"] = "profile";
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (ModelState.IsValid)
            {
                user.Bio = Bio;
                user.Birthdate = Birthdate;

                if (Photo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Photo.CopyToAsync(memoryStream);
                        user.Photo = memoryStream.ToArray();
                    }
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToPage();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
