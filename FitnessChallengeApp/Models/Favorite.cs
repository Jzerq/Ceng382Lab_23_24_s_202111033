using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessChallengeApp.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ChallengeId { get; set; }

        public string Title { get; set; } 
    }
}
