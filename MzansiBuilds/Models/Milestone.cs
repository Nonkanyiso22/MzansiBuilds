using System;
using System.ComponentModel.DataAnnotations;

namespace MzansiBuilds.Models
{
    public class Milestone
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public int ProjectId { get; set; }
        public DateTime AchievedAt { get; set; } = DateTime.Now;
    }
}
