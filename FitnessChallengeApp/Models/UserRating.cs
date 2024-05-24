using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessChallengeApp.Models
{
    public class UserRating
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ChallengeId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
