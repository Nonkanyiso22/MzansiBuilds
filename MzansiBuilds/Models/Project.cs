using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MzansiBuilds.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string Stage { get; set; } // Idea, In Progress, Completed
        public string SupportNeeded { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string UserId { get; set; } // Owner

        // New fields for extra credit
        public bool CollaborationRequested { get; set; } = false;
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Milestone> Milestones { get; set; } = new List<Milestone>();
    }
}