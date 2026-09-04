using System.ComponentModel.DataAnnotations;

namespace LearningPlatform.Models
{
    public class MasterCourse
    {
        public int Id { get; set; }

        [Required]
        public string MasterCourseName { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Thumbnail { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; }
    }
}