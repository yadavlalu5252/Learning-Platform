using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace LearningPlatform.Models
{
    public class AddMaterial
    {
        [Key]
        public int MaterialId { get; set; }
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
        public int TopicId { get; set; }
        [ForeignKey("TopicId")]
        [ValidateNever]
        public AddTopic TopicData { get; set; } = null!;
        public string MaterialType { get; set; } = "Assignment + MCQ";
        public string AssignmentAttachment { get; set; } = "";
        // MCQ 1
        public string MCQ1Question { get; set; } = "";
        public string MCQ1OptionA { get; set; } = "";
        public string MCQ1OptionB { get; set; } = "";
        public string MCQ1OptionC { get; set; } = "";
        public string MCQ1OptionD { get; set; } = "";
        public string MCQ1Answer { get; set; } = "";
        // MCQ 2
        public string MCQ2Question { get; set; } = "";
        public string MCQ2OptionA { get; set; } = "";
        public string MCQ2OptionB { get; set; } = "";
        public string MCQ2OptionC { get; set; } = "";
        public string MCQ2OptionD { get; set; } = "";
        public string MCQ2Answer { get; set; } = "";
        // MCQ 3
        public string MCQ3Question { get; set; } = "";
        public string MCQ3OptionA { get; set; } = "";
        public string MCQ3OptionB { get; set; } = "";
        public string MCQ3OptionC { get; set; } = "";
        public string MCQ3OptionD { get; set; } = "";
        public string MCQ3Answer { get; set; } = "";
        public string Status { get; set; } = "Active";
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = "Admin";
    }
}