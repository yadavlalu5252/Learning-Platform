using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class CourseProgress
    {
        [Key]
        public int ProgressId { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserData { get; set; } = null!;

        public int MasterCourseId { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourseData { get; set; } = null!;

        public int TopicId { get; set; }

        [ForeignKey("TopicId")]
        public AddTopic TopicData { get; set; } = null!;

        public bool IsCompleted { get; set; }

        public bool McqCompleted { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}