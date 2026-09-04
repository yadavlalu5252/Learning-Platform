using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class SubCourse
    {
        public int Id { get; set; }

        [Required]
        public int MasterCourseId { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourse { get; set; }

        [Required]
        public string SubCourseName { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Thumbnail { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; }
    }
}

