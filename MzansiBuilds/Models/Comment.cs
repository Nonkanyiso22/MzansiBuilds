
using System;
using System.ComponentModel.DataAnnotations;

namespace MzansiBuilds.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public string UserId { get; set; } // Commenter
        public int ProjectId { get; set; } // Related project

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}