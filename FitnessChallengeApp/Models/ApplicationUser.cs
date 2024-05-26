using Microsoft.AspNetCore.Identity;

namespace FitnessChallengeApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        
        public string? Bio { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime? JoinDate { get; set; }
        public byte[]? Photo { get; set; }
    }

}
