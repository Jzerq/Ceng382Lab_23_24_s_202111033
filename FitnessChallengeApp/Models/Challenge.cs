using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessChallengeApp.Models
{
    public class Challenge
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string DifficultyLevel { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
