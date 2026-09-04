using System.ComponentModel.DataAnnotations;

namespace LearningPlatform.Models
{
    public class MasterCourse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string MasterCourseName { get; set; } = null!;

        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public string Thumbnail { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; } = null!;
    }
}