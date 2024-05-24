using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessChallengeApp.Models
{
    public class UserChallenge
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ChallengeId { get; set; }

        public int Points { get; set; }

        public DateTime JoinDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
