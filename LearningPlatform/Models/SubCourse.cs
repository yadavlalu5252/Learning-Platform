using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class SubCourse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MasterCourseId { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourse { get; set; } = null!;

        [Required]
        public string SubCourseName { get; set; } = null!;

        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public string Thumbnail { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; } = null!;
    }
}