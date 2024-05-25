using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace FitnessChallengeApp.Models
{
    public class Challenge
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string DifficultyLevel { get; set; }

        [BindNever]
        public DateTime CreatedDate { get; set; }

        public ICollection<UserChallenge> UserChallenges { get; set; } = new List<UserChallenge>();
        public ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();
        

            

    }
}
