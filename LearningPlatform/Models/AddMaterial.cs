using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class AddMaterial
    {
        [Key]
        public int MaterialId { get; set; }

        public int MasterCourseId { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourseData { get; set; } = null!;

        public int SubCourseId { get; set; }

        [ForeignKey("SubCourseId")]
        public SubCourse SubCourseData { get; set; } = null!;

        public int TopicId { get; set; }

        [ForeignKey("TopicId")]
        public AddTopic TopicData { get; set; } = null!;

        public string MaterialType { get; set; } = null!;

        public string AssignmentAttachment { get; set; } = null!;

        public string MCQ1Question { get; set; } = null!;
        public string MCQ1OptionA { get; set; } = null!;
        public string MCQ1OptionB { get; set; } = null!;
        public string MCQ1OptionC { get; set; } = null!;
        public string MCQ1OptionD { get; set; } = null!;
        public string MCQ1Answer { get; set; } = null!;

        public string MCQ2Question { get; set; } = null!;
        public string MCQ2OptionA { get; set; } = null!;
        public string MCQ2OptionB { get; set; } = null!;
        public string MCQ2OptionC { get; set; } = null!;
        public string MCQ2OptionD { get; set; } = null!;
        public string MCQ2Answer { get; set; } = null!;

        public string MCQ3Question { get; set; } = null!;
        public string MCQ3OptionA { get; set; } = null!;
        public string MCQ3OptionB { get; set; } = null!;
        public string MCQ3OptionC { get; set; } = null!;
        public string MCQ3OptionD { get; set; } = null!;
        public string MCQ3Answer { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string CreatedAt { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;
    }
}