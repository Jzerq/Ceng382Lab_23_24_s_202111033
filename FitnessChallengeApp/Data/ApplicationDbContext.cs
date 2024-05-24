using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FitnessChallengeApp.Models;

namespace FitnessChallengeApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<UserChallenge> UserChallenges { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
    }
}
