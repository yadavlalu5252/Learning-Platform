using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models
{
    public class AddTopic
    {
        [Key]
        public int Id { get; set; }

        public int MasterCourseId { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourseData { get; set; } = null!;

        public int SubCourseId { get; set; }

        [ForeignKey("SubCourseId")]
        public SubCourse SubCourseData { get; set; } = null!;

        public string TopicName { get; set; } = null!;

        public string VideoUrl { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string Thumbnail { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; } = null!;
    }
}