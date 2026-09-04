using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LearningPlatform.Models
{
    public class AddTopic
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int MasterCourseId { get; set; }
        
        [ForeignKey("MasterCourseId")]
        [ValidateNever]
        public MasterCourse MasterCourseData { get; set; } = null!;
        
        [Required]
        public int SubCourseId { get; set; }
        [ForeignKey("SubCourseId")]
        [ValidateNever]
        public SubCourse SubCourseData { get; set; } = null!;
        [Required]
        public string TopicName { get; set; } = null!;
        [Required]
        public string VideoUrl { get; set; } = null!;
        [Required]
        public string Status { get; set; } = null!;
        [ValidateNever]
        public string Thumbnail { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        // Created automatically by controller
        [ValidateNever]
        public string CreatedBy { get; set; } = "Admin";
    }
}